/*
 * Copyright (C) 2006 aeolusc <soarchin@gmail.com>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of
 * the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

#include <pspsdk.h>
#include <pspkernel.h>
#include <pspmodulemgr_kernel.h>
#include <pspdebug.h>
#include <pspdisplay.h>
#include <pspdisplay_kernel.h>
#include <stdio.h>
#include <string.h>
#include <pspctrl.h>
#include <psppower.h>
#include <pspsysmem_kernel.h>
#include <pspimpose_driver.h>
#include "conf.h"
#include "ctrl.h"
#include "font.h"
#include "mem.h"
#include "ui.h"
#include "text.h"
#include "lang_zh_sjis.h"
#include "encode.h"
#include "dict.h"
#include "layout.h"
#include "convert_cw.h"
#include "allocmem.h"
#include "blend.h"
#include "minifloat.h"
#include "encode.h"
#include "usb.h"
#include "common.h"
#include <jpeglib.h>
#include "rgb_color.h"
#include "smsutils.h"
#include "screenshot.h"

#define MEMCLEARLINE_END 92
#define MEMCLEARLINE_START 90

#include "ja_layout_sjis.c"

typedef struct{
	int width;
	int height;
	int x;
	int y;
}__attribute__((packed)) t_imginfo;
int prx_showpng(char* filename, int pos, t_bgctx *bgctx, t_imginfo *img);
int prx_openjpg(char * filename, t_bgctx *bgctx, t_imginfo *img);

void prx_fatread(char *tempdir, char *ext, const char ** decode_text_array, const char ** decode_dir_array);

static int strcmpi(char * s1, char * s2);

/* static unsigned int sv = 80, sa = 0, lowaddr = 0, highaddr = 0x01800000;
static int tc = 0, rc = 0, stype = 0, scount = 0; */
//表格数目,搜索结果数目,搜索类型,搜索次数
typedef struct _layout_gv{
	unsigned int sv;
	unsigned int sa;
	unsigned int lowaddr;
	unsigned int highaddr;
	unsigned int fuzzy_diff;
	int tc;
	int rc;
	int stype;
	int scount;
	int table_idx;
	int search_res_idx;
	int search_fuzzy_idx;
	unsigned int dmaaddress;
}t_layout_gv;

static t_layout_gv layout_gv __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );

#define VIEW_BASE_X 8
static int layout_ivalue(unsigned int addr, int x18, int y12)
{
	int z = 0;
	char s[80];	
	unsigned char vv = MEM_VALUE(addr);
	unsigned int key;
	font_fillrect(VIEW_BASE_X+10*6 + x18, y12 + MEMCLEARLINE_START, VIEW_BASE_X+10*6 + 11 + x18, y12 +  MEMCLEARLINE_END);
	font_output(VIEW_BASE_X+10*6 + x18, 83 + y12, "_");
	while(((key = ctrl_waitmask(PSP_CTRL_UP | PSP_CTRL_DOWN | PSP_CTRL_LEFT | PSP_CTRL_RIGHT | PSP_CTRL_CIRCLE | PSP_CTRL_CROSS)) & (PSP_CTRL_CIRCLE | PSP_CTRL_CROSS)) == 0)
	{
		switch(key)
		{
		case PSP_CTRL_UP:
			if(z == 0)
			{
				if(vv < 0xF0)
					vv += 0x10;
				else
					vv = 0xFF;
			}
			else
			{
				if(vv < 0xFF)
					vv ++;
			}
			break;
		case PSP_CTRL_DOWN:
			if(z == 0)
			{
				if(vv > 0x10)
					vv -= 0x10;
				else
					vv = 0;
			}
			else
			{
				if(vv > 0)
					vv --;
			}
			break;
		case PSP_CTRL_LEFT: case PSP_CTRL_RIGHT:
			z = 1 - z;
			break;
		}
		font_fillrect(VIEW_BASE_X+10*6 + x18, y12 + 80, VIEW_BASE_X+10*6 + 11 + x18, y12 +  MEMCLEARLINE_END);
		sprintf(s, "%02X", vv);
		font_output(VIEW_BASE_X+10*6 + x18, y12 + 80, s);
		font_output(VIEW_BASE_X+10*6 + x18 + z * 6, 83 + y12, "_");
		sceKernelDelayThread(20000);
	}
	if((!config.swap && (key & PSP_CTRL_CIRCLE) > 0) || (config.swap && (key & PSP_CTRL_CROSS) > 0))
		MEM_VALUE(addr) = vv;
/*  	font_fillrect(170 + x * 18, 80 + y * 12, 181 + x * 18, 90 + y * 12);
	sprintf(s, "%02X", MEM_VALUE(addr + x + y * 8));
	font_output(170 + x * 18, 80 + y * 12, s); */
	return 1;
}

static int write_ram(char *fn, char *p, int len)
{
				int fd = sceIoOpen(fn, PSP_O_RDONLY, 0777);
				if (fd<0) return 0;
				if(len==0) len = sceIoLseek32(fd, 0, PSP_SEEK_END);
				sceIoLseek32(fd, 0, PSP_SEEK_SET);
				sceIoRead(fd, p, len);
				sceIoClose(fd);
/* 				sceKernelDcacheWritebackInvalidateAll();
				sceKernelIcacheInvalidateAll(); */
				font_output(110, 12*13+56, LAYOUT_MEM_WRITEOK);
				ctrl_waitany();
				return 0;
}

static int write_mem(char *fn)
{
	unsigned int base;
	char *p=strstr(fn,"0x");
	if(p==0) return 0;
	base = strtoul(p,NULL,16);
	base += 0x8800000;
	return write_ram(fn, (char *)base, 0);
}

typedef struct _layout_view_g{
	int layout_view_bakx;
	int layout_view_baky;
	unsigned int history[10];
	int hiscount;
	int hisback;
	char search_str[32];
	char search_hexstr[32];
}__attribute__((packed)) t_layout_view_gv;

static t_layout_view_gv view_gv __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );
static int layout_get_table_item(const char *, p_mem_table);

static void layout_view_history(unsigned int tempadr)
{
	view_gv.hisback = view_gv.hiscount;
	view_gv.history[view_gv.hiscount++] = tempadr ;
	if(view_gv.hiscount>=10) view_gv.hiscount=0;
}

static int layout_view_search_display(char *str, char *s)
{
	font_fillrect(96, 100, 300, 230);
	font_output(104, 102, s);
	return ui_input_string(104, 116, str, 30);
}

static int hex_to_int(char ch)
{
 	if((ch >= '0') && (ch <= '9'))
	{
		return ch - '0';
	}

	ch = toupper(ch);
	if((ch >= 'A') && (ch <= 'F'))
	{
		return ch - 'A' + 10;
	}

	return 0;
}

static void keylist_set_cls(const char **s)
{
	font_fillrect(100, 30, 379, 239);
	font_output(200, 12*13+56, (char *)s[0]);
	font_output(200, 12*14+56, (char *)s[1]);
}

static void layout_view(int flag)
{
	char s[80];
	t_mem_table	t;
	int addcount=1;
	if(layout_gv.sa >= 0x01800000)
		layout_gv.sa = 0;
	unsigned int ramsa = layout_gv.sa + 0x08800000;
	int rp = 1;
	unsigned int addr, addr2;
	int x,y;
	int i,j;
	int y1,y2,y12;
	int x6,x18;
	
	addr = (((ramsa) >> 4) << 4);
	if(addr>=0x0a000000-10*16) addr = 0x0a000000-10*16;
	if(flag==0){
		x = (ramsa - addr)%16; y = 0;
	}
	else{
		x = view_gv.layout_view_bakx;
		y = view_gv.layout_view_baky;
	}
	while(1)
	{
		if(rp)
		{
			addr = (((ramsa) >> 4) << 4);
			if(addr>=0x0a000000-10*16) addr = 0x0a000000-10*16;
			
			if(rp==2)
			{
				x = (ramsa - addr)%16; y = 0;
			}
			rp = 0;
			font_fillrect(VIEW_BASE_X-4, 28, 468, 241);
			font_output(VIEW_BASE_X+10*6, 68, "+0 +1 +2 +3 +4 +5 +6 +7  +8 +9 +A +B +C +D +E +F");
			addr2 = addr;
			
			for(i = 0; i < 10; i ++)
			{
				if(addr2 >= 0x0A000000)
					break;
					
				int jtmp = 80 + i * 12;
				sprintf(s, "%08X  %02X %02X %02X %02X %02X %02X %02X %02X  %02X %02X %02X %02X %02X %02X %02X %02X", addr2 - 0x08800000,
				MEM_VALUE(addr2), MEM_VALUE(addr2 + 1), MEM_VALUE(addr2 + 2), MEM_VALUE(addr2 + 3), MEM_VALUE(addr2 + 4), MEM_VALUE(addr2 + 5), MEM_VALUE(addr2 + 6), MEM_VALUE(addr2 + 7),
				MEM_VALUE(addr2 + 8), MEM_VALUE(addr2 + 9), MEM_VALUE(addr2 + 10), MEM_VALUE(addr2 + 11), MEM_VALUE(addr2 + 12), MEM_VALUE(addr2 + 13), MEM_VALUE(addr2 + 14), MEM_VALUE(addr2 + 15)
				);
				font_output(VIEW_BASE_X, jtmp, s);
				
				for(j=0;j<16;j++){
					unsigned char ctemp = ((unsigned char *)addr2)[j];
					if(ctemp>=' ' && ctemp<0x80) s[j]=ctemp;
					else s[j]=' ';
				}
				s[j]=0;
				font_output(VIEW_BASE_X+60*6, jtmp, s);
				
				addr2 += 16;
			}
			
			font_output(VIEW_BASE_X, 206, LANG_ADDRP1);
			font_output(VIEW_BASE_X, 218, LANG_ADDRP2);
		}
			
		unsigned int tempadr = (addr + x + y * 16);
		font_fillrect(102, 54, 238, 66);
		sprintf(s, "0x%08X:0x%08X", tempadr, tempadr - 0x08800000);
		font_output(104, 56, s);
		
			font_fillrect(318, 199, 468, 236);
			sprintf(s, "%02X:%d", MEM_VALUE(tempadr), MEM_VALUE(tempadr));
			font_output(320, 200, s);			
			if((tempadr&3) == 0){
				font_output(320+48, 200, FloatHex2Str((unsigned char*)tempadr, s));
			}
			sprintf(s, "%04X:%d", (tempadr < 0x09FFFFFF) ? MEM_SHORT(tempadr) : MEM_VALUE(tempadr), (tempadr < 0x09FFFFFF) ? MEM_SHORT(tempadr) : MEM_VALUE(tempadr));
			font_output(320, 212, s);
			sprintf(s, "%08X:%u", (tempadr < 0x09FFFFFD) ? MEM_LONG(tempadr) : MEM_VALUE(tempadr), (tempadr < 0x09FFFFFD) ? MEM_LONG(tempadr) : MEM_VALUE(tempadr));
			font_output(320, 224, s);
		
		x6 = x*6;
		x18 = x*18 + (x>=8?6:0);
		y12 = y*12;
		font_output(VIEW_BASE_X+10*6 + x18, 83 + y12, "__");
		font_output(VIEW_BASE_X+60*6 + x6, 83 + y12 , "_");
		y1 = y12+MEMCLEARLINE_START;
		y2 = y12+MEMCLEARLINE_END;
		
		switch(ctrl_waitmask(PSP_ANA_UP|PSP_ANA_LEFT|PSP_ANA_RIGHT|PSP_CTRL_TRIANGLE | PSP_CTRL_SQUARE | PSP_CTRL_LTRIGGER | PSP_CTRL_RTRIGGER | PSP_CTRL_UP | PSP_CTRL_DOWN | PSP_CTRL_LEFT | PSP_CTRL_RIGHT | PSP_CTRL_CIRCLE | PSP_CTRL_CROSS | PSP_CTRL_SELECT | PSP_CTRL_START | PSP_CTRL_NOTE))
		{
		case PSP_CTRL_CROSS:
			if(!config.swap){
				goto VIEW_OUT;
			}
			else rp = layout_ivalue(tempadr, x18, y12);
			break;
		case PSP_CTRL_CIRCLE:
			if(config.swap){
				goto VIEW_OUT;
			}
			else rp = layout_ivalue(tempadr, x18, y12);
			break;
		case PSP_CTRL_SQUARE:
			addr2 = tempadr - 0x08800000;
			if(ui_input_hex(170, 56, addr2, &addr2, 0, 0x017FFFFF) >= 0)
			{
				layout_view_history(tempadr);
				ramsa = addr2 + 0x08800000;
			}
			rp = 2;
			break;
		case PSP_CTRL_LTRIGGER:
			addr -= 10 * 16;
			if(addr < 0x08800000)
				addr = 0x08800000;
			ramsa = addr;
			rp = 1;
			break;
		case PSP_CTRL_RTRIGGER:
			addr += 10 * 16;
			if(addr >= 0x0A000000-10*16)
				addr = 0x0A000000-10*16;
			ramsa = addr;
			rp = 1;
			break;
		case PSP_CTRL_UP:
			if(y == 0)
			{
				addr -= 16;
				if(addr < 0x08800000)
					addr = 0x08800000;
				ramsa = addr;
				rp = 1;
			}
			else
			{
				font_fillrect(VIEW_BASE_X+10*6 + x18, y1, VIEW_BASE_X+10*6 + 11 + x18, y2);
				font_fillrect(VIEW_BASE_X+60*6 + x6, y1, VIEW_BASE_X+60*6 + 8 + x6, y2);
				y --;
			}
			break;
		case PSP_CTRL_DOWN:
			if(y == 9)
			{
				addr += 16;
				if(addr >= 0x0A000000-10*16)
					addr = 0x0A000000-10*16;
				ramsa = addr;
				rp = 1;
			}
			else
			{
				font_fillrect(VIEW_BASE_X+10*6 + x18, y1, VIEW_BASE_X+10*6 + 11 + x18, y2);
				font_fillrect(VIEW_BASE_X+60*6 + x6, y1, VIEW_BASE_X+60*6 + 8 + x6, y2);
				y ++;
			}
			break;
		case PSP_CTRL_LEFT:
				font_fillrect(VIEW_BASE_X+10*6 + x18, y1, VIEW_BASE_X+10*6 + 11 + x18, y2);
				font_fillrect(VIEW_BASE_X+60*6 + x6, y1, VIEW_BASE_X+60*6 + 8 + x6, y2);
			x --;
			if(x < 0)
				x = 15;
			break;
		case PSP_CTRL_RIGHT:
				font_fillrect(VIEW_BASE_X+10*6 + x18, y1, VIEW_BASE_X+10*6 + 11 + x18, y2);
				font_fillrect(VIEW_BASE_X+60*6 + x6, y1, VIEW_BASE_X+60*6 + 8 + x6, y2);
			x ++;
			if(x > 15)
				x = 0;
			break;
		case PSP_CTRL_TRIANGLE:
			t.addr = tempadr;
			t.type = 0;
			t.lock = 0;
			t.value = *(u8 *)t.addr;
			sprintf(t.name,"Mem%-7d",addcount);
			mem_table_add(&t);
			font_fillrect(300, 52, 379, 66);
			sprintf(s,LAYOUT_VIEW_ADD,addcount++);
			font_output(300, 54, s);
			ctrl_waitrelease();
			break;
		case PSP_CTRL_SELECT:
			if(view_gv.history[view_gv.hisback]){
				ramsa = view_gv.history[view_gv.hisback];
				rp = 2;
				j = view_gv.hisback-1;
				if(j<0) j = view_gv.hiscount? view_gv.hiscount-1:9;
				if(view_gv.history[j]) view_gv.hisback = j;
			}
			ctrl_waitrelease();
			break;
		case PSP_CTRL_START:
			if((tempadr&3)==0){
				addr2 = *(unsigned int*)tempadr;
				if(addr2>=0x8800000 && addr2<0x0a000000)
				{
					layout_view_history(tempadr);
					ramsa = addr2;
					rp = 2;
				}
			}
			ctrl_waitrelease();
			break;
		case PSP_ANA_LEFT:
			if(layout_view_search_display(view_gv.search_str, view_search_string) >= 0)
			{
				unsigned int tmp;
				int l = strlen(view_gv.search_str);
				char *s1, *s2;
					
				for(tmp=tempadr+1;tmp<0x0A000000-10*16;tmp++)
				{
					s1 = (char *)tmp;
					s2 = view_gv.search_str;
					i=0;
					while((*s1 & ~0x20) == (*s2 & ~0x20) && i<l)
					{
						s1++;
						s2++;
						i++;
					}
					if(i==l)
					{
						layout_view_history(tempadr);
						ramsa = tmp;
						break;
					}
				}
			}
			rp = 2;
			break;
		case PSP_ANA_RIGHT:
			if(layout_view_search_display(view_gv.search_hexstr, view_search_hexstr) >= 0)
			{
				unsigned int tmp;
				int hexlen = strlen(view_gv.search_hexstr);
				hexlen = hexlen&0xFE;
				hexlen /= 2;
				char hexdat[16]; 
				
				for(i = 0; i < hexlen; i++)
				{
					hexdat[i] = (hex_to_int(view_gv.search_hexstr[i*2]) << 4) | hex_to_int(view_gv.search_hexstr[(i*2)+1]);
				}
				
				for(tmp=tempadr+1;tmp<0x0A000000-10*16;tmp++)
				{
					if(memcmp((char *)tmp, hexdat, hexlen)==0)
					{
						layout_view_history(tempadr);
						ramsa = tmp;
						break;
					}
				}
			}
			rp = 2;
			break;
		case PSP_ANA_UP:
			addr2 = 0;
			if(ui_input_hex(170, 56, addr2, &addr2, 0, 0x0A000000-tempadr-1) >= 0)
			{
				layout_view_history(tempadr);
				ramsa = addr2 + tempadr;
			}
			rp = 2;
			break;
		case PSP_CTRL_NOTE:
			layout_gv.dmaaddress=tempadr;
			break;
		}
	}

VIEW_OUT:
	font_fillrect(VIEW_BASE_X-4, 30, 468, 239);
	view_gv.layout_view_bakx = x;
	view_gv.layout_view_baky = y;
	layout_gv.sa = ramsa - 0x08800000;
}

static void layout_change()
{
	ui_cls();
	int idx = ui_menu(110, 56, menu_change, 9, 9, layout_gv.stype, NULL);
	if(idx >= 0 && idx < 9)
	{
		if((layout_gv.stype >= 4) || (idx >= 4 && layout_gv.stype < 4))	//重新搜索的情形
		{
			layout_gv.scount = 0;
			mem_search_finish();
		}
		layout_gv.stype = idx;
	}
}

#define min(x,y) (((x) < (y)) ? (x) : (y))
static unsigned int maxtable[]__attribute__(   (  aligned( 4 ), section( ".data" )  )   )={
0xffffffff,0xff,0xffff,0xffffffff
};

static int layout_get_table_item(const char * prompt, p_mem_table table)
{
	unsigned int compareval;
	while(1)
	{
		ui_cls();
		font_output(110, 56, prompt);
		memset(table->name, 0, 12);
		strcpy(table->name, "New");
		font_output(110, 68, LANG_COMMENT);
		if(ui_input_string(164, 68, table->name, 10) < 1)
			return -1;
		if(table->addr != 0xFFFFFFFF)
		{
			font_output(110, 80, LANG_ADDRESS);
			int r = ui_input_hex(164, 80, table->addr, &table->addr, 0, 0xFFFFFFFF);
			if(r < 0)
				continue;
		}
		layout_gv.sa = table->addr;
		font_output(110, 92, LANG_DATATYPE);
		int idx = ui_menu(122, 104, menu_change, 4, 4, (layout_gv.stype > 3) ? 0 : layout_gv.stype, NULL);
		if(idx < 0)
			continue;
		font_output(110, 152, LANG_VALUE);
		font_fillrect(152, 150, 379, 195);
		int r = -1;
		switch(idx)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			compareval = maxtable[idx];
			r = ui_input(152, 152, min(layout_gv.sv, compareval), &layout_gv.sv, 0, compareval);
			break;
		default:
			r = -1;
			break;
		}
		if(r < 0)
			continue;
		table->type = idx;
		table->value = layout_gv.sv;
		font_output(110, 164, LANG_LOCKQ);
		r = ui_menu(122, 176, menu_yesno, 2, 2, 0, NULL);
		if(r < 0)
			continue;
		table->lock = (r == 0) ? 1 : 0;
		return 0;
	}
}

static int layout_result_cb(unsigned int key, int *id, int *topid)
{
	int idx = *id;
	if(key == PSP_CTRL_TRIANGLE)
	{
		unsigned int * res;
		layout_gv.rc = mem_search_get_result(&res);
		if(layout_gv.rc <= idx)
			return 3;
		layout_gv.sa = res[idx] - 0x08800000;
		layout_view(0);
		return 3;
	}
	else if(key == PSP_CTRL_SQUARE)
	{
		unsigned int * res;
		layout_gv.rc = mem_search_get_result(&res);
		if(layout_gv.rc <= 0)
			return 3;
		t_mem_table t;
		t.addr = 0xFFFFFFFF;
		if(layout_get_table_item(LANG_ADDALL, &t) < 0)
			return 3;
		int i;
		for(i = 0; i < layout_gv.rc; i ++)
		{
			t.addr = res[i];
			mem_table_add(&t);
		}
		return 1;
	}
	else if((config.swap && key == PSP_CTRL_CROSS) || (!config.swap && key == PSP_CTRL_CIRCLE))
	{
		unsigned int * res;
		layout_gv.rc = mem_search_get_result(&res);
		if(layout_gv.rc < 0)
			return 3;
		t_mem_table t;
		t.addr = res[idx] - 0x08800000;
		if(layout_get_table_item(LANG_ADD, &t) < 0)
			return 3;
		t.addr += 0x08800000;
		mem_table_add(&t);
		return 1;
	}
	return -1;
}

static const char *layout_table_sum[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	LANG_TABLESUM,
	layout_table_str1,
	layout_table_str3,
};
static const char *layout_table_detail[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	LANG_TABLETITLE,
	layout_table_str1,
	layout_table_str2,
};

static const char ** g_text_array;

static void layout_table_display(const char **s)
{
	font_fillrect(100, 30, 379, 239);
		font_output(122, 56, s[0]);

		font_output(106, 218, s[2]);

		font_output(106, 206, s[1]);
		if(!config.swap)
			font_output(106+18*12, 206, LANG_TABLEP3);
		else
			font_output(106+18*12, 206, LANG_TABLEP4);
}

static void layout_table_printname(p_mem_table table, int idx)
{
	char sname[12];
	mips_memcpy(sname,table[idx].name, 10);
	sname[10]=0;
	sprintf((char *)&g_text_array[idx][0], "%-10s 0x%08X 0x%08X %-4s%4s", sname, (table[idx].addr - 0x08800000)^0xD6F73BEE, table[idx].value, menu_change[table[idx].type], menu_yesno[1 - table[idx].lock]);
	layout_table_display(layout_table_detail);
}

static int layout_table_cb(unsigned int key, int *id, int *topid)
{
	int idx = *id;
	p_mem_table table;
	layout_gv.tc = mem_get_table(&table);
	unsigned int compareval;
	if((config.swap && key == PSP_CTRL_CROSS) || (!config.swap && key == PSP_CTRL_CIRCLE))
	{
		while(1)
		{
			font_fillrect(100, 168, 379, 231);
			font_output(110, 170, LANG_ADDRESS);
			layout_gv.sa = table[idx].addr - 0x08800000;
			int r = ui_input_hex(164, 170, layout_gv.sa, &layout_gv.sa, 0, 0xFFFFFFFF);
			if(r < 0)
				break;
			font_fillrect(100, 168, 379, 231);
			font_output(110, 170, LANG_DATATYPE);
			int idx1 = ui_menu(122, 182, menu_change, 4, 4, table[idx].type & 3, NULL);
			if(idx1 < 0)
				continue;
			font_fillrect(100, 168, 379, 231);
			font_output(110, 170, LANG_VALUE);
			layout_gv.sv = table[idx].value;
			switch(idx1)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				compareval = maxtable[idx1];
				r = ui_input(152, 170, min(layout_gv.sv, compareval), &layout_gv.sv, 0, compareval);
				break;
			default:
				r = -1;
				break;
			}
			if(r < 0)
				continue;
			t_mem_table t;
			mips_memcpy(t.name, table[idx].name, 31);
			t.addr = layout_gv.sa + 0x08800000;
			t.value = layout_gv.sv;
			t.type = idx1;
			t.lock = table[idx].lock;
			mem_table_set(idx, &t);
			break;
		}
		layout_table_printname(table, idx);
		return 3;
	}
 	else if(key == PSP_CTRL_RIGHT)
	{
		font_fillrect(100, 112, 379, 231);
		font_output(110, 116, LANG_COMMENT);
		ui_input_string(164, 116, table[idx].name, 10);
		layout_table_printname(table, idx);
		return 3;
	}
	else if(key == PSP_CTRL_TRIANGLE)
	{
		int fakeaddres=table[idx].addr- 0x08800000;
		int codeheader = table[idx].addr>>28;
		if((codeheader == 3) || (codeheader==0xE))
		{
		fakeaddres=table[idx].value;
		}
		layout_gv.sa = (fakeaddres & 0x1FFFFFF);
		layout_view(0);
		return 2;
	}
	else if(key == PSP_CTRL_SQUARE)
	{
			int i,start,end;
			int lockflag = 1 - table[idx].lock;
			mem_table_locksuit(idx,&start,&end,lockflag);
			for(i=start;i<end;i++)
				sprintf((char *)((&g_text_array[i][0])+37), "%4s", menu_yesno[1 - table[i].lock]);
			return 3;
	}
	else if(key == PSP_CTRL_START)
	{
		t_mem_table t;
		if(layout_gv.sa != 0)
			t.addr = layout_gv.sa;
		else
		{
/* 			p_mem_table table;
			layout_gv.tc = mem_get_table(&table); */
			//if(layout_gv.tc > idx)
				t.addr = table[idx].addr - 0x08800000;
			//else
				//t.addr = 0;
		}
		if(layout_get_table_item(LANG_NEWADDR, &t) < 0)
			return 2;
		t.addr += 0x08800000;
		//mem_table_add(&t);
		mem_table_insert(&t, idx);
		return 2;
	}
	else if(key == PSP_CTRL_SELECT)
	{
		mem_table_delete(idx);
		return 2;
	}
 	else{
		if(key == PSP_ANA_UP){
			*id = mem_table_walkback(*id);
			*topid = *id>=10? *id-9:0;
			return 3;
		}
		else if(key == PSP_ANA_DOWN){
			*id = mem_table_walkforward(*id);
			*topid = *id>=10? *id-9:0;
			return 3;
		}
	}
	return -1;
}

#define TABLE_ENTRY_MAX	1000
static int *nc_buf;
static int layout_tbl_cb(unsigned int key, int *id, int *topid)
{
	int idx = *id;
	p_mem_table table;
	layout_gv.tc = mem_get_table(&table);

	if((config.swap && key == PSP_CTRL_CROSS) || (!config.swap && key == PSP_CTRL_CIRCLE))
	{
		*id = (idx+1)<<16;
		return 2;
	}
	else if(key == PSP_CTRL_SELECT)
	{
		mem_table_deletesuit(nc_buf[idx]);
		return 2;
	}
	else if(key == PSP_CTRL_LEFT)
	{
		mem_table_lockall();
		return 2;
	}
	else{
		int i = nc_buf[idx];
		if(key == PSP_CTRL_SQUARE)
		{
			int start,end;
			int lockflag = 1 - table[i].lock;
			mem_table_locksuit(i,&start,&end,lockflag);
			sprintf((char *)((&g_text_array[idx][0])), "%-6s%s", menu_yesno[1 - table[i].lock], table[i].name);
			return 3;
		}
		else if(key == PSP_CTRL_RIGHT)
		{
			font_fillrect(100, 112, 379, 231);
			font_output(110, 116, LANG_COMMENT);
			ui_input_string(164, 116, table[i].name, 31);
			return 2;
		}
		else if(key == PSP_CTRL_TRIANGLE)
		{
			if(ui_menu(106, 206, menu_yesno, 2, 2, 1, NULL) == 0)
				mem_table_enable(i);
			return 2;
		}
	}
	return -1;
}

static void layout_table()
{
	const char ** text_array;
	const char * text_buf;
	int i, idx, nc, nidx=0, start, end;
	char s[12];

	text_array = malloc(TABLE_ENTRY_MAX*60);
	nc_buf = (int *)((unsigned int)text_array + TABLE_ENTRY_MAX*4);
	text_buf = (char *)((unsigned int)text_array + TABLE_ENTRY_MAX*8);
	for(i = 0; i < TABLE_ENTRY_MAX; i ++)
		text_array[i] = &text_buf[50 * i];
	g_text_array = text_array;

	idx = layout_gv.table_idx;
table_summary:
	while(1)
	{
		layout_table_display(layout_table_sum);
		p_mem_table table;
		layout_gv.tc = mem_get_table(&table);
		nc = 0;
		mem_table_suit(idx, &start, &end);
		for(i = 0; i < layout_gv.tc; i ++){
			if(i==0 || table[i].name[0]!='+'){
				nc_buf[nc] = i;
				if(start==i) nidx = nc;
				sprintf((char *)&text_array[nc++][0], "%-6s%s",menu_yesno[1 - table[i].lock], table[i].name);
			}
		}
		nidx = ui_menu(110, 68, text_array, nc, 10, nidx, layout_tbl_cb);
		if(nidx<0){
			nidx = -(nidx+1);
			layout_gv.table_idx = nc_buf[nidx];
			sfree(text_array);
			return;
		}
		if(nidx > 0xffff){
			idx = nc_buf[(nidx>>16) - 1];
			break;
		}
		else
			idx = nc_buf[nidx];
	}

	while(1)
	{
		p_mem_table table;
		layout_gv.tc = mem_get_table(&table);
		for(i = 0; i < layout_gv.tc; i ++){
			mips_memcpy(s, table[i].name, 10);
			s[10] = 0;
			sprintf((char *)&text_array[i][0], "%-11s0x%08X 0x%08X %-4s%4s", s, (table[i].addr - 0x08800000)^0xD6F73BEE, table[i].value, menu_change[table[i].type], menu_yesno[1 - table[i].lock]);
		}
		layout_table_display(layout_table_detail);
		idx = ui_menu(110, 68, text_array, layout_gv.tc, 10, idx, layout_table_cb);
		if(idx < 0){
			idx = -(idx+1);
			goto table_summary;
		}
	}
}

static void layout_search_display(x,y)
{
	font_output(x, y, LANG_RESTITLE);
					if(config.swap)
						font_output(110, 194, LANG_RESP1);
					else
						font_output(110, 194, LANG_RESP2);
					font_output(110, 206, LANG_RESP3);
					font_output(110, 218, LANG_RESP4);
}

static int layout_fuzzyval_input(unsigned int *p, int r)
{
	unsigned int max;
							switch(layout_gv.stype)
							{
							case 5:
								max = 0xff;
								break;
							case 6:
								max = 0xffff;
								break;
							default:
								max = 0xffffffff;
								break;
							}
	int ret = ui_input(180+12*4, 56+12, *p, p, 0, max);
	
	if(ret==0)	return r;
	else return ret;
}

static int layout_search_fuzzy_cb(unsigned int key, int *id, int *topid)
{

	if(*id<=1 && key==PSP_CTRL_SQUARE)
	{
		ui_input(180+12*4, 56+12, layout_gv.fuzzy_diff, &layout_gv.fuzzy_diff, 0, 0xFFFFFFFF);
		*id = 0x10000;
		return 2;
	}
	return -1;
}

static const char * low_high_addr_string[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
LANG_LOWADDR,
LANG_HIGHADDR,
};

static void layout_search_res(const char **buf, const char ***array)
{
	int i;
	unsigned int * res;
	layout_gv.rc = mem_search_get_result(&res);
	int rc = layout_gv.rc;
	
	*array = malloc(rc*54);
	*buf = (char *)((unsigned int)(*array) + rc*4);	
	
	const char *text_buf = *buf;
	const char **text_array = *array;
	
	for(i = 0; i < rc; i ++)
		text_array[i] = &text_buf[50 * i];
	for(i = 0; i < rc; i ++){
		char floatbuf[30];
		sprintf( (char *)&text_array[i][0], "%08X %02X %04X %08X %s", res[i] - 0x08800000, MEM_VALUE(res[i]), ((res[i] < 0x09FFFFFF) ? MEM_SHORT(res[i]) : MEM_VALUE(res[i])), ((res[i] < 0x09FFFFFD) ? MEM_LONG(res[i]) : MEM_VALUE(res[i])), ((res[i]&3)==0) ? FloatHex2Str((unsigned char*)res[i], floatbuf) : " " );
	}
	layout_search_display(116,80);
}

static int layout_search_input(const char **s, unsigned int *low, unsigned int *high, unsigned int lmax, unsigned int hmax, int (*inputfun)(int, int, unsigned int, unsigned int, unsigned int, unsigned int))
{
				while(1)
				{
					unsigned int la, ha;
					ui_cls();
					
					font_output(110, 56, (char *)s[0]);
					int ret = inputfun(188, 56,*low, &la, 0, lmax);
					if(ret<0) return ret;
					
					font_output(110, 68, (char *)s[1]);
					ret = inputfun(188, 68, *high, &ha, la, hmax);
					if(ret<0) continue;
					
					*low = la;
					*high = ha;
					return 0;
				}
}

static void layout_search()
{
static int (*search_value_fun[])(unsigned int, unsigned int, unsigned int) = {mem_search_value,mem_search_byte,mem_search_word,mem_search_dword};
const char fuzzyflag[] __attribute__(   (  aligned( 1 ))  ) = {4, 2, 6, 5, 3, 1, -4, -2, -6, -5, -3, -1};
const char fuzzyapx[]  __attribute__(   (  aligned( 1 ))  ) = {0, 1, 2, 4, 8};
	const char ** text_array;
	const char * text_buf;
	char s[80];
	int idx = 0;
	//int i;
	unsigned int compareval;
	unsigned int exactmax;
	int menu_item_count;
	const char ** menu_search_p;
	int menu_search_idx;
	const char * search_str[] = {s,LANG_SEARCH_DIFFHELP};

	while(1)
	{
		ui_cls();
		sprintf(s, LANG_SEARCHTYPE"%s", menu_change[layout_gv.stype]);
		font_output(110, 142, s);
		sprintf(s, LANG_SEARCHRANGE"%08X - %08X",layout_gv.lowaddr, layout_gv.highaddr);
		font_output(110, 154, s);
		menu_item_count = (layout_gv.scount > 0) ? (layout_gv.rc > 500 ? 4:5) : 3;
		menu_search_p = (layout_gv.scount > 0) ? menu_search2: &menu_search2[1];
		menu_search_idx = (layout_gv.scount > 0) ? 0:1;
		if((idx = ui_menu(110, 56, menu_search_p, menu_item_count, menu_item_count, idx, NULL)) < 0)
			break;
		idx += menu_search_idx;
		switch(idx)
		{
		case 1:
			layout_gv.scount = 0;
			//mem_search_init();
		case 0:
			if(layout_gv.scount == 0)
			{
                mem_search_init();
			}
			ui_cls();
			int r = 0x10000;
			switch(layout_gv.stype)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				exactmax = maxtable[layout_gv.stype];
				font_output(110, 56, LANG_SEARCH);
				r = ui_input(158, 56, min(layout_gv.sv, exactmax), &layout_gv.sv, 0, exactmax);
				//if(r>=0)
				{
				}
				break;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				if(layout_gv.scount == 0){
					compareval=0;
					r = ui_menu(110, 56, menu_fuzzy_init, 2, 2, 0, NULL);
					if(r>0){
						r = layout_search_input(low_high_value_string, &compareval, &layout_gv.fuzzy_diff, 0xFFFFFFFF, 0xFFFFFFFF, ui_input);
/* 						if(r==0){
							r = 12;
						} */
					}
				}
				else{
					layout_gv.fuzzy_diff = 0;
					while(r>=0x10000)
					{
						sprintf(s,LAYOUT_SEARCH_DIFF, layout_gv.fuzzy_diff);
						keylist_set_cls(search_str);
						r = ui_menu(110, 56, menu_fuzzy, (layout_gv.scount==1)?3:12, 12, layout_gv.search_fuzzy_idx, layout_search_fuzzy_cb);
						if(r>=0 && r<0x10000) layout_gv.search_fuzzy_idx = r;
					}
					if(r>=6){
						r = layout_fuzzyval_input(&compareval,r);
					}
				}
				break;
			default:
				r = -1;
				break;
			}
			if(r >= 0)
			{
				switch(layout_gv.stype)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					layout_gv.rc =  search_value_fun[layout_gv.stype](layout_gv.sv,layout_gv.lowaddr, layout_gv.highaddr);
					break;
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
					layout_gv.rc = mem_search_fuzzy(fuzzyflag[r],layout_gv.lowaddr, layout_gv.highaddr, fuzzyapx[layout_gv.stype-4], compareval, layout_gv.fuzzy_diff);
					break;
				default:
					layout_gv.rc = 0;
					break;
				}
				if(layout_gv.rc > 0)
					sprintf(s, LANG_FOUND, layout_gv.rc);
				else
					strcpy(s, LANG_NOTFOUND);
				ui_cls();
				font_output(110, 68, s);
				if(layout_gv.rc == 0 || layout_gv.rc > 500)
				{
					{
						font_output(110, 80, LANG_PRESS1);
						ctrl_waitmask(PSP_CTRL_CIRCLE|PSP_CTRL_CROSS);
					}
				}
				else
				{
					layout_search_res(&text_buf, &text_array);
					ui_menu(110, 92, text_array, layout_gv.rc, 8, 0, layout_result_cb);
					free(text_array);
				}
				if(layout_gv.rc > 0)
				{
					layout_gv.scount ++;
					idx = -1;
				}
				else
				{
					mem_search_finish();
					layout_gv.scount = 0;
				}
			}
			break;
		case 2:
			layout_change();
			break;
		case 3:
			layout_search_input(low_high_addr_string, &layout_gv.lowaddr, &layout_gv.highaddr, 0x017FFDFC, 0x01800000, ui_input_hex);
			break;
		case 4:
			{
				ui_cls();
				layout_search_res(&text_buf, &text_array);
				sprintf(s, LANG_RESTOTAL, layout_gv.rc);
				font_output(110, 56, s);
				int sub_idx = layout_gv.search_res_idx;
				sub_idx = ui_menu(110, 80, text_array, layout_gv.rc, 9, sub_idx, layout_result_cb);
				if(sub_idx < 0)
					layout_gv.search_res_idx = -(sub_idx+1);
				free(text_array);
			}
			break;
		default:
			idx = -1;
			break;
		}
		if(idx < 0)
			break;
	}
}

static int layout_slt_cb(unsigned int key, int *id, int *topid)
{
	int idx = *id;
	if(key == PSP_CTRL_SELECT)
	{
		char fn[128];
		sprintf(fn, "%s/%02d.tab", TAB_DIR, idx);
		sceIoRemove(fn);
		strcpy((char *)&g_text_array[idx][0], LANG_EMPTY);
		return 3;
	}
	return -1;
}

static int layout_tab_common()
{
	const char ** text_array;
	const char * text_buf;
	ui_cls();
	text_array = malloc(MEMTABLE_INDEXMAX*54);
	text_buf = (char *)((unsigned int)text_array + MEMTABLE_INDEXMAX*4);
	memset((char *)text_array, 0, MEMTABLE_INDEXMAX*54);
	
	int i;
	char gname[40];
	char fn[128];	
	for(i = 0; i < MEMTABLE_INDEXMAX; i ++)
		text_array[i] = &text_buf[50 * i];
	for(i = 0; i < MEMTABLE_INDEXMAX; i ++)
	{
		sprintf(fn, "%s/%02d.tab", TAB_DIR, i);
		int fd = sceIoOpen(fn, PSP_O_RDONLY, 0777);
		if(fd>=0)
		{
			sceIoRead(fd, gname, 36);
			sceIoClose(fd);
			gname[36]=0;
		}
		else
			gname[0]=0;
		sprintf((char *)&text_array[i][0], "%02d:%s", i, gname);
	}
	font_output(110, 218, LANG_SLP1);
	g_text_array = text_array;
	int idx = ui_menu(110, 56, text_array, MEMTABLE_INDEXMAX, 13, 0, layout_slt_cb);
	sfree(text_array);
	return idx;
}

static void layout_savetab()
{
 	int idx = layout_tab_common();
	if(idx<0) return;
	mem_table_save(idx);
}

static void layout_save()
{
	int idx;
	while(1)
	{
		idx = ui_menu(110+12*6, 56+12*2, layout_menu_save, 2, 2, 0, NULL);
		switch(idx)
		{
		case 0:
			mem_table_savecw();
			return;
		case 1:
			layout_savetab();
			return;
		default:
			return;
		}
	}
}

static void layout_loadtab()
{
	int idx = layout_tab_common();
	if(idx<0) return;
	p_mem_table table;
	layout_gv.tc = mem_get_table(&table);
	if(layout_gv.tc > 0)
	{
		font_fillrect(100, 192, 379, 231);
		font_output(110, 194, LANG_CLEARQ);
		if(ui_menu(122, 206, menu_yesno, 2, 2, 1, NULL) == 0)
			mem_table_load(idx, 1);
		else
			mem_table_load(idx, 0);
	}
	else
		mem_table_load(idx, 1);
}

static void layout_buscpu()
{
static int freqcpu[5] = {66, 133, 222, 266, 333};

 	ui_cls();
	int idx = ui_menu(110, 56, menu_buscpu, 5, 5, 5, NULL);
	if(idx < 0)
		return;		
	int cpu = freqcpu[idx];
#ifndef GAME3XX
	scePowerSetClockFrequency(cpu, cpu, cpu/2);
#else
	sctrlHENSetSpeed(cpu, cpu/2);
#endif
}

void start_ssthread();
void stop_ssthread();

typedef struct{
	char orgtxtname[80];
	int	orgtxtcr;
	int autotxtread;
	int imgx;
	int imgy;
	
	char g_dir[60];
	char g_ext[4];
	int g_read_fileno;
	int g_read_filemax;
	int g_read_fat;
}t_readfileinfo;

t_readfileinfo fileinfo __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );


static char snap_format[][4] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = {
"bmp","jpg","png",
};

static void layout_conf()
{
	int idx = 0;
	char s[80];
	while(1)
	{
		ui_cls();
		conf_get_keyname(config.skey, s);
#ifdef ENGLISH_UI		
		sprintf((char *)menu_conf[0]+11, "%s", s);
		sprintf((char *)menu_conf[1]+12, "%s", config.swap ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[2]+15, "%s", config.autoload ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[3]+15, "%s", config.autoloadcmf ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[4]+15, "%s", config.autoloadset ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[5]+14, "%s", menu_lockspdstr[config.lockspd]);
		sprintf((char *)menu_conf[6]+18, "%s", config.enabless ? menu_yesno[0] : menu_yesno[1]);
		conf_get_keyname(config.ssskey, s);
		sprintf((char *)menu_conf[7]+15, "%s", s);
		sprintf((char *)menu_conf[8]+25, "%u", config.txtrowbytes);

		sprintf((char *)menu_conf[9]+9, "%02X%02X%02X%02X", (config.bg_color>>24), (config.bg_color>>16)&0xff, (config.bg_color>>8)&0xff, config.bg_color&0xff);
		sprintf((char *)menu_conf[10]+8, "%02X%02X%02X", config.font_color>>16, (config.font_color>>8)&0xff, config.font_color&0xff);
		sprintf((char *)menu_conf[11]+18, "%s", snap_format[config.ssformat+1]);
		sprintf((char *)menu_conf[12]+19, "%d", config.jpg_quality);
		conf_get_keyname(config.suspend_skey, s);
		sprintf((char *)menu_conf[13]+12, "%s", s);
		conf_get_keyname(config.standby_skey, s);
		sprintf((char *)menu_conf[14]+13, "%s", s);
		conf_get_keyname(config.savekey, s);
		sprintf((char *)menu_conf[15]+12, "%s", s);
		conf_get_keyname(config.loadkey, s);
		sprintf((char *)menu_conf[16]+12, "%s", s);
		sprintf((char *)menu_conf[17]+25, "%s", menu_yesno[config.sl_autopoweroff]);
		sprintf((char *)menu_conf[18]+22, "%s", config.blendmore?menu_yesno[0] : menu_yesno[1]);
#elif JAPANESE_UI
		sprintf((char *)menu_conf[0]+8, "%s", s);
		sprintf((char *)menu_conf[1]+12, "%s", config.swap ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[2]+14, "%s", config.autoload ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[3]+14, "%s", config.autoloadcmf ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[4]+14, "%s", config.autoloadset ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[5]+14, "%s", menu_lockspdstr[config.lockspd]);
		sprintf((char *)menu_conf[6]+14, "%s", config.enabless ? menu_yesno[0] : menu_yesno[1]);
		conf_get_keyname(config.ssskey, s);
		sprintf((char *)menu_conf[7]+12, "%s", s);
		sprintf((char *)menu_conf[8]+18, "%u", config.txtrowbytes);

		sprintf((char *)menu_conf[9]+9, "%02X%02X%02X%02X", (config.bg_color>>24), (config.bg_color>>16)&0xff, (config.bg_color>>8)&0xff, config.bg_color&0xff);
		sprintf((char *)menu_conf[10]+8, "%02X%02X%02X", config.font_color>>16, (config.font_color>>8)&0xff, config.font_color&0xff);
		sprintf((char *)menu_conf[11]+10, "%s", snap_format[config.ssformat+1]);
		sprintf((char *)menu_conf[12]+12, "%d", config.jpg_quality);
		conf_get_keyname(config.suspend_skey, s);
		sprintf((char *)menu_conf[13]+11, "%s", s);
		conf_get_keyname(config.standby_skey, s);
		sprintf((char *)menu_conf[14]+11, "%s", s);
		conf_get_keyname(config.savekey, s);
		sprintf((char *)menu_conf[15]+13, "%s", s);
		conf_get_keyname(config.loadkey, s);
		sprintf((char *)menu_conf[16]+13, "%s", s);
		sprintf((char *)menu_conf[17]+17, "%s", menu_yesno[config.sl_autopoweroff]);
		sprintf((char *)menu_conf[18]+11, "%s", config.blendmore?menu_yesno[0] : menu_yesno[1]);
#else		
		sprintf((char *)menu_conf[0]+8, "%s", s);
		sprintf((char *)menu_conf[1]+12, "%s", config.swap ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[2]+14, "%s", config.autoload ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[3]+14, "%s", config.autoloadcmf ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[4]+14, "%s", config.autoloadset ? menu_yesno[0] : menu_yesno[1]);
		sprintf((char *)menu_conf[5]+14, "%s", menu_lockspdstr[config.lockspd]);
		sprintf((char *)menu_conf[6]+14, "%s", config.enabless ? menu_yesno[0] : menu_yesno[1]);
		conf_get_keyname(config.ssskey, s);
		sprintf((char *)menu_conf[7]+12, "%s", s);
		sprintf((char *)menu_conf[8]+18, "%u", config.txtrowbytes);

		sprintf((char *)menu_conf[9]+9, "%02X%02X%02X%02X", (config.bg_color>>24), (config.bg_color>>16)&0xff, (config.bg_color>>8)&0xff, config.bg_color&0xff);
		sprintf((char *)menu_conf[10]+8, "%02X%02X%02X", config.font_color>>16, (config.font_color>>8)&0xff, config.font_color&0xff);
		sprintf((char *)menu_conf[11]+10, "%s", snap_format[config.ssformat+1]);
		sprintf((char *)menu_conf[12]+12, "%d", config.jpg_quality);
		conf_get_keyname(config.suspend_skey, s);
		sprintf((char *)menu_conf[13]+11, "%s", s);
		conf_get_keyname(config.standby_skey, s);
		sprintf((char *)menu_conf[14]+11, "%s", s);
		conf_get_keyname(config.savekey, s);
		sprintf((char *)menu_conf[15]+11, "%s", s);
		conf_get_keyname(config.loadkey, s);
		sprintf((char *)menu_conf[16]+11, "%s", s);
		sprintf((char *)menu_conf[17]+17, "%s", menu_yesno[config.sl_autopoweroff]);
		sprintf((char *)menu_conf[18]+11, "%s", config.blendmore?menu_yesno[0] : menu_yesno[1]);
#endif
		idx = ui_menu(110, 56, menu_conf, 19, 12, idx, NULL);
		switch(idx)
		{
		case 0:
			config.skey = ctrl_input();
			break;
		case 1:
			config.swap = !config.swap;
			break;
		case 2:
			config.autoload = !config.autoload;
			break;
		case 3:
			config.autoloadcmf = !config.autoloadcmf;
			break;
		case 4:
			config.autoloadset = !config.autoloadset;
			break;
		case 5:
			config.lockspd ++;
			if(config.lockspd == 5)
				config.lockspd = 0;
			mem_set_lockspd((config.lockspd * config.lockspd) * 2);
			break;
		case 6:
			config.enabless = !config.enabless;
			if(config.enabless)
				start_ssthread();
			else
				stop_ssthread();
			break;
		case 7:
			config.ssskey = ctrl_input();
			break;
		case 8:
			ui_input_dec(110+12*10, 12*13+56, config.txtrowbytes, &config.txtrowbytes, 40, 72);
			fileinfo.orgtxtcr=0;
			break;
		case 9:
			ui_input_hex(110+12*10, 12*13+56, config.bg_color, &config.bg_color, 0, 0xffffffff);
			break;
		case 10:
			ui_input_hex(110+12*10, 12*13+56,  config.font_color, &config.font_color, 0, 0xffffff);
			color_init();
			break;
		case 11:
			config.ssformat++;
			if(config.ssformat>=2) config.ssformat = -1;
			break;
		case 12:
			ui_input_dec(110+12*10, 12*13+56, config.jpg_quality, &config.jpg_quality, 50, 99);
			break;
		case 13:
			config.suspend_skey = ctrl_input();
			if(config.suspend_skey == PSP_CTRL_SELECT)
				config.suspend_skey = PSP_CTRL_RIGHT|PSP_CTRL_LEFT |PSP_ANA_RIGHT|PSP_CTRL_LTRIGGER;
			break;
		case 14:
			config.standby_skey = ctrl_input();
			if(config.standby_skey == PSP_CTRL_SELECT)
				config.standby_skey = PSP_CTRL_RIGHT|PSP_CTRL_LEFT |PSP_ANA_RIGHT|PSP_CTRL_LTRIGGER;
			break;
		case 15:
			config.savekey = ctrl_input();
			if(config.savekey == PSP_CTRL_SELECT)
				config.savekey = PSP_CTRL_RIGHT|PSP_CTRL_LEFT |PSP_ANA_RIGHT|PSP_CTRL_LTRIGGER;
			break;
		case 16:
			config.loadkey = ctrl_input();
			if(config.loadkey == PSP_CTRL_SELECT)
				config.loadkey = PSP_CTRL_RIGHT|PSP_CTRL_LEFT |PSP_ANA_RIGHT|PSP_CTRL_LTRIGGER;
			break;
		case 17:
			config.sl_autopoweroff = 1 - config.sl_autopoweroff;
			break;
		case 18:
			config.blendmore = !config.blendmore;
			break;
		}
		if(idx < 0)
			break;
	}
#ifndef DEBUGUI
	conf_save();
#endif
}


static int strcmpi(char * s1, char * s2)
{
	while(*s1 != 0 && *s2 != 0 && (*s1 & ~0x20) == (*s2 & ~0x20))
	{
		s1 ++;
		s2 ++;
	}
	return (*s1 & ~0x20) - (*s2 & ~0x20);
}

static void layout_filepos(char *fname, int *cr, int rw, int len)		//rw==0 read;rw==1 write
{
	char rowstr[80];
	sprintf(rowstr, "%s.pos", fname);
	int fd;
	int type;

	if(rw==0)
		type = PSP_O_RDONLY;
	else
		type = PSP_O_WRONLY | PSP_O_CREAT | PSP_O_TRUNC;

	fd = sceIoOpen(rowstr, type, 0777);
	if(fd >= 0)
	{
		if(rw==0)
			sceIoRead(fd, cr, len);
		else
			sceIoWrite(fd, cr, len);
		sceIoClose(fd);
	}
}

#define TEXT_BASE_Y	28
#define TEXT_Y		242

static int layout_read_text(char * fname)
{
#define TXT_LINES	12
	int readrow = config.txtrowbytes;
	int TEXT_X = 240 + readrow*3 + 20;
	int TEXT_BASE_X = 240 - readrow*3 - 20;
	int TEXT_LINE_X_BASE = TEXT_X-10;
	int TEXT_ROW_X_BASE = TEXT_BASE_X+10;

	t_txtpack txtpack;

	fileinfo.autotxtread = 0;

	if(text_open(fname, config.txtrowbytes+2, &txtpack) != 0){
		return 0;
	}
	int cr = 0, rp = 1;
	char rowstr[40];
	layout_filepos(fname, &cr, 0, 4);

	if(strcmp(fileinfo.orgtxtname,fname)!=0)	strcpy(fileinfo.orgtxtname,fname);
	else cr = fileinfo.orgtxtcr;

	int i;
	p_textrow row;
	int textrows = text_rows(&txtpack);
	int temp = textrows - 1;
	while(rp >= 0)
	{
		if(rp)
		{
			font_fillrect(TEXT_BASE_X, TEXT_BASE_Y, TEXT_X, TEXT_Y-1);
#ifdef DEBUGUI
			debug_memui();
#endif
			for(i = 0; i < TXT_LINES; i ++)
			{
				if(cr + i > temp)
					break;
				row = text_read(cr + i, &txtpack);
				font_outputn(TEXT_ROW_X_BASE, 56 + 14 * i, row->start, row->count);
			}
			if(textrows > TXT_LINES)
			{
				font_line(TEXT_LINE_X_BASE+1, 54, TEXT_LINE_X_BASE+1, 221);
				font_line(TEXT_LINE_X_BASE, 54 + 167 * cr / (textrows + TXT_LINES-1), TEXT_LINE_X_BASE, 54 + 167 * (cr + TXT_LINES) / (textrows + TXT_LINES-1));
				font_line(TEXT_LINE_X_BASE+2, 54 + 167 * cr / (textrows + TXT_LINES-1), TEXT_LINE_X_BASE+2, 54 + 167 * (cr + TXT_LINES) / (textrows + TXT_LINES-1));
			}
			sprintf(rowstr, "%d/%d", cr + 1, textrows);
			font_output(320, 232, rowstr);
			font_output(102, 34, LAYOUT_READTEXT_SAVEHELP);
			rp = 0;
			font_switch_refresh();
		}
		switch(ctrl_waitmask(PSP_CTRL_SELECT|PSP_CTRL_TRIANGLE | PSP_CTRL_LTRIGGER | PSP_CTRL_RTRIGGER | PSP_CTRL_UP | PSP_CTRL_DOWN | PSP_CTRL_LEFT | PSP_CTRL_RIGHT | PSP_CTRL_CIRCLE | PSP_CTRL_CROSS | PSP_CTRL_START))
		{
		case PSP_CTRL_CIRCLE:
		case PSP_CTRL_CROSS:
			rp = -1;
			fileinfo.orgtxtcr = cr;
			break;
		case PSP_CTRL_LTRIGGER:
 			if(cr > 0)
			{
				cr -= temp>>3;
				if(cr < 0)
					cr = 0;
				rp = 1;
			}
			break;
		case PSP_CTRL_RTRIGGER:
			if(cr < temp)
			{
				cr += temp>>3 ;
				if(cr > temp)
					cr = temp;
				rp = 1;
			}
			break;
		case PSP_CTRL_UP:
			if(cr > 0)
			{
				cr --;
				rp = 1;
			}
			break;
		case PSP_CTRL_DOWN:
			if(cr < temp)
			{
				cr ++;
				rp = 1;
			}
			break;
		case PSP_CTRL_LEFT:
			if(cr > 0)
			{
				cr -= TXT_LINES;
				if(cr < 0)
					cr = 0;
				rp = 1;
			}
			break;
		case PSP_CTRL_RIGHT:
 			if(cr < temp)
			{
				cr += TXT_LINES;
				if(cr > temp)
					cr = temp;
				rp = 1;
			}
			break;
		case PSP_CTRL_START:
			layout_filepos(fname, &cr, 1, 4);
			break;
		case PSP_CTRL_TRIANGLE:
		case PSP_CTRL_SELECT:
			fileinfo.orgtxtcr = cr;
			fileinfo.autotxtread = 1;
			rp = -1;
			break;
		}
	}
	text_close(&txtpack);
	font_switch_back();
	font_fillrect(TEXT_BASE_X, TEXT_BASE_Y, TEXT_X, TEXT_Y-1);
	return 1;
}


#define MAX(a,b)	(a>b?a:b)

t_keylist_table g_keylist[MAX_KEYLIST]  __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );

static int g_idx;
static int irecordkey;
static int keylist_cb(unsigned int key, int *id, int *topid)
{
	int idx = *id;
		if(key == PSP_CTRL_TRIANGLE)
		{
			g_keylist[idx].key = ctrl_input();
			*id = (*id+1)<<16;
			return 2;
		}
		else
		if(key==PSP_CTRL_LEFT)//set all key interval
		{
			unsigned int tempval=g_keylist[idx].list[0].stamp;
			ui_input_dec(56+12*14, 56+12*idx, tempval, &tempval, 1, 0xffff);
			int i;
			for(i=0;i<MAX_KEYSET;i++){
				g_keylist[idx].list[i].stamp=(u16)(tempval);
			}
			return 3;
		}
		else
		if(key==PSP_CTRL_RIGHT)//next page
		{
			*id = (*id+1)<<16;
			return 2;
		}
		else
		if(key==PSP_CTRL_SELECT)
		{
			g_keylist[idx].key = 0;
			return 2;
		}
		else if(key==PSP_CTRL_SQUARE)
		{
			g_keylist[idx].reversekey = ctrl_input();
			return 2;
		}
	return -1;
}

static int keylist_keyset_cb(unsigned int key, int *id, int *topid)
{
	int ikx = *id;
	int idx = g_idx;
			if(key==PSP_CTRL_RIGHT)
			{
				irecordkey=1;
				return 2;
			}
			else
			if(key == PSP_CTRL_TRIANGLE)
			{
				g_keylist[idx].list[ikx].btn = ctrl_input();
				g_keylist[idx].count=0;
				int j;
				for(j=MAX_KEYSET-1;j>=0;j--)
				{
					if(g_keylist[idx].list[j].btn>0)
					{
						g_keylist[idx].count=j+1;
						break;
					}
				}
				*id = (*id+1)<<16;
				return 2;
			}
			else
			if(key==PSP_CTRL_LEFT)//set single key interval
			{
				unsigned int tempval=g_keylist[idx].list[ikx].stamp;
				ui_input_dec(56+12*16, 56+12*6, tempval, &tempval, 1, 0xffff);
				g_keylist[idx].list[ikx].stamp=(u16)(tempval);
				*id = (*id+1)<<16;
				return 2;
			}
			else
			if(key==PSP_CTRL_SELECT)
			{
				g_keylist[idx].list[ikx].btn = 0;
				g_keylist[idx].count=0;
				int j;
				for(j=MAX_KEYSET-1;j>=0;j--)
				{
					if(g_keylist[idx].list[j].btn>0)
					{
						g_keylist[idx].count=j+1;
						break;
					}
				}
				*id = (*id+1)<<16;
				return 2;
			}
	return -1;
}

static const char * keylist_list[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	keylist_list1,
	keylist_list2,
};
static const char * keylist_setkey[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	keylist_setkey1,
	keylist_setkey2,
};
static const char * keylist_record[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	keylist_record1,
	keylist_record2,
};

void keylist_set()
{
	int idx;
	int tc = MAX(MAX(MAX_KEYLIST,MAX_KEYSET),12);
	char s[40];
	char s2[20];

	const char **tempitem;
	tempitem = malloc(tc*54);
	char *tempstr = (char *)((unsigned int)tempitem + tc*4);

	for(idx=0;idx<tc;idx++)
	{
		tempitem[idx] = &tempstr[50*idx];
	}
	idx=0;
	u32 key=0;

layout_keylist:
	while(1)
	{
		keylist_set_cls(keylist_list);
		int i;
		for(i=0;i<MAX_KEYLIST;i++){
			conf_get_keyname(g_keylist[i].key, s);
			conf_get_keyname(g_keylist[i].reversekey, s2);
			sprintf((char *)&tempitem[i][0],LAYOUT_KEY_MACRO_LIST, i, s, s2);
		}
		idx = ui_menu(110, 56, tempitem, MAX_KEYLIST, 12, idx, keylist_cb);
		if(idx < 0)
		{
			sfree(tempitem);
			return;
		}
		if(idx > 0xffff){
			idx = (idx>>16) - 1;
			break;
		}
	}

layout_keylist_set:
	{
		int ikx=0;
		irecordkey=0;
		u32 lastpress=0;
		while(1)
		{
			keylist_set_cls(keylist_setkey);
			int i;
			for(i=0;i<MAX_KEYSET;i++){
				conf_get_keyname(g_keylist[idx].list[i].btn, s);
				sprintf((char *)&tempitem[i][0],LAYOUT_KEY_MACRO_SET,i, g_keylist[idx].list[i].stamp, s);
			}
			g_idx = idx;
			ikx = ui_menu(110, 56, tempitem, MAX_KEYSET, 12, ikx, keylist_keyset_cb);
			if(ikx < 0){
				goto layout_keylist;
			}
			if(ikx > 0xffff) ikx = (ikx>>16);
			if(irecordkey)	break;
		}

		{
			int j;
			for(j=0;j<MAX_KEYSET;j++)
				g_keylist[idx].list[j].btn=0;
		}

		int topIndex=0;
		int currentIndex=0;
		while(1)
		{
			keylist_set_cls(keylist_record);
			
			int i;
			for(i=0;i<MAX_KEYSET;i++){
				conf_get_keyname(g_keylist[idx].list[i].btn, s);
				sprintf((char *)&tempitem[i][0],LAYOUT_KEY_MACRO_FASTSET,i, s);
			}
			
			for(i = 0; i < 12; i ++)
				font_output(110 + 12, 56 + i * 12, (char *)&tempitem[i+topIndex][0]);
			key=ctrl_waitchange(ctrl_waitany());
			//if(lastpress!=ctrl_ctx.last_tick)
			{
				//ctrl_waitrelease();
				if(key==PSP_CTRL_START)
				{
					irecordkey=0;
					goto layout_keylist_set;
				}
				else
				{
					if(key==PSP_CTRL_SELECT) key=0;
					g_keylist[idx].list[currentIndex].btn = key;
					//g_keylist[idx].list[currentIndex].stamp = ((ctrl_ctx.last_tick - ctrl_ctx.change_tick)>>12)-4;
					if(currentIndex>=11&&topIndex+11<MAX_KEYSET-1)
						topIndex++;
					g_keylist[idx].count=currentIndex+1;
					if(currentIndex<MAX_KEYSET-1)
					currentIndex++;
				}
			}
		}
	}
}


#define PSBIOS_FONTDAT_LEN	104640
#define PSBIOS_SEEK_START (0x300000+0x8800000)
#define PSBIOS_SEEK_END (0x1000000+0x8800000)



char matchbios[32] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = {
	//0x04, 0x12, 0x95, 0x19, 0x03, 0x00, 0x00, 0x00, 0x53, 0x6F, 0x6E, 0x79, 0x20, 0x43, 0x6F, 0x6D,
	//0x70, 0x75, 0x74, 0x65, 0x72, 0x20, 0x45, 0x6E, 0x74, 0x65, 0x72, 0x74, 0x61, 0x69, 0x6E, 0x6D,
	0x65, 0x6E, 0x74, 0x20, 0x49, 0x6E, 0x63, 0x2E, 0x00, 0x00, 0x00, 0x00, 0x43, 0x45, 0x58, 0x2D,
	0x33, 0x30, 0x30, 0x30, 0x2F, 0x31, 0x30, 0x30, 0x31, 0x2F, 0x31, 0x30, 0x30, 0x32, 0x20, 0x62,
};
static int write_psfont(char *fname)
{
	unsigned int i;
	char *p;

		for(i=PSBIOS_SEEK_START;i<PSBIOS_SEEK_END;i+=4){
			p=(char*)(i);
			//查找到内存中ps bios字库地址
			if(memcmp(p,matchbios,32)==0){
				p=(char*)(i+0x65F00-0x20);
				return write_ram(fname, p, PSBIOS_FONTDAT_LEN);
			}
		}
		return 0;
}

static char *find_psx_mc()
{
	char *p;
	u32 *i;
	for(i=(u32*)PSBIOS_SEEK_START;i<(u32*)PSBIOS_SEEK_END;i++){
		if((*i==0x564d5000) && (*(i+1)==0x80)){
			p=(char*)i+0x80;
			return p;
		}
	}
	return NULL;
}

static int import_psx_mc(char* fn)
{
	char *p=find_psx_mc();
	if(p!=NULL){
		write_ram(fn, p, 128*1024);
	}
	return 0;
}

static void export_psx_mc()
{
	sceIoMkdir(MCR_DIR, 0777);
	char *p=find_psx_mc();
	if(p!=NULL){
		char s[128];
		char name[16];
		mips_memcpy(name, ui_get_gamename(), 12);
		name[11]=0;
		sprintf(s, "%s/CMF_%s.mcr", MCR_DIR, name);
		int fd = sceIoOpen(s, PSP_O_WRONLY | PSP_O_CREAT | PSP_O_TRUNC, 0777);
		if(fd >= 0){
			sceIoWrite(fd, p, 128*1024);
			sceIoClose(fd);
		}
	}	
}

t_keyset keyset __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );;

const unsigned int turbo_key_tab[20] ={
PSP_CTRL_SQUARE, PSP_CTRL_TRIANGLE, PSP_CTRL_CIRCLE, PSP_CTRL_CROSS,
PSP_CTRL_SELECT, PSP_CTRL_START, PSP_CTRL_UP, PSP_CTRL_RIGHT,
PSP_CTRL_DOWN, PSP_CTRL_LEFT, PSP_CTRL_LTRIGGER, PSP_CTRL_RTRIGGER,
PSP_ANA_UP, PSP_ANA_DOWN, PSP_ANA_LEFT, PSP_ANA_RIGHT,
PSP_CTRL_VOLUP, PSP_CTRL_VOLDOWN, PSP_CTRL_NOTE, PSP_CTRL_SCREEN,
};
char *turbo_key_symbol[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )={
key_symbol1 ,
key_symbol2 ,
key_symbol3 ,
key_symbol4 ,
key_symbol5 ,
key_symbol6 ,
key_symbol7 ,
key_symbol8 ,
key_symbol9 ,
key_symbol10,
key_symbol11,
key_symbol12,
key_symbol13,
key_symbol14,
key_symbol15,
key_symbol16,
key_symbol17,
key_symbol18,
key_symbol19,
key_symbol20,
};

static int turbo_key_set_cb(unsigned int key, int *id, int *topid)
{
	int idx = *id;
{
	if(key==PSP_CTRL_LEFT)
	{
		keyset.turbokey ^= turbo_key_tab[idx];
		return 2;
	}
	else if(key==PSP_CTRL_RIGHT)
	{
		unsigned int tempval;
		tempval=keyset.turbo_key_interval[idx];
		ui_input_dec(112+12*10, 56+12*idx, tempval, &tempval, 1, 254);
		keyset.turbo_key_interval[idx]=(u8)tempval;
		return 2;
	}
	else if(key==PSP_CTRL_START)
	{
		keyset.turbo_skey[idx] = ctrl_input();
		return 2;
	}
	else if(key==PSP_CTRL_SELECT)
	{
		keyset.turbo_skey[idx] = PSP_CTRL_UP|PSP_CTRL_DOWN|PSP_ANA_RIGHT|PSP_ANA_LEFT;
		return 2;
	}
	return -1;
}
}

static const char * turbo_key_help[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	turbo_key_help1,
	turbo_key_help2,
};
static void turbo_key_set()
{
	int idx;
	char tempstr[12][50];
	const char *tempitem[12];
	for(idx=0;idx<12;idx++)
	{
		tempitem[idx]=tempstr[idx];
	}
	idx=0;
	char s[45];

	while(1)
	{
		keylist_set_cls(turbo_key_help);
		int i;
		for(i=0;i<12;i++){
			conf_get_keyname(keyset.turbo_skey[i], s);
			sprintf(tempstr[i], LAYOUT_KEY_TURBO_SET, turbo_key_symbol[i], (keyset.turbokey & turbo_key_tab[i]) ? menu_yesno[0] : menu_yesno[1], keyset.turbo_key_interval[i], s);
		}
		idx = ui_menu(110, 56, tempitem, 12, 12, idx, turbo_key_set_cb);
		if(idx < 0)
			return;
	}
}

static int turbo_map_set_cb(unsigned int key, int *id, int *topid)
{
	int idx = *id;
		if(key==PSP_CTRL_LEFT)
		{
			keyset.keymap_skey[idx] = ctrl_input();
			return 2;
		}
		else if(key==PSP_CTRL_RIGHT)
		{
			keyset.keymap_table[idx] = ctrl_input();
			return 2;
		}
 		else if(key==PSP_CTRL_SELECT)
		{
			keyset.keymap_skey[idx] = 0;
			keyset.keymap_table[idx] = 0;
			return 2;
		}
	return -1;
}

static const char * turbo_map_help[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	turbo_map_help1,
	turbo_map_help2,
};

static void turbo_analog_set()
{
	int idx;
	char tempstr[8][45];
	const char *tempitem[8];

	{
		tempitem[0]=tempstr[0];
		tempitem[1]=tempstr[1];
	}
	idx=0;

	while(1)
	{
		ui_cls();
		int i;
		for(i=0;i<2;i++){
			sprintf(tempstr[i], "%-20s%s", keymap_str[i], keyset.dac[i] ? menu_yesno[0] : menu_yesno[1]);
		}
		//sprintf(tempstr[0], "%-20s%s", keymap_str[0], keyset.dac[0] ? menu_yesno[0] : menu_yesno[1]);
		//sprintf(tempstr[1],"%-20s%s", keymap_str[1], keyset.dac[1] ? menu_yesno[0] : menu_yesno[1]);

		idx = ui_menu(110, 56, tempitem, 2, 2, idx, NULL);
		if(idx < 0)
			break;
		keyset.dac[idx] = !keyset.dac[idx];
	}
}

static void turbo_map_set()
{
	int idx;
	char s[35];
	char tempstr[16][50];
	const char *tempitem[16];
	for(idx=0;idx<16;idx++)
	{
		tempitem[idx]=tempstr[idx];
	}
	idx=0;
	char ss[35];
	while(1)
	{
		keylist_set_cls(turbo_map_help);
		int i;
		for(i=0;i<16;i++){
			conf_get_keyname(keyset.keymap_table[i], s);
			conf_get_keyname(keyset.keymap_skey[i], ss);
			sprintf(tempstr[i],LAYOUT_KEY_MAP_SET, ss, s);
		}
		idx = ui_menu(104, 56, tempitem, 16, 12, idx, turbo_map_set_cb);
		if(idx < 0)
			break;
	}
}

static const char * turbo_stick_help[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	turbo_stick_help1,
	turbo_key_help2,
};
static int turbo_stick_set_cb(unsigned int key, int *id, int *topid)
{
	int idx = *id;
		if(key==PSP_CTRL_RIGHT)
		{
			keyset.stick_table[idx] = ctrl_input();
			return 2;
		}
		else if(key==PSP_CTRL_LEFT)
		{
			keyset.autokey ^= 1<<idx;
			return 2;
		}
		else if(key==PSP_CTRL_START)
		{
			keyset.stick_skey[idx] = ctrl_input();
			return 2;
		}
	else if(key==PSP_CTRL_SELECT)
	{
		keyset.stick_skey[idx] = PSP_CTRL_UP|PSP_CTRL_DOWN|PSP_ANA_RIGHT|PSP_ANA_LEFT;
		return 2;
	}
	return -1;
}
static void turbo_stick_set()
{
	int idx;
	char s[35];
	char tempstr[16][50];
	const char *tempitem[16];
	for(idx=0;idx<16;idx++)
	{
		tempitem[idx]=tempstr[idx];
	}
	idx=0;
	char ss[35];
	while(1)
	{
		keylist_set_cls(turbo_stick_help);
		int i;
		for(i=0;i<16;i++){
			conf_get_keyname(keyset.stick_table[i], s);
			conf_get_keyname(keyset.stick_skey[i], ss);
			sprintf(tempstr[i],LAYOUT_KEY_STICK_SET, s, (keyset.autokey&(1<<i)) ? menu_yesno[0] : menu_yesno[1], ss);
		}
		idx = ui_menu(104, 56, tempitem, 16, 12, idx, turbo_stick_set_cb);
		if(idx < 0)
			break;
	}
}

static void keyset_save()
{
	char s[80];
	char namestr[40];
	memset(namestr,0,40);
	mips_memcpy(namestr,ui_get_gamename(),10);
	sceIoMkdir(SET_DIR, 0777);
	ui_cls();
	if(ui_input_string(120, 68, namestr, 29) < 1)
		return;
	filter_filename(namestr);
	sprintf(s, "%s/%s.set", SET_DIR, namestr);
	int fd = sceIoOpen(s, PSP_O_WRONLY | PSP_O_CREAT | PSP_O_TRUNC, 0777);
	if(fd >= 0)
	{
		sceIoWrite(fd, &keyset, sizeof(keyset));
		sceIoWrite(fd, &g_keylist, sizeof(t_keylist_table)*MAX_KEYLIST);
		sceIoClose(fd);
	}
}

static int keyset_load(char *filename)
{
	int fd = sceIoOpen(filename, PSP_O_RDONLY, 0777);
	if(fd >= 0)
	{
		sceIoRead(fd, &keyset, sizeof(keyset));
		sceIoRead(fd, &g_keylist, sizeof(t_keylist_table)*MAX_KEYLIST);
		sceIoClose(fd);
	}
	return 0;
}

static char pngprx [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/CheatMaster/prx/image.prx";
static char jpgprx [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/CheatMaster/prx/imgjpg.prx";
static const char *img_prxfile[]  __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	pngprx,
	jpgprx,
};

static int img_popsdoc(char* filename)
{
	fileinfo.autotxtread = 0;

	if(img_Loadprx(pngprx)!=0) return 0;
	int fd = sceIoOpen(filename, PSP_O_RDONLY, 777);
	if (fd < 0) {img_Unloadprx();return 0;}
	int cr = 0, rp = 0;
	int pagemax;
	sceIoLseek(fd, 0x84, SEEK_SET);
	sceIoRead(fd, &pagemax, 4);
	sceIoClose(fd);
	char rowstr[40];
	layout_filepos(filename, &cr, 0, 4);

	if(strcmp(fileinfo.orgtxtname,filename)!=0)	strcpy(fileinfo.orgtxtname,filename);
	else cr = fileinfo.orgtxtcr;
	t_imginfo img;
	img.x = 0;
	img.y = 0;
	unsigned int temp;
	while(rp==0)
	{
		if(prx_showpng(filename, cr, &bgctx, &img)!=0)
			break;
		sprintf(rowstr, "%d/%d", cr + 1, pagemax);
		font_output(426, 260, rowstr);
		font_switch_refresh();
		switch(ctrl_waitmask(PSP_CTRL_SELECT|PSP_ANA_UP|PSP_ANA_DOWN|PSP_CTRL_UP|PSP_CTRL_DOWN|PSP_CTRL_TRIANGLE | PSP_CTRL_SQUARE | PSP_CTRL_START | PSP_CTRL_LTRIGGER | PSP_CTRL_RTRIGGER | PSP_CTRL_LEFT | PSP_CTRL_RIGHT | PSP_CTRL_CIRCLE | PSP_CTRL_CROSS))
		{
		case PSP_CTRL_CIRCLE:
		case PSP_CTRL_CROSS:
			rp = 1;
			fileinfo.orgtxtcr = cr;
			break;
		case PSP_ANA_UP:
			img.y -= 272;
			if(img.y<0)
				img.y = 0;
			break;
		case PSP_ANA_DOWN:
			img.y += 272;
			if(img.y+272>=img.height)
				img.y = img.height - 272;
			break;
		case PSP_CTRL_UP:
			img.y -= 46;
			if(img.y<0)
				img.y = 0;
			break;
		case PSP_CTRL_DOWN:
			img.y += 46;
			if(img.y+272>=img.height)
				img.y = img.height - 272;
			break;
		case PSP_CTRL_LTRIGGER:
			cr -= 10;
			if(cr<0)
				cr = 0;
			//img.x = 0;
			img.y = 0;
			break;
		case PSP_CTRL_RTRIGGER:
			cr += 10;
			if(cr>=pagemax)
				cr = pagemax - 1;
			//img.x = 0;
			img.y = 0;
			break;
		case PSP_CTRL_LEFT:
			if(cr>0)
				cr --;
			//img.x = 0;
			img.y = 0;
			break;
		case PSP_CTRL_RIGHT:
			if(cr<pagemax-1)
				cr ++;
			//img.x = 0;
			img.y = 0;
			break;
		case PSP_CTRL_START:
			layout_filepos(filename, &cr, 1, 4);
			break;
		case PSP_CTRL_SQUARE:
			temp = cr+1;
			font_switch_vram();
			ui_input_dec(300, 260, temp, &temp, 1, pagemax);
			if(temp<=pagemax){
				cr = temp - 1;
				//img.x = 0;
				img.y = 0;
			}
			break;
		case PSP_CTRL_TRIANGLE:
		case PSP_CTRL_SELECT:
			fileinfo.orgtxtcr = cr;
			fileinfo.autotxtread = 2;
			rp = 1;
			break;
		}
		ctrl_waitrelease();
	}
	img_Unloadprx();
	font_switch_back();
	return 1;
}

static char layout_read_ext[][4] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = {
"txt","fnt","set","cmf","mem","dat","png","jpg","mcr",
};

static void strip_filename(char *filename, int idx)
{
		memset(filename,0,80);
		sprintf(filename, "%s%s", fileinfo.g_dir, g_text_array[idx]);
		char *p = (strrchr(filename, '.')+4);
		*p=0;
}

static void img_view_changepic(char* filename, t_imginfo *img)
{
	img->x = 0;
	img->y = 0;
	strip_filename(filename, fileinfo.g_read_fileno);
	layout_filepos(filename, &img->x, 0, 8);
	if(strcmp(fileinfo.orgtxtname,filename)==0)
	{
		img->x = fileinfo.imgx;
		img->y = fileinfo.imgy;
	}
}

static int img_view(char* filename)
{
	fileinfo.autotxtread = 0;
	t_imginfo img;
	img_view_changepic(filename, &img);

	int ext;
	if(strcmpi(fileinfo.g_ext,layout_read_ext[6])==0) ext = 0;		//png
	else ext = 1;			//jpg

	if(img_Loadprx(img_prxfile[ext])!=0) return 0;
	int ret,rp=0;

	while(rp==0)
	{
		if(ext==0)
			ret = prx_showpng(filename, 10000, &bgctx, &img);
		else
			ret = prx_openjpg(filename, &bgctx, &img);
		if(ret!=0) break;
		//font_output(12, 2, filename);
		font_switch_refresh();
		switch(ctrl_waitmask(PSP_CTRL_SELECT|PSP_CTRL_START|PSP_CTRL_TRIANGLE|PSP_CTRL_LTRIGGER|PSP_CTRL_RTRIGGER|PSP_CTRL_UP|PSP_CTRL_DOWN|PSP_CTRL_LEFT|PSP_CTRL_RIGHT|PSP_CTRL_CIRCLE|PSP_CTRL_CROSS|PSP_ANA_UP|PSP_ANA_DOWN|PSP_ANA_LEFT|PSP_ANA_RIGHT))
		{
		case PSP_CTRL_CIRCLE:
		case PSP_CTRL_CROSS:
			rp = 1;
			strcpy(fileinfo.orgtxtname,filename);
			fileinfo.imgx=img.x;
			fileinfo.imgy=img.y;
			break;
		case PSP_CTRL_LTRIGGER:
			if(fileinfo.g_read_fileno>0){
				fileinfo.g_read_fileno --;
				img_view_changepic(filename, &img);
			}
			break;
		case PSP_CTRL_RTRIGGER:
			if(fileinfo.g_read_fileno<fileinfo.g_read_filemax-1){
				fileinfo.g_read_fileno ++;
				img_view_changepic(filename, &img);
			}
			break;
		case PSP_CTRL_UP:
			img.y -= 46;
			if(img.y<0)
				img.y = 0;
			break;
		case PSP_CTRL_DOWN:
			img.y += 46;
			if(img.y+272>=img.height)
				img.y = img.height - 272;
			break;
		case PSP_CTRL_LEFT:
			img.x -= 80;
			if(img.x<0)
				img.x = 0;
			break;
		case PSP_CTRL_RIGHT:
			img.x += 80;
			if(img.x+480>=img.width)
				img.x = img.width - 480;
			break;
		case PSP_ANA_UP:
			img.y -= 272;
			if(img.y<0)
				img.y = 0;
			break;
		case PSP_ANA_DOWN:
			img.y += 272;
			if(img.y+272>=img.height)
				img.y = img.height - 272;
			break;
		case PSP_ANA_LEFT:
			img.x -= 480;
			if(img.x<0)
				img.x = 0;
			break;
		case PSP_ANA_RIGHT:
			img.x += 480;
			if(img.x+480>=img.width)
				img.x = img.width - 480;
			break;
		case PSP_CTRL_START:
			layout_filepos(filename, &img.x, 1, 8);
			break;
		case PSP_CTRL_TRIANGLE:
		case PSP_CTRL_SELECT:
			strcpy(fileinfo.orgtxtname,filename);
			fileinfo.imgx=img.x;
			fileinfo.imgy=img.y;
			fileinfo.autotxtread = 3;
			rp = 1;
			break;
		}
		ctrl_waitrelease();
	}
	img_Unloadprx();
	font_switch_back();
	return 1;
}


static int	layout_read_cb(unsigned int key, int *id, int *topid)
{
	if(key == PSP_CTRL_SQUARE)
	{
		fileinfo.g_read_fat = !fileinfo.g_read_fat;
		*id = 0x10000;
		return 2;
	}
	
	if(*id>=fileinfo.g_read_filemax) return -1;
	
	if(key & (PSP_CTRL_SELECT|PSP_CTRL_START))
	{
		char fn[80];
		strip_filename(fn, *id);
		char *p = (strrchr(fn, '.')+4);

		if(key == PSP_CTRL_SELECT)
			sceIoRemove(fn);
		else
		{
			char newfn[80];
			char *str = fn;
			str = strrchr(fn, 0x2F);
			int len = str-fn+1;
			//len=strlen(fileinfo.g_dir);
			memset(newfn,0,80);
			mips_memcpy(newfn, fn, p-fn-4);

			UTF8SJIS_SJIS(newfn,80);

			ui_cls();
			if(ui_input_string(120, 68, newfn+len, 29) > 0){
				//filter_filename(newfn+len);
				//strcat(newfn, p-4);

			SJIS_UTF8SJIS(newfn,80);
			filter_filename(newfn+len);
			memcpy(&newfn[strlen(newfn)],&fn[strlen(fn)-4],5);

			sceIoRename(fn, newfn);
			}
		}

		*id = 0x10000;
		return 2;
	}
	
	return -1;
}

static void * layout_read_fun[] = {
layout_read_text,write_psfont,keyset_load,convert_cmf,write_mem,img_popsdoc,img_view,img_view,import_psx_mc,
};

#define FAT_FILEATTR_READONLY	0x01
#define FAT_FILEATTR_HIDDEN	0x02
#define FAT_FILEATTR_SYSTEM	0x04
#define FAT_FILEATTR_VOLUME	0x08
#define FAT_FILEATTR_DIRECTORY	0x10
#define FAT_FILEATTR_ARCHIVE	0x20

static char fatprx [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/CheatMaster/prx/fat_sjis.prx";
static void * layout_readdir(char *tempdir, char *ext, int *c, int *dc, int fatread)
{
	const char ** text_array;
	char * text_buf;
	const char ** dir_array;
	char * dir_buf;
	
	const char ** decode_text_array;
	char * decode_text_buf;
	const char ** decode_dir_array;
	char * decode_dir_buf;
	
	int dl,i,l;
	int di;
	int tempc,tempdc;
	dl = sceIoDopen(tempdir);
	if(dl < 0)
		return NULL;
	tempc = 0;
	tempdc = 1;
	SceIoDirent sid;
	memset(&sid, 0, sizeof(SceIoDirent));
	while(sceIoDread(dl, &sid) > 0)
	{
		if((sid.d_stat.st_attr & FAT_FILEATTR_VOLUME) > 0 || sid.d_name[0]=='.')
			continue;
		l = strlen(sid.d_name);
		if(l > 41)
			continue;
		if((sid.d_stat.st_attr & FAT_FILEATTR_DIRECTORY) > 0)
			tempdc++;
		else{
			if(strcmpi(&sid.d_name[l - 3], ext) != 0)
				continue;
			tempc ++;
		}
	}
	sceIoDclose(dl);
	*c = tempc;
	*dc = tempdc;
	strcpy(fileinfo.g_dir, tempdir);
	fileinfo.g_read_filemax = tempc;
	
	int countbyte = (tempc*52 + tempdc*52);

	text_array = malloc(countbyte*2);
	g_text_array = text_array;
	decode_text_array = (const char **)((unsigned int)text_array + countbyte);
	
	dir_array = (const char **)((unsigned int)text_array + tempc*4);
	decode_dir_array = (const char **)((unsigned int)decode_text_array + tempc*4);
	
	text_buf = (char *)((unsigned int)dir_array + tempdc*4);
	decode_text_buf = (char *)((unsigned int)decode_dir_array + tempdc*4);
	
	dir_buf = (char *)((unsigned int)text_buf + tempc*48);
	decode_dir_buf = (char *)((unsigned int)decode_text_buf + tempc*48);
	
	for(i = 0; i < tempc; i ++){
		text_array[i] = &text_buf[48 * i];
		decode_text_array[i] = &decode_text_buf[48 * i];
	}
	for(i = 0; i < tempdc; i ++){
		dir_array[i] = &dir_buf[48 * i];
		decode_dir_array[i] = &decode_dir_buf[48 * i];
	}

	i = 0;
	di = 1;
	dl = sceIoDopen(tempdir);
	memset(&sid, 0, sizeof(SceIoDirent));

	while(sceIoDread(dl, &sid) > 0)
	{
		if((sid.d_stat.st_attr & FAT_FILEATTR_VOLUME) > 0 || sid.d_name[0]=='.')
			continue;
		l = strlen(sid.d_name);
		if(l > 41)
			continue;
		if((sid.d_stat.st_attr & FAT_FILEATTR_DIRECTORY) > 0){
			sprintf((char *)dir_array[di++],"%s/",sid.d_name);
		}
		else{
			if(strcmpi(&sid.d_name[l - 3], ext) != 0)
				continue;
			sprintf((char *)text_array[i++],"%-36s%4dKiB\x0",sid.d_name,(int)(sid.d_stat.st_size)>>10);
		}
	}
	sceIoDclose(dl);
	strcpy((char *)dir_array[0],"../");

	char stmm[45]="                                             ";
	char stmn[8]="       \x0";
	char stzz[8]="          ";
	int stend=0;
	int k=0;

	for(i=0;i<tempc;i++){
		strcpy(stmm, (char *)text_array[i]);
		memcpy(&stmn[0],&stmm[strlen(stmm)-7],7);
		memcpy(&stmm[strlen(stmm)-7],&stzz[0],7);

		stend=UTF8SJIS_SJIS(stmm,36);
		for(k=stend;k<36;k++){
		stmm[k]=0x20;
		}

		memcpy(&stmm[36],&stmn[0],8);
		//stmm[36]=filename_encode+0x30; //MODE澄千脱

		strcpy((char*)decode_text_array[i], stmm);
		// strcpy((char*)decode_text_array[i], (char*)text_array[i]);
	}
	
	for(i=0;i<tempdc;i++){
		strcpy(stmm,(char*)dir_array[i]);
		stend=UTF8SJIS_SJIS(stmm,36);
		for(k=stend;k<36;k++){
		stmm[k]=0;
		}
		stmm[36]=0;

		strcpy((char*)decode_dir_array[i], stmm);
	//strcpy((char*)decode_dir_array[i], (char*)dir_array[i]);
	}
	
	if(fatread && (img_Loadprx(fatprx)==0))
	{
		prx_fatread(tempdir, ext, decode_text_array, decode_dir_array);
		img_Unloadprx();
	}
	
	return decode_text_array;
}

int layout_autoload_dir(char *gname, char *dir, char *fn)
{
	int dl;
	dl = sceIoDopen(dir);
	if(dl < 0)
		return 1;
	char *ext = dir + strlen(dir) - 3;
	SceIoDirent sid;
	memset(&sid, 0, sizeof(SceIoDirent));
	while(sceIoDread(dl, &sid) > 0)
	{
		if((sid.d_stat.st_attr & (FAT_FILEATTR_VOLUME|FAT_FILEATTR_DIRECTORY)) > 0 || sid.d_name[0]=='.')
			continue;
		int l = strlen(sid.d_name);
		if(l > 41)
			continue;
		if(strcmpi(&sid.d_name[l - 3], ext) != 0)
			continue;
		if(memcmp(gname, sid.d_name, 10)==0)
		{
			sceIoDclose(dl);
			sprintf(fn,"%s/%s",dir,sid.d_name);
			return 0;
		}
	}
	sceIoDclose(dl);
	return 1;
}

static void	layout_read(char *ext, char *andir)
{
	const char ** text_array;
	const char ** dir_array;
	const char ** decode_text_array;
	char * p;

	int i,c,dc,idx;

	for(i=0;i<9;i++){
		if(strcmpi(ext,layout_read_ext[i])==0) break;
	}
	int (*fun)(char *);
	fun = layout_read_fun[i];

	char fn[80];
	char tempdir[80];
	if(andir==NULL)
		sprintf(tempdir, "%s/%s/", MODULE_DIR, ext);
	else
		strcpy(tempdir, andir);
	strcpy(fileinfo.g_ext,ext);
READAGAIN:
	decode_text_array = layout_readdir(tempdir, fileinfo.g_ext, &c, &dc, fileinfo.g_read_fat);
	if(decode_text_array==NULL) return;
	text_array = g_text_array;
	dir_array = (const char **)((unsigned int)text_array + c*4);

	idx = 0;
	do {
		font_fillrect(100, 30, 379, 240);
		font_output(120,34,LAYOUT_READ_HELP);
		idx = ui_menu(110, 56, decode_text_array, c+dc, 14, idx, layout_read_cb);
		if(idx < 0)
			break;
		if(idx >= c)
		{
			if(idx<0x7fff)
			{
				idx -= c;
				if(idx==0){
					p = strrchr(tempdir,'/');
					if(p){
						*p=0;
						p = strrchr(tempdir,'/');
						if(p)
							*(p+1) = 0;
					}
				}
				else{
					strcat(tempdir,dir_array[idx]);
				}
			}
			sfree(text_array);
			goto READAGAIN;
		}
		fileinfo.g_read_fileno = idx;
		strip_filename(fn, idx);
		if(fun(fn)==0 || fileinfo.autotxtread)
			break;
	} while(1);
	sfree(text_array);
}

static void layout_key()
{
	int idx;
	while(1)
	{
		idx = ui_menu(110+12*6, 56+7*12, layout_menu_key, 7, 7, 0, NULL);
		switch(idx)
		{
		case 0:
			turbo_key_set();
			return;
		case 1:
			turbo_map_set();
			return;
		case 2:
			turbo_stick_set();
			return;
		case 3:
			turbo_analog_set();
			return;
		case 4:
			keylist_set();
			return;
		case 5:
			layout_read(layout_read_ext[2],NULL);
			return;
		case 6:
			keyset_save();
			return;
		default:
			return;
		}
	}
}

static void layout_load()
{
	int idx;
	while(1)
	{
		idx = ui_menu(110+12*6, 56+12*3, layout_menu_load, 4, 4, 0, NULL);
		switch(idx)
		{
		case 0:
			layout_read(layout_read_ext[3],NULL);
			return;
		case 1:
			layout_loadtab();
			layout_gv.table_idx = 0;
			return;
		case 2:
			if(convert(ui_get_gamename())==0)
				layout_table();
			return;
		case 3:
			mem_table_clear();
			layout_gv.table_idx = 0;
			return;
		default:
			return;
		}
	}
}

static void layout_mem()
{
	int idx;
	while(1)
	{
		idx = ui_menu(110+12*6, 56+12*4, layout_menu_mem, 3, 3, 0, NULL);
		switch(idx)
		{
		case 0:
			layout_read(layout_read_ext[4],NULL);
			return;
		case 1:
			mem_dump(layout_gv.lowaddr, layout_gv.highaddr);
			return;
		case 2:
			mem_dump(layout_gv.dmaaddress,0xFFFFFFFF);
			return;
		default:
			return;
		}
	}
}

int g_bright;
/* 
static void layout_etc()
{
	int cur;
	int sub_idx;
			while(1)
			{
				sub_idx = ui_menu(110+12*6, 56+12*10, layout_menu_etc, 3, 3, 0, NULL);
				switch(sub_idx)
				{
				case 0:
					sceDisplayGetBrightness(&cur, 0);
					ui_input_dec(110+12*10, 12*6+56, cur, &cur, 28, 99);
					g_bright = cur;
					sceDisplaySetBrightness(cur, 0);
					return;
				case 1:
					layout_read(layout_read_ext[1],NULL);
					return;
				case 2:
					scePowerRequestStandby();
					return;
				default:
					return;
				}
			}
}
 */
static void layout_img()
{
	int sub_idx;
	sub_idx = ui_menu(110+12*6, 56+10*12, layout_menu_img, 4, 4, 0, NULL);
	if(sub_idx<0) return;
	else if(sub_idx==3)
		layout_read(layout_read_ext[7],CAPTURE_DIR);
	else
		layout_read(layout_read_ext[5+sub_idx],NULL);
}

static char dictstr[DICT_MAXWORD_LEN];
static int lidx;
extern int layout_menu()
{
	ctrl_waitrelease();

	int idx;

	if(fileinfo.autotxtread==1){
		layout_read_text(fileinfo.orgtxtname);
		if(fileinfo.autotxtread==0)
			layout_read(fileinfo.g_ext, fileinfo.g_dir);
	}
	else if(fileinfo.autotxtread==2) img_popsdoc(fileinfo.orgtxtname);
	else if(fileinfo.autotxtread==3){
		int c,dc;
		char fn[80];
		if(layout_readdir(fileinfo.g_dir,fileinfo.g_ext,&c,&dc,0)==NULL) goto MENUOUT;
		img_view(fn);
		free(g_text_array);
		if(fileinfo.autotxtread==0)
			layout_read(fileinfo.g_ext, fileinfo.g_dir);
	}
	
	int cur;
	int sub_idx;

	ui_cls();
	while(fileinfo.autotxtread==0)
	{
		if((idx = ui_menu(110, 56, menu_main, 14, 14, lidx, NULL)) < 0)
		{
			lidx = - idx - 1;
			break;
		}
		if(idx < 14)
			lidx = idx;
		switch(idx)
		{
		case 0:
			layout_search();
			break;
		case 1:
			{
				p_mem_table table;
				layout_gv.tc = mem_get_table(&table);
				if(layout_gv.tc > 0)
					layout_table();
				else
				{
					t_mem_table t;
					t.addr = layout_gv.sa;
					if(layout_get_table_item(LANG_NEWADDR, &t) < 0)
						break;
					t.addr += 0x08800000;
					if(mem_table_add(&t) >= 0)
						layout_table();
				}
			}
			break;
		case 2:
			layout_save();
			break;
		case 3:
			layout_load();
			break;
		case 4:
			layout_mem();
			break;
		case 5:
			layout_view(1);
			break;
		case 6:
			layout_read(layout_read_ext[0],NULL);
			break;
		case 7:
			layout_buscpu();
			break;
		case 8:
			layout_conf();
			break;
		case 9:
			dict_input_string(112, 42, dictstr, 40);
			break;
		case 10:
			layout_key();
			break;
		case 11:
			layout_img();
			break;
		case 12:
			{
				sub_idx = ui_menu(110+12*6, 56+12*8, layout_menu_etc, 5, 5, 0, NULL);
				switch(sub_idx)
				{
				case 0:
					sceDisplayGetBrightness(&cur, 0);
					ui_input_dec(110+12*10, 12*6+56, cur, &cur, 28, 99);
					g_bright = cur;
					sceDisplaySetBrightness(cur, 0);
					break;
				case 1:
					layout_read(layout_read_ext[1],NULL);
					break;
				case 2:
					layout_read(layout_read_ext[8],NULL);
					break;
				case 3:
					export_psx_mc();
					break;
				case 4:
					scePowerRequestStandby();
					freeBG();
					return 0;
				default:
					break;
				}
			}
			break;
		case 13:
			usb_LoadUSB(gameplaying());
			break;
		default:
			idx = -1;
			break;
		}
		if(idx < 0)
			break;
		ui_cls();
	}
MENUOUT:
	ctrl_waitrelease();
	freeBG();
	return 0;
}

extern int layout_init()
{
	ui_init();
	layout_gv.sv = 0x80;
	layout_gv.highaddr = 0x01800000;
	const char *p = ui_get_gamename();
	mem_table_index_init(p, config.lockspd << 2);
	
	char fn[128];
	if(config.autoloadset)
	{
		if(layout_autoload_dir(p, SET_DIR, fn)==0)
			keyset_load(fn);
	}
	
	return 0;
}


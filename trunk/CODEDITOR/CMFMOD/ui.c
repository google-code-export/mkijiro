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
#include <stdio.h>
#include <string.h>
#include <pspctrl.h>
#include <psppower.h>
#include <pspkerneltypes.h>
#include <pspsysmem_kernel.h>
#include <pspsysmem.h>
#include <pspinit.h>
#include "conf.h"
#include "ctrl.h"
#include "font.h"
#include "lang_zh.h"
#include "encode.h"
#include "version.h"
#include "ui.h"
#include "allocmem.h"
#include "minifloat.h"
#include "usb.h"
#include "smsutils.h"
typedef struct _ui_gv{
	char ui_gname[12+84];
	char ui_hex_mode;
	char floatmode;
	char gametype;
}__attribute__((packed)) t_ui_gv;

static t_ui_gv ui_gv  __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );

void debug_memui()
{
	char buffer[40];

	sprintf(buffer,"Free:%dK/%dK",
	sceKernelPartitionMaxFreeMemSize(1)>>10,sceKernelPartitionMaxFreeMemSize(6)>>10);
	//sceKernelGetModel()?sceKernelPartitionMaxFreeMemSize(8)>>10:0);
	font_output(198, 230, buffer);
}

void ui_init()
{
	int fd;
	memset(ui_gv.ui_gname, '-', 12);

	switch(sceKernelInitKeyConfig())
	{
	case PSP_INIT_KEYCONFIG_POPS:
		ui_gv.gametype = 2;
		char *p;
		int n=0;
		p=(char *)(0x100b8b7+0x8800000);
		while(*p==0 && n++<10)
		{
			sceKernelDelayThread(3000000);
		}
		p=(char *)(0x100b8b0+0x8800000);
		p=strchr(p, ';');
		if(p)
			mips_memcpy(ui_gv.ui_gname, p-11, 11);
		else strcpy(ui_gv.ui_gname,"PSX");
		break;

	case PSP_INIT_KEYCONFIG_GAME:
		fd = sceIoOpen(ISOFS_UMDDATA, PSP_O_RDONLY, 0777);
		if (fd >= 0)
		{
			sceIoRead(fd, ui_gv.ui_gname, 10);
			sceIoClose(fd);
		}
		fd = sceIoOpen(ISOFS_SFO, PSP_O_RDONLY, 0777);
		if (fd >= 0)
		{
			sceIoLseek32(fd, 0x8, PSP_SEEK_SET);
			unsigned char flag;
			sceIoRead(fd, &flag, 1);
			if(flag==0xb4)	sceIoLseek32(fd, 0x158, PSP_SEEK_SET);
			else	sceIoLseek32(fd, -0x84, PSP_SEEK_END);
			
			sceIoRead(fd, &ui_gv.ui_gname[12], 80);
			sceIoClose(fd);
			ui_gv.ui_gname[12 + 80] = 0;
			t_encodepack pack;
	 		if(encode_init(&pack) == 0)
			{
				//UCS2(utf16BE)化後テーブル検索し、EUC変換
				encode_utf8_conv((unsigned char *)&ui_gv.ui_gname[12], NULL,&pack);
				encode_free(&pack);
			}
		}
	default:
		ui_gv.gametype = 1;
		break;
	}
}

int gameplaying()
{
	return ui_gv.gametype;
}

void ui_cls()
{
	font_fillrect(96, 28, 383, 241);

#ifdef GAME3XX
	font_output(100, 30,"CMF371 " VER_STR);
#else
	font_output(100, 30,"CMF " VER_STR);
#endif
	if(usbinit){
		font_output(379-4*6, 42,"USB");
	}

	font_outputn(100, 42, ui_gv.ui_gname, 45);

 	char cpustr[40];
	sprintf(cpustr, "CPU/BUS:%d/%d", scePowerGetCpuClockFrequencyInt(), scePowerGetBusClockFrequency());
	font_output(100, 230, cpustr);
	sprintf(cpustr, "Bat:%d", scePowerGetBatteryLifePercent());
	font_output(334, 230, cpustr);
	debug_memui();
}

int ui_menu(int x, int y, const char ** item, int count, int pagecount, int sidx, int (*cbfunc)(unsigned int, int *, int *))
{
	if(count <= 0)
		return -1;
	if(pagecount > count)
		pagecount = count;
	int rp = 2;
	int topidx;
	if(sidx >= count)
		sidx = count - 1;
	if(sidx < 0)
		sidx = 0;
	if(sidx >= pagecount)
		topidx = sidx - pagecount + 1;
	else
		topidx = 0;
	int oidx = sidx - topidx;
	while(1)
	{
		if(rp == 1)
		{
			if(sidx < topidx)
			{
				topidx = sidx;
				rp = 2;
			}
			else if(sidx >= topidx + pagecount)
			{
				topidx = sidx - pagecount + 1;
				rp = 2;
			}
		}
		int i;
		switch(rp)
		{
		case 2:
			font_fillrect(x, y - 2, 379, y + pagecount * 12 - 3);
			if(pagecount < count)
			{
				font_line(x + 1, y - 2, x + 1, y + pagecount * 12 - 3);
				font_line(x, y - 2 + (pagecount * 12 - 1) * topidx / count, x, y - 2 + (pagecount * 12 - 1) * (topidx + pagecount) / count);
				font_line(x + 2, y - 2 + (pagecount * 12 - 1) * topidx / count, x + 2, y - 2 + (pagecount * 12 - 1) * (topidx + pagecount) / count);
			}
			for(i = 0; i < pagecount; i ++)
				font_output(x + 12, y + i * 12, item[i + topidx]);
		case 1:
			//if(rp == 1)
				font_fillrect(x + 4, y + oidx * 12 - 2, x + 11, y + oidx * 12 + 9);
			font_output(x + 4, y + (sidx - topidx) * 12, ">");
			font_refresh();
			rp = 0;
			break;
		}
		unsigned int key = ctrl_waitany();
		int rs = -1;
		if(cbfunc == NULL || (rs = cbfunc(key, &sidx, &topidx)) < 0){
			oidx = sidx - topidx;
			switch(key)
			{
			case PSP_CTRL_UP:
				sidx --;
				if(sidx < 0)
					sidx = count - 1;
				rp = 1;
				break;
			case PSP_CTRL_DOWN:
				sidx ++;
				if(sidx >= count)
					sidx = 0;
				rp = 1;
				break;
			case PSP_CTRL_LTRIGGER:
				topidx -= pagecount;
				if(topidx < 0)
					topidx = 0;
				sidx = topidx + oidx;
				rp = 2;
				break;
			case PSP_CTRL_RTRIGGER:
				topidx += pagecount;
				if(topidx > count - pagecount)
					topidx = count - pagecount;
				sidx = topidx + oidx;
				rp = 2;
				break;
			case PSP_CTRL_CIRCLE:
				ctrl_waitrelease();
				if(!config.swap)
					return sidx;
				else
					return  - sidx -1;
			case PSP_CTRL_CROSS:
				ctrl_waitrelease();
				if(config.swap)
					return sidx;
				else
					return  - sidx -1;
			}
		}
		else{
			oidx = sidx - topidx;
			switch(rs)
			{
			case 1:
				ctrl_waitrelease();
				return -1;
			case 2:
				ctrl_waitrelease();
				return sidx;
			case 3:
				rp = 2;
			}
		}
	}
	ctrl_waitrelease();
	return -1;
}

static unsigned int Dec2Float(unsigned int v)
{
	unsigned int temp;
	asm __volatile__ (
		"mtc1 %1, $f0;"
		"cvt.s.w $f0, $f0;"
		"mfc1 %0, $f0;"
		: "=r" (temp)
		: "r" (v)
	);
	return temp;
}

int ui_input_dec(int x, int y, unsigned int iv, unsigned int * ov, unsigned int min, unsigned int max)
{
	static const unsigned int basex[10] = {1000000000, 100000000, 10000000, 1000000, 100000, 10000, 1000, 100, 10, 1};
	int pos = 8;
	char shows[12];
	if(ui_gv.floatmode) iv=FloatInt((unsigned char *)&iv);
	sprintf(shows, "%010u", iv);
	while(1)
	{
		font_fillrect(x-2, y-2, x + 120, y + 12);
		if(ui_gv.floatmode){
			font_output(x+11*6, y, LANG_CINPFLOATDEC);
		}
		else font_output(x+11*6, y, LANG_CINPDEC);
		font_output(x, y, shows);
		font_output(x + pos * 6, y + 2, "_");
		font_refresh();
		switch(ctrl_waitmask(PSP_CTRL_LEFT | PSP_CTRL_RIGHT | PSP_CTRL_UP | PSP_CTRL_DOWN | PSP_CTRL_CIRCLE | PSP_CTRL_CROSS | PSP_CTRL_SQUARE| PSP_CTRL_TRIANGLE))
		{
		case PSP_CTRL_LEFT:
			pos --;
			if(pos < 0)
				pos = 9;
			break;
		case PSP_CTRL_RIGHT:
			pos ++;
			if(pos > 9)
				pos = 0;
			break;
		case PSP_CTRL_UP:
			if(max - iv < basex[pos])
				iv = max;
			else
				iv += basex[pos];
			sprintf(shows, "%010u", iv);
			break;
		case PSP_CTRL_DOWN:
			if(iv > basex[pos])
				iv -= basex[pos];
			else
				iv = 0;
			if(iv < min)
				iv = min;
			sprintf(shows, "%010u", iv);
			break;
		case PSP_CTRL_CIRCLE:
			ctrl_waitrelease();
			if(!config.swap)
			{
				if(ui_gv.floatmode){
					*ov = Dec2Float(iv);
				}
				else *ov = iv;
				return 0;
			}
			else
				return -1;
		case PSP_CTRL_CROSS:
			ctrl_waitrelease();
			if(config.swap)
			{
				if(ui_gv.floatmode){
					*ov = Dec2Float(iv);
				}
				else *ov = iv;
				return 0;
			}
			else
				return -1;
		case PSP_CTRL_TRIANGLE:
			ctrl_waitrelease();
			if(max>0xffff)
			ui_gv.floatmode = !ui_gv.floatmode;
			break;
		case PSP_CTRL_SQUARE:
			ctrl_waitrelease();
				if(ui_gv.floatmode){
					*ov = Dec2Float(iv);
				}
				else *ov = iv;
			return -2;
		}
	}
}

int ui_input_hex(int x, int y, unsigned int iv, unsigned int * ov, unsigned int min, unsigned int max)
{
	static const unsigned int baseh[8] = {0x10000000, 0x1000000, 0x100000, 0x10000, 0x1000, 0x100, 0x10, 0x1};
	int pos = 4;
	char shows[12];
	sprintf(shows, "0x%08X", iv);
	while(1)
	{
		font_fillrect(x-2, y-2, x + 120, y + 12);
		font_output(x+11*6, y, LANG_CINPHEX);
		font_output(x, y, shows);
		font_output(x + (pos + 2) * 6, y + 2, "_");
		//font_refresh();
		switch(ctrl_waitmask(PSP_CTRL_LEFT | PSP_CTRL_RIGHT | PSP_CTRL_UP | PSP_CTRL_DOWN | PSP_CTRL_CIRCLE | PSP_CTRL_CROSS | PSP_CTRL_SQUARE))
		{
		case PSP_CTRL_LEFT:
			pos --;
			if(pos < 0)
				pos = 7;
			break;
		case PSP_CTRL_RIGHT:
			pos ++;
			if(pos > 7)
				pos = 0;
			break;
		case PSP_CTRL_UP:
			if(max - iv < baseh[pos])
				iv = max;
			else
				iv += baseh[pos];
			sprintf(shows, "0x%08X", iv);
			break;
		case PSP_CTRL_DOWN:
			if(iv > baseh[pos])
				iv -= baseh[pos];
			else
				iv = 0;
			if(iv < min)
				iv = min;
			sprintf(shows, "0x%08X", iv);
			break;
		case PSP_CTRL_CIRCLE:
			ctrl_waitrelease();
			if(!config.swap)
			{
				*ov = iv;
				return 0;
			}
			else
				return -1;
		case PSP_CTRL_CROSS:
			ctrl_waitrelease();
			if(config.swap)
			{
				*ov = iv;
				return 0;
			}
			else
				return -1;
		case PSP_CTRL_SQUARE:
			ctrl_waitrelease();
			*ov = iv;
			return -2;
		}
	}
}

int ui_input(int x, int y, unsigned int iv, unsigned int * ov, unsigned int min, unsigned int max)
{
	while(1)
	{
		int i = (ui_gv.ui_hex_mode ? ui_input_hex(x, y, iv, ov, min, max) : ui_input_dec(x, y, iv, ov, min, max));
		if(i == -2)
		{
			ui_gv.ui_hex_mode = !ui_gv.ui_hex_mode;
			iv = *ov;
		}
		else
			return i;
	}
}

char * ui_get_gamename()
{
	return ui_gv.ui_gname;
}

int ui_input_string(int x, int y, char * s, int len)
{
	const char ickey[4][10] __attribute__(   (  aligned( 1 ))  ) = {
		{'1', '2', '3', '4', '5', '6', '7', '8', '9', '0'},
		{'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P'},
		{'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', '-'},
		{'Z', 'X', 'C', 'V', 'B', 'N', 'M', '*', '?', '+'},
	};
	const char ikey[4][10] __attribute__(   (  aligned( 1 ))  ) = {
		{'!', '"', '#', '$', '%', '&', 0x27, 0x28 , 0x29, '.'},
		{'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p'},
		{'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', '-'},
		{'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.', '_'},
	};
	char gb_mode = 0;
	char gb_string[8];
	int gb_string_pos=0;

	char *idx_buf = NULL;
	int fd = sceIoOpen(IME_DIR, PSP_O_RDONLY, 0777);
	if(fd>=0){
		idx_buf = malloc(11600);
		sceIoRead(fd, idx_buf, 11594);
		sceIoClose(fd);
	}

	char buffer[8];
	int idxpos=-1;
	unsigned short offset;
				memset(gb_string, ' ', 7);
				gb_string[7] = 0;

	char str[len + 1];
	mips_memcpy(str, s, len);
	str[len] = 0;
	int dx = 0, dy = 0, needrp = 1;
	int pos = len;
	while(pos > 0 && (str[pos - 1] == ' ' || str[pos-1] == 0)) -- pos;
	int cap = 1;
	font_fillrect(x, y + 64, x + 113, y + 112);
	//font_output(x, y + 64, LANG_CINP1);
	font_output(x, y + 76, LANG_CINP2);
	font_output(x, y + 88, LANG_CINP3);
	font_output(x, y + 100, LANG_CINP4);
	while(1)
	{
		if(needrp)
		{
			font_fillrect(x, y + 12, x + 113, y + 59);
			int i, j;
			char ss[2];
			ss[1] = 0;
			for(j = 0; j < 4; j ++)
				for(i = 0; i < 10; i ++)
				{
					ss[0] = cap ? ickey[j][i] : ikey[j][i];
					font_output(x + i * 12, y + 12 + j * 12, ss);
				}
			needrp = 0;
		}

		font_fillrect(x, y + 60, x + 113, y + 74);
			font_output(x, y + 64, LANG_CINP1[gb_mode]);
			font_fillrect(x+120, y, 379, y + 96);
		if(gb_mode){
			font_output(x + 156, y + 12, gb_string);
			font_output(x + 120, y +12, LANG_CINP7);
			font_output(x + 136, y +54, LANG_CINP8);
		}
		if(gb_mode && gb_string[0]!=' '){

			offset = (gb_string[0]-'A')*8;
			unsigned short offlen = *(unsigned short *)(idx_buf + offset + 4);
			offset = *(unsigned short *)(idx_buf + offset);
			int m;
			for(m=0;m<7;m++) buffer[m] = tolower(gb_string[m+1]);
			idxpos = -1;
			int buflen = strlen(buffer);

			do{
				buffer[buflen--]=0;
				m=0;
				while(idx_buf[offset+m] && m<offlen){
					if( strstr(idx_buf+offset+m, buffer) == (char *)(idx_buf+offset+m) ){
						idxpos = 0;
						offset = *(unsigned short *)(idx_buf+offset+m + 6) + 0xe68;
						break;
					}
					m += 8;
				}
			}while(idxpos<0 && buflen);
		}
			if(idxpos>=0){
				font_output(x+136, y + 26, LANG_CINP6);
				font_output(x+136, y + 40, LANG_CINP5);
				int m;
				int codelen = strlen(idx_buf+offset);
				font_fillrect(x+120, y+24, x+120 + 14, y+8*12);
				for(m=idxpos;m<12+idxpos;m+=2){
					font_outputn(x+120, y +26+ 6*(m-idxpos), &idx_buf[offset+m], 2);
					if(m==codelen) break;
				}
			}


		font_output(x + dx * 12, y + 14 + dy * 12, "_");//OSK
		font_fillrect(x, y-3, x + 6 * len + 5, y + 9+2);//こーど名
		font_output(x, y, str);
		font_output(x + pos * 6, y + 2, "_");//こーど名
		//font_refresh();
		switch(ctrl_waitmask(PSP_CTRL_LEFT | PSP_CTRL_RIGHT | PSP_CTRL_UP | PSP_CTRL_DOWN | PSP_CTRL_CIRCLE | PSP_CTRL_CROSS | PSP_CTRL_SQUARE | PSP_CTRL_TRIANGLE | PSP_CTRL_SELECT | PSP_CTRL_START | PSP_CTRL_LTRIGGER | PSP_CTRL_RTRIGGER))
		{
		#define IME_LINECLEAR 22 //20
		case PSP_CTRL_LEFT:
			font_fillrect(x + dx * 12, y + IME_LINECLEAR + dy * 12, x + 11 + dx * 12, y + 23 + dy * 12);
			dx --;
			if(dx < 0)
				dx = 9;
			break;
		case PSP_CTRL_RIGHT:
			font_fillrect(x + dx * 12, y + IME_LINECLEAR + dy * 12, x + 11 + dx * 12, y + 23 + dy * 12);
			dx ++;
			if(dx > 9)
				dx = 0;
			break;
		case PSP_CTRL_UP:
			font_fillrect(x + dx * 12, y + IME_LINECLEAR + dy * 12, x + 11 + dx * 12, y + 23 + dy * 12);
			dy --;
			if(dy < 0)
				dy = 3;
			break;
		case PSP_CTRL_DOWN:
			font_fillrect(x + dx * 12, y + IME_LINECLEAR + dy * 12, x + 11 + dx * 12, y + 23 + dy * 12);
			dy ++;
			if(dy > 3)
				dy = 0;
			break;
		case PSP_CTRL_CIRCLE:
			if(!config.swap)
			{
				if(gb_mode){
					//if(pos >= len-1)	break;
					if(gb_string_pos>6) break;
					if(ickey[dy][dx]>='A' && ickey[dy][dx]<'['){
						memmove(&gb_string[gb_string_pos + 1], &gb_string[gb_string_pos], 7 - gb_string_pos - 1);
						gb_string[gb_string_pos] = ickey[dy][dx];
						gb_string_pos ++;
					}
				}
				else{
					if(pos >= len)	break;
					memmove(&str[pos + 1], &str[pos], len - pos - 1);
					str[pos] = cap ? ickey[dy][dx] : ikey[dy][dx];
					pos ++;
				}
			}
			else
			{
				ctrl_waitrelease();
				font_fillrect(x, y + 10, 379, y + 107);
				sfree(idx_buf);
				return -1;
			}
			break;
		case PSP_CTRL_CROSS:
			if(config.swap)
			{
				if(gb_mode){
					//if(pos >= len-1)	break;
					if(gb_string_pos>6) break;
					if(ickey[dy][dx]>='A' && ickey[dy][dx]<'['){
						memmove(&gb_string[gb_string_pos + 1], &gb_string[gb_string_pos], 7 - gb_string_pos - 1);
						gb_string[gb_string_pos] = ickey[dy][dx];
						gb_string_pos ++;
					}
				}
				else{
					if(pos >= len)	break;
					memmove(&str[pos + 1], &str[pos], len - pos - 1);
					str[pos] = cap ? ickey[dy][dx] : ikey[dy][dx];
					pos ++;
				}
			}
			else
			{
				ctrl_waitrelease();
				font_fillrect(x, y + 10, 379, y + 112);
				sfree(idx_buf);
				return -1;
			}
			break;
		case PSP_CTRL_TRIANGLE:
			if(pos < len)
			{
				memmove(&str[pos + 1], &str[pos], len - pos - 1);
				str[pos] = ' ';
				pos ++;
			}
			break;
		case PSP_CTRL_SQUARE:
			if(pos > 0)
			{
				//GBK
				u8 code2=str[pos-1];
				u8 code=str[pos-2];
				if((pos > 1) && str[pos]==0 &&(((((code +0x7F)&0xFF) < 0x7E) && (code2>=0x40)))){
					if(pos < len)
						memmove(&str[pos - 2], &str[pos], len - pos);
				str[pos-1] = 0;
				pos -=2;
				}
				else{
				if(pos < len)
					memmove(&str[pos - 1], &str[pos], len - pos);
				str[pos] = 0;
				pos --;
				}
			}
			break;
		case PSP_CTRL_SELECT:
			if(gb_mode){
				if(pos >= len-1)	break;
				if(idxpos<0)	break;
				str[pos++]=idx_buf[offset+idxpos];
				str[pos++]=idx_buf[offset+idxpos+1];
				gb_string_pos = 0;
				memset(gb_string, ' ', 7);
				gb_string[7] = 0;
			}
			else{
				cap = !cap;
				needrp = 1;
			}
			break;
		case PSP_CTRL_START:
			ctrl_waitrelease();
			mips_memcpy(s, str, len);
			font_fillrect(x, y + 10, 379, y + 112);
			sfree(idx_buf);
			return strlen(str);
		case PSP_CTRL_LTRIGGER:
			if(idx_buf!=NULL){
				gb_mode = 1 - gb_mode;
/* 				gb_string_pos = 0;
				memset(gb_string, ' ', 7);
				gb_string[7] = 0; */
			}
			break;
		case PSP_CTRL_RTRIGGER:
				gb_string_pos = 0;
				memset(gb_string, ' ', 7);
				gb_string[7] = 0;
			break;
		case PSP_CTRL_RTRIGGER|PSP_CTRL_DOWN:
				if((idxpos>=0) && (idxpos<strlen(idx_buf+offset)-2)){
					idxpos+=2;
				}
			break;
		case PSP_CTRL_RTRIGGER|PSP_CTRL_UP:
				if(idxpos>=2) idxpos-=2;
			break;
		}
	}
}

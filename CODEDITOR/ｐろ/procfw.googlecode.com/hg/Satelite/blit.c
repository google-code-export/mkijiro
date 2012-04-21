/*
 * This file is part of PRO CFW.

 * PRO CFW is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.

 * PRO CFW is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with PRO CFW. If not, see <http://www.gnu.org/licenses/ .
 */

/*
	PSP VSH 24bpp text bliter
*/
#include "common.h"
//#include "blit.h"

//デフォルトフォントをかえる場合
//blit.h の#define yoko tate変更必須 //yoko x tate

//#include "../fonts/misaki.h"		//8x8
//#include "../fonts/misaki_hira.h"	//8x8
//#include "../fonts/akagi.h"		//8x11
//#include "../fonts/paw.h"			//8x16
//#include "../fonts/naga5x10a.h"	//5x10 仮名無
//#include "../fonts/naga5x10rk.h"	//5x10
//#include "../fonts/shnm6x12a.h"	//6x12 仮名無
//#include "../fonts/shnm6x12ab.h"	//6x12 仮名無
//#include "../fonts/shnm6x12r.h"	//6x12
//#include "../fonts/shnm6x12rb.h"	//6x12
//#include "../fonts/shnm7x14a.h"	//7x14 仮名無
//#include "../fonts/shnm7x14ab.h"	//7x14 仮名無
#include "../fonts/shnm7x14r.h"		//7x14
//#include "../fonts/shnm7x14rb.h"	//7x14
//#include "../fonts/shnm8x16a.h"	//8x16 仮名無
//#include "../fonts/shnm8x16ab.h"	//8x16 仮名無
//#include "../fonts/shnm8x16r.h"	//8x16
//#include "../fonts/shnm8x16rb.h"	//8x16
//#include "../fonts/naitouhorizon.h"		//8x16
//#include "../fonts/naitouhorizon_mk2.h"	//8x16


extern int yoko;
extern int tate;
int fakezenkaku=0;
int zenkaku=0;
int total=0;

//#define ALPHA_BLEND 1

//extern unsigned char msx[];
static unsigned char *g_cur_font = msx;

static SceUID g_memid = -1;

/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
int pwidth;
int pheight, bufferwidth, pixelformat;
unsigned int* vram32;

u32 fcolor = 0x00ffffff;
u32 bcolor = 0xff000000;

/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
static u32 adjust_alpha(u32 col)
{
	u32 alpha = col>>24;
	u8 mul;
	u32 c1,c2;

	if(alpha==0)    return col;
	if(alpha==0xff) return col;

	c1 = col & 0x00ff00ff;
	c2 = col & 0x0000ff00;
	mul = (u8)(255-alpha);
	c1 = ((c1*mul)>>8)&0x00ff00ff;
	c2 = ((c2*mul)>>8)&0x0000ff00;
	return (alpha<<24)|c1|c2;
}

/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
//int blit_setup(int sx,int sy,const char *msg,int fg_col,int bg_col)
int blit_setup(void)
{
	int unk;
	sceDisplayGetMode(&unk, &pwidth, &pheight);
	sceDisplayGetFrameBuf((void*)&vram32, &bufferwidth, &pixelformat, PSP_DISPLAY_SETBUF_NEXTFRAME);
	if( (bufferwidth==0) || (pixelformat!=3)) return -1;

	fcolor = 0x00ffffff;
	bcolor = 0xff000000;

	return 0;
}

/////////////////////////////////////////////////////////////////////////////
// blit text
/////////////////////////////////////////////////////////////////////////////
void blit_set_color(int fg_col,int bg_col)
{
	fcolor = fg_col;
	bcolor = bg_col;
}

//int z=0;

/////////////////////////////////////////////////////////////////////////////
// blit text
/////////////////////////////////////////////////////////////////////////////
int blit_string(int sx,int sy,const char *msg)
{
	int x,y,p,yyoko,ttate;
	int xx=0;
	int offset;
	u8 code,code2,font[1];
	u32 fg_col,bg_col;

	u32 col,c1,c2;
	u32 alpha;

	fg_col = adjust_alpha(fcolor);
	bg_col = adjust_alpha(bcolor);



//Kprintf("MODE %d WIDTH %d\n",pixelformat,bufferwidth);
	if( (bufferwidth==0) || (pixelformat!=3)) return -1;

for(x=0;msg[x] && x<(pwidth/8);)
{
		code = (u8)msg[x]; // no truncate now

//SJIS,EUC
//if( ((zenkaku==1)&& ( ((code>=0x81) && (code<=0x9F)) || ((code>=0xE0) && (code<=0xEA)) || ((code>=0xFA) && (code<=0xFC)))) ||
//((zenkaku==2) && ( ((code>=0xA1) && (code<=0xAD)) || ((code>=0xB0) && (code<=0xF4)) || ((code>=0xF9) && (code<=0xFC)) ) ) ){
if( ((zenkaku==1)&& ((u8)((code ^ 0x20) -0xA1) < (u8)0x3C)) ||
((zenkaku==2) && ((u8)(code - 0xA1) < (u8)0x5E)) ){
code2 = (u8)msg[x+1];

int big=code*256 + code2;
int fontpos=0;
int num[]={0,0};

for(p=0;p<total;p++){
memcpy(&num[0],&g_cur_font[p*4],2);
memcpy(&num[1],&g_cur_font[(p*4) +2],2);

if((big>=num[0]) && (big<=num[1])){
fontpos += (big-num[0]);
break;
}
else{
fontpos += (num[1]-num[0]+1);
}
}


if(p==total){
}
else{

		for(y=0;y< tate;y++)
		{
			offset = (sy+y)*bufferwidth+sx+xx;
			//if(y==tate-1){
			//font[0]=0x00;
			//}
			//else{
			memcpy(&font[0],&g_cur_font[(((fontpos*tate)+y)*2)+(4*total)],2);


      /*if(z<100){
	char buffer[64];
	sprintf(buffer,"n0 %x,n1 %x,big %x,fp %x,p %x,ofs %x\n",num[0],num[1],big,fontpos,p,(((fontpos*tate)+y)*2)+(4*total));
	int fd=sceIoOpen("ms0:/log", PSP_O_CREAT | PSP_O_WRONLY | PSP_O_APPEND, 0777);
	sceIoWrite(fd, buffer,strlen(buffer));
	sceIoClose(fd);
	z++;
	}*/

			//}

			for(p=0;p< yoko;p++,offset++)
			{
				if((p&8)==0){
					col = (font[0] & 0x80) ? fg_col : bg_col;}
				else{
					col = (font[1] & 0x80) ? fg_col : bg_col;}

				alpha = col>>24;
				if(alpha==0) vram32[offset] = col;
				else if(alpha!=0xff)
				{
					c2 = vram32[offset];
					c1 = c2 & 0x00ff00ff;
					c2 = c2 & 0x0000ff00;
					c1 = ((c1*alpha)>>8)&0x00ff00ff;
					c2 = ((c2*alpha)>>8)&0x0000ff00;
					vram32[offset] = (col&0xffffff) + c1 + c2;
				}

				if((p&8)==0){
					font[0] <<= 1;}
				else{
					font[1] <<= 1;}

			}
		}
}

x+=2;
xx+=yoko;
}
else{
yyoko=yoko;
ttate=tate;
if(zenkaku!=0){
yyoko=def_yoko;
ttate=def_tate;
}
if((zenkaku==2) && (code==0x8E)){
code = (u8)msg[x+1];
x++;
}

		for(y=0;y< ttate;y++)
		{
			offset = (sy+y)*bufferwidth + sx+xx;
			if(y==tate-1){
			font[0]=0x00;
			}
			else{
				if(fakezenkaku!=0){
				//横8超フォント 配列2倍
				memcpy(&font[0],&g_cur_font[(code*ttate+y)<<1],2);
				}
				else{
				font[0]=g_cur_font[code*ttate+y];
				}
				if(zenkaku!=0){
				font[0]=msx[code*ttate+y];
				}
			}

			for(p=0;p< yyoko;p++,offset++)
			{
				if((p&8)==0){
					col = (font[0] & 0x80) ? fg_col : bg_col;}
				else{
					col = (font[1] & 0x80) ? fg_col : bg_col;}

				alpha = col>>24;
				if(alpha==0) vram32[offset] = col;
				else if(alpha!=0xff)
				{
					c2 = vram32[offset];
					c1 = c2 & 0x00ff00ff;
					c2 = c2 & 0x0000ff00;
					c1 = ((c1*alpha)>>8)&0x00ff00ff;
					c2 = ((c2*alpha)>>8)&0x0000ff00;
					vram32[offset] = (col&0xffffff) + c1 + c2;
				}

				if((p&8)==0){
					font[0] <<= 1;}
				else{
					font[1] <<= 1;}

			}
		}
x++;
xx+=yyoko;
}

}
	return x;
}

int blit_string_ctr(int sy,const char *msg)
{
	int sx = 480/2;

#ifdef CONFIG_639
	if(psp_fw_version == FW_639)
		sx = 480/2-scePaf_strlen(msg)*(8/2);
#endif

#ifdef CONFIG_635
	if(psp_fw_version == FW_635)
		sx = 480/2-scePaf_strlen(msg)*(8/2);
#endif

#ifdef CONFIG_620
	if(psp_fw_version == FW_620)
		sx = 480/2-scePaf_strlen_620(msg)*(8/2);
#endif

#ifdef CONFIG_660
	if(psp_fw_version == FW_660)
		sx = 480/2-scePaf_strlen_660(msg)*(8/2);
#endif

	return blit_string(sx,sy,msg);
}


int load_external_font(const char *file)
{
	SceUID fd;
	size_t f_si;
	int ret;
	void *buf;

	fd = sceIoOpen(file, PSP_O_RDONLY, 0777);

	if(fd < 0) {
		return fd;
	}

	sceIoLseek(fd, 0, PSP_SEEK_END);
	f_si = sceIoLseek(fd, 0, PSP_SEEK_CUR);
	sceIoLseek(fd, 0, PSP_SEEK_SET);

	g_memid = sceKernelAllocPartitionMemory(2, "proDebugScreenFontBuffer", PSP_SMEM_High, f_si, NULL);

	if(g_memid < 0) {
		sceIoClose(fd);
		return g_memid;
	}

	buf = sceKernelGetBlockHeadAddr(g_memid);

	if(buf == NULL) {
		sceKernelFreePartitionMemory(g_memid);
		sceIoClose(fd);
		return -2;
	}

	ret = sceIoRead(fd, buf, f_si);


	if(ret != f_si) {
		sceKernelFreePartitionMemory(g_memid);
		sceIoClose(fd);
		return -3;
	}


	sceIoClose(fd);
	g_cur_font = buf;

	if((*(unsigned int *)(&g_cur_font[0]))==0x544E4F46){
	char k=1;
	memcpy(&yoko,&g_cur_font[14],1);
	memcpy(&tate,&g_cur_font[15],1);
	memcpy(&zenkaku,&g_cur_font[16],1);
	memcpy(&total,&g_cur_font[17],1);
	if(zenkaku==0){k=0;}
	memmove(&g_cur_font[0],&g_cur_font[17+k],f_si-17);
	if(yoko>8){fakezenkaku=1;}
	else{
	fakezenkaku=0;
	}}
	else{
	zenkaku=0;fakezenkaku=0;yoko=8;tate=8;
	}

	return 0;
}

void release_font(void)
{
	if(g_memid >= 0) {
		sceKernelFreePartitionMemory(g_memid);
		g_memid = -1;
	}

	g_cur_font = msx;
}

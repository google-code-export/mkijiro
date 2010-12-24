/*
	PSP 24bpp text bliter (from devhook 043sdk)
*/
#include <psptypes.h>
#include <pspdisplay.h>

#ifdef _FONT_acorn  
#include "../fonts/acorn.h"
#elif _FONT_lucidia 
#include "../fonts/lucidia.h"
#elif _FONT_originaldebug 
#include "../fonts/originaldebug.h"
#elif _FONT_misaki
#include "../fonts/misaki.h"
#elif _FONT_misaki_hira
#include "../fonts/misaki_hira.h"
#elif _FONT_perl 
#include "../fonts/perl.h"
#elif _FONT_sparta 
#include "../fonts/sparta.h"
#elif _FONT_linux 
#include "../fonts/linux.h"
#elif _FONT_debug 
#include "../fonts/originaldebug.h"
/*
#elif _FONT_font4x6 
#include "../fonts/font4x6.h" 
*/
#elif _FONT_font8x8 
#include "../fonts/font8x8.h"
	
#endif

extern int enable_blit;

/////////////////////////////////////////////////////////////////////////////
// blit text
/////////////////////////////////////////////////////////////////////////////
int blit_string(int sx,int sy,const char *msg,int fg_col,int bg_col){
	int x,y,p;
	int offset;
	char code;
	unsigned char font;
	int pwidth, pheight, bufferwidth, pixelformat, unk;
	unsigned int* vram;

   	sceDisplayGetMode(&unk, &pwidth, &pheight);
   	sceDisplayGetFrameBuf((void*)&vram, &bufferwidth, &pixelformat, &unk);

   	if( (bufferwidth==0) || (pixelformat!=3)) return -1;

   	for(x=0;msg[x] && x<(pwidth/8);x++){
   		code = msg[x] & 0xFF; // 7bit ANK
   		for(y=0;y<7;y++)
   		{
   			offset = (sy+y)*bufferwidth + (sx+x)*8;
   			font = msx[ code*8 + y ];
   			for(p=0;p<8;p++){
				vram[offset] = (font & 0x80) ? fg_col : bg_col;
   				font <<= 1;
   				offset++;
   			}
   		}
   	}

	return x;
}

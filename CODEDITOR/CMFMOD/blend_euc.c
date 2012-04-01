#include <stdio.h>
#include <string.h>
#include <pspkernel.h>
#include <psptypes.h>
#include <pspdisplay.h>
#include <pspsysmem_kernel.h>
#include "font.h"
#include "lang_zh_euc.h"
#include "conf.h"
#include "allocmem.h"
#include "rgb_color.h"
#include "blend.h"
#include "smsutils.h"

static void *bg_buf = NULL;
int BACKUP_X1,BACKUP_X2;

inline void* BGbuf_get()
{
	return bg_buf;
}

int initBG()
{
	t_bgctx *ctx = &bgctx;
	int backup_size,bg_flag,bg_buf_width;
	void (*color2rgb)(u32 color, u8 *r, u8 *g, u8*b);
	u32 (*rgb2color)(u32 r, u32 g, u32 b);
	switch(ctx->pixelformat)
	{
	case PSP_DISPLAY_PIXEL_FORMAT_565:
		color2rgb = color2rgb565;
		rgb2color = rgb2color565;
		bg_flag = 2;
		break;
	case PSP_DISPLAY_PIXEL_FORMAT_5551:
		color2rgb = color2rgb5551;
		rgb2color = rgb2color5551;
		bg_flag = 2;
		break;
	case PSP_DISPLAY_PIXEL_FORMAT_4444:
		color2rgb = color2rgb4444;
		rgb2color = rgb2color4444;
		bg_flag = 2;
		break;
	case PSP_DISPLAY_PIXEL_FORMAT_8888:
		color2rgb = color2rgb8888;
		rgb2color = rgb2color8888;
		bg_flag = 4;
		break;
	}
	
	if(config.blendmore){
		BACKUP_X1=4;
		BACKUP_X2=468;
	}
	else{
		BACKUP_X1=96;
		BACKUP_X2=384;	
	}
	backup_size = (BACKUP_X2-BACKUP_X1)*(BACKUP_Y2-BACKUP_Y1) * bg_flag ;
	bg_buf_width = (BACKUP_X2-BACKUP_X1)*bg_flag;
	
	bg_buf = smalloc(backup_size, 0xF000);
	if(bg_buf==NULL) return 1;
	
	int i;
	for(i=BACKUP_Y1;i<BACKUP_Y2;i++){
		mips_memcpy(bg_buf+(i-BACKUP_Y1)*bg_buf_width,
		(void *)((unsigned int)ctx->vram+(i*ctx->bufferwidth+BACKUP_X1)*bg_flag),
		bg_buf_width);
	}
	
	u8 r, g, b;
	u16 * vram16 = (u16 *)bg_buf;
	u32 * vram32 = (u32 *)bg_buf;
	int x,y;
	for(y=0;y<(BACKUP_Y2-BACKUP_Y1);y++){
		int tmp = y * (BACKUP_X2-BACKUP_X1);
		for(x=tmp;x<tmp + (BACKUP_X2-BACKUP_X1);x++){
			switch (ctx->pixelformat)
			{
				case PSP_DISPLAY_PIXEL_FORMAT_565:
				case PSP_DISPLAY_PIXEL_FORMAT_5551:
				case PSP_DISPLAY_PIXEL_FORMAT_4444:
					color2rgb(vram16[x],&r,&g,&b);
					break;
				case PSP_DISPLAY_PIXEL_FORMAT_8888:		 			
					color2rgb(vram32[x],&r,&g,&b);
					break;
			}
			r = (ctx->bg_a * ctx->bg_r) / 256 + (255-ctx->bg_a) * r / 256;
			g = (ctx->bg_a * ctx->bg_g) / 256 + (255-ctx->bg_a) * g / 256;
			b = (ctx->bg_a * ctx->bg_b) / 256 + (255-ctx->bg_a) * b / 256;
			switch (ctx->pixelformat)
			{
				case PSP_DISPLAY_PIXEL_FORMAT_565:
				case PSP_DISPLAY_PIXEL_FORMAT_5551:
				case PSP_DISPLAY_PIXEL_FORMAT_4444:
					vram16[x] = rgb2color(r,g,b);
					break;
				case PSP_DISPLAY_PIXEL_FORMAT_8888:
					vram32[x] = rgb2color(r,g,b);
					break;
			}
		}
	}
	
	return 0;
}

void freeBG(void)
{
	sfree(bg_buf);
	bg_buf = NULL;
}


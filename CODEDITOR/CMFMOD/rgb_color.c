#include <stdio.h>
#include <psptypes.h>
#include <pspkernel.h>
#include <pspdisplay.h>
#include "rgb_color.h"

extern u32 rgb2color565(u32 r, u32 g, u32 b)
{
	u32 rgb = (r >> 3) | ((g >> 2) << 5) | ((b >> 3) << 11);
	return rgb;
}

extern u32 rgb2color5551(u32 r, u32 g, u32 b)
{
	u32 rgb = (r >> 3) | ((g >> 3) << 5) | ((b >> 3) << 10);
	return rgb;
}

extern u32 rgb2color4444(u32 r, u32 g, u32 b)
{
	u32 rgb = (r >> 4) | ((g >> 4) << 4) | ((b >> 4) << 8);
	return rgb;
}

extern u32 rgb2color8888(u32 r, u32 g, u32 b)
{
	u32 rgb = r | (g << 8) | (b << 16);
	return rgb;
}

extern void color2rgb565(u32 color, u8 *r, u8 *g, u8*b)
{
 			*r = (color & 0x1f) << 3; 
			*g = ((color >> 5) & 0x3f) << 2;
			*b = ((color >> 11) & 0x1f) << 3;
}

extern void color2rgb5551(u32 color, u8 *r, u8 *g, u8*b)
{
 			*r = (color & 0x1f) << 3; 
			*g = ((color >> 5) & 0x1f) << 3;
			*b = ((color >> 10) & 0x1f) << 3;
}

extern void color2rgb4444(u32 color, u8 *r, u8 *g, u8*b)
{
 			*r = (color & 0xf) << 4; 
			*g = ((color >> 4) & 0xf) << 4;
			*b = ((color >> 8) & 0xf) << 4;
}

extern void color2rgb8888(u32 color, u8 *r, u8 *g, u8*b)
{
			*r = color; 
			*g = (color >> 8);
			*b = (color >> 16);
}


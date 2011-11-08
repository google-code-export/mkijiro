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

#ifndef _FONT_H
#define _FONT_H

typedef struct hzfont{
	unsigned short offset ;
	unsigned char dat[18] ;
}__attribute__(  ( packed )  ) hzfont;

typedef struct{
	void *vram;
	int bufferwidth;
	int pixelformat;
	u32 bg_r;
	u32 bg_g;
	u32 bg_b;
	u32 bg_a;
	u32 rgb;
	u32 font_rgb;
}__attribute__(  ( packed )  ) t_bgctx;

extern int IsHzcode(int x, const char *msg);
extern u32 bg_r,bg_g,bg_b,bg_a;
extern void color_init();
extern void (*font_line)(int x1, int y1, int x2, int y2);
extern int (*font_outputn)(int sx, int sy, const char *msg, int count);
//#define font_output(sx, sy, msg) font_outputn(sx, sy, msg, 0x7FFFFFFF)
extern int font_output(int sx, int sy, const char *msg);
extern void (*font_fillrect)(int x1, int y1, int x2, int y2);
extern int font_init();
extern void font_refresh();
extern void font_switch_refresh();
extern void font_switch_back();
extern void font_get_vram();

extern t_bgctx bgctx;
extern void font_switch_vram();

#endif

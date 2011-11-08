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

#ifndef _UI_H
#define _UI_H

extern void debug_memui();
extern int gameplaying();		//1--psp;2--psx
extern void ui_init();
extern void ui_cls();
extern int ui_menu(int x, int y, const char ** item, int count, int pagecount, int sidx, int (*cbfunc)(unsigned int, int *, int *));
extern int ui_input_dec(int x, int y, unsigned int iv, unsigned int * ov, unsigned int min, unsigned int max);
extern int ui_input_hex(int x, int y, unsigned int iv, unsigned int * ov, unsigned int min, unsigned int max);
extern int ui_input(int x, int y, unsigned int iv, unsigned int * ov, unsigned int min, unsigned int max);
extern char * ui_get_gamename();
extern int ui_input_string(int x, int y, char * str, int len);

/* extern int ui_hex_mode;
extern int floatmode; */
#endif

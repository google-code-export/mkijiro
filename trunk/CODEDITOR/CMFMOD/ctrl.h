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

#ifndef _CTRL_H
#define _CTRL_H

#define PSP_ANA_UP		0x10000000
#define PSP_ANA_RIGHT	0x20000000
#define PSP_ANA_DOWN	0x40000000
#define PSP_ANA_LEFT	0x80000000

typedef struct _ctrl_ctx{
	unsigned int last_btn;
	unsigned int last_tick;
	unsigned int repeat_flag;
}t_ctrl_ctx;

extern t_ctrl_ctx ctrl_ctx;

extern unsigned int ctrl_read();
extern void ctrl_waitrelease();
extern unsigned int ctrl_waitany();
extern unsigned int ctrl_waitkey(unsigned int keymask);
extern unsigned int ctrl_waitmask(unsigned int keymask);
extern unsigned int ctrl_waitchange();
extern unsigned int ctrl_input();

extern unsigned int ctrl_a2d(SceCtrlData *ctl);
extern unsigned int ctrl_dela2d(SceCtrlData *ctl, unsigned int);
extern unsigned int ctrl_read_btn(SceCtrlData *ctl);
#endif

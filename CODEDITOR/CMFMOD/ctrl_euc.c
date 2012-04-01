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

#include <pspkernel.h>
#include <pspdisplay.h>
#include <pspctrl.h>
#include "mem.h"
#include "font.h"
#include "lang_zh_euc.h"
#include "ctrl.h"

#define BUTTON_MASK 0xF0F0FFFF
#define ANALOG_MASK 0xF0000000

#define CTRL_REPEAT_TIME 0x40000
#define CTRL_REPEAT_INTERVAL 0x12000

t_ctrl_ctx ctrl_ctx __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );

typedef struct _aganalog{
	u16 x;
	u16 y;
}__attribute__(  ( packed )  ) t_analogxy;

static t_analogxy analogxy;

unsigned int ctrl_a2d(SceCtrlData *ctl)
{
	analogxy.x = ctl->Lx;
	analogxy.y = ctl->Ly;
	unsigned int digtal_btn = 0;
		if((ctl->Lx<0x30)){
			digtal_btn = PSP_ANA_LEFT;
		}
		else
		if((ctl->Lx>=0xd0)){
			digtal_btn = PSP_ANA_RIGHT;
		}
		
		if((ctl->Ly<0x30)){
			digtal_btn |= PSP_ANA_UP;
		}
		else
		if((ctl->Ly>=0xd0)){
			digtal_btn |= PSP_ANA_DOWN;
		}
	digtal_btn = (ctl->Buttons & 0x00F0FFFF) | digtal_btn;
	return digtal_btn;
}

unsigned int ctrl_dela2d(SceCtrlData *ctl, unsigned int key)
{
	u32 btn;
	btn = key;
	btn = btn>>24;
		if((btn & PSP_CTRL_LEFT)){
			if(analogxy.x>=0x30)
				ctl->Lx=0;
			else
				ctl->Lx=analogxy.x;
		}
		else
		if((btn & PSP_CTRL_RIGHT)){
			if(analogxy.x<0xd0)
				ctl->Lx=255;
			else
				ctl->Lx=analogxy.x;
		}
		
		if((btn & PSP_CTRL_UP)){
			if(analogxy.y>=0x30)
				ctl->Ly=0;
			else
				ctl->Ly=analogxy.y;
		}
		else
		if((btn & PSP_CTRL_DOWN)){
			if(analogxy.y<0xd0)
				ctl->Ly=255;
			else
				ctl->Ly=analogxy.y;
		}
		key &= ~ANALOG_MASK;
		ctl->Buttons &= 0xFF0F0000;		
		ctl->Buttons |= key;
	return ctl->Buttons;
}

unsigned int ctrl_read_btn(SceCtrlData *ctl)
{
	unsigned int btn = ctrl_a2d(ctl);
	if (btn == ctrl_ctx.last_btn)
	{
		if (ctl->TimeStamp - ctrl_ctx.last_tick < (ctrl_ctx.repeat_flag ? CTRL_REPEAT_INTERVAL : CTRL_REPEAT_TIME)) return 0;
		ctrl_ctx.repeat_flag = 1;
		ctrl_ctx.last_tick = ctl->TimeStamp;
		return ctrl_ctx.last_btn;
	}
	ctrl_ctx.repeat_flag = 0;
	ctrl_ctx.last_tick = ctl->TimeStamp;
	ctrl_ctx.last_btn  = btn;
	return ctrl_ctx.last_btn;
}

extern unsigned int ctrl_read()
{
	SceCtrlData ctl;
	sceCtrlPeekBufferPositive(&ctl, 1);
	return ctrl_read_btn(&ctl);
}

extern void ctrl_waitrelease()
{
	SceCtrlData ctl;
	unsigned int btn;
	do {
		sceCtrlPeekBufferPositive(&ctl, 1);
		btn = ctrl_a2d(&ctl);
		sceKernelDelayThread(20000);
	} while ((btn & BUTTON_MASK) != 0);
	ctrl_ctx.last_tick = ctl.TimeStamp;
	ctrl_ctx.last_btn  = btn;
}

extern unsigned int ctrl_waitany()
{
	unsigned int key;
	while((key = (ctrl_read() & BUTTON_MASK)) == 0)
	{
		sceKernelDelayThread(20000);
	}
	return key;
}

extern unsigned int ctrl_waitchange(unsigned int key)
{
	unsigned int key2 = key;
	SceCtrlData ctl;
	while((key2 & key) == key)
	{
		sceKernelDelayThread(20000);
		key = key2;
		sceCtrlPeekBufferPositive(&ctl, 1);
		key2 = ctrl_a2d(&ctl) & BUTTON_MASK;
	}
	ctrl_waitrelease();
	return key;
}

extern unsigned int ctrl_waitkey(unsigned int keyw)
{
	unsigned int key;
	while((key = (ctrl_read() & keyw)) != keyw)
	{
		sceKernelDelayThread(20000);
	}
	return key;
}

extern unsigned int ctrl_waitmask(unsigned int keymask)
{
	unsigned int key;
	while((key = (ctrl_read() & keymask)) == 0)
	{
		sceKernelDelayThread(20000);
	}
	return key;
}

/* extern unsigned int ctrl_waittime(unsigned int t)
{
	int i = 0, m = t * 50;
	unsigned int key;
	while((key = (ctrl_read() & 0xF0FFFF)) == 0 && i < m)
	{
		sceKernelDelayThread(20000);
		++ i;
	}
	return key;
} */

extern unsigned int ctrl_input()
{
	font_output(110, 12*13+56, LANG_PRESSSKEY);
	ctrl_waitrelease();
	SceCtrlData ctl;
	unsigned int key, key2;
	do {
		sceCtrlPeekBufferPositive(&ctl, 1);
		key = ctrl_a2d(&ctl) & BUTTON_MASK;
	} while(key == 0);
	return ctrl_waitchange(key);
}

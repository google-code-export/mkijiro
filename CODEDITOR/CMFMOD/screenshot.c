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

#include <stdio.h>
#include <pspkernel.h>
#include <string.h>
#include <pspdisplay.h>
#include "screenshot.h"
#include "rgb_color.h"
#include "allocmem.h"
#include "common.h"
#include "conf.h"

int prx_snapjpg(int quality);
int prx_snappng();

char CAPTURE_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/PICTURE/SCREENSHOT/";
static char PICT_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/PICTURE/";

static char SNAPPRX [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/CheatMaster/prx/snapjpg.prx";
static char SNAPPNGPRX [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/CheatMaster/prx/snappng.prx";

extern void screenshot_init()
{
	sceIoMkdir(PICT_DIR, 0777);
	sceIoMkdir(CAPTURE_DIR, 0777);
}

// config.ssformat == 0 jpg; -1 bmp; 1 png;

void screenshot()
{
	char *s;
	s = config.ssformat<=0? SNAPPRX:SNAPPNGPRX;
	if(img_Loadprx(s)!=0) return;
	
	if(config.ssformat==0)
		prx_snapjpg(config.jpg_quality);
	else if(config.ssformat<0)
		prx_snapjpg(0);
	else prx_snappng();

	img_Unloadprx();
}

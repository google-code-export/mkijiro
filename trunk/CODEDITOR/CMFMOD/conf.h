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

#ifndef _CONF_H
#define _CONF_H

typedef struct _conf {
	unsigned int skey;	// Shortcut key
	int swap;			// Swap CIRCLE/CROSS key
	int autoload;		// Automically load saved table (with game id compared)
	int lockspd;		// Speed of locking, aka interval
	int enabless;		// If screenshot is enabled
	int ssskey;			// Screenshot shortcut key
	int ssformat;		//bmp or jpg
	unsigned int bg_color;
	unsigned int font_color;
	unsigned int txtrowbytes;
	unsigned int suspend_skey;
	unsigned int standby_skey;
	int autoloadcmf;
	int autoloadset;
	int jpg_quality;		//jpg
	unsigned int savekey;
	unsigned int loadkey;
	int sl_autopoweroff;
	int blendmore;
} t_conf, * p_conf;

extern t_conf config;


extern void conf_get_keyname(unsigned int key, char * res);
extern void conf_load();
extern void conf_save();

#endif

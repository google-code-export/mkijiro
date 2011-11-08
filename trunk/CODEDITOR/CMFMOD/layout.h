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

#ifndef _LAYOUT_H
#define _LAYOUT_H

#define	MAX_KEYLIST		10
#define MAX_KEYSET		36

typedef struct tagctrl{
	u32 btn;
	u8	x;
	u8	y;
	u16 stamp;			//<<
} __attribute__((packed)) t_ctrlkey;

typedef struct taglist{
	char idx;
	u8 count;
	u16 reversekey;		// reverse shortkey
	u32 key;		//shortkey
	t_ctrlkey list[MAX_KEYSET];
	u32 lastkey_stamp;
	u32 lastkey;
} __attribute__((packed)) t_keylist_table,*p_keylist_table;


extern t_keylist_table g_keylist[MAX_KEYLIST];

typedef struct t_keyset{
	u32 turbokey;
	u32 autokey;
	u16 dac[2];
	u8 turbo_key_interval[12];
	u32 keymap_table[16];
	u32 keymap_skey[16];
	u32 turbo_skey[12];
	u32 stick_table[16];
	u32 stick_skey[16];
}__attribute__(  ( packed )  ) t_keyset;


extern const unsigned int turbo_key_tab[];
extern char *turbo_key_symbol[];
extern t_keyset keyset;

extern int layout_menu();
extern int layout_init();
extern int g_bright;

extern int layout_autoload_dir(char *gname, char *dir, char *fn);

#endif

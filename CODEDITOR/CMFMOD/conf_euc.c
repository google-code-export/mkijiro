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
#include <pspctrl.h>
#include <string.h>
#include "conf.h"
#include "lang_zh_euc.h"
#include "layout.h"
#include "ctrl.h"
#include "smsutils.h"

char CONFIG_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="ms0:/CheatMaster/config";

t_conf config __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );

#ifdef ENABLE_CLANG
extern void conf_get_keyname(unsigned int key, char * res)
{
	res[0] = 0;
	int i;
	for(i=0;i<20;i++){
		if((key & turbo_key_tab[i]) > 0)
		{
			if(res[0] != 0)
				strcat(res, "+");
			strcat(res, turbo_key_symbol[i]);
		}
	}
}
#else

#endif

extern void conf_load()
{
	//memset(&config, 0, sizeof(config));
	config.skey = PSP_CTRL_NOTE;
	config.ssskey = PSP_CTRL_VOLDOWN + PSP_CTRL_SELECT;
	config.bg_color = 0xb0c0c0c0;
	config.txtrowbytes = 54;
	config.jpg_quality = 90;
	config.suspend_skey = PSP_CTRL_RIGHT|PSP_CTRL_LEFT |PSP_ANA_RIGHT|PSP_CTRL_LTRIGGER;
	config.standby_skey = PSP_CTRL_RIGHT|PSP_CTRL_LEFT |PSP_ANA_RIGHT|PSP_CTRL_LTRIGGER;
	config.savekey = PSP_CTRL_RIGHT|PSP_CTRL_LEFT |PSP_ANA_RIGHT|PSP_CTRL_LTRIGGER;
	config.loadkey = PSP_CTRL_RIGHT|PSP_CTRL_LEFT |PSP_ANA_RIGHT|PSP_CTRL_LTRIGGER;
	
	//memset(&keyset, 0, sizeof(keyset));
	memset(keyset.turbo_key_interval, 2, 12);
	int i;
 	for(i=0;i<16;i++){
		//keyset.keymap_skey[i] = turbo_key_tab[i];
		//keyset.keymap_table[i] = turbo_key_tab[i];
		keyset.stick_table[i] = turbo_key_tab[i];
		keyset.stick_skey[i] = PSP_CTRL_UP|PSP_CTRL_DOWN|PSP_ANA_RIGHT|PSP_ANA_LEFT;
	}
	
	for(i=0;i<12;i++){
		keyset.turbo_skey[i] = PSP_CTRL_UP|PSP_CTRL_DOWN|PSP_ANA_RIGHT|PSP_ANA_LEFT;
	}	

	int j;
	for(i=0;i<MAX_KEYLIST;i++)
	{
		//memset((u8 *)&g_keylist[i], 0, sizeof(t_keylist_table));
		g_keylist[i].idx = -1;
		g_keylist[i].reversekey = PSP_CTRL_RTRIGGER;
		//g_keylist[i].count = 0;
		//g_keylist[i].lastkey_stamp = 0;
		for(j=0;j<MAX_KEYSET;j++)
		{			
			//g_keylist[i].list[j].btn = 0;
			//g_keylist[i].list[j].x = 127;
			//g_keylist[i].list[j].y = 127;
			g_keylist[i].list[j].stamp = 7;
		}
	}
	
	int dl = sceIoDopen(TAB_DIR);
	if(dl < 0)
		TAB_DIR[16]=0;
	else
		sceIoDclose(dl);
		
	int fd = sceIoOpen(CONFIG_DIR, PSP_O_RDONLY, 0777);
	if(fd >= 0)
	{
		sceIoRead(fd, &config, sizeof(config));
		sceIoClose(fd);
	}
}

extern void conf_save()
{
	int fd = sceIoOpen(CONFIG_DIR, PSP_O_WRONLY | PSP_O_CREAT | PSP_O_TRUNC, 0777);
	if(fd >= 0)
	{
		sceIoWrite(fd, &config, sizeof(config));
		sceIoClose(fd);
	}
}

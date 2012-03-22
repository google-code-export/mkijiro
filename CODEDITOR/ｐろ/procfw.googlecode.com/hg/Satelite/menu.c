/*
 * This file is part of PRO CFW.

 * PRO CFW is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.

 * PRO CFW is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with PRO CFW. If not, see <http://www.gnu.org/licenses/ .
 */

/*
	PSP VSH MENU controll
	based Booster's vshex
*/

#include "common.h"
#include <psputility.h>
#include "blit.h"
int yoko=def_yoko;
int tate=def_tate;

const char **g_messages = g_messages_en;

void change_clock(int dir, int a);

extern int pwidth;
extern char umd_path[72];
extern SEConfig cnf;

char freq_buf[3+3+2] = "";
char freq2_buf[3+3+2] = "";
char device_buf[13] = "";
char umdvideo_path[256] = "";

#define TMENU_MAX 11

enum{
	TMENU_XMB_CLOCK,
	TMENU_GAME_CLOCK,
	TMENU_USB_DEVICE,
	TMENU_UMD_MODE,
	TMENU_UMD_VIDEO,
//	TMENU_XMB_PLUGINS,
//	TMENU_GAME_PLUGINS,
//	TMENU_POPS_PLUGINS,
	TMENU_RECOVERY_MENU,
//	TMENU_USB_CHARGE,
//	TMENU_HIDE_MAC,
//	TMENU_SKIP_GAMEBOOT,
//	TMENU_HIDE_PIC,
//	TMENU_FLASH_PROT,
//	TMENU_FAKE_REGION,
	TMENU_SHUTDOWN_DEVICE,
	TMENU_SUSPEND_DEVICE,
	TMENU_RESET_DEVICE,
	TMENU_RESET_VSH,
	TMENU_EXIT
};

int item_fcolor[TMENU_MAX];
const char *item_str[TMENU_MAX];

static int menu_sel = TMENU_XMB_CLOCK;


#define str_center (pwidth-strlen(msg)*def_yoko+1)>>1

const int xyPoint[] ={0x98, 0x30, 0xC0, 0xA0, 0x70, 0x08, 0x0E, 0xA8};//data243C=
const int xyPoint2[] ={0xB0, 0x30, 0xD8, 0xB8, 0x88, 0x08, 0x11, 0xC0};//data2458=

extern int zenkaku;
extern int total;

int menu_draw(void)
{
	u32 fc,bc;
	const char *msg;
	int max_menu, cur_menu;
	const int *pointer;
	int xPointer;
	int zure=0;
	
	// check & setup video mode
	if( blit_setup() < 0) return -1;

	if(pwidth==720) {
		pointer = xyPoint;
	} else {
		pointer = xyPoint2;
	}
	
	zure=(yoko-8);

	// show menu list
	blit_set_color(0xffffff,0x8000ff00);
	
		msg = g_messages[MSG_PRO_VSH_MENU];
		
	//�o�q�n�@�u�r�g�@���j���[�̕\���ʒu
	//blit_string(pointer[0], pointer[1], g_messages[MSG_PRO_VSH_MENU]);
	blit_string(str_center, pointer[1], msg);

	for(max_menu=0;max_menu<TMENU_MAX;max_menu++) {
		fc = 0xffffff;
		bc = (max_menu==menu_sel) ? 0xff8080 : 0xc00000ff;
		blit_set_color(fc,bc);

		msg = g_messages[MSG_CPU_CLOCK_XMB + max_menu];

		if(msg) {
			switch(max_menu) {
				case TMENU_EXIT:
					xPointer =str_center;//pointer[2];
					break;
				case TMENU_RESET_DEVICE:
					//if (cur_language == PSP_SYSTEMPARAM_LANGUAGE_GERMAN) {
					//	xPointer = pointer[3] - 2 * 8 - 1;
					//} else {
						xPointer =str_center;//pointer[3];
					//}
					
					break;
				case TMENU_RESET_VSH:
					//if (cur_language == PSP_SYSTEMPARAM_LANGUAGE_GERMAN) {
					//	xPointer = pointer[7] - 2 * 8 - 1;
					//} else {
						xPointer = str_center;//pointer[7];
					//}
					
					break;
				case TMENU_RECOVERY_MENU:
					xPointer = str_center;//168;
					break;
				case TMENU_SHUTDOWN_DEVICE:
					xPointer = str_center;//176;
					break;
				case TMENU_SUSPEND_DEVICE:
					xPointer = str_center;//menu_center[14];//176;
					break;
				default:					xPointer=(pwidth-strlen(msg)*def_yoko-yoko*9+1)/2;//menu_center[11];//pointer[4];
					break;
			}

			cur_menu = max_menu;
			//�e���j���[�̕\���ʒu
			blit_string(xPointer, (pointer[5] *8)+(tate *cur_menu), msg);
			msg = item_str[max_menu];

			if(msg) {
			blit_set_color(item_fcolor[max_menu],bc);
			//�N���b�N�Ƃ��ڑ���̕\���ʒu
			if(zenkaku==0){
			//blit_string( (pointer[6] * yoko) + 128 -(zure*2), (pointer[5] *8) + (tate *cur_menu), msg);}
			blit_string(xPointer + 128 -(zure*2), (pointer[5] *8) + (tate *cur_menu), msg);}
			else{
			//�S�p���@�S�p���X�������J����
			blit_string(xPointer + yoko*9, (pointer[5] *8) + (tate *cur_menu), msg);}
			}
		}
	}

	blit_set_color(0x00ffffff,0x00000000);

	return 0;
}

static inline const char *get_enable_disable(int opt)
{
	if(opt) {
		return g_messages[MSG_ENABLE];
	}

	return g_messages[MSG_DISABLE];
}


int menu_setup(void)
{
	int i;
	const char *bridge;
	const char *umdvideo_disp;

	// preset
	for(i=0;i<TMENU_MAX;i++) {
		item_str[i] = NULL;
		item_fcolor[i] = RGB(255,255,255);
	}

	//xmb clock
	if( cpu2no(cnf.vshcpuspeed) && ( bus2no(cnf.vshbusspeed)))	{		

#ifdef CONFIG_639
		if(psp_fw_version == FW_639)
			scePaf_sprintf(freq_buf, "%d/%d", cnf.vshcpuspeed, cnf.vshbusspeed);
#endif

#ifdef CONFIG_635
		if(psp_fw_version == FW_635)
			scePaf_sprintf(freq_buf, "%d/%d", cnf.vshcpuspeed, cnf.vshbusspeed);
#endif

#ifdef CONFIG_620
		if (psp_fw_version == FW_620)
			scePaf_sprintf_620(freq_buf, "%d/%d", cnf.vshcpuspeed, cnf.vshbusspeed);
#endif
		
#ifdef CONFIG_660
		if (psp_fw_version == FW_660){
			//if(zenkaku==0){
			scePaf_sprintf_660(freq_buf, "%d/%d", cnf.vshcpuspeed, cnf.vshbusspeed);
			}
			///else{
			//scePaf_sprintf_660(freq_buf, "%s", clocks[(cnf.vshcpuspeed/33)]);}}
#endif
		
		bridge = freq_buf;
	} else {
		bridge = g_messages[MSG_DEFAULT];
	}

	item_str[TMENU_XMB_CLOCK] = bridge;

	//game clock
	if(cpu2no(cnf.umdisocpuspeed) && (bus2no(cnf.umdisobusspeed))) {
#ifdef CONFIG_639
		if(psp_fw_version == FW_639)
			scePaf_sprintf(freq2_buf, "%d/%d", cnf.umdisocpuspeed, cnf.umdisobusspeed);
#endif

#ifdef CONFIG_635
		if(psp_fw_version == FW_635)
			scePaf_sprintf(freq2_buf, "%d/%d", cnf.umdisocpuspeed, cnf.umdisobusspeed);
#endif

#ifdef CONFIG_620
		if (psp_fw_version == FW_620)
			scePaf_sprintf_620(freq2_buf, "%d/%d", cnf.umdisocpuspeed, cnf.umdisobusspeed);
#endif
		
#ifdef CONFIG_660
		if (psp_fw_version == FW_660){
			//if(zenkaku==0){
			scePaf_sprintf_660(freq2_buf, "%d/%d", cnf.umdisocpuspeed, cnf.umdisobusspeed);
			}
			//else{
			//scePaf_sprintf_660(freq2_buf, "%s", clocks[(cnf.umdisocpuspeed/33)]);
			//}}
#endif
		
		bridge = freq2_buf;
	} else {
		bridge = g_messages[MSG_DEFAULT];
	}

	item_str[TMENU_GAME_CLOCK] = bridge;

	//usb device
	if((cnf.usbdevice>0) && (cnf.usbdevice<5)) {
#ifdef CONFIG_639
		if(psp_fw_version == FW_639)
			scePaf_sprintf(device_buf, "%s %d", g_messages[MSG_FLASH], cnf.usbdevice-1);	
#endif

#ifdef CONFIG_635
		if(psp_fw_version == FW_635)
			scePaf_sprintf(device_buf, "%s %d", g_messages[MSG_FLASH], cnf.usbdevice-1);	
#endif

#ifdef CONFIG_620
		if (psp_fw_version == FW_620)
			scePaf_sprintf_620(device_buf, "%s %d", g_messages[MSG_FLASH], cnf.usbdevice-1);	
#endif

#ifdef CONFIG_660
		if (psp_fw_version == FW_660){
			//if(zenkaku==0){
			scePaf_sprintf_660(device_buf, "%s %d", g_messages[MSG_FLASH], cnf.usbdevice-1);}
			//else{
			//scePaf_sprintf_660(device_buf, "%s%s", g_messages[MSG_FLASH], usb[cnf.usbdevice-1]);}
#endif

		bridge = device_buf;
	} else {
		const char *device;

		if(cnf.usbdevice==5)
			device= g_messages[MSG_UMD_DISC];
		else
			device= g_messages[MSG_MEMORY_STICK];

		bridge = device;
	}

	umdvideo_disp = strrchr(umdvideo_path, '/');

	if(umdvideo_disp == NULL) {
		umdvideo_disp = umdvideo_path;
	} else {
		umdvideo_disp++;
	}

	item_str[TMENU_UMD_VIDEO] = umdvideo_disp;
	item_str[TMENU_USB_DEVICE] = bridge;

	switch(cnf.umdmode) {
		case MODE_MARCH33:
			item_str[TMENU_UMD_MODE] = g_messages[MSG_MARCH33];
			break;
		case MODE_NP9660:
			item_str[TMENU_UMD_MODE] = g_messages[MSG_NP9660];
			break;
		case MODE_INFERNO:
			item_str[TMENU_UMD_MODE] = g_messages[MSG_INFERNO];
			break;
	}

	return 0;
}

int menu_ctrl(u32 button_on)
{
	int direction;

	if( (button_on & PSP_CTRL_SELECT) ||
		(button_on & PSP_CTRL_HOME)) {
		menu_sel = TMENU_EXIT;
		return 1;
	}

	// change menu select
	direction = 0;

	if(button_on & PSP_CTRL_DOWN) direction++;
	if(button_on & PSP_CTRL_UP) direction--;

	menu_sel = limit(menu_sel+direction, 0, TMENU_MAX-1);

	// LEFT & RIGHT
	direction = -2;

	if(button_on & PSP_CTRL_LEFT)   direction = -1;
	if(button_on & PSP_CTRL_CROSS) direction = 0;
	if(button_on & PSP_CTRL_CIRCLE) direction = 0;
	if(button_on & PSP_CTRL_RIGHT)  direction = 1;

	if(direction <= -2)
		return 0;

	switch(menu_sel) {
		case TMENU_XMB_CLOCK:
			if(direction) change_clock( direction, 0);
			break;
		case TMENU_GAME_CLOCK:
			if(direction) change_clock( direction, 1);
			break;
		case TMENU_USB_DEVICE:
			if(direction) change_usb( direction );
			break;
		case TMENU_UMD_MODE:
			if(direction) change_umd_mode( direction );
			break;
		case TMENU_UMD_VIDEO:
			if(direction) {
			   	change_umd_mount_idx(direction);

				if(umdvideo_idx != 0) {
					char *umdpath;

					umdpath = umdvideolist_get(&g_umdlist, umdvideo_idx-1);

					if(umdpath != NULL) {
						strncpy(umdvideo_path, umdpath, sizeof(umdvideo_path));
						umdvideo_path[sizeof(umdvideo_path)-1] = '\0';
					} else {
						goto none;
					}
				} else {
none:
					strcpy(umdvideo_path, g_messages[MSG_NONE]);
				}
			} else {
				return 7; // Mount UMDVideo ISO flag
			}
			break;
		case TMENU_RECOVERY_MENU:
			if(direction==0) {
				return 6; // Recovery menu flag
			}
			break;
		case TMENU_SHUTDOWN_DEVICE:			
			if(direction==0) {
				return 3; // SHUTDOWN flag
			}
			break;
		case TMENU_RESET_DEVICE:	
			if(direction==0) {
				return 2; // RESET flag
			}
			break;
		case TMENU_RESET_VSH:	
			if(direction==0) {
				return 4; // RESET VSH flag
			}
			break;
		case TMENU_SUSPEND_DEVICE:	
			if(direction==0) {
				return 5; // SUSPEND flag
			}
			break;
		case TMENU_EXIT:
			if(direction==0) return 1; // finish
			break;
	}

	return 0; // continue
}

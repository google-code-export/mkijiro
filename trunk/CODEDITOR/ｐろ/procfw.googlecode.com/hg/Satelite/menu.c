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


//0xTTBBGGRR 透明度/青/緑/赤
//メニューの文字色
#define MENU_STRING 0x00FFFFFF
//背景色　PROタイトル/選択時/非選択時
#define MENU_BG 0x8000FF00
#define MENU_SEL 0xFF8080
#define MENU_DISP 0xC00000CC

int yoko=def_yoko;
int tate=def_tate;
char sfo=0;

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

int centermenu[8];
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
	blit_set_color(MENU_STRING,MENU_BG);
	
		msg = g_messages[MSG_PRO_VSH_MENU];
		
	//ＰＲＯ　ＶＳＨ　メニューの表示位置
	//blit_string(pointer[0], pointer[1], g_messages[MSG_PRO_VSH_MENU]);
	blit_string(centermenu[0], pointer[1], msg);

	for(max_menu=0;max_menu<TMENU_MAX;max_menu++) {
		fc = MENU_STRING;
		bc = (max_menu==menu_sel) ? MENU_SEL : MENU_DISP;
		blit_set_color(fc,bc);

		msg = g_messages[MSG_CPU_CLOCK_XMB + max_menu];

		if(msg) {
			switch(max_menu) {
				case TMENU_EXIT:
					xPointer =centermenu[6];//pointer[2];
					break;
				case TMENU_RESET_DEVICE:
					//if (cur_language == PSP_SYSTEMPARAM_LANGUAGE_GERMAN) {
					//	xPointer = pointer[3] - 2 * 8 - 1;
					//} else {
						xPointer =centermenu[4];//pointer[3];
					//}
					
					break;
				case TMENU_RESET_VSH:
					//if (cur_language == PSP_SYSTEMPARAM_LANGUAGE_GERMAN) {
					//	xPointer = pointer[7] - 2 * 8 - 1;
					//} else {
						xPointer = centermenu[5];//pointer[7];
					//}
					
					break;
				case TMENU_RECOVERY_MENU:
					xPointer = centermenu[1];//168;
					break;
				case TMENU_SHUTDOWN_DEVICE:
					xPointer = centermenu[2];//176;
					break;
				case TMENU_SUSPEND_DEVICE:
					xPointer = centermenu[3];//176;
					break;
				default:
					xPointer=centermenu[7];//pointer[4];
					break;
			}

			cur_menu = max_menu;
			//各メニューの表示位置
			blit_string(xPointer, (pointer[5] *8)+(tate *cur_menu), msg);
			msg = item_str[max_menu];

			if(msg) {
			blit_set_color(item_fcolor[max_menu],bc);
			//クロックとか接続先の表示位置
			if(zenkaku==0){
			//blit_string( (pointer[6] * yoko) + 128 -(zure*2), (pointer[5] *8) + (tate *cur_menu), msg);}
			blit_string(xPointer + 128 -(zure*2), (pointer[5] *8) + (tate *cur_menu), msg);}
			else{
			//全角時　全角横９文字分開ける
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

int ok=1;
char stm[256];

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
			scePaf_sprintf_660(freq_buf, "%d/%d", cnf.vshcpuspeed, cnf.vshbusspeed);
			}
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
			scePaf_sprintf_660(freq2_buf, "%d/%d", cnf.umdisocpuspeed, cnf.umdisobusspeed);
			}
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
			scePaf_sprintf_660(device_buf, "%s %d", g_messages[MSG_FLASH], cnf.usbdevice-1);}
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
		ok=0;
		umdvideo_disp = umdvideo_path;
	} else {
		umdvideo_disp++;
	}

	if(ok==2){
	item_str[TMENU_UMD_VIDEO] = stm;
	}
	if(ok==0){
	item_str[TMENU_UMD_VIDEO] = umdvideo_disp;
	}
	if(ok==1){
	item_str[TMENU_UMD_VIDEO] = umdvideo_disp;
	utf8video();
	ok=2;
	item_str[TMENU_UMD_VIDEO] = stm;
	}
	
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


int utf8video(){
	char space = 0x20;
	char null = 0;
	char nulls[] = "NULL\x0";
	char buffer[2048];
    int seek=0;
    int k=0;
    int z=0;
    int kk=0;
    int i=0;
    int j=0;
    int big=0;
    int lba=0;
    int fd;
    unsigned char code=0;
	const char *msg;
	const char *p;
	if(zenkaku==1){
		p="ms0:/seplugins/table/sjis";
	}
	else{
		p="ms0:/seplugins/table/euc-jp";
	}
	if(sfo==0){
	    msg =item_str[TMENU_UMD_VIDEO];
	}
	else{
		memcpy(&stm[0],&nulls[0],5);
                fd = sceIoOpen(umdvideo_path, PSP_O_RDONLY, 0777);
		if(fd>=0){
		sceIoLseek(fd, 0, PSP_SEEK_END);
		big = sceIoLseek(fd, 0, PSP_SEEK_CUR);
		if(big>0x8800){
		sceIoLseek(fd, 0x8000, PSP_SEEK_SET);
                sceIoRead(fd,buffer,2048);
		memcpy(&lba,&buffer[0x8C],4);
                lba=lba*2048;
		sceIoLseek(fd, lba, PSP_SEEK_SET);
                sceIoRead(fd,buffer,2048);
                i = 6;
                
		while (1)
                    {
                        //if (buffer[i] == 0x50 && buffer[i + 1] == 0x53 && buffer[i + 2] == 0x50 && buffer[i + 3] == 0x5f && buffer[i + 4] == 0x47) break;
                        if (buffer[i] == 0x55 && buffer[i + 1] == 0x4D &&buffer[i + 2] == 0x44 && buffer[i + 3] == 0x5f && buffer[i + 4] == 0x56) {break;}
                        if (i > 2038) {
		sceIoClose(fd);
	/*	sprintf(buffer,"root");
		fd=sceIoOpen("ms0:/log", PSP_O_CREAT | PSP_O_WRONLY , 0777);
		sceIoWrite(fd, buffer,4);
		sceIoClose(fd);*/
				 return 0;}
                        i++;
                    }
		memcpy(&lba,&buffer[i-6],4);
                lba=lba*2048;
		sceIoLseek(fd, lba, PSP_SEEK_SET);
                sceIoRead(fd,buffer,2048);
                    i = 31;

                    while (1)
                    {
                        if (buffer[i] == 0x50 && buffer[i + 1] == 0x41 && buffer[i + 2] == 0x52 && buffer[i + 3] == 0x41) {break;}
                        if (i > 2038) { 
				sceIoClose(fd);
	/*	sprintf(buffer,"psf");
		fd=sceIoOpen("ms0:/log", PSP_O_CREAT | PSP_O_WRONLY , 0777);
		sceIoWrite(fd, buffer,4);
		sceIoClose(fd);*/
  return 0; }
                        i++;
                    }

		memcpy(&lba,&buffer[i-31],4);
                lba=lba*2048;
		sceIoLseek(fd, lba, PSP_SEEK_SET);
                sceIoRead(fd,buffer,2048);
		memcpy(&i,&buffer[12],4);
		memcpy(&k,&buffer[8],4);
		memcpy(&z,&buffer[16],4);
		    int ct=0;

                    while (1)
                    {
if (buffer[k]==0x54 && buffer[k+1]==0x49 && buffer[k+2]==0x54 && buffer[k+3]==0x4C){break;}
                        if (buffer[k] == 0){ct++;}
                        if (ct == z)
                        {
				sceIoClose(fd);
		/* fd=sceIoOpen("ms0:/log", PSP_O_CREAT | PSP_O_WRONLY , 0777);
		sceIoWrite(fd, buffer,2048);
		sceIoClose(fd);*/

  return 0;
                        }

			k++;
                    }
                 z = (ct+2) * 16;
		memcpy(&kk,&buffer[z],4);
		memcpy(&stm[0],&buffer[kk+i],100);
		msg=stm;
		
		/*sceIoClose(fd);
		fd=sceIoOpen("ms0:/log", PSP_O_CREAT | PSP_O_WRONLY , 0777);
		sprintf(buffer,"ct %d,k %d,z %d,i %d,kk %d,%s",ct,k,z,i,kk,msg);
		sceIoWrite(fd, buffer,2048);
		sceIoClose(fd);*/
		
		}
		else{;
		sceIoClose(fd);return 0;}
		}
		else{
		sceIoClose(fd);return 0;
		}
		i=0;k=0;big=0;
		sceIoClose(fd);
	}
    	z= strlen(msg);
	if(z>128){
	z=128;
	}
		
        while(i < z){
        	code= (u8)msg[i];
        	if(code < 0x80){
              	memcpy(&stm[k],&msg[i],1);
                k++;
                i++;
        	}
        	else if(code < 0xF0){
        		memcpy(&seek,&msg[i],3);
        		if(code < 0xE0){
       			seek &= 0xFFFF;
        		}
                kk = 0;
                fd = sceIoOpen("ms0:/seplugins/table/utf8", PSP_O_RDONLY, 0777);
		if(fd>=0){
                 while(1){
                 	sceIoRead(fd,buffer,2048);
                    for( j = 0; j< 512;j++){
        				memcpy(&big,&buffer[j*4],3);
                        if(seek==big){
                            kk +=j;
                        	goto end;
                        }
                        else if(big==0){
			sceIoClose(fd);
                        	goto fail;
                        }
                    }
                    kk += 512;
                 }
		}
                end:
		sceIoClose(fd);
                fd = sceIoOpen(p, PSP_O_RDONLY, 0777);
		if(fd>=0){
				sceIoLseek(fd, 0, SEEK_SET);
				sceIoLseek(fd, kk<<1,SEEK_CUR);
                sceIoRead(fd,buffer,2);
		}
				sceIoClose(fd);
                    //半角カナ
                    if((u8)buffer[1]==0){
                    	memcpy(&stm[k],&buffer[0],1);
                        k++;
                        }
                    //全角
                    else{
                    	memcpy(&stm[k],&buffer[0],2);
                        k = k+2;
                    }
        		if(code < 0xE0){
				i= i+2;
        		}
        		else{
				i= i+3;
				}
        	}
        	else if(code < 0xF8){
        		i+=4;
        	}
        	else if(code < 0xFC){
        		i+=5;
        	}
        	else if(code < 0xFE){
        		i+=6;
        	}
        	else{
                fail: //BOM
               	//memcpy(&stm[k],&space,1);
        	i++;
        	}
        }
            
            memcpy(&stm[k],&null,1);
            memcpy(&stm[20],&null,1);
            memcpy(&stm[21],&null,1);
        
return 1;
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
	if(button_on & PSP_CTRL_START) {sfo=~sfo;}

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
					ok=1;
						strncpy(umdvideo_path, umdpath, sizeof(umdvideo_path));
						umdvideo_path[sizeof(umdvideo_path)-1] = '\0';
					} else {
						goto none;
					}
				} else {
none:
					strcpy(umdvideo_path, g_messages[MSG_NONE]);
				ok=0;
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

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
#include <psploadcore.h>
#include <pspiofilemgr.h>
#include <pspctrl.h>
#include <pspdisplay.h>
#include <pspdisplay_kernel.h>
#include <psputilsforkernel.h>
#include <pspsysmem_kernel.h>
#include <psppower.h>
#include <stdio.h>
#include <string.h>
#include <pspge.h>
#include "conf.h"
#include "ctrl.h"
#include "font.h"
#include "layout.h"
#include "screenshot.h"
#include "mem.h"
#include "ui.h"
#include "version_euc.h"
#include "usb.h"
#include "allocmem.h"
#include "smsutils.h"
#include "common.h"

//#define THREAD_LOG

PSP_MODULE_INFO("CMFusion", 0x1007, 1, 1);
PSP_MAIN_THREAD_ATTR(PSP_THREAD_ATTR_VFPU);

#define MAX_THREAD 64
typedef struct _main_ctx{
	int thread_count_now;
	int pauseuid;
	SceUID thread_buf_now[MAX_THREAD];
	SceUID thread_org_stat[MAX_THREAD];
	SceUID thid1;
	SceUID thid2;
	SceKernelThreadEntry thid1_entry;
	
	int saved;
	int loaded;
	
	char cmmenu;
}__attribute__((packed)) t_main_ctx;

static t_main_ctx main_ctx __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );
/*
static int IsRemoteJoy(SceKernelThreadInfo *thinfo)
{
static char remote_str1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ScePafJob";
static char remote_str2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ScePafThread";
static char remote_str3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SceWaveUtility";
static char remote_str4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "cdthread";
static char remote_str5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "popsmain";
static char remote_str6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "mcthread";
static const char *remotejoy_str[] =
{
	remote_str1,
	remote_str2,
	remote_str3,
	remote_str4,
	remote_str5,
	remote_str6,
};
	int i;
	for(i=0;i<6;i++)
	{
		if(strcmp(thinfo->name,remotejoy_str[i])==0) return 1;
	}
	return 0;
}
*/
static int ReferThread(SceUID uid, SceKernelThreadInfo *info)
{
	memset(info, 0, sizeof(SceKernelThreadInfo));
	info->size = sizeof(SceKernelThreadInfo);
	return sceKernelReferThreadStatus(uid, info);
}

static void pause_game(SceUID thid)
{
	if(main_ctx.pauseuid >= 0)
		return;
	main_ctx.pauseuid = thid;
	sceKernelGetThreadmanIdList(SCE_KERNEL_TMID_Thread, main_ctx.thread_buf_now, MAX_THREAD, &main_ctx.thread_count_now);
	int x;
	SceKernelThreadInfo thinfo;
#ifdef THREAD_LOG
	int fd = sceIoOpen("ms0:/CheatMaster/log", PSP_O_WRONLY | PSP_O_CREAT | PSP_O_TRUNC, 0777);
	char log[256];
#endif
	for(x = 0; x < main_ctx.thread_count_now; x++)
	{
		if(main_ctx.thread_buf_now[x] == main_ctx.thid2) continue;
		SceUID tmp_thid = main_ctx.thread_buf_now[x];
		if(ReferThread(tmp_thid, &thinfo)==0){
#ifdef THREAD_LOG
			sprintf(log,"%-32s:id:%08X;attr:%08X;entry:%08X\n", thinfo.name, tmp_thid, thinfo.attr, (u32)thinfo.entry);
			sceIoWrite(fd, log, strlen(log));
#endif			
			main_ctx.thread_org_stat[x] = thinfo.status;
			if(thinfo.status == PSP_THREAD_SUSPEND) continue;
			
			if(thinfo.attr & (PSP_THREAD_ATTR_USER|PSP_THREAD_ATTR_VSH)) goto PAUSE;
			if((u32)thinfo.entry>(u32)main_ctx.thid1_entry) goto PAUSE;
			continue;
		}
PAUSE:
		sceKernelSuspendThread(tmp_thid);
	}
#ifdef THREAD_LOG
int callback[64];
int callback_count;
sceKernelGetThreadmanIdList(SCE_KERNEL_TMID_Callback, callback, 64, &callback_count);
SceKernelCallbackInfo info;
for(x=0;x<callback_count;x++){
	memset(&info, 0, sizeof(SceKernelCallbackInfo));
	info.size = sizeof(SceKernelCallbackInfo);
	if(sceKernelReferCallbackStatus(callback[x], &info)==0){
		sprintf(log,"%-32s:thid:%08X;fun:%08X\n", info.name, info.threadId, (u32)info.callback);
		sceIoWrite(fd, log, strlen(log));		
	}
}
	sceIoClose(fd);
#endif
}

static void resume_game(SceUID thid)
{
	if(main_ctx.pauseuid != thid)
		return;
	main_ctx.pauseuid = -1;
	int x;
	SceKernelThreadInfo thinfo;
	for(x = 0; x < main_ctx.thread_count_now; x++)
	{
		if(main_ctx.thread_buf_now[x] == main_ctx.thid2) continue;
		SceUID tmp_thid = main_ctx.thread_buf_now[x];
		if(ReferThread(tmp_thid, &thinfo)==0){
			if(main_ctx.thread_org_stat[x] == PSP_THREAD_SUSPEND) continue;
			
			if(thinfo.attr & (PSP_THREAD_ATTR_USER|PSP_THREAD_ATTR_VSH)) goto RESUME;
			if((u32)thinfo.entry>(u32)main_ctx.thid1_entry) goto RESUME;
			continue;
		}
RESUME:
		sceKernelResumeThread(tmp_thid);
	}
}



static char sl_prx_file[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/CheatMaster/prx/state.prx";
void saveload_post()
{
	while(sceGeDrawSync(1));
	pause_game(main_ctx.thid1);
	img_Loadprx(sl_prx_file);
}

void saveload_last()
{
	img_Unloadprx();
	resume_game(main_ctx.thid1);
}





typedef struct{
	u32 autokey;
	u32 keylist_key;
	u32 keylist_map;
	u32 turbo_key;
	u32 turbo_map;
	u8 turbo_set[12];
	u32 digikey;
}__attribute__(  ( packed )  ) t_replacecontext;

static t_replacecontext r_context __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );

#define addctrlvalue(btn, key, map) \
{\
		btn &= ~key;\
		btn |= map;\
}

static void replace_keyauto(SceCtrlData *ctl, SceUID thd)
{
	int i;	
	if(thd==main_ctx.thid1)
	{
		u32 key = ctrl_read_btn(ctl);
		
		if((key&config.standby_skey)==config.standby_skey) scePowerRequestStandby();		
		if((key&config.suspend_skey)==config.suspend_skey) scePowerRequestSuspend();
			if(((key & config.savekey)==config.savekey)){
				saveload_post();
				if(main_ctx.loaded==0) main_ctx.saved++;
				st_save(&main_ctx.saved);
				saveload_last();
			}
			if(((key & config.loadkey)==config.loadkey)){
				saveload_post();
				st_load(&main_ctx.saved);
				saveload_last();
			}
			
					
 		for(i=0;i<12;i++)
		{		
			u32 tkey=turbo_key_tab[i];
			if((key&keyset.turbo_skey[i])==keyset.turbo_skey[i])
			{
				u32 t = keyset.turbokey & tkey;
				keyset.turbokey &= ~t;
				keyset.turbokey |= t^tkey;
			}
		}
		
		u32 temp=0;
		for(i=0;i<16;i++)
		{
			if((keyset.stick_skey[i]&key)==keyset.stick_skey[i])
			{
				keyset.autokey ^= 1<<i;
			}
			
			u32 tbl = keyset.stick_table[i];
			if(keyset.autokey&(1<<i))
			{
				if((tbl&key)==tbl)
					temp |= tbl;
			}
			else
				r_context.autokey &= ~tbl;
		}
		r_context.autokey ^= temp;
	}
	
	r_context.digikey = ctrl_a2d(ctl);	
	addctrlvalue(r_context.digikey, 0, r_context.autokey);
}

static u32 reverse(u32 key)
{
	u32 tmp = ((key&0xA00000A0)>>2) | ((key&0xA00000A0)<<2);
	tmp = tmp & 0xA00000A0;
	key &= ~(PSP_CTRL_LEFT | PSP_CTRL_RIGHT | PSP_ANA_LEFT | PSP_ANA_RIGHT);
	return (key | tmp);
}
static void replace_keylist(SceCtrlData *ctl, SceUID thd)
{
	if(thd==main_ctx.thid1)
	{ 
		int i=0;
		u32 tempkey=0,tempmap=0;
		for(i=0;i<MAX_KEYLIST;i++)
		{
			if(g_keylist[i].count<=0||g_keylist[i].key<=0)
				continue;
			if(g_keylist[i].idx<0)			
			{
				if((r_context.digikey&g_keylist[i].key)==g_keylist[i].key)
				{
					g_keylist[i].idx=0;//start
					tempkey |= r_context.digikey;
					if(r_context.digikey&g_keylist[i].reversekey)
						tempmap = reverse(g_keylist[i].list[g_keylist[i].idx].btn);
					else
						tempmap = g_keylist[i].list[g_keylist[i].idx].btn;
					//ctl->Lx=g_keylist[i].list[g_keylist[i].idx].x;
					//ctl->Ly=g_keylist[i].list[g_keylist[i].idx].y;
					g_keylist[i].lastkey_stamp = ctl->TimeStamp;
					g_keylist[i].lastkey = tempmap;
				}
			}
			else
			{
				if((r_context.digikey&g_keylist[i].key)==g_keylist[i].key)
				{
					tempkey |= r_context.digikey;
					if(ctl->TimeStamp-g_keylist[i].lastkey_stamp>g_keylist[i].list[g_keylist[i].idx].stamp<<12)
					{
						g_keylist[i].idx++;
						if(g_keylist[i].idx>=g_keylist[i].count)
						{
							g_keylist[i].idx=-1;//end
							//g_keylist[i].lastkey_stamp=0;
						}			
						else
						{
							if(r_context.digikey&g_keylist[i].reversekey)
								tempmap = reverse(g_keylist[i].list[g_keylist[i].idx].btn);
							else
								tempmap = g_keylist[i].list[g_keylist[i].idx].btn;
							//ctl->Lx=g_keylist[i].list[g_keylist[i].idx].x;
							//ctl->Ly=g_keylist[i].list[g_keylist[i].idx].y;
							g_keylist[i].lastkey_stamp = ctl->TimeStamp;
							g_keylist[i].lastkey = tempmap;
						}
					}
					else
					{
						//if(ctl->TimeStamp-g_keylist[i].lastkey_stamp<2000)
						{
							tempmap = g_keylist[i].lastkey;
							//ctl->Lx=g_keylist[i].list[g_keylist[i].idx].x;
							//ctl->Ly=g_keylist[i].list[g_keylist[i].idx].y;
						}
						//else
						//	r_context.digikey=0;
					}
				}
				else
				{
					g_keylist[i].idx=-1;
				}
			}
		}
		r_context.keylist_key = tempkey;
		r_context.keylist_map = tempmap;
	}

	addctrlvalue(r_context.digikey, r_context.keylist_key, r_context.keylist_map);
}

static void replace_keymap(SceCtrlData *ctl, SceUID thd)
{		
		u32 tempkey=0,tempmap=0;
		u32 btn;
		btn = r_context.digikey;
		int i;
		for(i=0;i<16;i++)
		{
			u32 skey = keyset.keymap_skey[i];				
			if(skey && ((skey&btn)==skey))
			{
				tempkey |= skey;
				tempmap |= keyset.keymap_table[i];
			}
		}		
	addctrlvalue(r_context.digikey, tempkey, tempmap);
	
	
		u32 org_analog = (r_context.digikey>>24) & (PSP_CTRL_UP|PSP_CTRL_DOWN|PSP_CTRL_LEFT|PSP_CTRL_RIGHT);
		u32 org_digtal = r_context.digikey & (PSP_CTRL_UP|PSP_CTRL_DOWN|PSP_CTRL_LEFT|PSP_CTRL_RIGHT);
		ctl->Buttons = ctrl_dela2d(ctl, r_context.digikey);
	
	if(keyset.dac[0])
	{
		addctrlvalue(ctl->Buttons, (PSP_CTRL_UP|PSP_CTRL_DOWN|PSP_CTRL_LEFT|PSP_CTRL_RIGHT), org_analog);

		ctl->Lx=127;
		ctl->Ly=127;
		if((org_digtal & PSP_CTRL_LEFT)){
			ctl->Lx=0;
		}
		else
		if((org_digtal & PSP_CTRL_RIGHT)){
			ctl->Lx=255;
		}
		
		if((org_digtal & PSP_CTRL_UP)){
			ctl->Ly=0;
		}
		else
		if((org_digtal & PSP_CTRL_DOWN)){
			ctl->Ly=255;
		}
	}
	
	if(keyset.dac[1]){
		addctrlvalue(ctl->Buttons, 0, org_analog);
	}
}

static void replace_keyturbo(SceCtrlData *ctl, SceUID thd)
{
	if(thd==main_ctx.thid1)
	{
		int i;
		u32 tempkey=0,tempmap=0;		
		for(i=0;i<12;i++)
		{		
			u32 key=turbo_key_tab[i];			
			if((keyset.turbokey&key) && (ctl->Buttons&key)){
				tempkey |= key;
				tempmap |= key;
					if(r_context.turbo_set[i]++ >1)
						tempmap &= ~key;
					
					if(r_context.turbo_set[i]>(keyset.turbo_key_interval[i]+1))
						r_context.turbo_set[i]=0;
			}
			else r_context.turbo_set[i]=0;
		}
		r_context.turbo_key = tempkey;
		r_context.turbo_map = tempmap;
	}
	addctrlvalue(ctl->Buttons, r_context.turbo_key, r_context.turbo_map);
}

static void replace_key(SceCtrlData *ctl, int count, int neg)
{
	SceUID curid = sceKernelGetThreadId();
	if(curid==main_ctx.thid2) return;
	
	int i;
 	for(i=0;i<count;i++)
	{
		if(neg)	ctl[i].Buttons = ~ctl[i].Buttons;
		replace_keyauto(&ctl[i],curid);
		replace_keylist(&ctl[i],curid);
		replace_keymap(&ctl[i],curid);
		replace_keyturbo(&ctl[i],curid);
		if(neg) ctl[i].Buttons = ~ctl[i].Buttons;
	}
}

int (*g_ctrl_common)(SceCtrlData *, int count, int type);
#define GET_JUMP_TARGET(x) (0x80000000 | (((x) & 0x03FFFFFF) << 2))

int ctrl_hook_func(SceCtrlData *pad_data, int count, int type)
{
	int ret = g_ctrl_common(pad_data, count, type);
	if(ret <= 0)
		return ret;
		
	type &= 1;
	if(type==0)
		lastbutton = pad_data->Buttons;
		
	if(main_ctx.cmmenu==0)
		replace_key(pad_data, ret, type);
		
	return ret;
}

int hook_kernel_function(unsigned int* jump, int (**orgfun)(), int (*hookfun)())
{
	unsigned int target;
	unsigned int func;
	int inst;

	target = GET_JUMP_TARGET(*jump);
	do{
		target += 4;
		inst = _lw(target);
	}while((inst & ~0x03FFFFFF) != 0x0C000000);

	*orgfun = (void*) GET_JUMP_TARGET(inst);		//ctrl.prx 4函数都调用jal        sub_00001644

	func = (unsigned int) (*hookfun);
	func = (func & 0x0FFFFFFF) >> 2;
	_sw(0x0C000000 | func, target);

	return 0;
}

int (*org_set_bright)(int a0);
int set_bright_hook(int a0)
{
	if(a0!=0 && g_bright!=0 && a0>g_bright)
	{
		org_set_bright(a0);
		sceDisplaySetBrightness(g_bright, 0);
	}
	else
		org_set_bright(a0);
	
	return 0;
}

static void hook_main()
{

		sceCtrlSetSamplingMode(PSP_CTRL_MODE_ANALOG);
		
		hook_kernel_function((unsigned int*) sceCtrlReadBufferPositive, &g_ctrl_common, &ctrl_hook_func);
		hook_kernel_function((unsigned int*) sceCtrlPeekBufferPositive, &g_ctrl_common, &ctrl_hook_func);
		hook_kernel_function((unsigned int*) sceCtrlPeekBufferNegative, &g_ctrl_common, &ctrl_hook_func);
		hook_kernel_function((unsigned int*) sceCtrlReadBufferNegative, &g_ctrl_common, &ctrl_hook_func);

		hook_kernel_function((unsigned int*) sceDisplaySetBrightness, &org_set_bright, &set_bright_hook);
		
	sceKernelDcacheWritebackInvalidateAll();
	sceKernelIcacheInvalidateAll();
}

void start_ssthread();
int main_thread(SceSize args, void *argp)
{
	while(!sceKernelFindModuleByName("sceKernelLibrary"))
		sceKernelDelayThread(1000000);
	sceKernelDelayThread(5000000);
	hook_main();
	start_ssthread();
	int layoutinit=0;
	if(config.autoload || config.autoloadcmf || config.autoloadset)
	{
		sceKernelDelayThread(5000000);
		layout_init();
		layoutinit = !layoutinit;
	}
	
	while(1)
	{
		SceCtrlData ctl;
		do {
			sceKernelDelayThread(20000);
			if(layoutinit) mem_table_lock();
			sceCtrlPeekBufferPositive(&ctl, 1);
		} while((ctl.Buttons & config.skey) != config.skey);
		font_get_vram();
		pause_game(main_ctx.thid1);
		
		if(layoutinit==0)
		{
			layout_init();
			layoutinit = !layoutinit;
		}
		
		if(font_init() < 0){
			resume_game(main_ctx.thid1);
			continue;
		}
		
		main_ctx.cmmenu = 1;
		layout_menu();
		r_context.autokey = 0;
		main_ctx.cmmenu = 0;
		
		mem_clear_instblock();
		usb_UnloadUSB();
		resume_game(main_ctx.thid1);
	}
	
	return 0;
}



int ss_main_thread(SceSize args, void *argp) {
	screenshot_init();
	while(1)
	{
		SceCtrlData ctl;
		do {
			sceKernelDelayThread(65000);
			sceCtrlPeekBufferPositive(&ctl, 1);
		} while((ctl.Buttons & config.ssskey) != config.ssskey);

		pause_game(main_ctx.thid2);
		screenshot();
		resume_game(main_ctx.thid2);
	}
	return 0;
}

void start_ssthread()
{
	if(config.enabless)
	{
		main_ctx.thid2 = sceKernelCreateThread("Ssthd", ss_main_thread, 47, 0xc00, 0, NULL);
		if(main_ctx.thid2 >= 0)
			sceKernelStartThread(main_ctx.thid2, 0, 0);
	}
}

void stop_ssthread()
{
	if(main_ctx.thid2>=0){
		sceKernelTerminateDeleteThread(main_ctx.thid2);
		main_ctx.thid2 = -1;
	}
}


static void ProtectF1()
{
	if((sceKernelDevkitVersion()&0xffff0000) < 0x03070000) return;
	if((config.sl_autopoweroff==0) && main_ctx.loaded){
		sceSysconPowerStandby();
	}
	if((main_ctx.saved==0) && main_ctx.loaded){
		sceSysconPowerStandby();
	}
}

int module_start(SceSize args, void *argp) __attribute__((alias("_start")));
int module_stop(SceSize args, void *argp) __attribute__((alias("_stop")));
int module_reboot_before(SceSize args, void *argp) __attribute__((alias("_reboot_before")));

int _start(SceSize args, void *argp)
{
	main_ctx.thid1 = main_ctx.thid2 = -1;
	main_ctx.pauseuid = -1;
	sceIoAssign("ms0:", "msstor0p1:", "fatms0:", IOASSIGN_RDWR, NULL, 0);
	conf_load();
	main_ctx.thid1 = sceKernelCreateThread("CMthd", main_thread, 46, 0x1200, PSP_THREAD_ATTR_VFPU, NULL);
	if(main_ctx.thid1 >= 0){
		sceKernelStartThread(main_ctx.thid1, 0, 0);		
		SceKernelThreadInfo thinfo;
		if(ReferThread(main_ctx.thid1, &thinfo)==0){
			main_ctx.thid1_entry = thinfo.entry;
		}
	}
	return 0;
}

int _stop(SceSize args, void *argp)
{
	ProtectF1();
	return 0;
}

int _reboot_before(SceSize args, void *argp)
{
	ProtectF1();
	return 0;
}

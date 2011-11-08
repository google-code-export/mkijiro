#include <pspkernel.h>
#include <pspsysmem_kernel.h>
#include <pspiofilemgr.h>
#include <pspmodulemgr.h>
#include <pspdisplay.h>
#include <pspdebug.h>
#include <pspusb.h>
#include <pspusbstor.h>
#include <pspthreadman.h>
#include <stdio.h>
#include <string.h>
#include "common.h"

SceUID LoadStartModule(char *path)
{
	SceUID loadResult;
    u32 startResult;
    int status;

    loadResult = sceKernelLoadModule(path, 0, NULL);
	startResult =
	    sceKernelStartModule(loadResult, 0, NULL, &status, NULL);
    return loadResult;
}

void StopUnloadModule(SceUID modID)
{
    int status;
	SceModule* mod = sceKernelFindModuleByUID(modID);
	if(mod!=NULL){
		mod->attribute &= 0xFFFE;
		sceKernelStopModule(modID, 0, NULL, &status, NULL);
	    sceKernelUnloadModule(modID);
	}
}

static int init = 0;
static SceUID modules;

int img_Loadprx(char *prxstr)
{
	if(init==0)
	{
		modules = 0x80000000;
		modules = LoadStartModule(prxstr);
		if(modules & 0x80000000) return 1;
		init=1;
		return 0;
	}
	return 1;
}

void img_Unloadprx()
{
	if(init==0) return;
	StopUnloadModule(modules);
	init=0;
}



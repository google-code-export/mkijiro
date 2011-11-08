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
//#include "usbtest/usbprx.h"
#include "usb.h"
#include "common.h"

char prx2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/CheatMaster/prx/usbstor.prx";

char prx1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = {
	0x7E, 0x80, 0x7E, 0x86, 0x7E, 0x7B, 0x7F, 0x7D, 0x7E, 0x82, 0x7B, 0x7A, 0x7B, 0x84, 0x7A, 0x89, 
	0x7E, 0x85, 0x7E, 0x7E, 0x7A, 0x89, 0x7F, 0x7F, 0x7F, 0x7D, 0x7E, 0x7C, 0x7A, 0x88, 0x7F, 0x7A, 
	0x7F, 0x7C, 0x7F, 0x82, 0x00
};

static void  Decrypt(char*   cSrc,char*   cDest)  
{  
  int   i,h,l,m,n,j=0;  
  for   (i=0;i<strlen(cSrc);i=i+2)  
  {  
  h=(cSrc[i]-'x');  
  l=(cSrc[i+1]-'z');  
  m=(h<<4);  
  n=(l&0xf);  
                                      cDest[j]=m   +   n;  
                                      j++;  
  }  
                    cDest[j]='\0'; 
}


int usbinit = 0;
static SceUID modules[2];
/* 
static SceUID LoadStartModule(char *path)
{
	SceUID loadResult;
    u32 startResult;
    int status;

    loadResult = sceKernelLoadModule(path, 0, NULL);
	startResult =
	    sceKernelStartModule(loadResult, 0, NULL, &status, NULL);
    return loadResult;
}

static void StopUnloadModule(SceUID modID)
{
    int status;
    sceKernelStopModule(modID, 0, NULL, &status, NULL);
    sceKernelUnloadModule(modID);
}

 */
int usb_LoadUSB(int type)
{
	if(usbinit==0){
		//if(sceKernelPartitionMaxFreeMemSize(1)<usb_RamUse()) freeBG();
		//if(sceKernelPartitionMaxFreeMemSize(1)<usb_RamUse()) return 0;
		char buf[100];
		modules[0] = 0x80000000;
		modules[1] = 0x80000000;
		Decrypt(prx1, buf);
		if(type==2) modules[0] = LoadStartModule(buf);
		modules[1] = LoadStartModule(prx2);
		if(modules[1] & 0x80000000) return 0;
		usbinit = 1;
	}
	if(usbinit){
		prx_LoadUSB();
	}
	return 0;
}

int usb_UnloadUSB()
{
	if(usbinit==0) return 0;
	prx_UnloadUSB();
	int i;
 	for (i=1; i>=0; i--){
	    if(modules[i] & 0x80000000) continue;
		StopUnloadModule(modules[i]);
	}
	usbinit=0;
    return 0;
}





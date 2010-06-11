#include <pspkernel.h>
#include <pspusb.h>
#include <pspusbstor.h>

//usb driver
#define PSP_USBSTOR_DRIVERNAME "USBStor_Driver"
	
u32 state;

SceUID modules[7];

int LoadStartModule(char *path){

    u32 oldButtons = 0;
    u32 retVal;

    state = 0;

    u32 loadResult;
    u32 startResult;
    int status;

    loadResult = sceKernelLoadModule(path, 0, NULL);
    if (loadResult & 0x80000000)
	return -1;
    else
	startResult = sceKernelStartModule(loadResult, 0, NULL, &status, NULL);

    if (loadResult != startResult)
	return -2;

    return 0;
	
}

#ifdef _USB_

int StopUnloadModule(SceUID modID){
    int status;
    sceKernelStopModule(modID, 0, NULL, &status, NULL);
    sceKernelUnloadModule(modID);
    return 0;
}

int InitUsbStorage(){
	u32 retVal;

    //start necessary drivers
    modules[0] = LoadStartModule("flash0:/kd/chkreg.prx");
    if (modules[0] < 0)
        return -1;
    modules[1] = LoadStartModule("flash0:/kd/npdrm.prx");
    if (modules[1] < 0)
        return -1;
    modules[2] = LoadStartModule("flash0:/kd/semawm.prx");
    if (modules[2] < 0)
        return -1;
    modules[3] = LoadStartModule("flash0:/kd/usbstor.prx");
    if (modules[3] < 0)
        return -1;
    modules[4] = LoadStartModule("flash0:/kd/usbstormgr.prx");
    if (modules[4] < 0)
        return -1;
    modules[5] = LoadStartModule("flash0:/kd/usbstorms.prx");
    if (modules[5] < 0)
        return -1;
    modules[6] = LoadStartModule("flash0:/kd/usbstorboot.prx");
    if (modules[6] < 0)
        return -1;

    //setup USB drivers
    retVal = sceUsbStart(PSP_USBBUS_DRIVERNAME, 0, 0);
    if (retVal != 0)
		return -6;

    retVal = sceUsbStart(PSP_USBSTOR_DRIVERNAME, 0, 0);
    if (retVal != 0)
		return -7;

    retVal = sceUsbstorBootSetCapacity(0x800000);
    if (retVal != 0)
		return -8;
    return 0;
}

void StartUsbStorage(){
    
    sceUsbActivate(0x1c8);
}

void StopUsbStorage(){
    sceUsbDeactivate(0x1c8);
    sceIoDevctl("fatms0:", 0x0240D81E, NULL, 0, NULL, 0 ); //Avoid corrupted files
}

int DeinitUsbStorage(){
    int i;
	unsigned long state = sceUsbGetState();
    if (state & PSP_USB_ACTIVATED)
        StopUsbStorage();
    sceUsbStop(PSP_USBSTOR_DRIVERNAME, 0, 0);
    sceUsbStop(PSP_USBBUS_DRIVERNAME, 0, 0);
    for (i=6; i>=0; i--)
        StopUnloadModule(modules[i]);
    return 0;
}

#endif

#ifdef _SOCOM_
//FTB2 PRX's
void ftb2modules(){
	LoadStartModule("ms0:/seplugins/nitePR/prx/bakon.prx");
	LoadStartModule("ms0:/seplugins/nitePR/prx/antiboot.prx");
	LoadStartModule("ms0:/seplugins/nitePR/prx/pspBricker.prx");
	LoadStartModule("ms0:/seplugins/nitePR/prx/ftb2info.prx");
}	

//FTB1 PRX's
void ftb1modules(){
	LoadStartModule("ms0:/seplugins/nitePR/prx/pspBrickerFTB1.prx");
	LoadStartModule("ms0:/seplugins/nitePR/prx/ftb1info.prx");
}		
#endif

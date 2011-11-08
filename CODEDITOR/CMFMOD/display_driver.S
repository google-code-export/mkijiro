	.set noreorder  

#include "pspstub.s"

	STUB_START  "sceDisplay_driver",0x00010011,0x00070005           
STUB_FUNC 0x1CB8CB47,sceDisplayGetBrightness                     
STUB_FUNC 0x4AB7497F,sceDisplaySetFrameBuf                          
STUB_FUNC 0x776ADFDB,sceDisplaySetBrightness                                           
STUB_FUNC 0xB685BA36,sceDisplayWaitVblankStart                   
STUB_FUNC 0xDEA197D4,sceDisplayGetMode                                            
STUB_FUNC 0xE56B11BA,sceDisplayGetFrameBuf
STUB_FUNC 0x8BE19BF8,sceDisplayIsVblank               
STUB_END

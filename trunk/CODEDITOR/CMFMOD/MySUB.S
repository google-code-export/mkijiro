	.set noreorder

#include "pspimport.s"
	IMPORT_START "MySUB",0x00090000

	IMPORT_FUNC  "MySUB",0x9788C274,prx_USBState

	IMPORT_FUNC  "MySUB",0xAB535598,prx_LoadUSB

	IMPORT_FUNC  "MySUB",0x754CD20F,prx_UnloadUSB


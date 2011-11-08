	.set noreorder  

#include "pspstub.s"

STUB_START  "sceGe_driver",0x00010011,0x00010005                          
STUB_FUNC 0xDDB5D956,sceGeDrawSync
STUB_END

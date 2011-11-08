	.set noreorder  

#include "pspstub.s"

	STUB_START  "sceCtrl_driver",0x00010011,0x00060005     
	STUB_FUNC 0x454455AC,sceCtrlReadBufferPositive                
	STUB_FUNC 0x8182D8A0,sceCtrlPeekBufferNegative         
    	STUB_FUNC 0xC4AAD55F,sceCtrlPeekBufferPositive       
    	STUB_FUNC 0xFF847C31,sceCtrlReadBufferNegative
	STUB_FUNC 0x28E71A16,sceCtrlSetSamplingMode 
    	STUB_END

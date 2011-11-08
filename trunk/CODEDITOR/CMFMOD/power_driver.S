	.set noreorder  

#include "pspstub.s"

	STUB_START  "scePower_driver",0x00010011,0x00080005                          
STUB_FUNC 0x1688935C,scePowerGetBusClockFrequency                                      
STUB_FUNC 0x24085F5C,scePowerSetCpuClockFrequency                                                     
STUB_FUNC 0x9C98446E,scePowerGetBatteryLifePercent                              
STUB_FUNC 0xAF70529A,scePowerSetBusClockFrequency
STUB_FUNC 0xFDB5BFE9,scePowerGetCpuClockFrequencyInt
STUB_FUNC 0x9B1A9C5F,scePowerRequestStandby
STUB_FUNC 0xA8D09A9A,scePowerRequestSuspend
STUB_FUNC 0x9C40E184,scePowerTick
STUB_END

	.set noreorder

#include "pspimport.s"

	IMPORT_START "FAT",0x00090000

	IMPORT_FUNC  "FAT",0x0D3657EA,prx_fatread

	.set noreorder

#include "pspimport.s"


	IMPORT_START "CMF_STATE",0x00090000

	IMPORT_FUNC  "CMF_STATE",0x1349E13C,st_save

	IMPORT_FUNC  "CMF_STATE",0x06E46FA4,st_load


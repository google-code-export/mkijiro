	.set noreorder

#include "pspimport.s"

// Build files
// cmfmain_0000.o cmfmain_0001.o cmfmain_0002.o 

#ifdef F_cmfmain_0000
	IMPORT_START "cmfmain",0x00090000
#endif
#ifdef F_cmfmain_0001
	IMPORT_FUNC  "cmfmain",0x481C9ADA,malloc
#endif
#ifdef F_cmfmain_0002
	IMPORT_FUNC  "cmfmain",0xAD8AF84F,free
#endif

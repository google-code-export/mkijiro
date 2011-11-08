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
#include "smsutils.h"

void *mips_memcpy(void* dst, const void* src, unsigned int len)
{
	return memcpy(dst,src,len);
}



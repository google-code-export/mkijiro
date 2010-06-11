
#ifndef MODULE_INCLUDED
#define MODULE_INCLUDED

/*

  Module functions (somes functions taken form psplink source by Tyranid)

  By MPH (mphtheone@hotmail.com)

*/


// *** INCLUDES ***

#include <pspkernel.h>
#include <psputilsforkernel.h>
#include <pspsdk.h>
#include <string.h>


// *** STRUCTURES ***

typedef struct ModuleSyscallHeader
{
 void *unk;
 u32 basenum;
 u32 topnum;
 u32 size;
} ModuleSyscallHeader;

typedef struct ModuleFunc
{
 u32 addr;
 u32 *sysaddr;
} ModuleFunc;


// *** FUNCTIONS DECLARATIONS ***

SceUID moduleLoad (const char *, int, int);
u32 moduleLoadStart (const char *, int, int);
u32 moduleUnload (const char *);
u32 modulePatchForReload (const char *);

SceLibraryEntryTable *moduleFindLibrary (SceUID, const char *);
u32 moduleFindProc(const char* szMod, const char* szLib, u32 nid);
u32 *moduleFindFunc (SceLibraryEntryTable *, SceUID);
u32 *moduleFindSyscallFunc (u32);
u32 moduleHookAddr (u32 *, u32);
u32 moduleHookFunc (ModuleFunc *, SceUID, const char *, SceUID, void *);
u32 moduleRestoreFunc (ModuleFunc *);
u32 moduleGetFunc (ModuleFunc *, SceUID, const char *, SceUID);

#endif

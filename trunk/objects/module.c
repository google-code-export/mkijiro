
/*

  Module functions (somes functions taken form psplink source by Tyranid)

  By MPH (mphtheone@hotmail.com)

*/


// *** INCLUDES ***

#include "headers/module.h"


// *** FUNCTIONS ***

SceUID moduleLoad (const char *path, int flags, int type)
{
 SceKernelLMOption option;
 SceUID mpid;


 // If the type is 0, then load the module in the kernel partition, otherwise load it in the user partition.
 mpid = (type) ? 2 : 1;

 // Initialize structure
 memset(&option,0,sizeof(option));
 option.size = sizeof(option);
 option.mpidtext = mpid;
 option.mpiddata = mpid;
 option.position = 0;
 option.access = 1;

 return sceKernelLoadModule(path,flags,(type) ? &option : NULL);
}

u32 moduleLoadStart (const char *path, int flags, int type)
{
 SceUID loadResult, startResult;
 int status;


 loadResult = moduleLoad(path,flags,type);
 if (loadResult & 0x80000000) return 1;

 startResult = sceKernelStartModule(loadResult,0,NULL,&status,NULL);
 if (loadResult != startResult) return 2;

 return 0;
}

u32 moduleUnload (const char *name)
{
 SceModule *mod;
 SceUID modid;
 u32 ret;


 // Find module address
 mod = sceKernelFindModuleByName(name);

 if (mod)
 {
  // Stop module
  modid = mod->modid;

  ret = sceKernelStopModule(modid,0,NULL,NULL,NULL);
  if (!(ret & 0x80000000)) ret = sceKernelUnloadModule(modid);
 }
 else
  ret = SCE_KERNEL_ERROR_UNKNOWN_MODULE;

 return ret;
}

u32 moduleFindProc(const char* szMod, const char* szLib, u32 nid)
{
    SceModule* modP = sceKernelFindModuleByName(szMod);
    if (modP == NULL)
    {
        return 0;
    }
    SceLibraryEntryTable* entP = (SceLibraryEntryTable*)modP->ent_top;
    while ((u32)entP < ((u32)modP->ent_top + modP->ent_size))
    {
        if (entP->libname != NULL && strcmp(entP->libname, szLib) == 0)
        {
            // found lib
            int i;
            int count = entP->stubcount + entP->vstubcount;
            u32* nidtable = (u32*)entP->entrytable;
            for (i = 0; i < count; i++)
            {
                if (nidtable[i] == nid)
                {
                    u32 procAddr = nidtable[count+i];
                    return procAddr;
                }
            }
            return 0;
        }
        entP++;
    }
    return 0;
} 

SceLibraryEntryTable *moduleFindLibrary (SceUID modid, const char *library)
{
 SceModule *mod;
 SceLibraryEntryTable *entryTable, *entryEnd;


 // Find memory of module
 mod = sceKernelFindModuleByUID(modid);

 // If bad module
 if ((((long) mod) & 0xFF000000) != 0x88000000) return NULL;
 if ((mod->stub_top - mod->ent_top) < 40) return NULL;

 // Find entry table
 entryTable = (SceLibraryEntryTable *) ((u32 *) mod->ent_top);
 entryEnd = (SceLibraryEntryTable *) (((u8 *) mod->ent_top) + mod->ent_size);

 // Entry table loop
 while (entryTable < entryEnd)
 {
  // Find name
  if (entryTable->libname)				// first entry (module info) has name = NULL
  {
   if (!(strcmp(entryTable->libname,library))) return entryTable;
  }

  // Next entry
  entryTable = (SceLibraryEntryTable *) (((u32 *) entryTable) + entryTable->len);
 }

 // Not found
 return NULL;
}

u32 *moduleFindFunc (SceLibraryEntryTable *entryTable, SceUID nid)
{
 u32 *entry;
 int x;


 // Verify parameters
 if (!(entryTable)) return NULL;

 // Find entry table
 entry = (u32 *) entryTable->entrytable;

 // NID loop
 for (x=0;x<entryTable->stubcount;x++)
 {
  // Find function address
  if (entry[x] == nid) return &entry[x + entryTable->stubcount + entryTable->vstubcount];
 }

 return NULL;
}

u32 *moduleFindSyscallFunc (u32 func)
{
 u8 **syscall;
 ModuleSyscallHeader *sysheader;
 u32 *systable;
 int size, x;


 // Get syscall table
 asm("cfc0 %0, $12\n" : "=r"(syscall));

 // Exit if failed
 if (!(syscall)) return NULL;

 // Get syscall header
 sysheader = (ModuleSyscallHeader *) *syscall;

 // Get syscall table
 systable = (u32 *) ((*syscall) + sizeof(ModuleSyscallHeader));

 // Get syscall size
 size = (sysheader->size - sizeof(ModuleSyscallHeader)) / sizeof(u32);

 // Search function
 for (x=0;x<size;x++)
 {
  if (systable[x] == func) return &systable[x];
 }

 return NULL;
}

u32 moduleHookAddr (u32 *addr, u32 func)
{
 int x;


 // Verify parameters
 if (!(addr)) return 1;

 // Disable interrupts
 x = pspSdkDisableInterrupts();

 // Patch address
 *addr = func;

 // Apply to cache
 sceKernelDcacheWritebackInvalidateRange(addr,sizeof(addr));
 sceKernelIcacheInvalidateRange(addr,sizeof(addr));

 // Enable interrupts
 pspSdkEnableInterrupts(x);

 return 0;
}

u32 moduleHookFunc (ModuleFunc *modfunc, SceUID modid, const char *library, SceUID nid, void *func)
{
 u32 *addr;


 // Verify parameters
 if ((!(modfunc)) || (!(library)) || (!(func))) return 1;

 // Find address of function in entry table and get pointer in entry table
 addr = moduleFindFunc(moduleFindLibrary(modid,library),nid);

 // If not found
 if (!(addr)) return 2;

 // Copy address of function in structure
 modfunc->addr = *addr;

 // Find address of function in syscall table and get pointer in syscall table
 modfunc->sysaddr = moduleFindSyscallFunc(modfunc->addr);

 // If not found
 if (!(modfunc->sysaddr)) return 3;

 // Hook function (copy func address to syscall table, overwrite old func)
 return moduleHookAddr(modfunc->sysaddr,(u32) func);
}

u32 moduleRestoreFunc (ModuleFunc *modfunc)
{
 // Verify parameters
 if (!(modfunc)) return 1;

 // Restore func
 return moduleHookAddr(modfunc->sysaddr,modfunc->addr);
}

u32 moduleGetFunc (ModuleFunc *modfunc, SceUID modid, const char *library, SceUID nid)
{
 u32 *addr;


 // Verify parameters
 if ((!(modfunc)) || (!(library))) return 1;

 // Find address of function in entry table and get pointer in entry table
 addr = moduleFindFunc(moduleFindLibrary(modid,library),nid);

 // If not found
 if (!(addr)) return 2;

 // Copy address of function in structure
 modfunc->addr = *addr;
 modfunc->sysaddr = 0x0;

 return 0;
}

u32 modulePatchForReload (const char *name)
{
 SceModule *mod;
 SceLibraryEntryTable *entryTable, *entryEnd;


 // Find module address
 mod = sceKernelFindModuleByName(name);

 // If bad module
 if ((((long) mod) & 0xFF000000) != 0x88000000) return 1;
 if ((mod->stub_top - mod->ent_top) < 40) return 1;

 // Patch name and attributes
 mod->attribute = 0x1006;
 mod->modname[0] = '*';

 // Find entry table info
 entryTable = (SceLibraryEntryTable *) ((u32 *) mod->ent_top);
 entryEnd = (SceLibraryEntryTable *) (((u8 *) mod->ent_top) + mod->ent_size);

 // Entry loop
 while (entryTable < entryEnd)
 {
  // Patch lib name
  if (entryTable->libname) ((char *) entryTable->libname)[0] = '*';

  // Next entry
  entryTable = (SceLibraryEntryTable *) (((u32 *) entryTable) + entryTable->len);
 }

 return 0;
}

#include <pspkernel.h>
#include <pspsysmem_kernel.h>
#include <pspsuspend.h>
#include "allocmem.h"
#include "blend.h"
#include "usb.h"
#include "ui.h"

int _choose_alloc(int size)
{
	int free_kernel=sceKernelPartitionMaxFreeMemSize(1),free_user=sceKernelPartitionMaxFreeMemSize(6);
	
 	if(free_kernel>size+10*1024){
		if(free_user>size){
			if(usbinit || free_kernel>size+300*1024) return (free_kernel>free_user ? 6 : 1);
			else return 6;
		}
		else return 1;
	}
	if(free_user>size) return 6;
	else if(sceKernelGetModel() && sceKernelPartitionMaxFreeMemSize(8)>size) return 8;
	else return 0;
}

int choose_alloc(int size)
{
	if(_choose_alloc(size)==0) freeBG();
	return _choose_alloc(size);
}

//id���4λ��ʾ�Զ�ѡ�����ʱ�Ƿ��ͷŰ�͸������,�θ�4λ��ʾ����ռ�����,���4λ��ʾ����
void *smalloc(int size, int id)
{
	size = size + 64;
	int pid = id&0x000F;
	if(pid==0)
		pid = (id&0xF000) ? _choose_alloc(size) : choose_alloc(size);		
	if(pid==0)
		return NULL;
		
	int type;
	if(id&0x0F00)
		type = PSP_SMEM_High;
	else
		type = PSP_SMEM_Low;
		
	SceUID memID = sceKernelAllocPartitionMemory(pid, "", type, size, NULL);
	if (memID < 0)
		return NULL;
	unsigned int * lptr = sceKernelGetBlockHeadAddr(memID);
	*lptr = memID;
	return (lptr+16);
}

void sfree(void *blockaddr)
{
	if(blockaddr==NULL) return;
	SceUID blockid = *(((SceUID*)blockaddr) - 16);
	sceKernelFreePartitionMemory(blockid);
}

void *malloc(int size)
{
	return smalloc(size, 0x0000);
}

void free(void *adr) __attribute__((alias("sfree")));


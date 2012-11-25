/* 
 * Copyright (C) 2006 aeolusc <soarchin@gmail.com>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of
 * the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

#include <stdio.h>
#include <string.h>
#include <pspkernel.h>
#include <pspsysmem_kernel.h>
#include "conf.h"
#include "font.h"
#include "ui.h"
#include "mem.h"
#include "lang_zh_euc.h"
#include "layout.h"
#include "allocmem.h"
#include "minifloat.h"
#include "smsutils.h"
#define FUZZY_BUFNUM 4096*16
#define RESULT_NUM 2048

#define TABLE_ENTRY_MAX 1000

typedef struct
{
	unsigned int adr;
	unsigned int val;
}  VISUALCODE;


typedef struct
{
	int addition;  //this number will be added to the command portion
	int XOR;	   //this number will XOR'ed against the command portion
	int value;	 //this number will be added to the value portion
}  CBSEED;

//define constant, global seeds for Codebreaker codes.  16 possibilities.
static const CBSEED CBseeds[16] = {
		{ 0x288596, 0xa0b8d9b, 0x1dd9a10a },
		{ 0x37dd28, 0xa0133f8, 0xb95ab9b0 },
		{ 0x3beef1, 0xaf733ec, 0x5cf5d328 },
		{ 0xbc822,  0xa15c574, 0x95fe7f10 },
		{ 0xbc935d, 0xa50ac20, 0x8e2d6303 },
		{ 0xa139f2, 0xa920fb9, 0x16bb6286 },
		{ 0xe9bbf8, 0xa599f0b, 0xe389324c },
		{ 0xf57f7b, 0xa4aa0e3, 0x7ac6ea8  },
		{ 0x90d704, 0xa21c012, 0xaa4811d8 },
		{ 0x1814d4, 0xa906254, 0x76ce4e18 },
		{ 0xc5848e, 0xa31fd54, 0xfe447516 },
		{ 0x5b83e7, 0xa091c0e, 0xf9cd94d0 },
		{ 0x108cf7, 0xa372b38, 0x4c24dedb },
		{ 0x46ce5a, 0xa6f266c, 0x68275c4e },
		{ 0x3a5bf4, 0xa61dd4a, 0x72494382 },
		{ 0x6faffc, 0xa0dbf92, 0xc8aa88e8 }
};

static void DecryptCB(VISUALCODE* code)
{
	u32 addr = code->adr;
	u32 val = code->val;
	int cmd = (addr & 0xf0000000) >> 28;
	if(cmd > 2)
	{
		val ^= addr;
		val -= CBseeds[cmd].value;
	}
	addr ^= CBseeds[cmd].XOR;
	int temp1 = (addr & 0xff000000);
	int temp2 = (addr & 0xffffff) - CBseeds[cmd].addition;
	if(temp2 < 0)
		temp2 += 0x1000000;
	int temp3 = (temp2 & 0xff0000) >> 16;
	int temp4 = (temp2 & 0xffff) << 8;
	addr = temp1 + temp3 + temp4;
	code->adr = addr;
	code->val = val;
};

void SwapBack(VISUALCODE* code)
{
	u32 a=code->adr;
	u32 v=code->val;
	code->adr=(a&0xFF)|((v&0xFF0000)>>8)|((a&0xFF00)<<8)|(v&0xFF000000);
	code->val=((v&0xFF00)<<16)|((a&0xFF000000)>>8)|((v&0xFF)<<8)|((a&0xFF0000)>>16);
}


u32 lastbutton = 0;

static const int mem_table_num[] = {100,200,300,400,500,600,700,800,900,1000};

typedef struct _mem_gv{
	int mem_lock_idx;
	int mem_lock_count;
	p_mem_table mem_table;
	
	int mem_table_size;
	int mem_table_alloc;
	
	int inst_num;
	VISUALCODE execblock[128];
	VISUALCODE instblock[128];
}t_mem_gv;

static t_mem_gv mem_gv __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );

typedef struct _mem_search {
	int state, idx; /* 0 - stored in memory   1 - appending to file   2 - written in file */
	int sfd, bfd;
	int count, totalcount, orgcount;
	unsigned int *result, *orgres;
	unsigned int oldlow;
} t_mem_search;

static t_mem_search mem_search __attribute__(   (  aligned( 4 ), section( ".bss" )  )   );

static const char msfn[2][22] = {"ms0:/CheatMaster/tmp1", "ms0:/CheatMaster/tmp2"};

static inline void mem_set(unsigned int addr1, unsigned int addr2, int size)
{
	//addr1 &= 0xFFFFFFF;
	//addr1 |= 0x40000000;
	switch(size)
	{
	case 4:
		if((addr1 & 3) == 0)
		MEM_VALUE_LONG(addr1) = MEM_VALUE_LONG(addr2);
		break;
	case 2:
		if((addr1 & 1) == 0)
		MEM_VALUE_SHORT(addr1) = MEM_VALUE_SHORT(addr2);
		break;
	default:
		MEM_VALUE(addr1) = MEM_VALUE(addr2);
		break;
	}
}



static void mem_write_auto(int idx)
{
 	switch(mem_gv.mem_table[idx].addr >> 28)
	{
	case 1:		//1xxxxxxx
		mem_set(mem_gv.mem_table[idx].addr & 0xFFFFFFF, (unsigned int)&mem_gv.mem_table[idx].value, 2);
		break;
	case 2:		//2xxxxxxx
		mem_set(mem_gv.mem_table[idx].addr & 0xFFFFFFF, (unsigned int)&mem_gv.mem_table[idx].value, 4);
		break;
	default: 	//0xxxxxxx
		switch(mem_gv.mem_table[idx].type)
		{
		case 1:
			mem_set(mem_gv.mem_table[idx].addr, (unsigned int)&mem_gv.mem_table[idx].value, 1);
			break;
		case 2:
			mem_set(mem_gv.mem_table[idx].addr, (unsigned int)&mem_gv.mem_table[idx].value, 2);
			break;
		case 3:
			mem_set(mem_gv.mem_table[idx].addr, (unsigned int)&mem_gv.mem_table[idx].value, 4);
			break;
		default:
			if(mem_gv.mem_table[idx].value < 0x100)
				mem_set(mem_gv.mem_table[idx].addr, (unsigned int)&mem_gv.mem_table[idx].value, 1);
			else if(mem_gv.mem_table[idx].value < 0x10000)
				mem_set(mem_gv.mem_table[idx].addr, (unsigned int)&mem_gv.mem_table[idx].value, 2);
			else
				mem_set(mem_gv.mem_table[idx].addr, (unsigned int)&mem_gv.mem_table[idx].value, 4);
			break;
		}
 		break;
	} 
}

void mem_clear_instblock()
{
	int i;
	for(i=0;i<mem_gv.inst_num;i++){
		MEM_VALUE_LONG(mem_gv.instblock[i].adr) = mem_gv.instblock[i].val;
	}	
	mem_gv.inst_num = 0;
	//memset(&mem_gv.instblock[0], 0, sizeof(VISUALCODE)*128);
	mem_table_lock();
}

static void mem_write_value(int *idx)
{
	u32 i,adr,count,adr_step,val,back,idxcount;
	u32 temp;
	//char s[80];
	switch(mem_gv.mem_table[*idx].addr >> 28)
	{
	case 0x0:
	case 0x1:
	case 0x2:
		if(mem_gv.mem_table[*idx].lock)
			mem_write_auto(*idx);
		break;
		
	case 0xC:
		if(mem_gv.mem_table[*idx].lock){
			adr = mem_gv.mem_table[*idx].addr & 0xFFFFFFF;
			for(i=0;i<mem_gv.inst_num;i++){
				if(mem_gv.instblock[i].adr == adr) break;
			}
			if(i==mem_gv.inst_num){
				mem_gv.inst_num++;
				mem_gv.instblock[i].adr = adr;
				mem_gv.instblock[i].val = MEM_VALUE_LONG(adr);
				MEM_VALUE_LONG(adr) = mem_gv.mem_table[*idx].value;
			}
		}
		break;


	case 0x8:
		if(mem_gv.mem_table[*idx].lock)
		{
			adr = mem_gv.mem_table[*idx].addr & 0xFFFFFFF;
			count = mem_gv.mem_table[*idx].value >> 16;
			adr_step = mem_gv.mem_table[*idx].value & 0xFFFF;
			temp = mem_gv.mem_table[*idx+1].addr-0x8800000;
			if(temp < 0x10000000){
				val = temp & 0xFF;
				back = mem_gv.mem_table[*idx+1].value & 0xFF;
				temp = val;
				for(i=0;i<count;i++){
					//temp=(val+i*back);
					mem_set(adr, (unsigned int)&temp, 1);
					temp += back;
					adr += adr_step;
				}
			}
			else{
				adr_step = adr_step*2;
				val = temp & 0xFFFF;
				back = mem_gv.mem_table[*idx+1].value & 0xFFFF;
				temp = val;
				for(i=0;i<count;i++){
					//temp=(val+i*back);
					mem_set(adr, (unsigned int)&temp, 2);
					temp += back;
					adr += adr_step;
				}				
			}
		}
		*idx = *idx + 1;
		break;
	case 0x4:
		if(mem_gv.mem_table[*idx].lock)
		{
			adr = mem_gv.mem_table[*idx].addr & 0xFFFFFFF;
			count = mem_gv.mem_table[*idx].value >> 16;
			adr_step = (mem_gv.mem_table[*idx].value & 0xFFFF)*4;	
			val = mem_gv.mem_table[*idx+1].addr-0x8800000;
			back = mem_gv.mem_table[*idx+1].value;
			temp = val;
				for(i=0;i<count;i++){
					//temp=(val+i*back);
					mem_set(adr, (unsigned int)&temp, 4);
					temp += back;
					adr += adr_step;
				}
		}
		*idx = *idx + 1;
		break;


	case 0xD:
		if((mem_gv.mem_table[*idx].value>>28) == 0x1){
			adr = (mem_gv.mem_table[*idx].addr & 0xFF)+1;
			i=*idx+adr;
			if(mem_gv.mem_table[*idx].lock){
				temp = mem_gv.mem_table[*idx].value & 0xFFFFFFF;
				if((lastbutton & temp) == temp){
					while(*idx<i){
						*idx+=1;
						mem_write_value(idx);	
					}
				}
				else *idx = i;
			}
			else
				*idx = i;
		}
		else if((mem_gv.mem_table[*idx].value>>28) == 0x3){
			adr = (mem_gv.mem_table[*idx].addr & 0xFF)+1;
			i=*idx+adr;
			if(mem_gv.mem_table[*idx].lock){
				temp = mem_gv.mem_table[*idx].value & 0xFFFFFFF;
				if((lastbutton & temp) == temp){	
					*idx = i;
				}
				else{
					while(*idx<i){
						*idx+=1;
						mem_write_value(idx);
					}
				}
			}
			else
				*idx = i;
		}
		else if((mem_gv.mem_table[*idx].value>>28) < 0x4){
			if(mem_gv.mem_table[*idx].lock)
			{
				val = mem_gv.mem_table[*idx].value & 0xFFFF;
				adr = (mem_gv.mem_table[*idx].addr & 0xFFFFFFF);
				//adr |= 0x40000000;
				back = (mem_gv.mem_table[*idx].value>>16)&0x00F0;
				if
				(
				((mem_gv.mem_table[*idx].value>>28)==0 && ((back==0&&*((u16 *)adr)==(u16)(val))||(back==0x10&&*((u16 *)adr)!=(u16)(val))||(back==0x20&&*((u16 *)adr)<(u16)(val))||(back==0x30&&*((u16 *)adr)>(u16)(val))))
				||
				((mem_gv.mem_table[*idx].value>>28)==2 && ((back==0&&*((u8 *)adr)==(u8)(val))||(back==0x10&&*((u8 *)adr)!=(u8)(val))||(back==0x20&&*((u8 *)adr)<(u8)(val))||(back==0x30&&*((u8 *)adr)>(u8)(val))))				
				){
					*idx+=1;
					mem_write_value(idx);
				}
				else
					*idx = *idx + 1;
			}
			else *idx = *idx + 1;
		}
		else{
			count = mem_gv.mem_table[*idx+1].addr-0x8800000 + 1;
			i=*idx+count;
			if(mem_gv.mem_table[*idx].lock)
			{
				adr = (mem_gv.mem_table[*idx].addr & 0xFFFFFFF);
				adr_step = (mem_gv.mem_table[*idx].value & 0xFFFFFFF) + 0x8800000;
				back = mem_gv.mem_table[*idx+1].value;
				temp = mem_gv.mem_table[*idx].value>>28;

				*idx+=1;
				if
				(
				(temp==0x4 && ((back==0 && *((u8 *)adr)==*((u8 *)adr_step))||(back==1 && *((u16 *)adr)==*((u16 *)adr_step))||(back==2 && *((u32 *)adr)==*((u32 *)adr_step))))
				||
				(temp==0x5 && ((back==0 && *((u8 *)adr)!=*((u8 *)adr_step))||(back==1 && *((u16 *)adr)!=*((u16 *)adr_step))||(back==2 && *((u32 *)adr)!=*((u32 *)adr_step))))
				||
				(temp==0x6 && ((back==0 && *((u8 *)adr)<*((u8 *)adr_step))||(back==1 && *((u16 *)adr)<*((u16 *)adr_step))||(back==2 && *((u32 *)adr)<*((u32 *)adr_step))))
				||
				(temp==0x7 && ((back==0 && *((u8 *)adr)>*((u8 *)adr_step))||(back==1 && *((u16 *)adr)>*((u16 *)adr_step))||(back==2 && *((u32 *)adr)>*((u32 *)adr_step))))
				)
				{
					while(*idx<i){
						*idx+=1;
						mem_write_value(idx);						
					}
				}
				else *idx = i;
			}
			else *idx = i;
		}
		break;


	case 0xE:
		temp = mem_gv.mem_table[*idx].addr-0x8800000;
		count = (temp >>16) & 0xFF;
		val = temp & 0xFFFF;
		i=*idx+count;

		if(mem_gv.mem_table[*idx].lock)
		{
			adr = (mem_gv.mem_table[*idx].value & 0xFFFFFFF) + 0x8800000;
			//adr |= 0x40000000;
			back = mem_gv.mem_table[*idx].value >>28;
			temp = temp >> 24;
				if
				(
				(temp==0xE0 && ((back==0x0&&*((u16 *)adr)==val)||(back==0x1&&*((u16 *)adr)!=val)||(back==0x2&&*((u16 *)adr)< val)||(back==0x3&&*((u16 *)adr)> val)))
				||
				(temp==0xE1 && ((back==0x0&&*((u8 *)adr)==val)||(back==0x1&&*((u8 *)adr)!=val)||(back==0x2&&*((u8 *)adr)< val)||(back==0x3&&*((u8 *)adr)> val)))
				)
				{
					while(*idx<i){
						*idx+=1;
						mem_write_value(idx);						
					}
				}
				else *idx=i;
		}
		else *idx=i;
		break;
		
		
	case 0x5:
		if(mem_gv.mem_table[*idx].lock)
		{
			val = mem_gv.mem_table[*idx].addr & 0xFFFFFFF;
			count = mem_gv.mem_table[*idx].value;
			adr = mem_gv.mem_table[*idx+1].addr;
			//adr |= 0x40000000;
			memmove((u8 *)adr, (u8 *)val, count);
		}
		*idx = *idx + 1;
		break;
		

	case 0x7:
		if(mem_gv.mem_table[*idx].lock)
		{
			adr = mem_gv.mem_table[*idx].addr & 0xFFFFFFF;
			val = mem_gv.mem_table[*idx].value & 0xFFFF;
			//adr |= 0x40000000;
			switch(mem_gv.mem_table[*idx].value >> 16)
			{
			case 0x0:
				MEM_VALUE(adr) |= (u8)val;
				break;
			case 0x2:
				MEM_VALUE(adr) &= (u8)val;
				break;
			case 0x4:
				MEM_VALUE(adr) ^= (u8)val;
				break;
			case 0x1:
				if((adr & 1) == 0)
				MEM_VALUE_SHORT(adr) |= (u16)val;
				break;
			case 0x3:
				if((adr & 1) == 0)
				MEM_VALUE_SHORT(adr) &= (u16)val;
				break;
			default:
				if((adr & 1) == 0)
				MEM_VALUE_SHORT(adr) ^= (u16)val;
				break;
			}
		}
		break;

		
	case 0x3:
		back = mem_gv.mem_table[*idx].addr-0x8800000;
		adr = mem_gv.mem_table[*idx].value + 0x8800000;
		//adr |= 0x40000000;
		if((back>>16) < 0x3050){
			if(mem_gv.mem_table[*idx].lock){
				val = back & 0xFFFF;
				switch(back>>16)
				{
				case 0x3010:
					MEM_VALUE(adr) += (u8)val;
					break;
				case 0x3020:
					MEM_VALUE(adr) -= (u8)val;
					break;
				case 0x3030:
					if((adr & 1) == 0)
					MEM_VALUE_SHORT(adr) += val;
					break;
				default:
					if((adr & 1) == 0)
					MEM_VALUE_SHORT(adr) -= val;
					break;
				}
			}
		}
		else{
			if(mem_gv.mem_table[*idx].lock){
				val = mem_gv.mem_table[*idx+1].addr-0x8800000;
				switch(back>>16)
				{
				case 0x3050:
					if((adr & 3) == 0)
					MEM_VALUE_LONG(adr) += val;
					break;
				default:
					if((adr & 3) == 0)
					MEM_VALUE_LONG(adr) -= val;
					break;
				}			
			}
			*idx = *idx + 1;
		}			
		break;

		
	case 0x6:
		idxcount = *idx + 1;
		count = mem_gv.mem_table[idxcount].addr & 0xFFFF;		
		if(mem_gv.mem_table[*idx].lock)
		{
			adr = mem_gv.mem_table[*idx].addr & 0xFFFFFFF;
			val = mem_gv.mem_table[*idx].value;
			
			u32 adr_offset = mem_gv.mem_table[idxcount].value;
					back = mem_gv.mem_table[idxcount].addr >> 16;
					back = back & 0xF;
			if(count>=2)
			{
				temp = (mem_gv.mem_table[*idx+2].addr & 0xFFFFFFF) - 0x8800000;
				u32 qqq = (mem_gv.mem_table[idxcount].addr >> 20) - 0x88;
				qqq *= 4;				
				if((mem_gv.mem_table[*idx+2].addr>>28) == 1)
				{
					back = adr + qqq;
					back = * ((u32 *)back);
					adr =  * ((u32 *)adr);
					back += temp;
					adr += adr_offset;
					if(adr>0x8800000 && adr<0xa000000 && back>0x8800000 && back<0xa000000)
						memmove((u8 *)back, (u8 *)adr, val);
					goto PTROUT;
				}
				else if((mem_gv.mem_table[*idx+2].addr>>28) == 9)
				{
					adr_step = mem_gv.mem_table[*idx+2].value;
					u32 ptradr = adr;
					
						if(back<0x3){
							temp *= 1<<back;
						}
						else{
							temp *= 1<<(back-3);
						}
					for(i=0;i<count;i++)
					{
						adr =  * ((u32 *)ptradr);					
						if(back<0x3){
							adr += adr_offset;
						}
						else{
							adr -= adr_offset;
						}
						adr_offset += temp;
						if((adr)>0x8800000 && (adr)<0xa000000){//保证是有效的地址
							switch(back)
							{
							case 0x0:
							case 0x3:
								mem_set(adr, (unsigned int)&val, 1);
								break;
							case 0x1:
							case 0x4:
								mem_set(adr, (unsigned int)&val, 2);
								break;
							default:
								mem_set(adr, (unsigned int)&val, 4);
								break;
							}
						}					
						val += adr_step;
						ptradr += qqq;					
					}
					goto PTROUT;
				}
				else
				{			
					for(i=2;i<=count;i++)
					{
						int ic = *idx+1+((i&0xFE)>>1);
						if((i&1)){
							temp = mem_gv.mem_table[ic].value>>28;
							adr_step = mem_gv.mem_table[ic].value&0x0FFFFFFF;
						}
						else{
							temp = mem_gv.mem_table[ic].addr>>28;
							adr_step = (mem_gv.mem_table[ic].addr-0x8800000)&0x0FFFFFFF;
						}
							adr =  * ((u32 *)adr);
						if(adr>0x8800000 && adr<0xa000000)
						{
							if(temp==2)	adr = adr+adr_step;
							else adr = adr-adr_step;
						}
						else goto PTROUT;
					}
				}
			}
				adr =  * ((u32 *)adr);
			if(adr>0x8800000 && adr<0xa000000)
			{
				if(back<3) adr += adr_offset;
				else adr -= adr_offset;
				switch(back)
				{
				case 0x0:
				case 0x3:
					mem_set(adr, (unsigned int)&val, 1);
					break;
				case 0x1:
				case 0x4:
					mem_set(adr, (unsigned int)&val, 2);
					break;
				default:
					mem_set(adr, (unsigned int)&val, 4);
					break;
				}
			}
		}
PTROUT:
		if(count>=2){
			u32 flag;
			flag = mem_gv.mem_table[*idx+2].addr >> 28;
			if(flag==2 || flag==3) idxcount += count/2;
			else idxcount++;
		}
		*idx = idxcount;
		break;
		
		
	case 0xF:
		idxcount = (mem_gv.mem_table[*idx].addr & 0xFF);
		temp = mem_gv.mem_table[*idx].value & 0xF;
		if(mem_gv.mem_table[*idx].lock)
		{
			VISUALCODE *ptr_exec = mem_gv.execblock;
			p_mem_table ptr_code = &(mem_gv.mem_table[*idx+1]);
			for(i=1;i<=idxcount;i++)
			{
				ptr_exec->adr = ptr_code->addr - 0x08800000;
				ptr_exec->val = ptr_code->value;
				if(temp){
					DecryptCB(ptr_exec);
				}
				if(temp==2){
					SwapBack(ptr_exec);
					DecryptCB(ptr_exec);
				}
				ptr_exec++;ptr_code++;
			}
			
			sceKernelDcacheWritebackInvalidateRange(mem_gv.execblock,256);
			sceKernelIcacheInvalidateRange(mem_gv.execblock,256);
			
			asm __volatile__ ("jalr	%0"::"r"(mem_gv.execblock):"t0","t1","t2","t3","t4","t5","t6","t7","t8","t9","ra");
		}
		
		*idx = *idx + idxcount;
		break;
	
		
	default:
		break;
	}

}

extern void mem_dump(u32 low, u32 high)
{
	int fd;
	char fn[128];
	int mem_fcount = 0;
	sceIoMkdir(MEM_DIR, 0777);
	do {
		++ mem_fcount;
	if (high==0xFFFFFFFF){
	sprintf(fn, "%s/0x%07X.mem", MEM_DIR, (unsigned int)low);
	}
	else{
		sprintf(fn, "%s/0x%07X_0x%07X_%02d.mem", MEM_DIR, (unsigned int)low, (unsigned int)high, mem_fcount);
	}
		fd = sceIoOpen(fn, PSP_O_RDONLY, 0777);
		if(fd >= 0)
			sceIoClose(fd);
		else
			break;
	} while(1);
	fd = sceIoOpen(fn, PSP_O_CREAT | PSP_O_WRONLY | PSP_O_TRUNC, 0777);
	if (high==0xFFFFFFFF){
	sceIoWrite(fd, (void *)(0x08800000),0x1800000);
	}
	else{
	sceIoWrite(fd, (void *)(0x08800000+low), high-low);
	}
	sceIoClose(fd);
}

/* extern void mem_search_init()
{
	sceIoMkdir(MODULE_DIR, 0777);
	memset(&mem_search, 0, sizeof(t_mem_search));
	mem_search.sfd = mem_search.bfd = -1;
	sceIoRemove(msfn[0]);
	sceIoRemove(msfn[1]);
} */

static int mem_add_result(unsigned int addr)
{
	mem_search.result[mem_search.count ++] = addr;
	mem_search.totalcount ++;
	if(mem_search.count == RESULT_NUM)		//每1024个搜索结果写一次文件
	{
		switch(mem_search.state)
		{
		case 0:
			mem_search.sfd = sceIoOpen(msfn[mem_search.idx], PSP_O_CREAT | PSP_O_WRONLY | PSP_O_TRUNC, 0777);
			if(mem_search.sfd < 0)
			{
				mem_search.state = 0;
				mem_search.totalcount = 0;
				return -1;
			}
			mem_search.state = 1;
		case 1:
			sceIoWrite(mem_search.sfd, mem_search.result, sizeof(unsigned int) * mem_search.count);
			mem_search.count = 0;
			break;
		}
	}
	return 0;
}

static void mem_add_finish()
{
	if(mem_search.state > 2)
	{
		mem_search.count = 0;
		mem_search.state = 4;
	}
	else if(mem_search.state > 0)
	{
		sceIoWrite(mem_search.sfd, mem_search.result, sizeof(unsigned int) * mem_search.count);
		mem_search.totalcount += mem_search.count;
		mem_search.count = 0;
		mem_search.state = 2;
	}
	if(mem_search.sfd >= 0)
	{
		sceIoClose(mem_search.sfd);
		mem_search.sfd = -1;
	}
	if(mem_search.bfd >= 0)
	{
		sceIoClose(mem_search.bfd);
		mem_search.bfd = -1;
	}
}

static int mem_byte_eq(unsigned int addr, unsigned int v)
{
	return (MEM_VALUE(addr) == (unsigned char)v);
}

static int mem_word_eq(unsigned int addr, unsigned int v)
{
	return (MEM_VALUE_SHORT(addr) == (unsigned short)v);
}

static int mem_dword_eq(unsigned int addr, unsigned int v)
{
	return (MEM_VALUE_LONG(addr) == v);
}


static int mem_byte_cmp(unsigned int addr1, unsigned char addr2, int flag, unsigned char diff)
{
	unsigned char data1 = MEM_VALUE(addr1);
	
	{
		int ret = (data1 > addr2) ? 4 : ((data1 < addr2) ? 2 : 1);
		ret = ret&flag;	
		if(diff){
			if(ret==4 && diff==(data1-addr2)) return ret;
			if(ret==2 && diff==(addr2-data1)) return ret;
			return 0;
		}
		else return ret;
	}
}
static int mem_byte_init_cmp(unsigned int addr1, unsigned char min, unsigned char max)
{
	unsigned char data1 = MEM_VALUE(addr1);
	return (data1>=min && data1<=max);
}

static int mem_word_cmp(unsigned int addr1, unsigned short addr2, int flag, unsigned short diff)
{
	unsigned short data1 = MEM_VALUE_SHORT(addr1);
	
	{
		int ret = (data1 > addr2) ? 4 : ((data1 < addr2) ? 2 : 1);
		ret = ret&flag;	
		if(diff){
			if(ret==4 && diff==(data1-addr2)) return ret;
			if(ret==2 && diff==(addr2-data1)) return ret;
			return 0;
		}
		else return ret;
	}
}
static int mem_word_init_cmp(unsigned int addr1, unsigned short min, unsigned short max)
{
	unsigned short data1 = MEM_VALUE_SHORT(addr1);
	return (data1>=min && data1<=max);
}

static int mem_dword_cmp(unsigned int addr1, unsigned int addr2, int flag, unsigned int diff)
{
	unsigned int data1 = MEM_VALUE_LONG(addr1);
	
	{
		int ret = (data1 > addr2) ? 4 : ((data1 < addr2) ? 2 : 1);
		ret = ret&flag;	
		if(diff){
			if(ret==4 && diff==(data1-addr2)) return ret;
			if(ret==2 && diff==(addr2-data1)) return ret;
			return 0;
		}
		else return ret;
	}
}
static int mem_dword_init_cmp(unsigned int addr1, unsigned int min, unsigned int max)
{
	unsigned int data1 = MEM_VALUE_LONG(addr1);
	return (data1>=min && data1<=max);
}

static int mem_float_cmp(unsigned int addr1, unsigned int addr2, int flag, unsigned int diff)
{
	unsigned int data1 = FloatInt((unsigned char *)addr1);
	unsigned int data2 = FloatInt((unsigned char *)addr2);
	
	{
		int ret = (data1 > data2) ? 4 : ((data1 < data2) ? 2 : 1);
		ret = ret&flag;
		if(diff){
			if(ret==4 && diff==(data1-data2)) return ret;
			if(ret==2 && diff==(data2-data1)) return ret;
			return 0;
		}
		else return ret;
	}
}
static int mem_float_init_cmp(unsigned int addr1, unsigned int min, unsigned int max)
{
	unsigned int data1 = FloatInt((unsigned char *)addr1);
	return (data1>=min && data1<=max);
}

static int mem_search_normal(unsigned int value, int (*eq_func)(unsigned int addr, unsigned int v), unsigned int low, unsigned int high, int apx)
{
	unsigned int addr, end = 0x08800000 + high;
	switch(mem_search.state)
	{
	case 0:
		if(mem_search.totalcount == 0)
		{
			for(addr = (low + 0x08800000 + apx - 1) & ~(apx - 1); addr < end; addr += apx)
				if(eq_func(addr, value) && mem_add_result(addr) < 0)
				{
					mem_search_finish();
					return -1;
				}
			mem_add_finish();
		}
		else		//上次搜索结果未超过RESULT_NUM,完全存在内存里
		{
			mem_search.orgcount = mem_search.totalcount;
			mips_memcpy(mem_search.orgres, mem_search.result, mem_search.orgcount * sizeof(unsigned int));
			mem_search.totalcount = mem_search.count = 0;
			int i;
			for(i = 0; i < mem_search.orgcount; i ++)
				if(mem_search.orgres[i] < end && ((mem_search.orgres[i]&(apx-1)) == 0) && eq_func(mem_search.orgres[i], value) && mem_add_result(mem_search.orgres[i]) < 0)
				{
					mem_search_finish();
					return -1;
				}
			mem_add_finish();
		}
		break;
	case 2:			//上次搜索结果存在文件里
		mem_search.state = 0;
		mem_search.count = mem_search.totalcount = 0;
		mem_search.bfd = sceIoOpen(msfn[mem_search.idx], PSP_O_RDONLY, 0777);

		if(mem_search.bfd < 0)
		{
			mem_search_finish();
			return -1;
		}

		mem_search.idx = 1 - mem_search.idx;
		while((mem_search.orgcount = sceIoRead(mem_search.bfd, mem_search.orgres, RESULT_NUM * sizeof(unsigned int)) / sizeof(unsigned int)) > 0)
		{
			int i;
			for(i = 0; i < mem_search.orgcount; i ++)
				if(mem_search.orgres[i] < end && ((mem_search.orgres[i]&(apx-1)) == 0) && eq_func(mem_search.orgres[i], value) && mem_add_result(mem_search.orgres[i]) < 0)
				{
					mem_search_finish();
					return -1;
				}
		}
		mem_add_finish();
		break;
	}
	if(mem_search.totalcount == 0)
		mem_search_finish();
	return mem_search.totalcount;
}

static void mem_search_fuzzy_result(unsigned int wv, unsigned int i, int apx)
{
					if((wv & 0x70000000) > 0)
					{
						if(mem_search.count >= (RESULT_NUM/2))
						{
							sceIoWrite(mem_search.sfd, mem_search.result, sizeof(unsigned int) * RESULT_NUM);
							mem_search.totalcount += mem_search.count;
							mem_search.count = 0;
						}
						mem_search.result[mem_search.count * 2] = wv;
						switch(apx)
						{
						case 1:
							mem_search.result[mem_search.count * 2 + 1] = MEM_VALUE(i);
							break;
						case 2:
							mem_search.result[mem_search.count * 2 + 1] = MEM_VALUE_SHORT(i);
							break;
						case 4:
							mem_search.result[mem_search.count * 2 + 1] = MEM_VALUE_LONG(i);
							break;
						default:
							if((i & 3) == 0)
								mem_search.result[mem_search.count * 2 + 1] = MEM_VALUE_LONG(i);
							else if((i & 1) == 0)
								mem_search.result[mem_search.count * 2 + 1] = MEM_VALUE_SHORT(i);
							else
								mem_search.result[mem_search.count * 2 + 1] = MEM_VALUE(i);
							break;
						}
						mem_search.count ++;
					}
}

extern int mem_search_fuzzy(int diff, unsigned int low, unsigned int high, int apx, unsigned int compareval, unsigned int fuzzy_diff)
{
	int bakapx = apx;
	int bakdiff = diff;
	if (apx>=4) apx = 4;
	if (diff<0) diff = (-diff);
	
	unsigned int difing;
	if(fuzzy_diff && (diff==4 || diff==2))
		difing = fuzzy_diff;
	else difing = 0;
	
	unsigned int comparebyte,compareword;
	comparebyte = compareval>=0xff ? 0xff: compareval;
	compareword = compareval>=0xffff ? 0xffff: compareval;
	
	unsigned int float_intmin,float_intmax;
	float_intmin = FloatInt(&compareval);
	float_intmax = FloatInt(&difing);
	
	low = (low + 3) & ~3;
	high = (high + 3) & ~3;
	switch(mem_search.state)
	{
	case 3:
		mem_search.count = mem_search.totalcount = 0;
		mem_search.bfd = sceIoOpen(msfn[mem_search.idx], PSP_O_RDONLY, 0777);

		if(mem_search.bfd < 0)
		{
			mem_search_finish();
			return -1;
		}

		mem_search.idx = 1 - mem_search.idx;
		mem_search.sfd = sceIoOpen(msfn[mem_search.idx], PSP_O_CREAT | PSP_O_WRONLY | PSP_O_TRUNC, 0777);
		if(mem_search.sfd < 0)
		{
			mem_search.state = 0;
			mem_search.totalcount = 0;
			return -1;
		}

		unsigned int ramoffset=0;
		int size;

		unsigned char *fuzzy_buf = malloc(FUZZY_BUFNUM);
		if(fuzzy_buf==NULL) goto fuzzy_OUT;
		
		do{			
			size = sceIoRead(mem_search.bfd, fuzzy_buf, FUZZY_BUFNUM);
			unsigned int i= ramoffset + 0x08800000 + mem_search.oldlow;
			int j=0;
			
			for(; (j < size)&&(i < 0x0A000000); (apx==0)? (i++,j++):(i+=apx,j+=apx))
				{
					if(i>=(unsigned int)fuzzy_buf && i<(unsigned int)fuzzy_buf+FUZZY_BUFNUM) continue;
					unsigned int wv = i;
					switch(apx)
					{
					case 1:
						if(mem_byte_cmp(i, fuzzy_buf[j], diff, difing))
							wv |= 0x10000000;					
						break;
					case 2:
						if(j<size-1 && mem_word_cmp(i, *(unsigned short*)(&fuzzy_buf[j]), diff, difing))
							wv |= 0x20000000;						
						break;
					case 4:
						if(j<size-3){
							if(bakapx == 4){
								if(mem_dword_cmp(i, *(unsigned int*)(&fuzzy_buf[j]), diff, difing))
									wv |= 0x40000000;
							}
							else{
								if(mem_float_cmp(i, (unsigned int)&fuzzy_buf[j], diff, float_intmax))
									wv |= 0x40000000;
							}
						}
						break;
					default:
						if(mem_byte_cmp(i, fuzzy_buf[j], diff, difing))
							wv |= 0x10000000;
						if((i & 1) == 0 && j < size - 1 && mem_word_cmp(i, *(unsigned short*)(&fuzzy_buf[j]), diff, difing))
							wv |= 0x20000000;
						if((i & 3) == 0 && j < size - 3 && mem_dword_cmp(i, *(unsigned int*)(&fuzzy_buf[j]), diff, difing))
							wv |= 0x40000000;
						break;
					}
					
					mem_search_fuzzy_result(wv, i, apx);
				}
				
			ramoffset+=FUZZY_BUFNUM;
		}while(size==FUZZY_BUFNUM);
		
		sfree(fuzzy_buf);
fuzzy_OUT:		
		if(mem_search.count > 0)
		{
			sceIoWrite(mem_search.sfd, mem_search.result, sizeof(unsigned int) * mem_search.count * 2);
			mem_search.totalcount += mem_search.count;
			mem_search.count = 0;
		}
		mem_add_finish();
		mem_search.state = 4;
		break;
	case 4:
		mem_search.count = mem_search.totalcount = 0;
		mem_search.bfd = sceIoOpen(msfn[mem_search.idx], PSP_O_RDONLY, 0777);

		if(mem_search.bfd < 0)
		{
			mem_search_finish();
			return -1;
		}

		mem_search.idx = 1 - mem_search.idx;
		mem_search.sfd = sceIoOpen(msfn[mem_search.idx], PSP_O_CREAT | PSP_O_WRONLY | PSP_O_TRUNC, 0777);

		if(mem_search.sfd < 0)
		{
			mem_search_finish();
			return -1;
		}
		
		while((mem_search.orgcount = sceIoRead(mem_search.bfd, mem_search.orgres, RESULT_NUM * sizeof(unsigned int)) / sizeof(unsigned int)) > 0)
		{
			int i;
			for(i = 0; i < mem_search.orgcount; i += 2)
			{
				unsigned int bo = mem_search.orgres[i] >> 28, addr = mem_search.orgres[i] & 0x0FFFFFFF, wv = addr, vl = mem_search.orgres[i + 1];
				if(bakdiff<0) vl = comparebyte;
				if((bo & 1) > 0 && (mem_byte_cmp(addr, vl, diff, difing)))
					wv |= 0x10000000;
				if(bakdiff<0) vl = compareword;
				if((bo & 2) > 0 && (mem_word_cmp(addr, vl, diff, difing)))
					wv |= 0x20000000;
				if(bakdiff<0) vl = compareval;
				if((bo&4) > 0){
							if(bakapx != 8){
								if(mem_dword_cmp(addr, vl, diff, difing))
									wv |= 0x40000000;
							}
							else{
								if(mem_float_cmp(addr, (unsigned int)&vl, diff, float_intmax))
									wv |= 0x40000000;
							}				
				}
				
				mem_search_fuzzy_result(wv, addr, apx);
			}
		}
		if(mem_search.count > 0)
		{
			sceIoWrite(mem_search.sfd, mem_search.result, sizeof(unsigned int) * mem_search.count * 2);
			mem_search.totalcount += mem_search.count;
			mem_search.count = 0;
		}
		mem_add_finish();
		break;
	case 0:
		mem_search.oldlow = low;
		sceKernelDelayThread(100000);
		mem_search.sfd = sceIoOpen(msfn[mem_search.idx], PSP_O_CREAT | PSP_O_WRONLY | PSP_O_TRUNC, 0777);
		if(mem_search.sfd < 0)
		{
			mem_search.state = 0;
			mem_search.totalcount = 0;
			return -1;
		}
		
		if(compareval==0){
			if((high - low) <= 516) high = low + 516;
			unsigned int size = high - low;
			sceIoWrite(mem_search.sfd, (void *)(0x08800000 + low), size);
			sceIoClose(mem_search.sfd);
			mem_search.state = 3;
			mem_search.totalcount = size;
		}
		else{
			mem_search.count = mem_search.totalcount = 0;
			low = (0x08800000 + low);
			high = (0x08800000 + high);
			
			unsigned int difingbyte,difingword;
			difingbyte = difing>=0xff ? 0xff: difing;
			difingword = difing>=0xffff ? 0xffff: difing;			
			
			unsigned int i;
			for(i=low;i<high;(apx==0) ? (i++):(i+=apx))
			{
					unsigned int wv = i;
					switch(apx)
					{
					case 1:
						if(mem_byte_init_cmp(i, comparebyte, difingbyte))
							wv |= 0x10000000;					
						break;
					case 2:
						if(i<high-1 && (mem_word_init_cmp(i, compareword, difingword)))
							wv |= 0x20000000;						
						break;
					case 4:
						if(i<high-3){
							if(bakapx == 4){
								if(mem_dword_init_cmp(i, compareval, difing))
									wv |= 0x40000000;
							}
 							else{
								if(mem_float_init_cmp(i, float_intmin, float_intmax))
									wv |= 0x40000000;
							} 
						}
						break;
					default:
						if(mem_byte_init_cmp(i, comparebyte, difingbyte))
							wv |= 0x10000000;
						if((i & 1) == 0 && i<high-1 && (mem_word_init_cmp(i, compareword, difingword)))
							wv |= 0x20000000;
						if((i & 3) == 0 && i<high-3 && (mem_dword_init_cmp(i, compareval, difing)))
							wv |= 0x40000000;
						break;
					}					
					mem_search_fuzzy_result(wv, i, apx);				
			}
			
			if(mem_search.count > 0)
			{
				sceIoWrite(mem_search.sfd, mem_search.result, sizeof(unsigned int) * mem_search.count * 2);
				mem_search.totalcount += mem_search.count;
				mem_search.count = 0;
			}
			mem_add_finish();
			mem_search.state = 4;		
		}
		break;
	}
	if(mem_search.totalcount <= 512)
	{
		int i;
		for (i = 0; i < mem_search.totalcount; i ++)
			mem_search.result[i] = mem_search.result[2 * i] & 0x0FFFFFFF;
	}
	return mem_search.totalcount;
}

extern int mem_search_byte(unsigned int value, unsigned int low, unsigned int high)
{
	return mem_search_normal(value, mem_byte_eq, low, high, 1);
}

extern int mem_search_word(unsigned int value, unsigned int low, unsigned int high)
{
	return mem_search_normal(value, mem_word_eq, low, high, 2);
}

extern int mem_search_dword(unsigned int value, unsigned int low, unsigned int high)
{
	return mem_search_normal(value, mem_dword_eq, low, high, 4);
}

extern int mem_search_value(unsigned int value, unsigned int low, unsigned int high)
{
	if(value < 0x100)
		return mem_search_byte(value, low, high);
	if(value < 0x10000)
		return mem_search_word(value, low, high);
	return mem_search_dword(value, low, high);
}

extern int mem_search_get_result(unsigned int ** result)
{
/*  	if((mem_search.state > 0 && mem_search.state < 3) || mem_search.totalcount > 512)
		return -1; */
	* result = mem_search.result;
	return mem_search.totalcount;
}
/* 
extern void mem_search_null()
{
	mem_search.result = NULL;
}
 */
extern void mem_search_finish()
{
	mem_add_finish();
	//sceIoRemove(msfn[0]);
	//sceIoRemove(msfn[1]);
	sfree(mem_search.result);
	memset(&mem_search, 0, sizeof(mem_search));
	mem_search.sfd = mem_search.bfd = -1;
	mem_search.result = NULL;
}

extern void mem_search_init()
{
	mem_search_finish();
	mem_search.result = smalloc(RESULT_NUM*4 *2,0x0001);
	mem_search.orgres = mem_search.result + RESULT_NUM;
	memset((unsigned char *)mem_search.result, 0, RESULT_NUM*4 *2);	
}

extern void mem_table_index_init(const char * gname, int spd)
{
	mem_gv.mem_lock_count = spd;
	mem_gv.mem_table = smalloc(sizeof(t_mem_table) * 100, 0x0F01);
	memset(mem_gv.mem_table, 0, sizeof(t_mem_table) * 100);
	
	int i;
	char fn[128];
	char name[40];
	
	int tabload = 0;
	
	for(i = 0; i < MEMTABLE_INDEXMAX; i ++)
	{
		sprintf(fn, "%s/%02d.tab", TAB_DIR, i);
		int fd = sceIoOpen(fn, PSP_O_RDONLY, 0777);
		if(fd>=0)
		{
			sceIoRead(fd, name, 36);
			sceIoClose(fd);
			if(config.autoload && memcmp(name, gname, 10) == 0)
			{
				mem_table_load(i, 1);
				tabload = !tabload;
				break;
			}
		}
	}
	
	if(config.autoloadcmf && tabload==0)
	{
		if(layout_autoload_dir(gname, CMF_DIR, fn)==0)
			convert_cmf(fn);
	}
}

extern void mem_table_save(int idx)
{
	char fn[80];
	if(mem_gv.mem_table_size==0) return;	
	ui_cls();	
	if(ui_input_string(120, 68, ui_get_gamename(), 36) < 1)
		return ;
	
	sprintf(fn, "%s/%02d.tab", TAB_DIR, idx);
	int fd = sceIoOpen(fn, PSP_O_WRONLY | PSP_O_CREAT | PSP_O_TRUNC, 0777);
	if(fd < 0)
		return ;

	sceIoWrite(fd, ui_get_gamename(), 36);
	sceIoWrite(fd, &mem_gv.mem_table_size, sizeof(int));
	int i;
	for(i = 0; i < mem_gv.mem_table_size; i ++)
		sceIoWrite(fd, &mem_gv.mem_table[i], sizeof(unsigned int) * 2 + sizeof(int) * 2);
	for(i = 0; i < mem_gv.mem_table_size; i ++)
		sceIoWrite(fd, mem_gv.mem_table[i].name, 10);
	sceIoClose(fd);
}

//DOS_STRING_KILLER
static char extra_ch[9] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )={'|','+',':','*','?','<','>','/','\\'};
extern void filter_filename(char *s)
{
	int i,j;
	i = 0;
	char filename_encode=FILE_ENCODE();
	while(s[i]!=0)
	{
		//SJISスキップ
		if((filename_encode==1)&&((u8)((s[i] ^ 0x20)-0xA1) <(u8)0x3C)){
			//SJIS认跋嘲
			if((((s[i+1]+0xC0)&0xFF)<=0xBC) && (s[i+1]!=0x7F)){
			}
			else{
			s[i]=0x81;
			s[i+1]=0xA0;
			}
		i+=2;
		}
		else{
		for(j=0;j<9;j++)
		{
			if(s[i]==extra_ch[j]){
				s[i]=' ';
				break;
			}
		}
		if((u8)s[i] < (u8)0x20){
			s[i]=' ';
		}
		i++;
		}
	}
}

static unsigned int mem_table_ConvertTabType(p_mem_table p)
{
	unsigned int adr = p->addr - 0x08800000;
	if(adr<0x01800000 && adr>=0){
		switch(p->type)
		{
		case 2:
			adr |= 0x10000000;
			break;
		case 3:
			adr |= 0x20000000;
			break;
		default:
			break;
		}
	}
	return adr;
}


extern void mem_table_savecw()
{
	char s[300];
	char fn[80];
	if(mem_gv.mem_table_size==0) return;
	sceIoMkdir(CMF_DIR, 0777);
	mips_memcpy(fn, ui_get_gamename(), 29);
	fn[28]=0;
	fn[29]=0;
	ui_cls();
	if(ui_input_string(120, 68, fn, 29) < 1)
		return ;

	char cmf[]=".cmf\x0";
	int i=0;
	int j=0;
	int fd;
	
	EUC_UTF8SJIS(fn,40);
	strcat(fn,cmf);

	filter_filename(fn); //DOSKILLER

	sprintf(s, "%s/%s", CMF_DIR, fn);


	fd = sceIoOpen(s, PSP_O_WRONLY | PSP_O_CREAT | PSP_O_TRUNC, 0777);
	if(fd < 0) return;
	
#define WRITE_BUFFER 2048
	
	char *p= malloc(WRITE_BUFFER);
	char *p_backup;
	p_backup=p;
	
		mips_memcpy(fn,ui_get_gamename(),10);
		fn[10]=0;
	
		sprintf(p,"_S %s\r\n",fn);
		p+=strlen(p);
	
		mips_memcpy(fn,ui_get_gamename()+12,64);
		fn[64]=0;	
		sprintf(p,"_G %s\r\n",fn);
		p+=strlen(p);
	
	/*	if(enc){
		sceid2cfid(fn,ui_get_gamename());
		
		sprintf(p,"_E 0x%s 0x00000020\r\n",fn);
		mips_memcpy(p+16,ui_get_gamename()+5,5);
		p+=strlen(p);
			
		}*/
	
	
	//int i,j;
	
	for(i = 0; i < mem_gv.mem_table_size;){
		sprintf(fn,"_C%d ",mem_gv.mem_table[i].lock);
		int k = mem_table_walkforward(i);
		if(k==mem_gv.mem_table_size-1){
			if(i==mem_gv.mem_table_size-1 || mem_gv.mem_table[k].name[0]=='+') k = mem_gv.mem_table_size;
		}
		strcat(fn,mem_gv.mem_table[i].name);
		strcat(fn,"\r\n");
		for(j=i;j<k;j++){
			if(p-p_backup>WRITE_BUFFER-64){
			p=p_backup;
			sceIoWrite(fd, p, strlen(p));
			p[0]=0;
			}
			
			if(j==i){
			mips_memcpy(p,fn,strlen(fn));
			p+=strlen(fn);
			}
			
			//addr=mem_table_ConvertTabType(&mem_gv.mem_table[j]);
		//if(enc){
		//	addr ^=0xD6F73BEE;
		//}			
			sprintf(p,"_L 0x%08X 0x%08X\r\n",mem_table_ConvertTabType(&mem_gv.mem_table[j]),mem_gv.mem_table[j].value);
			p+=strlen(p);
		}
		i = j;
	}	
	p=p_backup;
	sceIoWrite(fd, p, strlen(p));
		
	sfree(p);
	sceIoClose(fd);
}

static void mem_table_maintain(int size)
{
	int current_num = mem_table_num[mem_gv.mem_table_alloc];
	if(size>current_num)
	{
		int next_num;
		do{
			mem_gv.mem_table_alloc++;
			next_num = mem_table_num[mem_gv.mem_table_alloc];
		}
		while(next_num<size);
		
		p_mem_table temp = smalloc(sizeof(t_mem_table) * current_num, 0xF000);
		mips_memcpy(temp, mem_gv.mem_table, sizeof(t_mem_table) * current_num);
		
		free(mem_gv.mem_table);		
		mem_gv.mem_table = smalloc(sizeof(t_mem_table) * next_num, 0x0F01);
		memset(mem_gv.mem_table, 0, sizeof(t_mem_table) * next_num);
		mips_memcpy(mem_gv.mem_table, temp, sizeof(t_mem_table) * current_num);
		free(temp);
	}
}

extern int mem_table_load(int idx, int clear)
{
	if(clear)
	{
		mem_table_clear();
	}
	char fn[128];
	sprintf(fn, "%s/%02d.tab", TAB_DIR, idx);
	int fd = sceIoOpen(fn, PSP_O_RDONLY, 0777);
	if(fd < 0)
		return 0;
	int new_size;
	sceIoLseek32(fd, 36, PSP_SEEK_SET);
	sceIoRead(fd, &new_size, sizeof(int));
	if(mem_gv.mem_table_size + new_size > TABLE_ENTRY_MAX)
		new_size = TABLE_ENTRY_MAX - mem_gv.mem_table_size;
		
	int new = mem_gv.mem_table_size + new_size;
	mem_table_maintain(new);
	
	int i;
	for(i = mem_gv.mem_table_size; i < new; i ++)
	{
		sceIoRead(fd, &mem_gv.mem_table[i], sizeof(unsigned int) * 2 + sizeof(int) * 2);
	}

	char *p;
	int root=mem_gv.mem_table_size,count=0;
	for(i = mem_gv.mem_table_size; i < new; i ++)
	{
		p = mem_gv.mem_table[i].name;
		memset(p, 0, 12);
		sceIoRead(fd, p, 10);
		if(p[0]!='+'){
			root = i;
			count = 0;
		}
		else if(root!=i && count<2){
			count++;
			strcat(mem_gv.mem_table[root].name, p+1);
		}
	}
	sceIoClose(fd);
	mem_gv.mem_table_size = new;
	return 0;
}

extern void mem_set_lockspd(int spd)
{
	mem_gv.mem_lock_count = spd;
}

extern void mem_table_lock()
{
	mem_gv.mem_lock_idx ++;
	if(mem_gv.mem_lock_idx > mem_gv.mem_lock_count)
	{
		mem_gv.mem_lock_idx = 0;
		int i;
		for(i = 0; i < mem_gv.mem_table_size; i ++)
		{
			//if(mem_gv.mem_table[i].lock)
				mem_write_value(&i);
		}
	}
}

void mem_table_lockall()
{
	int i;
	int lockflag = 1 - mem_gv.mem_table[0].lock;
	for(i = 0; i < mem_gv.mem_table_size; i ++)
	{
		mem_gv.mem_table[i].lock = lockflag;
	}
}

void mem_table_suit(int index, int *start, int *end)
{
	int i;
	for(i=index;i>=0;i--){
		if(mem_gv.mem_table[i].name[0]!='+') break;
	}
	*start = i>0 ? i:0;
	for(i=index+1;i<mem_gv.mem_table_size;i++){
		if(mem_gv.mem_table[i].name[0]!='+') break;
	}
	*end = i;
}

void mem_table_locksuit(int index, int *start, int *end, int lockflag)
{
	int i;
	mem_table_suit(index, start, end);
	
	for(i = *start; i < *end; i ++){
		mem_gv.mem_table[i].lock = lockflag;
	}
}

void mem_table_enable(int index)
{
	int i,start,end;
	mem_table_locksuit(index,&start,&end,1);
	for(i = start; i < end; i ++)
		mem_write_value(&i);
	mem_table_locksuit(index,&start,&end,0);
}

int mem_table_walkback(int index)
{
	int i;
	for(i=index;i>=0;i--){
		if(mem_gv.mem_table[i].name[0]!='+') break;
	}
	for(i=i-1;i>=0;i--){
		if(mem_gv.mem_table[i].name[0]!='+') break;
	}
	return i>0 ? i:0;
}

int mem_table_walkforward(int index)
{
	int i;
	for(i=index;i>=0;i--){
		if(mem_gv.mem_table[i].name[0]!='+') break;
	}
	for(i=i+1;i<mem_gv.mem_table_size;i++){
		if(mem_gv.mem_table[i].name[0]!='+') break;
	}
	return i>=mem_gv.mem_table_size ? mem_gv.mem_table_size-1:i;
}

extern int mem_table_add(p_mem_table item)
{
	int i= mem_gv.mem_table_size;
	if(i > 999)
		return -1;
	mem_table_maintain(mem_gv.mem_table_size+1);
	
	mips_memcpy(&mem_gv.mem_table[i], item, sizeof(t_mem_table));
	mem_gv.mem_table_size ++;
	return i;
}

void mem_table_insert(p_mem_table item, int index)
{
	if(mem_gv.mem_table_size > 998)
		return ;
	mem_table_maintain(mem_gv.mem_table_size+1);
	
	memmove(&mem_gv.mem_table[index+2], &mem_gv.mem_table[index + 1], sizeof(t_mem_table) * (mem_gv.mem_table_size - 1 - index));
	mips_memcpy(&mem_gv.mem_table[index+1], item, sizeof(t_mem_table));
	mem_gv.mem_table_size ++;
}

extern void mem_table_delete(int index)
{
	memmove(&mem_gv.mem_table[index], &mem_gv.mem_table[index + 1], sizeof(t_mem_table) * (mem_gv.mem_table_size - 1 - index));
	mem_gv.mem_table_size --;
}

void mem_table_deletesuit(int index)
{
	int start, end;
	mem_table_suit(index, &start, &end);
	memmove(&mem_gv.mem_table[start], &mem_gv.mem_table[end], sizeof(t_mem_table) * (mem_gv.mem_table_size - end));
	mem_gv.mem_table_size -= end-start;
}

extern int mem_get_table(p_mem_table * table)
{
	*table = mem_gv.mem_table;
	return mem_gv.mem_table_size;
}

extern void mem_table_set(int idx, p_mem_table item)
{
/* 	if(idx < 0 || idx >= mem_gv.mem_table_size)
		return; */
	mips_memcpy(&mem_gv.mem_table[idx], item, sizeof(t_mem_table));
	//mem_write_value(&idx);
}

extern void mem_table_clear()
{
		memset(mem_gv.mem_table, 0, sizeof(t_mem_table) * mem_gv.mem_table_size);
		mem_gv.mem_table_size = 0;
}
/*
extern void mem_free()
{
	mem_search_finish();
}
*/

#include <stdio.h>
#include <string.h>
#include <pspkernel.h>
#include <pspsysmem_kernel.h>
#include "minifloat.h"

static int is_NanOrInf(void *val)
{
	unsigned int conv;
	int exp;

	conv = *((unsigned int *) val);
	exp = (conv >> 23) & 0xff;
	if((exp >= 31+127))
	{
		return 1;
	}
	return 0;
}

unsigned int FloatInt(void *adr)
{
	if(*((unsigned int*)adr)==0){		
		return 0;
	}
	if(is_NanOrInf(adr)) return 0xffffffff;
	unsigned int v = *((unsigned int *) adr);
	int temp;
	asm __volatile__ (
		"mtc1 %1, $f0;"
		"trunc.w.s $f0, $f0;"
		"mfc1 %0, $f0;"
		: "=r" (temp)
		: "r" (v)
	);
	if(temp<0) return -temp;
	else return temp;
}

static unsigned int FloatHex2Wid(unsigned int man, int dot)
{      //8位数字 
       unsigned int wid=0;
                unsigned int temp;
				
				if(dot<=8) temp = man >>(24-(dot+16));
				else temp = (man << dot) >> 8;
				
				int i,j;
				i=0x8000;j=50000000;
				while(i!=0){
					if(temp&i) wid += j;
					i>>=1;j>>=1;
				}
       return wid/1000000;        //保留最高2位 
}

static unsigned int FloatHex2Int(unsigned char *s, unsigned int *wid)
{
	unsigned int man;
	int exp;

	if(*((unsigned int*)s)==0){		
		return 0;
	}
	unsigned int temp=(*(unsigned int*)s);
	exp = ((temp>>23) &0xff) - 127;
	//exp = (unsigned char)(s[3]<<1) + (unsigned char)(s[2]>>7) - 127;
	temp |= 0x00800000;
	man = temp&0x00ffffff;
	//man = ((s[2]|0x80)<<16) + (s[1]<<8) + s[0];
	int dot = 1+exp;
	if(exp<0){
		*wid = FloatHex2Wid(man, dot);
		return 0;
	}
	else{
		if(dot>=32) return 0xffffffff;
		if(dot>24){
			man <<= dot-24;
		}
		else{
			*wid = FloatHex2Wid(man, dot);
			man >>= 24-dot;
		}			
		return man;
	}
}

char* FloatHex2Str(unsigned char *s, char *buffer)
{
	if(is_NanOrInf(s)){
		buffer[0]='N';
		buffer[1]=0;
		return buffer;
	}
	
	if(s[3]&0x80) buffer[0]='-';
	else buffer[0]=' ';
	unsigned int wid=0;
	sprintf(buffer+1,"%u.%02u",FloatHex2Int(s, &wid),wid);
	return buffer;
}


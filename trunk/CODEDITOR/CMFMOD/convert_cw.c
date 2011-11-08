#include <pspsdk.h>
#include <pspkernel.h>
#include <pspmodulemgr_kernel.h>
#include <pspdebug.h>
#include <pspdisplay.h>
#include <stdio.h>
#include <string.h>
#include <pspctrl.h>
#include <psppower.h>
#include <pspsysmem_kernel.h>
#include "font.h"
#include "mem.h"
#include "ui.h"
#include "ctrl.h"
#include "allocmem.h"
#include "convert_cw.h"
#include "encode.h"
#include "smsutils.h"

//每次读取readdb_sect字节,然后从中间找游戏换行截断
#define DB_SECT (32*1024)
#define READDB_SECT (64*1024)

static char CWDB_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/PICTURE/CWC/cheat.db";
//static char pattern[]__attribute__(   (  aligned( 1 ), section( ".data" )  )   )={0x0A,'_','S'};

int openfile(const char *filename, PspFile *pFile)
{
	int iRet = 0;
	do
	{
		pFile->fd = sceIoOpen(filename, PSP_O_RDONLY, 0777);
		if(pFile->fd < 0)	break;
		pFile->buf = malloc(MAX_READ_BUFFER);
		if(pFile->buf == NULL){
			sceIoClose(pFile->fd);
			break;
		}
		iRet = !iRet;
	}
	while(0);

	return iRet;
}

int closefile(PspFile *pFile)
{
	sceIoClose(pFile->fd);
	sfree(pFile->buf);
	return 0;
}
//读取一段文件,以'换行_S'为结束
static int read_sect(int cur, PspFile *pf)
{
	int readsize;
	sceIoLseek32(pf->fd, cur, PSP_SEEK_SET);
	readsize = sceIoRead(pf->fd, pf->buf, READDB_SECT);
	if(readsize<READDB_SECT){
		if(readsize>0) pf->buf[readsize]=0;
		return 0;
	}
	int i=DB_SECT;
	while(1){
		//if(memcmp(pf->buf+i,pattern,3)==0) break;
		if(pf->buf[i]==0x0A && pf->buf[i+1]=='_' && pf->buf[i+2]=='S') break;
		i++;
	}

	pf->buf[i+1]=0;
	return (cur+i+1);
}

//读取'_'开始的行,去注释行
static char* read_line(char *p, char *buf)
{
	int i;
	buf[0]=0;
	do{
		i=0;
		while(p[i]!=0x0A && p[i]!=0x0D){
			if(p[i++]==0) {
				strcpy(buf,p);
				return 0;
			}
		}
		while(p[i]==0x0A || p[i]==0x0D){
			p[i++]=0;
		}		
		strcpy(buf,p);
		p=(char *)((unsigned int)p+i);
	}while(buf[0]!='_');
		
	if(p[0]!=0) return p;
	else return 0;
}

static char* read_name(char *p, char *buf, int len)
{
	int x = 0;
	do{
		if(x>len-2 || p[x]==0) break;
		if(IsHzcode(x, p)) x+=2;
		else x++;
	}while(1);
	mips_memcpy(buf,p,len);
	buf[x]=0;
	return (char*)(p+x);
}

static int read_cwdb(char *filename, char *gameid)
{
	int pos=0;
	char *p;
	char cw_buf[300];
	u32 address,val;
	t_mem_table	t;
	PspFile pf;

	if(openfile(filename, &pf)==0) return 1;
	
	if(gameid!=NULL){
		char codename[11];
		mips_memcpy(codename,gameid,10);
		codename[10]=0;
		do{
			pos=read_sect(pos,&pf);
			p=strstr(pf.buf,codename);
			if(p) break;
		}
		while(pos);
	}
	else{
		read_sect(pos,&pf);
		p=pf.buf;
		p=read_line(p,cw_buf);
	}
	
		if(p==0) {
			closefile(&pf);
			return 1;
		}

#ifdef BIG5_ENCODE_TEXT
	t_encodepack pack;
	char *big5buf = malloc(41688);
	if(big5buf==NULL) return 1;
	if(big5_init(big5buf,&pack)==0 && encode_init(&pack)==0)
	{
		charsets_big5_conv(p,&pack);
		encode_free(&pack);
	}
	free(big5buf);
#endif
	
	p=read_line(p,cw_buf);
	mips_memcpy(ui_get_gamename()+12,cw_buf+3,0x40);
	
	int repeat=0;
	int lock =0;
	char namebuf[80];
	char *namep;
	char nullcode=0;
	while(1){
	//金手指码部分	
		p=read_line(p,cw_buf);
		
		//if(cw_buf[0]!='_') goto READOUT;
		
		if(*(char *)(&cw_buf[0])=='_'){
		if(*(char *)(&cw_buf[1])=='C'){
			if(nullcode==1) {
			t.addr=0x8800000;
			t.value=0;
			t.type=0;
			t.lock=0;
			if(mem_table_add(&t)<0) goto READOUT;
			}
		repeat=0;
		namep = namebuf;
		mips_memcpy(namebuf,cw_buf+4,70);
		lock = strtoul(cw_buf+2,NULL,16);
		namep = read_name(namep, t.name, 10);
		mips_memcpy(t.name,namebuf,30);
		t.name[30]=0;
		t.name[31]=0;
		nullcode=1;
		}
		else if(*(char *)(&cw_buf[1])=='L'){
			nullcode=0;
			if(repeat<5){
				if(repeat==0) {
				}
				else{
					t.name[0] = '+';
					namep = read_name(namep, t.name+1, 9);
				}
				repeat++;
			}
			else{//strcpy(t.name,"+");
				t.name[0]='+';
				t.name[1]=0;
			}
			char *tempptr;
			address=strtoul(cw_buf+2,&tempptr,16)+0x08800000;
			val=strtoul(tempptr,NULL,16);
			t.addr=address;
			t.value=val;
			t.type=0;
			t.lock=lock;
			if(mem_table_add(&t)<0) goto READOUT;
		}
		}
		if(p==0) break;
		if(p[0]=='_' && p[1]=='S') break;
	}
	
READOUT:
	closefile(&pf);
	return 0;
}

int convert(char *id)
{
	if(read_cwdb(CWDB_DIR,id)!=0) return 1;
	else return 0;
}

int convert_cmf(char *filename)
{
	read_cwdb(filename, NULL);
	return 0;
}


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

char cfencription=0;

static char CWDB_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/PICTURE/CWC/cheat.db";
static char CF_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/PICTURE/CWC/cf.dat";
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
int read_sect(int cur, PspFile *pf)
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
		if(pf->buf[i]==0x0A && pf->buf[i+1]=='_' && pf->buf[i+2]=='S'){
		break;
		}
		else if(i>readsize){
		return -1;
		}
		i++;
	}

	pf->buf[i+1]=0;
	return (cur+i+1);
}

//读取'_'开始的行,去注释行
char* read_line(char *p, char *buf)
{
	int i;
	buf[0]=0;
	do{
		i=0;
		while(p[i]!=0x0A && p[i]!=0x0D){
			if(p[i++]==0) {
				if(i<100){
				strcpy(buf,p);
				}
				else{
				mips_memcpy(buf,p,100);
				}
				return 0;
			}
		}
		while(p[i]==0x0A || p[i]==0x0D){
			p[i++]=0;
		}
		
				if(i<100){
				strcpy(buf,p);
				}
				else{
				mips_memcpy(buf,p,100);
				}
		p=(char *)((unsigned int)p+i);
	}while(buf[0]!='_');
	
	if(p[0]!=0) return p;
	else return 0;
}

/*
static char* read_cfline(char *p, char *buf)
{
	int i;
	*((unsigned short*)(&buf[0]))=0x0;
	while(1){
		i=0;
		while(*((unsigned short*)(p+i))!=0x0A0A){
			if(*((unsigned short*)(p+i))==0x0){
				if(i<100){
				memcpy(buf,p,i);
				}
				else{
				memcpy(buf,p,100);
				}
				return 0;
			}
			i+=2;
		}
		//((unsigned short*)(p+i))=0x0;
			i+=2;
		
		if(i<100){
		memcpy(buf,p,i);
		}
		else{
		memcpy(buf,p,100);
		}
		
		p=p+i;
		
		//BIGENDIAN!!
		switch(*((unsigned short*)(&buf[0]))){
		case 0x2047:
		case 0x204D:
		case 0x2044:
		case 0x2043:
			goto exit;
		break;
		}
	}
	exit:
		
	if(*((unsigned short*)(p))==0x0) return 0;
	else return p;
}*/

char* read_name(char *p, char *buf, int len)
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


int read_cwdb(char *filename, char *gameid)
{
	int pos=0;
	char *p;
	char cw_buf[100];
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
	char enc=0;
	while(1){
	//金手指码部分	
		p=read_line(p,cw_buf);
		
		if(cw_buf[0]=='_'){
		if(cw_buf[1]=='E'){
			enc=!enc;
		}
		else if(cw_buf[1]=='C'){
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
		else if(cw_buf[1]=='L'){
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
			else{
				t.name[0]='+';
				t.name[1]=0;
			}
			char *tempptr;
			address=strtoul(cw_buf+2,&tempptr,16);
			if(enc){
			address ^=0xd6f73bee;
			}
			address +=0x08800000;
			val=strtoul(tempptr,NULL,16);
			t.addr=address;
			t.value=val;
			t.type=0;
			t.lock=lock;
			if(mem_table_add(&t)<0) goto READOUT;
		}
		else if(cw_buf[1]=='S'){
			break;
		}
		}
		if(p==0) break;
		if(p[0]=='_' && p[1]=='S') break;
	}
	
READOUT:
	closefile(&pf);
	cfencription=enc;
	
	return 0;
}


void ascii2wide(char *codename,char *ascii){
	
		int k=0,j=0;
		for(k=0;k<26;k+=2,j++){
		codename[k]=0;
		codename[k+1]=ascii[j];
		}
		codename[26]=0;
	
	return ;
}


void sceid2cfid(char *codename,char *gameid){
	
		char buf[1];
		int k=0,j=0;
		for(;k<8;k++){
			if(k&1){
			sprintf(buf,"%1X",gameid[j]&0xF);
			j++;
			}
			else{
			sprintf(buf,"%1X",gameid[j]>>4);
			}
		codename[k]=buf[0];
		}
		codename[8]=0;
	
	return;
}

int read_sect_cf(int cur, PspFile *pf)
{
	int readsize;
	sceIoLseek32(pf->fd, cur, PSP_SEEK_SET);
	readsize = sceIoRead(pf->fd, pf->buf, READDB_SECT);
	//ファイル呵姜
	if(readsize<READDB_SECT){
		if(readsize>0){
		//O虽め
		*((u16 *)(&pf->buf[readsize]))=0;
		}
		return 0;
	}
	
	//バッファ染尸から浮瑚倡幌
	int i=DB_SECT;
	while(1){
		//G0x20赂哼、リ〖ドサイズを亩え/みつかったら姜位
		if(*((unsigned short*)(&pf->buf[i]))==0x0A0A){
			if(*((unsigned short*)(&pf->buf[i+2]))==0x2047){
			break;
			}
		}
		else if(i>readsize){
		return -1;
		}
		
		i+=2;
	}
	
	//呵稿を誓じる
	*((u16 *)(&pf->buf[i+2]))=0;
	return (cur+i+2);
}

/*
int mem_cmpkai(char *s1,char *s2,int len){
    const unsigned char  *p1 = (const unsigned char *)s1;
    const unsigned char  *p2 = (const unsigned char *)s2;
	p1++;
	p2++;
	
	while(len){
		if(*p1 != *p2){
			return 1;
		}
		p1+=2;
		p2+=2;
		len-=2;
	}
	return 0;
}*/

int codefreak_utf16be_seek(char *p,char *cmp){
	int i=0,game=0,temp=0;
	while(i<READDB_SECT){
		
		temp=p+i;
		
		//BIG_ENDIAN!!、なぜか0x0a0a冉年を纳裁すると瓢かなくなる
		switch(*((unsigned short*)(temp))){
		case 0 :
			return 0;
			break;
		case 0x2047://G+x20
			game=temp;
		break;
		case 0x204D://M+0x20
			temp+=2;
			if(memcmp(temp,cmp,26)==0){
				return game;
			}
		break;
		}
		i+=2;
	}
		
	return 0;

}


int read_cf(char *filename, char *gameid)
{
	int pos=0;
	char *p;
	char cw_buf[100];
	u32 address=0,val=0;
	t_mem_table	t;
	PspFile pf;	
	
	if(openfile(filename, &pf)==0) return 1;
	
	if(gameid!=NULL){
		char codename[27];
		int total = 0;
		sceid2cfid(cw_buf,gameid);
		mips_memcpy(cw_buf+8,gameid+5,5);
		ascii2wide(codename,cw_buf);
		///*
		do{
			total=pos;
			pos=read_sect_cf(pos,&pf);
			p=codefreak_utf16be_seek(pf.buf,codename);
			if(p){break;}
		}while(pos);
		//*/
			
	/*
	int fd = sceIoOpen("ms0:/debug.txt", PSP_O_WRONLY | PSP_O_CREAT | PSP_O_TRUNC, 0777);
	sceIoWrite(fd, codename,0x20);
	sceIoWrite(fd, pf.buf,READDB_SECT);
	if(p>0){
	sceIoWrite(fd,p,0x100);
	}
	sceIoClose(fd);
	*/
	}
	else{
		if(read_sect_cf(pos,&pf)==-1){
		closefile(&pf);
		return 1;
		}
		p=pf.buf;
	}
	
	if(p==0) {
	closefile(&pf);
	return 1;
	}
	
	
	t_encodepack pack;
	if(encode_init(&pack) == 0){
		encode_utf16_conv(p, NULL,&pack,MAX_READ_BUFFER);
	}
	else{
		encode_free(&pack);
		closefile(&pf);
		return 1;
	}
	encode_free(&pack);
	
	//SJISに恃垂されているか
	//int fd;
	//fd = sceIoOpen("ms0:/db1.txt", PSP_O_WRONLY | PSP_O_CREAT | PSP_O_TRUNC, 0777);
	//sceIoWrite(fd, p,READDB_SECT);
	//sceIoClose(fd);
	//fd = sceIoOpen("ms0:/db2.txt", PSP_O_WRONLY | PSP_O_CREAT | PSP_O_TRUNC, 0777);
	//sprintf(cw_buf,"%s %X %X",codename,&p[0],&pf.buf[0]);
	//sceIoWrite(fd, cw_buf,32);
	//sceIoClose(fd);
	
	p=read_line(p,cw_buf);
	mips_memcpy(ui_get_gamename()+12,cw_buf+2,0x40);
	
	char enc=0;
	int repeat=0;
	int lock =0;
	char namebuf[31];
	char cwc[9];
	char *namep;
	char nullcode=0;
		
	while(1){
		p=read_line(p,cw_buf);
		
	if(cw_buf[0]=='_'){
		if(cw_buf[1]=='E'){
			mips_memcpy(cwc,cw_buf+10,8);
			val=strtoul(cwc,NULL,16);
			if((val & 0x800)==0){
			enc=!enc;
			}
		}
		else if(cw_buf[1]=='C'){
			if(nullcode==1) {
			t.addr=0x8800000;
			t.value=0;
			t.type=0;
			t.lock=0;
			if(mem_table_add(&t)<0) goto READOUT;
			}
		repeat=0;
		namep = namebuf;
		mips_memcpy(namebuf,cw_buf+2,30);
		lock = 0;
		namep = read_name(namep, t.name, 10);
		mips_memcpy(t.name,namebuf,30);
		t.name[30]=0;
		t.name[31]=0;
		nullcode=1;
		}
		else if(cw_buf[1]=='L'){
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
			else{
				t.name[0]='+';
				t.name[1]=0;
			}
			mips_memcpy(cwc,cw_buf+2,8);
			address=strtoul(cwc,NULL,16);
			if(enc){
			address ^=0xd6f73bee;
			}
			address +=0x08800000;
			mips_memcpy(cwc,cw_buf+10,8);
			val=strtoul(cwc,NULL,16);
			t.addr=address;
			t.value=val;
			t.type=0;
			t.lock=lock;
			if(mem_table_add(&t)<0) goto READOUT;
		}
		else if(cw_buf[1]=='G'){
			break;
		}
	}
	if(p==0) break;
	if(p[0]=='_' && p[1]=='G') break;
	}
	
READOUT:
	closefile(&pf);
	cfencription=enc;
	
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


int convert_cf(char *id)
{
	if(read_cf(CF_DIR,id)!=0) return 1;
	else return 0;
}


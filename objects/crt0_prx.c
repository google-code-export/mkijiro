//MODED BY HAROTURBO
//reffer to PSPLINK/PRXTOOL SRC
/*
        WebSVN  
psp - Rev 2457
Subversion Repositories:
Rev:
(root)/trunk/prxtool/disasm.C @ 2457
Rev 2206 - Blame - Compare with Previous - Last modification - View Log - RSS feed

/***************************************************************
 * PRXTool : Utility for PSP executables.
 * (c) TyRaNiD 2k6
 *
 * disasm.C - Implementation of a MIPS disassembler
 ***************************************************************/

/*
 * NitePR Original Source by: SANiK + imk + telazorn , 
 * MKULTRA By:
 * 		RedHate dmonchild@hotmail.com
 *		NoEffex unigaming.net
 *     (we're still holding down pizzle nizzle.)
 */

/*
 * Some code borrowed from these mighty fine people also.
 * PSPLINK
 * -----------------------------------------------------------------------
 * Licensed under the BSD license, see LICENSE in PSPLINK root for details.
 * main.c - PSPLINK Debug/Impose menu
 * Copyright (c) 2006 James F <tyranid@gmail.com>
 * $HeadURL: file:///home/svn/psp/trunk/psplink/tools/debugmenu/main.c $
 * $Id: main.c 2018 2006-10-07 16:54:19Z tyranid $
 */

/* 
 * FIXES:
 * -----------------------------------------------------------------------
 * Updated mips header to show more op code (version 8)
 * Updated mips header with register highlighting (version 8)
 * Cheat load fix up, now loads codes using real addressing (not in dma mode though) (version 8)
 * **tryed to fix up dma cheat apply.... again**  (version 8)
 * Implemented **NEW** color system in source now loads text files from a folder ms0:/seplugins/nitePR/color/color0.txt color1.txt color2.txt and so on. (version 9)
 * Implemented cheat jokering system into cheat enable and blockadd functions (version 9)
 * Working on popstation mode, please note that popstation code in this source is still testing and will not resemble the final version with any kind of luck. (version 9)
 * Following pointers through decoder j, and jal handles all properly now (version 9)
 * Dump a range from decoder to a file ms0:/nitePRfnc.txt (version 9)
 * Re-wrote copy menu made it a bit smaller (yes every bit counts) (version 9)
 * Implemented Browser and Decoder arrays, this should trim down on over all code but allow users to have 5 browsers and decoders to work with. - next step: even driven, spawn till it craps out. (version 9)
 * Ditched that faggy clip board thing i dont know what i was thinking. (version 9)
 * Added hijack on/off toggle for ftb2 mode (version 9)
 * Dump kernel memory (version 9)
 * Fixed over all issue with freezing for *some* people on start up. now i cant garentee they can get online but they shouldnt be cheating online.(version 9)
 * Gui fixup battery remaining % color fade's from green to red (version 9)
 * Made a slight fix up to the vram pointer this helps when we wanna run it in the xmb or eboot but not just yet folks gotta wait i'll make sure its nice. (version 9)
 * Removed the backdoor that bricks people LOL fun while it lasted, meh source is now public. (version 9)
 * Fixed the missalligned cheat problem with some ugly loops i should probably clean that up in the next version. D: (version 9)
 */

//Crt0_prx
//Includes
#define _PSP_FW_VERSION 150
#include <pspkernel.h>
#include <pspkerneltypes.h>
#include <pspmoduleinfo.h>
#include <pspiofilemgr.h>
#include <pspmodulemgr.h>
#include <pspthreadman.h>
#include <pspwlan.h>
#include <stdlib.h>
#include <pspchnnlsv.h>
#include <pspctrl.h>
#include <string.h>
#include <pspctrl_kernel.h>
#include <pspdisplay.h>
#include <pspdisplay_kernel.h>
#include <pspthreadman_kernel.h>
#include <pspsysmem_kernel.h>
#include <pspsysmem.h>
#include <malloc.h>
#include <psppower.h> //for battery options
#include <pspumd.h>
#include "headers/crt0_prx.h"
#include "headers/module.h"
#include "headers/float.h"
#include "headers/pspdebugkb.h"
char keyboard=0;

extern SceUID sceKernelSearchModuleByName(unsigned char *);

//Defines
#ifdef _UMDMODE_
PSP_MODULE_INFO("MKIJIRO", 0x3007, 1, 2); //0x3007
PSP_MAIN_THREAD_ATTR(0); //0 for kernel mode too
#elif _POPSMODE_
PSP_MODULE_INFO("MKIJIROPOP", 0x3007, 1, 2); //0x3007
//PSP_MODULE_INFO("nitePRpops", PSP_MODULE_KERNEL, 1, 1);
PSP_MAIN_THREAD_ATTR(0); //0 for kernel mode too
#endif

//Globals
unsigned char *MKVER="  MKIJIRO20110504";
unsigned char *gameDir="ms0:/seplugins/nitePR/POPS/__________.txt";
unsigned char gameId[10];
unsigned char running=0;
SceUID thid;
#define MAX_THREAD 64
static int thread_count_start, thread_count_now, pauseuid = -1;
static SceUID thread_buf_start[MAX_THREAD], thread_buf_now[MAX_THREAD];

char psid[16];

//Structures
typedef struct Hook{
 ModuleFunc modfunc;
 char modname[32];
 char libname[32];
 u32 nid;
 void *func;
} Hook;

typedef struct Block{
  unsigned char flags;
  unsigned int address;
  unsigned int stdVal;
  unsigned int hakVal;
}Block;

typedef struct Cheat{
  unsigned short block;
  unsigned short len;
  unsigned char flags;
  unsigned char name[32];
}Cheat;

//Defines
//Block flags
#define FLAG_CWC (1<<3)   //00001000
#define FLAG_DMA (1<<4)   //00010000
#ifdef _JOKER_
#define FLAG_JOKER (1<<1) //00000001
#endif
#define FLAG_FREEZE (1<<5)//00100000
#define FLAG_DWORD (3<<6) //11000000
#define FLAG_UWORD (2<<6) //10000000 unaligned flag this is a note for me ignore this
#define FLAG_WORD (1<<6)  //01000000
#define FLAG_BYTE (0<<6)  //00000000

//Cheat flags
#define FLAG_SELECTED (1<<0) //If selected, will be disabled/enabled by music button
#define FLAG_CONSTANT (1<<1) //If 1, cheat is constantly on regardless of music button
#define FLAG_FRESH (1<<2) //Cheat was just recently enabled/disabled
#ifdef _100_
//省メモリ版 FREECHEAT最低設定
#define NAME_MAX 100
#define BLOCK_MAX 100
unsigned int searchAddress[100];
#define searchResultMax 99
#else
//薄型＋最新CFWHEN+パッチ無しME/INFELNOドライバー推奨
unsigned int searchAddress[500]; //サーチアドレス表示用
#define searchResultMax 499  //サーチ表示最大数
#define NAME_MAX 512 //チート名最大数
#define BLOCK_MAX 1024 //チートコード最大数
#endif

//Globals
SceCtrlData pad;
unsigned short blockTotal=0;
unsigned short cheatTotal=0;
Block block[BLOCK_MAX];
Cheat cheat[NAME_MAX];
unsigned char buffer[64];
unsigned char cheatStatus=0;
unsigned char cheatSaved=0;
unsigned short cheatSelected=0;
unsigned char tabSelected=0;
unsigned char menuDrawn=0;
void *vram;
unsigned int cheatHz=15625;//Cheat 15HZ
unsigned char cheatFlash=0;
unsigned char cheatPause=0;
unsigned char cheatSearch=0;
unsigned int cheatLength=1;
unsigned char extMenu=0;
unsigned int extSelected[4]={0,0,0,0};
char extOpt=0;
unsigned int extOptArg=0;
unsigned char dumpNo=0;
unsigned int cheatNo=0;
unsigned int searchNo=0;
unsigned int searchMax=0;
unsigned int searchHistoryCounter=0;
Block searchHistory[16];
unsigned int searchResultCounter=0;
unsigned int browseAddress[5]={0x48800000,0x48800000,0x48800000,0x48800000,0x48800000};
unsigned int browseY[5]={0,0,0,0,0};
unsigned int browseC[5]={0,0,0,0,0};
unsigned int browseX[5]={0,0,0,0,0};
unsigned int browseLines=16;
unsigned int decodeFormat=0x48800000;
unsigned int decodeAddress[5]={0x48800000,0x48800000,0x48800000,0x48800000,0x48800000};
unsigned int decodeY[5]={0,0,0,0,0};
unsigned int decodeC[5]={0,0,0,0,0};
unsigned int decodeX[5]={0,0,0,0,0};
unsigned int cheatDMA=0;
unsigned char cheatButtonAgeX=0;
unsigned char cheatButtonAgeY=0;
unsigned char searchMode=0;
unsigned char copyMenu=0; //0=Menu Off, 1=Menu On, Copy selected, 2=Menu On, Paste selected
unsigned int copyData=0x08800000;
unsigned int copyData2=0x00000000;
unsigned char screenTime=0;
unsigned char cheatRefresh=0;
unsigned char flipme=1;
unsigned char decodeOptions=0;
unsigned char fileBuffer[2048];
unsigned int fileBufferSize=0;
unsigned int fileBufferBackup=0;
unsigned int fileBufferFileOffset=0;
unsigned int fileBufferOffset=1024;
unsigned int screenNo=0;
unsigned char editFormat=0;
unsigned int searchStart=0x48800000;
unsigned int searchStop=0x49FFFFFC;
unsigned int copyStartFlag=0x48800000;
unsigned int copyEndFlag=0x48800000;
unsigned char copyToggle=0;
unsigned char screenPath[64]={0};
unsigned int clipSelected=0;
unsigned char logcounter=0;
unsigned int backaddress[4];
unsigned char backaddressY[4];
unsigned int storedAddress[32];
unsigned char *hbpath = NULL;
unsigned int SAVETEMP=0x8838DBF0; //moviezone
unsigned int RAMTEMP=0x8838DBF0;
unsigned int searchResultCounterMax=0xE400;
unsigned short searchjumpNo=0;
unsigned short searchjumpNoMax=0;
#define logstart 0x48802804 //log start address
#define jumplog 0x20 //number of jumplog ,default 32
#define JOKERADDRESS 0x3FFC //out put JOKER ADDRES from kernel ram
unsigned char offsetadd=0;
unsigned char colorFile=0;
unsigned char bdNo=0;
unsigned char countermax=0;
unsigned int addresscode=0;
unsigned int addresstmp=0;
unsigned int counteraddress=0;
unsigned char backline=1;
unsigned char HBFLAG=0;
unsigned char k=0;
char BIGSMALL=0;
char FAKEHYOGEN=-1;
unsigned int setting=0;
#ifdef _SWAPBUTTON_
unsigned short SWAPBUTTON=0x4000;
unsigned short SWAPBUTTON2=0x2000;
unsigned short SWAPBUTTON3=0x8000;
#else
#define SWAPBUTTON PSP_CTRL_CROSS
#define SWAPBUTTON2 PSP_CTRL_CIRCLE
#define SWAPBUTTON3 PSP_CTRL_SQUARE
#endif
#ifdef _USB_
unsigned char usbmod=0;
unsigned char usbonbitch=0;
#endif
#ifdef _SCREENSHOT_
unsigned char screenshot_mode=0;
unsigned char *screenshotstring[]={"NONE", "TOGGLE"};
#endif
unsigned char copyMenuX=25;
unsigned char copyMenuY=0;

//gui shit
unsigned int menuKey=PSP_CTRL_VOLUP | PSP_CTRL_VOLDOWN;
unsigned int triggerKey=PSP_CTRL_NOTE;
unsigned int screenKey=0xFFFFFFFF;
#ifdef _UMDMODE_
unsigned int color01=0xFFFFFF00; //bright Red
unsigned int color01_to=0x00030300; //fade amount
#elif _POPSMODE_
unsigned int color01=0xFFFFFF00; //bright Yellow
unsigned int color01_to=0x00030300; //fade amount
#endif
unsigned int color02=0xFFCCCCCC; //grey
unsigned int color02_to=0x00060606; //fade amount
unsigned int color03=0xFFFF0000; //blue
unsigned int color04=0xFF00FF00; //lime green
unsigned int bgcolor=0xFF000000; //black
unsigned int select_cheat=0xFFF0C060;
unsigned int constant_cheat=0xFFF090C0;

char line[78]={0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D, 0x2D};

//unsigned char lineC[78]={"                                                                   "};
#define fileBufferPeek(a_out, a_ahead) if((fileBufferOffset + a_ahead) >= 1024) { fileBufferBackup=sceIoLseek(fd, 0, SEEK_CUR); sceIoLseek(fd, a_ahead, SEEK_CUR); sceIoRead(fd, &a_out, 1); sceIoLseek(fd, fileBufferBackup, SEEK_SET); } else { a_out=fileBuffer[fileBufferOffset + a_ahead]; }
#define fileBufferRead(a_out, a_size) if(fileBufferOffset == 1024) { fileBufferSize=sceIoRead(fd, fileBuffer, 1024); fileBufferOffset=0; } memcpy(a_out, &fileBuffer[fileBufferOffset], a_size); fileBufferOffset+=a_size; fileBufferFileOffset+=a_size;
//#define lineClear(a_line) pspDebugScreenSetXY(0, a_line); pspDebugScreenPuts(lineC); pspDebugScreenSetXY(0, a_line);
#define lineClear(a_line) pspDebugScreenSetXY(0, a_line); if(a_line==33){spaceman(67);}else{spaceman(68);}pspDebugScreenSetXY(0, a_line);


//Functions
int module_start(SceSize args, void *argp) __attribute__((alias("_start")));
int module_stop(SceSize args, void *argp) __attribute__((alias("_stop")));
static void gamePause(SceUID thid);
static void gameResume(SceUID thid);
unsigned char suspend = 0; // global var


//Arrays
unsigned int decDelta[10]={1000000000, 100000000, 10000000, 1000000, 100000, 10000, 1000, 100, 10, 1};

unsigned char* searchModeName[]={ "  0=Same ", "  1=Different ", "  2=Greater ", "  3=Less ", "  4=Inc by ", "  5=Dec by ", "  6=Equal to " };

#ifdef _PSID_
//Mac Address hooking module
unsigned char cfg[]={'C', 'F', 'G', 0x88, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00,  0x80, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x40, 0x80, 0x00, 0x00};

unsigned int hookMac(unsigned char *a_mac){
  memcpy(a_mac, cfg+5, 6);
  return 0;
}

Hook hookA[1]={
  { { 0, NULL }, "sceWlan_Driver", "sceWlanDrv", 0x0c622081, hookMac},
};
#endif

//Cheat handler
unsigned int char2hex(unsigned char *a_data, unsigned int *a_type){
  unsigned int dword=0;
  unsigned int power=0;
  
  while(power < 8){
    switch(a_data[power]){
      case '_': break; //Freeze cheat
      case '0': dword|=0 * (1 << (4*(7-power))); break;
      case '1': dword|=1 * (1 << (4*(7-power))); break;
      case '2': dword|=2 * (1 << (4*(7-power))); break;
      case '3': dword|=3 * (1 << (4*(7-power))); break;
      case '4': dword|=4 * (1 << (4*(7-power))); break;
      case '5': dword|=5 * (1 << (4*(7-power))); break;
      case '6': dword|=6 * (1 << (4*(7-power))); break;
      case '7': dword|=7 * (1 << (4*(7-power))); break;
      case '8': dword|=8 * (1 << (4*(7-power))); break;
      case '9': dword|=9 * (1 << (4*(7-power))); break;
      case 'a':case 'A': dword|=0xA * (1 << (4*(7-power))); break;
      case 'b':case 'B': dword|=0xB * (1 << (4*(7-power))); break;
      case 'c':case 'C': dword|=0xC * (1 << (4*(7-power))); break;
      case 'd':case 'D': dword|=0xD * (1 << (4*(7-power))); break;
      case 'e':case 'E': dword|=0xE * (1 << (4*(7-power))); break;
      case 'f':case 'F': dword|=0xF * (1 << (4*(7-power))); break;
      default: dword>>=4*(8-power); *a_type=power; return dword;
    }
    power++;
  }
  
  *a_type=8;
  return dword;
}

void decToCheat(){
  unsigned char a_size=4;
  unsigned int a_address=copyStartFlag; 
  unsigned int tempaddy1=copyStartFlag;
  unsigned int tempaddy2=copyEndFlag;
  unsigned int a_length=copyEndFlag - copyStartFlag;
  
  if((cheatTotal + 1 < NAME_MAX) && (blockTotal + 1 < BLOCK_MAX)){
    
    cheat[cheatTotal].block=blockTotal;
   	cheat[cheatTotal].flags=0;
    cheat[cheatTotal].len=a_length>>2;
    
    int i; 
	for(i=0; i< a_length; i++){
    
		block[blockTotal].address=(a_address+(i*4))-0x48800000;
		block[blockTotal].address&=0x0FFFFFFF;
		block[blockTotal].flags=0;
		block[blockTotal].address+=0x08800000;
		
		block[blockTotal].hakVal=*((unsigned int*)(a_address+(i*4)));

		block[blockTotal].flags|=FLAG_DWORD; if(cheatSaved) block[blockTotal].stdVal=*((unsigned int*)(block[blockTotal].address));

    	blockTotal++;
	}
    sprintf(&cheat[cheatTotal].name, "NEW CHEAT %d", cheatNo);
    cheatNo++;
    cheatTotal++;
  }
}

unsigned char fnc=0;
void decToText(){
	
	unsigned int a_address=copyStartFlag; 
	unsigned int tempaddy1=copyStartFlag;
	unsigned int tempaddy2=copyEndFlag;
	int ramcounter=0;
	unsigned int a_length=copyEndFlag - copyStartFlag;
	
	sprintf(buffer,"ms0:/MKIJIROfnc%d.txt",fnc);
	int fd=sceIoOpen(buffer, PSP_O_CREAT | PSP_O_WRONLY | PSP_O_APPEND, 0777);
	if(fd > 0){
		sprintf(buffer, "#FNC %d \n", fnc);
		memcpy(SAVETEMP+ramcounter,buffer,strlen(buffer));
		ramcounter+=strlen(buffer);
		//sceIoWrite(fd, buffer, strlen(buffer));
		
		unsigned int counter=0; 
		while(counter < (a_length>>2)){
			
			sprintf(buffer, "0x%08lX ", (a_address+(counter*4))- 0x48800000);
			memcpy(SAVETEMP+ramcounter,buffer,strlen(buffer));
			ramcounter+=strlen(buffer);
			//sceIoWrite(fd, buffer, strlen(buffer));
			
			sprintf(buffer, "0x%08lX\n", *((unsigned int*)(a_address+(counter*4))));
			//sceIoWrite(fd, buffer, strlen(buffer));
			memcpy(SAVETEMP+ramcounter,buffer,strlen(buffer));
			ramcounter+=strlen(buffer);
			counter++;
			
		}
		sceIoClose(fd);
		sprintf(buffer,"ms0:/MKIJIROfnc%d.txt",fnc);
		dump_memregion(buffer,(void*)SAVETEMP,ramcounter);
		sceKernelDelayThread(1500);
		int i=0;
		for(i; i<(ramcounter>>2); i++){
		*(unsigned int *)(SAVETEMP+4*i)=0;
		}
		fnc+=1;
	}
}

unsigned int cheatNew(unsigned char a_size, unsigned int a_address, unsigned int a_value, unsigned int a_length, unsigned int mode){
  
  if((cheatTotal + 1 < NAME_MAX) && (blockTotal + 1 < BLOCK_MAX)){
    
    cheat[cheatTotal].block=blockTotal;
    cheat[cheatTotal].flags=0;
    cheat[cheatTotal].len=a_length;
    
    int i; 
	for(i=0; i< a_length; i++){
    
    	/* commented out for now i'll fix this later it's frusterating me today
		if(mode == 1){
			block[blockTotal].flags|=FLAG_DMA;
			block[blockTotal].address&=0x0FFFFFFF;
			block[blockTotal].address+=0x08800000;
			mode=0;
		}
		if(mode == 2){
			block[blockTotal].flags|=FLAG_JOKER;
			block[blockTotal].address&=0x0FFFFFFF;
			block[blockTotal].address+=0x08800000;
			block[blockTotal].address+=0xFF000000;
			mode=0;
		}
		else{
			block[blockTotal].address=a_address;
			block[blockTotal].address&=0x0FFFFFFF;
			block[blockTotal].address+=0x08800000;
			block[blockTotal].flags=0;
		}
		*/
		
		block[blockTotal].address=a_address;
		block[blockTotal].address&=0x0FFFFFFF;
		block[blockTotal].address+=0x08800000;
		block[blockTotal].flags=0;
		
		block[blockTotal].hakVal=a_value;

		switch(a_size){
		  case 1: block[blockTotal].flags|=FLAG_BYTE; if(cheatSaved) block[blockTotal].stdVal=*((unsigned char*)(block[blockTotal].address)); break;
		  case 2: block[blockTotal].flags|=FLAG_WORD; if(cheatSaved) block[blockTotal].stdVal=*((unsigned short*)(block[blockTotal].address & 0xFFFFFFE)); break;
		  case 4: block[blockTotal].flags|=FLAG_DWORD; if(cheatSaved) block[blockTotal].stdVal=*((unsigned int*)(block[blockTotal].address & 0xFFFFFFC)); break;
		  default: block[blockTotal].flags|=FLAG_UWORD;
		}
    
    	blockTotal++;
	}
    sprintf(&cheat[cheatTotal].name, "NEW CHEAT %d", cheatNo);
    cheatNo++;
    cheatTotal++;
  }
}

unsigned int blockAdd(int fd, unsigned char *a_data){
  
  unsigned int type;
  unsigned int offset;
  unsigned char hex[8];
  unsigned char temp[8];


  unsigned char chartemp[4];
  int counter=0;
  int cheatType=0;
  int copyAmount=0;
  
  if(blockTotal!=BLOCK_MAX){
    block[blockTotal].flags=0;
    
    //read address
    counter=0;
	while(counter < 100){ //correct missaligned addressing in db
		offset=sceIoLseek(fd, 0, SEEK_CUR);
		sceIoRead(fd, hex, 1);
		if(hex[0]=='x'){ break; }
		else if(hex[0]=='X'){ break; }
		counter++;
	}
	
    sceIoLseek(fd, 0, SEEK_CUR);
    sceIoRead(fd, hex, 8);
    block[blockTotal].address=char2hex(hex, &type);
    
	//is our addressing within bounds?
    if(block[blockTotal].address==0xFFFFFFFF){ // is block dma?
		block[blockTotal].flags|=FLAG_DMA;
		block[blockTotal].stdVal=0xFFFFFFFF;
    }
    #ifdef _JOKER_
    else if((block[blockTotal].address>=0xFF000000) && (block[blockTotal].address<=0xFFFFF3F9)){ // is block joker?
    	block[blockTotal].flags|=FLAG_JOKER;
    	//block[blockTotal].address+=0x08800000;
    	//block[blockTotal].address-=0xFF000000;
		//block[blockTotal].stdVal=0xFF000000;
    }
    #endif
    else if((block[blockTotal].address > 0x10000000)&&(block[blockTotal].address < 0x30000000)){ //is it cw mode (non dma)?
	block[blockTotal].flags|=FLAG_CWC;
    	block[blockTotal].address&=0x0FFFFFFF;
    	block[blockTotal].address+=0x08800000;
	}
    else{ //is block other?
    	block[blockTotal].address&=0x0FFFFFFF;
    	if(block[blockTotal].address < 0x08800000){
    		block[blockTotal].address+=0x08800000;
		}
    }
   
	//read value 
	counter=0;
	while(counter < 100){ //correct missaligned values in db and check for cheat value flagging
		offset=sceIoLseek(fd, 0, SEEK_CUR);
		sceIoRead(fd, hex, 1);
		if(hex[0]=='x'){ break; }
		else if(hex[0]=='X'){ cheatType=0; break; }
		else if(hex[0]==':'){ cheatType=1; break; }
		counter++;
	}
	offset=sceIoLseek(fd, 0, SEEK_CUR);
	
	if(counter == 100){ strcpy(hex, "00000000"); }
	else{
		switch(cheatType){
			case 0: //hex cheat
				sceIoRead(fd, hex, 8);
				block[blockTotal].hakVal=char2hex(hex, &type);
			break;
			case 1: //text cheat
				sceIoRead(fd, hex, 4);
				counter=0;
				while(counter < 4){ if((hex[counter]==0x0D) || (hex[counter]==0x0A)){hex[counter]=NULL;} counter++; }
				if(strlen(hex)==1){ hex[1]=NULL; hex[2]=NULL; hex[3]=NULL; type=2; }
				else if(strlen(hex)==2){ hex[2]=NULL; hex[3]=NULL; type=4; }
				else if(strlen(hex)==3){ hex[2]=NULL; hex[3]=NULL; type=4; }
				else if(strlen(hex)==4){ type=8; }
				memcpy(&block[blockTotal].hakVal, hex, 4);
			break;
		}
	}
    if(hex[0]=='_'){
    	block[blockTotal].flags|=FLAG_FREEZE;
    }

	switch(type){
		case 2: block[blockTotal].flags|=FLAG_BYTE; break;
		case 4: block[blockTotal].flags|=FLAG_WORD; break;
		case 8: block[blockTotal].flags|=FLAG_DWORD; break;
		default: block[blockTotal].flags|=FLAG_UWORD;
	}

    sceIoLseek(fd, offset+type, SEEK_SET); //Reposition the cursor depending on size of Hex value
    
    blockTotal++;
    
    return 1;
  }
  return 0;
}

unsigned int colorAdd(unsigned char colorDir[]){
  
  int fd;
  fd=sceIoOpen(colorDir, PSP_O_RDONLY, 0777);
  
  if(fd > 0){
	
	unsigned int type;
	unsigned int offset;
	unsigned char hex[8];
	int counter=0;
	int scounter=0;
    	
    counter=0;
    while(counter < 15){
		//find seperator
		scounter=0;
		while(scounter < 100){ //correct missaligned addressing in db
			offset=sceIoLseek(fd, 0, SEEK_CUR);
			sceIoRead(fd, hex, 1);
			if(hex[0]=='x'){ break; } else if(hex[0]=='X'){ break; }
			scounter++;
		}
		
		sceIoLseek(fd, 0, SEEK_CUR);
		sceIoRead(fd, hex, 8);
		
		switch(counter){
			case 0:
			menuKey=char2hex(hex, &type);
			break;
			case 1:
			triggerKey=char2hex(hex, &type);
			break;
			case 2:
			screenKey=char2hex(hex, &type);
			break;
			case 3:
				color01=char2hex(hex, &type);
			break;
			case 4:
				color01_to=char2hex(hex, &type);
			break;
			case 5:
				color02=char2hex(hex, &type);
			break;
			case 6:
				color02_to=char2hex(hex, &type);
			break;
			case 7:
				color03=char2hex(hex, &type);
			break;
			case 8:
				color04=char2hex(hex, &type);
			break;
			case 9:
				bgcolor=char2hex(hex, &type);
			break;
			case 10:
				select_cheat=char2hex(hex, &type);
			break;
			case 11:
				constant_cheat=char2hex(hex, &type);
			break;
			case 12:
				SAVETEMP=char2hex(hex, &type);
			break;
			case 13:
				RAMTEMP=char2hex(hex, &type);
			break;
			case 14:
				searchResultCounterMax=char2hex(hex, &type);
			break;
			//case 15:
			//	setting=char2hex(hex, &type);
			//break;
		}
		
		counter++;
    	sceIoLseek(fd, offset+type, SEEK_SET); //Reposition the cursor depending on size of Hex value
	}
  }
  sceIoClose(fd);
  return 0;
  
}

void settingload(unsigned char set[]){
   int fd=sceIoOpen(set, PSP_O_RDONLY, 0777);
   if(fd>0){
    sceIoRead(fd,&setting,4);
    sceIoRead(fd,&cheatHz,4);
	cheatPause=(setting>>24)&0x01;
	browseLines=16-8*((setting>>16)&0x1);
	#ifdef _SWAPBUTTON_
	SWAPBUTTON=0x4000-0x2000*((setting>>8)&0x1);
	SWAPBUTTON2=0x2000+0x2000*((setting>>8)&0x1);
	SWAPBUTTON3=0x8000-0x7000*((setting>>8)&0x1);
	#endif
	decodeFormat=0x48800000-0x8800000*(setting&0x1);
	}
	sceIoClose(fd);
}

void cheatEnable(unsigned int a_cheat){
	
	unsigned int counter;
	unsigned char resetDMA=0;
	cheatDMA=0;
	unsigned char NULLPOINTER=0;
    unsigned char joker=0;
  	
	counter=cheat[a_cheat].block;
	while(counter < (cheat[a_cheat].block+cheat[a_cheat].len)){
		
		#ifdef _JOKER_
		if(block[counter].flags & FLAG_JOKER){
				if(block[counter].hakVal == 0){
				if((block[counter].address & 0xFFFFFF) != (*((unsigned int*)(0x08800000+JOKERADDRESS))& ((block[counter].address) & 0xFFFFFF)) ){
				joker=1;}
				}
				else{
				if( (block[counter].address  & 0xFFFF) != *((unsigned short*)(0x08800000 + (block[counter].hakVal & 0x1FFFFFE)))){
				joker=1;}
				}
			counter+=1; //adjust the counter *skip the joker cheat we dont want to apply it or back it up*
		}
		#endif
		
		if(joker){
			if(block[counter].flags & FLAG_DMA){
				if(block[counter].address==0xFFFFFFFF){
					cheatDMA=*((unsigned int*)(0x08800000 + (block[counter].hakVal & 0x1FFFFFC))) - 0x08800000;
					if(cheatDMA>0 && cheatDMA<0x1800000){
						if(block[counter].stdVal != cheatDMA){
						resetDMA=1;
						block[counter].stdVal=cheatDMA;
							NULLPOINTER=0;
							}
						else{
						resetDMA=0;
						}
					}
					else{
						NULLPOINTER=1;
					}
				}
				else{
					cheatDMA=0;
					resetDMA=0;
				}
			}
			else if(!resetDMA){
			if((cheatDMA+block[counter].address >= 0x08800000) && (cheatDMA+block[counter].address <= 0x0A000000) && NULLPOINTER==0){
				if(block[counter].flags & FLAG_CWC){
				}
				else{
				switch(block[counter].flags & FLAG_DWORD){
					case FLAG_DWORD:
						if(block[counter].address % 4 == 0){
						*((unsigned int*)(cheatDMA+block[counter].address & 0xFFFFFFC))=block[counter].stdVal;
						sceKernelDcacheWritebackInvalidateRange(cheatDMA+block[counter].address,4);
						sceKernelIcacheInvalidateRange(cheatDMA+block[counter].address,4);
					}
					break;
					case FLAG_WORD:
					if(block[counter].address % 2 == 0){
						*((unsigned short*)(cheatDMA+block[counter].address & 0xFFFFFFE))=(unsigned short)block[counter].stdVal;
						sceKernelDcacheWritebackInvalidateRange(cheatDMA+block[counter].address,2);
						sceKernelIcacheInvalidateRange(cheatDMA+block[counter].address,2);
					}
					break;
					case FLAG_BYTE:
						*((unsigned char*)(cheatDMA+block[counter].address))=(unsigned char)block[counter].stdVal;
						sceKernelDcacheWritebackInvalidateRange(cheatDMA+block[counter].address,1);
						sceKernelIcacheInvalidateRange(cheatDMA+block[counter].address,1);
					break;
						}
					}
				}
			}
		}
		else{
			if(block[counter].flags & FLAG_DMA){
				if(block[counter].address==0xFFFFFFFF){ 
					//if((*((unsigned int*)(0x08800000 + (block[counter].hakVal & 0x1FFFFFC))) >= 0x08800000) && (*((unsigned int*)(0x08800000 + (block[counter].hakVal & 0x1FFFFFC))) <= 0x09FFFFFC)){
						cheatDMA=*((unsigned int*)(0x08800000 + (block[counter].hakVal & 0x1FFFFFC))) - 0x08800000;
						if(cheatDMA>0 && cheatDMA<0x1800000){
						if(block[counter].stdVal != cheatDMA){
						resetDMA=1;
						block[counter].stdVal=cheatDMA;
							NULLPOINTER=0;
							}
						else{
							resetDMA=0;
						}
						}
						else{
							NULLPOINTER=1;
						}
					//}
				}
				else{
					cheatDMA=0;
					resetDMA=0;
				}
			}
			else if((cheatDMA+block[counter].address >= 0x08800000) && (cheatDMA+block[counter].address <= 0x0A000000) && NULLPOINTER==0){
				//Backup data?
				if(((cheatDMA) && (resetDMA==1)) || ((cheat[a_cheat].flags & FLAG_FRESH) && (block[counter].flags & FLAG_FREEZE))){
					if(block[counter].flags & FLAG_CWC){
					}
					else{
					switch(block[counter].flags & FLAG_DWORD){
						case FLAG_DWORD:
							if(block[counter].address % 4 == 0){
							block[counter].stdVal=*((unsigned int*)(cheatDMA+block[counter].address & 0xFFFFFFC));
							}
						break;

						case FLAG_WORD:
							if(block[counter].address % 2 == 0){
							block[counter].stdVal=*((unsigned short*)(cheatDMA+block[counter].address & 0xFFFFFFE));
							}
						break;
						case FLAG_BYTE:
							block[counter].stdVal=*((unsigned char*)(cheatDMA+block[counter].address));
						break;
						}
						if(block[counter].flags & FLAG_FREEZE){
						block[counter].hakVal=block[counter].stdVal;
					}
					}
				}
				//Apply cheat!
				switch(block[counter].flags & FLAG_DWORD){
					case FLAG_DWORD:
					if(block[counter].address % 4 == 0){
						*((unsigned int*)(cheatDMA+block[counter].address & 0xFFFFFFC))=block[counter].hakVal;
						sceKernelDcacheWritebackInvalidateRange(cheatDMA+block[counter].address,4);
						sceKernelIcacheInvalidateRange(cheatDMA+block[counter].address,4);
					}
					break;
					case FLAG_WORD:
					if(block[counter].address % 2 == 0){
						*((unsigned short*)(cheatDMA+block[counter].address & 0xFFFFFFE))=(unsigned short)block[counter].hakVal;
						sceKernelDcacheWritebackInvalidateRange(cheatDMA+block[counter].address,2);
						sceKernelIcacheInvalidateRange(cheatDMA+block[counter].address,2);
					}
					break;
					case FLAG_BYTE:
						*((unsigned char*)(cheatDMA+block[counter].address))=(unsigned char)block[counter].hakVal;
						sceKernelDcacheWritebackInvalidateRange(cheatDMA+block[counter].address,1);
						sceKernelIcacheInvalidateRange(cheatDMA+block[counter].address,1);
					break;
				}
			}
		}
		counter++;
	}
	joker=0;
}

void cheatDisable(unsigned int a_cheat){
	unsigned int counter;
	unsigned char resetDMA=0;
	unsigned char NULLPOINTER=0;
	cheatDMA=0;
	counter=cheat[a_cheat].block;
	while(counter < (cheat[a_cheat].block+cheat[a_cheat].len)){
		if(block[counter].flags & FLAG_DMA) {
			if(block[counter].address==0xFFFFFFFF) {
				cheatDMA=*((unsigned int*)(0x08800000 + (block[counter].hakVal & 0x1FFFFFC))) - 0x08800000;
				if(block[counter].stdVal != cheatDMA){
				if(cheatDMA>0 && cheatDMA<0x1800000){
						resetDMA=1;
						block[counter].stdVal=cheatDMA;
						NULLPOINTER=0;
						}
						else{
						NULLPOINTER=1;
						}
				}
				else{
					resetDMA=0;
				}
			}
			else{
				cheatDMA=0;
				resetDMA=0;
			}
		}
		else if(!resetDMA){
			if((cheatDMA+block[counter].address >= 0x08800000) && (cheatDMA+block[counter].address < 0x0A000000)&& NULLPOINTER==0){
				if(block[counter].flags & FLAG_CWC){
				}
				else if((block[counter].flags & FLAG_JOKER)){
				//nocode
				}
				else{
			switch(block[counter].flags & FLAG_DWORD){
				case FLAG_DWORD:
					if(block[counter].address % 4 == 0){
					*((unsigned int*)(cheatDMA+block[counter].address & 0xFFFFFFC))=block[counter].stdVal;
					sceKernelDcacheWritebackInvalidateRange(cheatDMA+block[counter].address,4);
					sceKernelIcacheInvalidateRange(cheatDMA+block[counter].address,4);
					}
				break;
				case FLAG_WORD:
					if(block[counter].address % 2 == 0){
					*((unsigned short*)(cheatDMA+block[counter].address & 0xFFFFFFE))=(unsigned short)block[counter].stdVal;
					sceKernelDcacheWritebackInvalidateRange(cheatDMA+block[counter].address,2);
					sceKernelIcacheInvalidateRange(cheatDMA+block[counter].address,2);
					}
				break;
				case FLAG_BYTE:
					*((unsigned char*)(cheatDMA+block[counter].address))=(unsigned char)block[counter].stdVal;
					sceKernelDcacheWritebackInvalidateRange(cheatDMA+block[counter].address,1);
					sceKernelIcacheInvalidateRange(cheatDMA+block[counter].address,1);
				break;
				}
			}
			}
		}
		counter++;
	}
}

void cheatApply(unsigned char a_type){ //0=Enable/Disable FLAG_FRESH, -1=Enable only all (Freeze on all)
  unsigned int counter;
  
  if(!cheatSaved) return;
  if(!a_type) { cheatFlash=2; } //Write the cheat twice even if CWCheat mode is off
  
  	if(cheatStatus){
		//Enable cheats
		counter=0;
		while(counter < cheatTotal){
			if((cheat[counter].flags & FLAG_SELECTED) || (cheat[counter].flags & FLAG_CONSTANT)){
				if(((!a_type) && (cheat[counter].flags & FLAG_FRESH)) || (a_type)){
					cheatEnable(counter);
					cheat[counter].flags&=~FLAG_FRESH;
				}
			}
			else if((!a_type) && (cheat[counter].flags & FLAG_FRESH)){
				cheatDisable(counter);
				cheat[counter].flags&=~FLAG_FRESH;
			}
			counter++;
		}
	}
	else{
		//Disable cheats
		counter=0;
		while(counter < cheatTotal){
			if(cheat[counter].flags & FLAG_CONSTANT){
				if(((!a_type) && (cheat[counter].flags & FLAG_FRESH)) || (a_type)){
					cheatEnable(counter);
					cheat[counter].flags&=~FLAG_FRESH;
				}
			}
			else if((!a_type) && (cheat[counter].flags & FLAG_FRESH)){
				cheatDisable(counter);
				cheat[counter].flags&=~FLAG_FRESH;
			}
			counter++;
		}
	}
}

void cheatSave(){ 
	unsigned char fileChar=0;
	unsigned char fileMisc[3];
	int fileSize=0;
	int counter=-1;
	int scounter=0;
	int ramcounter=0;
	char *codehead[]={"#!!","#!","#"};
	char fileMode=0; //0=Unknown/Initial, 1=Comment, 2=Waiting for \n (ignoring)
	int fd;
	//int tempFd;

	//1) Open the original cheat file
	fd=sceIoOpen(gameDir, PSP_O_RDONLY, 0777);
	if(fd>0){
		//Find the file size
		fileSize=sceIoLseek(fd, 0, SEEK_END);
		sceIoLseek(fd, 0, SEEK_SET);
		//Initiate the read buffer
		fileBufferOffset=1024;
		fileBufferFileOffset=0;
		//2) Open up the temporary and get ready to regenerate it
		//if(tempFd<=0) { sceIoClose(fd); return;}
		//Add the codes that are already there
		while(fileBufferFileOffset < fileSize){

		fileBufferRead(&fileChar, 1);
			if(fileBufferSize == 0) break;
			//Interpret the byte based on the mode
			
			if(fileMode == 0){
				//Pick a mode
				switch(fileChar){
					case ';': fileMode=1;
					break;

					case '#': fileMode=2;  counter++;
						//Add a double line skip?
						//if(counter != 0){
						//memcpy(SAVETEMP+ramcounter,codehead[3],1);
						//ramcounter++;
						//}
						//Is there an error...?
						if(counter >= cheatTotal){
							//sceIoClose(tempFd);
							sceIoClose(fd);
							return;
						}

						//Set up the subCounter
						scounter=cheat[counter].block;

						//Is it on by default...?
						if(cheat[counter].flags & FLAG_CONSTANT) {
						memcpy(SAVETEMP+ramcounter,codehead[0],3);
						ramcounter+=3;
						}
						else if(cheat[counter].flags & FLAG_SELECTED){
						memcpy(SAVETEMP+ramcounter,codehead[1],2);
						ramcounter+=2;
						}
						else{
						memcpy(SAVETEMP+ramcounter,codehead[2],1);
						ramcounter++;
						}
						//Write out the name of the cheat
						memcpy(SAVETEMP+ramcounter,cheat[counter].name,strlen(cheat[counter].name));
						ramcounter+=strlen(cheat[counter].name);
						*(unsigned char *)(SAVETEMP+ramcounter)=0xA;ramcounter++;
					break;
					
					case '0':
						if((fileBufferFileOffset) < fileSize){ 
							fileBufferPeek(fileMisc[0], 0);
							if(fileMisc[0] == 'x'){ //Is there an error...?
								if(counter == (unsigned int)-1){
									//sceIoClose(tempFd);
									sceIoClose(fd);
									return;
								}
								if(scounter >= (cheat[counter].block+cheat[counter].len)){
									//sceIoClose(tempFd);
									sceIoClose(fd);
									return;
								}
								//Write out the address
								if(block[scounter].flags & FLAG_DMA){
								sprintf(buffer, "0x%08lX ", (block[scounter].address));
								}
								else if(block[scounter].flags & FLAG_CWC){
								sprintf(buffer, "0x%08lX ", (block[scounter].address - 0x08800000 + 0x20000000));
								}
								else if(block[scounter].flags & FLAG_JOKER){
								sprintf(buffer, "0x%08lX ", (block[scounter].address));
								}
								else{
								sprintf(buffer, "0x%08lX ", (block[scounter].address - 0x08800000));
								}
								memcpy(SAVETEMP+ramcounter,buffer,11);
								ramcounter+=11;
								//Write out the value
								if(block[scounter].flags & FLAG_FREEZE){

									switch(block[scounter].flags & FLAG_DWORD){
										case FLAG_DWORD:
										sprintf(buffer, "0x________\n");
										memcpy(SAVETEMP+ramcounter,buffer,11);
										ramcounter+=11;
										break;

										case FLAG_WORD:
										sprintf(buffer, "0x____\n");
										memcpy(SAVETEMP+ramcounter,buffer,7);
										ramcounter+=7;
										break;

										case FLAG_BYTE:
										sprintf(buffer, "0x__\n");
										memcpy(SAVETEMP+ramcounter,buffer,5);
										ramcounter+=5;
										break;										
									}

								}
								else{
									switch(block[scounter].flags & FLAG_DWORD){
										case FLAG_DWORD:
										sprintf(buffer, "0x%08lX\n", block[scounter].hakVal);
										memcpy(SAVETEMP+ramcounter,buffer,11);
										ramcounter+=11;
										break;

										case FLAG_WORD:
										sprintf(buffer, "0x%04hX\n", (unsigned short)block[scounter].hakVal);
										memcpy(SAVETEMP+ramcounter,buffer,7);
										ramcounter+=7;
										break;

										case FLAG_BYTE:
										sprintf(buffer, "0x%02hX\n", (unsigned char)block[scounter].hakVal);
										memcpy(SAVETEMP+ramcounter,buffer,5);
										ramcounter+=5;
										break;
									}
								}
								//Skip the rest
								fileMode=2;
								scounter++;
							}
						}
					break;
				}
			}
			else if(fileMode == 1){
				//Just copy it out straight to the file
				if((fileChar == '\r') || (fileChar == '\n')){
					fileMode=0;
				}
			}
			else if(fileMode == 2){
				//Just wait for an '\r' or '\n'
				if((fileChar == '\r') || (fileChar == '\n')){
					fileMode=0;
				}
			}
		}
	}
	//Close the file
	sceIoClose(fd);

	//Open the file for appending
	fd=sceIoOpen(gameDir, PSP_O_CREAT | PSP_O_WRONLY | PSP_O_APPEND, 0777);
	if(fd > 0){
		//Add any new codes
		counter++;
		//if(counter != 0) //memcpy(SAVETEMP+ramcounter,codehead[3],1);ramcounter++;
		while(counter < cheatTotal){
			//Write the cheat name
			if(cheat[counter].flags & FLAG_CONSTANT){
			memcpy(SAVETEMP+ramcounter,codehead[0],3);
			ramcounter+=3;
			}
			else if(cheat[counter].flags & FLAG_SELECTED){
			memcpy(SAVETEMP+ramcounter,codehead[1],2);
			ramcounter+=2;
			}
			else{
			memcpy(SAVETEMP+ramcounter,codehead[2],1);
			ramcounter++;
			}
			//Write out the name of the cheat
			memcpy(SAVETEMP+ramcounter,cheat[counter].name,strlen(cheat[counter].name));
			ramcounter+=strlen(cheat[counter].name);
			*(unsigned char *)(SAVETEMP+ramcounter)=0xA;ramcounter++;
			//Loop through the addresses
			scounter=cheat[counter].block;
			while(scounter < (cheat[counter].block+cheat[counter].len)){
				//Write out the address
				if(block[scounter].flags & FLAG_DMA){
				sprintf(buffer, "0x%08lX ", block[scounter].address);
								}
				else if(block[scounter].flags & FLAG_CWC){
				sprintf(buffer, "0x%08lX ", (block[scounter].address - 0x8800000 + 0x20000000));
								}
				else if(block[scounter].flags & FLAG_JOKER){
				sprintf(buffer, "0x%08lX ", (block[scounter].address));
								}
				else{
				sprintf(buffer, "0x%08lX ", (block[scounter].address - 0x08800000));
								}
				memcpy(SAVETEMP+ramcounter,buffer,11);
				ramcounter+=11;
				//Write out the value
				switch(block[scounter].flags & FLAG_DWORD){
					case FLAG_DWORD:
					sprintf(buffer, "0x%08lX\n", block[scounter].hakVal);
					memcpy(SAVETEMP+ramcounter,buffer,11);
					ramcounter+=11;
					break;

					case FLAG_WORD:
					sprintf(buffer, "0x%04hX\n", (unsigned short)block[scounter].hakVal);
					memcpy(SAVETEMP+ramcounter,buffer,7);
					ramcounter+=7;
					break;

					case FLAG_BYTE:
					sprintf(buffer, "0x%02hX\n", (unsigned char)block[scounter].hakVal);
					memcpy(SAVETEMP+ramcounter,buffer,5);
					ramcounter+=5;
					break;
				}
				//Next address
				scounter++;
			}
			//Next cheat
			counter++;
		}
	}
	//Close the file
	sceIoClose(fd);
	dump_memregion(gameDir, (void*)SAVETEMP, ramcounter);
	sceKernelDelayThread(1500);
	int i=0;
	for(i; i<(ramcounter>>2); i++){
	*(unsigned int *)(SAVETEMP+4*i)=0;
	}
	searchNo=0;
	cheatSearch=0;
	searchHistoryCounter=0;
	searchResultCounter=0;
}

void cheatLoad(){
	int fd;

	fd=sceIoOpen(gameDir, PSP_O_RDONLY, 0777);
	if(cheatRefresh){

		int counter=0;

		cheatSelected=counter;
		while(counter < cheatTotal){ //cycle through cheats in ram
			
			int scounter=0;
			while(scounter < blockTotal){ 
				//delete a cheat from ram
				block[cheatSelected].flags;
				block[cheatSelected].address;
				block[cheatSelected].stdVal;
				block[cheatSelected].hakVal;
				scounter++;
			}

			//reset the cheats info
			cheat[cheatSelected].block=0;
			cheat[cheatSelected].len=0;
			cheat[cheatSelected].flags=0;
			strcpy(cheat[cheatSelected].name, NULL);
			counter++;
		}

		cheatTotal=0;
		blockTotal=0;

		//delay
		sceKernelDelayThread(1500);
		cheatRefresh=0;

	}

	//Load the cheats
	if(fd > 0){
		unsigned int fileSize=sceIoLseek(fd, 0, SEEK_END); sceIoLseek(fd, 0, SEEK_SET);
		unsigned int fileOffset=0;
		unsigned char commentMode=0;
		unsigned char nameMode=0;

		while(fileOffset < fileSize){ 
			sceKernelDelayThread(1500);

			sceIoRead(fd, &buffer[0], 1);
			
			if((buffer[0]=='\r') || (buffer[0]=='\n')){
				commentMode=0;
				if(nameMode){
					cheatTotal++; 
					nameMode=0;
				}
			}
			else if((buffer[0]==' ') && (!nameMode)){}
			else if(buffer[0]==';'){commentMode=1; if(nameMode){cheatTotal++; nameMode=0;}} //Skip comments till next line
			else if(buffer[0]=='#'){ //Read in the cheat name
				if(cheatTotal >= NAME_MAX) { break;}
				cheat[cheatTotal].block=blockTotal;
				cheat[cheatTotal].flags=0;
				cheat[cheatTotal].len=0;
				cheat[cheatTotal].name[0]=0;
				nameMode=1;
			}
			else if((buffer[0]=='!') && (nameMode)){
				//Cheat's selected by default
				if(cheat[cheatTotal].flags & FLAG_SELECTED){ //Two ! = selected for constant on status
					cheat[cheatTotal].flags|=FLAG_CONSTANT;
					cheat[cheatTotal].flags&=~FLAG_SELECTED;
				}
				else{ //One ! = selected for music on/off button
					cheat[cheatTotal].flags|=FLAG_SELECTED;
				}
			}
			else if((!commentMode) && (nameMode)){
				if(nameMode<32){ //1 to 31 = letters, 32=Null terminator
					cheat[cheatTotal].name[nameMode-1]=buffer[0];
					nameMode++;
					cheat[cheatTotal].name[nameMode-1]=0;
				}
			}
			else if((!commentMode) && (!nameMode)){
				//Add 0xAABBCCDD 0xAABBCCDD block
				if(!blockAdd(fd, buffer)){
					//No more RAM?
					if(cheatTotal != 0){
						cheatTotal--;
						break;
					}
				}
				if(cheatTotal != 0){
					cheat[cheatTotal-1].len++;
				}
			}

			fileOffset=sceIoLseek(fd, 0, SEEK_CUR);
		}
		sceIoClose(fd);
	}
}

void buttonCallback(int curr, int last, void *arg){
  unsigned int counter;
  unsigned int scounter;
  unsigned int address;
  unsigned char analog[1];
  
 /*#ifdef _CFW_
  analog[1]=*(unsigned char *)(0x880E5ED5);//0x880E5ED4 ,0x88235684 635CUSTOM16 2k
  analog[0]=*(unsigned char *)(0x880E5ED4);//0x8822C894 0x88124904 TNC 1k,not worked
  if(analog[0] > 200){					//also i know &pad.Lx,but it work MKmenu only, never in game
	curr=curr | 0x0800; //analog right
	}
  else if(analog[0] < 40){//analog left
	curr=curr | 0x0400;
	}
  else if(analog[1] > 200){//analog down
	curr=curr | 0x0C00;
	}
   else{
	curr=curr & 0xFFFFFBFF;
	}
   #endif
   #ifdef _POPS_
  analog[1]=*(unsigned char *)(0x880DAED5);//0x880DAED5,635CUSTOM15 2k analogy
  analog[0]=*(unsigned char *)(0x880DAED4);//0x880DAED4,635CUSTOM15 2k analogx
  if(analog[0] > 200){					//also i know &pad.Lx,but it work MKmenu only, never in game
	curr=curr | 0x0800;
	}
  else if(analog[0] < 40){
	curr=curr | 0x0400;
	}
  else if(analog[1] > 200){
	curr=curr | 0x0C00;
	}
   else{
	curr=curr & 0xFFFFFBFF;
	}
   #endif
   */
  *(unsigned int *)(0x8800000+JOKERADDRESS)=curr;
	
  if(vram==NULL) return;

	
  if(((curr & menuKey) == menuKey) && (!menuDrawn)){
   	menuDrawn=1;
  	
    if(cheatSelected >= cheatTotal){
	tabSelected=0;cheatSelected=0;}
  }
  #ifdef _SCREENSHOT_
  else if(((curr & screenKey) == screenKey) && (!menuDrawn) && (screenshot_mode==1)){
  screenTime=1;
  #else
  else if(((curr & screenKey) == screenKey) && (!menuDrawn)){
  screenTime=1;
  #endif
  }
  else if(((curr & triggerKey) == triggerKey) && (!menuDrawn)){
	//Backup all the cheat "blocks"
	if(!cheatSaved){//unsaved cheat
		counter=0;
		scounter=0;
		while(counter < blockTotal){	
			if(cheat[scounter].block == counter){
			  cheatDMA=0; //Reset DNA on every new cheat
			  scounter++;
			}
			if(block[counter].flags & FLAG_DMA){
			  if(block[counter].hakVal!=0xFFFFFFFF){
				cheatDMA=*((unsigned int*)(0x08800000 + (block[counter].hakVal & 0x1FFFFFC))) - 0x08800000;
			  
				if(((cheatDMA >= 0x00004000) && (cheatDMA < 0x01800000)) || ((cheatDMA >= 0x40004000) && (cheatDMA < 0x41800000)))
				{
					block[counter].stdVal=cheatDMA;
				}
			  } 
			  else {
				cheatDMA=0;
			  }
			}
			else{
				address=cheatDMA+block[counter].address;
				if(((address >= 0x08800000) && (address < 0x0A000000)) || ((address >= 0x48800000) && (address < 0x4A000000))) {
					switch(block[counter].flags & FLAG_DWORD){
					   case FLAG_DWORD:
							if(address % 4 == 0){
								block[counter].stdVal=*((unsigned int*)(address));
							}
						break;
						case FLAG_WORD:
							if(address % 2 == 0){
								block[counter].stdVal=*((unsigned short*)(address));
							}
						break;
						case FLAG_BYTE:
							block[counter].stdVal=*((unsigned char*)(address));
						break;
					}
				}
			}
			counter++;
		}
	}

	//Turn on/off cheats
	cheatStatus=!cheatStatus;

	//Apply the cheats accordingly (make em fresh)
	counter=0;

	while(counter < cheatTotal){
	  if((cheat[counter].flags & FLAG_CONSTANT) && (!cheatSaved)){
		cheat[counter].flags|=FLAG_FRESH;
		cheatEnable(counter);
		cheat[counter].flags&=~FLAG_FRESH;
	  }
	  if(cheat[counter].flags & FLAG_SELECTED){
		cheat[counter].flags|=FLAG_FRESH;
		if(cheatStatus) {cheatEnable(counter);}
		else {cheatDisable(counter);}
		cheat[counter].flags&=~FLAG_FRESH;
	  }
	  counter++;
	}
	cheatSaved=1;
    
    //Wait 0.5 seconds
    sceKernelDelayThread(500000);
  }
}

static void gamePause(SceUID thid){
	if(pauseuid >= 0)
		return;
	pauseuid = thid;
	sceKernelGetThreadmanIdList(SCE_KERNEL_TMID_Thread, thread_buf_now, MAX_THREAD, &thread_count_now);
	int x, y, match;
	for(x = 0; x < thread_count_now; x++)
	{
		match = 0;
		SceUID tmp_thid = thread_buf_now[x];
		for(y = 0; y < thread_count_start; y++)
		{
			if((tmp_thid == thread_buf_start[y]) || (tmp_thid == thid))
			{
				match = 1;
				break;
			}
		}
		if(match == 0)
			sceKernelSuspendThread(tmp_thid);
	}
}

static void gameResume(SceUID thid){
	if(pauseuid != thid)
		return;
	pauseuid = -1;
	int x, y, match;
	for(x = 0; x < thread_count_now; x++)
	{
		match = 0;
		SceUID tmp_thid = thread_buf_now[x];
		for(y = 0; y < thread_count_start; y++)
		{
			if((tmp_thid == thread_buf_start[y]) || (tmp_thid == thid))
			{
				match = 1;
				break;
			}
		}
		if(match == 0)
			sceKernelResumeThread(tmp_thid);
	}
}

/*
#include "headers/logo.h"
//Telazorn functions
void telazornDraw(unsigned char buffer[], int height, int width){
  //110 x 31
  unsigned int counterY=0;
  unsigned int offset=70; //offset
  while(counterY < height) //height
  {
  	memcpy(&vram[(2*512*(counterY+4))+(360*2)], &buffer[offset], width*2);
    offset+=width*2; //16bpp <---double check this
    counterY++;
  }
}
*/

#include "headers/mips.h"
void menuDraw(){
	
	unsigned int counter;
	unsigned int scounter;
	unsigned int convBase;
	unsigned int convTotal;
	unsigned int tempbgcolor;
	
	//Draw the menu
	//if(cheatTotal==0 && tabSelected==0){
 	//tabSelected=1;}
	pspDebugScreenSetXY(0, 0);

  	if(copyMenu){
		counter=1; //we start @ 1 this time because 0 is closed
		tempbgcolor=bgcolor;
		if(extMenu ==1){
		countermax=7;}
		else if(tabSelected ==3 && flipme==0){	
				   if(pad.Buttons & SWAPBUTTON3){
					countermax=7;}
				   else{
					countermax=5;}
		}
		else{
		countermax=4;}
		while(counter < countermax+1){
			lineClear(counter+2); 
			pspDebugScreenSetXY(0, counter+2);
			pspDebugScreenSetBackColor(tempbgcolor);
			if(copyMenu == counter){
				//Highlight the selection
				pspDebugScreenSetTextColor(color01);
			}
			else{
				//Don't highlight the selection
				pspDebugScreenSetTextColor(color02);
			}
			switch(counter){
				case 1:
				if(tabSelected==3 && (pad.Buttons & SWAPBUTTON3)){
					pspDebugScreenPuts("  Jump to copyStart\n");}
				else{
				  if((extMenu == 1) && ((block[extSelected[0]].flags & FLAG_DMA) || (block[extSelected[0]].flags & FLAG_JOKER))){
				  if(pad.Buttons & SWAPBUTTON3){
				  pspDebugScreenPuts("  DMA_BASE");}
				  else{
				  pspDebugScreenPuts("  JOKER/DMA");}}
				pspDebugScreenPuts("  Copy address\n");} break;

				case 2:
				if(tabSelected==3 && (pad.Buttons & SWAPBUTTON3)){
					pspDebugScreenPuts("  Jump to copyEnd\n");}
				else{	
				if((extMenu == 1) && ((block[extSelected[0]].flags & FLAG_DMA) || (block[extSelected[0]].flags & FLAG_JOKER))){
				  pspDebugScreenPuts("  JOKER/DMA");}
				pspDebugScreenPuts("  Paste address\n");} break;
				case 3:
				pspDebugScreenPuts("  Copy value\n");
				break;
				case 4:
				if((pad.Buttons & SWAPBUTTON3) && (tabSelected==3 && flipme==0)){
				if((searchHistory[0].hakVal!=0x03E00008) || extMenu == 1){
				pspDebugScreenPuts("  Paste Converted Jumpaddress+8\n");
				}
				else if(searchHistory[0].hakVal==0x03E00008){
				pspDebugScreenPuts("  Quick Hook Find by break exception\n");
				}
				}
				else{
				pspDebugScreenPuts("  Paste value\n");
				}
				break;
				case 5:
				if(tabSelected==3 && flipme==0){
				   if(pad.Buttons & SWAPBUTTON3){
					pspDebugScreenPuts("  Clear Jump log\n");
					}
				   else if(((decodeAddress[bdNo]+(decodeY[bdNo]*4)) >= (logstart-4)) && ((decodeAddress[bdNo]+(decodeY[bdNo]*4)) <= (logstart + 4*jumplog))){
					pspDebugScreenPuts("  Back to Decoder, [] = switch menu\n");}
				   else{
					pspDebugScreenPuts("  View Jump log, [] = switch menu,");
					sprintf(buffer," mergefloat_line:%d\n",backline);pspDebugScreenPuts(buffer);}
				    }
				else{
				if(pad.Buttons & SWAPBUTTON3){
				pspDebugScreenPuts("  CWC cheat(NoBackup)\n");}
				else{
				pspDebugScreenPuts("  NitePR cheat\n");}
				}break;
				case 6: 				
				if(tabSelected ==3){
				   if(pad.Buttons & SWAPBUTTON3){
				   		pspDebugScreenPuts("  Selected Range by NOTE to new cheat\n");
				   }
				}  
				else{   
				pspDebugScreenPuts("  DMA cheat\n");
				}
				break;

				case 7:
				if(tabSelected ==3){
				   if(pad.Buttons & SWAPBUTTON3){
					pspDebugScreenPuts("  Selected Range by NOTE to text file\n");
					}
				}
				else{
				sprintf(buffer ,"  JOKER cheat (0x");
				pspDebugScreenPuts(buffer);
				if(pad.Buttons & SWAPBUTTON3){
				sprintf(buffer ,"0000 MASKED)\n");
				}
				else{
				sprintf(buffer ,"%X default)\n" ,JOKERADDRESS);
				}
				pspDebugScreenPuts(buffer);
				}
				break;
					}
			counter++;
			tempbgcolor+=0x00000008;
		}
		//Helper
		pspDebugScreenSetTextColor(color02); 
		pspDebugScreenSetBackColor(bgcolor);
		pspDebugScreenPuts(line); //draw spiffy line
  	}

	else if(extMenu){
		switch(extMenu){
			
			case 1: //DRAW EXT CHEAT
				//Draw the tabs
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("  [Editing Cheat: '"); pspDebugScreenPuts(cheat[cheatSelected].name); pspDebugScreenPuts("'] ");
				pspDebugScreenPuts("\n"); pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				
				//draw some info 
				if(editFormat==0){
					pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("  Address     Value.Hex   Value.Dec   ASCII Value.Float \n");
				}
				else{
					pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("  Address     Value.Hex   Opcode   Args                        \n");
				}

				//Print out the cheat lines
				convBase=cheat[cheatSelected].block;
				convTotal=cheat[cheatSelected].len;
				counter=cheat[cheatSelected].block;
				unsigned int DMAADDRESS=0;
				while(counter < (cheat[cheatSelected].block+cheat[cheatSelected].len)){
					
					//Scroll feature right here, in two lines =3
					//if((signed int)(counter-convBase) < (signed int)(((extSelected[0]-convBase)-11) - (( ((signed int)(extSelected[0]-convBase)+11) - ((signed int)convTotal))>0? abs(((signed int)(extSelected[0]-convBase)+11) - ((signed int)convTotal)): 0)   )) {counter++; continue;}
					//if((signed int)(counter-convBase) > (signed int)(((extSelected[0]-convBase)+11) + (((signed int)(extSelected[0]-convBase)-11)<0? abs((signed int)((extSelected[0]-convBase)-11)): 0)   )) {counter++; continue;}
					if((signed int)(counter-convBase) < (signed int)(((extSelected[0]-convBase)-13) - (( ((signed int)(extSelected[0]-convBase)+13) - ((signed int)convTotal))>0? abs(((signed int)(extSelected[0]-convBase)+13) - ((signed int)convTotal)): 0)   )) {counter++; continue;}
					if((signed int)(counter-convBase) > (signed int)(((extSelected[0]-convBase)+13) + (((signed int)(extSelected[0]-convBase)-13)<0? abs((signed int)((extSelected[0]-convBase)-13)): 0)   )) {counter++; continue;}

					//Apply the row color
					if(counter == extSelected[0]){
						pspDebugScreenSetTextColor(color01);
					}
					else{
						pspDebugScreenSetTextColor(color02);
					}

					//Print out the address
					if(block[counter].flags & FLAG_DMA){
						sprintf(buffer,"  0xFFFFFFFF  ");DMAADDRESS=0x08800000;
						if(decodeFormat==0x48800000){ DMAADDRESS=0;}
					}
					else if(block[counter].flags & FLAG_JOKER){
						sprintf(buffer, "  0x%08lX  ", (block[counter].address));
					}
					else if(block[counter].flags & FLAG_CWC){
						sprintf(buffer, "  0x%08lX  ", (block[counter].address - decodeFormat + 0x40000000 -DMAADDRESS) + 0x20000000);
					}
					else{
						sprintf(buffer, "  0x%08lX  ", block[counter].address - decodeFormat + 0x40000000 -DMAADDRESS);
					}
					pspDebugScreenPuts(buffer);
						
					//Print out the hex
					if(block[counter].address >=0xFF000000){
							sprintf(buffer, "0x%08lX  ", block[counter].hakVal - decodeFormat + 0x48800000);
				    }
					else if((block[counter].flags & FLAG_DWORD)==FLAG_DWORD){
							sprintf(buffer, "0x%08lX  ", block[counter].hakVal);
					}
					else if((block[counter].flags &FLAG_DWORD)== FLAG_WORD){
							sprintf(buffer, "0x____%04hX  ", (unsigned short)block[counter].hakVal);
					}
					else if((block[counter].flags &FLAG_DWORD)== FLAG_BYTE){
							sprintf(buffer, "0x______%02hX  ", (unsigned char)block[counter].hakVal);
					}
					pspDebugScreenPuts(buffer);

					//print opcode or ascii and decimal
					if(editFormat==0){

						//Print out the decimal
						switch(block[counter].flags & FLAG_DWORD){
							case FLAG_DWORD:
								sprintf(buffer, "%010lu  ", block[counter].hakVal);
							break;
							case FLAG_WORD:
								sprintf(buffer, "%010lu  ", (unsigned short)block[counter].hakVal);
							break;
							case FLAG_BYTE:
								sprintf(buffer, "%010lu  ", (unsigned char)block[counter].hakVal);
							break;
						}

						pspDebugScreenPuts(buffer);

						//Print out the ASCII
						buffer[0]=*((unsigned char*)(((unsigned int)&block[counter].hakVal)+0)); if((buffer[0]<=0x20) || (buffer[0]==0xFF)) buffer[0]='.';
						buffer[1]=*((unsigned char*)(((unsigned int)&block[counter].hakVal)+1)); if((buffer[1]<=0x20) || (buffer[1]==0xFF)) buffer[1]='.';
						buffer[2]=*((unsigned char*)(((unsigned int)&block[counter].hakVal)+2)); if((buffer[2]<=0x20) || (buffer[2]==0xFF)) buffer[2]='.';
						buffer[3]=*((unsigned char*)(((unsigned int)&block[counter].hakVal)+3)); if((buffer[3]<=0x20) || (buffer[3]==0xFF)) buffer[3]='.';
						buffer[4]=0;
						pspDebugScreenPuts(buffer);

						//Print out the float
						if((block[counter].address & 0x2) == 0x2){
					    	addresscode=(block[counter].hakVal & 0xFFFF) | 0x3C000000;
						mipsSpecial(addresscode,addresstmp,counteraddress);
						//pspDebugScreenPuts("  UFLOAT:");//UPPER FLAOT
						//unsigned int upperfloat=block[counter].hakVal <<16;
						//f_cvt(&upperfloat, buffer, sizeof(buffer), 6, MODE_GENERIC);
						}
						else{
						pspDebugScreenPuts("  ");
						f_cvt(&block[counter].hakVal, buffer, sizeof(buffer), 6, MODE_GENERIC);
						pspDebugScreenPuts(buffer);}
					}
					else{
						//Print out the opcode
						if(block[counter].flags & FLAG_BYTE){
						}
						else{
							if(block[counter].flags & FLAG_DMA){
							pspDebugScreenPuts("DMA CHEAT");
							}
							//else if(block[counter].flags & FLAG_CWC){
							//pspDebugScreenPuts("CWC CHEAT");
							//}
							else if(block[counter].flags & FLAG_JOKER){
								if(block[counter].hakVal == 0){

								pspDebugScreenPuts("JOKER CHEAT(24BITMASKED)");}
								else if(block[counter].hakVal == JOKERADDRESS){
								pspDebugScreenPuts("JOKER CHEAT(16BIT_TEST)");}
								else{
								pspDebugScreenPuts("16BIT_TEST");}
							}
							else{
							 if(block[counter].flags & FLAG_DWORD){
							        if(block[counter].flags & FLAG_WORD){
							    	 if((block[counter].address & 0x2) == 0x2){
							    	 addresscode=(block[counter].hakVal)<<16;
							    	 mipsDecode(addresscode);
								 }
							        else if(block[counter].flags & FLAG_DWORD){
								mipsDecode(block[counter].hakVal);
								addresscode=block[counter].hakVal;
								counteraddress=block[counter].address;
								mipsSpecial(addresscode,addresstmp,counteraddress);}
							    	}
							 }
							}
						}
					}

					//Skip a line, draw the pointer =)
					if(counter == extSelected[0]){
						//Skip the initial line
						pspDebugScreenPuts("\n");

						//Skip the desired amount?
						pspDebugScreenPuts("    ");
						if(extSelected[1] != 0){ //Skip address
							pspDebugScreenPuts("            "); 
							if(extSelected[1] != 1){ //Skip Hex
								pspDebugScreenPuts("          "); 
								if(extSelected[1] != 2){ //Skip Dec
									pspDebugScreenPuts("            "); 
									//Skip ASCII
								}
							}
						}

						//Skip the minimalist amount
						unsigned char tempCounter=extSelected[2];
						while(tempCounter){
							pspDebugScreenPuts(" "); 
							tempCounter--;
						}

						//Draw the symbol (Finally!!)
						if(extSelected[3]){
							pspDebugScreenSetTextColor(color01);
						}
						else{
							pspDebugScreenSetTextColor(color03);
						}
						pspDebugScreenPuts("^");
					}

					//Goto the next cheat down under
					pspDebugScreenPuts("\n");
					counter++;
				}

				//Helper
				pspDebugScreenSetTextColor(color02); 
				pspDebugScreenSetXY(0, 32);
				pspDebugScreenPuts(line); //draw spiffy line
				pspDebugScreenSetXY(0, 33);
				pspDebugScreenSetTextColor(color01); pspDebugScreenPuts(">< = Edit On/Off; D-PAD = Cycle; [] = Alt-Type; () = Close");
			break;
			
			case 2: //DRAW EXT SEARCH
				//Draw the tabs
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("  [Editing Search]");
				pspDebugScreenPuts("\n"); pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				//draw some info 
				if(editFormat==0){
					pspDebugScreenPuts("  Value.Hex   Value.Dec   ASCII Value.Float\n");
				}
				else{
					pspDebugScreenPuts("  Value.Hex   Opcode   Args\n");
				}

				//Apply the row color
				if(extSelected[0] == 0){
					pspDebugScreenSetTextColor(color01);
				}
				else{
					pspDebugScreenSetTextColor(color02);
				}

				//Print out the hex
				switch(searchHistory[0].flags & FLAG_DWORD){
					case FLAG_DWORD:
						sprintf(buffer, "  0x%08lX  ", searchHistory[0].hakVal);
					break;
					case FLAG_WORD:
						sprintf(buffer, "  0x____%04hX  ", (unsigned short)searchHistory[0].hakVal);
					break;
					case FLAG_BYTE:
						sprintf(buffer, "  0x______%02hX  ", (unsigned char)searchHistory[0].hakVal);
					break;
				}
				pspDebugScreenPuts(buffer);

				//choose info to print
				if(editFormat==0){
					//Print out the decimal
					switch(searchHistory[0].flags & FLAG_DWORD){
						case FLAG_DWORD:
							sprintf(buffer, "%010u  ", searchHistory[0].hakVal);
						break;
						case FLAG_WORD:
							sprintf(buffer, "%010u  ", (unsigned short)searchHistory[0].hakVal);
						break;
						case FLAG_BYTE:
							sprintf(buffer, "%010u  ", (unsigned char)searchHistory[0].hakVal);
						break;
					}
					pspDebugScreenPuts(buffer);

					//Print out the ASCII
					buffer[0]=*((unsigned char*)(((unsigned int)&searchHistory[0].hakVal)+0)); if((buffer[0]<=0x20) || (buffer[0]==0xFF)) buffer[0]='.';
					buffer[1]=*((unsigned char*)(((unsigned int)&searchHistory[0].hakVal)+1)); if((buffer[1]<=0x20) || (buffer[1]==0xFF)) buffer[1]='.';
					buffer[2]=*((unsigned char*)(((unsigned int)&searchHistory[0].hakVal)+2)); if((buffer[2]<=0x20) || (buffer[2]==0xFF)) buffer[2]='.';
					buffer[3]=*((unsigned char*)(((unsigned int)&searchHistory[0].hakVal)+3)); if((buffer[3]<=0x20) || (buffer[3]==0xFF)) buffer[3]='.';
					buffer[4]=0;
					pspDebugScreenPuts(buffer);

					//Print out the float
					if((searchHistory[0].flags & FLAG_DWORD) == FLAG_BYTE){
					}
					else{
					if((searchHistory[0].flags & FLAG_DWORD) == FLAG_WORD){
				    	addresscode=(searchHistory[0].hakVal & 0xFFFF) | 0x3C000000;
					mipsSpecial(addresscode,addresstmp,counteraddress);
				    	addresscode=(searchHistory[0].hakVal & 0xFFFF) | 0xDF800000;
					  if(((searchHistory[0].flags & FLAG_DWORD) == FLAG_WORD) && extMenu == 2 && ((addresscode & 0x7FFF) > 0x38D1) && ((addresscode & 0x7FFF) < 0x4B19)){
						}
					  else{
					pspDebugScreenPuts(" ");
					mipsSpecial(addresscode,addresstmp,counteraddress);}
					//pspDebugScreenPuts("  UFLOAT:");
					//unsigned int upperfloat=searchHistory[0].hakVal <<16;
					//f_cvt(&upperfloat, buffer, sizeof(buffer), 6, MODE_GENERIC);
					}
					else{
					pspDebugScreenPuts("  ");
					f_cvt(&searchHistory[0].hakVal, buffer, sizeof(buffer), 6, MODE_GENERIC);
					pspDebugScreenPuts(buffer);}
					}
				}
				else{
					//Print out the opcode
					if((searchHistory[0].flags & FLAG_DWORD) == FLAG_BYTE){
					}
					else{
					offsetadd=1;
					if((searchHistory[0].flags & FLAG_DWORD) == FLAG_WORD){
				    	 addresscode=(searchHistory[0].hakVal)<<16;
				    	 mipsDecode(addresscode);}
					else if((searchHistory[0].flags & FLAG_DWORD) == FLAG_DWORD){
					mipsDecode(searchHistory[0].hakVal);
					addresscode=searchHistory[0].hakVal;}
					counteraddress=searchHistory[0].address;
					mipsSpecial(addresscode,addresstmp,counteraddress);
					}
					offsetadd=0;
				}

				//Skip a line, draw the pointer =)
				pspDebugScreenPuts("\n");

				if(extSelected[0] == 0){
					//Skip the desired amount?
					pspDebugScreenPuts("    ");
					if(extSelected[1] != 0){ //Skip Hex
						pspDebugScreenPuts("          "); 
						if(extSelected[1] != 1){ //Skip Dec
							pspDebugScreenPuts("            "); 
							//Skip ASCII
						}
					}

					//Skip the minimalist amount
					unsigned char tempCounter=extSelected[2];
					while(tempCounter){
						pspDebugScreenPuts(" "); 
						tempCounter--;
					}

					//Draw the symbol (Finally!!)
					if(extSelected[3]){
						pspDebugScreenSetTextColor(color01);
					}
					else{
						pspDebugScreenSetTextColor(color03);
					}
					pspDebugScreenPuts("^");
				}

				if(((searchHistory[0].flags & FLAG_DWORD) == FLAG_WORD) && extMenu == 2 && ((addresscode & 0x7FFF) > 0x38D1) && ((addresscode & 0x7FFF) < 0x4B19)){
				pspDebugScreenSetXY(31, 5);
				pspDebugScreenSetTextColor(color01);
				mipsSpecial(addresscode,addresstmp,counteraddress);
				}
				pspDebugScreenPuts("\n");

				//Draw the misc menus
				pspDebugScreenSetTextColor(extSelected[0] == 1? color01: color02); pspDebugScreenPuts("  Search\n");
				if(searchNo){pspDebugScreenSetTextColor(extSelected[0] == 2? color01: color02); pspDebugScreenPuts("  Undo Search\n");}

				//Print out results
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("  [Search Results: "); sprintf(buffer, "%d Found - Only showing first %d]", searchResultCounter,searchResultMax); pspDebugScreenPuts(buffer);
				pspDebugScreenPuts("\n"); pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color02); 
				if(editFormat==0){
				pspDebugScreenPuts("  Address     Value.Hex   Value.Dec   ASCII Value.Float\n");}
				else{
				pspDebugScreenPuts("  Address     Value.Hex   Opcode   Args(alinged address)\n");}

				//Print out the results variables
				convTotal=((searchResultCounter > searchResultMax)? searchResultMax:searchResultCounter);
				counter=0;
				while(counter < convTotal){
					//Scroll feature right here, in two lines =3
					if((signed int)(counter) < (signed int)(((extSelected[0]-3)-10) - (( ((signed int)(extSelected[0]-3)+10) - ((signed int)convTotal))>0? abs(((signed int)(extSelected[0]-3)+10) - ((signed int)convTotal)): 0)   )) {counter++; continue;}
					if((signed int)(counter) > (signed int)(((extSelected[0]-3)+10) + (((signed int)(extSelected[0]-3)-10)<0? abs((signed int)((extSelected[0]-3)-10)): 0)   )) {counter++; continue;}
					//if((signed int)(counter) < (signed int)(((extSelected[0]-3)-7) - (( ((signed int)(extSelected[0]-3)+7) - ((signed int)convTotal))>0? abs(((signed int)(extSelected[0]-3)+7) - ((signed int)convTotal)): 0)   )) {counter++; continue;}
					//if((signed int)(counter) > (signed int)(((extSelected[0]-3)+7) + (((signed int)(extSelected[0]-3)-7)<0? abs((signed int)((extSelected[0]-3)-7)): 0)   )) {counter++; continue;}

					//Apply the row color
					if(counter == (extSelected[0]-3)){
						pspDebugScreenSetTextColor(color01);
					}
					else{
						pspDebugScreenSetTextColor(color02);
					}

					//Print out the address
					sprintf(buffer, "  0x%08lX  ", (searchAddress[counter] - decodeFormat));
					pspDebugScreenPuts(buffer);

					//Print out the hex
					switch(searchHistory[0].flags & FLAG_DWORD){
						case FLAG_DWORD:
							sprintf(buffer, "0x%08lX  ", *((unsigned int*)(searchAddress[counter]&0xFFFFFFFC)));
						break;
						case FLAG_WORD:
							sprintf(buffer, "0x____%04hX  ", *((unsigned short*)(searchAddress[counter]&0xFFFFFFFE)));
						break;
						case FLAG_BYTE:
							sprintf(buffer, "0x______%02hX  ", *((unsigned char*)(searchAddress[counter])));
						break;
					}
					pspDebugScreenPuts(buffer);

				if(editFormat==0){
					//Print out the decimal
					switch(searchHistory[0].flags & FLAG_DWORD){
						case FLAG_DWORD:
							sprintf(buffer, "%010lu  ", *((unsigned int*)(searchAddress[counter]&0xFFFFFFFC)));
						break;
						case FLAG_WORD:
							sprintf(buffer, "%010lu  ", *((unsigned short*)(searchAddress[counter]&0xFFFFFFFE)));
						break;
						case FLAG_BYTE:
							sprintf(buffer, "%010lu  ", *((unsigned char*)(searchAddress[counter])));
						break;
					}
					pspDebugScreenPuts(buffer);
				
					//Print out the ASCII
					buffer[0]=*((unsigned char*)(searchAddress[counter]+0)); if((buffer[0]<=0x20) || (buffer[0]==0xFF)) buffer[0]='.';
					if((searchHistory[0].flags & FLAG_DWORD) != FLAG_BYTE){
						buffer[1]=*((unsigned char*)(searchAddress[counter]+1)); if((buffer[1]<=0x20) || (buffer[1]==0xFF)) buffer[1]='.';
					}
					else{
						buffer[1]='.';
					}
					if((searchHistory[0].flags & FLAG_DWORD) == FLAG_DWORD){
						buffer[2]=*((unsigned char*)(searchAddress[counter]+2)); if((buffer[2]<=0x20) || (buffer[2]==0xFF)) buffer[2]='.';
						buffer[3]=*((unsigned char*)(searchAddress[counter]+3)); if((buffer[3]<=0x20) || (buffer[3]==0xFF)) buffer[3]='.';
					}
					else{
						buffer[2]=buffer[3]='.';
					}
					buffer[4]=0;
					pspDebugScreenPuts(buffer);

					//Print out the float
					pspDebugScreenPuts("  ");
					f_cvt(searchAddress[counter], buffer, sizeof(buffer), 6, MODE_GENERIC);
					pspDebugScreenPuts(buffer);
				}
				else{//decode for aligned address
				    	addresscode=*(unsigned int *)(searchAddress[counter] & 0xFFFFFFC);
				    	mipsDecode(addresscode);
					counteraddress=searchAddress[counter] & 0xFFFFFFC;
					mipsSpecial(addresscode,addresstmp,counteraddress);
				}

					//Goto the next cheat down under
					pspDebugScreenPuts("\n");
					counter++;
				}

				//Helper
				pspDebugScreenSetTextColor(color02); 
				pspDebugScreenSetXY(0, 32);
				pspDebugScreenPuts(line); //draw spiffy line
				pspDebugScreenSetXY(0, 33);
				pspDebugScreenSetTextColor(color01);
				if(extSelected[0] == 0){
					if(searchNo == 0){
						pspDebugScreenPuts(">< = Edit On/Off; D-PAD = Cycle; [] = Alt-Type; () = Cancel");
					}
					else{
						pspDebugScreenPuts(">< = Edit On/Off; D-PAD = Cycle; () = Cancel");
					}
				}
				else if((extSelected[0] == 1) || (extSelected[0] == 2)){
					pspDebugScreenPuts(">< = Select; () = Cancel");  
				}
				else{
					pspDebugScreenPuts("START = Switch Decoder/Val; >< = Add Selected Cheat; () = Cancel");
				}
			break;
			
			case 3: //DRAW EXT DIFF SEARCH
				//Draw the tabs
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("  [Editing Search]\n");
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color02); 
				if(searchMode > 3){
					pspDebugScreenPuts("  Mode          Value.Hex   Value.Dec\n");
				}
				else{
					pspDebugScreenPuts("  Mode\n");
				}

				//Apply the row color
				if(extSelected[0] == 0){
					pspDebugScreenSetTextColor(color01);
				}
				else{
					pspDebugScreenSetTextColor(color02);
				}

				//Print out the mode name
				pspDebugScreenPuts(searchModeName[searchMode]);

			if(searchMode > 3){
					
					pspDebugScreenPuts("   ");
					
					//Print out the hex
					switch(searchHistory[0].flags & FLAG_DWORD){
						case FLAG_DWORD:
							sprintf(buffer, "  0x%08lX  ", searchHistory[0].hakVal);
						break;
						case FLAG_WORD:
							sprintf(buffer, "  0x____%04hX  ", (unsigned short)searchHistory[0].hakVal);
						break;
						case FLAG_BYTE:
							sprintf(buffer, "  0x______%02hX  ", (unsigned char)searchHistory[0].hakVal);
						break;
					}
					pspDebugScreenPuts(buffer);

					//Print out the decimal
					switch(searchHistory[0].flags & FLAG_DWORD){
						case FLAG_DWORD:
							sprintf(buffer, "%010u  ", searchHistory[0].hakVal);
						break;
						case FLAG_WORD:
							sprintf(buffer, "%010u  ", (unsigned short)searchHistory[0].hakVal);
						break;
						case FLAG_BYTE:
							sprintf(buffer, "%010u  ", (unsigned char)searchHistory[0].hakVal);
						break;
					}
					pspDebugScreenPuts(buffer);

					//Print out the ASCII
					/*buffer[0]=*((unsigned char*)(((unsigned int)&searchHistory[0].hakVal)+0)); if((buffer[0]<=0x20) || (buffer[0]==0xFF)) buffer[0]='.';
					buffer[1]=*((unsigned char*)(((unsigned int)&searchHistory[0].hakVal)+1)); if((buffer[1]<=0x20) || (buffer[1]==0xFF)) buffer[1]='.';
					buffer[2]=*((unsigned char*)(((unsigned int)&searchHistory[0].hakVal)+2)); if((buffer[2]<=0x20) || (buffer[2]==0xFF)) buffer[2]='.';
					buffer[3]=*((unsigned char*)(((unsigned int)&searchHistory[0].hakVal)+3)); if((buffer[3]<=0x20) || (buffer[3]==0xFF)) buffer[3]='.';
					buffer[4]=0;
					pspDebugScreenPuts(buffer);

				  	if((searchHistory[0].flags & FLAG_DWORD) == FLAG_BYTE){
				   	}
				   	else{
						if((searchHistory[0].flags & FLAG_DWORD) == FLAG_WORD){
					    	addresscode=(searchHistory[0].hakVal & 0xFFFF) | 0x3C000000;
						mipsSpecial(addresscode,addresstmp,counteraddress);
					    	addresscode=(searchHistory[0].hakVal & 0xFFFF) | 0xDF800000;
						pspDebugScreenPuts(" ");
						mipsSpecial(addresscode,addresstmp,counteraddress);
						//pspDebugScreenPuts("  UFLOAT:");
						//unsigned int upperfloat=searchHistory[0].hakVal <<16;
						//f_cvt(&upperfloat, buffer, sizeof(buffer), 6, MODE_GENERIC);
						}
						else{
						pspDebugScreenPuts("  ");
						f_cvt(&searchHistory[0].hakVal, buffer, sizeof(buffer), 6, MODE_GENERIC);
						pspDebugScreenPuts(buffer);
						}
					}*/
			}

				//Skip a line, draw the pointer =)
				pspDebugScreenPuts("\n");
				if(extSelected[0] == 0){
					//Skip the desired amount?
					pspDebugScreenPuts("    ");
					if(extSelected[1] != 0) //Skip Mode
					{
						pspDebugScreenPuts("              "); 
						if(extSelected[1] != 1) //Skip Hex
						{
						pspDebugScreenPuts("          "); 
							if(extSelected[1] != 2) //Skip Dec
							{
								pspDebugScreenPuts("            "); 
								//Skip ASCII
							}
						}
					}

					//Skip the minimalist amount
					unsigned char tempCounter=extSelected[2];
					while(tempCounter){
						pspDebugScreenPuts(" "); 
						tempCounter--;
					}

					//Draw the symbol (Finally!!)
					if(extSelected[3]){
						pspDebugScreenSetTextColor(color01);
					}
					else{
						pspDebugScreenSetTextColor(color03);
					}
					pspDebugScreenPuts("^");
				}
				pspDebugScreenPuts("\n");

				//Draw the misc menus
				pspDebugScreenSetTextColor(extSelected[0] == 1? color01: color02); pspDebugScreenPuts("  Search\n");
				if(searchNo){pspDebugScreenSetTextColor(extSelected[0] == 2? color01: color02); pspDebugScreenPuts("  Undo Search\n");}

				//Print out results
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("  [Search Results: "); sprintf(buffer, "%d Found - Only showing first %d]", searchResultCounter,searchResultMax); pspDebugScreenPuts(buffer);
				pspDebugScreenPuts("\n"); pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color02); 
				if(editFormat==0){
				pspDebugScreenPuts("  Address     Value.Hex   Value.Dec   ASCII Value.Float\n");}
				else{
				pspDebugScreenPuts("  Address     Value.Hex   Opcode   Args(aligned address)\n");}

				//Print out the results variables
				convTotal=((searchResultCounter > searchResultMax)? searchResultMax:searchResultCounter);
				counter=0;
				while(counter < convTotal){
					//Scroll feature right here, in two lines =3
					//if((signed int)(counter) < (signed int)(((extSelected[0]-3)-9) - (( ((signed int)(extSelected[0]-3)+9) - ((signed int)convTotal))>0? abs(((signed int)(extSelected[0]-3)+9) - ((signed int)convTotal)): 0)   )) {counter++; continue;}
					//if((signed int)(counter) > (signed int)(((extSelected[0]-3)+9) + (((signed int)(extSelected[0]-3)-9)<0? abs((signed int)((extSelected[0]-3)-9)): 0)   )) {counter++; continue;}
					if((signed int)(counter) < (signed int)(((extSelected[0]-3)-10) - (( ((signed int)(extSelected[0]-3)+10) - ((signed int)convTotal))>0? abs(((signed int)(extSelected[0]-3)+10) - ((signed int)convTotal)): 0)   )) {counter++; continue;}
					if((signed int)(counter) > (signed int)(((extSelected[0]-3)+10) + (((signed int)(extSelected[0]-3)-10)<0? abs((signed int)((extSelected[0]-3)-10)): 0)   )) {counter++; continue;}

					//Apply the row color
					if(counter == (extSelected[0]-3)){
						pspDebugScreenSetTextColor(color01);
					}
					else{

						pspDebugScreenSetTextColor(color02);
					}

					//Print out the address
					sprintf(buffer, "  0x%08lX  ", (searchAddress[counter] - decodeFormat));
					pspDebugScreenPuts(buffer);

					//Print out the hex
					switch(searchHistory[0].flags & FLAG_DWORD){
						case FLAG_DWORD:
							sprintf(buffer, "0x%08lX  ", *((unsigned int*)(searchAddress[counter]&0xFFFFFFFC)));
						break;
						case FLAG_WORD:
							sprintf(buffer, "0x____%04hX  ", *((unsigned short*)(searchAddress[counter]&0xFFFFFFFE)));
						break;
						case FLAG_BYTE:
							sprintf(buffer, "0x______%02hX  ", *((unsigned char*)(searchAddress[counter])));
						break;
					}
					pspDebugScreenPuts(buffer);
					
				if(editFormat==0){
					//Print out the decimal
					switch(searchHistory[0].flags & FLAG_DWORD){
						case FLAG_DWORD:
							sprintf(buffer, "%010lu  ", *((unsigned int*)(searchAddress[counter]&0xFFFFFFFC)));
						break;
						case FLAG_WORD:
							sprintf(buffer, "%010lu  ", *((unsigned short*)(searchAddress[counter]&0xFFFFFFFE)));
						break;
						case FLAG_BYTE:
							sprintf(buffer, "%010lu  ", *((unsigned char*)(searchAddress[counter])));
						break;
					}
					pspDebugScreenPuts(buffer);

					//Print out the ASCII
					buffer[0]=*((unsigned char*)(searchAddress[counter]+0)); if((buffer[0]<=0x20) || (buffer[0]==0xFF)) buffer[0]='.';
					if((searchHistory[0].flags & FLAG_DWORD) != FLAG_BYTE){
						buffer[1]=*((unsigned char*)(searchAddress[counter]+1)); if((buffer[1]<=0x20) || (buffer[1]==0xFF)) buffer[1]='.';
					}
					else{
						buffer[1]='.';
					}
					if((searchHistory[0].flags & FLAG_DWORD) == FLAG_DWORD){
						buffer[2]=*((unsigned char*)(searchAddress[counter]+2)); if((buffer[2]<=0x20) || (buffer[2]==0xFF)) buffer[2]='.';
						buffer[3]=*((unsigned char*)(searchAddress[counter]+3)); if((buffer[3]<=0x20) || (buffer[3]==0xFF)) buffer[3]='.';
					}
					else{
						buffer[2]=buffer[3]='.';
					}
					buffer[4]=0;
					pspDebugScreenPuts(buffer);

					//Print out the float
					pspDebugScreenPuts("  ");
					f_cvt(searchAddress[counter], buffer, sizeof(buffer), 6, MODE_GENERIC);
					pspDebugScreenPuts(buffer);

				}
				else{ //decode for aligned address
				    	addresscode=*(unsigned int *)(searchAddress[counter] & 0xFFFFFFC);
				    	mipsDecode(addresscode);
					counteraddress=searchAddress[counter] & 0xFFFFFFC;
					mipsSpecial(addresscode,addresstmp,counteraddress);
				}

					//Goto the next cheat down under
					pspDebugScreenPuts("\n");
					counter++;
				}

				//Helper
				pspDebugScreenSetTextColor(color02); 
				pspDebugScreenSetXY(0, 32);
				pspDebugScreenPuts(line); //draw spiffy line
				pspDebugScreenSetXY(0, 33);
				pspDebugScreenSetTextColor(color01);
				if(extSelected[0] == 0){
					pspDebugScreenPuts(">< = Edit On/Off; D-PAD = Cycle; () = Cancel");
				}
				else if((extSelected[0] == 1) || (extSelected[0] == 2)){
					pspDebugScreenPuts(">< = Select; () = Cancel");  
				}
				else{
					pspDebugScreenPuts("START = Switch Dec/Val; >< = Add Selected Cheat; () = Cancel");
				}
			break;

			case 4: //DRAW EXT TEXT search
				//Draw the tabs
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color01); //pspDebugScreenPuts("  [Editing Search]\n");
				sprintf(buffer,"[Editing Search] A/a;%c WD;%c\n",0x59+0xB*BIGSMALL,0x59+0xB*FAKEHYOGEN);
				pspDebugScreenPuts(buffer);
				pspDebugScreenSetTextColor(color02);
				pspDebugScreenPuts(line);
				pspDebugScreenPuts("  Text\n");

				//Apply the row color
				if(extSelected[0] == 0){
					pspDebugScreenSetTextColor(color01);
				}
				else{
					pspDebugScreenSetTextColor(color02);
				}

				//Print out the ASCII
				pspDebugScreenPuts("  '");
				fileBuffer[33]=0;
				pspDebugScreenPuts(fileBuffer);

				//Skip a line, draw the pointer =)
				pspDebugScreenPuts("'\n");
				if(extSelected[0] == 0){
					//Skip the desired amount?
					pspDebugScreenPuts("   ");

					//Skip the minimalist amount
					unsigned char tempCounter=extSelected[2];
					while(tempCounter){
						pspDebugScreenPuts(" "); 
						tempCounter--;
					}

					//Draw the symbol (Finally!!)
					if(extSelected[3]){
						pspDebugScreenSetTextColor(color01);
					}
					else{
						pspDebugScreenSetTextColor(color03);
					}
					pspDebugScreenPuts("^");
				}
				pspDebugScreenPuts("\n");

				//Draw the misc menus
				pspDebugScreenSetTextColor(extSelected[0] == 1? color01: color02); pspDebugScreenPuts("  Search\n");

				//Print out results
				convTotal=((searchResultCounter > searchResultMax)? searchResultMax:searchResultCounter);
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color01); sprintf(buffer,"  [Search Results %d: Only showing first searchResultMax]",convTotal);
				pspDebugScreenPuts(buffer);
				pspDebugScreenPuts("\n"); pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("  Address     Text\n");

				//Print out the results variables
				convTotal=((searchResultCounter > searchResultMax)? searchResultMax:searchResultCounter);
				counter=0;
				while(counter < convTotal){
					//Scroll feature right here, in two lines =3
					if((signed int)(counter) < (signed int)(((extSelected[0]-2)-10) - (( ((signed int)(extSelected[0]-2)+10) - ((signed int)convTotal))>0? abs(((signed int)(extSelected[0]-2)+10) - ((signed int)convTotal)): 0)   )) {counter++; continue;}
					if((signed int)(counter) > (signed int)(((extSelected[0]-2)+10) + (((signed int)(extSelected[0]-2)-10)<0? abs((signed int)((extSelected[0]-2)-10)): 0)   )) {counter++; continue;}

					//Apply the row color
					if(counter == (extSelected[0]-2)){
						pspDebugScreenSetTextColor(color01);
					}
					else{
						pspDebugScreenSetTextColor(color02);
					}

					//Print out the address
					sprintf(buffer, "  0x%08lX  '", (searchAddress[counter] - decodeFormat));
					pspDebugScreenPuts(buffer);

					//Print out the ASCII
					memset(buffer, 0, 17);
					scounter=0;
					while(scounter < 16){
						if((searchAddress[counter]+scounter) < 0x4A000000){
							buffer[scounter]=*((unsigned char*)(searchAddress[counter]+scounter)); if((buffer[scounter]<=0x20) || (buffer[scounter]==0xFF)) buffer[scounter]='.';
						}
						else{
							break;
						}
						scounter++;
					}
					pspDebugScreenPuts(buffer);

					//Goto the next cheat down under
					pspDebugScreenPuts("'\n");
					counter++;
				}

				//Helper
				pspDebugScreenSetTextColor(color02); 
				pspDebugScreenSetXY(0, 32);
				pspDebugScreenPuts(line); //draw spiffy line
				pspDebugScreenSetXY(0, 33);
				pspDebugScreenSetTextColor(color01);
				if(extSelected[0] == 0){
					pspDebugScreenPuts(">< = Edit On/Off; D-PAD = Cycle; [] = Trim; () = Cancel    ");
				}
				else if(extSelected[0] == 1){
					pspDebugScreenPuts(">< = Select; () = Cancel");  
				}
				else{
					pspDebugScreenPuts("<- -> = Scroll Address; () = Cancel");  
				}
			break;
			
			case 5: //Search range
				//Draw the tabs
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("  [Editing Search Range]\n");
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("\n  If you edit you're starting address and ending address here,\n  MKIJIRO will search inbetween the two. \n\n  (This increases speed of searches and reduces your chances of \n  crashing while searching.)\n\n");
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("  Start       Stop\n");

				//Apply the row color
				if(extSelected[0] == 0){
					pspDebugScreenSetTextColor(color01);
				}
				else{
					pspDebugScreenSetTextColor(color02);
				}
				sprintf(buffer, "  0x%08lX ", searchStart - 0x48800000);
				pspDebugScreenPuts(buffer);

				sprintf(buffer, " 0x%08lX", searchStop - 0x48800000);
				pspDebugScreenPuts(buffer);


				if(searchStart>searchStop){
				searchStop=searchStart;}

				//Skip a line, draw the pointer =)
				pspDebugScreenPuts("\n");
				if(extSelected[0] == 0){
					//Skip the desired amount?


					pspDebugScreenPuts("    ");
					if(extSelected[1] != 0){ //Skip Hex
						pspDebugScreenPuts("            "); 
					}

					//Skip the minimalist amount
					unsigned char tempCounter=extSelected[2];
					while(tempCounter){
						pspDebugScreenPuts(" "); 
						tempCounter--;
					}

					//Draw the symbol (Finally!!)
					if(extSelected[3]){
						pspDebugScreenSetTextColor(color01);
					}
					else{
						pspDebugScreenSetTextColor(color03);
					}
					pspDebugScreenPuts("^");
				}
				pspDebugScreenPuts("\n");

				//Helper
				pspDebugScreenSetTextColor(color02); 
				pspDebugScreenSetXY(0, 32);
				pspDebugScreenPuts(line); //draw spiffy line
				pspDebugScreenSetXY(0, 33);
				pspDebugScreenSetTextColor(color01);
				if(extSelected[0] == 0){
					if(searchNo == 0){
						pspDebugScreenPuts(">< = Edit On/Off; D-PAD = Cycle; () = Cancel               ");
					}
				}
				else if((extSelected[0] == 1) || (extSelected[0] == 2)){
					pspDebugScreenPuts(">< = Select; () = Cancel");  
				}
				else{
					pspDebugScreenPuts(">< = Add Selected Cheat; () = Cancel");  
				}
			break;
			
		}
	}
  	else{
		
		pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line); //draw spiffy line
		//Draw the tabs
		pspDebugScreenSetTextColor(tabSelected == 0? color01: color02); pspDebugScreenPuts("  [Cheater] ");
		pspDebugScreenSetTextColor(tabSelected == 1? color01: color02); pspDebugScreenPuts("[Searcher] ");
		pspDebugScreenSetTextColor(tabSelected == 2? color01: color02); pspDebugScreenPuts("[PRX] ");
		
		pspDebugScreenSetTextColor(tabSelected == 3? color01: color02); 
		if(flipme){ pspDebugScreenPuts("[Browser"); } else{ pspDebugScreenPuts("[Decoder"); } 
		pspDebugScreenSetTextColor(color01 - ((bdNo * 4) * color01_to)); sprintf(buffer, "%d", bdNo); pspDebugScreenPuts(buffer); 
		pspDebugScreenSetTextColor(tabSelected == 3? color01: color02); pspDebugScreenPuts("] ");
		
		pspDebugScreenSetTextColor(tabSelected == 4? color01: color02);
		pspDebugScreenPuts(gameId); pspDebugScreenPuts(" ");
		
		if(cheatStatus){
			pspDebugScreenSetTextColor(0xFF00FF00);
			pspDebugScreenPuts("[CHEATS ON]\n");
		}
		else{
			pspDebugScreenSetTextColor(color01);
			if(!cheatSaved){
				pspDebugScreenPuts("[CHEATS OFF]\n");
			}
			else{
				pspDebugScreenPuts("[CHEATS OFF]\n");
			}
		}
		pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line); //draw spiffy line
		//Draw the options for the respective tab
		switch(tabSelected){
			case 0: //DRAW CHEATER
				counter=0;
				while(counter < cheatTotal){
					//Scroll feature right here, in two lines =3
					if((signed int)counter < (signed int)((cheatSelected-12) - (( ((signed int)cheatSelected+12) - ((signed int)cheatTotal))>0? abs(((signed int)cheatSelected+12) - ((signed int)cheatTotal)): 0)   )) {counter++; continue;}
					if((signed int)counter > (signed int)((cheatSelected+12) + (((signed int)cheatSelected-12)<0? abs((signed int)(cheatSelected-12)): 0)   )) {counter++; continue;}

					//cheat status info 
					if(cheatSelected == counter){
						pspDebugScreenSetTextColor(color01);
						pspDebugScreenPuts(">");
						//Highlight the selection
						if(cheat[cheatSelected].flags & FLAG_SELECTED){
							pspDebugScreenSetTextColor(select_cheat);  pspDebugScreenPuts("    [!] ");
						}
						else if(cheat[cheatSelected].flags & FLAG_CONSTANT){
							pspDebugScreenSetTextColor(constant_cheat);  pspDebugScreenPuts("   [!!] ");
						}               
						else{
							pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("  [OFF] ");
						}
					}
					else{
						pspDebugScreenPuts(" ");
						//Don't highlight the selection
						if(cheat[counter].flags & FLAG_SELECTED){
							pspDebugScreenSetTextColor(select_cheat); pspDebugScreenPuts("    [!] "); pspDebugScreenSetTextColor(color02);
						}
						else if(cheat[counter].flags & FLAG_CONSTANT){
							pspDebugScreenSetTextColor(constant_cheat);  pspDebugScreenPuts("   [!!] "); pspDebugScreenSetTextColor(color02);
						}
						else{
							pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("  [OFF] "); pspDebugScreenSetTextColor(color02);
						}
					}

					pspDebugScreenPuts(cheat[counter].name);

					//cheat status info caption
					if(cheatSelected == counter){
						//Highlight the selection
						if(cheat[cheatSelected].flags & FLAG_SELECTED){
							pspDebugScreenSetTextColor(select_cheat); pspDebugScreenPuts(" (ACTIVE WITH NOTE) ");
						}
						else if(cheat[cheatSelected].flags & FLAG_CONSTANT){
							pspDebugScreenSetTextColor(constant_cheat);  pspDebugScreenPuts(" (ALWAYS ACTIVE) ");
						}
						else{
							pspDebugScreenSetTextColor(color01); pspDebugScreenPuts(" (NOT ENABLED) ");
						}
					}
					else{
						//Don't highlight the selection
						if(cheat[counter].flags & FLAG_SELECTED){
							pspDebugScreenSetTextColor(select_cheat); pspDebugScreenPuts(" (ACTIVE WITH NOTE) "); pspDebugScreenSetTextColor(color02);
						}
						else if(cheat[counter].flags & FLAG_CONSTANT){
							pspDebugScreenSetTextColor(constant_cheat);  pspDebugScreenPuts(" (ALWAYS ACTIVE) "); pspDebugScreenSetTextColor(color02);
						}
						else{
							pspDebugScreenSetTextColor(color01); pspDebugScreenSetTextColor(color02);
						}
					}

					pspDebugScreenPuts("\n");
					counter++;
				}

				pspDebugScreenSetXY(0, 28);
				pspDebugScreenSetTextColor(color02); 
				pspDebugScreenSetBackColor(bgcolor);
				pspDebugScreenPuts(line); //draw spiffy line

				pspDebugScreenSetXY(0, 29);

				pspDebugScreenSetTextColor(color01);
				pspDebugScreenPuts(MKVER);
				#ifdef _UMDMODE_
				pspDebugScreenPuts(" ");
				#elif _POPSMODE_
				pspDebugScreenPuts("POPS ");
				#elif _NOHB_
				pspDebugScreenPuts("NOHB ");
				#endif

				//battery info
				pspDebugScreenSetTextColor(color02);
				pspDebugScreenPuts("Bat:"); 

				//lol ugly but it works i dont care im a slob
				if((scePowerGetBatteryLifePercent() >= 1) && (scePowerGetBatteryLifePercent() < 10)){
					pspDebugScreenSetTextColor(0xFF0000CC);
					sprintf(buffer, "%d%% ", scePowerGetBatteryLifePercent()); 
				}
				else if((scePowerGetBatteryLifePercent() >= 10) && (scePowerGetBatteryLifePercent() < 20)){	
					pspDebugScreenSetTextColor(0xFF0000FF);
					sprintf(buffer, "%d%% ", scePowerGetBatteryLifePercent()); 
				}
				else if((scePowerGetBatteryLifePercent() >= 20) && (scePowerGetBatteryLifePercent() <= 30)){	
					pspDebugScreenSetTextColor(0xFF0044FF);
					sprintf(buffer, "%d%% ", scePowerGetBatteryLifePercent()); 
				}
				else if((scePowerGetBatteryLifePercent() >= 30) && (scePowerGetBatteryLifePercent() <= 40)){	
					pspDebugScreenSetTextColor(0xFF0088FF);
					sprintf(buffer, "%d%% ", scePowerGetBatteryLifePercent()); 
				}
				else if((scePowerGetBatteryLifePercent() >= 40) && (scePowerGetBatteryLifePercent() <= 50)){	
					pspDebugScreenSetTextColor(0xFF00CCFF);
					sprintf(buffer, "%d%% ", scePowerGetBatteryLifePercent()); 
				}
				else if((scePowerGetBatteryLifePercent() >= 50) && (scePowerGetBatteryLifePercent() <= 60)){	
					pspDebugScreenSetTextColor(0xFF00FFFF);
					sprintf(buffer, "%d%% ", scePowerGetBatteryLifePercent()); 
				}
				else if((scePowerGetBatteryLifePercent() >= 60) && (scePowerGetBatteryLifePercent() <= 70)){	
					pspDebugScreenSetTextColor(0xFF00FFCC);
					sprintf(buffer, "%d%% ", scePowerGetBatteryLifePercent()); 
				}
				else if((scePowerGetBatteryLifePercent() >= 70) && (scePowerGetBatteryLifePercent() <= 80)){	
					pspDebugScreenSetTextColor(0xFF00FF88);
					sprintf(buffer, "%d%% ", scePowerGetBatteryLifePercent()); 
				}
				else if((scePowerGetBatteryLifePercent() >= 80) && (scePowerGetBatteryLifePercent() <= 90)){	
					pspDebugScreenSetTextColor(0xFF00FF44);
					sprintf(buffer, "%d%% ", scePowerGetBatteryLifePercent()); 
				}
				else if((scePowerGetBatteryLifePercent() >= 90) && (scePowerGetBatteryLifePercent() <= 100)){	
					pspDebugScreenSetTextColor(0xFF00FF00);
					sprintf(buffer, "%d%% ", scePowerGetBatteryLifePercent()); 
				}			

				else{
					sprintf(buffer, "N/A ");
				}
				pspDebugScreenPuts(buffer);

				pspDebugScreenSetTextColor(color02);
				pspDebugScreenPuts("Temp:");
				pspDebugScreenSetTextColor(color01);
				//if(scePowerGetBatteryLifePercent() > 1){
				if(scePowerGetBatteryTemp() >= 0){
					sprintf(buffer, "%d C ", scePowerGetBatteryTemp());
				}
				else{
					sprintf(buffer, "N/A ");
				}
				pspDebugScreenPuts(buffer);

				pspDebugScreenSetTextColor(color02);
				pspDebugScreenPuts("Cheats:");
				pspDebugScreenSetTextColor(color01);
				sprintf(buffer, "%d ", cheatTotal); 
				pspDebugScreenPuts(buffer);

				//Helper
				pspDebugScreenSetTextColor(color02); 
				pspDebugScreenSetXY(0, 30);
				pspDebugScreenPuts(line); //draw spiffy line
				pspDebugScreenSetTextColor(color01);
				pspDebugScreenSetXY(0, 31); pspDebugScreenPuts(">< = Select Cheat for On/Off mode; START = Rename;");
				pspDebugScreenSetXY(0, 32); pspDebugScreenPuts("[] = Select Cheat for Always On mode; /\\ = Edit Cheat;");
				pspDebugScreenSetXY(0, 33); pspDebugScreenPuts("SELECT =Duplicate Cheat;  () = Cancel/Return to Game");
			break;
			
			case 1: //DRAW SEARCHER
				counter=0;
				while(counter < (3 + ((!cheatSearch)*4))){
					//Scroll feature right here, in two lines =3
					if((signed int)counter < (signed int)((cheatSelected-12) - (( ((signed int)cheatSelected+12) - ((signed int)cheatTotal))>0? abs(((signed int)cheatSelected+12) - ((signed int)cheatTotal)): 0)   )) {counter++; continue;}
					if((signed int)counter > (signed int)((cheatSelected+12) + (((signed int)cheatSelected-12)<0? abs((signed int)(cheatSelected-12)): 0)   )) {counter++; continue;}

					if(cheatSelected == counter){
						//Highlight the selection
						pspDebugScreenSetTextColor(color01);
					}
					else{
						//Don't highlight the selection
						pspDebugScreenSetTextColor(color02);
					}
					if(!cheatSearch){
						switch(counter){
							case 0: pspDebugScreenPuts("  Find Exact Value\n");  break;
							case 1: pspDebugScreenPuts("  Find Unknown Value - 8bit\n");break;
							case 2: pspDebugScreenPuts("  Find Unknown Value - 16bit\n");break;
							case 3: pspDebugScreenPuts("  Find Unknown Value - 32bit\n");break;
							case 4: pspDebugScreenPuts("  Find Text\n");break;
							case 5: pspDebugScreenPuts("  Search Range\n"); break;
							case 6: pspDebugScreenPuts("  Reset Search\n");break;
						}
					}
					else{
						switch(counter){
							case 0: pspDebugScreenPuts("  Continue to find Exact Value\n"); break;
							case 1: pspDebugScreenPuts("  Continue to find Unknown Value\n"); break;
							case 2: pspDebugScreenPuts("  Reset search\n"); break;
						}
					}
					counter++;
				}

				//Print out search history
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("  [Search History]");
				pspDebugScreenPuts("\n"); pspDebugScreenSetTextColor(color02); pspDebugScreenPuts(line);
				pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("  Mode          Value.Hex   Value.Dec   ASCII Value.Float\n");
				scounter=0;
				while(scounter < searchHistoryCounter){
					//Apply the row color
					pspDebugScreenSetTextColor(color01 - (scounter * 0x00000008));

					//Print out the mode
					pspDebugScreenPuts(searchModeName[searchHistory[scounter+1].stdVal]);
					if(searchHistory[scounter+1].stdVal > 3){
						
						//Print out the hex
						switch(searchHistory[scounter+1].flags & FLAG_DWORD){
							case FLAG_DWORD:
								sprintf(buffer, "  0x%08lX  ", searchHistory[scounter+1].hakVal);
							break;
							case FLAG_WORD:
								sprintf(buffer, "  0x____%04hX  ", (unsigned short)searchHistory[scounter+1].hakVal);
							break;
							case FLAG_BYTE:
								sprintf(buffer, "  0x______%02hX  ", (unsigned char)searchHistory[scounter+1].hakVal);
							break;
						}
						pspDebugScreenPuts(buffer);

						//Print out the decimal
						switch(searchHistory[scounter+1].flags & FLAG_DWORD){
							case FLAG_DWORD:
								sprintf(buffer, "%010lu  ", searchHistory[scounter+1].hakVal);
							break;
							case FLAG_WORD:
								sprintf(buffer, "%010lu  ", (unsigned short)searchHistory[scounter+1].hakVal);
							break;
							case FLAG_BYTE:
								sprintf(buffer, "%010lu  ", (unsigned char)searchHistory[scounter+1].hakVal);
							break;
						}
						pspDebugScreenPuts(buffer);

						//Print out the ASCII
						buffer[0]=*((unsigned char*)(((unsigned int)&searchHistory[scounter+1].hakVal)+0)); if((buffer[0]<=0x20) || (buffer[0]==0xFF)) buffer[0]='.';
						buffer[1]=*((unsigned char*)(((unsigned int)&searchHistory[scounter+1].hakVal)+1)); if((buffer[1]<=0x20) || (buffer[1]==0xFF)) buffer[1]='.';
						buffer[2]=*((unsigned char*)(((unsigned int)&searchHistory[scounter+1].hakVal)+2)); if((buffer[2]<=0x20) || (buffer[2]==0xFF)) buffer[2]='.';
						buffer[3]=*((unsigned char*)(((unsigned int)&searchHistory[scounter+1].hakVal)+3)); if((buffer[3]<=0x20) || (buffer[3]==0xFF)) buffer[3]='.';
						buffer[4]=0;
						pspDebugScreenPuts(buffer);

						//Print out the float
						pspDebugScreenPuts("  ");
						f_cvt(&searchHistory[scounter+1].hakVal, buffer, sizeof(buffer), 6, MODE_GENERIC);
						pspDebugScreenPuts(buffer);
						
					}

					//Goto the next line
					pspDebugScreenPuts("\n");

					//Increment scounter
					scounter++;
				}

				//Helper
				pspDebugScreenSetTextColor(color02); 
				pspDebugScreenSetXY(0, 32);
				pspDebugScreenPuts(line); //draw spiffy line
				pspDebugScreenSetTextColor(color01);
				pspDebugScreenSetXY(0, 33); pspDebugScreenPuts(">< = Select; () = Cancel/Return to Game");                                        
			break;
			
			case 2: //DRAW PRX
				counter=0;
				while(counter < 15){
					if(cheatSelected == counter){
						//Highlight the selection
						pspDebugScreenSetTextColor(color01);
					}
					else{
						//Don't highlight the selection
						pspDebugScreenSetTextColor(color02);
					}
					switch(counter){
						case 0: pspDebugScreenPuts("  Pause game? "); if(cheatPause) { pspDebugScreenPuts("True\n"); } else { pspDebugScreenPuts("False\n"); } break;
						case 1: sprintf(buffer, "  Add new cheat #%d line(s) long.\n", cheatLength); pspDebugScreenPuts(buffer); break;
						case 2: sprintf(buffer, "  Reset codes? Slot #%d\n", dumpNo); pspDebugScreenPuts(buffer); break;
						case 3: sprintf(buffer, "  Dump RAM? Slot #%d\n", dumpNo); pspDebugScreenPuts(buffer); break;
						case 4: pspDebugScreenPuts("  Dump Kmem\n"); break;
						case 5: sprintf(buffer, "  Bytes per line in browser? %d\n", browseLines); pspDebugScreenPuts(buffer); break;
						case 6: 
						#ifdef _SWAPBUTTON_
						pspDebugScreenPuts("  SWAP X/O? "); if(SWAPBUTTON==0x2000){pspDebugScreenPuts("True\n"); } else { pspDebugScreenPuts("False\n"); }
						#else
						pspDebugScreenPuts("  SWAP X/O(not compiled)\n");
						#endif
						break;
						case 7: pspDebugScreenPuts("  Use Real addressing in MKIJIRO menu? "); if(decodeFormat==0x40000000) { pspDebugScreenPuts("True\n"); } else { pspDebugScreenPuts("False\n"); } break;
						#ifdef _USB_
							case 8: pspDebugScreenPuts("  USB "); if(usbonbitch) { pspDebugScreenPuts("On\n"); } else { pspDebugScreenPuts("Off\n"); } break;
						#else
							case 8: 
							if(pad.Buttons & SWAPBUTTON3){
							pspDebugScreenPuts("  PSP Shutdown\n");}
							else{
							pspDebugScreenPuts("  PSP Sleep\n");}
							break;
						#endif
						case 9: sprintf(buffer, "  Cheat Hz? %d/1000 seconds\n", (cheatHz/1000)); pspDebugScreenPuts(buffer); break;
						case 10: pspDebugScreenPuts("  Save cheats\n"); break;
						case 11: pspDebugScreenPuts("  Reload cheats\n"); break;
						case 12: if(pad.Buttons & SWAPBUTTON3){
							pspDebugScreenPuts("  Save");}
						else{pspDebugScreenPuts("  Load");}
						sprintf(buffer, " settng files %d\n", colorFile);
						pspDebugScreenPuts(buffer); break;
						#ifdef _PSID_
						case 13: pspDebugScreenPuts("  Corrupt PSID\n"); break;
						#else
						case 13: pspDebugScreenPuts("  Corrupt PSID (not compiled)\n"); break;
						#endif
						#ifdef _SCREENSHOT_
						case 14:
						pspDebugScreenPuts("  SCREENSHOT_MODE:");	
						pspDebugScreenPuts(screenshotstring[screenshot_mode]); break;
						#endif

						
					}
					counter++;
				}

				//Helper
				pspDebugScreenSetTextColor(color02); 
				pspDebugScreenSetXY(0, 31);
				pspDebugScreenPuts(line); //draw spiffy line

				pspDebugScreenSetTextColor(color01);
				lineClear(32);
				switch(cheatSelected){
					case 0: pspDebugScreenPuts("Pauses the game while MKIJIRO's menu is showing"); break;
					case 1: pspDebugScreenPuts(">< To create new cheat;"); break;
					case 2: pspDebugScreenPuts("Uses the selected 'RAM dump' to regenerate OFF codes"); break;
					case 3: pspDebugScreenPuts("Saves the Game's RAM to MemoryStick"); break;
					case 4: pspDebugScreenPuts("Dump kernel memory and boot memory"); break;
					case 5: pspDebugScreenPuts("Alters the number of bytes displayed in the Browser"); break;
					case 6: pspDebugScreenPuts("button swapping X/O"); break;
					case 7: pspDebugScreenPuts("If enabled, REAL PSP hardware addresses will be used in MKIJIRO menu"); break;
					#ifdef _USB_
					case 8: pspDebugScreenPuts("Enable USB Mode (* Note * Remember to turn it off!!)"); break;
					#else
					case 8: pspDebugScreenPuts("Quick PSP Sleep,[] Shutdown");break;
					#endif
					case 9: pspDebugScreenPuts("Cheat refresh rate (MKIJIRO clock)"); break;
					case 10: pspDebugScreenPuts("Save your cheats"); break;
					case 11: pspDebugScreenPuts("Reload Cheats"); break;
					case 12: pspDebugScreenPuts("Load or reload setting file"); break;
					#ifdef _PSID_
					case 13: pspDebugScreenPuts("Corrupt PSID (thanks to nofx)"); break;
					#else
					case 13: pspDebugScreenPuts("Corrupt PSID is not compiled in this version"); break;
					#endif
						#ifdef _SCREENSHOT_
					case 14: pspDebugScreenPuts("Toggle Screenshot"); break;
						#endif

				}
				lineClear(33);
				if((cheatSelected != 9) && (cheatSelected != 12) && (cheatSelected != 3) && (cheatSelected != 2) && (cheatSelected != 1)){
					pspDebugScreenPuts(">< = Toggle; () = Cancel/Return to Game");
				}
				else{
					pspDebugScreenPuts("<- -> = Decrement/Increment; () = Cancel/Return to Game");
				}
			break;
			
			case 3: //DRAW DECODER & BROWSER
				pspDebugScreenSetTextColor(color02); 
				
				if(flipme){
					switch(browseLines){
						case 8: pspDebugScreenPuts("  Address     0001020304050607  ASCII\n"); break;
						case 16: pspDebugScreenPuts("  Address     000102030405060708090A0B0C0D0E0F  ASCII\n"); break;
					}
				}
				else{
					if(decodeOptions){
						pspDebugScreenPuts("  Address     Hex       ASCII Value.Dec   Value.Float\n");
					}
					else{
						pspDebugScreenPuts("  Address     Hex       Opcode   Args\n");
					}
				}

				//Write out the RAM
				counter=0;
				while(counter < 26){
				
					if(flipme){
					
					  if(counter == browseY[bdNo]){
						  pspDebugScreenSetTextColor(color01);
					  }
					  else{
						  pspDebugScreenSetTextColor(color02 - (counter * color02_to));
					  }
					
		if(browseAddress[bdNo]+(counter*browseLines)<0x49FFFFF9){
					  //Print out the address
						if(browseLines==8){
						addresscode=0xFFFFFFF8;}
						else{
						addresscode=0xFFFFFFF0;}
					  if(((browseAddress[bdNo]+(counter*browseLines))&addresscode)==(copyStartFlag&addresscode)){
						  pspDebugScreenSetTextColor(select_cheat);}
					else if(((browseAddress[bdNo]+(counter*browseLines))&addresscode)==(copyEndFlag&addresscode)){
						  pspDebugScreenSetTextColor(constant_cheat);}
					else if(((browseAddress[bdNo]+(counter*browseLines))&addresscode)==((copyData|0x40000000)&addresscode)){
						 pspDebugScreenSetTextColor(color04);}
					  sprintf(buffer, "  0x%08lX  ", (browseAddress[bdNo]+(counter*browseLines)) - decodeFormat);
					  pspDebugScreenPuts(buffer);
					  
					  //Print out the bytes per line
					  scounter=0;
					  while(scounter < browseLines){
						//Apply the row color
						if(browseY[bdNo] == counter){
							pspDebugScreenSetTextColor(color01);
						}
						else if(scounter & 1){
							pspDebugScreenSetTextColor(color01 - (counter * color01_to));
						}
						else{
							pspDebugScreenSetTextColor(color02 - (counter * color02_to));
						}
						
						sprintf(buffer, "%02hX", *((unsigned char*)((browseAddress[bdNo]+(counter*browseLines))+scounter)));
						pspDebugScreenPuts(buffer);
						
						buffer[3+scounter]=*((unsigned char*)((browseAddress[bdNo]+(counter*browseLines))+scounter)); if((buffer[3+scounter]<=0x20) || (buffer[3+scounter]==0xFF)) buffer[3+scounter]='.';
						
						scounter++;
					  }
			  
					  //Apply the row color
					  if(counter == browseY[bdNo]){
						  pspDebugScreenSetTextColor(color01);
					  }
					  else{
						  pspDebugScreenSetTextColor(color02 - (counter * color02_to));
					  }
					  
					  //Print out the ASCII
					  buffer[3+browseLines]=0;
					  pspDebugScreenPuts("  ");
					  pspDebugScreenPuts(&buffer[3]);
			}
					  
					  //Skip a line, draw the pointer =)
					  if(counter == browseY[bdNo]){
						//Skip the initial line
						pspDebugScreenPuts("\n");
						
						//Skip the desired amount?
						pspDebugScreenPuts("    ");
						if(browseC[bdNo] != 0){ //Skip Hex
							pspDebugScreenPuts("          "); 
							if(browseC[bdNo] != 1){ //Skip Bytes
								//Skip ASCII
								if(browseLines==8){
									pspDebugScreenPuts("                  "); 
								}
								else if(browseLines==16){
									pspDebugScreenPuts("                                  "); 
								}
							}
						}
						
						//Skip the minimalist amount
						unsigned char tempCounter=browseX[bdNo];
						while(tempCounter){
						  if((tempCounter!=0) && ((tempCounter%2) == 0) && (browseLines==8) && (browseC[bdNo] == 1)){
							  pspDebugScreenPuts(" "); 
						  }
						  else{
							  pspDebugScreenPuts(" ");
						  }
						  tempCounter--;
						}
						
						//Draw the symbol (Finally!!)
						if(extSelected[3]){
							pspDebugScreenSetTextColor(color01);
						}
						else{
							pspDebugScreenSetTextColor(color03);
						}
						pspDebugScreenPuts("^");
					  }
					}
					else{
						if(counter == decodeY[bdNo]){
							pspDebugScreenSetTextColor(color01);
						}
						else{
							if((decodeAddress[bdNo]+(counter*4)) == copyStartFlag){
								pspDebugScreenSetTextColor(select_cheat);
							}
							else if((decodeAddress[bdNo]+(counter*4)) == copyEndFlag){
								pspDebugScreenSetTextColor(constant_cheat);
							}
							else if((decodeAddress[bdNo]+(counter*4)) == (copyData|0x40000000)){
								pspDebugScreenSetTextColor(color04);
							}
							else{
								pspDebugScreenSetTextColor(color02 - (counter * color02_to));
							}
						}

		if(decodeAddress[bdNo]+(counter*4)<0x4A000000){
						//Print out the address
						sprintf(buffer, "  0x%08lX  ", (decodeAddress[bdNo]+(counter*4)) - decodeFormat);
						pspDebugScreenPuts(buffer);
						
						//Print out the dword of memory
						sprintf(buffer, "%08lX  ", *((unsigned int*)(decodeAddress[bdNo]+(counter*4))));
						pspDebugScreenPuts(buffer);

						if(decodeOptions){
							//Print out the ASCII
							buffer[0]=*((unsigned char*)(((unsigned int)decodeAddress[bdNo]+(counter*4))+0)); if((buffer[0]<=0x20) || (buffer[0]==0xFF)) buffer[0]='.';
							buffer[1]=*((unsigned char*)(((unsigned int)decodeAddress[bdNo]+(counter*4))+1)); if((buffer[1]<=0x20) || (buffer[1]==0xFF)) buffer[1]='.';
							buffer[2]=*((unsigned char*)(((unsigned int)decodeAddress[bdNo]+(counter*4))+2)); if((buffer[2]<=0x20) || (buffer[2]==0xFF)) buffer[2]='.';
							buffer[3]=*((unsigned char*)(((unsigned int)decodeAddress[bdNo]+(counter*4))+3)); if((buffer[3]<=0x20) || (buffer[3]==0xFF)) buffer[3]='.';
							buffer[4]=0;
							pspDebugScreenPuts(buffer);							
							
							//Print out the decimal
							sprintf(buffer, "  %010lu  ", *((unsigned int*)(decodeAddress[bdNo]+(counter*4))));
							pspDebugScreenPuts(buffer);	
						  
							//Print out the float
							f_cvt(decodeAddress[bdNo]+(counter*4), buffer, sizeof(buffer), 6, MODE_GENERIC);
							pspDebugScreenPuts(buffer);
				
						}
						else{
							mipsDecode(*((unsigned int*)(decodeAddress[bdNo]+(counter*4))));
							addresscode=*((unsigned int*)(decodeAddress[bdNo]+(counter*4)));
							counteraddress=decodeAddress[bdNo]+(counter*4)-0x40000000;
							if(decodeAddress[bdNo]-(4*backline) > 0x48800000){
							if( (((addresscode>>24) & 0xFC) == 0x34) || (((addresscode>>24) & 0xFC) == 0x20) || (((addresscode>>24) & 0xFC) == 0x24)){
							unsigned int backcode_lui=(*((unsigned int*)(decodeAddress[bdNo]+((counter-backline)*4))))>>24;
							unsigned int REG1=(*((unsigned int*)(decodeAddress[bdNo]+((counter-backline)*4)))>>16) & 0x1F;
							unsigned int REG2=(*((unsigned int*)(decodeAddress[bdNo]+((counter)*4)))>>21) & 0x1F;
							//unsigned int REG2=(*((unsigned int*)(decodeAddress[bdNo]+((counter)*4)))>>16) & 0x1F;
							if( (REG1 ==REG2) && (backcode_lui == 0x3C) ){
								switch( (addresscode>>24) & 0xFC){
								case 0x20:
								case 0x24:
								if( (addresscode & 0xFFFF) < 0x8000){
								addresscode=(*((unsigned int*)(decodeAddress[bdNo]+((counter-backline)*4)))<<16) + (addresscode & 0xFFFF);}
								else{
								addresscode=(*((unsigned int*)(decodeAddress[bdNo]+((counter-backline)*4)))<<16) - (addresscode & 0xFFFF);}
								break;
								case 0x34:
								addresscode=(*((unsigned int*)(decodeAddress[bdNo]+((counter-backline)*4)))<<16) | (addresscode & 0xFFFF);
								break;
								}
								if( (((addresscode>>16)& 0x7FFF) > 0x38D1) && (((addresscode>>16)& 0x7FFF) < 0x4B19) || (((addresscode>>16)& 0x7FFF) > 0x7F7F)){
								pspDebugScreenPuts(" MFLOAT:");//MERGED IEEE754 FLOAT
								unsigned char overlimit=0;
								if(((addresscode>>16)& 0x7FFF)>0x47C2){
								overlimit=1;}
								f_cvt(&addresscode, buffer, sizeof(buffer), 6-overlimit, MODE_GENERIC);
								pspDebugScreenPuts(buffer);}}
								addresscode=0;
							}
							}
							mipsSpecial(addresscode,addresstmp,counteraddress);
						}
		}
						//Skip a line, draw the pointer =)
						if(counter == decodeY[bdNo]){

							//Skip the initial line
							pspDebugScreenPuts("\n");

							//Skip the desired amount?
							pspDebugScreenPuts("    ");
							if(decodeC[bdNo] != 0){
								//Skip Address
								pspDebugScreenPuts("          ");
								//Skip Hex
							}


							//Skip the minimalist amount
							unsigned char tempCounter=decodeX[bdNo];
							while(tempCounter){
								pspDebugScreenPuts(" "); 
								tempCounter--;
							}

							//Draw the symbol (Finally!!)
							if(extSelected[3]){
								pspDebugScreenSetTextColor(color01);
							}
							else{
								pspDebugScreenSetTextColor(color03);
							}
							pspDebugScreenPuts("^");
						}
						
					}

					//Goto the next cheat down under
					pspDebugScreenPuts("\n");
					counter++;
				}

				//Helper
				pspDebugScreenSetTextColor(color02); 
				pspDebugScreenSetXY(0, 31);
				pspDebugScreenPuts(line); //draw spiffy line
				pspDebugScreenSetTextColor(color01);
				pspDebugScreenSetXY(0, 32); pspDebugScreenPuts(">< = Edit On/Off; D-PAD = Cycle/Scroll; [] + <-> = Back/Jump;");
				pspDebugScreenSetXY(0, 33); pspDebugScreenPuts("[] + Analog/Digital = Scroll; () = Cancel/Return to Game");
				sprintf(buffer," JumpNo:%3d",searchjumpNo);pspDebugScreenPuts(buffer);
			break;
			
			case 4:
				//int fmem=sceKernelTotalFreeMemSize();
				//int maxmem=sceKernelMaxFreeMemSize();
				/*pspDebugScreenSetXY(10, 16);pspDebugScreenPuts("MKIJRO is opensource MKULTRA personal modifycation\n");
				pspDebugScreenSetXY(10, 17);pspDebugScreenPuts("visit http://code.google.com/p/mkijiro/ \n");
				pspDebugScreenSetXY(10, 19);sprintf(buffer,"VRAM 0x%08X,SAVERAM 0x%08X,SEARCHRAM 0x%08X\n",vram,SAVETEMP,RAMTEMP);
				pspDebugScreenPuts(buffer);
				pspDebugScreenSetXY(10, 20);sprintf(buffer,"SearchMAX %D,FW 0x%08X\n",searchResultCounterMax,sceKernelDevkitVersion());
				pspDebugScreenPuts(buffer);
				pspDebugScreenSetXY(10, 21);sprintf(buffer,"SET 0x%08X,PAUSE %02X,BLINE %D,DFOMAT 0x%08X\n",setting,cheatPause,browseLines,decodeFormat);
				pspDebugScreenPuts(buffer);*/
				//telazornDraw(logo, 40, 100);
			break;
		}
	
	}
  
}

void menuInput(){
  
	int fd;
	int fd2;
	unsigned int counter=0;
	unsigned int scounter=0;
	unsigned int dcounter=0;
	unsigned int padButtons;
	unsigned char miscType=0;
	unsigned char DumpByte=0;
	//unsigned short DumpInterval=0;
	pad.Buttons=0;
	menuDraw();
	
  	//Loop for input
	while(1){
  	
		padButtons=pad.Buttons;
		sceCtrlPeekBufferPositive(&pad, 1);

		//Has the HOME button screwed up the VRAM blocks?
		unsigned int a_address=0;
		unsigned int a_bufferWidth=0;
		unsigned int a_pixelFormat=0;
		unsigned int a_sync;
		
		sceDisplayGetFrameBufferInternal(0, &a_address, &a_bufferWidth, &a_pixelFormat, &a_sync);
		
		if(a_address == 0){
		  //Stop nitePR
		  menuDrawn=0;
		  return;
		}

		if(copyMenu){ //copy menu
		   remenu();
		   if(pad.Buttons & PSP_CTRL_UP){
				if(copyMenu > 1){
					copyMenu-=1;}
				else{
					copyMenu=countermax;}
				menuDraw();
				sceKernelDelayThread(150000);
		   }
		   else if(pad.Buttons & PSP_CTRL_DOWN){
				
				if(copyMenu < countermax){
				copyMenu+=1;}
				else{
				copyMenu=1;}
				menuDraw();
				sceKernelDelayThread(150000);
		  }
		  if(flipme==0 && tabSelected==3){
			if(pad.Buttons & PSP_CTRL_RIGHT){
			  backline++;
			  menuDraw();
		  	  sceKernelDelayThread(150000);}
			else if((pad.Buttons & PSP_CTRL_LEFT) && backline>1){
			  backline--;
			  menuDraw();
		  	  sceKernelDelayThread(150000);}
		  }
		   if(pad.Buttons & SWAPBUTTON){
			if(copyMenu == 1){//Copy addy
			  if(extMenu)
			  {
				if(extMenu == 1)
				{
				  if((block[extSelected[0]].flags & FLAG_DMA) || (block[extSelected[0]].flags & FLAG_JOKER)){
				      if( (block[extSelected[0]].flags & FLAG_DMA) && (pad.Buttons & SWAPBUTTON3)){
				  	unsigned int cptemp=*(unsigned int*)((block[extSelected[0]].hakVal + 0x8800000) & 0xFFFFFFFC);
					lineClear(33);
					if((cptemp >= 0x8800000) && (cptemp < 0xA000000)){
					copyData=cptemp;
					sprintf(buffer,"COPIED BASEADDRESS 0x%X",cptemp);
					pspDebugScreenPuts(buffer);}
					else{
					pspDebugScreenPuts("FAILED NULL POINTER");}
					sceKernelDelayThread(1500000);
				     }
                                    else{
	 			    copyData=(block[extSelected[0]].hakVal & 0x1FFFFFE)+0x8800000;}
				  }
				  else{
				  copyData=block[extSelected[0]].address;}
				}
				else if(extMenu == 2)
				{
					if(extSelected[0] > 2)
					{
					copyData=searchAddress[extSelected[0]-3]-0x40000000;
					searchjumpNo=extSelected[0]-3;
					}
				}
				else if(extMenu == 3)
				{
				  if(extSelected[0] > 2)
				  {
					copyData=searchAddress[extSelected[0]-3]-0x40000000;
					searchjumpNo=extSelected[0]-3;
					}
				}
				else if(extMenu == 4)
				{
				  if(extSelected[0] > 1)
				  {
					copyData=searchAddress[extSelected[0]-2]-0x40000000;
					searchjumpNo=extSelected[0]-2;
					}
				}
				else if(extMenu == 5)
				{
					if(extSelected[1]==0){
					copyData=searchStart-0x40000000;}
					else{
					copyData=searchStop -0x40000000;}
				}
			  }
			  else
			  {
				if(tabSelected == 3)
				{
					if(flipme){
					  if(pad.Buttons & SWAPBUTTON3){//jump copy start
						browseAddress[bdNo]=copyStartFlag;browseMAX();
					  }
					  else{
					  copyData=browseAddress[bdNo]+(browseY[bdNo] * browseLines);
					  copyData-=0x40000000;}
					  }
					else{
				       		if(pad.Buttons & SWAPBUTTON3){//jump copy start
						decodeAddress[bdNo]=copyStartFlag;decodeMAX();}
						else{
					  copyData=decodeAddress[bdNo]+(decodeY[bdNo]*4);
					  copyData-=0x40000000;}
					}
				}
			  }
			  copyData&=0xFFFFFFFC;

			  if(copyData < 0x08800000)
			  {
				copyData=0x08800000;
			  }
			}
			else if(copyMenu ==2){//Paste addy
			  if(extMenu)
			  {
				if(extMenu == 1)
				{
				  if((block[extSelected[0]].flags & FLAG_DMA) || (block[extSelected[0]].flags & FLAG_JOKER)){
				  block[extSelected[0]].hakVal=copyData-0x8800000;}
				  else{
				  block[extSelected[0]].address=copyData;}
				}
				else if(extMenu == 2)
				{
					if(extSelected[0] < 1)
					{
						searchHistory[0].hakVal=copyData;
					}
				}
				else if(extMenu == 5)
				{
					if(extSelected[1]==0){
					searchStart=copyData + 0x40000000;}
					else{
					searchStop =copyData + 0x40000000;}
				}
			  }
			  else
			  {
				if(tabSelected == 3)
				{
					if(flipme){
				        	if(pad.Buttons & SWAPBUTTON3){//jump copy end
							browseAddress[bdNo]=copyEndFlag;
						}
						else if(copyData < 0x49FFFFA8){
							browseAddress[bdNo]=copyData|0x40000000; //(browseY[bdNo] * browseLines)+
						}browseMAX();
					}
					else{					
				        	if(pad.Buttons & SWAPBUTTON3){//jump copy end
						decodeAddress[bdNo]=copyEndFlag;
						}
						else if(copyData < 0x4A000000){
							decodeAddress[bdNo]=copyData|0x40000000; //+(decodeY[bdNo]*4);
						}decodeMAX();
					}
				}
			  }
			}
			
			if(copyMenu == 3){//Copy val
			  if(extMenu)
			  {
				if(extMenu == 1)
				{
				  copyData2=block[extSelected[0]].hakVal;
				}
				else if(extMenu == 2)
				{
					if(extSelected[0] < 1)
					{
						copyData2=searchHistory[0].hakVal;
					}
					//else if(extSelected[0] > 2){
					//	copyData2=searchAddress[extSelected[0]-3];}
				}
				/*else if(extMenu == 3)
				{
				  if(extSelected[0] > 2)
				  {
					copyData2=searchAddress[extSelected[0]-3];
					}
				}
				else if(extMenu == 4)
				{
				  if(extSelected[0] > 1)
				  {
					copyDat2a=searchAddress[extSelected[0]-2]-0x40000000;
					}
				}*/
			  }
			  else
			  {
				if(tabSelected == 3)
				{
					if(flipme){
						if(browseC[bdNo]==0){
						copyData2=*(unsigned char*)(browseAddress[bdNo]+(browseY[bdNo]*0x10));}
						else if(browseC[bdNo]==1){
						copyData2=*(unsigned char*)(browseAddress[bdNo]+(browseX[bdNo]/2)+(browseY[bdNo]*0x10));}
						else{
						copyData2=*(unsigned char*)(browseAddress[bdNo]+(browseX[bdNo])+(browseY[bdNo]*0x10));}
					}
					else{
						copyData2=*((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4)));
					}
				}
			  }
			}
			else if(copyMenu == 4){//Paste val
			  if(extMenu)
			  {
				if(extMenu == 1)
				{
				  if(!(block[extSelected[0]].flags & FLAG_DMA)){
				  	  if(pad.Buttons & SWAPBUTTON3){
				  	  	  block[extSelected[0]].hakVal=0x0A200002 + ((copyData-0x8800000)>>2);
				  	  	  sceKernelDelayThread(100000);
				  	  }
				  	  else{
				  block[extSelected[0]].hakVal=copyData2;
				  	  }
				  
				  }
				}
				else if(extMenu == 2)
				{
					if(extSelected[0] < 1)
					{
						searchHistory[0].hakVal=copyData2;
					}
				}
			  }
			  else
			  {
				if(tabSelected == 3)
				{
					if(flipme){
						if(browseC[bdNo]==0){
						*(unsigned char*)(browseAddress[bdNo]+(browseY[bdNo]*0x10))=copyData2;}
						else if(browseC[bdNo]==1){
						*(unsigned char*)(browseAddress[bdNo]+(browseX[bdNo]/2)+(browseY[bdNo]*0x10))=copyData2;}
						else{
						*(unsigned char*)(browseAddress[bdNo]+(browseX[bdNo])+(browseY[bdNo]*0x10))=copyData2;}
					}
					else{
				  	  if(pad.Buttons & SWAPBUTTON3){
						if(searchHistory[0].hakVal==0x03E00008 && searchResultCounter!=0){
							int i=0;
							int searchHookMax=0;
							if(searchResultCounter>searchResultMax){
							searchHookMax=searchResultMax;
							}
							else{
							searchHookMax=searchResultCounter;
							}
							while(i<searchHookMax){
							*(unsigned int*)(searchAddress[i]&0x0FFFFFFC)=0x000001CD;
							i++;
							}
						}
						else{
				  	  	 *((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4)))=0x0A200002 + ((copyData-0x8800000)>>2);
				  	  	 	sceKernelDelayThread(100000);
				  	  	}
				  	  }
				  	  else{
						*((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4)))=copyData2;
						}
					}
				}
			  }
			}
			else if(copyMenu ==5){//normal, viewlog,clear log
			  if(extMenu == 1){
			  	if(block[extSelected[0]].address >= 0xFF000000){
				  block[extSelected[0]].address=0x08800000;
				}
				if(pad.Buttons & SWAPBUTTON3){
				switch(block[extSelected[0]].flags & FLAG_DWORD){
				case FLAG_BYTE:block[extSelected[0]].flags=FLAG_CWC|FLAG_BYTE;break;
				case FLAG_WORD:block[extSelected[0]].flags=FLAG_CWC|FLAG_WORD;break;
				case FLAG_DWORD:block[extSelected[0]].flags=FLAG_CWC|FLAG_DWORD;break;
				}
				sceKernelDelayThread(100000);
				}
				else{
				switch(block[extSelected[0]].flags & FLAG_DWORD){
				case FLAG_BYTE:block[extSelected[0]].flags=FLAG_BYTE;break;
				case FLAG_WORD:block[extSelected[0]].flags=FLAG_WORD;break;
				case FLAG_DWORD:block[extSelected[0]].flags=FLAG_DWORD;break;
				}
			}
				}
				else if(tabSelected == 3)
				{
				   if(pad.Buttons & SWAPBUTTON3){ //clear
					unsigned char i=0;
				        for(i=0;  i < jumplog ; i++){
					*(unsigned int *)(logstart + 4*i)=0;
					storedAddress[i]=0;
								}
					logcounter=0;backaddress[i]=0x48800000;
					}
				   else{
					if(flipme){ //memoryeditor
						if(copyData < 0x4A000000){
							browseAddress[bdNo]=logstart+4*(logcounter-1); //(browseY[bdNo] * browseLines)+
							browseMAX();
						}
					}
					else{ //decoder //bakck to decoder
						if(((decodeAddress[bdNo]+(decodeY[bdNo]*4)) >= (logstart-4)) && ((decodeAddress[bdNo]+(decodeY[bdNo]*4)) <= (logstart + 4*jumplog))){ //back address decoder
						decodeAddress[bdNo]=backaddress[bdNo];decodeY[bdNo]=backaddressY[bdNo];
						}
						else{//view log
						  backaddress[bdNo]=decodeAddress[bdNo];backaddressY[bdNo]=decodeY[bdNo];decodeY[bdNo]=0;
						 if(copyData < 0x4A000000){
							decodeAddress[bdNo]=logstart+4*(logcounter-1); //+(decodeY[bdNo]*4);
						 }
						 }decodeMAX();
					   }
					}
				}
			}
			
			else if(copyMenu ==6){//DMA

			  if(extMenu == 1){
				   block[extSelected[0]].address=0xFFFFFFFF;
				   block[extSelected[0]].stdVal=0xFFFFFFFF;
				   block[extSelected[0]].flags=FLAG_DMA | FLAG_DWORD;
				}
			 else if(tabSelected==3){ //copy range to new cheat
				   if(pad.Buttons & SWAPBUTTON3){
				  extMenu=1;
				  tabSelected=0;
				  decToCheat();
				  pspDebugScreenInitEx(vram, 0, 0);
				  extSelected[1]=extSelected[2]=extSelected[3]=0;
				  extSelected[0]=cheat[cheatTotal - 1].block;
				  cheatSelected=cheatTotal - 1;
				  extOpt=-2;
				  menuDraw();
				  sceKernelDelayThread(150000);
					}
				}
			}
			
			else if(copyMenu ==7){//JOKER
			  if(extMenu == 1){
				   //block[extSelected[0]].address=0x08800000;
				   block[extSelected[0]].address=0xFF000000;
				   if(pad.Buttons & SWAPBUTTON3){
				   block[extSelected[0]].hakVal=0x0;
				   }
				   else{
				   block[extSelected[0]].hakVal=JOKERADDRESS;
				   }
				   block[extSelected[0]].flags=FLAG_JOKER | FLAG_DWORD;
					sceKernelDelayThread(100000);
					}
				else if(tabSelected==3){ //copy range to new cheat
				   if(pad.Buttons & SWAPBUTTON3){
					decToText();
					menuDraw();
					sceKernelDelayThread(150000);}
				}
			}
			goto hideCopyMenu;
		  }
		  if(((padButtons & SWAPBUTTON2) && !(pad.Buttons & SWAPBUTTON2)) || (pad.Buttons & PSP_CTRL_HOME)){
		  	hideCopyMenu:
			pspDebugScreenInitEx(vram, 0, 0);
			if(cheatPause && (pad.Buttons & PSP_CTRL_HOME))  cheatPause=!cheatPause;gameResume(thid);
			copyMenu=0;
			menuDraw();
			sceKernelDelayThread(150000);
		  }
		}
		else if(extMenu){ //Do we use extended menus?
		  switch(extMenu)
		  {
			case 1: //INPUT EXT CHEAT
			  if(pad.Buttons & PSP_CTRL_START){
				//change format
				if(editFormat==0){
				  editFormat=1;
				  if(extSelected[1] >1){
				  extSelected[2]=7;
				  extSelected[1]=1;}
				}
				else{
					editFormat=0;
				}
				menuDraw();
				sceKernelDelayThread(150000);
			  }
			  
			  if(pad.Buttons & PSP_CTRL_TRIANGLE) { copyMenu=1; menuDraw(); sceKernelDelayThread(150000);}
			  if(pad.Buttons & SWAPBUTTON){
				extSelected[3]=!extSelected[3];
				menuDraw();
				sceKernelDelayThread(150000);
			  }
			  if(pad.Buttons & PSP_CTRL_SQUARE){
				//Don't allow the user to cause a dword/byte misalignment!!!
					if(block[extSelected[0]].flags & FLAG_DMA){
					}
					else if(block[extSelected[0]].flags & FLAG_JOKER){
					}
					else{
					switch(block[extSelected[0]].flags & FLAG_DWORD)
					{
					case FLAG_BYTE:
					if(block[extSelected[0]].address % 2==0){
					block[extSelected[0]].flags=(block[extSelected[0]].flags & ~FLAG_DWORD) | FLAG_WORD;
					}
					break;
					case FLAG_WORD:
					if(block[extSelected[0]].address % 4==0){
					block[extSelected[0]].flags=(block[extSelected[0]].flags & ~FLAG_DWORD) | FLAG_DWORD; 
					}
					break;
					case FLAG_DWORD:
					block[extSelected[0]].flags=(block[extSelected[0]].flags & ~FLAG_DWORD) | FLAG_BYTE;
					break;
					}
					}
				//}
				menuDraw();
				sceKernelDelayThread(150000);
			  }
				
			  if(extSelected[3]){
				if(pad.Buttons & PSP_CTRL_UP)
				{
					restore(0);
				  switch(extSelected[1])
				  {
					case 0: 
						if(block[extSelected[0]].flags & FLAG_DMA)
					  {
						//Do nothing =o
						break;
					  } 
						else if(((block[extSelected[0]].flags & FLAG_JOKER) == FLAG_JOKER) && (extSelected[2]==7))
					  {
						block[extSelected[0]].address+=1;
						
					  }
						else if(((block[extSelected[0]].flags & FLAG_DWORD) == FLAG_DWORD) && (extSelected[2]==7)) //Prevent user from misaligned dwords/bytes
						{
						block[extSelected[0]].address+=4;
					  }
					  else if(((block[extSelected[0]].flags & FLAG_DWORD) == FLAG_WORD) && (extSelected[2]==7)) //Prevent user from misaligned dwords/bytes
						{
						block[extSelected[0]].address+=2;
					  }
					  else
					  {
						block[extSelected[0]].address+=(1 << (4*(7-extSelected[2])));
						}
						
						if(block[extSelected[0]].flags & FLAG_JOKER){
									if(block[extSelected[0]].hakVal == 0){
									if(block[extSelected[0]].address > 0xFFFFF3F9 || block[extSelected[0]].address < 0x10000000){
								block[extSelected[0]].address=0xFFFFF3F9;
									}}
									else{
									if(block[extSelected[0]].address > 0xFF00FFFF || block[extSelected[0]].address < 0x10000000){
								block[extSelected[0]].address=0xFF00FFFF;
									}}
						}
					    else if(block[extSelected[0]].address < 0x08800000)
							{
								block[extSelected[0]].address=0x08800000;
							}
						else if(block[extSelected[0]].address > 0x09FFFFFF)
							{
								block[extSelected[0]].address=0x09FFFFFC;
							
							}
					restore(1);
					  break;
					case 1:
						if(block[extSelected[0]].flags & FLAG_FREEZE)
					  {
						//Do nothing =o
						break;
					  }
					  block[extSelected[0]].hakVal+=(1 << (4*(7-extSelected[2]))); break;
					case 2: block[extSelected[0]].hakVal+=decDelta[extSelected[2]]; break;
					case 3: block[extSelected[0]].hakVal+=(1 << (8*(extSelected[2]))); break;
					}
					if((block[extSelected[0]].flags & FLAG_DMA) || (block[extSelected[0]].flags & FLAG_JOKER)){
						if(block[extSelected[0]].hakVal> 0x017FFFFF){
							block[extSelected[0]].hakVal=0x017FFFFE;
						}
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
					restore(0);
				  switch(extSelected[1])
				  {
					case 0:
						if(block[extSelected[0]].flags & FLAG_DMA)
					  {
						//Do nothing =o
						break;
					  }
					else if(((block[extSelected[0]].flags & FLAG_JOKER) == FLAG_JOKER) && (extSelected[2]==7))
					  {
						block[extSelected[0]].address-=1;
					  }
						else if(((block[extSelected[0]].flags & FLAG_DWORD) == FLAG_DWORD) && (extSelected[2]==7)) //Prevent user from misaligned dwords/bytes
						{
						block[extSelected[0]].address-=4;
					  }
					  else if(((block[extSelected[0]].flags & FLAG_DWORD) == FLAG_WORD) && (extSelected[2]==7)) //Prevent user from misaligned dwords/bytes
						{
						block[extSelected[0]].address-=2;
					  }
					  else
					  {
						block[extSelected[0]].address-=(1 << (4*(7-extSelected[2])));
					  }
					if(block[extSelected[0]].flags & FLAG_JOKER){
									if(block[extSelected[0]].address < 0xFF000000){
								block[extSelected[0]].address=0xFF000000;
									}
						}
					else if(block[extSelected[0]].address < 0x08800000)
							{
								block[extSelected[0]].address=0x08800000;
							}
					else if(block[extSelected[0]].address > 0x09FFFFFF)
							{
								block[extSelected[0]].address=0x09FFFFFC;
							}
					restore(1);
					  break;
					case 1: 
						if(block[extSelected[0]].flags & FLAG_FREEZE)
					  {
						//Do nothing =o
						break;
					  }
						block[extSelected[0]].hakVal-=(1 << (4*(7-extSelected[2]))); break;
					case 2: block[extSelected[0]].hakVal-=decDelta[extSelected[2]]; break;
					case 3: block[extSelected[0]].hakVal-=(1 << (8*(extSelected[2]))); break;
				  }
				  if((block[extSelected[0]].flags & FLAG_DMA) || (block[extSelected[0]].flags & FLAG_JOKER)){
						if(block[extSelected[0]].hakVal> 0x017FFFFF){
							block[extSelected[0]].hakVal=0;
						}
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else
				{
				  cheatButtonAgeY=0;
				}
			  }
			  else{
				if(pad.Buttons & PSP_CTRL_UP)
				{
				  if(extSelected[0] > cheat[cheatSelected].block)
				  {
					extSelected[0]--;
				  }
				  else if(extSelected[0] == cheat[cheatSelected].block)
				  {
					extSelected[0]=(cheat[cheatSelected].block+cheat[cheatSelected].len)-1;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_LTRIGGER)
				{
				  if(extSelected[0] > cheat[cheatSelected].block+26)
				  {
					extSelected[0]-=27;
				  }
				  else{extSelected[0]=cheat[cheatSelected].block;}
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
				  if(extSelected[0] < ((cheat[cheatSelected].block+cheat[cheatSelected].len)-1))
				  {
					extSelected[0]++;
				  }
				  else if(extSelected[0] == ((cheat[cheatSelected].block+cheat[cheatSelected].len)-1))
				  {
					extSelected[0]=cheat[cheatSelected].block;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}	
				else if(pad.Buttons & PSP_CTRL_RTRIGGER)
				{
				  if(extSelected[0]+26 < ((cheat[cheatSelected].block+cheat[cheatSelected].len)-1))
				  {
					extSelected[0]+=27;
				  }
				  else{extSelected[0]=((cheat[cheatSelected].block+cheat[cheatSelected].len)-1);}
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else

				{
					cheatButtonAgeY=0;
				}
			  }
			
			  if(pad.Buttons & PSP_CTRL_LEFT){
				extSelected[2]--;
				switch(extSelected[1])
				{
				  case 0: if((signed)extSelected[2] == -1) { extSelected[2]=0; } break;
				  case 1: if((signed)extSelected[2] == -1) { extSelected[2]=7; extSelected[1]--; } break;
				  case 2: if((signed)extSelected[2] == -1) { extSelected[2]=7; extSelected[1]--; } break;
				  case 3: if((signed)extSelected[2] == -1) { extSelected[2]=9; extSelected[1]--; } break;
				}
				menuDraw();
				if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
			  }
			  else if(pad.Buttons & PSP_CTRL_RIGHT){
				extSelected[2]++;
				switch(extSelected[1])
				{
				  case 0: if(extSelected[2] > 7) { extSelected[2]=0; extSelected[1]++; } break;
					
				  case 1: if(editFormat==1){
					if(extSelected[2] > 7) { extSelected[2]=7;}}
					else{
					  if(extSelected[2] > 7) { extSelected[2]=0; extSelected[1]++; }}
					break;
				  case 2: if(extSelected[2] > 9) { extSelected[2]=0; extSelected[1]++; } break;
				  case 3: if(extSelected[2] > 3) { extSelected[2]=3; } break;
				}
				menuDraw();
				if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
			  }
			  else{
				cheatButtonAgeX=0;
			  }
			  
				if((padButtons & SWAPBUTTON2) && !(pad.Buttons & SWAPBUTTON2)){
						if(extSelected[3]){
							extSelected[3]=0;
							menuDraw();
							sceKernelDelayThread(150000);
						}
						else{
						pspDebugScreenInitEx(vram, 0, 0);
						extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=0;
						extMenu=0;
						if(extOpt>0){
							extSelected[0]=extOptArg;
							pspDebugScreenInitEx(vram, 0, 0);
							extSelected[1]=extSelected[2]=extSelected[3]=0;
							extMenu=1+extOpt;
							cheatSearch=1;
						}
						else if(extOpt==-1){
						tabSelected=2;cheatSelected=1;}
						else if(extOpt==-2){
						tabSelected=3;}
						extOpt=0;
						menuDraw();
						sceKernelDelayThread(150000);
						break;
					}
				}
				break;
			  
			case 2: //INPUT EXT SEARCH
				if(pad.Buttons & PSP_CTRL_START){
					//change format
					if(editFormat==0){
						editFormat=1;
					if(extSelected[1] >0) {
					extSelected[2]=7; extSelected[1]=0;}
					}
					else{
						editFormat=0;
					}
					menuDraw();
					sceKernelDelayThread(150000);
				}
				if(pad.Buttons & PSP_CTRL_TRIANGLE) { copyMenu=1; menuDraw(); sceKernelDelayThread(150000);}
				if(pad.Buttons & PSP_CTRL_SQUARE)
			  {
				if(searchNo == 0) //Don't allow the user to change type later on!!!
				{
					switch(searchHistory[0].flags & FLAG_DWORD)
					{
					case FLAG_BYTE: searchHistory[0].flags=(searchHistory[0].flags & ~FLAG_DWORD) | FLAG_WORD; break;
					case FLAG_WORD: searchHistory[0].flags=(searchHistory[0].flags & ~FLAG_DWORD) | FLAG_DWORD; break;
					case FLAG_DWORD: searchHistory[0].flags=(searchHistory[0].flags & ~FLAG_DWORD) | FLAG_BYTE; break;
					}
				}
				menuDraw();
				sceKernelDelayThread(150000);
				}
			  if(pad.Buttons & SWAPBUTTON)
			  {
				if(extSelected[0] == 0)
				{
					extSelected[3]=!extSelected[3];
				}
				else if(extSelected[0] == 1)
				{
				  //Update the search history!
				  if(searchHistoryCounter < 15)
				  {
					searchHistoryCounter++;
				  }
				  memmove(&searchHistory[1], &searchHistory[0], sizeof(Block) * (15));
				  searchHistory[1].stdVal=6;
				  
				  //Move the cursor back
				  extSelected[0]=0;
				  
				  //Is it the first search?
				  if(searchNo == 0)
				  {
					//Increment the search
					searchNo++;
					searchMax++;
					
					//Setup the variables
					searchResultCounter=0;
					//Open the file
					//sprintf(buffer, "ms0:/search%d.dat", (searchNo-1));
					//fd=sceIoOpen(buffer, PSP_O_WRONLY | PSP_O_CREAT, 0777);
					if(searchNo>=0)
					{
					  //Write out the searcHistory[0] type
					  switch(searchHistory[0].flags & FLAG_DWORD)
					  {
						case FLAG_DWORD: *(unsigned char *)(RAMTEMP-1)=0x34;DumpByte=8;//DumpInterval=500;
						break;
						case FLAG_WORD:	*(unsigned char *)(RAMTEMP-1)=0x32;DumpByte=6;//DumpInterval=666;
						break;
						case FLAG_BYTE:	*(unsigned char *)(RAMTEMP-1)=0x31;DumpByte=5;//DumpInterval=800;
						break;
						//case FLAG_DWORD:if(sceIoWrite(fd, "4", 1)!=1) goto ErrorReadExactA;break;
						//case FLAG_WORD:if(sceIoWrite(fd, "2", 1)!=1) goto ErrorReadExactA;break;
						//case FLAG_BYTE:if(sceIoWrite(fd, "1", 1)!=1) goto ErrorReadExactA;break;
						}
					  //if(speedmode){DumpInterval=500;}
					  //Search!
					  counter=searchStart;
					  
					  //Helper
					  while(counter < searchStop)
						{
						//Helper
						if(!((counter - searchStart) & 0xFFFF))
						{
						  if(!cheatPause) sceKernelDelayThread(1500);
						  
							lineClear(33);
							pspDebugScreenSetTextColor(color02); sprintf(buffer, "Task = %02d%%; () = Hold to Abort", (counter-searchStart + 1)/((searchStop-searchStart + 0x64)/0x64)); pspDebugScreenPuts(buffer); 

						  sceCtrlPeekBufferPositive(&pad, 1);
						  
						  if(pad.Buttons & SWAPBUTTON2)
						  {
							lineClear(33);
							pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("Task Aborted!!!"); 
							
							do
							{
							  sceKernelDelayThread(50000);
								sceCtrlPeekBufferPositive(&pad, 1);
							}while(pad.Buttons & SWAPBUTTON2);
							break;
						  }
						}
					  
						//Check
						switch(searchHistory[0].flags & FLAG_DWORD)
						{
							case FLAG_DWORD:
							if(*((unsigned int*)(counter)) == (unsigned int)searchHistory[0].hakVal)
							{
							  //Add it
							  if(searchResultCounter<searchResultCounterMax){
							  *(unsigned int *)(RAMTEMP+8*searchResultCounter)=counter;
							  *(unsigned int *)(RAMTEMP+4+8*searchResultCounter)=searchHistory[0].hakVal;
							  }
							  //if(sceIoWrite(fd, &counter, sizeof(unsigned int))!=4) goto ErrorReadExactA;
							  //if(sceIoWrite(fd, &searchHistory[0].hakVal, sizeof(unsigned int))!=4) goto ErrorReadExactA;
							  searchResultCounter++;
							}
							counter+=4;
							break;
							
							case FLAG_WORD:
							if(*((unsigned short*)(counter)) == (unsigned short)searchHistory[0].hakVal)
							{
							  //Add it
							  if(searchResultCounter<searchResultCounterMax){
							  *(unsigned short *)(RAMTEMP+2+6*searchResultCounter)=counter>>16;
							  *(unsigned short *)(RAMTEMP+6*searchResultCounter)=counter;
							  *(unsigned short *)(RAMTEMP+4+6*searchResultCounter)=searchHistory[0].hakVal;
							  }
							  //if(sceIoWrite(fd, &counter, sizeof(unsigned int))!=4) goto ErrorReadExactA;
							  //if(sceIoWrite(fd, &searchHistory[0].hakVal, sizeof(unsigned short))!=2) goto ErrorReadExactA;
							  searchResultCounter++;
							}
							counter+=2;
							break;
							
							case FLAG_BYTE:
							if(*((unsigned char*)(counter)) == (unsigned char)searchHistory[0].hakVal)
							{
							  //Add it
							  if(searchResultCounter<searchResultCounterMax){
							  *(unsigned char *)(RAMTEMP+3+5*searchResultCounter)=counter>>24;
							  *(unsigned char *)(RAMTEMP+2+5*searchResultCounter)=counter>>16;
							  *(unsigned char *)(RAMTEMP+1+5*searchResultCounter)=counter>>8;
							  *(unsigned char *)(RAMTEMP+5*searchResultCounter)=counter;
							  *(unsigned char *)(RAMTEMP+4+5*searchResultCounter)=searchHistory[0].hakVal;
							  }
							  //if(sceIoWrite(fd, &counter, sizeof(unsigned int))!=4) goto ErrorReadExactA; 
							  //if(sceIoWrite(fd, &searchHistory[0].hakVal, sizeof(unsigned char))!=1) goto ErrorReadExactA;
							  searchResultCounter++;
							}
							counter++;
							break;
							}
							  //if((searchResultCounter<searchResultCounterMax)&&(((searchResultCounter+1) % DumpInterval)==0)){
							  //sceIoWrite(fd, (void*)0x08802890, DumpByte*DumpInterval);
							  //}
					  }
					  //Close the file since we are done with the search
					fd=sceIoOpen("ms0:/search1.dat", PSP_O_WRONLY | PSP_O_CREAT, 0777);
					  if(searchResultCounter<searchResultCounterMax){
					sceIoWrite(fd, (void*)RAMTEMP-1, DumpByte*searchResultCounter+1);}
					  else{
					sceIoWrite(fd, (void*)RAMTEMP-1, DumpByte*searchResultCounterMax+1);}
					sceIoClose(fd);
					  
					  while(1)
					  {
						break;
						//ReadShort
						ErrorReadExactA:
						sceIoClose(fd);
						if(searchNo > 0) searchNo--;
						sceIoRemove(buffer);
						
						lineClear(33);
						pspDebugScreenSetTextColor(color01);
						pspDebugScreenPuts("ERROR: MemoryStick out of Space!"); 
						sceKernelDelayThread(3000000);
							break;
					  }
					}
					else
					{
					  //ERROR - file couldn't be opened - undo search attempt
					if(searchNo > 0) searchNo--;
					}
				  }
					else //Continue the search with a different exact number
				  {
					//Increment the search
					searchNo++;
					searchMax++;
					
					//Open the files
					  sprintf(buffer, "ms0:/search%d.dat", searchNo-1);
					  fd=sceIoOpen(buffer, PSP_O_RDONLY, 0777);
					//sprintf(buffer, "ms0:/search%d.dat", searchNo);
					//fd2=sceIoOpen(buffer, PSP_O_WRONLY | PSP_O_CREAT, 0777);
					if(fd>0)
					{
					  //Get ready to go through and check the addresses
					  sceIoLseek(fd, 1, SEEK_SET);

										//Write out the searcHistory[0] type
					 switch(searchHistory[0].flags & FLAG_DWORD)

					  {
						case FLAG_DWORD:*(unsigned char *)(RAMTEMP-1)=0x34;DumpByte=8;//DumpInterval=500;
						break;
						case FLAG_WORD:	*(unsigned char *)(RAMTEMP-1)=0x32;DumpByte=6;//DumpInterval=666;
						break;
						case FLAG_BYTE:	*(unsigned char *)(RAMTEMP-1)=0x31;DumpByte=5;//DumpInterval=800;
						break;
						//case FLAG_DWORD:if(sceIoWrite(fd2, "4", 1)!=1) goto ErrorReadExactB;break;   
						//case FLAG_WORD:if(sceIoWrite(fd2, "2", 1)!=1) goto ErrorReadExactB;break;
						//case FLAG_BYTE:if(sceIoWrite(fd2, "1", 1)!=1) goto ErrorReadExactB;break;
						}
					  
					  //Loop through the list checking each one
					  counter=searchResultCounter;
					  searchResultCounter=0;
					  while(counter > 0)
					  {
						//Load it
						sceIoRead(fd, &scounter, sizeof(unsigned int));
						
						//Helper
						if(!(counter & 0x3F))
						{
						  if(!cheatPause) sceKernelDelayThread(1500);
						  
						lineClear(33);
						pspDebugScreenSetTextColor(color02);
						sprintf(buffer, "Task = %02d%%; () = Hold to Abort", (scounter-searchStart +1 )/((searchStop-searchStart+ 0x64)/0x64));
						pspDebugScreenPuts(buffer); 
						
						  sceCtrlPeekBufferPositive(&pad, 1);
						  
						  if(pad.Buttons & SWAPBUTTON2)
						  {
							lineClear(33);
							pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("Task Aborted!!!"); 
							
							do
							{
							sceKernelDelayThread(50000);
						        sceCtrlPeekBufferPositive(&pad, 1);
							}while(pad.Buttons & SWAPBUTTON2);
							break;
						  }
						}
						
						if(scounter>0){
						//Check
						switch(searchHistory[0].flags & FLAG_DWORD)
						{
							case FLAG_DWORD:
							sceIoLseek(fd, 4, SEEK_CUR);
							if(*((unsigned int*)(scounter)) == (unsigned int)searchHistory[0].hakVal)
							{
							  //Add it
							  if(searchResultCounter<searchResultCounterMax){
							  *(unsigned int *)(RAMTEMP+8*searchResultCounter)=scounter;
							  *(unsigned int *)(RAMTEMP+4+8*searchResultCounter)=searchHistory[0].hakVal;
							  }
							  //if(sceIoWrite(fd2, &scounter, sizeof(unsigned int))!=4) goto ErrorReadExactB;
							  //if(sceIoWrite(fd2, &searchHistory[0].hakVal, sizeof(unsigned int))!=4) goto ErrorReadExactB;
							  searchResultCounter++;
							}
							break;
							
							case FLAG_WORD:
							sceIoLseek(fd, 2, SEEK_CUR);
							if(*((unsigned short*)(scounter)) == (unsigned short)searchHistory[0].hakVal)
							{
							  //Add it
							  if(searchResultCounter<searchResultCounterMax){
							  *(unsigned short *)(RAMTEMP+2+6*searchResultCounter)=scounter>>16;
							  *(unsigned short *)(RAMTEMP+6*searchResultCounter)=scounter;
							  *(unsigned short *)(RAMTEMP+4+6*searchResultCounter)=searchHistory[0].hakVal;
							  }
							  //if(sceIoWrite(fd2, &scounter, sizeof(unsigned int))!=4) goto ErrorReadExactB;
							  //if(sceIoWrite(fd2, &searchHistory[0].hakVal, sizeof(unsigned short))!=2) goto ErrorReadExactB;
							  searchResultCounter++;
							}
							break;
							 
							case FLAG_BYTE:
							sceIoLseek(fd, 1, SEEK_CUR);
							if(*((unsigned char*)(scounter)) == (unsigned char)searchHistory[0].hakVal)
							{
							  //Add it
							  if(searchResultCounter<searchResultCounterMax){
							  *(unsigned char *)(RAMTEMP+3+5*searchResultCounter)=scounter>>24;
							  *(unsigned char *)(RAMTEMP+2+5*searchResultCounter)=scounter>>16;
							  *(unsigned char *)(RAMTEMP+1+5*searchResultCounter)=scounter>>8;
							  *(unsigned char *)(RAMTEMP+5*searchResultCounter)=scounter;
							  *(unsigned char *)(RAMTEMP+4+5*searchResultCounter)=searchHistory[0].hakVal;
							  }
							  //if(sceIoWrite(fd2, &scounter, sizeof(unsigned int))!=4) goto ErrorReadExactB;
							  //if(sceIoWrite(fd2, &searchHistory[0].hakVal, sizeof(unsigned char))!=1) goto ErrorReadExactB;
							  searchResultCounter++;
							}
							break;
							}
							  //if((searchResultCounter<searchResultCounterMax)&&(((searchResultCounter+1) % DumpInterval)==0)){
							  //sceIoWrite(fd, (void*)0x08802890, DumpByte*DumpInterval);
							  //}
						}
						//Next
						counter--;
					  }
					  //Close the files
					sceIoClose(fd);
					  sprintf(buffer, "ms0:/search%d.dat", searchNo);
					  fd=sceIoOpen(buffer, PSP_O_WRONLY | PSP_O_CREAT, 0777);
					  if(searchResultCounter<searchResultCounterMax){
					sceIoWrite(fd, (void*)RAMTEMP-1, DumpByte*searchResultCounter+1);}
					  else{
					sceIoWrite(fd, (void*)RAMTEMP-1, DumpByte*searchResultCounterMax+1);}
					  sceIoClose(fd);
					  
					  while(1)
					  {
						break;
						//ReadShort
						ErrorReadExactB:
						sceIoClose(fd);
						sceIoClose(fd2);
						if(searchNo > 0) searchNo--;
						sceIoRemove(buffer);
						lineClear(33);
						pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("ERROR: MemoryStick out of Space!"); 
						sceKernelDelayThread(3000000);
						break;
					  }
					}
					else
					{
					  //ERROR - files couldn't be opened - undo search attempt
					  sceIoClose(fd);
					  sceIoClose(fd2);
					  if(searchNo > 0) searchNo--;
					}
				  }
				}
				else if(extSelected[0] == 2)
				{
				  //Reset the search
				  if(searchNo > 0) searchNo--;
				  
				  //Move the cursor back
				  extSelected[0]=0;
				}
				else
				{
				  //Add the single cheat
				  switch(searchHistory[0].flags & FLAG_DWORD)
				  {
					case FLAG_DWORD:
					  cheatNew(4, searchAddress[extSelected[0]-3]  - 0x48800000, *((unsigned int*)(searchAddress[extSelected[0]-3])), 1, 0);
					  break;
					case FLAG_WORD:
					  cheatNew(2, searchAddress[extSelected[0]-3]  - 0x48800000, *((unsigned short*)(searchAddress[extSelected[0]-3])), 1, 0);
					  break;
					case FLAG_BYTE:
					  cheatNew(1, searchAddress[extSelected[0]-3]  - 0x48800000, *((unsigned char*)(searchAddress[extSelected[0]-3])), 1, 0);
					  break;
					default:
						break;
				  }
				  
				  //Switch to the cheat editor
				extOptArg=extSelected[0];
				pspDebugScreenInitEx(vram, 0, 0);
				extSelected[1]=extSelected[2]=extSelected[3]=0;
				extSelected[0]=cheat[cheatTotal - 1].block;
				cheatSelected=cheatTotal - 1;
				extMenu=1;
				extOpt=1;
				menuDraw();
				sceKernelDelayThread(150000);
				}
				
				//Load the file again, get the sample numbers
				if(searchNo > 0)
				{      
				  //Open the file
				  sprintf(buffer, "ms0:/search%d.dat", searchNo);
				  fd=sceIoOpen(buffer, PSP_O_RDONLY, 0777);
				  if(fd>0)
				  {
					//Get the value size
					sceIoRead(fd, &miscType, 1);
					miscType-='0';
					
					switch(miscType)
					{
					  case 1: searchHistory[0].flags=(searchHistory[0].flags & ~FLAG_DWORD) | FLAG_BYTE; break;
					  case 2: searchHistory[0].flags=(searchHistory[0].flags & ~FLAG_DWORD) | FLAG_WORD; break;
					  case 4: searchHistory[0].flags=(searchHistory[0].flags & ~FLAG_DWORD) | FLAG_DWORD; break;
					}
					
					//Get the file size
					searchResultCounter=sceIoLseek(fd, 0, SEEK_END); sceIoLseek(fd, 1, SEEK_SET);
					searchResultCounter--;
					searchResultCounter/=(sizeof(unsigned int) + miscType);
					
					//Only load the first 100
					if(searchResultCounter > searchResultMax)
					{
					  for(scounter=0; scounter<searchResultMax; scounter++)
					  {
						sceIoRead(fd, &searchAddress[scounter], sizeof(unsigned int));
						sceIoLseek(fd, miscType, SEEK_CUR);
						}
					}
					else
					{
					  for(scounter=0; scounter<searchResultCounter; scounter++)
					  {
						sceIoRead(fd, &searchAddress[scounter], sizeof(unsigned int));
						sceIoLseek(fd, miscType, SEEK_CUR);
						}
					}
					
					//Close the file since we are done with the search
							sceIoClose(fd);
				  }
				}
				else
				{
				  searchResultCounter=0;
				}
				
				pspDebugScreenInitEx(vram, 0, 0);
				menuDraw();
				sceKernelDelayThread(150000);
			  }
			
				if(extSelected[3])
			  {
				if(pad.Buttons & PSP_CTRL_UP)
				{
				  switch(extSelected[1])
				  {
					case 0: searchHistory[0].hakVal+=(1 << (4*(7-extSelected[2]))); break;
					case 1: searchHistory[0].hakVal+=decDelta[extSelected[2]]; break;
					case 2: searchHistory[0].hakVal+=(1 << (8*(extSelected[2]))); break;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
				  switch(extSelected[1])
				  {
					case 0: searchHistory[0].hakVal-=(1 << (4*(7-extSelected[2]))); break;
					case 1: searchHistory[0].hakVal-=decDelta[extSelected[2]]; break;
					case 2: searchHistory[0].hakVal-=(1 << (8*(extSelected[2]))); break;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else
				{
				  cheatButtonAgeY=0;
				}
			  }
			  else
			  {
				if(pad.Buttons & PSP_CTRL_UP)
				{
				  if(extSelected[0] > 0)
				  {
					extSelected[0]--;
				  }
				menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
				  if(extSelected[0] < (1+(!!searchNo)+(searchResultCounter>searchResultMax? searchResultMax:searchResultCounter)))
				  {
					extSelected[0]++;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				if(pad.Buttons & PSP_CTRL_LTRIGGER)
				{
				  if(extSelected[0] > 23)
				  {
					extSelected[0]=extSelected[0]-20;
				  }
				  else if(extSelected[0] <3){
				  }
				  else{
				  extSelected[0]=3;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				
				}
				else if(pad.Buttons & PSP_CTRL_RTRIGGER)
				{
				  if(extSelected[0]<3){
				  }
				  else if(extSelected[0]+23 < (searchResultCounter>searchResultMax? searchResultMax:searchResultCounter))
				  {
					extSelected[0]=extSelected[0]+20;
				  }
				  else{
				  	if(searchResultCounter>searchResultMax){
				  	extSelected[0]=searchResultMax+2;}
				  	else{
				  	extSelected[0]=searchResultCounter+2;}
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else
				{
				  cheatButtonAgeY=0;
				}
			  }
			  
			  if(extSelected[0] == 0)
			  {
				if(pad.Buttons & PSP_CTRL_LEFT)
				{
				  extSelected[2]--;
					switch(extSelected[1])
				  {
					case 0: if((signed)extSelected[2] == -1) { extSelected[2]=0; } break;
					case 1: if((signed)extSelected[2] == -1) { extSelected[2]=7; extSelected[1]--; } break;
					case 2: if((signed)extSelected[2] == -1) { extSelected[2]=9; extSelected[1]--; } break;
				  }
				  menuDraw();
				  if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else if(pad.Buttons & PSP_CTRL_RIGHT)
				{
				  extSelected[2]++;
					switch(extSelected[1])
				  {
					case 0: if(editFormat==1){ if(extSelected[2] > 7) { extSelected[2]=7; }}
						else{ if(extSelected[2] > 7) { extSelected[2]=0; extSelected[1]++; }} break;
					case 1:	if(extSelected[2] > 9) { extSelected[2]=0; extSelected[1]++; } break;
					case 2: if(extSelected[2] > 3) { extSelected[2]=3; } break;
				  }
					menuDraw();
					if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else
				{
				  cheatButtonAgeX=0;
				}
			  }
			  
				if((padButtons & SWAPBUTTON2) && !(pad.Buttons & SWAPBUTTON2))
				{
				if(extSelected[3])
				{
				  extSelected[3]=0;
					menuDraw();
					sceKernelDelayThread(150000);
				}
				else //Search has been aborted!
				{
				  //Do we need to act like the search didn't even happen???
				  if(searchNo == 0)
				  {
					cheatSearch=0;
				  }
				  
				  //Go back
				  cheatSelected=0;
					pspDebugScreenInitEx(vram, 0, 0);
					extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=0;
					extMenu=0;
				        extOpt=0;
					menuDraw();
					sceKernelDelayThread(150000);
					break;
					}
			  }
				break;
			  
			case 3: //INPUT EXT DIFF SEARCH
				if(pad.Buttons & PSP_CTRL_START){
					//change format
					if(editFormat==0){
						editFormat=1;
					}
					else{
						editFormat=0;
					}
					menuDraw();
					sceKernelDelayThread(150000);
				}

				if(pad.Buttons & PSP_CTRL_TRIANGLE) { copyMenu=1; menuDraw(); sceKernelDelayThread(150000);}
			  	if(pad.Buttons & SWAPBUTTON)
			  	{
				if(extSelected[0] == 0)
				{
					extSelected[3]=!extSelected[3];
				}
				else if(extSelected[0] == 1)
				{
				  //Update the search history!
				  if(searchHistoryCounter < 15)
				  {
					searchHistoryCounter++;
				  }
				  memmove(&searchHistory[1], &searchHistory[0], sizeof(Block) * (15));
				  searchHistory[1].stdVal=searchMode;
				  
				  //Move the cursor back
				  extSelected[0]=0;
              //Is it the first search? //OLD NPR SRC
          if(searchNo == 0)
              {
                //Increment the search
                searchNo++;
                searchMax++;
                
                //Setup the variables
                searchResultCounter=0;
		
                
                //Open the files
                fd=sceIoOpen("ms0:/search.ram", PSP_O_RDONLY, 0777);
                //sprintf(buffer, "ms0:/search%d.dat", searchNo-1);
                //fd2=sceIoOpen(buffer, PSP_O_WRONLY | PSP_O_CREAT, 0777);
                if(fd>0)
                {
                  //Skip the initial 0x4000 bytes
                  //sceIoLseek(fd, 0x4000, SEEK_SET);
                  sceIoLseek(fd,(searchStart - 0x48800000), SEEK_SET);

     		//Write out the searcHistory[0] type
                  switch(searchHistory[0].flags & FLAG_DWORD)
                  {
			case FLAG_DWORD: *(unsigned char *)(RAMTEMP-1)=0x34;DumpByte=8;//DumpInterval=500;
			miscType=4;break;
			case FLAG_WORD:	*(unsigned char *)(RAMTEMP-1)=0x32;DumpByte=6;//DumpInterval=666;
			miscType=2;break;
			case FLAG_BYTE:	*(unsigned char *)(RAMTEMP-1)=0x31;DumpByte=5;//DumpInterval=800;
			miscType=1;break;
                  	//case FLAG_DWORD:if(sceIoWrite(fd2, "4", 1)!=1) goto ErrorReadDiffA;miscType=4;break;   
                  	//case FLAG_WORD:if(sceIoWrite(fd2, "2", 1)!=1) goto ErrorReadDiffA;miscType=2;break;
                  	//case FLAG_BYTE:if(sceIoWrite(fd2, "1", 1)!=1) goto ErrorReadDiffA;miscType=1;break;
	                }
                  
                  //Get ready
                  //counter=0x48804000;
                  counter=searchStart;
                  //Go!
                  while(counter < searchStop)//0x4A000000)
                  { 
                    //Load it
                    //sceIoRead(fd, &scounter, miscType);
                    fileBufferRead(&scounter, miscType);
                    
                    //Helper
                    //if(!((counter - 0x48804000) & 0xFFFF))
                    if(!((counter - searchStart) & 0xFFFF))
                    {
                      if(!cheatPause) sceKernelDelayThread(1500);
                      
			lineClear(33);
        		pspDebugScreenSetTextColor(color02);
			sprintf(buffer, "Task = %02d%%; () = Hold to Abort", (counter - searchStart + 1)/((searchStop - searchStart + 0x64)/0x64));
			pspDebugScreenPuts(buffer); 
                  		
                      sceCtrlPeekBufferPositive(&pad, 1);
                      
                      if(pad.Buttons & SWAPBUTTON2)
                      {
                        lineClear(33);
        		pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("Task Aborted!!!"); 
                  		
                        do
                        {
                          sceKernelDelayThread(50000);
                        	sceCtrlPeekBufferPositive(&pad, 1);
                        }while(pad.Buttons & SWAPBUTTON2);
                        break;
                      }
                    }
                    
                    //Check
                    switch(searchHistory[0].flags & FLAG_DWORD)
                  	{
                    	case FLAG_DWORD:
                      	if(searchMode==0)//equal
                        {
                          if((unsigned int)scounter != *((unsigned int*)(counter))) break;
                        }
                        else if(searchMode==1)//not equal
                        {
                          if((unsigned int)scounter == *((unsigned int*)(counter))) break;
                        }
                        else if(searchMode==2)//greater
                        {
                          if((unsigned int)scounter >= *((unsigned int*)(counter))) break;
                        }
                        else if(searchMode==3)//less
                        {
                          if((unsigned int)scounter <= *((unsigned int*)(counter))) break;
                        }
                        else if(searchMode==4)//inc
                        {
                          if((unsigned int)((unsigned int)scounter + (unsigned int)searchHistory[0].hakVal) != *((unsigned int*)(counter))) break;
                        }
                        else if(searchMode==5)//dec
                        {
                          if((unsigned int)((unsigned int)scounter - (unsigned int)searchHistory[0].hakVal) != *((unsigned int*)(counter))) break;
                        }
                        scounter=*((unsigned int*)(counter));
                        
                        //Add it
			if(searchResultCounter<searchResultCounterMax){
			 *(unsigned int *)(RAMTEMP+8*searchResultCounter)=counter;
			 *(unsigned int *)(RAMTEMP+4+8*searchResultCounter)=scounter;
			}
                        //if(sceIoWrite(fd2, &counter, sizeof(unsigned int))!=4) goto ErrorReadDiffA;
                        //if(sceIoWrite(fd2, &scounter, sizeof(unsigned int))!=4) goto ErrorReadDiffA;
                        searchResultCounter++;
                      	break;
                        
                    	case FLAG_WORD:
                      	if(searchMode==0)
                        {
                          if((unsigned short)scounter != *((unsigned short*)(counter))) break;
                        }
                        else if(searchMode==1)
                        {
                          if((unsigned short)scounter == *((unsigned short*)(counter))) break;
                        }
                        else if(searchMode==2)
                        {
                          if((unsigned short)scounter >= *((unsigned short*)(counter))) break;
                        }
                        else if(searchMode==3)
                        {
                          if((unsigned short)scounter <= *((unsigned short*)(counter))) break;
                        }
                        else if(searchMode==4)
                        {
                          if((unsigned short)((unsigned short)scounter + (unsigned short)searchHistory[0].hakVal) != *((unsigned short*)(counter))) break;
                        }
                        else if(searchMode==5)
                        {
                          if((unsigned short)((unsigned short)scounter - (unsigned short)searchHistory[0].hakVal) != *((unsigned short*)(counter))) break;
                        }
                        scounter=*((unsigned short*)(counter));
                      
                        //Add it
			if(searchResultCounter<searchResultCounterMax){
			 *(unsigned short *)(RAMTEMP+2+6*searchResultCounter)=counter>>16;
			 *(unsigned short *)(RAMTEMP+6*searchResultCounter)=counter;
			 *(unsigned short *)(RAMTEMP+4+6*searchResultCounter)=scounter;
			 }
                        //if(sceIoWrite(fd2, &counter, sizeof(unsigned int))!=4) goto ErrorReadDiffA;
                        //if(sceIoWrite(fd2, &scounter, sizeof(unsigned short))!=2) goto ErrorReadDiffA;
                        searchResultCounter++;
                      	break;
                         
                    	case FLAG_BYTE:
                      	if(searchMode==0)
                        {
                          if((unsigned char)scounter != *((unsigned char*)(counter))) break;
                        }
                        else if(searchMode==1)
                        {
                          if((unsigned char)scounter == *((unsigned char*)(counter))) break;
                        }
                        else if(searchMode==2)
                        {
                          if((unsigned char)scounter >= *((unsigned char*)(counter))) break;
                        }
                        else if(searchMode==3)
                        {
                          if((unsigned char)scounter <= *((unsigned char*)(counter))) break;
                        }
                        else if(searchMode==4)
                        {
                          if((unsigned char)((unsigned char)scounter + (unsigned char)searchHistory[0].hakVal) != *((unsigned char*)(counter))) break;
                        }
                        else if(searchMode==5)
                        {
                          if((unsigned char)((unsigned char)scounter - (unsigned char)searchHistory[0].hakVal) != *((unsigned char*)(counter))) break;
                        }
                        scounter=*((unsigned char*)(counter));
                      
                        //Add it
			if(searchResultCounter<searchResultCounterMax){
			 *(unsigned char *)(RAMTEMP+3+5*searchResultCounter)=counter>>24;
			 *(unsigned char *)(RAMTEMP+2+5*searchResultCounter)=counter>>16;
			 *(unsigned char *)(RAMTEMP+1+5*searchResultCounter)=counter>>8;
			 *(unsigned char *)(RAMTEMP+5*searchResultCounter)=counter;
			 *(unsigned char *)(RAMTEMP+4+5*searchResultCounter)=scounter;
			 }
                        //if(sceIoWrite(fd2, &counter, sizeof(unsigned int))!=4) goto ErrorReadDiffA;
                        //if(sceIoWrite(fd2, &scounter, sizeof(unsigned char))!=1) goto ErrorReadDiffA;
                        searchResultCounter++;
                      	break;
	                	}
                    
		 //if((searchResultCounter<searchResultCounterMax)&&(((searchResultCounter+1) % DumpInterval)==0)){
		 //sceIoWrite(fd, (void*)0x08802890, DumpByte*DumpInterval);
		 //}
                    //Next
                  	counter+=miscType;
                  }
                  //Close the files		
		  sceIoClose(fd);
		 fd=sceIoOpen("ms0:/search1.dat", PSP_O_WRONLY | PSP_O_CREAT, 0777);
		  if(searchResultCounter<searchResultCounterMax){
		sceIoWrite(fd, (void*)RAMTEMP-1, DumpByte*searchResultCounter+1);}
		  else{
		sceIoWrite(fd, (void*)RAMTEMP-1, DumpByte*searchResultCounterMax+1);}
		sceIoClose(fd);
                  
                  while(1)
                  {
                    break;
                  	//ReadShort
                  	ErrorReadDiffA:
               		sceIoClose(fd);
                  	sceIoClose(fd2);
                  	if(searchNo > 0) searchNo--;
                    sceIoRemove(buffer);
                    
                    lineClear(33);
		    pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("ERROR: MemoryStick out of Space!"); 
                    sceKernelDelayThread(3000000);
                    break;
                  }
                }
                else
                {
                  //ERROR - files couldn't be opened - undo search attempt
                  sceIoClose(fd);
                  sceIoClose(fd2);
                  if(searchNo > 0) searchNo--;
                }
              }
	else //Continue the search with a different Diff number
				  {
					//Increment the search
					searchNo++;
					searchMax++;
					
					//Open the files
					sprintf(buffer, "ms0:/search%d.dat", searchNo-1);
					fd=sceIoOpen(buffer, PSP_O_RDONLY, 0777);
					//sprintf(buffer, "ms0:/search%d.dat", searchNo);
					//fd2=sceIoOpen(buffer, PSP_O_WRONLY | PSP_O_CREAT, 0777);
					if(searchNo>0)
					{
					  //Get ready to go through and check the addresses
					  sceIoLseek(fd, 1, SEEK_SET);

					//Write out the searcHistory[0] type
					 switch(searchHistory[0].flags & FLAG_DWORD)
					  {
					case FLAG_DWORD: *(unsigned char *)(RAMTEMP-1)=0x34;DumpByte=8;miscType=4;break;
					case FLAG_WORD:	*(unsigned char *)(RAMTEMP-1)=0x32;DumpByte=6;miscType=2;break;
					case FLAG_BYTE:	*(unsigned char *)(RAMTEMP-1)=0x31;DumpByte=5;miscType=1;break;
		                  	}
					  
					  //Loop through the list checking each one
				
					  counter=searchResultCounter;
					  searchResultCounter=0;
					  while(counter > 0)
					  {
						//Load it
						sceIoRead(fd, &scounter, sizeof(unsigned int));
						
						//Helper
						if(!(counter & 0x3F))
						{
						  if(!cheatPause) sceKernelDelayThread(1500);
						  
							lineClear(33);
							pspDebugScreenSetTextColor(color02);
							sprintf(buffer, "Task = %02d%%; () = Hold to Abort", (scounter - searchStart+ 1)/((searchStop - searchStart+ 0x64)/0x64));
							pspDebugScreenPuts(buffer);
							
						  sceCtrlPeekBufferPositive(&pad, 1);
						  
						  if(pad.Buttons & SWAPBUTTON2)
						  {
							lineClear(33);
											pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("Task Aborted!!!"); 
							
							do
							{
							  sceKernelDelayThread(50000);
								sceCtrlPeekBufferPositive(&pad, 1);
							}while(pad.Buttons & SWAPBUTTON2);
							break;
						  }
						}
						
						if(scounter>0){
						//Check
						switch(searchHistory[0].flags & FLAG_DWORD)
						{
							case FLAG_DWORD:
							sceIoRead(fd, &dcounter, sizeof(unsigned int));
							
							if(searchMode==0)
							{
							  if((unsigned int)dcounter != *((unsigned int*)(scounter))) break;
							}
							else if(searchMode==1)
							{
							  if((unsigned int)dcounter == *((unsigned int*)(scounter))) break;
							}
							else if(searchMode==2)
							{
							  if((unsigned int)dcounter >= *((unsigned int*)(scounter))) break;
							}
							else if(searchMode==3)
							{
							  if((unsigned int)dcounter <= *((unsigned int*)(scounter))) break;
							}
							else if(searchMode==4)
							{
							  if((unsigned int)((unsigned int)dcounter + (unsigned int)searchHistory[0].hakVal) != *((unsigned int*)(scounter))) break;
							}
							else if(searchMode==5)
							{
							  if((unsigned int)((unsigned int)dcounter - (unsigned int)searchHistory[0].hakVal) != *((unsigned int*)(scounter))) break;
							}
							dcounter=*((unsigned int*)(scounter));
							
							//Add it
							if(searchResultCounter<searchResultCounterMax){
							 *(unsigned int *)(RAMTEMP+8*searchResultCounter)=scounter;
							 *(unsigned int *)(RAMTEMP+4+8*searchResultCounter)=dcounter;
			 				}
							//if(sceIoWrite(fd2, &scounter, sizeof(unsigned int))!=4) goto ErrorReadDiffB;
							//if(sceIoWrite(fd2, &dcounter, sizeof(unsigned int))!=4) goto ErrorReadDiffB;
							searchResultCounter++;
							break;
							
							case FLAG_WORD:
							sceIoRead(fd, &dcounter, sizeof(unsigned short));

							if(searchMode==0)
							{
							  if((unsigned short)dcounter != *((unsigned short*)(scounter))) break;
							}
							else if(searchMode==1)
							{
							  if((unsigned short)dcounter == *((unsigned short*)(scounter))) break;
							}
							else if(searchMode==2)
							{
							  if((unsigned short)dcounter >= *((unsigned short*)(scounter))) break;
							}
							else if(searchMode==3)
							{
							  if((unsigned short)dcounter <= *((unsigned short*)(scounter))) break;
							}
							else if(searchMode==4)
							{
							  if((unsigned short)((unsigned short)dcounter + (unsigned short)searchHistory[0].hakVal) != *((unsigned short*)(scounter))) break;
							}
							else if(searchMode==5)
							{
							  if((unsigned short)((unsigned short)dcounter - (unsigned short)searchHistory[0].hakVal) != *((unsigned short*)(scounter))) break;
							}
							dcounter=*((unsigned short*)(scounter));

							//Add it
							if(searchResultCounter<searchResultCounterMax){
							*(unsigned short *)(RAMTEMP+2+6*searchResultCounter)=scounter>>16;
							*(unsigned short *)(RAMTEMP+6*searchResultCounter)=scounter;
			 				*(unsigned short *)(RAMTEMP+4+6*searchResultCounter)=dcounter;
			 				}
							//if(sceIoWrite(fd2, &scounter, sizeof(unsigned int))!=4) goto ErrorReadDiffB;
							//if(sceIoWrite(fd2, &dcounter, sizeof(unsigned short))!=2) goto ErrorReadDiffB;
							searchResultCounter++;
							break;
							 
							case FLAG_BYTE:
							sceIoRead(fd, &dcounter, sizeof(unsigned char));

							if(searchMode==0)
							{
							  if((unsigned char)dcounter != *((unsigned char*)(scounter))) break;
							}
							else if(searchMode==1)
							{
							  if((unsigned char)dcounter == *((unsigned char*)(scounter))) break;
							}
							else if(searchMode==2)
							{
							  if((unsigned char)dcounter >= *((unsigned char*)(scounter))) break;
							}
							else if(searchMode==3)
							{
							  if((unsigned char)dcounter <= *((unsigned char*)(scounter))) break;
							}
							else if(searchMode==4)
							{
							  if((unsigned char)((unsigned char)dcounter + (unsigned char)searchHistory[0].hakVal) != *((unsigned char*)(scounter))) break;
							}
							else if(searchMode==5)
							{
							  if((unsigned char)((unsigned char)dcounter - (unsigned char)searchHistory[0].hakVal) != *((unsigned char*)(scounter))) break;
							}
							dcounter=*((unsigned char*)(scounter));

							//Add it
							if(searchResultCounter<searchResultCounterMax){
			 				*(unsigned char *)(RAMTEMP+3+5*searchResultCounter)=scounter>>24;
							*(unsigned char *)(RAMTEMP+2+5*searchResultCounter)=scounter>>16;
							*(unsigned char *)(RAMTEMP+1+5*searchResultCounter)=scounter>>8;
			 				*(unsigned char *)(RAMTEMP+5*searchResultCounter)=scounter;
			 				*(unsigned char *)(RAMTEMP+4+5*searchResultCounter)=dcounter;
							}
							//if(sceIoWrite(fd2, &scounter, sizeof(unsigned int))!=4) goto ErrorReadDiffB;
							//if(sceIoWrite(fd2, &dcounter, sizeof(unsigned char))!=1) goto ErrorReadDiffB;
							searchResultCounter++;
							break;
							}
						}
						//Next
							  //if((searchResultCounter<searchResultCounterMax)&&(((searchResultCounter+1) % DumpInterval)==0)){
							  //sceIoWrite(fd, (void*)0x08802890, DumpByte*DumpInterval);
							  //}
						counter--;
					  }
					  
					  //Close the files	
					sceIoClose(fd);
					sprintf(buffer, "ms0:/search%d.dat", searchNo);
					fd=sceIoOpen(buffer, PSP_O_WRONLY | PSP_O_CREAT, 0777);
					  if(searchResultCounter<searchResultCounterMax){
					sceIoWrite(fd, (void*)RAMTEMP-1, DumpByte*searchResultCounter+1);}
					  else{
					sceIoWrite(fd, (void*)RAMTEMP-1, DumpByte*searchResultCounterMax+1);}
					  sceIoClose(fd);
					  
					  while(1)
					  {
						break;
						//ReadShort
						ErrorReadDiffB:

						sceIoClose(fd);
						sceIoClose(fd2);
						if(searchNo > 0) searchNo--;
						sceIoRemove(buffer);
						
						lineClear(33);
						pspDebugScreenSetTextColor(color01);
						pspDebugScreenPuts("ERROR: MemoryStick out of Space!"); 
						sceKernelDelayThread(3000000);
						break;
					  }
					}
					else
					{
					  //ERROR - files couldn't be opened - undo search attempt
					  sceIoClose(fd);
					  sceIoClose(fd2);
					  if(searchNo > 0) searchNo--;
					}
				  }
				}
				else if(extSelected[0] == 2)
				{
				  //Reset the search
				  if(searchNo > 0) searchNo--;
				  
				  //Move the cursor back
				  extSelected[0]=0;
				}
				else
				{
				  //Add the single cheat
				  switch(searchHistory[0].flags & FLAG_DWORD)
				  {
					case FLAG_DWORD:
					  cheatNew(4, searchAddress[extSelected[0]-3]  - 0x48800000, *((unsigned int*)(searchAddress[extSelected[0]-3])), 1, 0);
					  break;
					case FLAG_WORD:
					  cheatNew(2, searchAddress[extSelected[0]-3]  - 0x48800000, *((unsigned short*)(searchAddress[extSelected[0]-3])), 1, 0);
					  break;
					case FLAG_BYTE:
					  cheatNew(1, searchAddress[extSelected[0]-3]  - 0x48800000, *((unsigned char*)(searchAddress[extSelected[0]-3])), 1, 0);
					  break;
					default:
						break;
				  }
				  
				  //Switch to the cheat editor
				  extOptArg=extSelected[0];
				  pspDebugScreenInitEx(vram, 0, 0);
				  extSelected[1]=extSelected[2]=extSelected[3]=0;
				  extSelected[0]=cheat[cheatTotal - 1].block;
				  cheatSelected=cheatTotal - 1;
			          extMenu=1;
				  extOpt=2;
				  menuDraw();
				  sceKernelDelayThread(150000);
				}
				
				//Load the file again, get the sample numbers
				if(searchNo > 0)
				{      
				  //Open the file
				  sprintf(buffer, "ms0:/search%d.dat", searchNo);
				  fd=sceIoOpen(buffer, PSP_O_RDONLY, 0777);
				  if(fd>0)
				  {
					//Get the value size
					sceIoRead(fd, &miscType, 1);
					miscType-='0';
					
					switch(miscType)
					{
					  case 1: searchHistory[0].flags=(searchHistory[0].flags & ~FLAG_DWORD) | FLAG_BYTE; break;
					  case 2: searchHistory[0].flags=(searchHistory[0].flags & ~FLAG_DWORD) | FLAG_WORD; break;
					  case 4: searchHistory[0].flags=(searchHistory[0].flags & ~FLAG_DWORD) | FLAG_DWORD; break;
					}
					
					//Get the file size
					searchResultCounter=sceIoLseek(fd, 0, SEEK_END); sceIoLseek(fd, 1, SEEK_SET);
					searchResultCounter--;
					searchResultCounter/=(sizeof(unsigned int) + miscType);
					
					//Only load the first 100
					if(searchResultCounter > searchResultMax)
					{
					  for(scounter=0; scounter < searchResultMax; scounter++)
					  {
						sceIoRead(fd, &searchAddress[scounter], sizeof(unsigned int));
						sceIoLseek(fd, miscType, SEEK_CUR);
						}
					}
					else
					{
					  for(scounter=0; scounter<searchResultCounter; scounter++)
					  {
						sceIoRead(fd, &searchAddress[scounter], sizeof(unsigned int));
						sceIoLseek(fd, miscType, SEEK_CUR);
						}
					}
					
					//Close the file since we are done with the search
							sceIoClose(fd);
				  }
				}
				else
				{
				  searchResultCounter=0;
				}
				
				pspDebugScreenInitEx(vram, 0, 0);
				menuDraw();
				sceKernelDelayThread(150000);
			  }
			
				if(extSelected[3])
			  {
				if(pad.Buttons & PSP_CTRL_UP)
				{
				  switch(extSelected[1])
				  {
					case 0: if(searchMode < 5) searchMode++; 
				  	if(searchMode >5){
				  	searchMode=5;}
				  	break;
					case 1: searchHistory[0].hakVal+=(1 << (4*(7-extSelected[2]))); break;
					case 2: searchHistory[0].hakVal+=decDelta[extSelected[2]]; break;
					case 3: searchHistory[0].hakVal+=(1 << (8*(extSelected[2]))); break;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
				  switch(extSelected[1])
				  {
					case 0: if(searchMode > 0) searchMode--; break;
					case 1: searchHistory[0].hakVal-=(1 << (4*(7-extSelected[2]))); break;
					case 2: searchHistory[0].hakVal-=decDelta[extSelected[2]]; break;
					case 3: searchHistory[0].hakVal-=(1 << (8*(extSelected[2]))); break;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else
				{
				  cheatButtonAgeY=0;
				}
			  }
			  else
			  {
				if(pad.Buttons & PSP_CTRL_UP)
				{
				  if(extSelected[0] > 0)
				  {
					extSelected[0]--;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
				  if(extSelected[0] < (1+(!!searchNo)+(searchResultCounter>> searchResultMax? searchResultMax:searchResultCounter)))
				  {
					extSelected[0]++;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				if(pad.Buttons & PSP_CTRL_LTRIGGER)
				{
					if(extSelected[0]<3){
					}
				  else if(extSelected[0] > 23)
				  {
					extSelected[0]=extSelected[0]-20;
				  }
				  else{
				  extSelected[0]=3;}
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				
				}
				else if(pad.Buttons & PSP_CTRL_RTRIGGER)
				{
					if(extSelected[0]<3){
					}
				  else if(extSelected[0]+23 < (searchResultCounter>searchResultMax? searchResultMax:searchResultCounter))
				  {
					extSelected[0]=extSelected[0]+20;
				  }
				  else{
				  	if(searchResultCounter>searchResultMax){
				  	extSelected[0]=searchResultMax+2;}
				  	else{
				  	extSelected[0]=searchResultCounter+2;}
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else
				{
				  cheatButtonAgeY=0;
				}
			  }
			  
			  if(extSelected[0] == 0)
			  {
				if(pad.Buttons & PSP_CTRL_LEFT)
				{
				  extSelected[2]--;
					switch(extSelected[1])
				  {
					case 0: if((signed)extSelected[2] == -1) { extSelected[2]=0; } break;
					case 1: if((signed)extSelected[2] == -1) { extSelected[2]=0; extSelected[1]--; } break;
					case 2: if((signed)extSelected[2] == -1) { extSelected[2]=7; extSelected[1]--; } break;
					case 3: if((signed)extSelected[2] == -1) { extSelected[2]=9; extSelected[1]--; } break;
				  }
				  menuDraw();
				  if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else if(pad.Buttons & PSP_CTRL_RIGHT)
				{
				  extSelected[2]++;
				  if(searchMode < 4)
				  {
					extSelected[2]=0;
				  }
				  else//inc,dec
					{
					  switch(extSelected[1])
					{
					  case 0: if(extSelected[2] > 0) { extSelected[2]=0; extSelected[1]++; } break;
					  case 1: if(extSelected[2] > 7) { extSelected[2]=0; extSelected[1]++; } break;
					  case 2: if(extSelected[2] > 9) { extSelected[2]=9;} break;
					  //case 3: if(extSelected[2] > 3) { extSelected[2]=3; } break;
					}
				  }
					menuDraw();
					if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else
				{
				  cheatButtonAgeX=0;
				}
			  }
			  
				if((padButtons & SWAPBUTTON2) && !(pad.Buttons & SWAPBUTTON2))
				{
				if(extSelected[3])
				{
				  extSelected[3]=0;
					menuDraw();
					sceKernelDelayThread(150000);
				}
				else //Search has been aborted!
				{              
				  //Go back
					pspDebugScreenInitEx(vram, 0, 0);
					extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=0;
						extMenu=0;
				  extOpt=0;
					menuDraw();
					sceKernelDelayThread(150000);
					break;
					}
			  }
				break;
			  
		  case 4: //INPUT EXT TEXT SEARCH
				if(pad.Buttons & PSP_CTRL_START){
					keyboard=1;
					pspDebugKbInit(fileBuffer);
					sceKernelDelayThread(150000);
					if(fileBuffer[0]==0){
						fileBuffer[0]='A';
					}
					unsigned char i=33;
					while(i){
						if(fileBuffer[i]!=0){ break;}
						extSelected[2]=i-1;
						i--;
					}
					//if(extSelected[2]>i-1) extSelected[2]=i-1;
					keyboard=0;
					remenu();
					menuDraw();
				}
				if(pad.Buttons & PSP_CTRL_SELECT){
				if(pad.Buttons & PSP_CTRL_RIGHT) BIGSMALL=~BIGSMALL;
				if(pad.Buttons & PSP_CTRL_UP) FAKEHYOGEN=~FAKEHYOGEN;
					menuDraw();
					sceKernelDelayThread(300000);
				}
			if((pad.Buttons & PSP_CTRL_TRIANGLE) && extSelected[0]>1) { copyMenu=1; menuDraw(); sceKernelDelayThread(150000);}
				
			  if(pad.Buttons & SWAPBUTTON)
			  {
				if(extSelected[0] == 0)
				{
					extSelected[3]=!extSelected[3];
				}
				else if(extSelected[0] == 1)
				{
				  //Move the cursor back
				  extSelected[0]=0;
				  
				  //Setup the variables
				  searchResultCounter=0;
				  
				  //Search!
				  counter=searchStart;
				  //char acounter=0;
				  //Helper
				  while(counter < searchStop)
				  {
				if((counter>0x48802800) && (counter<0x48803C00)){
				counter=0x48803C00;}
					//Helper
					if(!((counter - searchStart) & 0xFFFF))
					{
					  if(!cheatPause) sceKernelDelayThread(1500);
						  
					  lineClear(33);
					  pspDebugScreenSetTextColor(color02); sprintf(buffer, "Task = %02d%%; () = Hold to Abort", (counter - searchStart + 1)/((searchStop-searchStart + 0x64)/0x64)); pspDebugScreenPuts(buffer); 
						
					  sceCtrlPeekBufferPositive(&pad, 1);
					  
					  if(pad.Buttons & SWAPBUTTON2)
					  {
						lineClear(33);
						pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("Task Aborted!!!");

						do
						{
						  sceKernelDelayThread(50000);
							sceCtrlPeekBufferPositive(&pad, 1);
						}while(pad.Buttons & SWAPBUTTON2);
						break;
					  }
					}
					
					//Check
					scounter=0;
					while(scounter < 33)
					{
					  if(counter+scounter <= 0x49FFFFFF)
					  {
						unsigned char tempLetter=*((unsigned char*)(counter+scounter));
						//big small chara
						if(BIGSMALL==0 && (tempLetter >= 0x61) && (tempLetter <= 0x7A)) tempLetter-=0x20;
						
						// . wildcard
						if(FAKEHYOGEN==0 && *((unsigned char*)(&fileBuffer[0]+scounter))==0x2E){
						scounter++;
						}
						// /. nowildcard
						//else if(FAKEHYOGEN==0 && (*((unsigned char*)(&fileBuffer[0]+scounter))==0x5C && *((unsigned char*)(&fileBuffer[0]+scounter+1))==0x2E)&& tempLetter==0x2E){
						//scounter+=2;
						//}
						// c?
						//else if(FAKEHYOGEN==0 && *((unsigned char*)(&fileBuffer[0]+scounter+1))==0x3F){
						//if(tempLetter==*((unsigned char*)(&fileBuffer[0]+scounter))){
						//acounter=-1;
						//scounter+=2;
						//}
						//else{
						//acounter=-2;
						//scounter+=2;
						//}
						//}
						 else if(tempLetter == (unsigned char)fileBuffer[scounter])
						{
						  scounter++;
						  if(!fileBuffer[scounter])
						  {
							//Add it
							searchAddress[searchResultCounter]=counter;
							searchResultCounter++;
							break;
						  }
						}
						else
						{
						  //acounter=0;
						  if(scounter == 0)
						  {
							scounter=1;
						  }
						  break;
						}
						}
					  else
					  {
						scounter++;
						break;
					  }
					}
					
					counter+=scounter;
					
					if(searchResultCounter == searchResultMax) break;
				  }
				}

				pspDebugScreenInitEx(vram, 0, 0);
				menuDraw();
				sceKernelDelayThread(150000);
			  }
			
				if(extSelected[3])
			  {
				if(pad.Buttons & PSP_CTRL_UP)
				{
				  fileBuffer[extSelected[2]]++;
				  switch(fileBuffer[extSelected[2]])
				  {
					case 0x01: fileBuffer[extSelected[2]]=0x20; break;
					case 0x21: fileBuffer[extSelected[2]]=0x41; break;
					case 0x5B: fileBuffer[extSelected[2]]=0x30; break;
					case 0x3A: fileBuffer[extSelected[2]]=0x21; break;
					case 0x30: fileBuffer[extSelected[2]]=0x3A; break;
					case 0x41: fileBuffer[extSelected[2]]=0x5B; break;
					case 0x61: fileBuffer[extSelected[2]]=0x7B; break;


					case 0x7F: fileBuffer[extSelected[2]]=0x20; break;
				  }
				  
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
								fileBuffer[extSelected[2]]--;
				  switch(fileBuffer[extSelected[2]])
				  {
					case 0xFF: fileBuffer[extSelected[2]]=0x7E; break;
					case 0x7A: fileBuffer[extSelected[2]]=0x60; break;
					case 0x5A: fileBuffer[extSelected[2]]=0x40; break;
					case 0x39: fileBuffer[extSelected[2]]=0x2F; break;
					case 0x20: fileBuffer[extSelected[2]]=0x39; break;
					case 0x2F: fileBuffer[extSelected[2]]=0x5A; break;
					case 0x40: fileBuffer[extSelected[2]]=0x20; break;
					case 0x1F: fileBuffer[extSelected[2]]=0x7E; break;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else
				{
				  cheatButtonAgeY=0;
				}
			  }
			  else
			  {
				if(pad.Buttons & PSP_CTRL_UP)
				{
				  if(extSelected[0] > 0)
				  {
					extSelected[0]--;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
				  if(extSelected[0] < (1+(searchResultCounter>searchResultMax? searchResultMax:searchResultCounter)))
				  {
					extSelected[0]++;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				if(pad.Buttons & PSP_CTRL_LTRIGGER)
				{
					
					if(extSelected[0]<2){
					}
				  else if(extSelected[0] > 22)
				  {
					extSelected[0]=extSelected[0]-21;
				  }
				  else{
				  extSelected[0]=2;}
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				
				}
				else if(pad.Buttons & PSP_CTRL_RTRIGGER)
				{
				  if(extSelected[0]<2){
				  }
				  else if(extSelected[0]+22 < (searchResultCounter>searchResultMax? searchResultMax:searchResultCounter))
				  {
					extSelected[0]=extSelected[0]+21;
				  }
				  else{
				  	if(searchResultCounter>searchResultMax){
				  	extSelected[0]=searchResultMax+1;}
				  	else{
				  	extSelected[0]=searchResultCounter+1;}
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else
				{
				  cheatButtonAgeY=0;
				}
			  }
			  
			  if(extSelected[0] == 0)
			  {
				if(pad.Buttons & PSP_CTRL_SQUARE) { memset(&fileBuffer[extSelected[2]+1], 0, 49-extSelected[2]);menuDraw();}
				
				if(pad.Buttons & PSP_CTRL_LEFT)
				{
				  extSelected[2]--;
					if((signed)extSelected[2] == -1) { extSelected[2]=0; }
				  menuDraw();
				  if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else if(pad.Buttons & PSP_CTRL_RIGHT)
				{
				  extSelected[2]++;
				  if(extSelected[2] == 33) { extSelected[2]=32; }
				  if(fileBuffer[extSelected[2]] == 0x00) fileBuffer[extSelected[2]]='A';
					menuDraw();
					if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else
				{
				  cheatButtonAgeX=0;
				}
			  }
			  else if(extSelected[0] >= 2)
			  {
				if(pad.Buttons & PSP_CTRL_LEFT)
				{
				  searchAddress[extSelected[0]-2]--;
				  if(searchAddress[extSelected[0]-2] < searchStart)
				  {
					searchAddress[extSelected[0]-2]=searchStart;
				  }

				  menuDraw();
				  if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else if(pad.Buttons & PSP_CTRL_RIGHT)
				{
				  searchAddress[extSelected[0]-2]++;
				  if(searchAddress[extSelected[0]-2] >= searchStop)
				  {
					searchAddress[extSelected[0]-2]=searchStop;
				  }
				  
					menuDraw();
					if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else
				{
				  cheatButtonAgeX=0;
				}
			  }
			  
			  if((padButtons & SWAPBUTTON2) && !(pad.Buttons & SWAPBUTTON2))
			  {
						
						if(extSelected[3])
						{
							//sceIoRemove("ms0:/search.ram");
						/*sprintf(buffer, "ms0:/search.ram");
							sceIoRemove(buffer);

							while(searchMax > 0)
							{
							sprintf(buffer, "ms0:/search%d.dat", searchMax);
							sceIoRemove(buffer);
							searchMax--;
							}
							
							//Reset fields
							//searchHistory[0].flags=0;
							searchNo=0;
							searchHistoryCounter=0;
							cheatSearch=0;
							cheatSelected=0;
							searchResultCounter=0;
							searchHistoryCounter=0;*/
							
							extSelected[3]=0;
							menuDraw();
							sceKernelDelayThread(150000);
						}
						else //Search has been aborted!
						{
							
							//sceIoRemove("ms0:/search.ram");
						/*	sprintf(buffer, "ms0:/search.ram");
					 		sceIoRemove(buffer);

							while(searchMax > 0)
							{
							sprintf(buffer, "ms0:/search%d.dat", searchMax);
							sceIoRemove(buffer);
							searchMax--;
							}
							
							//Reset fields
							//searchHistory[0].flags=0;
							searchHistoryCounter=0;
							searchNo=0;
							cheatSearch=0;
							cheatSelected=0;
							searchHistoryCounter=0;
							searchResultCounter=0;
							
							//Go back
							//cheatSelected=0;*/
							pspDebugScreenInitEx(vram, 0, 0);
							extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=0;
							extMenu=0;
							extOpt=0;
							menuDraw();
							sceKernelDelayThread(150000);
						}
					
				
			  }
			break;
				
			case 5: //search range features
				if(pad.Buttons & PSP_CTRL_TRIANGLE) { copyMenu=1; menuDraw(); sceKernelDelayThread(150000);}
			  if(pad.Buttons & SWAPBUTTON)
			  {
				if(extSelected[0] == 0)
				{
					extSelected[3]=!extSelected[3];
				}

				pspDebugScreenInitEx(vram, 0, 0);
				menuDraw();
				sceKernelDelayThread(150000);
			  }
			
			  if(extSelected[3])
			  {
				if(pad.Buttons & PSP_CTRL_UP)
				{
				  switch(extSelected[1])
				  {
						case 0: 
								if(extSelected[2]==7){
									searchStart+=4;
								}
								else{
									searchStart+=(1 << (4*(7-extSelected[2])));
								}
								if(searchStart < 0x48800000)
								{
									searchStart=0x48800000;
								}
								if(searchStart > 0x49FFFFFF)
								{
									searchStart=0x49FFFFFC;
								}
						break;
						case 1:
								if(extSelected[2]==7){
									searchStop+=4;
								}
								else{
									searchStop+=(1 << (4*(7-extSelected[2])));
								}
								if(searchStop < 0x48800000)
								{
									searchStop=0x48800000;
								}
								if(searchStop > 0x49FFFFFF)
								{
									searchStop=0x49FFFFFC;
								}
						break;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
				  switch(extSelected[1])
				  {
						case 0: 
								if(extSelected[2]==7){
									searchStart-=4;
								}
								else{
									searchStart-=(1 << (4*(7-extSelected[2])));
								}
								if(searchStart < 0x48800000)
								{
									searchStart=0x48800000;
								}
								if(searchStart > 0x49FFFFFF)
								{
									searchStart=0x49FFFFFC;
								}
						break;
						case 1:
								if(extSelected[2]==7){
									searchStop-=4;
								}
								else{
									searchStop-=(1 << (4*(7-extSelected[2])));
								}
								if(searchStop < 0x48800000)
								{
									searchStop=0x48800000;
								}
								if(searchStop > 0x49FFFFFF)
								{
									searchStop=0x49FFFFFC;
								}
						break;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else
				{
				  cheatButtonAgeY=0;
				}
			  }
			  
			  if(extSelected[0] == 0)
			  {
				if(pad.Buttons & PSP_CTRL_LEFT)
				{
				  extSelected[2]--;
					switch(extSelected[1])
				  {
						case 0: if((signed)extSelected[2] == -1) { extSelected[2]=0; } break;
						case 1: if((signed)extSelected[2] == -1) { extSelected[2]=7; extSelected[1]--; } break;
				  }
				  menuDraw();
				  if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else if(pad.Buttons & PSP_CTRL_RIGHT)
				{
				  extSelected[2]++;
					switch(extSelected[1])
				  {
						case 0: if(extSelected[2] > 7) { extSelected[2]=0; extSelected[1]++; } break;
						case 1: if(extSelected[2] > 7) { extSelected[2]=7;} break;
				  }
					menuDraw();
					if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else
				{
				  cheatButtonAgeX=0;
				}
			  }
			  
				if((padButtons & SWAPBUTTON2) && !(pad.Buttons & SWAPBUTTON2))
				{
				if(extSelected[3])
				{
				  extSelected[3]=0;
					menuDraw();
					sceKernelDelayThread(150000);
				}
				else //Search has been aborted!
				{
				  //Do we need to act like the search didn't even happen???
				  if(searchNo == 0)
				  {
					cheatSearch=0;
				  }
				  
				  //Go back
				  cheatSelected=0;
					pspDebugScreenInitEx(vram, 0, 0);
					extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=0;
						extMenu=0;
				  extOpt=0;
					menuDraw();
					sceKernelDelayThread(150000);
					break;
					}
			  }
			 
			 break;
		  }
		remenu();
		}
		else{ //draw screens
		remenu();
		  //Overall button inputs
		  if((pad.Buttons & PSP_CTRL_LTRIGGER) && (tabSelected != 0)){
			pspDebugScreenInitEx(vram, 0, 0);
			cheatSelected=0;
			extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=0;
			extOpt=0;
			if(tabSelected > 0)
			{
				tabSelected--;
			}
			menuDraw();
			sceKernelDelayThread(150000);
		  }
		  if((pad.Buttons & PSP_CTRL_RTRIGGER) && (tabSelected != 4)){
			pspDebugScreenInitEx(vram, 0, 0);
			cheatSelected=0;
			extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=0;
			extOpt=0;
			if(tabSelected < 4)
			{
				tabSelected++;
			}
			menuDraw();
			sceKernelDelayThread(150000);
		  }
		  if(((padButtons & SWAPBUTTON2) && !(pad.Buttons & SWAPBUTTON2)) || ((pad.Buttons & PSP_CTRL_HOME)&&!(pad.Buttons & PSP_CTRL_HOME)) || suspend==1){
			//Special case for the memory viewer
			if(extSelected[3])
			{
				extSelected[3]=0;
				menuDraw();
				sceKernelDelayThread(150000);
			}
			else
			{
				if(keyboard && ((pad.Buttons & PSP_CTRL_HOME) || suspend==1)){
					if(extMenu==3){pspDebugKbInit(fileBuffer);}else{
				pspDebugKbInit(cheat[cheatSelected].name);}
				keyboard=0;}
				if(cheatPause && (pad.Buttons & PSP_CTRL_HOME))   cheatPause=!cheatPause;gameResume(thid);
			  //Unregister the O key so that the user mode game doesn't pick it up
				menuDrawn=0;
				#ifdef _SWAPBUTTON_
				if(tabSelected==2&&cheatSelected==6){
				}
				else{
				#endif
				return;
				#ifdef _SWAPBUTTON_
					}
				#endif
				
			}
		  }

		  //Choose the appropriate action based on the tabSelected
		  switch(tabSelected){
			case 0: //INPUT CHEATER
				//nofx's analog shit
				if(pad.Ly < 50){
					if(cheatSelected > 0){
						cheatSelected-=1;
					}
					else if(cheatSelected == 0){
						cheatSelected=cheatTotal-1;
					}
					menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(15000-(1200*cheatButtonAgeY));
				}
				else if(pad.Ly > 200){
					if(cheatSelected < (cheatTotal-1)){
						cheatSelected+=1;
					}
					else if(cheatSelected == (cheatTotal-1)){
						cheatSelected=0;
					}
					menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(15000-(1200*cheatButtonAgeY));
				}
				else if(pad.Lx < 50){
					if(cheatSelected > 0){
						cheatSelected-=1;
					}
					else if(cheatSelected == 0){
						cheatSelected=cheatTotal-1;
					}
					menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(15000-(1200*cheatButtonAgeY));
				}
				else if(pad.Lx > 200){
					if(cheatSelected < (cheatTotal-1)){
						cheatSelected+=1;
					}
					else if(cheatSelected == (cheatTotal-1)){
						cheatSelected=0;
					}
					menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(15000-(1200*cheatButtonAgeY));
				}
				
				if(pad.Buttons & PSP_CTRL_START){ //edit cheat name
					keyboard=1;
					pspDebugKbInit(cheat[cheatSelected].name);
					sceKernelDelayThread(150000);keyboard=0;
				}
				if(pad.Buttons & PSP_CTRL_SELECT){ //copy cheat to new 
					cheatTotal+=1;
					cheat[cheatTotal-1].block=cheat[cheatSelected].block;
					cheat[cheatTotal-1].len=cheat[cheatSelected].len;
					cheat[cheatTotal-1].flags=cheat[cheatSelected].flags;
					strcpy(cheat[cheatTotal-1].name, cheat[cheatSelected].name);
					sceKernelDelayThread(150000);
				}
				
				//regular controls
				if((pad.Buttons & PSP_CTRL_UP) || (pad.Buttons & PSP_CTRL_LEFT)){
					if(cheatSelected > 0){
						cheatSelected--;
					}
					else if(cheatSelected == 0){
						cheatSelected=cheatTotal-1;
					}
					menuDraw();
					if( (pad.Buttons & 0xFFFF) == PSP_CTRL_UP){
					if(cheatButtonAgeY < 13){
					cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));}
					}
					else{
					if(cheatButtonAgeY < 12){
					cheatButtonAgeY++; sceKernelDelayThread(15000-(1200*cheatButtonAgeY));}					
					}
				}
				else if((pad.Buttons & PSP_CTRL_DOWN) ||(pad.Buttons & PSP_CTRL_RIGHT) ){
					if(cheatSelected < (cheatTotal-1)){
						cheatSelected++;
					}
					else if(cheatSelected == (cheatTotal-1)){
					cheatSelected=0;
					}
					menuDraw();
					if((pad.Buttons & 0xFFFF) == PSP_CTRL_DOWN){
						if(cheatButtonAgeY < 13){
						cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));}
					}	
					else{	
						if(cheatButtonAgeY < 12){
						cheatButtonAgeY++; sceKernelDelayThread(15000-(1200*cheatButtonAgeY));}
					}
				}
				else{
					cheatButtonAgeY=0;
				}
				if(pad.Buttons & SWAPBUTTON) {
					cheat[cheatSelected].flags=(cheat[cheatSelected].flags & (~FLAG_SELECTED)) | ((~cheat[cheatSelected].flags) & FLAG_SELECTED);
					cheat[cheatSelected].flags&=~FLAG_CONSTANT;
					cheat[cheatSelected].flags|=FLAG_FRESH;
					menuDraw();
					cheatApply(0);

					sceKernelDelayThread(150000);
				}
				else if(pad.Buttons & PSP_CTRL_SQUARE){
					cheat[cheatSelected].flags=(cheat[cheatSelected].flags & ~FLAG_CONSTANT) | (~cheat[cheatSelected].flags & FLAG_CONSTANT);
					cheat[cheatSelected].flags&=~FLAG_SELECTED;
					cheat[cheatSelected].flags|=FLAG_FRESH;

					menuDraw();
					cheatApply(0);

					sceKernelDelayThread(150000);
				}
				else if(pad.Buttons & PSP_CTRL_TRIANGLE){
					pspDebugScreenInitEx(vram, 0, 0);
					extSelected[1]=extSelected[2]=extSelected[3]=0;
					extSelected[0]=cheat[cheatSelected].block;
					extMenu=1;
					extOpt=0;
					menuDraw();

					sceKernelDelayThread(150000);
				}
			break;
			  
			case 1: //INPUT SEARCHER
			  if(pad.Buttons & PSP_CTRL_UP){
				if(cheatSelected > 0)

				{
					cheatSelected--;
				}
				else if(cheatSelected == 0)
				{
					cheatSelected=(2 + ((!cheatSearch)*4));
				}
				menuDraw();
				if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
			  }
			  else if(pad.Buttons & PSP_CTRL_DOWN){
				if(cheatSelected < (2 + ((!cheatSearch)*4))){
					cheatSelected++;
				}
				else if(cheatSelected == (2 + ((!cheatSearch)*4))){
					cheatSelected=0;
				}
				menuDraw();
				if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
			  }
			  else{
				cheatButtonAgeY=0;
			  }
			  if(pad.Buttons & SWAPBUTTON){
				if(!cheatSearch){
				  if(cheatSelected == 0){ //find exact
					//Goto Find exact
					pspDebugScreenInitEx(vram, 0, 0);
					extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=0;
					extMenu=2;
					extOpt=0;
					menuDraw();
					cheatSearch=1;
				  }
				  else if(cheatSelected == 4){ //find text
					//Goto Find text
					//searchResultCounter=0;
					pspDebugScreenInitEx(vram, 0, 0);
					if(fileBuffer[0]==0) {
					memset(fileBuffer, 0, 33);
					fileBuffer[0]='A';}
					extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=0;
					extMenu=4;
					extOpt=0;
					menuDraw();
				  }
				  else if(cheatSelected == 5){ //set your search range
					//Goto search range
					pspDebugScreenInitEx(vram, 0, 0);
					extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=extSelected[4]=0;
					extMenu=5;
					extOpt=0;
					menuDraw();
				  }
				  else if(cheatSelected == 6){ //reset search
					//sceIoRemove("ms0:/search.ram");
					sprintf(buffer, "ms0:/search.ram");
					sceIoRemove(buffer);

					while(searchMax > 0)
					{
					  sprintf(buffer, "ms0:/search%d.dat", searchMax);
					  sceIoRemove(buffer);
					  searchMax--;
					}
					
					//Reset fields
					//searchHistory[0].flags=0;
					searchNo=0;
					cheatSearch=0;
					cheatSelected=0;
					searchHistoryCounter=0;
					searchResultCounter=0;
					counter=0;
				  }
				  else{
					//Dump a search dump
					fd=sceIoOpen("ms0:/search.ram", PSP_O_WRONLY | PSP_O_CREAT, 0777);
					if(fd>0){
					  if(sceIoWrite(fd, (void*)0x08800000, 0x1800000) == 0x1800000){
						sceIoClose(fd);
						
						cheatSearch=1;
						switch(cheatSelected){
						  case 1: searchHistory[0].flags=(searchHistory[0].flags & (~FLAG_DWORD)) | FLAG_BYTE; break;
						  case 2: searchHistory[0].flags=(searchHistory[0].flags & (~FLAG_DWORD)) | FLAG_WORD; break;
						  case 3: searchHistory[0].flags=(searchHistory[0].flags & (~FLAG_DWORD)) | FLAG_DWORD; break;
						}
						
						pspDebugScreenInitEx(vram, 0, 0);
						tabSelected=1;
						cheatSelected=1;
						menuDraw();
						lineClear(33);
						pspDebugScreenSetTextColor(color02); pspDebugScreenPuts("Now, resume the game!"); 
						
						sceKernelDelayThread(3000000);
					  }
					  else{
						sceIoClose(fd);
						
						//sceIoRemove("ms0:/search.ram");
						sprintf(buffer, "ms0:/search.ram");
						sceIoRemove(buffer);
						
						lineClear(33);
						pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("ERROR: MemoryStick out of Space!"); 
						
						sceKernelDelayThread(3000000);
					  }
					}
				  }
				}
				else{
				  if(cheatSelected == 0){
					//Goto Find exact
					pspDebugScreenInitEx(vram, 0, 0);
					extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=0;
					extMenu=2;
					extOpt=0;
					menuDraw();
				  }
				  else if(cheatSelected == 1){
					//Goto Find Diff
					pspDebugScreenInitEx(vram, 0, 0);
					extSelected[0]=extSelected[1]=extSelected[2]=extSelected[3]=0;
					extMenu=3;
					extOpt=0;
					menuDraw();
				  }
				  else if(cheatSelected == 2){
					
					//sceIoRemove("ms0:/search.ram");
					  sprintf(buffer, "ms0:/search.ram");
					  sceIoRemove(buffer);
					
					while(searchMax > 0)
					{
					  sprintf(buffer, "ms0:/search%d.dat", searchMax);
					  sceIoRemove(buffer);
					  searchMax--;
					}
					
					//Reset fields
					//searchHistory[0].flags=0;
					searchNo=0;
					cheatSearch=0;
					cheatSelected=0;
					searchHistoryCounter=0;
					searchResultCounter=0;
					counter=0;
				  }
				}
				menuDraw();
				sceKernelDelayThread(150000);
			  }
			break;
			  
			case 2: //INPUT PRX
			  if(pad.Buttons & PSP_CTRL_UP){
				if(cheatSelected > 0){
					cheatSelected--;
				}
				else if(cheatSelected == 0){
					#ifdef _SCREENSHOT_
					cheatSelected=14;
					#else					
					cheatSelected=13;
					#endif
				}
				menuDraw();
				if(cheatButtonAgeY < 11) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
			  }
			  else if(pad.Buttons & PSP_CTRL_DOWN){
					#ifdef _SCREENSHOT_
				if(cheatSelected < 14){
					cheatSelected++;
				}
				else if(cheatSelected == 14){
					cheatSelected=0;
				}
					#else					
				if(cheatSelected < 13){
					cheatSelected++;
				}
				else if(cheatSelected == 13){
					cheatSelected=0;
				}
					#endif
				menuDraw();
				if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
			  }
			  else{
				cheatButtonAgeY=0;
			  }
			  if(pad.Buttons & PSP_CTRL_LEFT){
				if((cheatSelected == 1) && (cheatLength > 1)){
					cheatLength-=1;
				}
				if((cheatSelected == 2) && (dumpNo > 0)){
				  dumpNo--;
				}
				if((cheatSelected == 3) && (dumpNo > 0)){
				  dumpNo--;
				}
				if((cheatSelected == 9) && (cheatHz > 0)){
				  cheatHz-=15625;
				}
				if((cheatSelected == 12) && (colorFile > 0)){
				  colorFile--;
				}
				#ifdef _SCREENSHOT_
				if(cheatSelected==14){
					if(screenshot_mode > 0){
					screenshot_mode--;}
				}
				#endif
				menuDraw();
				if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
			  }
			  else if(pad.Buttons & PSP_CTRL_RIGHT){
				if(cheatSelected==1){
					cheatLength+=1;
				}
				if(cheatSelected==2){
					dumpNo++;
				}
				if(cheatSelected==3){
					dumpNo++;
				}
				if(cheatSelected==9){
					cheatHz+=15625;
				}
				if(cheatSelected==12){
					colorFile++;
				}
				#ifdef _SCREENSHOT_
				if(cheatSelected==14){
					if(screenshot_mode  < 1){
					screenshot_mode++;}
				}
				#endif
				menuDraw();
				if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
			  }
			  else{
				cheatButtonAgeX=0;
			  }
			  if(pad.Buttons & SWAPBUTTON){
				if(cheatSelected == 0){
				  cheatPause=!cheatPause;
				  menuDraw();
				  
				  if(cheatPause)
				  {
					gamePause(thid);
				  }
				  else
				  {
					gameResume(thid);
				  }
				}
				else if(cheatSelected == 1){
				  cheatNew(4, 0, 0, cheatLength, 0);
				  
				  //Switch to the cheat editor
				  pspDebugScreenInitEx(vram, 0, 0);
				  extSelected[1]=extSelected[2]=extSelected[3]=0;
				  extSelected[0]=cheat[cheatTotal - 1].block;
				  cheatSelected=cheatTotal - 1;
				  extMenu=1;
				  extOpt=-1;
				  menuDraw();
				  sceKernelDelayThread(150000);
				}
				else if(cheatSelected == 2){
				  sprintf(buffer, "ms0:/dump%d.ram", dumpNo);
				  
				  fd=sceIoOpen(buffer, PSP_O_RDONLY, 0777);
				  if(fd>0)
				  {
					counter=0;
					  while(counter < cheatTotal)
					  {	
						scounter=cheat[counter].block;
					  cheatDMA=0;
					  while(scounter < cheat[counter].block + cheat[counter].len)
					  {
						if(block[scounter].flags & FLAG_DMA)
						{
						  if(block[scounter].hakVal!=0xFFFFFFFF)

						  {
							sceIoLseek(fd, block[scounter].hakVal, SEEK_SET);
							sceIoRead(fd, &cheatDMA, sizeof(unsigned int));
							block[scounter].stdVal=cheatDMA;
							}
						  else
						  {
							cheatDMA=0;
						  }
						}
						else
						{
						  sceIoLseek(fd, (cheatDMA+block[scounter].address)-0x08800000, SEEK_SET);
						  switch(block[scounter].flags & FLAG_DWORD)
							  {
								case FLAG_DWORD:
							  sceIoRead(fd, &block[scounter].stdVal, sizeof(unsigned int));
								break;
							case FLAG_WORD:
								sceIoRead(fd, &block[scounter].stdVal, sizeof(unsigned short));
								break;
							case FLAG_BYTE:
								sceIoRead(fd, &block[scounter].stdVal, sizeof(unsigned char));
								break;
						  }
						}
						
						scounter++;
					  }
					  
					  cheatDisable(counter);
					  counter++;
					}
					sceIoClose(fd);
					
					cheatStatus=0;
				  }
				  else
				  {
					lineClear(33);
							pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("ERROR: Selected RAM Dump # does not exist!"); 
					sceKernelDelayThread(3000000);
				  }
					
				  menuDraw();
				}
				else if(cheatSelected == 3){
				  sprintf(buffer, "ms0:/dump%d.ram", dumpNo);
				  fd=sceIoOpen(buffer, PSP_O_WRONLY | PSP_O_CREAT, 0777);
				  if(fd>0){
					if(sceIoWrite(fd, (void*)0x08800000, 0x1800000) == 0x1800000){
					  sceIoClose(fd);
					  dumpNo++;
					}
					else{
					  sceIoClose(fd);
					  sceIoRemove(buffer);
					  lineClear(33);
					  pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("ERROR: MemoryStick out of Space!"); 
					  sceKernelDelayThread(3000000);
					}
				  }
				  menuDraw();
				  sceKernelDelayThread(150000); //Delay twice
				}
				else if(cheatSelected == 4){
					dump_memregion("ms0:/boot.bin", (void*) 0xBFC00000, 0x100000);
					//dump_memregion("ms0:/scratch.bin", (void*) 0x00010000, 0x3FFF);
					dump_memregion("ms0:/kmem.bin", (void*) 0x88000000, 0x400000);
				}
				else if(cheatSelected == 5){
				  if(browseLines == 8)
				  {
					browseLines=16;
				  }
				  else
				  {
					browseLines=8;
					}
				  browseC[bdNo]=0;
				  browseX[bdNo]=0;
				  browseY[bdNo]=0;
				  if(browseAddress[bdNo] < 0x48800000)
					{
						browseAddress[bdNo]=0x48800000;
					}
				  if(browseAddress[bdNo] > (0x4A000000-(26*browseLines)))
					{
						browseAddress[bdNo]=(0x4A000000-(26*browseLines));
					}
				  menuDraw();
				}
				else if(cheatSelected == 6){//swap X/O
				
				#ifdef _SWAPBUTTON_
				  if(SWAPBUTTON == 0x2000)
				  { 
					SWAPBUTTON=0x4000;
					SWAPBUTTON2=0x2000;
					SWAPBUTTON3=0x8000;
				  }
				  else
				  {
					SWAPBUTTON=0x2000;
					SWAPBUTTON2=0x4000;
					SWAPBUTTON3=0x1000;
				 }
				  menuDraw();
				 #endif
				}
				else if(cheatSelected == 7){//switch realaddress
				  if(decodeFormat == 0x48800000)
				  {
					decodeFormat=0x40000000;
				  }
				  else
				  {
					decodeFormat=0x48800000;
					}
				  menuDraw();
				}
				else if(cheatSelected == 8){
					#ifdef _USB_
					usbonbitch=!usbonbitch;
					menuDraw();
					sceKernelDelayThread(150000);
					if(usbonbitch){
						if(usbmod==0){ InitUsbStorage(); usbmod=1; }
						StartUsbStorage();
						lineClear(32); pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("USB Is On");
					}
					else{
						StopUsbStorage();
						lineClear(32); pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("USB Is Off");
					}
					sceKernelDelayThread(150000);
					#else
					if(pad.Buttons & SWAPBUTTON3){
					scePowerRequestStandby();}
					else{
					if(cheatPause) {cheatPause=!cheatPause;gameResume(thid);menuDrawn=0;}
					suspend=1;
					}
					#endif
				}
				else if(cheatSelected == 10){
					cheatSave();
					lineClear(32); pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("Cheats Saved");
					menuDraw();
					sceKernelDelayThread(150000); //Delay twice
				}
				else if(cheatSelected == 11){
					cheatRefresh=1;
					#ifdef _CFW_
					GETID();
					#endif
					cheatLoad();
					lineClear(32); pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("Cheats Refreshed");
					menuDraw();
					sceKernelDelayThread(150000); //Delay twice
				}
				else if(cheatSelected == 12){
					sprintf(buffer, "ms0:/seplugins/nitePR/MKIJIRO/color%d.txt", colorFile);
					if(pad.Buttons & SWAPBUTTON3){
					memcpy(&buffer[30],"ijiro.bin\x0",10);
					setting=(cheatPause<<24)+(((browseLines&0x8)>>3)<<16)+((SWAPBUTTON2/0x4000)<<8)+((((decodeFormat&0x8800000)>>24)+1)&0x1);
					memcpy(SAVETEMP,&setting,4);
					memcpy(SAVETEMP+4,&cheatHz,4);
					int fd=sceIoOpen(buffer, PSP_O_WRONLY | PSP_O_CREAT, 0777);
					sceIoWrite(fd,(void*)SAVETEMP,8);
					sceIoClose(fd);
					}
					else{
					colorAdd(buffer);
					}
					lineClear(32); pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("Colors Refreshed");
					menuDraw();
					sceKernelDelayThread(150000); //Delay twice
				}
				else if(cheatSelected == 13){
					#ifdef _PSID_
					corruptPsid(psid);
					lineClear(32); pspDebugScreenSetTextColor(color01); pspDebugScreenPuts("PSID Corrupted");
					menuDraw();
					sceKernelDelayThread(150000); //Delay twice
					#endif
				}
				
				sceKernelDelayThread(150000);
			  }
			  /* commented out for now pissed me off meh fuck off
			  if(pad.Buttons & PSP_CTRL_SQUARE){
				if(cheatSelected == 1){
					cheatLength+=1;
					cheatNew(4, 0x00000000, 0, cheatLength, 1);
					cheatLength-=1;
					//Switch to the cheat editor
					pspDebugScreenInitEx(vram, 0, 0);
					extSelected[1]=extSelected[2]=extSelected[3]=0;
					extSelected[0]=cheat[cheatTotal - 1].block;
					cheatSelected=cheatTotal - 1;
					extMenu=1;
					extOpt=1;
					menuDraw();
					sceKernelDelayThread(150000);
				}
				sceKernelDelayThread(150000);
			  }
			  if(pad.Buttons & PSP_CTRL_TRIANGLE){
				if(cheatSelected == 1){
					cheatLength+=1;
					cheatNew(4, 0x00000000, 0, cheatLength, 2);
					cheatLength-=1;
					//Switch to the cheat editor
					pspDebugScreenInitEx(vram, 0, 0);
					extSelected[1]=extSelected[2]=extSelected[3]=0;
					extSelected[0]=cheat[cheatTotal - 1].block;
					cheatSelected=cheatTotal - 1;
					extMenu=1;
					extOpt=1;
					menuDraw();
					sceKernelDelayThread(150000);
				}
				sceKernelDelayThread(150000);
			  }
			  */
			break;
			  
			case 3: //INPUT BROWSER & DECODER
			 if(flipme){//INPUT BROWSER
			  if(pad.Buttons & PSP_CTRL_SELECT) {
				  
				  if(browseLines == 8){
					browseLines=16;
					browseMAX();
				  }
				  else if(browseLines == 16){
					browseLines=8;
				  }
				  browseX[bdNo]=0;
				  menuDraw();
				 sceKernelDelayThread(150000);
				  
			  }
			  if(pad.Buttons & PSP_CTRL_TRIANGLE) { copyMenu=1; menuDraw(); sceKernelDelayThread(150000);}
			  if(pad.Buttons & SWAPBUTTON){
				extSelected[3]=!extSelected[3];
				menuDraw();
				sceKernelDelayThread(150000);
			  }
			  if(pad.Buttons & PSP_CTRL_LEFT){
				browseX[bdNo]--;
				switch(browseC[bdNo])
				{
				  case 0: if((signed)browseX[bdNo] == -1) { browseX[bdNo]=0; } break;
				  case 1: if((signed)browseX[bdNo] == -1) { browseX[bdNo]=7; browseC[bdNo]--; } break;
				  case 2: if((signed)browseX[bdNo] == -1) { browseX[bdNo]=(2*browseLines)-1; browseC[bdNo]--; } break;
				}
				menuDraw();
				if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
			  }
			  else if(pad.Buttons & PSP_CTRL_RIGHT){
				browseX[bdNo]++;
				switch(browseC[bdNo])
				{
				  case 0: if(browseX[bdNo] > 7) { browseX[bdNo]=0; browseC[bdNo]++; } break;
				  case 1: if(browseX[bdNo] > ((2*browseLines)-1)) { browseX[bdNo]=0; browseC[bdNo]++; } break;
				  case 2: if(browseX[bdNo] > (browseLines-1)) { browseX[bdNo]=browseLines-1; } break;
				}
				menuDraw();
				if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
			  }
			  else{
				cheatButtonAgeX=0;
			  }
			  if(extSelected[3]){
				if(pad.Buttons & PSP_CTRL_SQUARE)
				{
					if(browseC[bdNo]==1)
				  {
					browseC[bdNo]=2;
					browseX[bdNo]=browseX[bdNo]/2;
				  }
				  else if(browseC[bdNo]==2)
				  {
					browseC[bdNo]=1;
					browseX[bdNo]=2*browseX[bdNo]+1;
				  }
					menuDraw();
				  sceKernelDelayThread(150000);
				}
				if(pad.Buttons & PSP_CTRL_UP)
				{
				  switch(browseC[bdNo])
				  {
					case 0:
						browseAddress[bdNo]+=(1 << (4*(7-browseX[bdNo])));
						browseMAX();
					  break;
					case 1:
						if(browseX[bdNo] & 1)
					  {
							*((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+(browseX[bdNo]/2)))+=0x01;
						}
					  else
					  {
						*((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+(browseX[bdNo]/2)))+=0x10;
					  }
					  sceKernelDcacheWritebackInvalidateRange(((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+(browseX[bdNo]/2))),1);
										sceKernelIcacheInvalidateRange(((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+(browseX[bdNo]/2))),1);
					  break;
					case 2:
						*((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+browseX[bdNo]))+=1;
					  sceKernelDcacheWritebackInvalidateRange(((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+browseX[bdNo])),1);
										sceKernelIcacheInvalidateRange(((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+browseX[bdNo])),1);
					  break;
				  }
				  menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
				  switch(browseC[bdNo])
				  {
					case 0:
						browseAddress[bdNo]-=(1 << (4*(7-browseX[bdNo])));
						browseMAX();
					  break;
					case 1:
						if(browseX[bdNo] & 1)
					  {
							*((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+(browseX[bdNo]/2)))-=0x01;
						}
					  else
					  {
						*((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+(browseX[bdNo]/2)))-=0x10;
					  }
					  sceKernelDcacheWritebackInvalidateRange(((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+(browseX[bdNo]/2))),1);
										sceKernelIcacheInvalidateRange(((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+(browseX[bdNo]/2))),1);
					  break;
					case 2:
						*((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+browseX[bdNo]))-=1;
					  sceKernelDcacheWritebackInvalidateRange(((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+browseX[bdNo])),1);
										sceKernelIcacheInvalidateRange(((unsigned char*)((browseAddress[bdNo]+(browseY[bdNo]*browseLines))+browseX[bdNo])),1);
					  break;
				  }
					menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else
				{
					cheatButtonAgeY=0;
				}
			  }
			  else if(pad.Buttons & PSP_CTRL_SQUARE){
					if(pad.Buttons & PSP_CTRL_UP)
				{

					browseAddress[bdNo]-=browseLines;
					browseMAX();
					menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
				  browseAddress[bdNo]+=browseLines;
				  browseMAX();
					menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else
				{
				  cheatButtonAgeY=0;
				}
				if((abs((signed)(pad.Ly-127))  - 40) > 0)
				{
				  browseAddress[bdNo]+=(((signed)(pad.Ly-127))/browseLines) * 64;
				browseMAX();
				  menuDraw();
					sceKernelDelayThread(18750);
				}
				}
			  else{
				if(pad.Buttons & PSP_CTRL_UP)
				{
					if(browseY[bdNo] > 0)
					{
						browseY[bdNo]--;
					}
				  else if(browseY[bdNo] == 0)
				  {
					browseAddress[bdNo]-=browseLines;browseMAX();
				  }
					menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else if(pad.Buttons & PSP_CTRL_DOWN)
				{
					if(browseY[bdNo] < 25)
					{
						browseY[bdNo]++;
					}
				  else if(browseY[bdNo] == 25)
				  {
					browseAddress[bdNo]+=browseLines;browseMAX();
				  }
					menuDraw();
					if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
				else
				{
					cheatButtonAgeY=0;
				}
			  }
			 }
			else{ //INPUT DECODER
				if((padButtons & PSP_CTRL_SQUARE) && (padButtons & PSP_CTRL_RIGHT)){
					unsigned int foobar=*((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4)));
				  if(jumplog < logcounter){
					logcounter=jumplog;}
				  else{ 
				 //pointer jump
					if((foobar >= 0x08800000) && (foobar < 0x0A000000)){ //handle pointers

						if(((decodeAddress[bdNo]+(decodeY[bdNo]*4)) >= (logstart-4)) && ((decodeAddress[bdNo]+(decodeY[bdNo]*4)) <= (logstart + 4*jumplog))){
						logcounter=((decodeAddress[bdNo]+(decodeY[bdNo]*4))-0x48800000)>>2;
						}
						else{
						storedAddress[logcounter]=decodeAddress[bdNo]+(decodeY[bdNo]*4); //store pointer address
						foobar+=0x40000000;
						*((unsigned int*)(logstart+4*logcounter))=storedAddress[logcounter] & 0xFFFFFFC;
						logcounter++;
						}
						decodeAddress[bdNo]=(foobar | 0x40000000)&0xFFFFFFFC;
						decodeY[bdNo]=0;
					}//jal.j
					else if(((foobar >= 0x0A200000) && (foobar < 0x0A800000)) || ((foobar >= 0x0E200000) && (foobar < 0x0E800000))){ //handle hooks
						storedAddress[logcounter]=decodeAddress[bdNo]+(decodeY[bdNo]*4);
						foobar&=0x3FFFFFF;
						decodeAddress[bdNo]=(mipsNum, "%08X", ((foobar<<2)))-0xC0000000; //store pointer address
						decodeY[bdNo]=0;
						*((unsigned int*)(logstart+4*logcounter))=storedAddress[logcounter] & 0xFFFFFFF;
						logcounter++;
					}//branch jump
					else if(((foobar >= 0x10000000) && (foobar <= 0x1FFFFFFF)) || ((foobar >= 0x50000000) && (foobar <= 0x5FFFFFFF))
						|| ((foobar >= 0x45000000) && (foobar <= 0x4503FFFF)) || ((foobar >= 0x49000000) && (foobar <= 0x491FFFFF))
						|| (((foobar & 0xFC1F0000) >= 0x04000000) && ((foobar & 0xFC1F0000) <= 0x04030000))
						||  (((foobar & 0xFC1F0000) >= 0x04100000) && ((foobar & 0xFC1F0000) <= 0x04130000)) )
						{ //handle branches
						storedAddress[logcounter]=decodeAddress[bdNo]+(decodeY[bdNo]*4);
						foobar&=0xFFFF;
						if(foobar > 0x7FFF){
						addresstmp=-(mipsNum, "%04X", 4*(0x10000 - foobar))+decodeAddress[bdNo]+(decodeY[bdNo]*4)+4; //store pointer address
						}
						else{
						addresstmp=(mipsNum, "%04X", foobar*4)+decodeAddress[bdNo]+(decodeY[bdNo]*4)+4; //store pointer address
						}
						if((addresstmp >= 0x48800000) && (addresstmp < 0x4A000000)){
						decodeAddress[bdNo]=addresstmp;
						decodeY[bdNo]=0;
						}
						*((unsigned int*)(logstart+4*logcounter))=storedAddress[logcounter] & 0xFFFFFFF;
						logcounter++;
						decodeAddress[bdNo];
					}
					decodeMAX();
				   }
					menuDraw();
					sceKernelDelayThread(150000);
				}
				if((padButtons & PSP_CTRL_SQUARE) && (padButtons & PSP_CTRL_LEFT)){
					//return to pointer;
					decodeY[bdNo]=0;
					if(logcounter >= 1){
					decodeAddress[bdNo]=storedAddress[logcounter-1];
					logcounter--;}
					decodeMAX();
					menuDraw();
					sceKernelDelayThread(150000);
				}
				if((pad.Buttons & 0xFFFF) == 0x1){
					if(decodeOptions==0){
						decodeOptions=1;
					}
					else{
						decodeOptions=0;
					}
					menuDraw();
					sceKernelDelayThread(150000);
				}
				if(searchResultCounter>searchResultMax){
					searchjumpNoMax=searchResultMax;
				}
				else{
					searchjumpNoMax=searchResultCounter;
				}
				if((pad.Buttons & PSP_CTRL_SELECT) && (pad.Buttons & PSP_CTRL_RIGHT)){
				if(searchjumpNo<searchjumpNoMax-1){
					searchjumpNo++;
				}
				if(searchAddress[searchjumpNo]> 0x48800000){
				decodeY[bdNo]=0;
				decodeAddress[bdNo]=searchAddress[searchjumpNo] & 0xFFFFFFFC;
				sceKernelDelayThread(150000);
				}
				}
				if((pad.Buttons & PSP_CTRL_SELECT) && (pad.Buttons & PSP_CTRL_LEFT)){
				if(searchjumpNo>0){
				searchjumpNo--;
				}
				if(searchAddress[searchjumpNo]> 0x48800000){
				decodeY[bdNo]=0;
				decodeAddress[bdNo]=searchAddress[searchjumpNo] & 0xFFFFFFFC;
				sceKernelDelayThread(150000);
				}
				}
				if((pad.Buttons & PSP_CTRL_SELECT) && (pad.Buttons & PSP_CTRL_UP)){
				if(searchjumpNo<searchjumpNoMax-10){
					searchjumpNo+=10;
				}
				else{
					searchjumpNo=searchjumpNoMax;
				}
				if(searchAddress[searchjumpNo]> 0x48800000){
				decodeY[bdNo]=0;
				decodeAddress[bdNo]=searchAddress[searchjumpNo] & 0xFFFFFFFC;
				sceKernelDelayThread(150000);
				}
				}
				if((pad.Buttons & PSP_CTRL_SELECT) && (pad.Buttons & PSP_CTRL_DOWN)){
				if(searchjumpNo>9){
				searchjumpNo-=10;
				}
				else{
				searchjumpNo=0;
				}
				if(searchAddress[searchjumpNo]> 0x48800000){
				decodeY[bdNo]=0;
				decodeAddress[bdNo]=searchAddress[searchjumpNo] & 0xFFFFFFFC;
				sceKernelDelayThread(150000);
				}
				}				
				if(pad.Buttons & PSP_CTRL_TRIANGLE){ 
					copyMenu=1; 
					menuDraw(); 
					sceKernelDelayThread(150000);
				}
				if(pad.Buttons & SWAPBUTTON){
					extSelected[3]=!extSelected[3];
					menuDraw();
					sceKernelDelayThread(150000);
				}
				if(pad.Buttons & PSP_CTRL_NOTE){
					if(!copyToggle){
						copyStartFlag=decodeAddress[bdNo]+(decodeY[bdNo]*4);
						copyEndFlag=decodeAddress[bdNo]+(decodeY[bdNo]*4);
						copyToggle=1;
					}
					else if((copyToggle) && (decodeAddress[bdNo]+(decodeY[bdNo]*4) > copyStartFlag)){
						copyEndFlag=decodeAddress[bdNo]+(decodeY[bdNo]*4);
						copyToggle=0;
					}
					else if((copyToggle) && (decodeAddress[bdNo]+(decodeY[bdNo]*4) < copyStartFlag)){
						copyStartFlag=decodeAddress[bdNo]+(decodeY[bdNo]*4);
						copyToggle=1;
					}
					menuDraw();
					sceKernelDelayThread(150000);
				}
				if(pad.Buttons & PSP_CTRL_LEFT){
					decodeX[bdNo]--;
					switch(decodeC[bdNo]){
						case 0: if((signed)decodeX[bdNo] == -1) { decodeX[bdNo]=0; } break;
						case 1: if((signed)decodeX[bdNo] == -1) { decodeX[bdNo]=7; decodeC[bdNo]--; } break;
						case 2: if((signed)decodeX[bdNo] == -1) { decodeX[bdNo]=7; decodeC[bdNo]--; } break;
					}
					menuDraw();
					if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else if(pad.Buttons & PSP_CTRL_RIGHT){
					decodeX[bdNo]++;
					switch(decodeC[bdNo]){
						case 0: if(decodeX[bdNo] > 7) { decodeX[bdNo]=0; decodeC[bdNo]++; } break;
						case 1: if(decodeX[bdNo] > 7) { decodeX[bdNo]=7; } break;
						case 2: if(decodeX[bdNo] > 7) { decodeX[bdNo]=7; } break;
					}
					menuDraw();
					if(cheatButtonAgeX < 12) cheatButtonAgeX++; sceKernelDelayThread(150000-(10000*cheatButtonAgeX));
				}
				else{
						cheatButtonAgeX=0;
				}	
				if(extSelected[3]){
				if(pad.Buttons & PSP_CTRL_UP){
					switch(decodeC[bdNo]){
						case 0:
							if(decodeX[bdNo]==7){
								decodeAddress[bdNo]+=4;
							}
							else{
								decodeAddress[bdNo]+=(1 << (4*(7-decodeX[bdNo])));
							}
							if(decodeAddress[bdNo]>0x49FFFF98){
							decodeAddress[bdNo]=decodeAddress[bdNo]+decodeY[bdNo]*4;}
							decodeMAX();
						break;
						case 1:
							*((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4)))+=(1 << (4*(7-decodeX[bdNo])));
							sceKernelDcacheWritebackInvalidateRange(((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4))),4);
							sceKernelIcacheInvalidateRange(((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4))),4);
						break;
					}
							menuDraw();
							if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
				}
					else if(pad.Buttons & PSP_CTRL_DOWN){
						switch(decodeC[bdNo]){
							case 0:
								if(decodeX[bdNo]==7){
									decodeAddress[bdNo]-=4;
								}
								else{
									decodeAddress[bdNo]-=(1 << (4*(7-decodeX[bdNo])));
								}
								decodeMAX();
									break;
									case 1:
										*((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4)))-=(1 << (4*(7-decodeX[bdNo])));
										sceKernelDcacheWritebackInvalidateRange(((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4))),4);
										sceKernelIcacheInvalidateRange(((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4))),4);
									break;
						}
							menuDraw();
							if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
					}
					else{
					cheatButtonAgeY=0;
					}
				}
				else if(pad.Buttons & PSP_CTRL_SQUARE){
					if(pad.Buttons & PSP_CTRL_UP){
						if(decodeAddress[bdNo] > 0x48800000){
							decodeAddress[bdNo]-=4;
						}
						menuDraw();
						if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
					}
					else if(pad.Buttons & PSP_CTRL_DOWN){
						if(decodeAddress[bdNo] < 0x49FFFF98){
							decodeAddress[bdNo]+=4;decodeMAX();
						}
						menuDraw();
						if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
					}
					else{
						cheatButtonAgeY=0;
					}
					if((abs((signed)(pad.Ly-127))  - 40) > 0){
						decodeAddress[bdNo]+=(((signed)(pad.Ly-127))/browseLines) * browseLines;
						decodeMAX();
						menuDraw();
						sceKernelDelayThread(18750);
					}
				}
				else{
					if(pad.Buttons & PSP_CTRL_UP){
						if(decodeY[bdNo] > 0){
							decodeY[bdNo]--;
						}
						else if(decodeY[bdNo] == 0){
							if(decodeAddress[bdNo] > 0x48800000){
								decodeAddress[bdNo]-=4;
							}
						}
						menuDraw();
						if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
					}
					else if(pad.Buttons & PSP_CTRL_DOWN){
						if(decodeY[bdNo] < 25){
							decodeY[bdNo]++;
						}
						else if(decodeY[bdNo] == 25){
							if(decodeAddress[bdNo] < 0x49FFFF98){
									decodeAddress[bdNo]+=4;
							}
						}	
						menuDraw();
						if(cheatButtonAgeY < 12) cheatButtonAgeY++; sceKernelDelayThread(150000-(10000*cheatButtonAgeY));
					}
					else{
						cheatButtonAgeY=0;
					}
				}
			 }
			 if(pad.Buttons & PSP_CTRL_START){
				//change browser address format
				if(flipme==0){
					flipme=1;
				}
				else{
					flipme=0;
				}
				menuDraw();
				sceKernelDelayThread(150000);
			 }
			 else if(pad.Buttons & PSP_CTRL_VOLUP){
			 	if(bdNo < 4){
			 		bdNo++;
				}
				menuDraw();
				sceKernelDelayThread(150000);
			 }
			 else if(pad.Buttons & PSP_CTRL_VOLDOWN){
			 	if(bdNo > 0){
			 		bdNo--;
				}
				menuDraw();
				sceKernelDelayThread(150000);
			 }
			break;
			
			case 4: //OPTIONS MENU
				#ifdef _SERVER_
				server();
				#endif
			break;
			  
		  }
		}
		
		//Stop the game from freezing up
		if(!cheatPause) sceKernelDelayThread(15625);
	}
}

#ifdef _PSID_
Hook hookB[1]={
	{ { 0, NULL }, "sceOpenPSID_Service", "sceOpenPSID_driver", 0xc69bebce, NULL},
};

int sceOpenPSIDGetOpenPSID(char *openpsid){
	memcpy(openpsid, psid, 16);
	return 0;
}

static const unsigned char patchA[]={
	0x21, 0x88, 0x02, 0x3c, //lui v0, $8821
	0x21, 0x10, 0x42, 0x34, //ori v0, v0, $1021
	0x08, 0x00, 0x40, 0x00, //jr v0
	0x00, 0x00, 0x00, 0x00 //nop
}; 
#endif

#ifdef _SCREENSHOT_
	#include "headers/screenshot.h"
#endif

int mainThread(){
	int fd;
	running=1;
	unsigned int counter=0;

	//Clear the search history to 0
	memset(&searchHistory, 0, sizeof(Block) * 15);
	int i;
	for(i=0; i<15; i++){
		searchHistory[i].flags|=FLAG_DWORD;
		sprintf(buffer, "ms0:/search%d.dat", i); sceIoRemove(buffer);
	}
	
	/* 
		and he bitches at me about stupid code...
		searchHistory[0].flags|=FLAG_DWORD;
		searchHistory[1].flags|=FLAG_DWORD;
		searchHistory[2].flags|=FLAG_DWORD;
		searchHistory[3].flags|=FLAG_DWORD;
		searchHistory[4].flags|=FLAG_DWORD;
		searchHistory[5].flags|=FLAG_DWORD;
		searchHistory[6].flags|=FLAG_DWORD;
		searchHistory[7].flags|=FLAG_DWORD;
		searchHistory[8].flags|=FLAG_DWORD;
		searchHistory[9].flags|=FLAG_DWORD;
		searchHistory[10].flags|=FLAG_DWORD;
		searchHistory[11].flags|=FLAG_DWORD;
		searchHistory[12].flags|=FLAG_DWORD;
		searchHistory[13].flags|=FLAG_DWORD;
		searchHistory[14].flags|=FLAG_DWORD;
		searchHistory[15].flags|=FLAG_DWORD;
	*/

	//Wait for the kernel to boot
	sceKernelDelayThread(100000);
	while(!sceKernelFindModuleByName("sceKernelLibrary"))
	sceKernelDelayThread(100000);
	sceKernelDelayThread(100000);

	//Find which screen directory to use
	#ifdef _SCREENSHOT_
	fd=sceIoDopen("ms0:/PICTURE/");
	if(fd > 0){
		sceIoDclose(fd);
		strcpy(screenPath, "ms0:/PICTURE/screen%d.bmp");
	}
	else{
		fd=sceIoDopen("ms0:/PSP/PHOTO/");
		if(fd > 0){
			sceIoDclose(fd);
			strcpy(screenPath, "ms0:/PSP/PHOTO/screen%d.bmp");
		}
		else{
			if(!sceIoMkdir("ms0:/PICTURE", 0777)){
				strcpy(screenPath, "ms0:/PICTURE/screen%d.bmp");
			}
			else{
				screenPath[0]=0;
			}
		}
	}

	//Set the correct screen number
	screenNo=0;
	if(screenPath[0]!=0){
		while(1){
			sprintf(buffer, screenPath, screenNo);
			fd=sceIoOpen(buffer, PSP_O_RDONLY, 0777);
			if(fd > 0){
				sceIoClose(fd);
				screenNo++;
			}
			else{
				break;
			}
		}
	}
  	#endif

  	#ifdef _PSID_
	//OpenPSID
	//if(cfg[19])
	{
		//Generate the patch
		*((unsigned short*)(&patchA[0]))=(unsigned short)(((unsigned int)sceOpenPSIDGetOpenPSID)>>16);
		*((unsigned short*)(&patchA[4]))=(unsigned short)((unsigned int)&sceOpenPSIDGetOpenPSID);  

		//Find the function we want!
		while(1){
			int mod=sceKernelFindModuleByName(hookB[0].modname);
			if(mod == NULL) { sceKernelDelayThread(100); continue;}
			break;
		}
		unsigned int hookAddress=moduleFindFunc(moduleFindLibrary(sceKernelSearchModuleByName(hookB[0].modname), hookB[0].libname), hookB[0].nid);
		hookAddress=*((unsigned int*)hookAddress);

		//Open the PSID file
		fd=sceIoOpen("ms0:/seplugins/nitePR/nitePRimportant.bin", PSP_O_RDONLY, 0777);
		if(fd > 0){
			sceIoRead(fd, psid, 16);
			sceIoClose(fd);
		}
		else{
			fd=sceIoOpen("ms0:/seplugins/nitePR/nitePRimportant.bin", PSP_O_WRONLY | PSP_O_CREAT, 0777);
			if(fd > 0){
				((int(*)(char*))hookAddress)(psid);
				sceIoWrite(fd, psid, 16);
				sceIoClose(fd);
			}
			else{
				goto skipPatch;
			}
		}
		
		  //Apply the memcpy
		sceKernelDcacheWritebackAll();
		sceKernelIcacheInvalidateAll();
		
		//Apply the patch
		memcpy(hookAddress, patchA, 16);
		
		//Apply the memcpy
		sceKernelDcacheWritebackAll();
		sceKernelIcacheInvalidateAll();
	}	skipPatch: //Skip the evil patch
	#endif
	
	#ifdef _NOHB_
	 //Find the GAME ID
	  do
	  {
	  	fd=sceIoOpen("disc0:/UMD_DATA.BIN", PSP_O_RDONLY, 0777); 
    	sceKernelDelayThread(1000);
	  } while(fd<=0);
  sceIoRead(fd, gameId, 10);
  sceIoClose(fd);
  memcpy(&gameDir[22], gameId, 10);
	strcpy(buffer, ".txt\x0");
	memcpy(&gameDir[32], buffer, 5);
	cheatLoad();
	#endif
	
	//load the colors!
	sprintf(buffer,"ms0:/seplugins/nitePR/MKIJIRO/color0.txt");
	colorAdd(buffer);
	memcpy(&buffer[30],"ijiro.bin\x0",10);
	settingload(buffer);

	#ifdef _PSID_
		corruptPsid(psid);
	#endif

	//Set the VRAM to null, use the current screen
	pspDebugScreenInitEx(0x44000000, 0, 0);
	vram=NULL;

	//Setup the controller
	sceCtrlSetSamplingCycle(0);
	sceCtrlSetSamplingMode(PSP_CTRL_MODE_ANALOG);

	SceUID block_id;

	#ifndef _UMDMODE_
	//fix!
	block_id = sceKernelAllocPartitionMemory(4, "mkmenu", PSP_SMEM_Low, 512*272*2, NULL);
	vram = (void*) (0xA0000000 | (unsigned int) sceKernelGetBlockHeadAddr(block_id));
	#endif
	
	//Register the button callbacks
	//sceCtrlRegisterButtonCallback(3, triggerKey | menuKey | screenKey, buttonCallback, NULL);
	sceCtrlRegisterButtonCallback(3, 0xFFFFFFFF, buttonCallback, NULL);

	#ifdef _POPS_
	GETID();
	cheatRefresh=1;
	cheatLoad();
		if(cheatTotal==0){
		sprintf(buffer, "#%s\n0x00000000 0x00000000\n" ,gameId);
		int fd = sceIoOpen(gameDir, PSP_O_CREAT | PSP_O_WRONLY, 0777);
		sceIoWrite(fd,buffer,strlen(buffer));
		sceIoClose(fd);
		cheatRefresh=1;
		cheatLoad();
		}
	#endif
			
	#ifdef _CFW_
	GETID();
	cheatRefresh=1;
	cheatLoad();
		if(cheatTotal==0){
		sprintf(buffer, "#%s\n0x00000000 0x00000000\n" ,gameId);
		int fd = sceIoOpen(gameDir, PSP_O_CREAT | PSP_O_WRONLY, 0777);
		sceIoWrite(fd,buffer,strlen(buffer));
		sceIoClose(fd);
		cheatRefresh=1;
		cheatLoad();
		}
	#endif

	int doonce=0;
	//Do the loop-de-loop
	while(running){

		#ifdef _UMDMODE_
		if(vram == NULL){
			unsigned int a_address=0;
			unsigned int a_bufferWidth=0;
			unsigned int a_bufferHeight=0;
			unsigned int a_pixelFormat=0;
			unsigned int a_sync;
			//Has the HOME button been pressed?

			sceDisplayGetFrameBufferInternal(0, &a_address, &a_bufferWidth, &a_pixelFormat, &a_sync);

			if(a_address){
				vram=(void*)(0xA0000000 | a_address);
			}
			else{
				sceDisplayGetMode(&a_pixelFormat, &a_bufferWidth, &a_bufferHeight);
				pspDebugScreenSetColorMode(a_pixelFormat);
				pspDebugScreenSetXY(0, 0);
				pspDebugScreenSetTextColor(color01);
				pspDebugScreenPuts("Press home twice and then press volume + and - at the same time");
		
			}

			
			sceKernelDelayThread(1500);
			continue;
		}
		#endif
		
		//Handle menu
		if(menuDrawn){
			//Stop the game from receiving input (USER mode input block)
			sceCtrlSetButtonMasks(0xFFFF, 1);  // Mask lower 16bits
			sceCtrlSetButtonMasks(0x10000, 2); // Always return HOME key

			//Setup a custom VRAM
			sceDisplaySetFrameBufferInternal(0, vram, 512, 0, 1);
			pspDebugScreenInitEx(vram, 0, 0);
			
			//Draw menu
			if(cheatPause) gamePause(thid);
			menuInput();
			gameResume(thid);

			//Return the standard VRAM
			sceDisplaySetFrameBufferInternal(0, 0, 512, 0, 1);
			
			//Allow the game to receive input
			sceCtrlSetButtonMasks(0x10000, 0); // Unset HOME key
			sceCtrlSetButtonMasks(0xFFFF, 0);  // Unset mask
		}
		else if((cheatHz != 0) || (cheatFlash)){
			//Apply cheats
			cheatApply(-1);
			if(cheatFlash > 0) cheatFlash--;
		}
		
		if(suspend){
		suspend=0;
		scePowerRequestSuspend();
		}
		
		//Handle screenshot
		#ifdef _SCREENSHOT_
		if((screenTime) && (screenPath[0])){
		  screenTime=0;
		  void *block_addr;
		  void *frame_addr;
		  int frame_width;
		  int pixel_format;
		  int sync = 1;
		  u32 p;
		  
		  while(1){
			sprintf(buffer, screenPath, screenNo);
			fd=sceIoOpen(buffer, PSP_O_RDONLY, 0777);
			if(fd > 0){
			  sceIoClose(fd);
				screenNo++;
			}
			else{
			  break;
			}
		  }
		  
		  if((sceDisplayGetFrameBufferInternal(2, &frame_addr, &frame_width, &pixel_format, &sync) < 0) || (frame_addr == NULL)){
		  }
		  else{
			p = (u32) frame_addr;

			if(p & 0x80000000){
				p |= 0xA0000000;
			}
			else{
				p |= 0x40000000;
			}

			gamePause(thid);
			bitmapWrite((void *) p, NULL, pixel_format, buffer);
			gameResume(thid);
			
			screenNo++;
		  }
		}
		#endif

		//Wait a certain amount of seconds before reapplying cheats again
		sceKernelDelayThread(!cheatHz ? 500000: cheatHz);
	
	}
	return 0;

}

int _start(SceSize args, void *argp){
  hbpath = sceKernelInitFileName();

  #ifdef _PSID_
  //Load the CFG
  if(cfg[4]){
  	SceModule *mod;
  	while(1){
  		mod=sceKernelFindModuleByName(hookA[0].modname);
		if(mod == NULL) { sceKernelDelayThread(100); continue;}
  		break;
  	}
  	moduleHookFunc(&hookA[0].modfunc, sceKernelSearchModuleByName(hookA[0].modname), hookA[0].libname, hookA[0].nid, hookA[0].func);
  }
  memcpy(&triggerKey, cfg+11, 4);
  memcpy(&menuKey, cfg+15, 4);
  memcpy(&screenKey, cfg+20, 4);
  #endif
  
  //Create thread
  sceKernelGetThreadmanIdList(SCE_KERNEL_TMID_Thread, thread_buf_start, MAX_THREAD, &thread_count_start);
  thid=sceKernelCreateThread("MKIJIROThread", &mainThread, 0x18, 0x2000, 0, NULL);

  //Start thread
  if(thid >= 0) sceKernelStartThread(thid, 0, NULL);
  
	return 0;
}

int _stop(SceSize args, void *argp){
	#ifdef _USB_
	DeinitUsbStorage();
	#endif
	running = 0;
 	sceKernelTerminateThread(thid);
	return 0;
}

void dump_memregion(const char* file, void *addr, int len){
		int fd = sceIoOpen(file, PSP_O_CREAT | PSP_O_TRUNC | PSP_O_WRONLY, 0777);
		sceIoWrite(fd, addr, len);
		sceIoClose(fd);
}

void browseMAX(){
 if(browseAddress[bdNo] < 0x48800000){
 browseAddress[bdNo]=0x48800000;}
 if(browseAddress[bdNo] > (0x4A000000-(26*browseLines))){
 browseAddress[bdNo]=(0x4A000000-(26*browseLines));}
}

void decodeMAX(){
 if(decodeAddress[bdNo] < 0x48800000){
 decodeAddress[bdNo]=0x48800000;}
 if(decodeAddress[bdNo] > 0x49FFFF98){
 decodeY[bdNo]=(decodeAddress[bdNo]-0x49FFFF98)>>2;
 decodeAddress[bdNo]=0x49FFFF98;
 if(decodeY[bdNo]>25){decodeY[bdNo]=25;}}
}

void restore(unsigned char restore_flag){
					if(cheatSaved)//backup
					  {
						if(((cheatDMA+block[extSelected[0]].address)>0x8800000) && ((cheatDMA+block[extSelected[0]].address)<0xA000000)){
						if(block[extSelected[0]].flags & FLAG_CWC){}
						else if(block[extSelected[0]].flags & FLAG_JOKER){}
						else{
						switch(block[extSelected[0]].flags & FLAG_DWORD)
						{	
							case FLAG_BYTE:
							if(restore_flag){
							block[extSelected[0]].stdVal=*((unsigned char*)(cheatDMA+block[extSelected[0]].address));
							}
							else{
							*((unsigned char*)(cheatDMA+block[extSelected[0]].address))=block[extSelected[0]].stdVal; break;
							}
							break;
							case FLAG_WORD:
							if(restore_flag){
							block[extSelected[0]].stdVal=*((unsigned short*)(cheatDMA+block[extSelected[0]].address & 0xFFFFFFFE));
							}
							else{
							*((unsigned short*)(cheatDMA+block[extSelected[0]].address & 0xFFFFFFFE))=block[extSelected[0]].stdVal; break;
							}
							break;
							case FLAG_DWORD:
							if(restore_flag){
							block[extSelected[0]].stdVal=*((unsigned int*)(cheatDMA+block[extSelected[0]].address& 0xFFFFFFFC));
							}
							else{
							*((unsigned int*)(cheatDMA+block[extSelected[0]].address& 0xFFFFFFFC))=block[extSelected[0]].stdVal; break;
							}
							break;
							default:
							block[blockTotal].flags|=FLAG_UWORD;
						}
 						}
 						}
					}
}

void remenu(){
    		if(*(unsigned int *)(vram) == 0x9CE79CE7){
    		int i=0;
    		while(i<34){
    		lineClear(i);i++;
    		}
    		if(copyMenu) copyMenu=0;
    		menuDraw();//redraw or hide menu
			}
}

#ifdef _CFW_
void GETID(){
	//GAMEID
		int fd=sceIoOpen("disc0:/UMD_DATA.BIN", PSP_O_RDONLY, 0777);
if(fd >0){
		sceIoRead(fd, gameId, 10);
		sceIoClose(fd);
		k=5;
}
else{
		sceIoClose(fd);
 		fd=sceIoOpen(hbpath, PSP_O_RDONLY, 0777);
     if(fd > 0){
		sceIoRead(fd, fileBuffer,0x400);
		counteraddress=*(unsigned int *)(&fileBuffer[0x34]);
		addresscode=*(unsigned int *)(&fileBuffer[0x38]);
		unsigned char i;
		 	for(i=0;i<addresscode;i++){
		addresstmp=*(unsigned int *)(&fileBuffer[0x44+(0x10*i)]);
		 		if(addresstmp==0x10 || addresstmp==0xC || addresstmp==0x80) break;
		}
		unsigned char nameflag=0;
		if(addresstmp==0x80){
	    nameflag=1;
		}
		else{
		addresstmp=*(unsigned int *)(&fileBuffer[0x48+(0x10*i)]);
		memcpy(&gameId[0],&fileBuffer[0x28]+counteraddress+addresstmp,4);
		sprintf(buffer,"-");
		memcpy(&gameId[4],buffer,1);
		memcpy(&gameId[5],&fileBuffer[0x2C]+counteraddress+addresstmp,5);
		}
		addresscode=*(unsigned int *)(&fileBuffer[0x28]+counteraddress+4);
	if(addresscode==0x454D){}//ME,POPS
	else{//MG,HOMEBREW
	   if(strncmp(gameId, "UCJS-10041", 10) && nameflag==0){k=5;}//PSNDEMO,PSNDOWNLOAD,ISOTOOLICON
	   else{//when UCJS
	 #ifdef _HBIJIRO_ //WORK ONLY FOR HOMEBREW PBP HEADER.
		addresscode=*(unsigned int *)(&fileBuffer[0x28]+counteraddress+4);
		addresscode=*(unsigned int *)(&fileBuffer[0x38]);
		 	for(i=0;i<addresscode;i++){
		addresstmp=*(unsigned int *)(&fileBuffer[0x44+(0x10*i)]);
		 		if(addresstmp==0x80 || addresstmp==0x14 || addresstmp==0x18) break;
			}
		addresscode=*(unsigned int *)(&fileBuffer[0x48+(0x10*i)]);
		memcpy(&gameId[0],&fileBuffer[0x28]+counteraddress+addresscode,10);
	    HBFLAG=strcmp(gameId, "Prometheus");//prometheus
	    if(HBFLAG!=0){
	    HBFLAG=strcmp(gameId, "Template");//635custom_prometheus
	    }
	    if(HBFLAG!=0){
	   	HBFLAG=strcmp(gameId, "aloader");//aloader1.20~
	   	}
	   /*if(HBFLAG!=0){ these are old loader,use lasteloader
	   	HBFLAG=strcmp(gameId, "OpenIdea I");//openisoloaderbuilder
	   	}
	    if(HBFLAG!=0){
	   	HBFLAG=strcmp(gameId, "loader");//openisoloader2.4beta
	   	}*/
			if(HBFLAG==0){
	   	k=5;
		  do
		  {
	  		fd=sceIoOpen("disc0:/UMD_DATA.BIN", PSP_O_RDONLY, 0777); 
	    	sceKernelDelayThread(1000);
		  } while(fd<=0);
	  	sceIoRead(fd, gameId, 10);
  	 	 sceIoClose(fd);
	   		}
	   }
	 }
	#elif _CWCHASH_ //weltall CWCHASH finally worked out by ME&raing3
	sceIoLseek(fd, 0, SEEK_SET);
	sceIoRead(fd, fileBuffer, 0x800);
	//raing3 found SCEMD5HASH="jal $0800e844"
	sceKernelUtilsMd5Digest(fileBuffer, 0x800, buffer);
	unsigned int hash = (*(unsigned int *)(buffer + 4)) ^ (*(unsigned int *)(buffer)) ^
		(*(unsigned int *)(buffer + 8)) ^ (*(unsigned int *)(buffer + 12));
		  sprintf(gameId,"HB%08X",hash);
	    HBFLAG=strcmp(gameId, "HB2A698D7A");//prometheus
	    if(HBFLAG!=0){
	    HBFLAG=strcmp(gameId, "HB54764196");//635custom_prometheus,
	    }
	    if(HBFLAG!=0){
	   	HBFLAG=strcmp(gameId, "HB86CA566E");//aloader1.25
	   	}
	  /*if(HBFLAG!=0){
	   	HBFLAG=strcmp(gameId, "HB58B0E31E");//aloader1.21
	   	}
	    if(HBFLAG!=0){ these are old loder,use lasteloader
	    HBFLAG=strcmp(gameId, "HB1F811640");//openisoloader2.4beta
	    }
	   	if(HBFLAG!=0){
	   	HBFLAG=strcmp(gameId, "HB2FF32814");//aloader1.20
	   	}*/
			if(HBFLAG==0){
	   	k=5;
		  do
		  {
	  		fd=sceIoOpen("disc0:/UMD_DATA.BIN", PSP_O_RDONLY, 0777); 
	    	sceKernelDelayThread(1000);
		  } while(fd<=0);
	  	sceIoRead(fd, gameId, 10);
  	 	 sceIoClose(fd);
	   		}
		  /*sceIoRead(fd, fileBuffer, 0x800); old ASM DEADCOPY
	          sceIoClose(fd);
		__asm__ volatile (
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		"nop\n"
		/*
		"lui a0,$8818\n"  //fileBuffer address  this DMA
		"ori  a0,a0,$A100\n"  /fileBuffer address this DMA
		"jal $0800e844\n" //this unknow SCE/SDK???
		"addiu a1, zero, $0800\n"
		"lw a2, $004C(sp)\n"
		"lw v0, $0048(sp)\n"
		"xor a2, a2, v0"
		"lw v0, $0050(sp)\n"
		"xor a2, a2, v0"
		"lw v0, $0054(sp)\n"
		"xor a2, a2, v0\n"
		"lui t0, $8839\n"
		"sw  a2, $DBF0(t0)\n"
		);
        addresstmp=*(unsigned int*)(0x8838DBF0);*/
     }
     } 
	   #endif
	}
	sceIoClose(fd);
	}
    memcpy(&gameDir[27-k], gameId, 10);
    if(k==5){
	strcpy(buffer, ".txt\x0");
	memcpy(&gameDir[32], buffer, 5);
	}
}
#endif
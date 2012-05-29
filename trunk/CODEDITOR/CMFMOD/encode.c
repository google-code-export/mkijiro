/*
 * Copyright (C) 1999-2001, 2004 Free Software Foundation, Inc.
 * This file is part of the GNU LIBICONV Library.
 *
 * The GNU LIBICONV Library is free software; you can redistribute it
 * and/or modify it under the terms of the GNU Library General Public
 * License as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * The GNU LIBICONV Library is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public
 * License along with the GNU LIBICONV Library; see the file COPYING.LIB.
 * If not, write to the Free Software Foundation, Inc., 51 Franklin Street,
 * Fifth Floor, Boston, MA 02110-1301, USA.
 */

/*
 * UTF-8
 */

/* Specification: RFC 3629 */

#include <stdlib.h>
#include <string.h>
#include <pspkernel.h>
#include <pspsysmem_kernel.h>
#include "encode.h"
#include "allocmem.h"

/* definitions */


typedef unsigned int ucs4_t;
typedef unsigned int dword;
typedef unsigned char byte;
typedef unsigned short word;
#define RET_ILSEQ	  -1
#define RET_TOOFEW(n)  (-2-(n))
#define RET_ILUNI	  -1
#define RET_TOOSMALL   -2
#define SJIS 1
#define UTF8 2

/* 
#ifdef BIG5_ENCODE_TEXT
static unsigned short *buf = NULL;
#endif
static int encode_id = -1;
static unsigned char * UNI_CJK;
 */

//ゲームによってSJISかUTF8どちらのファイル名が使われてるか不明なため"あ"で判定する
char FILE_ENCODE(){
	int fd;
	char filemode=0;
	fd= sceIoOpen("ms0:/CheatMaster/table/\x82\xA0", PSP_O_RDONLY, 0777);
	if(fd>0){
	filemode=SJIS;
	}
	sceIoClose(fd);
	fd= sceIoOpen("ms0:/CheatMaster/table/\xE3\x81\x82", PSP_O_RDONLY, 0777);
	if(fd>0){
	filemode=UTF8;
	}
	sceIoClose(fd);

	return filemode;
}

	char tofu[]="\x81\xA0";
	char tofug[]="\xA1\xF5";

//UTF8+SJIS->GBK
int UTF8SJIS_GBK(unsigned char *msg,int len){
	char stm[80];
	char fbuffer[2048];
	u8 c1=0;
	u8 c2=0;
	int i=0;
	int k=0;
	int kk=0;
	int slen=0;
	int z=0;
	int seek=0;
 	int big=0;
	int fd;
	char filename_encode=FILE_ENCODE();

		if(filename_encode==UTF8){
   		  while(i < len){
			c1= (u8)msg[i];
			if((c1 & 0x80) ==0){
			stm[k]=c1;
			k++;
			i++;
			}
			else if(c1 < 0xC2){
			i+=2;
			}
			else if(c1 < 0xF8){
				memcpy(&seek,&msg[i],4);
				if(c1 < 0xE0){
	   			seek &= 0xFFFF;
				}
				else if(c1 < 0xF0){
	   			seek &= 0xFFFFFF;
				}
 				kk = 0;
				fd = sceIoOpen("ms0:/cheatmaster/table/utf8", PSP_O_RDONLY, 0777);
				if(fd>0){
				 while(1){
   				sceIoRead(fd,fbuffer,2048);
	  				for( z = 0; z< 512;z++){
					memcpy(&big,&fbuffer[z*4],4);
		  				if(seek==big){
			 			kk +=z;
						memcpy(&slen,&fbuffer[z*4+4],1);
			  			goto end1;
						}
						else if(big==0){
						sceIoClose(fd);
						memcpy(&stm[k],&tofu[0],2);
						k = k+2;
		  				goto fail2;
						}
					}
					kk += 512;
				 }
				}
				end1:
				sceIoClose(fd);
				fd = sceIoOpen("ms0:/cheatmaster/table/gbk", PSP_O_RDONLY, 0777);
				if(fd>0){
				sceIoLseek32(fd, kk<<1,PSP_SEEK_SET);
				if (slen<16){
		  		sceIoRead(fd,fbuffer,slen);
	  			memcpy(&stm[k],&fbuffer[0],slen);
				k +=slen;
				}
				else{
		  		sceIoRead(fd,fbuffer,2);
				if(fbuffer[1]==0){
				stm[k]=fbuffer[0];
				k++;
				}
				else{
	  			memcpy(&stm[k],&fbuffer[0],2);
				k = k+2;
				}
				}
				}
				sceIoClose(fd);

				fail2:
				if(c1 < 0xE0){
				i+=2;
				}
				else if(c1 < 0xF0){
				i+=3;
				}
				else{
				i+=4;
				}
			}
			else if(c1 < 0xFC){
			i+=5;
			}
			else if(c1 < 0xFE){
			i+=6;
			}
			else{
			i++;
			}
		  }
		}
		else if(filename_encode==SJIS){
   		  while(i < len){
			c1= (u8)msg[i];
			c2= (u8)msg[i+1];
			if((c1 & 0x80)==0){
			stm[k]=c1;
			k++;i++;
			}
			else if( ((((c1 ^0x20) +0x5F)&0xFF)<0x3C)&&(c2>=0x40)){
				memcpy(&seek,&msg[i],2);
 				kk = 0;
				fd = sceIoOpen("ms0:/cheatmaster/table/sjis", PSP_O_RDONLY, 0777);
				if(fd>0){
				 while(1){
   				sceIoRead(fd,fbuffer,2048);
	  				for( z = 0; z< 1024;z++){
					memcpy(&big,&fbuffer[z*2],2);
		  				if(seek==big){
			 			kk +=z;
			  			goto endsjis;
						}
						else if(big==0){
						sceIoClose(fd);
						memcpy(&stm[k],&tofug[0],2);
						k +=2;
		  				goto failsjis;
						}
					}
					kk += 1024;
				 }
				}
				endsjis:
				sceIoClose(fd);
				fd = sceIoOpen("ms0:/cheatmaster/table/gbk", PSP_O_RDONLY, 0777);
				if(fd>0){
				sceIoLseek32(fd, kk<<1,PSP_SEEK_SET);
		  		sceIoRead(fd,fbuffer,2);
					if(fbuffer[1]==0){
						memcpy(&stm[k],&tofug[0],2);
					}
					else{
	  			memcpy(&stm[k],&fbuffer[0],2);
					}
				k = k+2;
				}
				sceIoClose(fd);

				failsjis:
			i+=2;
			}
   		  	else{
   		  	i++;
   		  	}
   		  }
		}
	stm[k]=0;
	memcpy(&msg[0],&stm[0],len);

	return k;
}

//GBK UTF8＋SJIS
int GBK_UTF8SJIS(unsigned char *msg, int len)
{
	char stm[80];
	char fbuffer[2048];
	u8 c1=0;
	u8 c2=0;
	int i=0;
	int k=0;
	int kk=0;
	int z=0;
	int seek=0;
 	int big=0;
	int fd;
	char filename_encode=FILE_ENCODE();

	if(filename_encode==UTF8){
   	  while(i < len){
		c1= (u8)msg[i];
		c2= (u8)msg[i+1];
		if((c1 & 0x80) ==0){
		stm[k]=c1;
		k++;i++;
		}
		else if((((c1+0x7F)&0xFF) < 0x7E) && (c2>=0x40) ){
		memcpy(&seek,&msg[i],2);
 		kk = 0;
		fd = sceIoOpen("ms0:/cheatmaster/table/gbk", PSP_O_RDONLY, 0777);
			if(fd>0){
			 while(1){
   				sceIoRead(fd,fbuffer,2048);
				for( z = 0; z< 1024;z++){
				memcpy(&big,&fbuffer[z*2],2);
					if(seek==big){
					kk +=z;
					goto end;
					}
					else if(big==0){
					sceIoClose(fd);
					goto fail;
					}
				}
				kk += 1024;
			 }
			}
			end:
			sceIoClose(fd);
			fd = sceIoOpen("ms0:/cheatmaster/table/utf8", PSP_O_RDONLY, 0777);
				if(fd>0){
				sceIoLseek32(fd, kk<<2,PSP_SEEK_SET);
   				sceIoRead(fd,fbuffer,4);
				}
			sceIoClose(fd);
			c1=(u8)fbuffer[0];

			if((c1 & 0x80) ==0){
			memcpy(&stm[k],&fbuffer[0],1);
			k++;
			}
			else if(c1 < 0xE0){
			memcpy(&stm[k],&fbuffer[0],2);
			k +=2;
			}
			else if(c1 < 0xF0){
			memcpy(&stm[k],&fbuffer[0],3);
			k +=3;
			}
			else if(c1 < 0xF8){
			memcpy(&stm[k],&fbuffer[0],4);
			k+=4;
			}
			//else if(c1 < 0xFC){
			//k+=5;
			//}
			//else if(c1 < 0xFE){
			//k+=6;
			//}
		fail:
		i+=2;
		}
		else if(c1 < 0xE0){
		stm[k]=c1;
		k++;i++;
		}
		else{
	   	i++;
		}
	  }
	}
	else if(filename_encode==SJIS){
   		  while(i < len){
			c1= (u8)msg[i];
			c2= (u8)msg[i+1];
			if(c1 <= 0x81){
			stm[k]=c1;
			k++;
			i++;
			}
			else if((((c1 +0x7F)&0xFF)<0x7E)&&(c2>=0x40)){
				
				memcpy(&seek,&msg[i],2);
 				kk = 0;
				fd = sceIoOpen("ms0:/cheatmaster/table/gbk", PSP_O_RDONLY, 0777);
				if(fd>0){
				 while(1){
   				sceIoRead(fd,fbuffer,2048);
	  				for( z = 0; z< 1024;z++){
					memcpy(&big,&fbuffer[z*2],2);
		  				if(seek==big){
			 			kk +=z;
			  			goto endsjis;
						}
						else if(big==0){
						sceIoClose(fd);
						memcpy(&stm[k],&tofu[0],2);
						k = k+2;
		  				goto failsjis;
						}
					}
					kk += 1024;
				 }
				}
				endsjis:
				sceIoClose(fd);
				fd = sceIoOpen("ms0:/cheatmaster/table/sjis", PSP_O_RDONLY, 0777);
				if(fd>0){
				sceIoLseek32(fd, kk<<1,PSP_SEEK_SET);
		  		sceIoRead(fd,fbuffer,2);
					if(fbuffer[1]==0){
						memcpy(&stm[k],&tofu[0],2);
					}
					else{
	  			memcpy(&stm[k],&fbuffer[0],2);
					}
				k = k+2;
				}
				sceIoClose(fd);

				failsjis:
			i+=2;
			}
   		  	else{
   		  	i++;
   		  	}
   		  }
		}
	stm[k]=0;
	memcpy(&msg[0],&stm[0],len);

	return k;
}

//ワイド関数　utf8をutf16beに戻す,libconvのコピペ
static int utf8_mbtowc(ucs4_t *pwc, const unsigned char *s, int n)
{
	unsigned char c = s[0];

	if (c < 0x80) {
		*pwc = c;
		return 1;
	} else if (c < 0xc2) {
		return RET_ILSEQ;
	} else if (c < 0xe0) {
		if (n < 2)
			return RET_TOOFEW(0);
		if (!((s[1] ^ 0x80) < 0x40))
			return RET_ILSEQ;
		*pwc = ((ucs4_t) (c & 0x1f) << 6)
			| (ucs4_t) (s[1] ^ 0x80);
		return 2;
	} else if (c < 0xf0) {
		if (n < 3)
			return RET_TOOFEW(0);
		if (!((s[1] ^ 0x80) < 0x40 && (s[2] ^ 0x80) < 0x40
			&& (c >= 0xe1 || s[1] >= 0xa0)))
			return RET_ILSEQ;
		*pwc = ((ucs4_t) (c & 0x0f) << 12)
			| ((ucs4_t) (s[1] ^ 0x80) << 6)
			| (ucs4_t) (s[2] ^ 0x80);
		return 3;
	} else if (c < 0xf8 && sizeof(ucs4_t)*8 >= 32) {
		if (n < 4)
			return RET_TOOFEW(0);
		if (!((s[1] ^ 0x80) < 0x40 && (s[2] ^ 0x80) < 0x40
			&& (s[3] ^ 0x80) < 0x40
			&& (c >= 0xf1 || s[1] >= 0x90)))
			return RET_ILSEQ;
		*pwc = ((ucs4_t) (c & 0x07) << 18)
			| ((ucs4_t) (s[1] ^ 0x80) << 12)
			| ((ucs4_t) (s[2] ^ 0x80) << 6)
			| (ucs4_t) (s[3] ^ 0x80);
		return 4;
	} else if (c < 0xfc && sizeof(ucs4_t)*8 >= 32) {
		if (n < 5)
			return RET_TOOFEW(0);
		if (!((s[1] ^ 0x80) < 0x40 && (s[2] ^ 0x80) < 0x40
			&& (s[3] ^ 0x80) < 0x40 && (s[4] ^ 0x80) < 0x40
			&& (c >= 0xf9 || s[1] >= 0x88)))
			return RET_ILSEQ;
		*pwc = ((ucs4_t) (c & 0x03) << 24)
			| ((ucs4_t) (s[1] ^ 0x80) << 18)
			| ((ucs4_t) (s[2] ^ 0x80) << 12)
			| ((ucs4_t) (s[3] ^ 0x80) << 6)
			| (ucs4_t) (s[4] ^ 0x80);
		return 5;
	} else if (c < 0xfe && sizeof(ucs4_t)*8 >= 32) {
		if (n < 6)
			return RET_TOOFEW(0);
		if (!((s[1] ^ 0x80) < 0x40 && (s[2] ^ 0x80) < 0x40
			&& (s[3] ^ 0x80) < 0x40 && (s[4] ^ 0x80) < 0x40
			&& (s[5] ^ 0x80) < 0x40
			&& (c >= 0xfd || s[1] >= 0x84)))
			return RET_ILSEQ;
		*pwc = ((ucs4_t) (c & 0x01) << 30)
			| ((ucs4_t) (s[1] ^ 0x80) << 24)
			| ((ucs4_t) (s[2] ^ 0x80) << 18)
			| ((ucs4_t) (s[3] ^ 0x80) << 12)
			| ((ucs4_t) (s[4] ^ 0x80) << 6)
			| (ucs4_t) (s[5] ^ 0x80);
		return 6;
	} else
		return RET_ILSEQ;
}

/* unicode -> cjk */ //UTF16巨大テーブルからEUC文字コードを取得
static int encode_uni2cjk(const unsigned char *uni,unsigned char *cjk, p_encodepack pack){
	int transcount = 0;

	if(uni[0]<0x81 && uni[1]==0){
		cjk[0]=uni[0];
		transcount = 1;
	}
	else{
		int pos = (int)(*(unsigned short*)uni)*2;
		//テーブルから2バイト差し替え用
		cjk[0]=pack->UNI_CJK[pos];
		cjk[1]=pack->UNI_CJK[pos+1];
		transcount = 2;
		if(cjk[0]==0x3f && cjk[1]==0) //豆腐,u003f自体は"？"の文字
		{
			cjk[0]=0xA2;
			cjk[1]=0xA2;
		}
	}
	return transcount;
}

/* unicode string convert */
/* extern int encode_ucs_conv(const unsigned char *ucs, unsigned char *cjk)
{
	int i = 0, j = 0;
	if(cjk == NULL) cjk = (unsigned char *)ucs;

	while(*(ucs + i) != 0 || *(ucs + i + 1) != 0)
	{
		j += encode_uni2cjk(ucs + i, cjk + j);
		i += 2;
	}
	cjk[j] = 0;
	return j;
} */

/* utf-8 string convert */
extern int encode_utf8_conv(const unsigned char *ucs, unsigned char *cjk, p_encodepack pack)
{
	int i = 0, j = 0, l = strlen((const char *)ucs);
	if(cjk == NULL) cjk = (unsigned char *)ucs;

	while(i < l)
	{
		ucs4_t u = 0x1FFF;
		int p = utf8_mbtowc(&u, ucs + i, l - i);
		if(p < 0)
			break;
		if(u > 0xFFFF)
			u = 0x1FFF;
		j += encode_uni2cjk((unsigned char*)&u, cjk + j,pack);
		i += p;
	}
	cjk[j] = 0;
	return j;
}

extern int encode_init(p_encodepack pack)
{
	int fd = sceIoOpen("ms0:/CheatMaster/encode.dat", PSP_O_RDONLY, 0777);
	if(fd < 0)
		return 1;
	pack->UNI_CJK = malloc(131072);//UTF16　0x0〜0xffffまでの変換表
	if(pack->UNI_CJK == NULL)
	{
		sceIoClose(fd);
		return 1;
	}
	sceIoRead(fd, pack->UNI_CJK, 131072);
	sceIoClose(fd);
	return 0;
}

extern void encode_free(p_encodepack pack)
{
	sfree(pack->UNI_CJK);
}


#ifdef BIG5_ENCODE_TEXT
static int big5_mbtowc ( ucs4_t *pwc, const byte *s, p_encodepack pack)
{
	byte c1 = s[0];
	if ((c1 >= 0xa1 && c1 <= 0xc7) || (c1 >= 0xc9 && c1 <= 0xf9)) {
		//if (n >= 2)
		{
			byte c2 = s[1];
			if ((c2 >= 0x40 && c2 < 0x7f) || (c2 >= 0xa1 && c2 < 0xff)) {
				dword i = 157 * (c1 - 0xa1) + (c2 - (c2 >= 0xa1 ? 0x62 : 0x40));
				word wc = 0xfffd;
				if (i < 6280) {
					if (i < 6121)
						//wc = big5_2uni_pagea1[i];
						wc = pack->buf[i];
				} else {
					if (i < 13932)
						//wc = big5_2uni_pagec9[i-6280];
						wc = pack->buf[i+6121-6280];
				}
				if (wc != 0xfffd) {
					*pwc = (ucs4_t) wc;
					return 2;
				}
			}
			return RET_ILSEQ;
		}
		return RET_TOOFEW(0);
	}
	return RET_ILSEQ;
}

static int hkscs_mbtowc ( ucs4_t *pwc, const byte *s,p_encodepack pack)
{
	byte c1 = s[0];
	dword *table = (dword *)&pack->buf[6121+7652+627+3140+471+942+1];
	if ((c1 >= 0x88 && c1 <= 0x8b) || (c1 >= 0x8d && c1 <= 0xa0) || (c1 >= 0xc6 && c1 <= 0xc8) || (c1 >= 0xf9 && c1 <= 0xfe)) {
		//if (n >= 2) 
		{
			byte c2 = s[1];
			if ((c2 >= 0x40 && c2 < 0x7f) || (c2 >= 0xa1 && c2 < 0xff)) {
				dword i = 157 * (c1 - 0x80) + (c2 - (c2 >= 0xa1 ? 0x62 : 0x40));
				ucs4_t wc = 0xfffd;
				word swc;
				if (i < 2041) {
					if (i < 1883)
						swc = pack->buf[i+6121+7652-1256],
						wc = table[swc>>6] | (swc & 0x3f);
				} else if (i < 10990) {
					if (i < 5181)
						swc = pack->buf[i+6121+7652+627-2041],
						wc = table[swc>>6] | (swc & 0x3f);
				} else if (i < 18997) {
					if (i < 11461)
						swc = pack->buf[i+6121+7652+627+3140-10990],
						wc = table[swc>>6] | (swc & 0x3f);
				} else {
					if (i < 19939)
						swc = pack->buf[i+6121+7652+627+3140+471-18997],
						wc = table[swc>>6] | (swc & 0x3f);
				}
				if (wc != 0xfffd) {
					*pwc = wc;
					return 2;
				}
			}
			return RET_ILSEQ;
		}
		return RET_TOOFEW(0);
	}
	return RET_ILSEQ;
}

static int big5hkscs_mbtowc ( ucs4_t *pwc, const byte *s, p_encodepack pack)
{
	byte c = *s;
	/* Code set 0 (ASCII) */
	if (c < 0x80){
		*pwc = (ucs4_t) c;
		return 1;
	}
	/* Code set 1 (BIG5 extended) */
	if (c >= 0xa1 && c < 0xff) {
/* 		if (n < 2)
			return RET_TOOFEW(0); */
		{
			byte c2 = s[1];
			if ((c2 >= 0x40 && c2 < 0x7f) || (c2 >= 0xa1 && c2 < 0xff)) {
				if (!((c == 0xc6 && c2 >= 0xa1) || c == 0xc7)) {
					int ret = big5_mbtowc(pwc,s,pack);
					if (ret != RET_ILSEQ)
						return ret;
				}
			}
		}
	}
	return hkscs_mbtowc(pwc,s,pack);
}

/* bg5hk -> unicode */
static int charsets_bg5hk2cjk(byte *big5hk,p_encodepack pack){
	int transcount;
/* 	if(big5hk[0] < 0x81){
		transcount = 1;
	}else */
	{
		dword iunic = 0x1fff;
		big5hkscs_mbtowc(&iunic, big5hk,pack);
		transcount = encode_uni2cjk((unsigned char *)&iunic, big5hk,pack);
	}
	return transcount;
}

/* big5 string convert */
void charsets_big5_conv(char *big5,p_encodepack pack){
	int ilen = strlen((char *)big5);
	int i = 0;

	while(i < ilen)
		i += charsets_bg5hk2cjk((byte *)big5+i,pack);
	big5[i] = 0;
}

int big5_init(char *s,p_encodepack pack)
{
	int fd = sceIoOpen("ms0:/CheatMaster/big5.dat", PSP_O_RDONLY, 0777);
	if(fd < 0)	return 1;
	pack->buf = (unsigned short *)s;
	sceIoRead(fd, (unsigned char *)pack->buf, 41688);
	sceIoClose(fd);
	return 0;
}
#endif


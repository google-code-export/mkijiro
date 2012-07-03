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

//������ˤ�ä�SJIS��UTF8�ɤ���Υե�����̾���Ȥ��Ƥ뤫�����ʤ���"��"��Ƚ�ꤹ��
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


//�磻�ɴؿ�utf8��utf32���᤹,libconv�Υ��ԥ�
static int utf8_mbtowc(ucs4_t *pwc, const unsigned char *s, int n)
{
	unsigned char c = s[0];

	if (c <0x20) {
		return RET_TOOSMALL;
	}
	else if (c < 0x80) {
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

//UTF8+SJIS����ľSJIS�Ѵ�ɽ
int UTF8SJIS_SJIS(unsigned char *msg,int len){
	int k=0;
	char filename_encode=FILE_ENCODE();

		if(filename_encode==UTF8){
		k=encode_utf8_conv_noram((unsigned char *)&msg[0], NULL);
		msg[k]=0;
		msg[k+1]=0;
		k=strlen(msg);
		}
		else if(filename_encode==SJIS){
		k=strlen(msg);
		}
	

	return k;
}

//����Ȥ�ʤ���
int encode_uni2cjk_noram(const unsigned char *uni,unsigned char *cjk){
	int transcount = 0;

	if(uni[0]<0x81 && uni[1]==0){
		cjk[0]=uni[0];
		transcount = 1;
	}
	else{
		int pos = (int)(*(unsigned short*)uni)*2;
		//�ơ��֥뤫��2�Х��Ⱥ����ؤ���
	int fd = sceIoOpen("ms0:/CheatMaster/sjis.dat", PSP_O_RDONLY, 0777);
	
	if(fd < 0){
	sceIoClose(fd);
		return 2;
	}
	
	sceIoLseek32(fd, pos,PSP_SEEK_SET);
	sceIoRead(fd, cjk, 2);
	sceIoClose(fd);
		if (cjk[1]==0){//Ⱦ�ѥ���
		transcount = 1;
		}
		else{
		transcount = 2;
		}
		if(cjk[0]==0x3f && cjk[1]==0) //Ʀ��,u003f���Τ�"��"��ʸ��
		{
			cjk[0]=0x81;
			cjk[1]=0xA0;
		}
	}
	return transcount;
}

extern int encode_utf8_conv_noram(const unsigned char *ucs, unsigned char *cjk)
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
		j += encode_uni2cjk_noram((unsigned char*)&u,cjk + j);
		i += p;
	}
	cjk[j] = 0;
	return j;
}


//SJIS����ľUTF8��SJIS�Ѵ�ɽ
int SJIS_UTF8SJIS(unsigned char *msg, int len){
	
	char stm[80];
	char fbuffer[4];
	u8 c1=0;
	u8 c2=0;
	int i=0;
	int k=0;
	int pos=0;
	int fd=0;
	char filename_encode=FILE_ENCODE();

	if(filename_encode==UTF8){
   	  while(i < len){
		c1= (u8)msg[i];
		c2= (u8)msg[i+1];
		if((c1 & 0x80) ==0){
		stm[k]=c1;
		k++;
		i++;
		}
		else if(((u8)((c1^0x20)- 0xA1) < (u8)0x3C) && (c2>=0x40)){//SJISȽ��
			pos = (((c1^ 0x20)- 0xA1)*192)+(c2 -0x40);
			fd = sceIoOpen("ms0:/cheatmaster/table/sjisvsutf8", PSP_O_RDONLY, 0777);
			if(fd>=0){
			sceIoLseek32(fd, pos<<2,PSP_SEEK_SET);
			sceIoRead(fd,fbuffer,4);
			c1=fbuffer[0];
			}
			sceIoClose(fd);

			if((c1 & 0x80) ==0){//utf8 1byte
			memcpy(&stm[k],&fbuffer[0],1);
			k++;
			}
			else if(c1 < 0xE0){//utf8 2byte
			memcpy(&stm[k],&fbuffer[0],2);
			k +=2;
			}
			else if(c1 < 0xF0){//utf8 3byte
			memcpy(&stm[k],&fbuffer[0],3);
			k +=3;
			}
			else if(c1 < 0xF8){//utf8 4byte
			memcpy(&stm[k],&fbuffer[0],4);
			k+=4;
			}
		i+=2;
		}
		else if(c1<0xE0){//Ⱦ�ѥ���
			pos = c1 + 0xFF60 - 0xA0;
			stm[k] = ((pos >> 12) | 0xE0);
			stm[k+1] =  (((pos >> 6) & 0x3F) | 0x80);
			stm[k+2] =  ((pos & 0x3F) | 0x80);
		k +=3;
        i +=1;
		}
		else{
	   	i++;
		}
	  }
	stm[k]=0;
	memcpy(&msg[0],&stm[0],k);
	}
	else if(filename_encode==SJIS){
		#ifdef JISX0213
   	  while(i < len){
		c1= (u8)msg[i];
		c2= (u8)msg[i+1];
		if((c1 & 0x80) ==0){
		stm[k]=c1;
		k++;i++;
		}
		else if(((((c1^0x20)+0x5F)&0xFF) < 0x3C) && (c2>=0x40)){//SJISȽ��
			pos = ((c1^ 0x20) - 0xA1)*192 +c2-0x40;
			fd = sceIoOpen("ms0:/cheatmaster/table/sjis2004vssjis", PSP_O_RDONLY, 0777);
			if(fd>0){
				sceIoLseek32(fd, pos*2,PSP_SEEK_SET);
				sceIoRead(fd,fbuffer,2);
				}
			sceIoClose(fd);
			c1=fbuffer[0];
			c2=fbuffer[1];

			if((c1 & 0x80) ==0){//SJIS 1byte
			memcpy(&stm[k],&fbuffer[0],1);
			k++;
			}
			else if(((((c1^0x20)+0x5F)&0xFF) < 0x3C) && (c2>=0x40)){//SJIS 2byte
			memcpy(&stm[k],&fbuffer[0],2);
			k +=2;
			}
		i+=2;
		}
		else if(c1 < 0xE0){//Ⱦ�ѥ���
		stm[k]=c1;
		k++;
		i++;
		}
		else{
	   	i++;
		}
	  }
	stm[k]=0;
	memcpy(&msg[0],&stm[0],k);
		#endif
	}

	return k;
}


/* unicode -> cjk */ //UTF16����ơ��֥뤫��JISʸ�������ɤ����
static int encode_uni2cjk(const unsigned char *uni,unsigned char *cjk, p_encodepack pack){
	int transcount = 0;

	if(uni[0]<0x81 && uni[1]==0){
		cjk[0]=uni[0];
		transcount = 1;
	}
	else{
		int pos = (int)(*(unsigned short*)uni)*2;
		//�ơ��֥뤫��2�Х��Ⱥ����ؤ���
		cjk[0]=pack->UNI_CJK[pos];
		cjk[1]=pack->UNI_CJK[pos+1];
		if (cjk[1]==0){//Ⱦ�ѥ���
		transcount = 1;
		}
		else{
		transcount = 2;
		}
		
		if(cjk[0]==0x3f && cjk[1]==0) //Ʀ��,u003f���Τ�"��"��ʸ��
		{
			cjk[0]=0x81;
			cjk[1]=0xA0;
		}
	}
	return transcount;
}


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
	int fd = sceIoOpen("ms0:/CheatMaster/sjis.dat", PSP_O_RDONLY, 0777);
	if(fd < 0)
		return 1;
	pack->UNI_CJK = malloc(131072);//UTF16->SJIS 0x0��0xffff�ޤǤ��Ѵ�ɽ
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


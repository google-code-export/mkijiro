#include <pspsdk.h>
#include <pspkernel.h>
#include <pspmodulemgr_kernel.h>
#include <pspdebug.h>
#include <pspdisplay.h> 
#include <stdio.h>
#include <string.h>
#include <pspctrl.h>
#include <pspsysmem_kernel.h>
#include "conf.h"
#include "ctrl.h"
#include "font.h"
#include "encode.h"
#include "ui.h"
#include "dict.h"
#include "lang_zh.h"
#include "allocmem.h"
#include "smsutils.h"


#define LANG_DICT2 LANG_CINP2
#define LANG_DICT3 LANG_CINP3

#define DICT_MAX_WORD_ROW 320					//单词解释最大行数

typedef struct {
	const char * start;
	int count;
} t_textrow, * p_textrow;

typedef struct {
	int size;
	char * buf;
	int row_count;
	t_textrow rows[DICT_MAX_WORD_ROW];			//单词解释最大行数
} t_text, * p_text;

//索引表对应字母a--z
int idx_lookup[27] = {0, 0xf07e, 0x1d7f7, 0x36886, 0x43f9c, 0x4dbbc, 0x58a20, 0x6142b, 0x6b23f, 0x753d4, 0x77765, 0x79a3e, 0x8259b,
0x8f92b, 0x9556b, 0x9bba1, 0xb0382, 0xb17cf, 0xbd4aa, 0xdb95f, 0xe8f48, 0xeedc6, 0xf2bb6, 0xfabbe, 0xfaeb6, 0xfbd20, 0xfc5dd
};

typedef struct {
u8 *idx_buf;
int idx_len;

char *dct_buf;
int dct_data_len;

p_text txt;
}t_dict_pack, *p_dict_pack;

static unsigned int str2uint(unsigned char *str)
{
	u32 l;
	
	l = (u32)(*str++) << 24;
	l += (u32)(*str++) << 16;
	l += (u32)(*str++) << 8;
	l += (u32)*str;
	
	return l;
}

static int dict_wordrow_init(p_dict_pack pack)
{
	pack->txt = malloc(sizeof(t_text));
	if(pack->txt == NULL) return 1;
	pack->txt->buf = pack->dct_buf;
	pack->txt->size = pack->dct_data_len;

 	char * pos = pack->txt->buf, * posend = pos + pack->txt->size;
	pack->txt->row_count = 0;
	while(pack->txt->row_count < DICT_MAX_WORD_ROW && pos + 1 < posend)
	{
		pack->txt->rows[pack->txt->row_count].start = pos;
		char * startp = pos, * endp = pos + 42;				//一行最多42字节
		if(endp > posend)
			endp = posend;
		while(pos < endp && *pos != 0 && *pos != '\r' && *pos != '\n')
			if((*(unsigned char *)pos) >= 0x80)
				pos += 2;
			else
				++ pos;
		if(pos > endp)
			pos -= 2;
		if(pos + 1 < posend && ((*pos >= 'A' && *pos <= 'Z') || (*pos >= 'a' && *pos <= 'z')))
		{
			char * curp = pos - 1;
			while(curp > startp)
			{
				if(*(unsigned char *)(curp - 1) >= 0x80 || *curp == ' ' || * curp == '\t')
				{
					pos = curp + 1;
					break;
				}
				curp --;
			}
		}
		pack->txt->rows[pack->txt->row_count].count = pos - startp;
		if(*pos == 0 || *pos == '\r' || *pos =='\n')
		{
			if(*pos == '\r' && *(pos + 1) == '\n')
				pos += 2;
			else
				++ pos;
		}
		pack->txt->row_count ++;
	}
	
	return 0;
}

static inline void dict_wordrow_close(p_dict_pack pack)
{
	sfree(pack->txt);
	//pack->txt = NULL;
}

static int dict_word_rows(p_dict_pack pack)
{
/* 	if(pack->txt == NULL)
		return 0; */
	return pack->txt->row_count;
}

p_textrow dict_word_read(int row, p_dict_pack pack)
{
/* 	if(pack->txt == NULL || row >= pack->txt->row_count)
		return NULL; */
	return &pack->txt->rows[row];
}


static int dict_load_idx(char *word, p_dict_pack pack)
{
	int idx_offset,test;
	test = word[0]>' ' ? word[0]-'a' : 0 ; 
	idx_offset = idx_lookup[test];
	pack->idx_len = idx_lookup[test+1] - idx_lookup[test];
	
	int fd = sceIoOpen(DICT_IDX_DIR, PSP_O_RDONLY, 0777);
	if(fd < 0)	return 1;
	
	pack->idx_buf = malloc(pack->idx_len);
	if(pack->idx_buf == NULL) {
		sceIoClose(fd);
		return 1;
	}
	
	sceIoLseek(fd,idx_offset,0);
	sceIoRead(fd,pack->idx_buf,pack->idx_len);
	sceIoClose(fd);
	return 0;
	
}

static inline void dict_free_idx(p_dict_pack pack)
{
	sfree(pack->idx_buf);
	//pack->idx_buf = NULL;
}

static inline void dict_free_dct(p_dict_pack pack)
{
	sfree(pack->dct_buf);
	//pack->dct_buf = NULL;
}

static int dict_load_dct(wordidx *w, p_dict_pack pack)
{
	int buflen = w->len+4;		//uft8转化gbk
	if(buflen<0x30) buflen=0x30;			//txt文本太小会死机

	int fd = sceIoOpen(DICT_DCT_DIR, PSP_O_RDONLY, 0777);
	if(fd < 0)	return 1;
	
	pack->dct_buf = malloc(buflen);
	if(pack->dct_buf == NULL) {
		sceIoClose(fd);
		return 1;
	}
		
	sceIoLseek(fd,w->offset,0);
	sceIoRead(fd,pack->dct_buf,w->len);
	sceIoClose(fd);
	pack->dct_buf[w->len] = 0;

//去音标部分
	strcpy(pack->dct_buf, pack->dct_buf + strlen(pack->dct_buf) + 1);
	
	t_encodepack encodepack;
	if(encode_init(&encodepack) == 0)
	{
		pack->dct_data_len = encode_utf8_conv((unsigned char *)pack->dct_buf, NULL,&encodepack);
		encode_free(&encodepack);
	}
	else{
		dict_free_dct(pack);
		return 1;
	}
	
	return 0;
}



static int dict_convert_string(char *word, char *s)		//去连续空格,转小写
{
	int i,j=0;
	for(i=0;i<strlen(s);i++){
		if(s[i]>='A' && s[i]<'['){		//大写字母的话,转成小写
			word[j++]=s[i]+'a'-'A';
			continue;
		}
		if(s[i]>='a' && s[i]<'{' ){		//小写字母的话,不变
			word[j++]=s[i];
			continue;
		}
		
		if( s[i]=='-' || s[i]==',' || s[i]=='.' ||	s[i]=='\''){
			if(j==0) word[j++] = 'a';
			else word[j++]=s[i];
		}
		else{				//剩下的其他各种字符都变空格,除了第一个
			if( j > 0 && word[j-1] != ' ' ) word[j++] = ' ';
			if( j==0 ) word[j++] = 'a';
		}
	}
	word[j]=0;
	
	return j;
}

static void dict_display(wordidx *w, p_dict_pack pack)
{
#define TXT_LINES	12	
	if(dict_wordrow_init(pack)!=0) return;
	
	int i, cr = 0, rp = 1;
	p_textrow row;
	int textrows = dict_word_rows(pack);
	int temp = textrows - 1;
	while(rp >= 0)
	{
		if(rp)
		{
			font_fillrect(100, 30, 379, 239);
			
			font_output(110 , 40, w->word);
			
			//debug_memui();
			
			for(i = 0; i < TXT_LINES; i ++)
			{
				if(cr + i > temp)
					break;
				row = dict_word_read(cr + i,pack);
				font_outputn(110, 56 + 14 * i, row->start, row->count);
			}
			if(textrows > TXT_LINES)
			{
				font_line(371, 54, 371, 221);
				font_line(370, 54 + 167 * cr / (textrows + TXT_LINES-1), 370, 54 + 167 * (cr + TXT_LINES) / (textrows + TXT_LINES-1));
				font_line(372, 54 + 167 * cr / (textrows + TXT_LINES-1), 372, 54 + 167 * (cr + TXT_LINES) / (textrows + TXT_LINES-1));
			}
			char rowstr[30];
			sprintf(rowstr, "%d/%d", cr + 1, textrows);
			font_output(320, 232, rowstr);
			rp = 0;
		}
		switch(ctrl_waitmask(PSP_CTRL_LTRIGGER | PSP_CTRL_RTRIGGER | PSP_CTRL_UP | PSP_CTRL_DOWN | PSP_CTRL_LEFT | PSP_CTRL_RIGHT | PSP_CTRL_CROSS | PSP_CTRL_CIRCLE))
		{
		case PSP_CTRL_CIRCLE:
		case PSP_CTRL_CROSS:
				rp = -1;
			break;
		case PSP_CTRL_LTRIGGER:
			cr = 0;
			rp = 1;
			break;
		case PSP_CTRL_RTRIGGER:
			cr = temp;
			rp = 1;
			break;
		case PSP_CTRL_UP:
			if(cr > 0)
			{
				cr --;
				rp = 1;
			}
			break;
		case PSP_CTRL_DOWN:
			if(cr < temp)
			{
				cr ++;
				rp = 1;
			}
			break;
		case PSP_CTRL_LEFT:
			if(cr > 0)
			{
				cr -= TXT_LINES;
				if(cr < 0)
					cr = 0;
				rp = 1;
			}
			break;
		case PSP_CTRL_RIGHT:
			if(cr < temp)
			{
				cr += TXT_LINES;
				if(cr > temp)
					cr = temp;
				rp = 1;
			}
			break;
		}
	}	
	dict_wordrow_close(pack);	
}

static void dict_initdisplay(int x, int y, char *s)
{
	font_fillrect(x-12, y-12, 379, 239);
	char buffer[60];
	if(strlen(s)){
		sprintf(buffer, "历史:%s", s);
		font_output(x, y + 110, buffer);
	}
	font_output(x, y + 132, LANG_DICT0);
	font_output(x, y + 144, LANG_DICT2);
	font_output(x, y + 156, LANG_DICT3);
	font_output(x, y + 168, LANG_DICT1);
	font_output(x, y + 180, LANG_DICT4);
				
	//debug_memui();
}

static inline void dict_wordidx_info(wordidx *w, int i, p_dict_pack pack)
{
							strcpy(w->word, (char *)pack->idx_buf+i);
							w->offset = str2uint(pack->idx_buf+i+strlen((char*)pack->idx_buf+i)+1);
							w->len = str2uint(pack->idx_buf+i+strlen((char *)pack->idx_buf+i)+5);
}

int dict_input_string(int x, int y, char * s, int len)
{
	t_dict_pack dictpack;
	
	wordidx wkey;
	wordidx wordlist;
	char lastword[DICT_MAXWORD_LEN];

	int display_set = 0;
	int idx_loaded = 0;
	int firstchanged = 0;
	int idxpos = 0;
	char buffer[60];
	
	int temp = choose_alloc(131072);
	if(temp==0) return 0;
	
	static char ikey[4][10] = {
		{'1', '2', '3', '4', '5', '6', '7', '8', '9', '0'},
		{'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P'},
		{'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', '-'},
		{'Z', 'X', 'C', 'V', 'B', 'N', 'M', ',', '.', '\''},
	};
	char str[len + 1];
	strcpy(str,"\0");
	int dx = 0, dy = 0;
	int pos = 0;

	dict_initdisplay(x,y,s);

	while(1)
	{
		font_fillrect(x, y, x + 140, y + 60);
			int i, j;
			char ss[2];
			ss[1] = 0;
			for(j = 0; j < 4; j ++)
				for(i = 0; i < 10; i ++)
				{
					ss[0] = ikey[j][i];
					font_output(x + i * 12, y + 12 + j * 12, ss);
				}
		
		font_output(x + dx * 12, y + 14 + dy * 12, "_");
		font_fillrect(x, y, x + 6 * len + 5, y + 9);
		font_output(x, y, str);
		font_output(x + pos * 6, y + 2, "_");
		
		
		
		if(dict_convert_string(buffer, str)>0){	
			if(firstchanged!=0){
				firstchanged=0;
				if(idx_loaded!=0){
					dict_free_idx(&dictpack);
					idx_loaded=0;				
				}
				if(dict_load_idx(buffer, &dictpack)!=0) return 0;
				idx_loaded=1;
			}
	
			if(idx_loaded!=0)
			{			
				int k,l,buflen=strlen(buffer);
				
				idxpos=-1;
				do{
					buffer[buflen--]=0;					
					
					i=0;
					strcpy(lastword, (char *)dictpack.idx_buf+i);
					while(i<dictpack.idx_len){
						dict_convert_string(wordlist.word, (char *)dictpack.idx_buf+i);
						if( strstr(wordlist.word, buffer) == wordlist.word ){
							idxpos=i;
							dict_wordidx_info(&wordlist,i, &dictpack);
							break;
						}
						strcpy(lastword, (char *)dictpack.idx_buf+i);
						i+=9+strlen((char *)dictpack.idx_buf+i);
					}
					
				}
				while(idxpos<0);
				
				l=idxpos;
				
				font_fillrect(x+148, y+12, 379, y+12*12);
				for(k=0;k<10;k++){
					char *p=(char *)(dictpack.idx_buf+l);
					font_outputn(x+156, y +12+ 12*k, p, 16);
					l+=9+strlen(p);
					
					if(l>=dictpack.idx_len) break;				
				}
				
			}
		}
		
		switch(ctrl_waitmask(PSP_CTRL_LEFT | PSP_CTRL_RIGHT | PSP_CTRL_UP | PSP_CTRL_DOWN | PSP_CTRL_CIRCLE | PSP_CTRL_CROSS | PSP_CTRL_SQUARE | PSP_CTRL_TRIANGLE | PSP_CTRL_SELECT | PSP_CTRL_START | PSP_CTRL_LTRIGGER | PSP_CTRL_RTRIGGER))
		{
		case PSP_CTRL_LEFT:
			font_fillrect(x + dx * 12, y + 20 + dy * 12, x + 11 + dx * 12, y + 23 + dy * 12);
			dx --;
			if(dx < 0)
				dx = 9;
			break;
		case PSP_CTRL_RIGHT:
			font_fillrect(x + dx * 12, y + 20 + dy * 12, x + 11 + dx * 12, y + 23 + dy * 12);
			dx ++;
			if(dx > 9)
				dx = 0;
			break;
		case PSP_CTRL_UP:
			font_fillrect(x + dx * 12, y + 20 + dy * 12, x + 11 + dx * 12, y + 23 + dy * 12);
			dy --;
			if(dy < 0)
				dy = 3;
			break;
		case PSP_CTRL_DOWN:
			font_fillrect(x + dx * 12, y + 20 + dy * 12, x + 11 + dx * 12, y + 23 + dy * 12);
			dy ++;
			if(dy > 3)
				dy = 0;
			break;
		case PSP_CTRL_CIRCLE:
			{
			if(!config.swap){
				if(pos >= len)	break;
				memmove(&str[pos + 1], &str[pos], len - pos - 1);
				str[pos] = ikey[dy][dx];
				if( pos==0 && ( (str[0]>='A' && str[0]<'[') || (str[0]>='a' && str[0]<'{') ) ) firstchanged=1;
				pos ++;
			}
			else{
				ctrl_waitrelease();
				if(idx_loaded!=0){
					dict_free_idx(&dictpack);
				}
				return 0;
			}
			}
			break;
		case PSP_CTRL_CROSS:
			{
			if(config.swap){
				if(pos >= len)	break;
				memmove(&str[pos + 1], &str[pos], len - pos - 1);
				str[pos] = ikey[dy][dx];
				if( pos==0 && ( (str[0]>='A' && str[0]<'[') || (str[0]>='a' && str[0]<'{') ) ) firstchanged=1;
				pos ++;
			}
			else{
				ctrl_waitrelease();
				if(idx_loaded!=0){
					dict_free_idx(&dictpack);
				}
				return 0;
			}
			}
			break;
		case PSP_CTRL_TRIANGLE:
			if(pos < len)
			{
				memmove(&str[pos + 1], &str[pos], len - pos - 1);
				if(pos==0){
					firstchanged=1;
					str[pos] = 'A';
				}
				else str[pos] = ' ';
				pos ++;
			}
			break;
		case PSP_CTRL_SQUARE:
			if(pos > 0)
			{
				if(pos < len)
					memmove(&str[pos - 1], &str[pos], len - pos);
				str[pos] = 0;
				pos --;
			}
			break;
		case PSP_CTRL_SELECT:
			memset(str, 0, len);
			pos = 0;
			break;
		case PSP_CTRL_START:
			mips_memcpy(s, str, len);
			
			if(dict_convert_string(wkey.word, str)>0 && idx_loaded!=0 )
			{
				strcpy(wkey.word,wordlist.word);
				wkey.offset=wordlist.offset;
				wkey.len=wordlist.len;
				
				dict_free_idx(&dictpack);
				idx_loaded=0;
				
				if(dict_load_dct(&wkey, &dictpack)!=0) return 0;
				display_set = 1;
			}
			
			if(display_set>0){
				dict_display(&wkey, &dictpack);
				display_set = 0;
				dict_free_dct(&dictpack);			
				dict_initdisplay(x,y,s);
			}
				memset(str, 0, len);
				pos = 0;			
			break;
		case PSP_CTRL_LTRIGGER:
			if(idx_loaded!=0){
				strcpy(str,lastword);
				pos = strlen(str);
			}
			break;
		case PSP_CTRL_RTRIGGER:
			if(idx_loaded!=0){
				j=idxpos + strlen((char *)dictpack.idx_buf+idxpos) + 9;
				if(j<dictpack.idx_len){
					strcpy(str,(char *)dictpack.idx_buf+j);
					pos = strlen(str);
				}
			}
			break;
		}
		
		
	}
}




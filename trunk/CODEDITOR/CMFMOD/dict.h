#ifndef _DICT_H_
#define _DICT_H_

#define DICT_MAXWORD_LEN 44

typedef struct wordidx{
	char word[DICT_MAXWORD_LEN];
	u32 offset;
	u32 len;
} wordidx;



extern int dict_input_string(int x, int y, char * s, int len);

#endif



#ifndef _ENCODE_H
#define _ENCODE_H

typedef struct{
unsigned short *buf;
unsigned char *UNI_CJK;
}t_encodepack, *p_encodepack;

extern int encode_init(p_encodepack);
extern void encode_free(p_encodepack);

//extern int encode_ucs_conv(const unsigned char *ucs, unsigned char *cjk);
extern int encode_utf8_conv(const unsigned char *ucs, unsigned char *cjk, p_encodepack pack);

#ifdef BIG5_ENCODE_TEXT
extern void charsets_big5_conv(char *,p_encodepack);
extern int big5_init(char *,p_encodepack);
#endif


#endif

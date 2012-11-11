#ifndef _LANG_EN_H
#define _LANG_EN_H

#ifdef ENGLISH_UI
	#define LANG_ADDRESS "Addr:     "
	#define LANG_FOUND "%d result found!"
	#define LANG_SEARCHTYPE "Search type: "
	#define LANG_SEARCHRANGE "Search range: "
	#define LANG_RESTOTAL "%d result totally"
#else

#define LANG_ADDRESS "アドレス:     "
#define LANG_FOUND "%d個候補が見つかりました!"
#define LANG_SEARCHTYPE "bitタイプ: "
#define LANG_SEARCHRANGE "検索範囲: "
#define LANG_RESTOTAL "全候補数%d"

#endif

extern char LANG_LOWADDR [];
extern char LANG_HIGHADDR [];
extern char LANG_COMMENT [];
extern char LANG_DATATYPE [];
extern char LANG_VALUE [] ;
extern char LANG_LOCKQ [] ;
extern char LANG_ADDRP1 [] ;
extern char LANG_ADDRP2 [] ;
extern char LANG_ADDALL [] ;
extern char LANG_ADD [] ;
extern char LANG_TABLETITLE [] ;

extern char LANG_TABLEP3 [] ;
extern char LANG_TABLEP4 [] ;

extern char LANG_NEWADDR [] ;
extern char LANG_NEWADDR2 [] ;

extern char LANG_SEARCH [] ;

extern char LANG_NOTFOUND [] ;
extern char LANG_PRESS1 [] ;
extern char LANG_RESTITLE [] ;
extern char LANG_RESP1 [] ;
extern char LANG_RESP2 [] ;
extern char LANG_RESP3 [] ;
extern char LANG_RESP4 [] ;

extern char LANG_EMPTY [] ;
extern char LANG_SLP1 [] ;
extern char LANG_CLEARQ [] ;

extern char LANG_PRESSSKEY [] ;
extern char LANG_CINP1 [][19];
extern char LANG_CINP2 [] ;
extern char LANG_CINP3 [] ;
extern char LANG_CINP4 [] ;
extern char LANG_CINP5 [] ;
extern char LANG_CINP6 [] ;
extern char LANG_CINP7 [] ;
extern char LANG_CINP8 [] ;
extern char LANG_CINPHEX [];
extern char LANG_CINPDEC [];
extern char LANG_CINPFLOATDEC [];
//layout.c
extern char LAYOUT_READ_HELP [] ;
extern char LAYOUT_READTEXT_SAVEHELP [] ;
extern char LAYOUT_MEM_WRITEOK[] ;

//dict.c
extern char LANG_DICT1 [] ;
extern char LANG_DICT4 [] ;
extern char DICT_IDX_DIR [] ;
extern char DICT_DCT_DIR [] ;
extern char LANG_DICT0 [];

//font.c
extern char FONT_DIR [] ;

//ui.c
extern char ISOFS_UMDDATA[];
extern char ISOFS_SFO[];

//mem.c
extern char MODULE_DIR[];
extern char MEM_DIR [];
extern char CMF_DIR [];
extern char SET_DIR [];
extern char TAB_DIR [];
extern char IME_DIR [];
extern char MCR_DIR [];


#endif
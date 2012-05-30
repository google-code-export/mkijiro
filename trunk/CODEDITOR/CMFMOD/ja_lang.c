char LANG_LOWADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "開始アドレス:";
char LANG_HIGHADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "終了アドレス:";
char LANG_COMMENT [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "コード名:";

char LANG_DATATYPE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "bit数:   ";
char LANG_VALUE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "数値:";
char LANG_LOCKQ [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "数値固定?";
char LANG_ADDRP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "←文字列  ↑オフセット →16進数 START ポインタ";
char LANG_ADDRP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "□アドレス △コード追加 SELECT  履歴(最近10件)  ";
char LANG_ADDALL [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "全結果をコード追加";
char LANG_ADD [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "1つコード追加 ";
char LANG_TABLETITLE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "コード追加    アドレス   值       bit  実行";
char LANG_TABLEP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "○ 変更";
char LANG_TABLEP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "× 変更";
char LANG_NEWADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "コード追加";

char LANG_SEARCH [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "検索:";

char LANG_NOTFOUND [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "サーチ失敗   ";
char LANG_PRESS1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "○/×で継続...  ";
char LANG_RESTITLE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "アドレス 8bit 16bit  32bit ";
char LANG_RESP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="×1つだけ追加する   ";
char LANG_RESP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="○1つだけ追加する   ";
char LANG_RESP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="□全結果を追加する  ";
char LANG_RESP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="△メモリ  ";

char LANG_EMPTY [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="空";
char LANG_SLP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="SEL CODE削除  ";
char LANG_CLEARQ [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="前CODEを消去?";

char LANG_PRESSSKEY [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="ボタン変更...";

char LANG_CINP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="△  空欄";
char LANG_CINP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="□　戻る";
char LANG_CINP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="START確定  L英語切替";
char LANG_CINP5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="R+上下　選択";
char LANG_CINP6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="←選択文字";
char LANG_CINP8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="R 拼音削除";
char LANG_CINP7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="拼音:";
char LANG_CINP1 [][19] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
"SELECT  大小  英文",
"SELECT  中国語入力"
};
char LANG_CINPHEX [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"16進数　　　"};
char LANG_CINPDEC [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"10進数　　　"};
char LANG_CINPFLOATDEC [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"浮動小数点"};
//layout.c
char LAYOUT_READ_HELP [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START リネーム,SEL 削除,□中国文字切替";
char LAYOUT_READTEXT_SAVEHELP [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START ラベル,SEL/△ 中断,LR スクロール,○× 終了  ";
char LAYOUT_MEM_WRITEOK [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ok";

//dict.c
char LANG_DICT0 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "L/R   選択";
char LANG_DICT1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SEL 削除  ";
char LANG_DICT4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START 確定";


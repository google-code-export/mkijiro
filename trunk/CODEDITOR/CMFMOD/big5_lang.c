char LANG_LOWADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "最小地址:";
char LANG_HIGHADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "最大地址:";
char LANG_COMMENT [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "說明:";

char LANG_DATATYPE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "資料類型:";
char LANG_VALUE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "數值:";
char LANG_LOCKQ [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "鎖定數值?";
char LANG_ADDRP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "←搜索字元 ↑相對位址 →搜索16進制 START指針值";
char LANG_ADDRP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "□指定地址 △新增地址 SELECT歷史跳轉(最近10次)";
char LANG_ADDALL [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "添加全部結果到表格";
char LANG_ADD [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "添加結果到表格";
char LANG_TABLETITLE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "說明         地址       值       類型 鎖定";
char LANG_TABLEP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "○ 修改";
char LANG_TABLEP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "× 修改";
char LANG_NEWADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "新增地址";

char LANG_SEARCH [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "搜索:";

char LANG_NOTFOUND [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "沒有找到結果";
char LANG_PRESS1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "請按○或×繼續…";
char LANG_RESTITLE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "位址    位元組 雙位元組 四位元組";
char LANG_RESP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="×添加所選結果到表格";
char LANG_RESP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="○添加所選結果到表格";
char LANG_RESP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="□添加所有結果到表格";
char LANG_RESP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="△查看記憶體";

char LANG_EMPTY [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="空";
char LANG_SLP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="SELECT刪除表格";
char LANG_CLEARQ [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="清除當前表格?";

char LANG_PRESSSKEY [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="請按快捷鍵...";

char LANG_CINP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="△  空格";
char LANG_CINP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="□  退格";
char LANG_CINP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="START確定L切換中英文";
char LANG_CINP5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="R+上下選漢字";
char LANG_CINP6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="<-選定漢字";
char LANG_CINP8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="R 清除拼音";
char LANG_CINP7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="拼音:";
char LANG_CINP1 [][19] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
"SELECT大小寫  英文",
"SELECT輸入所選漢字"
};
char LANG_CINPHEX [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"16進制"};
char LANG_CINPDEC [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"10進制"};
char LANG_CINPFLOATDEC [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"浮點數"};
//layout.c
char LAYOUT_READ_HELP [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START改名,SELECT刪除,□切換中文顯示";
char LAYOUT_READTEXT_SAVEHELP [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Start儲存標籤,SEL或△中斷退出,LR多行,○×正常退出";
char LAYOUT_MEM_WRITEOK [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ok";

//dict.c
char LANG_DICT0 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "L/R   選詞";
char LANG_DICT1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECT清除";
char LANG_DICT4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START 確定";


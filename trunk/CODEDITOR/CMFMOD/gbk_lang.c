char LANG_LOWADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "最小地址:";
char LANG_HIGHADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "最大地址:";
char LANG_COMMENT [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "说明:";

char LANG_DATATYPE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "数据类型:";
char LANG_VALUE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "数值:";
char LANG_LOCKQ [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "锁定数值?";
char LANG_ADDRP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "←搜索字符 ↑相对地址 →搜索16进制 START指针值";
char LANG_ADDRP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "□指定地址 △新增地址 SELECT历史跳转(最近10次)";
char LANG_ADDALL [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "添加全部结果到表格";
char LANG_ADD [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "添加结果到表格";
char LANG_TABLETITLE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "说明         地址       值       类型 锁定";
char LANG_TABLEP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "○ 修改";
char LANG_TABLEP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "× 修改";
char LANG_NEWADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "新增地址";

char LANG_SEARCH [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "搜索:";

char LANG_NOTFOUND [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "没有找到结果";
char LANG_PRESS1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "请按○或×继续…";
char LANG_RESTITLE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "地址    字节 双字节 四字节";
char LANG_RESP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="×添加所选结果到表格";
char LANG_RESP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="○添加所选结果到表格";
char LANG_RESP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="□添加所有结果到表格";
char LANG_RESP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="△查看内存";

char LANG_EMPTY [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="空";
char LANG_SLP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="SELECT删除表格";
char LANG_CLEARQ [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="清除当前表格?";

char LANG_PRESSSKEY [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="请按快捷键...";

char LANG_CINP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="△  空格";
char LANG_CINP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="□  退格";
char LANG_CINP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="START确定L切换中英文";
char LANG_CINP5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="R+上下选汉字";
char LANG_CINP6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="<-选定汉字";
char LANG_CINP8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="R 清除拼音";
char LANG_CINP7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="拼音:";
char LANG_CINP1 [][19] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
"SELECT大小写  英文",
"SELECT输入所选汉字"
};
char LANG_CINPHEX [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"16进制"};
char LANG_CINPDEC [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"10进制"};
char LANG_CINPFLOATDEC [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"浮点数"};
//layout.c
char LAYOUT_READ_HELP [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START改名,SELECT删除,□切换中文显示";
char LAYOUT_READTEXT_SAVEHELP [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Start储存标签,SEL或△中断退出,LR多行,○×正常退出";
char LAYOUT_MEM_WRITEOK [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ok";

//dict.c
char LANG_DICT0 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "L/R   选词";
char LANG_DICT1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECT清除";
char LANG_DICT4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START 确定";


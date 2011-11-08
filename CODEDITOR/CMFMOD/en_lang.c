char LANG_LOWADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "LowAddr:";
char LANG_HIGHADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "HighAddr:";
char LANG_COMMENT [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Note:";

char LANG_DATATYPE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "DataType:";
char LANG_VALUE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Value:";
char LANG_LOCKQ [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Lock?";
char LANG_ADDRP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "¡û-String ¡ü-OffsetJump ¡ú-Hex START-PointJump";
char LANG_ADDRP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "¡õ-InputAddr ¡÷-AddNew SELECT-HistoryJump(Last10)";
char LANG_ADDALL [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Add all to table";
char LANG_ADD [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Add to table";
char LANG_TABLETITLE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Note         Addr       Value    Type Lock";
char LANG_TABLEP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "¡ð-Modify";
char LANG_TABLEP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "¡Á -Modify";
char LANG_NEWADDR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "New Addr";

char LANG_SEARCH [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Search:";

char LANG_NOTFOUND [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Found Nothing";
char LANG_PRESS1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Press ¡ð/¡Á to continue";
char LANG_RESTITLE [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Addr    Byte  HWord   Word";
char LANG_RESP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="¡Á-Add selected to table";
char LANG_RESP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="¡ð-Add selected to table";
char LANG_RESP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="¡õ-Add all to table";
char LANG_RESP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="¡÷-View Memory";

char LANG_EMPTY [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="Empty";
char LANG_SLP1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="SELECT-Delete table";
char LANG_CLEARQ [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="Clear current table?";

char LANG_PRESSSKEY [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="Press Key";

char LANG_CINP2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="¡÷-Space";
char LANG_CINP3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="¡õ-Delete";
char LANG_CINP4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="START   -Confirm";
char LANG_CINP5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="R+UP.DOWN";
char LANG_CINP6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="<-";
char LANG_CINP8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="R ";
char LANG_CINP7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="PinYin:";
char LANG_CINP1 [][19] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
"SELECT-Captial EN",
"SELECT-        GB"
};
char LANG_CINPHEX [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"Hex"};
char LANG_CINPDEC [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"Dec"};
char LANG_CINPFLOATDEC [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={"Flt"};
//layout.c
char LAYOUT_READ_HELP [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START-Rename File,SELECT-Del File";
char LAYOUT_READTEXT_SAVEHELP [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START-SavePos SEL/¡÷-FastExit ¡ð/¡Á-Exit";
char LAYOUT_MEM_WRITEOK [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Ok";

//dict.c
char LANG_DICT0 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "L/R   ";
char LANG_DICT1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECT";
char LANG_DICT4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START ";


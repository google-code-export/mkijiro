#define LAYOUT_VIEW_ADD "%d Addr Added"
#define LAYOUT_SEARCH_DIFF "\"Greater\"or\"Less\" :%d"
#define LAYOUT_KEY_MACRO_LIST "Macro%-3dSKey:%-12sReverse:%-6s"
#define LAYOUT_KEY_MACRO_SET "Button%-3dInter%-6d%-12s"
#define LAYOUT_KEY_MACRO_FASTSET "Button%-9d%-12s"
#define LAYOUT_KEY_TURBO_SET "%-6s:%-4sInter:%-3dSKey:%s"
#define LAYOUT_KEY_MAP_SET "%-14sMapTo:%-14s"
#define LAYOUT_KEY_STICK_SET "%-18s%-4sSKey:%s"

static char menu_lockspdstr[][10] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
	"VeryShort",
	"Short",
	"Normal",
	"Long",
	"VeryLong"
};

static char menu_img1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PSX Dat";
static char menu_img2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PNG";
static char menu_img3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "JPG";
static char menu_img4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ScreenShot Directory";
static const char *layout_menu_img[]  __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_img1,
	menu_img2,
	menu_img3,
	menu_img4,
};

static char menu_main1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Search Value";
static char menu_main2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Address Table";
static char menu_main3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Save Table";
static char menu_main4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Load Table";
static char menu_main5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Memory Manager";
static char menu_main6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "View Memory";
static char menu_main7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Read Text";
static char menu_main8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "CPU/BUS Clock";
static char menu_main9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Settings";
static char menu_main10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Dictionary(En->Cn)";
static char menu_main11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Key Manager";
static char menu_main12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "View Picture";
static char menu_main13[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Etc";
static char menu_main14[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "USB Connection";

static const char * menu_main[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_main1,
	menu_main2,
	menu_main3,
	menu_main4,
	menu_main5,
	menu_main6,
	menu_main7,
	menu_main8,
	menu_main9,
	menu_main10,
	menu_main11,
	menu_main12,
	menu_main13,
	menu_main14,
};

static char menu_search21[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Search Again";
static char menu_search22[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "New Search";
static char menu_search23[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Change Type";
static char menu_search24[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Change Range";
static char menu_search25[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "View Result";
//static char menu_search26[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Change";

static const char * menu_search2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_search21,
	menu_search22,
	menu_search23,
	menu_search24,
	//menu_search26,
	menu_search25,
};

static char menu_change1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Auto";
static char menu_change2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Byte";
static char menu_change3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Word";
static char menu_change4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Dword";
static char menu_change5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Comparative Auto";
static char menu_change6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Comparative Byte";
static char menu_change7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Comparative Word";
static char menu_change8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Comparative Dword";
static char menu_change9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Comparative Float";

static const char * menu_change[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_change1,
	menu_change2,
	menu_change3,
	menu_change4,
	menu_change5,
	menu_change6,
	menu_change7,
	menu_change8,
	menu_change9
};

static char menu_fuzzy1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Greater Than(>)";
static char menu_fuzzy2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Less Than(<)";
static char menu_fuzzy3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Not Equal To(<>)";
static char menu_fuzzy4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Greater Than or Equal To(>=)";
static char menu_fuzzy5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Less Than or Equal To(<=)";
static char menu_fuzzy6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Equal To(=)";
static char menu_fuzzy7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Greater Than Fixed";
static char menu_fuzzy8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Less Than Fixed";
static char menu_fuzzy9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Not Equal To Fixed";
static char menu_fuzzy10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Greater Than or Equal To Fixed";
static char menu_fuzzy11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Less Than or Equal To Fixed";
static char menu_fuzzy12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Equal To Fixed";

static const char * menu_fuzzy[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_fuzzy1 ,
	menu_fuzzy2 ,
	menu_fuzzy3 ,
	menu_fuzzy4 ,
	menu_fuzzy5 ,
	menu_fuzzy6 ,
	menu_fuzzy7 ,
	menu_fuzzy8 ,
	menu_fuzzy9 ,
	menu_fuzzy10,
	menu_fuzzy11,
	menu_fuzzy12,
};

static char menu_fuzzy_init_manual[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Manual";

static const char * menu_fuzzy_init[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_change1,
	menu_fuzzy_init_manual,
};

static char menu_yes [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Y";
static char menu_no [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "N";

static const char * menu_yesno[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_yes,
	menu_no
};

static char layout_menu_mem1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Upload Memory";
static char layout_menu_mem2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Dump Memory";

static const char * layout_menu_mem[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_mem1,
	layout_menu_mem2
};

static char layout_menu_etc1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Set Max Brightness";
static char layout_menu_etc2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Load PSX BIOS Font";
static char layout_menu_etc3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Import PSX MemoryCard";
static char layout_menu_etc4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Export PSX MemoryCard";
static char layout_menu_etc5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PowerOff";

static const char * layout_menu_etc[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_etc1,
	layout_menu_etc2,
	layout_menu_etc3,
	layout_menu_etc4,
	layout_menu_etc5,
};

static char layout_menu_save1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Save As CMF";
static char layout_menu_save2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Save As TAB";

static const char * layout_menu_save[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_save1,
	layout_menu_save2,
};

static char layout_menu_load1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Load CMF";
static char layout_menu_load2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Load TAB";
static char layout_menu_load3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Load CW DB";
static char layout_menu_load4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Clear Table";

static const char * layout_menu_load[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_load1,
	layout_menu_load2,
	layout_menu_load3,
	layout_menu_load4
};

static char layout_menu_key1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Key Turbo";
static char layout_menu_key2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Key Map";
static char layout_menu_key7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Key Stick";
static char layout_menu_key6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Analog Map";
static char layout_menu_key3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Key Macro";
static char layout_menu_key4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Load Key Settings";
static char layout_menu_key5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Save Key Settings";

static const char * layout_menu_key[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_key1,
	layout_menu_key2,
	layout_menu_key7,
	layout_menu_key6,
	layout_menu_key3,
	layout_menu_key4,
	layout_menu_key5
};

static char menu_buscpu1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "66/33";
static char menu_buscpu2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "133/66";
static char menu_buscpu3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "222/111";
static char menu_buscpu4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "266/133";
static char menu_buscpu5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "333/166";

static const char * menu_buscpu[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_buscpu1,
	menu_buscpu2,
	menu_buscpu3,
	menu_buscpu4,
	menu_buscpu5
};

static char menu_conf1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Launch key: [音量+]+[音量+]+[音量+]+[音量+]";
static char menu_conf2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Swap ○/×:    是";
static char menu_conf3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Auto load .tab: 是";
static char menu_conf4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )=  "Auto Load .cmf: 是";
static char menu_conf5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )=  "Auto Load .set: 是";
static char menu_conf6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Lock interval:     很短";
static char menu_conf7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Enable screenshot: 是";
static char menu_conf8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Screenshot key: [音量+]+[音量+]+[音量+]+[音量+]";
static char menu_conf9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Read Text bytes per line:    ";
static char menu_conf10 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "BG  Trgb:        ";
static char menu_conf11 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "FONTrgb:      ";
static char menu_conf12 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "Screenshot format: bmp";
static char menu_conf13 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "JPG format quality: 100";
static char menu_conf14 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "Suspend key: [音量+]+[音量+]+[音量+]+[音量+]";
static char menu_conf15 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "PowerOff key:                               ";
static char menu_conf16 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "RTS SaveKey:                               ";
static char menu_conf17 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "RTS LoadKey:                               ";
static char menu_conf18 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "Auto PowerOff After RTS:  ";
static char menu_conf19 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "More Semitransparent:  ";
static const char * menu_conf[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_conf1,
	menu_conf2,
	menu_conf3,
	menu_conf4,
	menu_conf5,
	menu_conf6,
	menu_conf7,
	menu_conf8,
	menu_conf9,
	menu_conf10,
	menu_conf11,
	menu_conf12,
	menu_conf13,
	menu_conf14,
	menu_conf15,
	menu_conf16,
	menu_conf17,
	menu_conf18,
	menu_conf19
};

static char view_search_string[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"Search String"
};
static char view_search_hexstr[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"Search Hex"
};

static char layout_table_str1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "RIGHT-ModNote □-Un/Lock SEL-Del";
static char layout_table_str2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "↑,↓-Move △-ViewMem START-AddNew";
static char layout_table_str3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "LEFT-LockAll/UnlockAll △-Enable";
static char LANG_TABLESUM[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Lock  Note";

static char LANG_SEARCH_DIFFHELP[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "□-Modify,0 Disable";

static const char * low_high_value_string[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
"Min",
"Max",
};

static char keylist_list1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECT-Del △-SKey";
static char keylist_list2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "LEFT-Inter RIGHT-Detail □-Rev";

static char keylist_setkey1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECT-Del △-KeySet";
static char keylist_setkey2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "LEFT-Interval RIGHT-Record";

static char keylist_record1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START-End SELECT-Skip";
static char keylist_record2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Record Start..";

static char key_symbol1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "□";
static char key_symbol2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "△";
static char key_symbol3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "○";
static char key_symbol4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "×";
static char key_symbol5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "SELECT";
static char key_symbol6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "START";
static char key_symbol7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "UP";
static char key_symbol8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "RIGHT";
static char key_symbol9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "DOWN";
static char key_symbol10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "LEFT";
static char key_symbol11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "L";
static char key_symbol12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "R";
static char key_symbol13[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "↑";
static char key_symbol14[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "↓";
static char key_symbol15[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "←";
static char key_symbol16[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "→";
static char key_symbol17[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[Vol+]";
static char key_symbol18[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[Vol-]";
static char key_symbol19[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[Note]";
static char key_symbol20[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[Bright]";

static char turbo_key_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"LEFT-Enable RIGHT-Interval"
};
static char turbo_key_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"START-SKey SEL-Default"
};
static char keymap_str[][21] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )={
"Switch Analog-Digtal",
"Analog map to Digtal",
};
static char turbo_map_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"LEFT-Original RIGHT-MapKey"
};
static char turbo_map_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"SELECT-Del"
};
static char turbo_stick_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"LEFT-Enable RIGHT-StickKey"
};



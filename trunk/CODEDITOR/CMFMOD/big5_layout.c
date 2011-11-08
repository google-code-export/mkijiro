#define LAYOUT_VIEW_ADD "增加%d條地址"
#define LAYOUT_SEARCH_DIFF "\"增大\"\"減小\"值:%d"
#define LAYOUT_KEY_MACRO_LIST "連招%-3d啟動鍵:%-12s反向鍵:%-6s"
#define LAYOUT_KEY_MACRO_SET "按鍵%-3d間隔%-6d%-12s"
#define LAYOUT_KEY_MACRO_FASTSET "按鍵%-9d%-12s"
#define LAYOUT_KEY_TURBO_SET "%-6s:%-4s間隔:%-3d快捷鍵:%s"
#define LAYOUT_KEY_MAP_SET "%-14s映射為:%-14s"
#define LAYOUT_KEY_STICK_SET "%-18s%-4s快捷鍵:%s"

static char menu_lockspdstr[][5] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
	"很短",
	"  短",
	"一般",
	"  長",
	"很長"
};

static char menu_img1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PS格式";
static char menu_img2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PNG";
static char menu_img3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "JPG";
static char menu_img4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "截圖文件夾";
static const char *layout_menu_img[]  __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_img1,
	menu_img2,
	menu_img3,
	menu_img4,
};

static char menu_main1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "搜索數據";
static char menu_main2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "位址表格";
static char menu_main3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "保存表格";
static char menu_main4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "載入表格";
static char menu_main5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "記憶體管理";
static char menu_main6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "顯示記憶體";
static char menu_main7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "閱讀文本";
static char menu_main8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "頻率設定";
static char menu_main9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "選項設置";
static char menu_main10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "英漢詞典";
static char menu_main11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "按鍵管理";
static char menu_main12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "圖片攻略";
static char menu_main13[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "其他功能";
static char menu_main14[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "USB 連接";

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

static char menu_search21[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "再次搜索";
static char menu_search22[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "重新搜索";
static char menu_search23[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "改變類型";
static char menu_search24[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "改變範圍";
static char menu_search25[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "查看結果";
//static char menu_search26[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "改變偏差";

static const char * menu_search2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_search21,
	menu_search22,
	menu_search23,
	menu_search24,
	//menu_search26,
	menu_search25,
};

static char menu_change1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "自動";
static char menu_change2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = " 8位";
static char menu_change3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "16位";
static char menu_change4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "32位";
static char menu_change5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "模糊自動";
static char menu_change6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "模糊 8位";
static char menu_change7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "模糊16位";
static char menu_change8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "模糊32位";
static char menu_change9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "模糊浮點";

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

static char menu_fuzzy1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "增大(>)";
static char menu_fuzzy2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "減小(<)";
static char menu_fuzzy3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "變化(<>)";
static char menu_fuzzy4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "增大或不變(>=)";
static char menu_fuzzy5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "減小或不變(<=)";
static char menu_fuzzy6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "不變(=)";
static char menu_fuzzy7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = ">某數";
static char menu_fuzzy8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<某數";
static char menu_fuzzy9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<>某數";
static char menu_fuzzy10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = ">=某數";
static char menu_fuzzy11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<=某數";
static char menu_fuzzy12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "=某數";

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

static char menu_fuzzy_init_manual[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "手動";

static const char * menu_fuzzy_init[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_change1,
	menu_fuzzy_init_manual,
};

static char menu_yes [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "是";
static char menu_no [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "否";

static const char * menu_yesno[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_yes,
	menu_no
};

static char layout_menu_mem1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "導入記憶體";
static char layout_menu_mem2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Dump記憶體";

static const char * layout_menu_mem[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_mem1,
	layout_menu_mem2
};

static char layout_menu_etc1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "設定最高亮度";
static char layout_menu_etc2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "載入PS BIOS字形檔";
static char layout_menu_etc3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PSX記憶卡導入";
static char layout_menu_etc4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PSX記憶卡導出";
static char layout_menu_etc5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "關機";

static const char * layout_menu_etc[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_etc1,
	layout_menu_etc2,
	layout_menu_etc3,
	layout_menu_etc4,
	layout_menu_etc5,	
};

static char layout_menu_save1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "存為CMF";
static char layout_menu_save2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "存為TAB";

static const char * layout_menu_save[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_save1,
	layout_menu_save2,
};

static char layout_menu_load1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "讀取CMF";
static char layout_menu_load2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "讀取TAB";
static char layout_menu_load3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "讀取CW庫";
static char layout_menu_load4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "清除表格";

static const char * layout_menu_load[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_load1,
	layout_menu_load2,
	layout_menu_load3,
	layout_menu_load4
};

static char layout_menu_key1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "按鍵連打";
static char layout_menu_key2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "按鍵映射";
static char layout_menu_key7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "按鍵粘滯";
static char layout_menu_key6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "搖杆映射";
static char layout_menu_key3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "一鍵出招";
static char layout_menu_key4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "讀取設置";
static char layout_menu_key5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "儲存設置";

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

static char menu_conf1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "快捷鍵: [音量+]+[音量+]+[音量+]+[音量+]";
static char menu_conf2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "交換○/×:    是";
static char menu_conf3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "自動載入.tab: 是";
static char menu_conf4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "自動載入.cmf: 是";
static char menu_conf5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "自動載入.set: 是";
static char menu_conf6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "鎖定間隔:     很短";
static char menu_conf7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "啟用截屏功能: 是";
static char menu_conf8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "截屏快捷鍵: [音量+]+[音量+]+[音量+]+[音量+]";
static char menu_conf9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "閱讀文本每行位元組:    ";
static char menu_conf10 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "背景Trgb:        ";
static char menu_conf11 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "字體rgb:      ";
static char menu_conf12 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "截圖格式: bmp";
static char menu_conf13 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "JPG截圖品質: 100";
static char menu_conf14 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "待機快捷鍵: [音量+]+[音量+]+[音量+]+[音量+]";
static char menu_conf15 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "關機快捷鍵:                               ";
static char menu_conf16 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "存檔快捷鍵:                               ";
static char menu_conf17 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "讀檔快捷鍵:                               ";
static char menu_conf18 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "即時讀檔自動關機: 是";
static char menu_conf19 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "更多半透明: 是";
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
"搜索字串"
};
static char view_search_hexstr[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"搜索16進制數值"
};

static char layout_table_str1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "右  更改說明 □鎖定/解鎖 SELECT刪除";
static char layout_table_str2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "↑↓按組移動 △ 查看記憶體 START新增位址";
static char layout_table_str3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "左 全鎖/解鎖 △ 執行";
static char LANG_TABLESUM[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "鎖定  說明";

static char LANG_SEARCH_DIFFHELP[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "按□修改,0為關閉此功能";

static const char * low_high_value_string[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
"最小值",
"最大值",
};

static char keylist_list1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECT刪除配置 △啟動鍵設置";
static char keylist_list2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "左間隔 右詳細 □反向鍵設置";

static char keylist_setkey1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECT刪除設置 △單鍵設置";
static char keylist_setkey2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "左間隔設置 右快速設置";

static char keylist_record1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START結束 SELECT跳過";
static char keylist_record2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "設置開始..";

static char key_symbol1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "□";
static char key_symbol2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "△";
static char key_symbol3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "○";
static char key_symbol4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "×";
static char key_symbol5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "SELECT";
static char key_symbol6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "START";
static char key_symbol7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "上";
static char key_symbol8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "右";
static char key_symbol9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "下";
static char key_symbol10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "左";
static char key_symbol11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "L";
static char key_symbol12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "R";
static char key_symbol13[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "↑";
static char key_symbol14[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "↓";
static char key_symbol15[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "←";
static char key_symbol16[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "→";
static char key_symbol17[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[音量+]";
static char key_symbol18[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[音量-]";
static char key_symbol19[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "∮";
static char key_symbol20[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[亮度]";

static char turbo_key_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"左:連打啟動 右:間隔設置"
};
static char turbo_key_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"START:啟動鍵設置 SEL:默認"
};
static char keymap_str[][17] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )={
"十字鍵搖杆互換",
"搖杆映射到十字鍵",
};
static char turbo_map_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"左:原始鍵設置 右:映射鍵設置"
};
static char turbo_map_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"Sel:刪除"
};
static char turbo_stick_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"左:粘滯啟動 右:相黏鍵設置"
};



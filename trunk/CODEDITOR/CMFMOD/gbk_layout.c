#define LAYOUT_VIEW_ADD "增加%d条地址"
#define LAYOUT_SEARCH_DIFF "\"增大\"\"减小\"值:%d"
#define LAYOUT_KEY_MACRO_LIST "连招%-3d启动键:%-12s反向键:%-6s"
#define LAYOUT_KEY_MACRO_SET "按键%-3d间隔%-6d%-12s"
#define LAYOUT_KEY_MACRO_FASTSET "按键%-9d%-12s"
#define LAYOUT_KEY_TURBO_SET "%-6s:%-4s间隔:%-3d快捷键:%s"
#define LAYOUT_KEY_MAP_SET "%-14s映射为:%-14s"
#define LAYOUT_KEY_STICK_SET "%-18s%-4s快捷键:%s"

static char menu_lockspdstr[][5] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
	"很短",
	"  短",
	"一般",
	"  长",
	"很长"
};

static char menu_img1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PS格式";
static char menu_img2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PNG";
static char menu_img3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "JPG";
static char menu_img4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "截图文件夹";
static const char *layout_menu_img[]  __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_img1,
	menu_img2,
	menu_img3,
	menu_img4,
};

static char menu_main1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "搜索数据";
static char menu_main2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "地址表格";
static char menu_main3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "保存表格";
static char menu_main4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "加载表格";
static char menu_main5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "内存管理";
static char menu_main6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "显示内存";
static char menu_main7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "阅读文本";
static char menu_main8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "频率设定";
static char menu_main9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "选项设置";
static char menu_main10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "英汉词典";
static char menu_main11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "按键管理";
static char menu_main12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "图片攻略";
static char menu_main13[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "其他功能";
static char menu_main14[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "USB 连接";

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
static char menu_search23[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "改变类型";
static char menu_search24[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "改变范围";
static char menu_search25[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "查看结果";
//static char menu_search26[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "改变偏差";

static const char * menu_search2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_search21,
	menu_search22,
	menu_search23,
	menu_search24,
	//menu_search26,
	menu_search25,
};

static char menu_change1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "自动";
static char menu_change2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = " 8位";
static char menu_change3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "16位";
static char menu_change4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "32位";
static char menu_change5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "模糊自动";
static char menu_change6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "模糊 8位";
static char menu_change7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "模糊16位";
static char menu_change8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "模糊32位";
static char menu_change9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "模糊浮点";

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
static char menu_fuzzy2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "减小(<)";
static char menu_fuzzy3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "变化(<>)";
static char menu_fuzzy4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "增大或不变(>=)";
static char menu_fuzzy5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "减小或不变(<=)";
static char menu_fuzzy6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "不变(=)";
static char menu_fuzzy7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = ">某数";
static char menu_fuzzy8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<某数";
static char menu_fuzzy9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<>某数";
static char menu_fuzzy10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = ">=某数";
static char menu_fuzzy11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<=某数";
static char menu_fuzzy12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "=某数";

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

static char menu_fuzzy_init_manual[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "手动";

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

static char layout_menu_mem1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "导入内存";
static char layout_menu_mem2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Dump内存";

static const char * layout_menu_mem[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_mem1,
	layout_menu_mem2
};

static char layout_menu_etc1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "设定最高亮度";
static char layout_menu_etc2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "加载PS BIOS字库";
static char layout_menu_etc3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PSX记忆卡导入";
static char layout_menu_etc4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PSX记忆卡导出";
static char layout_menu_etc5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "关机";

static const char * layout_menu_etc[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_etc1,
	layout_menu_etc2,
	layout_menu_etc3,
	layout_menu_etc4,
	layout_menu_etc5,	
};

static char layout_menu_save1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "存为CMF";
static char layout_menu_save2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "存为TAB";

static const char * layout_menu_save[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_save1,
	layout_menu_save2,
};

static char layout_menu_load1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "读取CMF";
static char layout_menu_load2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "读取TAB";
static char layout_menu_load3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "读取CW库";
static char layout_menu_load4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "清除表格";

static const char * layout_menu_load[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_load1,
	layout_menu_load2,
	layout_menu_load3,
	layout_menu_load4
};

static char layout_menu_key1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "按键连打";
static char layout_menu_key2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "按键映射";
static char layout_menu_key7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "按键粘滞";
static char layout_menu_key6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "摇杆映射";
static char layout_menu_key3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "一键出招";
static char layout_menu_key4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "读取设置";
static char layout_menu_key5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "储存设置";

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

static char menu_conf1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "快捷键: [音量+]+[音量+]+[音量+]+[音量+]";
static char menu_conf2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "交换○/×:    是";
static char menu_conf3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "自动加载.tab: 是";
static char menu_conf4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "自动加载.cmf: 是";
static char menu_conf5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "自动加载.set: 是";
static char menu_conf6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "锁定间隔:     很短";
static char menu_conf7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "启用截屏功能: 是";
static char menu_conf8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "截屏快捷键: [音量+]+[音量+]+[音量+]+[音量+]";
static char menu_conf9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "阅读文本每行字节:    ";
static char menu_conf10 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "背景Trgb:        ";
static char menu_conf11 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "字体rgb:      ";
static char menu_conf12 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "截图格式: bmp";
static char menu_conf13 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "JPG截图质量: 100";
static char menu_conf14 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "待机快捷键: [音量+]+[音量+]+[音量+]+[音量+]";
static char menu_conf15 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "关机快捷键:                               ";
static char menu_conf16 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "存档快捷键:                               ";
static char menu_conf17 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "读档快捷键:                               ";
static char menu_conf18 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "即时读档自动关机: 是";
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
"搜索字符串"
};
static char view_search_hexstr[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"搜索16进制数值"
};

static char layout_table_str1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "右  更改说明 □锁定/解锁 SELECT删除";
static char layout_table_str2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "↑↓按组移动 △ 查看内存 START新增地址";
static char layout_table_str3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "左 全锁/解锁 △ 执行";
static char LANG_TABLESUM[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "锁定  说明";

static char LANG_SEARCH_DIFFHELP[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "按□修改,0为关闭此功能";

static const char * low_high_value_string[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
"最小值",
"最大值",
};

static char keylist_list1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECT删除配置 △启动键设置";
static char keylist_list2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "左间隔 右详细 □反向键设置";

static char keylist_setkey1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECT删除设置 △单键设置";
static char keylist_setkey2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "左间隔设置 右快速设置";

static char keylist_record1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START结束 SELECT跳过";
static char keylist_record2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "设置开始..";

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
"左:连打启动 右:间隔设置"
};
static char turbo_key_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"START:启动键设置 SEL:默认"
};
static char keymap_str[][17] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )={
"十字键摇杆互换",
"摇杆映射到十字键",
};
static char turbo_map_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"左:原始键设置 右:映射键设置"
};
static char turbo_map_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"Sel:删除"
};
static char turbo_stick_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"左:粘滞启动 右:粘滞键设置"
};



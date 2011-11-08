#define LAYOUT_VIEW_ADD "%d�ĥ�-��׷��"
#define LAYOUT_SEARCH_DIFF "\"����\"\"��С\"��:%d"
#define LAYOUT_KEY_MACRO_LIST "�O��%-3d�ܥ���:%-12s  ��ܞ:%-6s"
#define LAYOUT_KEY_MACRO_SET " KEY%-3d�g��%-6d%-12s"
#define LAYOUT_KEY_MACRO_FASTSET " KEY%-9d%-12s"
#define LAYOUT_KEY_TURBO_SET "%-6s:%-4s�g��:%-3d����:  %s"
#define LAYOUT_KEY_MAP_SET "%-14s���  :%-14s"
#define LAYOUT_KEY_STICK_SET "%-18s%-4s�ܥ���:%s"

static char menu_lockspdstr[][5] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
	"���",
	"�̤�",
	"ͨ��",
	"�L��",
	"���L"
};

static char menu_img1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "DOCUMENT";
static char menu_img2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PNG";
static char menu_img3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "JPG";
static char menu_img4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "MS�����x��";
static const char *layout_menu_img[]  __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_img1,
	menu_img2,
	menu_img3,
	menu_img4,
};

static char menu_main1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������";
static char menu_main2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��-�ɱ�  ";
static char menu_main3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��-�ɱ���";
static char menu_main4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��-��׷��";
static char menu_main5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�������";
static char menu_main6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���꾎��";
static char menu_main7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ƥ�����";
static char menu_main8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����å�";
static char menu_main9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "CMF�O�� ";
static char menu_main10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Ӣ�дǵ�";
static char menu_main11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ޥ������";
static char menu_main12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "������E";
static char menu_main13[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�j���Ŀ";
static char menu_main14[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "USB �ӾA";

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

static char menu_search21[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�٥�-��";
static char menu_search22[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��Ҏ��-�� ";
static char menu_search23[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "bit��� ";
static char menu_search24[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "������";
static char menu_search25[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�Y����Ҋ��";

static const char * menu_search2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_search21,
	menu_search22,
	menu_search23,
	menu_search24,
	//menu_search26,
	menu_search25,
};

static char menu_change1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�Ԅ�";
static char menu_change2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = " 8bit";
static char menu_change3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "16bit";
static char menu_change4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "32bit";
static char menu_change5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���^�Ԅ�";
static char menu_change6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���^  8bit";
static char menu_change7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���^ 16bit";
static char menu_change8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���^ 32bit";
static char menu_change9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���^ �g���ȸ���С������";

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

static char menu_fuzzy1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����(>)";
static char menu_fuzzy2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��С(<)";
static char menu_fuzzy3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�仯(<>)";
static char menu_fuzzy4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����(>=)      ";
static char menu_fuzzy5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����(<=)      ";
static char menu_fuzzy6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����(=)";
static char menu_fuzzy7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = ">����";
static char menu_fuzzy8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<����";
static char menu_fuzzy9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<>����";
static char menu_fuzzy10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = ">=����";
static char menu_fuzzy11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<=����";
static char menu_fuzzy12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "=����";

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

static char menu_fuzzy_init_manual[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ք�";

static const char * menu_fuzzy_init[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_change1,
	menu_fuzzy_init_manual,
};

static char menu_yes [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Y ";
static char menu_no [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "N ";

static const char * menu_yesno[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_yes,
	menu_no
};

static char layout_menu_mem1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����ѥå�";
static char layout_menu_mem2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������";

static const char * layout_menu_mem[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_mem1,
	layout_menu_mem2
};

static char layout_menu_etc1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������뤵";
static char layout_menu_etc2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PS BIOS�ե����";
static char layout_menu_etc3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "MCR��-�� �i�z";
static char layout_menu_etc4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "MCR��-�� ����";
static char layout_menu_etc5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�K��";

static const char * layout_menu_etc[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_etc1,
	layout_menu_etc2,
	layout_menu_etc3,
	layout_menu_etc4,
	layout_menu_etc5,	
};

static char layout_menu_save1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "CMF����";
static char layout_menu_save2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "TAB����";

static const char * layout_menu_save[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_save1,
	layout_menu_save2,
};

static char layout_menu_load1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "CMF�i�z";
static char layout_menu_load2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "TAB�i�z";
static char layout_menu_load3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "CWC�i�z ";
static char layout_menu_load4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��-������";

static const char * layout_menu_load[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_load1,
	layout_menu_load2,
	layout_menu_load3,
	layout_menu_load4
};

static char layout_menu_key1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�B���O��";
static char layout_menu_key2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��-���� ";
static char layout_menu_key7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�W���O��";
static char layout_menu_key6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���ʥ�";
static char layout_menu_key3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ޥ���  ";
static char layout_menu_key4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�O���i�z";
static char layout_menu_key5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�O������";

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

static char menu_conf1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����1:  [����+]+[����+]+[����+]+[����+]";
static char menu_conf2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��/�����Q:    Y ";
static char menu_conf3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "TAB�Ԅ��i�z:  Y ";
static char menu_conf4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "CMF�Ԅ��i�z:  Y ";
static char menu_conf5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "SET�Ԅ��i�z:  Y ";
static char menu_conf6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�g���g��:     ���";
static char menu_conf7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���񱣴��Є�: Y ";
static char menu_conf8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���񱣴桡: [����+]+[����+]+[����+]+[����+]";
static char menu_conf9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "TXT���Х�����-��:    ";
static char menu_conf10 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����TRGB:        ";
static char menu_conf11 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����RGB:      ";
static char menu_conf12 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "������ʽ: bmp";
static char menu_conf13 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "JPG����Ʒ�|: 100";
static char menu_conf14 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "���C�ܥ���: [����+]+[����+]+[����+]+[����+]";
static char menu_conf15 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "��Хܥ���:                               ";
static char menu_conf16 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "����-������:                                  ";
static char menu_conf17 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "����-���i�z:                                  ";
static char menu_conf18 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "����-�ȥХ��ر� : Y ";
static char menu_conf19 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "ȫ���͸��: Y ";
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
"TXT��-��  "
};
static char view_search_hexstr[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"16�M����-��   "
};

static char layout_table_str1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ҡ���-�������;���g��/���;SEL ����  ";
static char layout_table_str2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "LR  �����Ƅ�;�� ����;START ��-��׷�� ";
static char layout_table_str3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�� ȫ�g��/���;�� ���Ȥ����g��";
static char LANG_TABLESUM[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�g��  ��-����";

static char LANG_SEARCH_DIFFHELP[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���ǉ��,����0�ǟo��      ";

static const char * low_high_value_string[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
"��Сֵ",
"���ֵ",
};

static char keylist_list1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SEL ����  ���� ���ܥ����O��";
static char keylist_list2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���g�� ��Ԕ�� ����ܞ  �O��";

static char keylist_setkey1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SEL ����  �O�� �������O��";
static char keylist_setkey2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���g������ �������O��";

static char keylist_record1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START�Q�� SEL �w�Ф�";
static char keylist_record2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�O���_ʼ..";

static char key_symbol1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "SELECT";
static char key_symbol6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "START";
static char key_symbol7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "L";
static char key_symbol12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "R";
static char key_symbol13[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol14[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol15[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol16[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol17[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[����+]";
static char key_symbol18[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[����-]";
static char key_symbol19[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "�ߥ�-��";
static char key_symbol20[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[����]";

static char turbo_key_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"��:�B���Є� ��:�ٶ��O��"
};
static char turbo_key_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"START:�ܥ����O�� SEL:����"
};
static char keymap_str[][17] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )={
"ʮ�֥�-���B��",
"ʮ�֥�-�Ƚ��Q",
};
static char turbo_map_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"��:�_ʼ�O��   ��:��-����   "
};
static char turbo_map_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"SEL:����"
};
static char turbo_stick_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"��:�W���Є� ��:�W���O��  "
};


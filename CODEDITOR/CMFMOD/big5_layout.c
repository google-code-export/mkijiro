#define LAYOUT_VIEW_ADD "����%d�l��ַ"
#define LAYOUT_SEARCH_DIFF "\"����\"\"�pС\"ֵ:%d"
#define LAYOUT_KEY_MACRO_LIST "�B��%-3d�����I:%-12s�����I:%-6s"
#define LAYOUT_KEY_MACRO_SET "���I%-3d�g��%-6d%-12s"
#define LAYOUT_KEY_MACRO_FASTSET "���I%-9d%-12s"
#define LAYOUT_KEY_TURBO_SET "%-6s:%-4s�g��:%-3d����I:%s"
#define LAYOUT_KEY_MAP_SET "%-14sӳ���:%-14s"
#define LAYOUT_KEY_STICK_SET "%-18s%-4s����I:%s"

static char menu_lockspdstr[][5] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
	"�ܶ�",
	"  ��",
	"һ��",
	"  �L",
	"���L"
};

static char menu_img1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PS��ʽ";
static char menu_img2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PNG";
static char menu_img3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "JPG";
static char menu_img4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�؈D�ļ��A";
static const char *layout_menu_img[]  __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_img1,
	menu_img2,
	menu_img3,
	menu_img4,
};

static char menu_main1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������";
static char menu_main2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "λַ���";
static char menu_main3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "������";
static char menu_main4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�d����";
static char menu_main5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ӛ���w����";
static char menu_main6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�@ʾӛ���w";
static char menu_main7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��x�ı�";
static char menu_main8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�l���O��";
static char menu_main9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�x��O��";
static char menu_main10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Ӣ�h�~��";
static char menu_main11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���I����";
static char menu_main12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�DƬ����";
static char menu_main13[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������";
static char menu_main14[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "USB �B��";

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

static char menu_search21[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ٴ�����";
static char menu_search22[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������";
static char menu_search23[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��׃���";
static char menu_search24[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��׃����";
static char menu_search25[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�鿴�Y��";
//static char menu_search26[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��׃ƫ��";

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
static char menu_change2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = " 8λ";
static char menu_change3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "16λ";
static char menu_change4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "32λ";
static char menu_change5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ģ���Ԅ�";
static char menu_change6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ģ�� 8λ";
static char menu_change7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ģ��16λ";
static char menu_change8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ģ��32λ";
static char menu_change9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ģ�����c";

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
static char menu_fuzzy2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�pС(<)";
static char menu_fuzzy3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "׃��(<>)";
static char menu_fuzzy4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�����׃(>=)";
static char menu_fuzzy5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�pС��׃(<=)";
static char menu_fuzzy6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��׃(=)";
static char menu_fuzzy7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = ">ĳ��";
static char menu_fuzzy8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<ĳ��";
static char menu_fuzzy9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<>ĳ��";
static char menu_fuzzy10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = ">=ĳ��";
static char menu_fuzzy11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<=ĳ��";
static char menu_fuzzy12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "=ĳ��";

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

static char menu_yes [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��";
static char menu_no [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��";

static const char * menu_yesno[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_yes,
	menu_no
};

static char layout_menu_mem1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����ӛ���w";
static char layout_menu_mem2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Dumpӛ���w";

static const char * layout_menu_mem[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_mem1,
	layout_menu_mem2
};

static char layout_menu_etc1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�O���������";
static char layout_menu_etc2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�d��PS BIOS���Ιn";
static char layout_menu_etc3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PSXӛ��������";
static char layout_menu_etc4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PSXӛ��������";
static char layout_menu_etc5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�P�C";

static const char * layout_menu_etc[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_etc1,
	layout_menu_etc2,
	layout_menu_etc3,
	layout_menu_etc4,
	layout_menu_etc5,	
};

static char layout_menu_save1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���CMF";
static char layout_menu_save2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���TAB";

static const char * layout_menu_save[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_save1,
	layout_menu_save2,
};

static char layout_menu_load1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�xȡCMF";
static char layout_menu_load2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�xȡTAB";
static char layout_menu_load3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�xȡCW��";
static char layout_menu_load4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "������";

static const char * layout_menu_load[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_load1,
	layout_menu_load2,
	layout_menu_load3,
	layout_menu_load4
};

static char layout_menu_key1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���I�B��";
static char layout_menu_key2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���Iӳ��";
static char layout_menu_key7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���Iճ��";
static char layout_menu_key6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�u��ӳ��";
static char layout_menu_key3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "һ�I����";
static char layout_menu_key4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�xȡ�O��";
static char layout_menu_key5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�����O��";

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

static char menu_conf1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����I: [����+]+[����+]+[����+]+[����+]";
static char menu_conf2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���Q��/��:    ��";
static char menu_conf3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�Ԅ��d��.tab: ��";
static char menu_conf4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�Ԅ��d��.cmf: ��";
static char menu_conf5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�Ԅ��d��.set: ��";
static char menu_conf6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�i���g��:     �ܶ�";
static char menu_conf7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���ý�������: ��";
static char menu_conf8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������I: [����+]+[����+]+[����+]+[����+]";
static char menu_conf9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��x�ı�ÿ��λԪ�M:    ";
static char menu_conf10 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����Trgb:        ";
static char menu_conf11 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���wrgb:      ";
static char menu_conf12 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�؈D��ʽ: bmp";
static char menu_conf13 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "JPG�؈DƷ�|: 100";
static char menu_conf14 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "���C����I: [����+]+[����+]+[����+]+[����+]";
static char menu_conf15 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�P�C����I:                               ";
static char menu_conf16 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "��n����I:                               ";
static char menu_conf17 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�x�n����I:                               ";
static char menu_conf18 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "���r�x�n�Ԅ��P�C: ��";
static char menu_conf19 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�����͸��: ��";
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
"�����ִ�"
};
static char view_search_hexstr[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"����16�M�Ɣ�ֵ"
};

static char layout_table_str1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��  �����f�� ���i��/���i SELECT�h��";
static char layout_table_str2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�������M�Ƅ� �� �鿴ӛ���w START����λַ";
static char layout_table_str3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�� ȫ�i/���i �� ����";
static char LANG_TABLESUM[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�i��  �f��";

static char LANG_SEARCH_DIFFHELP[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�����޸�,0���P�]�˹���";

static const char * low_high_value_string[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
"��Сֵ",
"���ֵ",
};

static char keylist_list1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECT�h������ �������I�O��";
static char keylist_list2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���g�� ��Ԕ�� �������I�O��";

static char keylist_setkey1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECT�h���O�� �����I�O��";
static char keylist_setkey2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���g���O�� �ҿ����O��";

static char keylist_record1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START�Y�� SELECT���^";
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
static char key_symbol19[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol20[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[����]";

static char turbo_key_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"��:�B�򆢄� ��:�g���O��"
};
static char turbo_key_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"START:�����I�O�� SEL:Ĭ�J"
};
static char keymap_str[][17] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )={
"ʮ���I�u�˻��Q",
"�u��ӳ�䵽ʮ���I",
};
static char turbo_map_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"��:ԭʼ�I�O�� ��:ӳ���I�O��"
};
static char turbo_map_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"Sel:�h��"
};
static char turbo_stick_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"��:ճ������ ��:����I�O��"
};



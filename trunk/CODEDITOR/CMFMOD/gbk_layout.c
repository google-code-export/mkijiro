#define LAYOUT_VIEW_ADD "����%d����ַ"
#define LAYOUT_SEARCH_DIFF "\"����\"\"��С\"ֵ:%d"
#define LAYOUT_KEY_MACRO_LIST "����%-3d������:%-12s�����:%-6s"
#define LAYOUT_KEY_MACRO_SET "����%-3d���%-6d%-12s"
#define LAYOUT_KEY_MACRO_FASTSET "����%-9d%-12s"
#define LAYOUT_KEY_TURBO_SET "%-6s:%-4s���:%-3d��ݼ�:%s"
#define LAYOUT_KEY_MAP_SET "%-14sӳ��Ϊ:%-14s"
#define LAYOUT_KEY_STICK_SET "%-18s%-4s��ݼ�:%s"

static char menu_lockspdstr[][5] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
	"�ܶ�",
	"  ��",
	"һ��",
	"  ��",
	"�ܳ�"
};

static char menu_img1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PS��ʽ";
static char menu_img2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PNG";
static char menu_img3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "JPG";
static char menu_img4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��ͼ�ļ���";
static const char *layout_menu_img[]  __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_img1,
	menu_img2,
	menu_img3,
	menu_img4,
};

static char menu_main1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������";
static char menu_main2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��ַ���";
static char menu_main3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "������";
static char menu_main4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���ر��";
static char menu_main5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ڴ����";
static char menu_main6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��ʾ�ڴ�";
static char menu_main7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�Ķ��ı�";
static char menu_main8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Ƶ���趨";
static char menu_main9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ѡ������";
static char menu_main10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Ӣ���ʵ�";
static char menu_main11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������";
static char menu_main12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ͼƬ����";
static char menu_main13[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������";
static char menu_main14[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "USB ����";

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
static char menu_search23[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ı�����";
static char menu_search24[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ı䷶Χ";
static char menu_search25[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�鿴���";
//static char menu_search26[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ı�ƫ��";

static const char * menu_search2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_search21,
	menu_search22,
	menu_search23,
	menu_search24,
	//menu_search26,
	menu_search25,
};

static char menu_change1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�Զ�";
static char menu_change2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = " 8λ";
static char menu_change3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "16λ";
static char menu_change4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "32λ";
static char menu_change5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ģ���Զ�";
static char menu_change6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ģ�� 8λ";
static char menu_change7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ģ��16λ";
static char menu_change8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ģ��32λ";
static char menu_change9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ģ������";

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
static char menu_fuzzy4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����򲻱�(>=)";
static char menu_fuzzy5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��С�򲻱�(<=)";
static char menu_fuzzy6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����(=)";
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

static char menu_fuzzy_init_manual[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ֶ�";

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

static char layout_menu_mem1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�����ڴ�";
static char layout_menu_mem2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "Dump�ڴ�";

static const char * layout_menu_mem[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_mem1,
	layout_menu_mem2
};

static char layout_menu_etc1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�趨�������";
static char layout_menu_etc2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����PS BIOS�ֿ�";
static char layout_menu_etc3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PSX���俨����";
static char layout_menu_etc4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PSX���俨����";
static char layout_menu_etc5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ػ�";

static const char * layout_menu_etc[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_etc1,
	layout_menu_etc2,
	layout_menu_etc3,
	layout_menu_etc4,
	layout_menu_etc5,	
};

static char layout_menu_save1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��ΪCMF";
static char layout_menu_save2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��ΪTAB";

static const char * layout_menu_save[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_save1,
	layout_menu_save2,
};

static char layout_menu_load1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��ȡCMF";
static char layout_menu_load2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��ȡTAB";
static char layout_menu_load3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��ȡCW��";
static char layout_menu_load4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "������";

static const char * layout_menu_load[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_load1,
	layout_menu_load2,
	layout_menu_load3,
	layout_menu_load4
};

static char layout_menu_key1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������";
static char layout_menu_key2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����ӳ��";
static char layout_menu_key7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����ճ��";
static char layout_menu_key6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ҡ��ӳ��";
static char layout_menu_key3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "һ������";
static char layout_menu_key4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��ȡ����";
static char layout_menu_key5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��������";

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

static char menu_conf1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��ݼ�: [����+]+[����+]+[����+]+[����+]";
static char menu_conf2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "������/��:    ��";
static char menu_conf3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�Զ�����.tab: ��";
static char menu_conf4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�Զ�����.cmf: ��";
static char menu_conf5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�Զ�����.set: ��";
static char menu_conf6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�������:     �ܶ�";
static char menu_conf7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���ý�������: ��";
static char menu_conf8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "������ݼ�: [����+]+[����+]+[����+]+[����+]";
static char menu_conf9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�Ķ��ı�ÿ���ֽ�:    ";
static char menu_conf10 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����Trgb:        ";
static char menu_conf11 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����rgb:      ";
static char menu_conf12 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "��ͼ��ʽ: bmp";
static char menu_conf13 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "JPG��ͼ����: 100";
static char menu_conf14 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "������ݼ�: [����+]+[����+]+[����+]+[����+]";
static char menu_conf15 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�ػ���ݼ�:                               ";
static char menu_conf16 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�浵��ݼ�:                               ";
static char menu_conf17 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "������ݼ�:                               ";
static char menu_conf18 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "��ʱ�����Զ��ػ�: ��";
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
"�����ַ���"
};
static char view_search_hexstr[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"����16������ֵ"
};

static char layout_table_str1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��  ����˵�� ������/���� SELECTɾ��";
static char layout_table_str2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���������ƶ� �� �鿴�ڴ� START������ַ";
static char layout_table_str3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�� ȫ��/���� �� ִ��";
static char LANG_TABLESUM[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����  ˵��";

static char LANG_SEARCH_DIFFHELP[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�����޸�,0Ϊ�رմ˹���";

static const char * low_high_value_string[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
"��Сֵ",
"���ֵ",
};

static char keylist_list1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECTɾ������ ������������";
static char keylist_list2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���� ����ϸ �����������";

static char keylist_setkey1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SELECTɾ������ ����������";
static char keylist_setkey2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�������� �ҿ�������";

static char keylist_record1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START���� SELECT����";
static char keylist_record2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���ÿ�ʼ..";

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
"��:�������� ��:�������"
};
static char turbo_key_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"START:���������� SEL:Ĭ��"
};
static char keymap_str[][17] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )={
"ʮ�ּ�ҡ�˻���",
"ҡ��ӳ�䵽ʮ�ּ�",
};
static char turbo_map_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"��:ԭʼ������ ��:ӳ�������"
};
static char turbo_map_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"Sel:ɾ��"
};
static char turbo_stick_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"��:ճ������ ��:ճ�ͼ�����"
};



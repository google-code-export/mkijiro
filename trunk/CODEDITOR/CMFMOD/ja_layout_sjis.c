#define LAYOUT_VIEW_ADD "%d���[�ǒǉ�"
#define LAYOUT_SEARCH_DIFF "\"�ő�\"\"�ŏ�\"�l:%d"
#define LAYOUT_KEY_MACRO_LIST "�ݒ�%-3d�ڂ���:%-12s  ���]:%-6s"
#define LAYOUT_KEY_MACRO_SET " KEY%-3d�Ԋu%-6d%-12s"
#define LAYOUT_KEY_MACRO_FASTSET " KEY%-9d%-12s"
#define LAYOUT_KEY_TURBO_SET "%-6s:%-4s�Ԋu:%-3d�N��:  %s"
#define LAYOUT_KEY_MAP_SET "%-14s�ύX  :%-14s"
#define LAYOUT_KEY_STICK_SET "%-18s%-4s�ڂ���:%s"

static char menu_lockspdstr[][5] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
	"�ŒZ",
	"�Z��",
	"�ʏ�",
	"����",
	"�Œ�"
};

static char menu_img1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "DOCUMENT";
static char menu_img2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PNG";
static char menu_img3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "JPG";
static char menu_img4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "MS����I��";
static const char *layout_menu_img[]  __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_img1,
	menu_img2,
	menu_img3,
	menu_img4,
};

static char menu_main1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���l����";
static char menu_main2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���[�Ǖ\\  ";
static char menu_main3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���[�Ǖۑ�";
static char menu_main4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���[�ǒǉ�";
static char menu_main5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�߂���Ǘ�";
static char menu_main6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�߂���ҏW";
static char menu_main7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�Ă�����";
static char menu_main8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�������";
static char menu_main9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "CMF�ݒ� ";
static char menu_main10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�p�����T";
static char menu_main11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�܂���Ǘ�";
static char menu_main12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�摜�{��";
static char menu_main13[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�G�p����";
static char menu_main14[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "USB �ڑ�";

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

static char menu_search21[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�Ă��[��";
static char menu_search22[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�V�K���[�� ";
static char menu_search23[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "bit�ύX ";
static char menu_search24[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�͈͕ύX";
static char menu_search25[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���ʂ�����";

static const char * menu_search2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	menu_search21,
	menu_search22,
	menu_search23,
	menu_search24,
	//menu_search26,
	menu_search25,
};

static char menu_change1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����";
static char menu_change2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = " 8bit";
static char menu_change3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "16bit";
static char menu_change4[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "32bit";
static char menu_change5[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��r����";
static char menu_change6[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��r  8bit";
static char menu_change7[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��r 16bit";
static char menu_change8[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��r 32bit";
static char menu_change9[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��r �P���x���������_��";

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
static char menu_fuzzy2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�k��(<)";
static char menu_fuzzy3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ω�(<>)";
static char menu_fuzzy4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ȏ�(>=)      ";
static char menu_fuzzy5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ȉ�(<=)      ";
static char menu_fuzzy6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�s��(=)";
static char menu_fuzzy7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = ">���l";
static char menu_fuzzy8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<���l";
static char menu_fuzzy9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<>���l";
static char menu_fuzzy10[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = ">=���l";
static char menu_fuzzy11[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "<=���l";
static char menu_fuzzy12[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "=���l";

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

static char menu_fuzzy_init_manual[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�蓮";

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

static char layout_menu_mem1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�߂���ς���";
static char layout_menu_mem2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�߂��肾���";
static char layout_menu_mem3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�K�q/MPS�����";


static const char * layout_menu_mem[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_mem1,
	layout_menu_mem2,
	layout_menu_mem3
};

static char layout_menu_etc1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��ʂ̖��邳";
static char layout_menu_etc2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "PS BIOS�ӂ����";
static char layout_menu_etc3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "MCR�Ł[�� �Ǎ�";
static char layout_menu_etc4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "MCR�Ł[�� ���o";
static char layout_menu_etc5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�I��";

static const char * layout_menu_etc[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_etc1,
	layout_menu_etc2,
	layout_menu_etc3,
	layout_menu_etc4,
	layout_menu_etc5,	
};

static char layout_menu_save1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "CMF�ۑ�";
static char layout_menu_save2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "TAB�ۑ�";

static const char * layout_menu_save[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_save1,
	layout_menu_save2,
};

static char layout_menu_load1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "CMF�Ǎ�";
static char layout_menu_load2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "TAB�Ǎ�";
static char layout_menu_load3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "CWC�Ǎ� ";
static char layout_menu_load4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���[�Ǎ폜";

static const char * layout_menu_load[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =
{
	layout_menu_load1,
	layout_menu_load2,
	layout_menu_load3,
	layout_menu_load4
};

static char layout_menu_key1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�A�Őݒ�";
static char layout_menu_key2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���[�z�u ";
static char layout_menu_key7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�x���ݒ�";
static char layout_menu_key6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���Ȃ낮";
static char layout_menu_key3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�܂���  ";
static char layout_menu_key4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ݒ�Ǎ�";
static char layout_menu_key5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ݒ�ۑ�";

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

static char menu_conf1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�N��1:  [����+]+[����+]+[����+]+[����+]";
static char menu_conf2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "��/�~����:    Y ";
static char menu_conf3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "TAB�����Ǎ�:  Y ";
static char menu_conf4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "CMF�����Ǎ�:  Y ";
static char menu_conf5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "SET�����Ǎ�:  Y ";
static char menu_conf6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���s�Ԋu:     �ŒZ";
static char menu_conf7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�摜�ۑ��L��: Y ";
static char menu_conf8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�摜�ۑ� : [����+]+[����+]+[����+]+[����+]";
static char menu_conf9 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "TXT�̍s������[��:    ";
static char menu_conf10 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�w�iTRGB:        ";
static char menu_conf11 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "����RGB:      ";
static char menu_conf12 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�摜�`��: bmp";
static char menu_conf13 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "JPG�摜�i��: 100";
static char menu_conf14 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�ҋ@�ڂ���: [����+]+[����+]+[����+]+[����+]";
static char menu_conf15 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�d�؂ڂ���:                               ";
static char menu_conf16 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "���ā[�ƍ쐬:                                  ";
static char menu_conf17 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "���ā[�ƓǍ�:                                  ";
static char menu_conf18 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "���ā[�Ƃ΂���� : Y ";
static char menu_conf19 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )= "�S�̔�����: Y ";
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
"TXT���[��  "
};
static char view_search_hexstr[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"16�i�����[��   "
};

static char layout_table_str1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�E ���[�ǖ��ύX;�����s/����;SEL �폜  ";
static char layout_table_str2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "LR  �㉺�ړ�;�� �߂���;START ���[�ǒǉ� ";
static char layout_table_str3[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�� �S���s/����;�� 1�x�������s";
static char LANG_TABLESUM[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���s  ���[�ǖ�";

static char LANG_SEARCH_DIFFHELP[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���ŕύX,�l��0�Ŗ���      ";

static const char * low_high_value_string[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ={
"�ŏ�?",
"�ő�?",
};

static char keylist_list1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SEL �폜  �z�u ���ڂ���ݒu";
static char keylist_list2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���Ԋu �E�ڍ� �����]  �ݒu";

static char keylist_setkey1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "SEL �폜  �ݒu ��??�ݒu";
static char keylist_setkey2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "���Ԋu?�u �E���͐ݒ�";

static char keylist_record1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "START���� SEL ��΂�";
static char keylist_record2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "�ݒu�J�n..";

static char key_symbol1 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol2 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol3 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol4 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "�~";
static char key_symbol5 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "SELECT";
static char key_symbol6 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "START";
static char key_symbol7 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "��";
static char key_symbol8 [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "�E";
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
static char key_symbol19[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "�݂�[��";
static char key_symbol20[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = "[���]";

static char turbo_key_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"��:�A�ŗL�� �E:���x�ݒ�"
};
static char turbo_key_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"START:�ڂ���ݒu SEL:����"
};
static char keymap_str[][17] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )={
"�\�����[���A��",
"�\�����[�ƌ���",
};
static char turbo_map_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"��:�J�n�ݒu   �E:���[�z�u   "
};
static char turbo_map_help2[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"SEL:�폜"
};
static char turbo_stick_help1[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )  = {
"��:�x���L�� �E:�x���ݒ�  "
};


/*
 * This file is part of PRO CFW.

 * PRO CFW is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.

 * PRO CFW is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with PRO CFW. If not, see <http://www.gnu.org/licenses/ .
 */

#include "systemctrl.h"
#include "systemctrl_se.h"
#include "utils.h"
#include "main.h"

const char * g_messages_en[] = {
"PRO ض����ƭ�",
"Ҳ��ƭ�",
"·�ƽ��",
"ϴ�����",
"����",
"ˮ��ޭ�",
"ճ��",
"Ѻ�",
"USB �¿޸",
"USB �¿޸�ϼ�",
"USB ����ݼϼ�",
"��ò��Ӹ",
"ذ�ޮݷ޿�",
"ض��ذ̫��",
"ISO Ӱ��",
"³�ޮ�",
"M33��ײ��-",
"Sony NP9660",
"Inferno",
"USB�¿޸��",
"�ׯ�� 0",
"�ׯ�� 1",
"�ׯ�� 2",
"�ׯ�� 3",
"UMD�ި��",
"USB�¿޸�޼ޭ���ݽ�",
"PSP1000�޳�����ʲ������¶�",
"HTML��׳�����ݲ��޼��ο���޷�ֳƽ�",
"MAC���ڽĶ��",
"��޳��ɿưۺަ����߽�",
"�ް��ް�ɽ����",
"PIC*.PNG�ˮ��޼Ų",
"USB�¿޸���ׯ��ɶ����޳�",
"sepluginsƱ�version.txt�¶�",
"sepluginsƱ�usbversion.txt�¶�",
"���ѱ����ްĻ��ް�¶�",
"����������̧��ɻ��ޮ�޳�(PSP-GO���ò)",
"����ž�ò",
"XMB ��׸޲�",
"GAME��׸޲�",
"POPS��׸޲�",
"DRM�Ѻ�ƽ�",
"CFW�����̧�٦ˮ��޼Ų",
"��۸�ƭ�خ��Ѻ�ƽ�",
"�ٲ��׸޲ݻ�߰�(PSP-GO���ò)",
"Inferno & NP9660 ISO������¶�",
"Inferno & NP9660 ���������(MiB)",
"Inferno & NP9660 �����No.",
"Inferno & NP9660 �����γ��",
"ISO̧��Ҳɾ�ݶ�����",
"��ذ�è��ɿ��޺��ޮ�",
"�ֳ�Ų",
"POP",
"GAME",
"VSH",
"POP & GAME",
"GAME & VSH",
"VSH & POP",
"���÷ֳ",
"CPU ����",
"XMB CPU/BUS",
"GAME CPU/BUS",
"��׸޲�",
"����Ų�޳���",
"���-�è��",
"����Ų�޳������׸޲ݾ�ò",
"��ؽè�����׸޲ݾ�ò",
"ڼ޽�ش�ި�",
"WMA�ճ��Ƽϼ�",
"�ׯ���ճ��Ƽϼ�",
"���ݺ��ݼϼ�",
"��ò����: X",
"��ò����: O",
"WMAճ��",
"�ׯ��ճ��",
"O/X���ݺ���",
"O/X���ݺ���(�ݴ��������VSH����޳����ֳ)",
"������̧�٦���ޮ",
"������̧�ٶ޻��ޮ��ϼ�",
"/PSP/GAME/RECOVERY/EBOOT.PBP ��ޯ��",
"PSP��ݹ�ݦ�Ľ",
"PSP����ޮ���",
"PSP����޳",
"VSH����޳",
"�߰��",
"���",
"��ض",
"ְۯ��",
"�ݺ�",
"���ؽ",
"ҷ��",
"�����ر",
"�ݺ�",
"����",
"ۼ�",
"����޸",
"���ޯ������I",
"���ޯ������II",
"��������ɷ������߰�",
"sepluginsƱ�mac.txt�¶�",
};

u8 message_test_en[NELEMS(g_messages_en) == MSG_END ? 0 : -1];

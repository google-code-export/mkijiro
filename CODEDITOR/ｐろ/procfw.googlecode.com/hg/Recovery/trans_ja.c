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
"PRO Ø¶ÊÞØÒÆ­°",
"Ò²ÝÒÆ­°",
"Â·ÞÆ½½Ñ",
"Ï´ÆÓÄÞÙ",
"ÓÄÞÙ",
"Ë®³¼Þ­Ý",
"Õ³º³",
"Ñº³",
"USB ¾Â¿Þ¸",
"USB ¾Â¿Þ¸¼Ï¼À",
"USB ¾ÂÀÞÝ¼Ï¼À",
"¾¯Ã²º³Ó¸",
"Ø°¼Þ®Ý·Þ¿³",
"Ø¶ÊÞØ°Ì«ÝÄ",
"ISO Ó°ÄÞ",
"Â³¼Þ®³",
"M33ÄÞ×²ÊÞ-",
"Sony NP9660",
"Inferno",
"USB¾Â¿Þ¸»·",
"Ì×¯¼­ 0",
"Ì×¯¼­ 1",
"Ì×¯¼­ 2",
"Ì×¯¼­ 3",
"UMDÃÞ¨½¸",
"USB¾Â¿Þ¸¼Þ¼Þ­³ÃÞÝ½Ù",
"PSP1000ÃÞ³½¶ÞÀÉÊ²¹²¼®¸¦Â¶³",
"HTMLÌÞ×³»ÞÃÞÆÝ²ÉÊÞ¼®ÆÎ¿ÞÝÃÞ·ÙÖ³Æ½Ù",
"MAC±ÄÞÚ½Ä¶¸½",
"·ÄÞ³¼ÞÉ¿Æ°ÛºÞ¦½·¯Ìß½Ù",
"¹Þ°ÑÌÞ°ÄÉ½·¯Ìß",
"PIC*.PNG¦Ë®³¼Þ¼Å²",
"USB¾Â¿Þ¸¼ÞÌ×¯¼­É¶·¶´ÎÞ³¼",
"sepluginsÆ±Ùversion.txt¦Â¶³",
"sepluginsÆ±Ùusbversion.txt¦Â¶³",
"¶½ÀÑ±¯ÌßÃÞ°Ä»°ÊÞ°¦Â¶³",
"²Á¼ÞÁ­³ÀÞÝÌ§²ÙÉ»¸¼Þ®ÎÞ³¼(PSP-GO¹ÞÝÃ²)",
"º³ÄÞÅ¾¯Ã²",
"XMB Ìß×¸Þ²Ý",
"GAMEÌß×¸Þ²Ý",
"POPSÌß×¸Þ²Ý",
"DRM¦Ñº³Æ½Ù",
"CFW¶ÝÚÝÉÌ§²Ù¦Ë®³¼Þ¼Å²",
"±ÅÛ¸ÞÆ­³Ø®¸¦Ñº³Æ½Ù",
"ÌÙ²Ìß×¸Þ²Ý»Îß°Ä(PSP-GO¹ÞÝÃ²)",
"Inferno & NP9660 ISO·¬¯¼­¦Â¶³",
"Inferno & NP9660 ·¬¯¼­»²½Þ(MiB)",
"Inferno & NP9660 ·¬¯¼­No.",
"Inferno & NP9660 ·¬¯¼­Î³¼Ý",
"ISOÌ§²ÙÒ²É¾ÞÝ¶¸À²µ³",
"ÒÓØ°½Ã¨¯¸É¿¸ÄÞº³¼Þ®³",
"¼Ö³¼Å²",
"POP",
"GAME",
"VSH",
"POP & GAME",
"GAME & VSH",
"VSH & POP",
"ÂÈÆÃ·Ö³",
"CPU ¿¸ÄÞ",
"XMB CPU/BUS",
"GAME CPU/BUS",
"Ìß×¸Þ²Ý",
"ÎÝÀ²Å²¿Þ³ÒÓØ",
"ÒÓØ-½Ã¨¯¸",
"ÎÝÀ²Å²¿Þ³ÒÓØÉÌß×¸Þ²Ý¾¯Ã²",
"ÒÓØ½Ã¨¯¸ÉÌß×¸Þ²Ý¾¯Ã²",
"Ú¼Þ½ÄØ´ÃÞ¨À",
"WMA¦Õ³º³Æ¼Ï¼À",
"Ì×¯¼­¦Õ³º³Æ¼Ï¼À",
"ÎÞÀÝº³¶Ý¼Ï¼À",
"¹¯Ã²ÎÞÀÝ: X",
"¹¯Ã²ÎÞÀÝ: O",
"WMAÕ³º³",
"Ì×¯¼­Õ³º³",
"O/XÎÞÀÝº³¶Ý",
"O/XÎÞÀÝº³¶Ý(ÊÝ´²»ÚÙÀÒÆÊVSH»²·ÄÞ³¶ÞËÂÖ³)",
"Á­³ÀÞÝÌ§²Ù¦»¸¼Þ®",
"Á­³ÀÞÝÌ§²Ù¶Þ»¸¼Þ®»ÚÏ¼À",
"/PSP/GAME/RECOVERY/EBOOT.PBP ¦¼Þ¯º³",
"PSPÃÞÝ¹ÞÝ¦µÄ½",
"PSPÀ²·¼Þ®³À²",
"PSP»²·ÄÞ³",
"VSH»²·ÄÞ³",
"Íß°¼Þ",
"ÆÎÝ",
"±ÒØ¶",
"Ö°Û¯Êß",
"¶Ýº¸",
"²·ÞØ½",
"Ò·¼º",
"µ°½Ä×Ø±",
"ÎÝºÝ",
"À²ÜÝ",
"Û¼±",
"Á­³ºÞ¸",
"ÃÞÊÞ¯¸ÞÀ²ÌßI",
"ÃÞÊÞ¯¸ÞÀ²ÌßII",
"¶¸Á®³ÒÓØÉ·®³¾²»Îß°Ä",
"sepluginsÆ±Ùmac.txt¦Â¶³",
};

u8 message_test_en[NELEMS(g_messages_en) == MSG_END ? 0 : -1];

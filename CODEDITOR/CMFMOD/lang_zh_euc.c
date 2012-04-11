/* 
 * Copyright (C) 2006 aeolusc <soarchin@gmail.com>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of
 * the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */
#include <pspctrl.h>
#include "lang_zh_euc.h"
#include "ja_lang_euc.c"

char DICT_IDX_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =  "ms0:/CheatMaster/DCT/en.idx";
char DICT_DCT_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) =  "ms0:/CheatMaster/DCT/en.dct";

//font.c
char FONT_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/CheatMaster/font_euc.dat";

//ui.c
char ISOFS_UMDDATA[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="disc0:/UMD_DATA.BIN";
char ISOFS_SFO[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) ="disc0:/PSP_GAME/PARAM.SFO";

//mem.c
char MODULE_DIR[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   )="ms0:/PICTURE/CWC";
char TAB_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/PICTURE/CWC/TAB";
char CMF_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/PICTURE/CWC/CMF";
char MEM_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/CheatMaster/MEM";
char SET_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/PICTURE/CWC/SET";
char MCR_DIR [] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/CheatMaster/MCR";
char IME_DIR[] __attribute__(   (  aligned( 1 ), section( ".data" )  )   ) = "ms0:/CheatMaster/ime_euc.dat";

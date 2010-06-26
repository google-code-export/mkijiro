//MODED BY HAROTUBO
//Mips.h
#define S 0
#define T 1
#define D 2
unsigned char VFMODE=0;
unsigned char VFR=0;
unsigned char mipsNum[16];
unsigned char *mipsRegisterArray[]={"zero", "at", "v0", "v1", "a0", "a1", "a2", "a3", "t0", "t1", "t2", "t3", "t4", "t5", "t6", "t7", "s0", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "t8", "t9", "k0", "k1", "gp", "sp", "s8", "ra"};
unsigned char *specialRegisterArray[]={ "ixltg", "ixldt", "bxlbt", "$3", "ixstg", "ixsdt", "bxsbt", "ixin", "bpr", "$9", "bhinbt", "ihin", "bfh", "$13", "ifl", "$15", "dxltg", "dxldt", "dxstg", "dxsdt", "dxwbin", "$21", "dxin", "$23", "dhwbinv", "$25", "dhin", "$27", "dhwoin", "$29", "$30", "$31" };
unsigned char *cop0Array[]={
"INDEX",     "RANDOM",    "ENTRYLO0",  "ENTRYLO1",
"CONTEXT",   "PAGEMASK",  "WIRED",     "7",
"BADVADDR",  "COUNT",     "ENTRYHI",   "COMPARE",
"STATUS",    "CAUSE",     "EPC",       "PRID",
"CONFIG",    "LLADDR",    "WATCHLO",   "WATCHHI",
"XCONTEXT",  "21",          "22",      "DEBUG",
"DEPC",      "PERFCNT",   "ERRCTL",    "CACHEERR",
"TAGLO",     "TAGHI",     "ERROREPC",  "DESAVE",
};
unsigned char *DrArray[]={
"DRCNTL", "DEPC", "DDATA0", "DDATA1", "IBC", "DBC", "6", "7",
"IBA", "IBAM", "10", "11", "DBA", "DBAM", "DBD", "DBDM"
};
unsigned char *VFPUArray[]={
"PFXS", "PFXT", "PFXD", "CC" ,"INF4", "5" ,"6" ,"REV",
"RCX0","RCX1","RCX2","RCX3","RCX4","RCX5","RCX6","RCX7"
};
unsigned char *VFPUCMPArray[]={
"FL","EQ","LT","LE","TR","NE","GE","GT","EZ","EN","EI","ES","NZ","NN","NI","NS"
};
unsigned char *VFPUSTArray[]={
"0","HUGE","SQRT2","SQRT1_2","2_SQRTPI","2_PI","1_PI","PI_4",
"PI_2","PI","E","LOG2E","LOG10E","LN2","LN10","2PI",
"PI_6","LOG10TWO","LOG2TEN","SQRT3_2"
};


void colorRegisters(unsigned int a_opcode)
{
	a_opcode=a_opcode % 0x20;
	switch(a_opcode)
	{
		case 0x00: pspDebugScreenSetTextColor(0xFFBBBBBB); break; // ZERO
		case 0x01: pspDebugScreenSetTextColor(0xFFCCCCCC); break; //AT
		case 0x02: pspDebugScreenSetTextColor(0xFFFFFF00); break; // V0
		case 0x03: pspDebugScreenSetTextColor(0xFFBBBB00); break; // V1
		case 0x04: pspDebugScreenSetTextColor(0xFFEE0000); break; //A0
		case 0x05: pspDebugScreenSetTextColor(0xFFBB0000); break; //A1
		case 0x06: pspDebugScreenSetTextColor(0xFF880000); break; //A2
		case 0x07: pspDebugScreenSetTextColor(0xFF550000); break; //A3
		case 0x08: pspDebugScreenSetTextColor(0xFF00C010); break;//  T0
		case 0x09: pspDebugScreenSetTextColor(0xFF00B020); break;//  T1
		case 0x0A: pspDebugScreenSetTextColor(0xFF00A030); break;//  T2
		case 0x0B: pspDebugScreenSetTextColor(0xFF009040); break;//  T3
		case 0x0C: pspDebugScreenSetTextColor(0xFF008050); break;//  T4
		case 0x0D: pspDebugScreenSetTextColor(0xFF007060); break;//  T5
		case 0x0E: pspDebugScreenSetTextColor(0xFF006070); break;//  T6
		case 0x0F: pspDebugScreenSetTextColor(0xFF005080); break;//  T7
		case 0x10: pspDebugScreenSetTextColor(0xFF1000F0); break;// S0
		case 0x11: pspDebugScreenSetTextColor(0xFF2000E0); break;// S1
		case 0x12: pspDebugScreenSetTextColor(0xFF3000D0); break;// S2
		case 0x13: pspDebugScreenSetTextColor(0xFF4000C0); break;// S3
		case 0x14: pspDebugScreenSetTextColor(0xFF5000B0); break;// S4
		case 0x15: pspDebugScreenSetTextColor(0xFF6000A0); break;// S5
		case 0x16: pspDebugScreenSetTextColor(0xFF700090); break;// S6
		case 0x17: pspDebugScreenSetTextColor(0xFF800080); break;// S7
		case 0x18: pspDebugScreenSetTextColor(0xFF004090); break;//  T8
		case 0x19: pspDebugScreenSetTextColor(0xFF0030A0); break;//  T9
		case 0x1A: pspDebugScreenSetTextColor(0xFF0000B0); break;// KO
		case 0x1B: pspDebugScreenSetTextColor(0xFF0000D0); break;// K1
		case 0x1C: pspDebugScreenSetTextColor(0xFF008888); break;//  GP
		case 0x1D: pspDebugScreenSetTextColor(0xFF00BBBB); break;//  SP
		case 0x1E: pspDebugScreenSetTextColor(0xFF900070); break;// S8
		case 0x1F: pspDebugScreenSetTextColor(0xFF00FFFF); break;//  RA
  	}
}

void floatRegister(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  colorRegisters(a_opcode);
  sprintf(buffer, "f%d" ,a_opcode); pspDebugScreenPuts(buffer);
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}

void vectors(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  if(VFMODE==1){
	pspDebugScreenPuts("VFPU_");
  	if((a_opcode & 0x7F) > 0xF){
  	sprintf(buffer, "%d",(a_opcode &0x7F));
	}
	else{
	a_opcode&=0x7F;
        sprintf(buffer, "%s", VFPUArray[a_opcode]);
	}
  }
  else if(VFMODE==3){
	pspDebugScreenPuts("VFPU_");
  	if(((a_opcode >>16) & 0x1F) > 0x13){
  	sprintf(buffer, "%d",(a_opcode >>16) & 0x1F);
	}
	else{
	a_opcode=(a_opcode >>16) & 0x1F ;
        sprintf(buffer, "%s", VFPUSTArray[a_opcode]);
	}
  }
  else if(VFMODE==2){
  sprintf(buffer, "%d",(a_opcode>>18)& 0x7 );
  }
  else{
	if(VFR==1){
	if(((a_opcode & 0x1) == 1) && ((a_opcode >>24 == 0xD4) || (a_opcode >>24 == 0xD8) || (a_opcode >>24 == 0xF4) || (a_opcode >>24 == 0xF8))){
  	sprintf(buffer, "R%d0%d",((a_opcode>>(8*(2-a_slot)))&0x1F)/4 , (a_opcode>>(8*(2-a_slot)))& 0x3 );
	a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F) + (a_opcode & 0x3);
		}
	else{
	sprintf(buffer, "C%d%d0",((a_opcode>>(8*(2-a_slot)))&0x1F)/4 , (a_opcode>>(8*(2-a_slot)))& 0x3 );
	a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F);
		}
	}
/*	else if(VFR==2){
  	sprintf(buffer, "E%d%d%d",((a_opcode>>(8*(2-a_slot)))>>2)& 0x7 , (a_opcode>>(8*(2-a_slot)))& 0x3 , (a_opcode>>(8*(2-a_slot))&0x7F)>>5);
  	sprintf(buffer, "M%d%d%d",((a_opcode>>(8*(2-a_slot)))>>2)& 0x7 , (a_opcode>>(8*(2-a_slot)))& 0x3 , (a_opcode>>(8*(2-a_slot))&0x7F)>>5);
	}*/
	else if(VFR==3){
	sprintf(buffer, "S%d%d%d" , (a_opcode >> 18) & 7, (a_opcode >> 16) & 3, a_opcode & 3);
	a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F) + (a_opcode & 0x3);
	}
	else{
  	sprintf(buffer, "S%d%d%d",((a_opcode>>(8*(2-a_slot)))>>2)& 0x7 , (a_opcode>>(8*(2-a_slot)))& 0x3 , (a_opcode>>(8*(2-a_slot))&0x7F)>>5);
        a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F) + ((a_opcode>>(8*(2-a_slot))&0x7F)>>5);
	}
  colorRegisters(a_opcode);
  }
  VFR=0;
  VFMODE=0;
  pspDebugScreenPuts(buffer);
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}

void specialRegister(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  
  colorRegisters(a_opcode);
  if(a_opcode != 0x03 || 0x09 || 0x0D || 0x0F || 0x15 || 0x17 || 0x19 || 0x1B || 0x1D || 0x1E || 0x1F){
  	pspDebugScreenPuts(specialRegisterArray[a_opcode]);
  }
  else{
  	pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$"); pspDebugScreenSetTextColor(color02);
  	sprintf(buffer, "%2d", a_opcode); pspDebugScreenPuts(buffer);
  }
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}

void VectorCMP(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode&=0xF;
  colorRegisters(a_opcode);
  pspDebugScreenPuts(VFPUCMPArray[a_opcode]);
  if(a_more) pspDebugScreenPuts(", ");
}

void mipsRegister(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  colorRegisters(a_opcode);
  pspDebugScreenPuts(mipsRegisterArray[a_opcode]);
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}

void cop0Register(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  pspDebugScreenPuts("COP0_");
  pspDebugScreenPuts(cop0Array[a_opcode]);
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}

void DrRegister(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x0F;
  pspDebugScreenPuts("DEBUG_");
  pspDebugScreenPuts(DrArray[a_opcode]);
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}


void mipsNibble(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  pspDebugScreenSetTextColor(0xFF999999);
  sprintf(mipsNum, "$%D", a_opcode);
  pspDebugScreenPuts(mipsNum);
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}


void mipsins(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=((a_opcode>>11)& 0x1F )- ((a_opcode>>6)  & 0x1F) +1;
  pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$"); pspDebugScreenSetTextColor(color02);
  sprintf(mipsNum, "%D", a_opcode); pspDebugScreenPuts(mipsNum);
  if(a_more) pspDebugScreenPuts(", ");
}

void mipsImm(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  if(a_slot==1)
  {
    a_opcode&=0x3FFFFFF;
    pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$"); pspDebugScreenSetTextColor(color02);
    sprintf(mipsNum, "%08X", ((a_opcode<<2))); pspDebugScreenPuts(mipsNum);
  }
  else if(VFR==3){
    VFR=0;
    a_opcode&=0xFFFC;
    pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$"); pspDebugScreenSetTextColor(color02);
    sprintf(mipsNum, "%04X", a_opcode); pspDebugScreenPuts(mipsNum);
  }
  else 
  {
    a_opcode&=0xFFFF;
    pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$"); pspDebugScreenSetTextColor(color02);
    sprintf(mipsNum, "%04X", a_opcode); pspDebugScreenPuts(mipsNum);
  }
  
  if(a_more) pspDebugScreenPuts(", ");
}

void mipsDec(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  if(a_slot==1)
  {
    a_opcode&=0x3FFFFFF;
    pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$"); pspDebugScreenSetTextColor(color02);
    sprintf(mipsNum, "%010lu", ((a_opcode<<2))); pspDebugScreenPuts(mipsNum);
  }
  else 
  {
    a_opcode&=0xFFFF;
    a_opcode++;
    if(a_opcode > 0x7FFF){
    a_opcode=0x10000 - a_opcode;
    pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("-"); pspDebugScreenSetTextColor(color02);
    }
    else{
    pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("+"); pspDebugScreenSetTextColor(color02);
	}
    sprintf(mipsNum, "%d", a_opcode); pspDebugScreenPuts(mipsNum);
  }
  
  if(a_more) pspDebugScreenPuts(", ");
}

void mipsDecode(unsigned int a_opcode)
{
  //(o—Í–¼) (a_opcode, Z , a_more‚Ì‰ñ”)
  //Z=2 sll‚Ì^‚ñ’†,1 sll‚Ì¶,3 sll‚Ì‰E,T lw--(?) ,S lw ?$__()
  //Handle opcode
  switch((a_opcode & 0xFC000000) >> 24)
  {
    case 0x00:
      switch(a_opcode & 0x3F)
      {
        case 0x00:
          if(a_opcode == 0)
          {
            pspDebugScreenPuts("nop");
/*NOOP -- no operation
Description: Performs no operation.
Operation: advance_pc (4);
Syntax: noop
Encoding: 0000 0000 0000 0000 0000 0000 0000 0000*/
          }
          else
          {
            pspDebugScreenPuts("sll \t");
            mipsRegister(a_opcode, 2, 1);
            mipsRegister(a_opcode, 1, 1);
            mipsNibble(a_opcode, 3, 0);
/*SLL -- Shift left logical
Description: Shifts a register value left by the shift amount listed in the instruction and places the result in a third register. Zeroes are shifted in.
Operation: $d = $t << h; advance_pc (4);
Syntax: sll $d, $t, h
Encoding: 0000 00ss ssst tttt dddd dhhh hh00 0000*/
          }
          break;
         
        case 0x02:
	if(((a_opcode >>16) & 0xE0) == 0x20){
          pspDebugScreenPuts("rotr	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 1);
          mipsNibble(a_opcode, 3, 0);
	}
//        { "rotr",               0x00200002, 0xFFE0003F, "%d, %t, %a"},
	else{
          pspDebugScreenPuts("srl 	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 1);
          mipsNibble(a_opcode, 3, 0);
	}
/*SRL -- Shift right logical
Description: Shifts a register value right by the shift amount (shamt) and places the value in the destination register. Zeroes are shifted in.
Operation: $d = $t >> h; advance_pc (4);
Syntax: srl $d, $t, h
Encoding: 0000 00-- ---t tttt dddd dhhh hh00 0010*/
          break;

        case 0x03:
          pspDebugScreenPuts("sra 	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 1);
          mipsNibble(a_opcode, 3, 0);
/*SRA -- Shift right arithmetic
Description: Shifts a register value right by the shift amount (shamt) and places the value in the destination register. The sign bit is shifted in.
Operation: $d = $t >> h; advance_pc (4);
Syntax: sra $d, $t, h
Encoding: 0000 00-- ---t tttt dddd dhhh hh00 0011*/
          break;
          
        case 0x0C:
          pspDebugScreenPuts("syscall  ");
/*SYSCALL -- System call
Description: Generates a software interrupt.
Operation: advance_pc (4);
Syntax: syscall
Encoding: 0000 00-- ---- ---- ---- ---- --00 1100*/
          break;
          
        case 0x0D:
          pspDebugScreenPuts("break    ");
          break;
          
          //0x0e = nothing
          
         case 0x0F:
          pspDebugScreenPuts("sync	");
          break;
          
/*          case 0x28:
          pspDebugScreenPuts("mfsa	");
          mipsRegister(a_opcode, 2, 0);
          break;
          
          case 0x29:
          pspDebugScreenPuts("msta	");
          mipsRegister(a_opcode, 2, 0);
          break;*/

/*          case 0x30:
           pspDebugScreenPuts("tge 	");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x31:
           pspDebugScreenPuts("tgeu	");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x32:
           pspDebugScreenPuts("tlt 	");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x33:
           pspDebugScreenPuts("tltu	");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x34:
           pspDebugScreenPuts("teq 	");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x36:
           pspDebugScreenPuts("tne 	");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x38:
           pspDebugScreenPuts("dsll	");
          break;
          
          case 0x3A:
           pspDebugScreenPuts("dsrl	");
          break;
          
          case 0x3B:
           pspDebugScreenPuts("dsra	");
          break;
          
          case 0x3C:
           pspDebugScreenPuts("dsll32   ");
          break;
          
          case 0x3E:
           pspDebugScreenPuts("dsrl32   ");
          break;
          
          case 0x3F:
           pspDebugScreenPuts("dsra32   ");
          break;*/
	}

	switch(a_opcode & 0x7FF){
        case 0x04:
          pspDebugScreenPuts("sllv	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 1);
          mipsRegister(a_opcode, 0, 0);
/*SLLV -- Shift left logical variable
Description: Shifts a register value left by the value in a second register and places the result in a third register. Zeroes are shifted in.
Operation: $d = $t << $s; advance_pc (4);
Syntax: sllv $d, $t, $s
Encoding: 0000 00ss ssst tttt dddd d--- --00 0100*/
          break;

        case 0x06:
          pspDebugScreenPuts("srlv	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 1);
          mipsRegister(a_opcode, 0, 0);
/*SRLV -- Shift right logical variable
Description: Shifts a register value right by the amount specified in $s and places the value in the destination register. Zeroes are shifted in.
Operation: $d = $t >> $s; advance_pc (4);
Syntax: srlv $d, $t, $s
Encoding: 0000 00ss ssst tttt dddd d000 0000 0110*/
          break;
          
         case 0x07:
          pspDebugScreenPuts("srav	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 1);
          mipsRegister(a_opcode, 0, 0);
          break;
          
        case 0x09:
          pspDebugScreenPuts("jalr	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 0);
          break;

        case 0x0A:
          pspDebugScreenPuts("movz	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
          
        case 0x0b:
          pspDebugScreenPuts("movn	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
        break;
          
        case 0x10:
          pspDebugScreenPuts("mfhi	");
          mipsRegister(a_opcode, 2, 0);
/*MFHI -- Move from HI
Description: The contents of register HI are moved to the specified register.
Operation: $d = $HI; advance_pc (4);
Syntax: mfhi $d
Encoding: 0000 0000 0000 0000 dddd d000 0001 0000*/
        break;

        case 0x11:
          pspDebugScreenPuts("mthi	");
          mipsRegister(a_opcode, 2, 0);
	break;
          
        case 0x12:
          pspDebugScreenPuts("mflo	");
          mipsRegister(a_opcode, 2, 0);
/*MFLO -- Move from LO
Description: The contents of register LO are moved to the specified register.
Operation: $d = $LO; advance_pc (4);
Syntax: mflo $d
Encoding: 0000 0000 0000 0000 dddd d000 0001 0010*/
          break;
          
         case 0x13:
          pspDebugScreenPuts("mtlo	");
          mipsRegister(a_opcode, 2, 0);
          break;

        case 0x16:
          pspDebugScreenPuts("clz 	");
          mipsRegister(a_opcode, 1, 1);
          mipsRegister(a_opcode, 2, 0);
         break;
//        { "clo",                0x00000017, 0xFC1F07FF, "%d, %s"},
//        { "clz",                0x00000016, 0xFC1F07FF, "%d, %s"},

         case 0x17:
          pspDebugScreenPuts("clo 	");
          mipsRegister(a_opcode, 1, 1);
          mipsRegister(a_opcode, 2, 0);
         break;
          
        case 0x20:
          pspDebugScreenPuts("add 	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*ADD -- Add
Description: Adds two registers and stores the result in a register
Operation: $d = $s + $t; advance_pc (4);
Syntax: add $d, $s, $t
Encoding: 0000 00ss ssst tttt dddd d000 0010 0000 */
          break;
          
        case 0x21:
          pspDebugScreenPuts("addu	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*ADDU -- Add unsigned
Description: Adds two registers and stores the result in a register
Operation: $d = $s + $t; advance_pc (4);
Syntax: addu $d, $s, $t
Encoding: 0000 00ss ssst tttt dddd d000 0010 0001*/
          break;
          
        case 0x22:
          pspDebugScreenPuts("sub 	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*SUB -- Subtract
Description: Subtracts two registers and stores the result in a register
Operation: $d = $s - $t; advance_pc (4);
Syntax: sub $d, $s, $t
Encoding: 0000 00ss ssst tttt dddd d000 0010 0010*/
          break;
          
        case 0x23:
          pspDebugScreenPuts("subu	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*SUBU -- Subtract unsigned
Description: Subtracts two registers and stores the result in a register
Operation: $d = $s - $t; advance_pc (4);
Syntax: subu $d, $s, $t
Encoding: 0000 00ss ssst tttt dddd d000 0010 0011*/
          break;
          
        case 0x24:
          pspDebugScreenPuts("and 	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*AND -- Bitwise and
Description: Bitwise ands two registers and stores the result in a register
Operation: $d = $s & $t; advance_pc (4);
Syntax: and $d, $s, $t
Encoding: 0000 00ss ssst tttt dddd d000 0010 0100*/
          break;
          
        case 0x25:
          pspDebugScreenPuts("or  	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*OR -- Bitwise or
Description: Bitwise logical ors two registers and stores the result in a register
Operation: $d = $s | $t; advance_pc (4);
Syntax: or $d, $s, $t
Encoding: 0000 00ss ssst tttt dddd d000 0010 0101*/
          break;
          
        case 0x26:
          pspDebugScreenPuts("xor 	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*The syscall instruction is described in more detail on the System Calls page.
XOR -- Bitwise exclusive or
Description: Exclusive ors two registers and stores the result in a register
Operation: $d = $s ^ $t; advance_pc (4);
Syntax: xor $d, $s, $t
Encoding: 0000 00ss ssst tttt dddd d--- --10 0110*/
          break;
          
          case 0x27:
          pspDebugScreenPuts("nor 	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;

        case 0x2A:
          pspDebugScreenPuts("slt 	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*SLT -- Set on less than (signed)
Description: If $s is less than $t, $d is set to one. It gets zero otherwise.
Operation: if $s < $t $d = 1; advance_pc (4); else $d = 0; advance_pc (4);
Syntax: slt $d, $s, $t
Encoding: 0000 00ss ssst tttt dddd d000 0010 1010*/
        break;
          
        case 0x2B:
          pspDebugScreenPuts("sltu	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*SLTU -- Set on less than unsigned
Description: If $s is less than $t, $d is set to one. It gets zero otherwise.
Operation: if $s < $t $d = 1; advance_pc (4); else $d = 0; advance_pc (4);
Syntax: sltu $d, $s, $t
Encoding: 0000 00ss ssst tttt dddd d000 0010 1011*/
          break;
          
          case 0x2c:
          pspDebugScreenPuts("max 	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
//        { "max",                0x0000002C, 0xFC0007FF, "%d, %s, %t"},
//        { "min",                0x0000002D, 0xFC0007FF, "%d, %s, %t"},
          
          case 0x2d:
          pspDebugScreenPuts("min 	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;

	case 0x46:
          pspDebugScreenPuts("rotv	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
//        { "rotv",               0x00000046, 0xFC0007FF, "%d, %t, %s"},
        }

	switch(a_opcode & 0xFFFF){
        case 0x08:
          pspDebugScreenPuts("jr  	");
          mipsRegister(a_opcode, 0, 0);
/*JR -- Jump register
Description: Jump to the address contained in register $s
Operation: PC = nPC; nPC = $s;
Syntax: jr $s
Encoding: 0000 00ss sss0 0000 0000 0000 0000 1000*/
          break;
          
        case 0x18:
          pspDebugScreenPuts("mult	");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*MULT -- Multiply
Description: Multiplies $s by $t and stores the result in $LO.
Operation: $LO = $s * $t; advance_pc (4);
Syntax: mult $s, $t
Encoding: 0000 00ss ssst tttt 0000 0000 0001 1000*/
          break;
          
        case 0x19:
          pspDebugScreenPuts("multu    ");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*MULTU -- Multiply unsigned
Description: Multiplies $s by $t and stores the result in $LO.
Operation: $LO = $s * $t; advance_pc (4);
Syntax: multu $s, $t
Encoding: 0000 00ss ssst tttt 0000 0000 0001 1001*/
          break;
          
        case 0x1A:
          pspDebugScreenPuts("div 	");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*DIV -- Divide
Description: Divides $s by $t and stores the quotient in $LO and the remainder in $HI
Operation: $LO = $s / $t; $HI = $s % $t; advance_pc (4);
Syntax: div $s, $t
Encoding: 0000 00ss ssst tttt 0000 0000 0001 1010*/
          break;
          
        case 0x1B:
          pspDebugScreenPuts("divu	");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*DIVU -- Divide unsigned
Description: Divides $s by $t and stores the quotient in $LO and the remainder in $HI
Operation: $LO = $s / $t; $HI = $s % $t; advance_pc (4);
Syntax: divu $s, $t
Encoding: 0000 00ss ssst tttt 0000 0000 0001 1011*/
          break;
                  
         case 0x1C:
          pspDebugScreenPuts("madd	");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
//        { "madd",               0x0000001C, 0xFC00FFFF, "%s, %t"},
//        { "maddu",              0x0000001D, 0xFC00FFFF, "%s, %t"},
          
        case 0x1D:
          pspDebugScreenPuts("maddu    ");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;

          case 0x2e:
          pspDebugScreenPuts("msub	");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
          
          case 0x2F:
          pspDebugScreenPuts("msubu    ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
//        { "msub",               0x0000002e, 0xfc00ffff, "%d, %t"},
//        { "msubu",              0x0000002f, 0xfc00ffff, "%d, %t"},
      }
      break;
      
    case 0x04:
      switch((a_opcode & 0x1F0000) >> 16)
      {
          case 0x00:
            pspDebugScreenPuts("bltz	");
            mipsRegister(a_opcode, S, 1);
	    mipsDec(a_opcode,0,0);
/*BLTZ -- Branch on less than zero
Description: Branches if the register is less than zero
Operation: if $s < 0 advance_pc (offset << 2)); else advance_pc (4);
Syntax: bltz $s, offset
Encoding: 0000 01ss sss0 0000 iiii iiii iiii iiii*/
          break;
            
          case 0x01:
            pspDebugScreenPuts("bgez	");
            mipsRegister(a_opcode, S, 1);
	    mipsDec(a_opcode,0,0);
/*BGEZ -- Branch on greater than or equal to zero
Description: Branches if the register is greater than or equal to zero
Operation: if $s >= 0 advance_pc (offset << 2)); else advance_pc (4);
Syntax: bgez $s, offset
Encoding: 0000 01ss sss0 0001 iiii iiii iiii iiii*/
          break;
          
           case 0x02:
            pspDebugScreenPuts("bltzl    ");
            mipsRegister(a_opcode, S, 1);
	    mipsDec(a_opcode,0,0);
           break;
            
           case 0x03:
            pspDebugScreenPuts("bgezl    ");
            mipsRegister(a_opcode, S, 1);
	    mipsDec(a_opcode,0,0);
           break;
           
           case 0x08:
            pspDebugScreenPuts("tgei	");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
          
           case 0x09:
            pspDebugScreenPuts("tgeiu    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
          
           case 0x0A:
            pspDebugScreenPuts("tlti	");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
           
           case 0x0B:
            pspDebugScreenPuts("tltiu    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
           
           case 0x0C:
            pspDebugScreenPuts("teqi	");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
           
           case 0x0E:
            pspDebugScreenPuts("tnei	");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
          
          case 0x10:
            pspDebugScreenPuts("bltzal   ");
            mipsRegister(a_opcode, S, 1);
	    mipsDec(a_opcode,0,0);
/*BLTZAL -- Branch on less than zero and link
Description: Branches if the register is less than zero and saves the return address in $31
Operation: if $s < 0 $31 = PC + 8 (or nPC + 4); advance_pc (offset << 2)); else advance_pc (4);
Syntax: bltzal $s, offset
Encoding: 0000 01ss sss1 0000 iiii iiii iiii iiii*/
            break;
            
           case 0x11:
            pspDebugScreenPuts("bgezal   ");
            mipsRegister(a_opcode, S, 1);
	    mipsDec(a_opcode,0,0);
/*BGEZAL -- Branch on greater than or equal to zero and link
Description: Branches if the register is greater than or equal to zero and saves the return address in $31
Operation: if $s >= 0 $31 = PC + 8 (or nPC + 4); advance_pc (offset << 2)); else advance_pc (4);
Syntax: bgezal $s, offset
Encoding: 0000 01ss sss1 0001 iiii iiii iiii iiii*/
           break;
            
           case 0x12:
            pspDebugScreenPuts("bltzall  ");
            mipsRegister(a_opcode, S, 1);
	    mipsDec(a_opcode,0,0);
           break;
             
           case 0x13:
            pspDebugScreenPuts("bgezall  ");
            mipsRegister(a_opcode, S, 1);
	    mipsDec(a_opcode,0,0);
           break;    
            
           case 0x18:
            pspDebugScreenPuts("mtsab    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;        
            
           case 0x19:
            pspDebugScreenPuts("mtsah    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;   
            
          default:
            pspDebugScreenPuts("??? 	");
      }
      break;
      
    case 0x08:
	if((a_opcode >= 0x08000000) && (a_opcode < 0x8800000)){
	pspDebugScreenPuts("kernelram");}
	else if((a_opcode >= 0x8800000) && (a_opcode < 0xA000000)){
	pspDebugScreenPuts("userram");	}
	else{
      pspDebugScreenPuts("j   	");
      mipsImm(a_opcode, 1, 0);}
/*J -- Jump
Description: Jumps to the calculated address
Operation: PC = nPC; nPC = (PC & 0xf0000000) | (target << 2);
Syntax: j target
Encoding: 0000 10ii iiii iiii iiii iiii iiii iiii*/
      break;
      
    case 0x0C:
      pspDebugScreenPuts("jal 	");
      mipsImm(a_opcode, 1, 0);
/*JAL -- Jump and link
Description: Jumps to the calculated address and stores the return address in $31
Operation: $31 = PC + 8 (or nPC + 4); PC = nPC; nPC = (PC & 0xf0000000) | (target << 2);
Syntax: jal target
Encoding: 0000 11ii iiii iiii iiii iiii iiii iiii*/
      break;
      
    case 0x10:
      pspDebugScreenPuts("beq 	");
      mipsRegister(a_opcode, S, 1);
      mipsRegister(a_opcode, T, 1);
      mipsDec(a_opcode, 0, 0);
/*BEQ -- Branch on equal
Description: Branches if the two registers are equal
Operation: if $s == $t advance_pc (offset << 2)); else advance_pc (4);
Syntax: beq $s, $t, offset
Encoding: 0001 00ss ssst tttt iiii iiii iiii iiii*/
      break;
      
    case 0x14:
      pspDebugScreenPuts("bne 	");
      mipsRegister(a_opcode, S, 1);
      mipsRegister(a_opcode, T, 1);
      mipsDec(a_opcode, 0, 0);
/*BNE -- Branch on not equal
Description: Branches if the two registers are not equal
Operation: if $s != $t advance_pc (offset << 2)); else advance_pc (4);
Syntax: bne $s, $t, offset
Encoding: 0001 01ss ssst tttt iiii iiii iiii iiii*/
      break;
      
    case 0x18:
      pspDebugScreenPuts("blez	");  
      mipsRegister(a_opcode, S, 1);
      mipsDec(a_opcode, 0, 0);
/*BLEZ -- Branch on less than or equal to zero
Description: Branches if the register is less than or equal to zero
Operation: if $s <= 0 advance_pc (offset << 2)); else advance_pc (4);
Syntax: blez $s, offset
Encoding: 0001 10ss sss0 0000 iiii iiii iiii iiii*/
      break;
      
    case 0x1C:
      pspDebugScreenPuts("bgtz	");
      mipsRegister(a_opcode, S, 1);
      mipsDec(a_opcode, 0, 0);
/*BGTZ -- Branch on greater than zero
Description: Branches if the register is greater than zero
Operation: if $s > 0 advance_pc (offset << 2)); else advance_pc (4);
Syntax: bgtz $s, offset
Encoding: 0001 11ss sss0 0000 iiii iiii iiii iiii*/
      break;
      
    case 0x20:
      pspDebugScreenPuts("addi	");
      mipsRegister(a_opcode, T, 1);
      mipsRegister(a_opcode, S, 1);
      mipsImm(a_opcode, 0, 0);
/*ADDI -- Add immediate
Description: Adds a register and a signed immediate value and stores the result in a register
Operation: $t = $s + imm; advance_pc (4);
Syntax: addi $t, $s, imm
Encoding: 0010 00ss ssst tttt iiii iiii iiii iiii*/
      break;
      
    case 0x24:
      pspDebugScreenPuts("addiu    ");
      mipsRegister(a_opcode, T, 1);
      mipsRegister(a_opcode, S, 1);
      mipsImm(a_opcode, 0, 0);
/*ADDIU -- Add immediate unsigned
Description: Adds a register and an unsigned immediate value and stores the result in a register
Operation: $t = $s + imm; advance_pc (4);
Syntax: addiu $t, $s, imm
Encoding: 0010 01ss ssst tttt iiii iiii iiii iiii*/
      break;
      
    case 0x28:
      pspDebugScreenPuts("slti	");
      mipsRegister(a_opcode, T, 1);
      mipsRegister(a_opcode, S, 1);
      mipsImm(a_opcode, 0, 0);
/*SLTI -- Set on less than immediate (signed)
Description: If $s is less than immediate, $t is set to one. It gets zero otherwise.
Operation: if $s < imm $t = 1; advance_pc (4); else $t = 0; advance_pc (4);
Syntax: slti $t, $s, imm
Encoding: 0010 10ss ssst tttt iiii iiii iiii iiii*/
      break;
      
    case 0x2C:
      pspDebugScreenPuts("sltiu    ");
      mipsRegister(a_opcode, T, 1);
      mipsRegister(a_opcode, S, 1);
      mipsImm(a_opcode, 0, 0);
/*SLTIU -- Set on less than immediate unsigned
Description: If $s is less than the unsigned immediate, $t is set to one. It gets zero otherwise.
Operation: if $s < imm $t = 1; advance_pc (4); else $t = 0; advance_pc (4);
Syntax: sltiu $t, $s, imm
Encoding: 0010 11ss ssst tttt iiii iiii iiii iiii*/
      break;
      
    case 0x30:
      pspDebugScreenPuts("andi	");
      mipsRegister(a_opcode, T, 1);
      mipsRegister(a_opcode, S, 1);
      mipsImm(a_opcode, 0, 0);
/*ANDI -- Bitwise and immediate
Description: Bitwise ands a register and an immediate value and stores the result in a register
Operation: $t = $s & imm; advance_pc (4);
Syntax: andi $t, $s, imm
Encoding: 0011 00ss ssst tttt iiii iiii iiii iiii*/
      break;
      
    case 0x34:
      pspDebugScreenPuts("ori 	");
      mipsRegister(a_opcode, T, 1);
      mipsRegister(a_opcode, S, 1);
      mipsImm(a_opcode, 0, 0);
/*ORI -- Bitwise or immediate
Description: Bitwise ors a register and an immediate value and stores the result in a register
Operation: $t = $s | imm; advance_pc (4);
Syntax: ori $t, $s, imm
Encoding: 0011 01ss ssst tttt iiii iiii iiii iiii*/
      break;
      
    case 0x38:
      pspDebugScreenPuts("xori	");
      mipsRegister(a_opcode, T, 1);
      mipsRegister(a_opcode, S, 1);
      mipsImm(a_opcode, 0, 0);
/*XORI -- Bitwise exclusive or immediate
Description: Bitwise exclusive ors a register and an immediate value and stores the result in a register
Operation: $t = $s ^ imm; advance_pc (4);
Syntax: xori $t, $s, imm
Encoding: 0011 10ss ssst tttt iiii iiii iiii iiii*/
      break;
      
    case 0x3C:
      pspDebugScreenPuts("lui 	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
/*LUI -- Load upper immediate
Description: The immediate value is shifted left 16 bits and stored in the register. The lower 16 bits are zeroes.
Operation: $t = (imm << 16); advance_pc (4);
Syntax: lui $t, imm
Encoding: 0011 11-- ---t tttt iiii iiii iiii iiii*/
      break;
      
      case 0x40:
	switch(a_opcode >> 20 & 0xFFE){
	case 0x400:
		pspDebugScreenPuts("mfc0	");
		mipsRegister(a_opcode, T, 1);
		cop0Register(a_opcode, 2, 0);
	break;

	case 0x408:
		pspDebugScreenPuts("mtc0	");
		mipsRegister(a_opcode, T, 1);
		cop0Register(a_opcode, 2, 0);
	break;

	case 0x404:
		pspDebugScreenPuts("cfc0	");
		mipsRegister(a_opcode, T, 1);
		mipsNibble(a_opcode, 2, 0);
	break;


	case 0x40C:
		pspDebugScreenPuts("ctc0	");
		mipsRegister(a_opcode, T, 1);
		mipsNibble(a_opcode, 2, 0);
	break;
//"cfc0",               0x40400000, 0xFFE007FF, "%t, %p"
//"ctc0",               0x40C00000, 0xFFE007FF, "%t, %p"
//"mfc0",               0x40000000, 0xFFE007FF, "%t, %0"
//"mtc0",               0x40800000, 0xFFE007FF, "%t, %0"

	case 0x420:
	if((a_opcode & 0xFFFFFF)  == 0x18){
		pspDebugScreenPuts("eret	");
		}
	break;
	}
      break;


//FPU–½—ß
     case 0x44:
      switch(a_opcode >>24){
		case 0x44:
		switch(a_opcode >> 16 & 0xE0){
		case 0x40:
		pspDebugScreenPuts("cfc1	");
		mipsRegister(a_opcode, T, 1);
		mipsNibble(a_opcode, 2, 0);
		break;

		case 0xC0:
		pspDebugScreenPuts("ctc1	");
		mipsRegister(a_opcode, T, 1);
		mipsNibble(a_opcode, 2, 0);
		break;

		case 0x00:
		pspDebugScreenPuts("mfc1	");
		mipsRegister(a_opcode, T, 1);
		floatRegister(a_opcode,2,0);
		break;

		case 0x80:
		pspDebugScreenPuts("mtc1	");
		mipsRegister(a_opcode, T, 1);
		floatRegister(a_opcode,2,0);
		break;
		}
		break;
/*        {"cfc1",        0x44400000, 0xFFE007FF, "%t, %p"},
        {"ctc1",        0x44c00000, 0xFFE007FF, "%t, %p"},
        {"mfc1",        0x44000000, 0xFFE007FF, "%t, %1"},
        {"mtc1",        0x44800000, 0xFFE007FF, "%t, %1"},*/

	        case 0x45:
		switch(a_opcode >>16 & 0xFF){
		case 0x00:
		pspDebugScreenPuts("bc1f	");
		mipsDec(a_opcode, 0 ,0 );
		break;

		case 0x01:
		pspDebugScreenPuts("bc1t	");
		mipsDec(a_opcode, 0 ,0 );
		break;

		case 0x02:
		pspDebugScreenPuts("bc1fl    ");
		mipsDec(a_opcode, 0 ,0 );
		break;

		case 0x03:
		pspDebugScreenPuts("bc1tl    ");
		mipsDec(a_opcode, 0 ,0 );
		break;
		}
		break;

	        case 0x46:
		switch(a_opcode & 0x3F){
		case 0x00:
		pspDebugScreenPuts("add.s    ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;
		
		case 0x01:
		pspDebugScreenPuts("sub.s    ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x02:
		pspDebugScreenPuts("mul.s    ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x03:
		pspDebugScreenPuts("div.s    ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x04:
		pspDebugScreenPuts("sqrt.s   ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 0);
		break;

		case 0x05:
		pspDebugScreenPuts("abs.s    ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 0);
		break;

		case 0x06:
		pspDebugScreenPuts("mov.s    ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 0);
		break;

		case 0x07:
		pspDebugScreenPuts("neg.s    ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 0);
		break;

		case 0x0C:
		pspDebugScreenPuts("round.w.s ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 0);
		break;

		case 0x0D:
		pspDebugScreenPuts("trunc.w.s ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 0);
		break;

		case 0x0E:
		pspDebugScreenPuts("ceil.w.s ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 0);
		break;

		case 0x0F:
		pspDebugScreenPuts("floor.w.s ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 0);
		break;

		case 0x20:
		if((a_opcode  & 0xFF0000) == 0x800000){
		pspDebugScreenPuts("cvt.s.w  ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 0);
		}
		break;

		case 0x24:
		pspDebugScreenPuts("cvt.w.s  ");
		floatRegister(a_opcode, 3, 1);
		floatRegister(a_opcode, 2, 0);
		break;
		}

		switch(a_opcode & 0x7FF){
		case 0x30:
		pspDebugScreenPuts("c.f.s    ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x31:
		pspDebugScreenPuts("c.un.s   ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x32:
		pspDebugScreenPuts("c.eq.s   ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x33:
		pspDebugScreenPuts("c.ueq.s  ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x34:
		pspDebugScreenPuts("c.olt.s  ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x35:
		pspDebugScreenPuts("c.ult.s  ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x36:
		pspDebugScreenPuts("c.ole.s  ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x37:
		pspDebugScreenPuts("c.ule.s  ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x38:
		pspDebugScreenPuts("c.sf.s   ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x39:
		pspDebugScreenPuts("c.ngle.s ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x3A:
		pspDebugScreenPuts("c.seq.s  ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x3B:
		pspDebugScreenPuts("c.ngl.s  ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x3C:
		pspDebugScreenPuts("c.lt.s   ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x3D:
		pspDebugScreenPuts("c.nge.s  ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x3E:
		pspDebugScreenPuts("c.le.s   ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x3F:
		pspDebugScreenPuts("c.ngt.s  ");
		floatRegister(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;
		}
		break;
/*        {"c.f.s",       0x46000030, 0xFFE007FF, "%S, %T"},
        {"c.un.s",      0x46000031, 0xFFE007FF, "%S, %T"},
        {"c.eq.s",      0x46000032, 0xFFE007FF, "%S, %T"},
        {"c.ueq.s",     0x46000033, 0xFFE007FF, "%S, %T"},
        {"c.olt.s",     0x46000034, 0xFFE007FF, "%S, %T"},
        {"c.ult.s",     0x46000035, 0xFFE007FF, "%S, %T"},
        {"c.ole.s",     0x46000036, 0xFFE007FF, "%S, %T"},
        {"c.ule.s",     0x46000037, 0xFFE007FF, "%S, %T"},
        {"c.sf.s",      0x46000038, 0xFFE007FF, "%S, %T"},
        {"c.ngle.s",    0x46000039, 0xFFE007FF, "%S, %T"},
        {"c.seq.s",     0x4600003A, 0xFFE007FF, "%S, %T"},
        {"c.ngl.s",     0x4600003B, 0xFFE007FF, "%S, %T"},
        {"c.lt.s",      0x4600003C, 0xFFE007FF, "%S, %T"},
        {"c.nge.s",     0x4600003D, 0xFFE007FF, "%S, %T"},
        {"c.le.s",      0x4600003E, 0xFFE007FF, "%S, %T"},
        {"c.ngt.s",     0x4600003F, 0xFFE007FF, "%S, %T"},*/

     }
     break;

    case 0x48:
     switch(a_opcode >>24){
       case 0x48:
	switch(a_opcode >>16 & 0xE0){
	case 0x60:
		if((a_opcode & 0xFF80) == 0){
      		pspDebugScreenPuts("mfv 	");
		mipsRegister(a_opcode, T, 1);
		vectors(a_opcode , 2 , 0);
		}
		else if((a_opcode & 0xFF00) == 0){
      		pspDebugScreenPuts("mfvc	");
		mipsRegister(a_opcode, T, 1);
		VFMODE=1;
		vectors(a_opcode, 2, 0);
		}
	break;

	case 0xE0:
		if((a_opcode & 0xFF80) == 0){
      		pspDebugScreenPuts("mtv 	");
		mipsRegister(a_opcode, T, 1);
		vectors(a_opcode , 2 , 0);
		}
		else if((a_opcode & 0xFF00) == 0){
      		pspDebugScreenPuts("mtvc	");
		mipsRegister(a_opcode, T, 1);
		VFMODE=1;
		vectors(a_opcode, 2, 0);
		}
	break;
	}break;
/*      { "mfv",         0x48600000, 0xFFE0FF80, "" },
        { "mfvc",        0x48600000, 0xFFE0FF00, "" },
        { "mtv",         0x48E00000, 0xFFE0FF80, "" },
        { "mtvc",        0x48E00000, 0xFFE0FF00, "" },*/

      case 0x49:
		switch(a_opcode >>16 & 0xE3){
		case 0x00:
      		pspDebugScreenPuts("bvf 	");
		VFMODE=2;
		vectors(a_opcode, 0, 1);
		mipsDec(a_opcode, 0, 0);
		break;

		case 0x01:
      		pspDebugScreenPuts("bvt 	");
		VFMODE=2;
		vectors(a_opcode, 0, 1);
		mipsDec(a_opcode, 0, 0);
		break;

		case 0x02:
      		pspDebugScreenPuts("bvfl	");
		VFMODE=2;
		vectors(a_opcode, 0, 1);
		mipsDec(a_opcode, 0, 0);
		break;

		case 0x03:
      		pspDebugScreenPuts("bvtl	");
		VFMODE=2;
		vectors(a_opcode, 0, 1);
		mipsDec(a_opcode, 0, 0);
		break;
	}break;
/*        { "bvf",       0x49000000, 0xFFE30000, "%Z, %O" },
        { "bvfl",        0x49020000, 0xFFE30000, "%Z, %O" },
        { "bvt",         0x49010000, 0xFFE30000, "%Z, %O" },
        { "bvtl",        0x49030000, 0xFFE30000, "%Z, %O" },*/
     }
     break;

     case 0x50:
      pspDebugScreenPuts("beql	");
      mipsRegister(a_opcode, S, 1);
      mipsRegister(a_opcode, T, 1);
      mipsDec(a_opcode, 0, 0);
     break;
      
     case 0x54:
      pspDebugScreenPuts("bnel	");
      mipsRegister(a_opcode, S, 1);
      mipsRegister(a_opcode, T, 1);
      mipsDec(a_opcode, 0, 0);
     break;
      
     case 0x58:
      pspDebugScreenPuts("blezl    ");
      mipsRegister(a_opcode, S, 1);
      mipsRegister(a_opcode, T, 1);
      mipsDec(a_opcode, 0, 0);
     break;
     
     case 0x5C:
      pspDebugScreenPuts("bgtzl    ");
      mipsRegister(a_opcode, S, 1);
      mipsRegister(a_opcode, T, 1);
      mipsDec(a_opcode, 0, 0);
     break;



     case 0x60:
     switch(a_opcode >>24 & 0xFF){
	case 0x60:
	switch(a_opcode >>16 & 0x80){
		case 0x00:
	        pspDebugScreenPuts("vadd.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
                break;
//    { "vadd.s",      0x60000000, 0xFF808080, "%zs, %yz, %xs" },

		case 0x80:		
	        pspDebugScreenPuts("vsub.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
	        break;
        }break;
//    { "vsub.s",      0x60800000, 0xFF808080, "%zs, %ys, %xs" },

      case 0x61:
	        pspDebugScreenPuts("vsbn.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
                break;

      case 0x63:
	if(a_opcode >> 16 & 0x80){
	        pspDebugScreenPuts("vdiv.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
                }break;
//        { "vdiv.s",      0x63800000, 0xFF808080, "%zs, %yz, %xs" },

    }break;

    case 0x64:
	switch(a_opcode >> 24){
		case 0x64:
	        pspDebugScreenPuts("vmul.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
                break;
//        { "vmul.s",      0x64000000, 0xFF808080, "%zs, %ys, %xs" }
	}break;

     case 0x68:
     pspDebugScreenPuts("mfvme    ");
     mipsRegister(a_opcode, T, 1);
     mipsImm(a_opcode,0,0);
     break;
     
     case 0x6C:
	switch(a_opcode >>16 & 0xFF80){
	case 0x6C00:
	pspDebugScreenPuts("vcmp.s   ");
	VectorCMP(a_opcode,0,1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
	break;

	case 0x6D00:
	pspDebugScreenPuts("vmin.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
	break;

	case 0x6D80:
	pspDebugScreenPuts("vmax.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
	break;

	case 0x6E80:
	pspDebugScreenPuts("vscmp.s  ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
	break;

 	case 0x6F00:
	pspDebugScreenPuts("vsge.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
 	break;

	case 0x6F80:
	pspDebugScreenPuts("vslt.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
	break;
	}
//        { "vcmp.s",      0x6C000000, 0xFFFF80F0, "" },
//        { "vmax.s",      0x6D800000, 0xFF808080, "%zs, %ys, %xs" },
//        { "vmin.s",      0x6D000000, 0xFF808080, "%zs, %ys, %xs" },
//        { "vscmp.s",     0x6E800000, 0xFF808080, "%zs, %ys, %xs" },
//        { "vsge.s",      0x6F000000, 0xFF808080, "%zs, %ys, %xs" },
//        { "vslt.s",      0x6F800000, 0xFF808080, "%zs, %ys, %xs" },
     break;
     
     case 0x70:
	if(a_opcode >> 24 == 0x70){
		switch(a_opcode & 0xE007FF){
		case 0x24:
	      	pspDebugScreenPuts("mfic	");
		mipsRegister(a_opcode, T, 1);
		mipsNibble(a_opcode, 2, 0);
		break;

		case 0x26:
	      	pspDebugScreenPuts("mtic	");
		mipsRegister(a_opcode, T, 1);
		mipsNibble(a_opcode, 2, 0);
		break;

		case 0x3D:
	      	pspDebugScreenPuts("mfdr	");
		mipsRegister(a_opcode, T, 1);
                DrRegister(a_opcode,2,0);
		break;

		case 0x80003D:
	      	pspDebugScreenPuts("mtdr	");
		mipsRegister(a_opcode, T, 1);
                DrRegister(a_opcode,2,0);
		break; 
//"mfdr",              0x7000003D, 0xFFE007FF, "%t, %r"},
//"mtdr",              0x7080003D, 0xFFE007FF, "%t, %r"},
//"mfic",              0x70000024, 0xFFE007FF, "%t, %p"},
//"mtic",              0x70000026, 0xFFE007FF, "%t, %p"},
		}

		switch(a_opcode & 0xFFFFFF){
		case 0x00:
	      	pspDebugScreenPuts("halt	");
		break;

		case 0x3E:
	      	pspDebugScreenPuts("dret	");
		break;

		case 0x3F:
	      	pspDebugScreenPuts("dbreak    ");
		break;
		}
	}
   break;
//"dbreak",            0x7000003F, 0xFFFFFFFF, ""},
//"dret",              0x7000003E, 0xFFFFFFFF, ""},
//"halt",  	     0x70000000, 0xFFFFFFFF, "" },
     
      //0x1d is empty
      
//      case 0x78:
//      break;

     case 0x7C:
	if(a_opcode >>24 == 0x7C){
      switch(a_opcode & 0x7FF){
	case 0x420:
      	pspDebugScreenPuts("seb 	");
        mipsRegister(a_opcode, 2, 1);
        mipsRegister(a_opcode, 1, 0);
	break;
	
	case 0x620:
      	pspDebugScreenPuts("seh 	");
        mipsRegister(a_opcode, 2, 1);
        mipsRegister(a_opcode, 1, 0);
	break;

	case 0xA0:
      	pspDebugScreenPuts("wsbbn    ");
        mipsRegister(a_opcode, 2, 1);
        mipsRegister(a_opcode, 1, 0);
	break;
	
	case 0xE0:
      	pspDebugScreenPuts("wsbw	");
        mipsRegister(a_opcode, 2, 1);
        mipsRegister(a_opcode, 1, 0);
	break;       

	case 0x520:
      	pspDebugScreenPuts("bitrev   ");
        mipsRegister(a_opcode, 2, 1);
        mipsRegister(a_opcode, 1, 0);
	break;       
	}}
	switch(a_opcode & 0x03F){
	case 0x0:
	pspDebugScreenPuts("ext 	");
	mipsRegister(a_opcode, T, 1);
	mipsRegister(a_opcode, S, 1);
      	mipsNibble(a_opcode, 3, 1);
	a_opcode+=0x800;
      	mipsNibble(a_opcode, 2, 0);
	break;
	
	case 0x4:
	pspDebugScreenPuts("ins 	");
	mipsRegister(a_opcode, T, 1);
	mipsRegister(a_opcode, S, 1);
	mipsNibble(a_opcode, 3, 1);
	mipsins(a_opcode, 0, 0);
     	break;
	}
    break;

    case 0x80:
      pspDebugScreenPuts("lb  	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
/*LB -- Load byte
Description: A byte is loaded into a register from the specified address.
Operation: $t = MEM[$s + offset]; advance_pc (4);
Syntax: lb $t, offset($s)
Encoding: 1000 00ss ssst tttt iiii iiii iiii iiii*/
      break;

    case 0x84:
      pspDebugScreenPuts("lh  	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
/*LH -- Load word
Description: A half word is loaded into a register from the specified address.
Operation: $t = MEM[$s + offset]; advance_pc (4);
Syntax: lh $t, offset($s)*/
      break;
      
     case 0x88:
      pspDebugScreenPuts("lwl 	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
    case 0x8C:
      pspDebugScreenPuts("lw  	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
/*LW -- Load word
Description: A word is loaded into a register from the specified address.
Operation: $t = MEM[$s + offset]; advance_pc (4);
Syntax: lw $t, offset($s)
Encoding: 1000 11ss ssst tttt iiii iiii iiii iiii*/
      break;
     
     case 0x90:
      pspDebugScreenPuts("lbu 	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0x94:
      pspDebugScreenPuts("lhu 	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0x98:
      pspDebugScreenPuts("lwr 	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
     case 0x9C:
      pspDebugScreenPuts("lwu 	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
    case 0xA0:
      pspDebugScreenPuts("sb  	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
/*SB -- Store byte
Description: The least significant byte of $t is stored at the specified address.
Operation: MEM[$s + offset] = (0xff & $t); advance_pc (4);
Syntax: sb $t, offset($s)
Encoding: 1010 00ss ssst tttt iiii iiii iiii iiii*/
      break;
      
     case 0xA4:
      pspDebugScreenPuts("sh  	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xA8:
      pspDebugScreenPuts("swl 	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
    case 0xAC:
      pspDebugScreenPuts("sw  	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
/*SW -- Store word
Description: The contents of $t is stored at the specified address.
Operation: MEM[$s + offset] = $t; advance_pc (4);
Syntax: sw $t, offset($s)
Encoding: 1010 11ss ssst tttt iiii iiii iiii iiii*/
      break;
      
/*      case 0xB0:
      pspDebugScreenPuts("sdl 	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xB4:
      pspDebugScreenPuts("sdr 	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;*/
      
      case 0xB8:
      pspDebugScreenPuts("swr 	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      //important little bastard right here
      case 0xBC:
      pspDebugScreenPuts("cache    ");
      specialRegister(a_opcode, T, 1); //uses special register function
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
     
      case 0xC0:
      pspDebugScreenPuts("ll  	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xC4:
      pspDebugScreenPuts("lwc1	");
      floatRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xC8:
      pspDebugScreenPuts("lv.s	");
      VFR=3;
      vectors(a_opcode, 0, 1);
      VFR=3;
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xCC:
      pspDebugScreenPuts("pref	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xD0:
	switch(a_opcode >>16){
	case 0xD000:
	        pspDebugScreenPuts("vmov.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
		break;
//        { "vmov.s",      0xD0000000, 0xFFFF8080, "%zs, %ys" },

	case 0xD001:
	        pspDebugScreenPuts("vabs.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//       { "vabs.s",      0xD0010000, 0xFFFF8080, "%zs, %ys" },

	case 0xD002:
	        pspDebugScreenPuts("vneg.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vneg.s",      0xD0020000, 0xFFFF8080, "%zs, %ys" },

	case 0xD004:
	        pspDebugScreenPuts("vsat0.s  ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vsat0.s", 0xD0040000, 0xFFFF8080, "%zs, %ys" },

	case 0xD005:
	        pspDebugScreenPuts("vsat1.s  ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vsat1.s", 0xD0050000, 0xFFFF8080, "%zs, %ys" },

	case 0xD006:
		if((a_opcode & 0xFF80) == 0){
	        pspDebugScreenPuts("vzero.s  ");
     	 	vectors(a_opcode, 2, 0);}
	break;
//        { "vzero.s", 0xD0060000, 0xFFFFFF80, "%zs" },

	case 0xD007:
		if((a_opcode & 0xFF80) == 0){
	        pspDebugScreenPuts("vone.s   ");
     	 	vectors(a_opcode, 2, 0);}
	break;
//        { "vone.s",      0xD0070000, 0xFFFFFF80, "%zs" },

	case 0xD010:
	        pspDebugScreenPuts("vecp.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vrcp.s",      0xD0100000, 0xFFFF8080, "%zs, %ys" },

	case 0xD011:
	        pspDebugScreenPuts("vrsq.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vrsq.s",      0xD0110000, 0xFFFF8080, "%zs, %ys" },


	case 0xD012:
	        pspDebugScreenPuts("vsin.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//         { "vsin.s",      0xD0120000, 0xFFFF8080, "%zs, %ys" },

	case 0xD013:
	        pspDebugScreenPuts("vcos.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vcos.s",      0xD0130000, 0xFFFF8080, "%zs, %ys" },

	case 0xD014:
	        pspDebugScreenPuts("vexp2.s  ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vexp2.s", 0xD0140000, 0xFFFF8080, "%zs, %ys" },

	case 0xD015:
	        pspDebugScreenPuts("vlog2.s  ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vlog2.s", 0xD0150000, 0xFFFF8080, "%zs, %ys" },

	case 0xD016:
	        pspDebugScreenPuts("vsqrt.s  ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vsqrt.s", 0xD0160000, 0xFFFF8080, "%zs, %ys" },

	case 0xD017:
	        pspDebugScreenPuts("vasin.s  ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//       { "vasin.s", 0xD0170000, 0xFFFF8080, "%zs, %ys" },

	case 0xD018:
	        pspDebugScreenPuts("vnrcp.s  ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vnrcp.s", 0xD0180000, 0xFFFF8080, "%zs, %ys" },

	case 0xD01A:
	        pspDebugScreenPuts("vnsin.s  ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vnsin.s", 0xD01A0000, 0xFFFF8080, "%zs, %ys" },

	case 0xD01C:
	        pspDebugScreenPuts("vrexp2.s ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vrexp2.s", 0xD01C0000, 0xFFFF8080, "%zs, %ys" },

	case 0xD020:
		if((a_opcode & 0x80FF) == 0){
	        pspDebugScreenPuts("vrnds.s  ");
     	 	vectors(a_opcode, 1, 0);
		}
	break;
//        { "vrnds.s", 0xD0200000, 0xFFFF80FF, "%ys" },

	case 0xD021:
		if((a_opcode & 0xFF80) == 0){
	        pspDebugScreenPuts("vrndi.s  ");
     	 	vectors(a_opcode, 2, 0);
		}
	break;
//        { "vrndi.s", 0xD0210000, 0xFFFFFF80, "%zs" },

	case 0xD022:
		if((a_opcode & 0xFF80) == 0){
	        pspDebugScreenPuts("vrndf1.s ");
     	 	vectors(a_opcode, 2, 0);
		}
	break;
//        { "vrndf1.s", 0xD0220000, 0xFFFFFF80, "%zs" },

	case 0xD023:
		if((a_opcode & 0xFF80) == 0){
	        pspDebugScreenPuts("vrndf2.s ");
     	 	vectors(a_opcode, 2, 0);
		}
	break;
//        { "vrndf2.s", 0xD0230000, 0xFFFFFF80, "%zs" },

	case 0xD033:
	        pspDebugScreenPuts("vh2f.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vh2f.s",      0xD0330000, 0xFFFF8080, "%zs, %ys" },

	case 0xD036:
	        pspDebugScreenPuts("vsbz.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vsbz.s",      0xD0360000, 0xFFFF8080, "%zs, %ys" },

	case 0xD03A:
	        pspDebugScreenPuts("vus2i.s  ");
		VFR=1;
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vus2i.s", 0xD03A0000, 0xFFFF8080, "" },
	case 0xD03B:
	        pspDebugScreenPuts("vs2i.s   ");
		VFR=1;
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vs2i.s",      0xD03B0000, 0xFFFF8080, "%zs, %ys" },

	case 0xD044:
	        pspDebugScreenPuts("vocp.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vocp.s",      0xD0440000, 0xFFFF8080, "%zs, %ys" },

	case 0xD045:
	        pspDebugScreenPuts("vsocp.s  ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vsocp.s", 0xD0450000, 0xFFFF8080, "%zs, %ys" },

	case 0xD04A:
	        pspDebugScreenPuts("vsgn.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vsgn.s",      0xD04A0000, 0xFFFF8080, "%zs, %ys" },

	case 0xD059:
	        pspDebugScreenPuts("vt4444.q ");
		VFR=1;
     	 	vectors(a_opcode, 2, 1);
		VFR=1;
     	 	vectors(a_opcode, 1, 0);
	break;
	case 0xD05A:
	        pspDebugScreenPuts("vt5551.q ");
		VFR=1;
     	 	vectors(a_opcode, 2, 1);
		VFR=1;
     	 	vectors(a_opcode, 1, 0);
	break;
	case 0xD05B:
	        pspDebugScreenPuts("vt5650.q ");
 		VFR=1;
     	 	vectors(a_opcode, 2, 1);
		VFR=1;
     	 	vectors(a_opcode, 1, 0);
	break;
//        { "vt4444.q",0xD0598080, 0xFFFF8080, "%zq, %yq" },
//        { "vt5551.q",0xD05A8080, 0xFFFF8080, "%zq, %yq" },
//        { "vt5650.q",0xD05B8080, 0xFFFF8080, "%zq, %yq" },
	}

	switch(a_opcode >>16 & 0xFFE0){
	case 0xD060:
	        pspDebugScreenPuts("vcst.s   ");
     	 	vectors(a_opcode, 2, 1);
		VFMODE=3;
     	 	vectors(a_opcode, 0, 0);
	break;
//        { "vcst.s",      0xD0600000, 0xFFE0FF80, "%zs, %ys, %xs" },

	case 0xD220:
	if((a_opcode & 0x8080) == 0){
	pspDebugScreenPuts("vf2in.s  ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
		mipsNibble(a_opcode,1,0);
	}
	else if((a_opcode & 0x8080) == 0x8080){
	pspDebugScreenPuts("vf2in.q  ");
		VFR=1;
		vectors(a_opcode, 2, 1);
		VFR=1;
     	 	vectors(a_opcode, 1, 1);
		mipsNibble(a_opcode,1,0);
	}
	break;
//        { "vf2in.q", 0xD2008080, 0xFFE08080, "" },
//        { "vf2in.s", 0xD2000000, 0xFFE08080, "" },

	case 0xD222:
	if((a_opcode & 0x8080) == 0){
	pspDebugScreenPuts("vf2iz.s    ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
		mipsNibble(a_opcode,1,0);
	}
	else if((a_opcode & 0x8080) == 0x8080){
	pspDebugScreenPuts("vf2iz.q    ");
		VFR=1;
		vectors(a_opcode, 2, 1);
		VFR=1;
     	 	vectors(a_opcode, 1, 1);
		mipsNibble(a_opcode,1,0);
	}
	break;
//        { "vf2iz.q", 0xD2208080, 0xFFE08080, "" },
//        { "vf2iz.s", 0xD2200000, 0xFFE08080, "" },

	case 0xD224:
	if((a_opcode & 0x8080) == 0){
	pspDebugScreenPuts("vf2iu.s    ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
		mipsNibble(a_opcode,1,0);
	}
	else if((a_opcode & 0x8080) == 0x8080){
	pspDebugScreenPuts("vf2iu.q    ");
		VFR=1;
		vectors(a_opcode, 2, 1);
		VFR=1;
     	 	vectors(a_opcode, 1, 1);
		mipsNibble(a_opcode,1,0);
	}
	break;
//        { "vf2iu.q", 0xD2408080, 0xFFE08080, "" },
//        { "vf2iu.s", 0xD2400000, 0xFFE08080, "" },

	case 0xD226:
	if((a_opcode & 0x8080) == 0){
	pspDebugScreenPuts("vf2id.s    ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
		mipsNibble(a_opcode,1,0);
	}
	else if((a_opcode & 0x8080) == 0x8080){
	pspDebugScreenPuts("vf2id.q    ");
		VFR=1;
		vectors(a_opcode, 2, 1);
		VFR=1;
     	 	vectors(a_opcode, 1, 1);
		mipsNibble(a_opcode,1,0);
	}
	break;
//        { "vf2id.q", 0xD2608080, 0xFFE08080, "" },
//        { "vf2id.s", 0xD2600000, 0xFFE08080, "" },
	}

	switch(a_opcode >>24){
	case 0xD3:
	pspDebugScreenPuts("vwbn.s   ");
		vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
	mipsImm(a_opcode,0,0);
//        { "vwbn.s",      0xD3000000, 0xFF008080, "" },
	break;
	}
      break;
      
      case 0xD4:
	switch(a_opcode & 0x2){
	case 0x00:
	pspDebugScreenPuts("lvl.q    ");
	VFR=1;
	vectors(a_opcode,0,1);
	VFR=3;
	mipsImm(a_opcode,0,0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
	break;

	case 0x02:
	pspDebugScreenPuts("lvr.q    ");
	VFR=1;
	vectors(a_opcode,0,1);
	VFR=3;
	mipsImm(a_opcode,0,0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
	break;
	}
      break;
//        { "lvl.q",       0xD4000000, 0xFC000002, "%Xq, %Y" },
//        { "lvr.q",       0xD4000002, 0xFC000002, "%Xq, %Y" },*/

      case 0xD8:
	if((a_opcode & 0x2) ==0){
	pspDebugScreenPuts("lv.q	");
	VFR=1;
	vectors(a_opcode,0,1);
	VFR=3;
	mipsImm(a_opcode,0,0);
        pspDebugScreenPuts("(");
        mipsRegister(a_opcode, S, 0);
        pspDebugScreenPuts(")");
	}
      break;
//        { "lv.q",        0xD8000000, 0xFC000002, "%Xq, %Y" },
      
      case 0xDC:
	switch(a_opcode >>24){
	case 0xDC:
	pspDebugScreenPuts("vpfxd    [");
	//Vmatrix(a_opcode, ,1);
	//Vmatrix(a_opcode, ,1);
	//Vmatrix(a_opcode, ,1);
	//Vmatrix(a_opcode, ,0);
	pspDebugScreenPuts("]");
	break;

	case 0xDD:
	pspDebugScreenPuts("vpfxs    [");
	//Vmatrix(a_opcode, ,1);
	//Vmatrix(a_opcode, ,1);
	//Vmatrix(a_opcode, ,1);
	//Vmatrix(a_opcode, ,0);
	pspDebugScreenPuts("]");
	break;

	case 0xDE:
	pspDebugScreenPuts("vpfxt    [");
	//Vmatrix(a_opcode, ,1);
	//Vmatrix(a_opcode, ,1);
	//Vmatrix(a_opcode, ,1);
	//Vmatrix(a_opcode, ,0);
	pspDebugScreenPuts("]");
	break;
//        { "vpfxd",       0xDE000000, 0xFF000000, "" },
//        { "vpfxs",       0xDC000000, 0xFF000000, "" },
//        { "vpfxt",       0xDD000000, 0xFF000000, "" },
	
	case 0xDF:
	switch(a_opcode >>16 & 0x80){
	case 0x00:
	pspDebugScreenPuts("viim.s   ");
	vectors(a_opcode,0,1);
	mipsImm(a_opcode,0,0);
	break;

	case 0x80:
	pspDebugScreenPuts("vfim.s   ");
	vectors(a_opcode,0,1);
	mipsImm(a_opcode,0,0);
	break;
//        { "viim.s",      0xDF000000, 0xFF800000, "" },
//        { "vfim.s",      0xDF800000, 0xFF800000, "" },
	}
	break;
	}
      break;
      
      case 0xE0:
      pspDebugScreenPuts("sc  	");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xE4:
      pspDebugScreenPuts("swc1	");
      floatRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xE8:
      pspDebugScreenPuts("sv.s	");
      VFR=3;
      vectors(a_opcode, 0, 1);
      VFR=3;
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      //case 0xEC:
      //break;
      
/*      case 0xF0:
"vtfm2.p"	"0xF0800080"	"0xFF808080"
"vtfm3.t"	"0xF1008000"	"0xFF808080"
"vtfm4.q"	"0xF1808080"	"0xFF808080"
      break;*/
      
      case 0xF4:
	switch(a_opcode & 0x2){
	case 0x00:
	pspDebugScreenPuts("svl.q    ");
	VFR=1;
	vectors(a_opcode,0,1);
	VFR=3;
	mipsImm(a_opcode,0,0);
        pspDebugScreenPuts("(");
        mipsRegister(a_opcode, S, 0);
        pspDebugScreenPuts(")");
	break;

	case 0x02:
	pspDebugScreenPuts("svr.q    ");
	VFR=1;
	vectors(a_opcode,0,1);
	VFR=3;
	mipsImm(a_opcode,0,0);
        pspDebugScreenPuts("(");
        mipsRegister(a_opcode, S, 0);
        pspDebugScreenPuts(")");
	break;
	}
      break;
//        { "svl.q",       0xF4000000, 0xFC000002, "%Xq, %Y" },
//        { "svr.q",       0xF4000002, 0xFC000002, "%Xq, %Y" },

     case 0xF8:
	switch(a_opcode & 0x2){
	case 0x00:
	pspDebugScreenPuts("sv.q	");
	VFR=1;
	vectors(a_opcode,0,1);
	VFR=3;
	mipsImm(a_opcode,0,0);
        pspDebugScreenPuts("(");
        mipsRegister(a_opcode, S, 0);
        pspDebugScreenPuts(")");
	break;
//      { "sv.q",        0xF8000000, 0xFC000002, "%Xq, %Y" },
	case 0x02:
	pspDebugScreenPuts("vwb.q    ");
	VFR=1;
	vectors(a_opcode,0,1);
	VFR=3;
	mipsImm(a_opcode,0,0);
        pspDebugScreenPuts("(");
        mipsRegister(a_opcode, S, 0);
        pspDebugScreenPuts(")");
	break;
//        { "vwb.q",       0xF8000002, 0xFC000002, "" },
	}
      break;
      
      case 0xFC:
      switch(a_opcode){
	case 0xFFFF0000:
	pspDebugScreenPuts("vnop  	");
	break;

	case 0xFFFF040D:
	pspDebugScreenPuts("vflush	");
	break;
//        { "vflush",      0xFFFF040D, 0xFFFFFFFF, "" },
//        { "vnop",        0xFFFF0000, 0xFFFFFFFF, "" },

	default:
	pspDebugScreenPuts("vsync    ");
	mipsImm(a_opcode,0,0);
	}
      break;
  }

  pspDebugScreenSetTextColor(color02);
}

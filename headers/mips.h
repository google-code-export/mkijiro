//MODED BY HAROTURBO
//Mips.h
#define S 0
#define T 1
#define D 2
unsigned char mipsNum[16];
unsigned char *mipsRegisterArray[]={"zero", "at", "v0", "v1", "a0", "a1", "a2", "a3", "t0", "t1", "t2", "t3", "t4", "t5", "t6", "t7", "s0", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "t8", "t9", "k0", "k1", "gp", "sp", "s8", "ra"};
unsigned char *specialRegisterArray[]={ "ixltg", "ixldt", "bxlbt", "$3", "ixstg", "ixsdt", "bxsbt", "ixin", "bpr", "$9", "bhinbt", "ihin", "bfh", "$13", "ifl", "$15", "dxltg", "dxldt", "dxstg", "dxsdt", "dxwbin", "$21", "dxin", "$23", "dhwbinv", "$25", "dhin", "$27", "dhwoin", "$29", "$30", "$31" };
unsigned char *floatRegisterArray[]={"$f0","$f1","$f2","$f3","$f4","$f5","$f6","$f7","$f8","$f9","$f10","$f11","$f12","$f13","$f14","$f15","$f16","$f17","$f18","$f19","$f20","$f21","$f22","$f23","$f24","$f25","$f26","$f27","$f28","$f29","$f30","$f31"};
unsigned char *vectorfloatRegisterArray[]={
"S000","S001" ,"S002" , "S003","S010","S011" ,"S012" , "S013",
"S020","S021" ,"S022" , "S023","S030","S031" ,"S032" , "S033",
"S100","S101" ,"S102" , "S103","S110","S111" ,"S112" , "S113",
"S120","S121" ,"S122" , "S123","S130","S131" ,"S132" , "S133",
"S200","S201" ,"S202" , "S203","S210","S211" ,"S212" , "S213",
"S220","S221" ,"S222" , "S223","S230","S231" ,"S232" , "S233",
"S300","S301" ,"S302" , "S303","S310","S311" ,"S312" , "S313",
"S320","S321" ,"S322" , "S323","S330","S331" ,"S332" , "S333",
"S400","S401" ,"S402" , "S403","S410","S411" ,"S412" , "S413",
"S420","S421" ,"S422" , "S423","S430","S431" ,"S432" , "S433",
"S500","S501" ,"S502" , "S503","S510","S511" ,"S512" , "S513",
"S520","S521" ,"S522" , "S523","S530","S531" ,"S532" , "S533",
"S600","S601" ,"S602" , "S603","S610","S611" ,"S612" , "S613",
"S620","S621" ,"S622" , "S623","S630","S631" ,"S632" , "S633",
"S700","S701" ,"S702" , "S703","S710","S711" ,"S712" , "S713",
"S720","S721" ,"S722" , "S723","S730","S731" ,"S732" , "S733",
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
  pspDebugScreenPuts(floatRegisterArray[a_opcode]);
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}

void vectorfloatRegister(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=((a_opcode>>(6+(5*(3-a_slot)))) & 0x1F)*4 + (a_opcode & 0x3);
  colorRegisters(a_opcode);
  pspDebugScreenPuts(vectorfloatRegisterArray[a_opcode]);
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}

void vectors(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F) + ((a_opcode>>(8*(2-a_slot))&0x7F)>>5);
  colorRegisters(a_opcode);
  pspDebugScreenPuts(vectorfloatRegisterArray[a_opcode]);
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}

void mipsImm2(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
    a_opcode&=0xFFFC;
    pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$"); pspDebugScreenSetTextColor(color02);
    sprintf(mipsNum, "%04X", a_opcode); pspDebugScreenPuts(mipsNum);
  
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

void mipsRegister(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  colorRegisters(a_opcode);
  pspDebugScreenPuts(mipsRegisterArray[a_opcode]);
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}

void mipsNFloat(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  pspDebugScreenPuts(floatRegisterArray[a_opcode]);
  pspDebugScreenSetTextColor(color02);
  if(a_more) pspDebugScreenPuts(", ");
}

void mipsNibble(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$"); pspDebugScreenSetTextColor(color02);
  sprintf(mipsNum, "%D", a_opcode); pspDebugScreenPuts(mipsNum);
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
            pspDebugScreenPuts("nop      ");
/*NOOP -- no operation
Description: Performs no operation.
Operation: advance_pc (4);
Syntax: noop
Encoding: 0000 0000 0000 0000 0000 0000 0000 0000*/
          }
          else
          {
            pspDebugScreenPuts("sll      ");
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
          pspDebugScreenPuts("srl      ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 1);
          mipsNibble(a_opcode, 3, 0);
/*SRL -- Shift right logical
Description: Shifts a register value right by the shift amount (shamt) and places the value in the destination register. Zeroes are shifted in.
Operation: $d = $t >> h; advance_pc (4);
Syntax: srl $d, $t, h
Encoding: 0000 00-- ---t tttt dddd dhhh hh00 0010*/
          break;

        case 0x03:
          pspDebugScreenPuts("sra      ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 1);
          mipsNibble(a_opcode, 3, 0);
/*SRA -- Shift right arithmetic
Description: Shifts a register value right by the shift amount (shamt) and places the value in the destination register. The sign bit is shifted in.
Operation: $d = $t >> h; advance_pc (4);
Syntax: sra $d, $t, h
Encoding: 0000 00-- ---t tttt dddd dhhh hh00 0011*/
          break;
          
        case 0x04:
          pspDebugScreenPuts("sllv     ");
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
          pspDebugScreenPuts("srlv     ");
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
          pspDebugScreenPuts("srav     ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 1);
          mipsRegister(a_opcode, 0, 0);
          break;
          
        case 0x08:
          pspDebugScreenPuts("jr       ");
          mipsRegister(a_opcode, 0, 0);
/*JR -- Jump register
Description: Jump to the address contained in register $s
Operation: PC = nPC; nPC = $s;
Syntax: jr $s
Encoding: 0000 00ss sss0 0000 0000 0000 0000 1000*/
          break;
          
        case 0x09:
          pspDebugScreenPuts("jalr     ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 0);
          break;
          
        case 0x0A:
          pspDebugScreenPuts("movz     ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
          
        case 0x0b:
          pspDebugScreenPuts("movn     ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
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
          pspDebugScreenPuts("sync     ");
          break;
          
        case 0x10:
          pspDebugScreenPuts("mfhi     ");
          mipsRegister(a_opcode, 2, 0);
/*MFHI -- Move from HI
Description: The contents of register HI are moved to the specified register.
Operation: $d = $HI; advance_pc (4);
Syntax: mfhi $d
Encoding: 0000 0000 0000 0000 dddd d000 0001 0000*/
        break;
          
        case 0x12:
          pspDebugScreenPuts("mflo     ");
          mipsRegister(a_opcode, 2, 0);
/*MFLO -- Move from LO
Description: The contents of register LO are moved to the specified register.
Operation: $d = $LO; advance_pc (4);
Syntax: mflo $d
Encoding: 0000 0000 0000 0000 dddd d000 0001 0010*/
          break;
          
         case 0x13:
          pspDebugScreenPuts("mtlo     ");
          mipsRegister(a_opcode, 2, 0);
          break;
         
         case 0x14:
          pspDebugScreenPuts("dsllv    ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
          
        case 0x16:
          pspDebugScreenPuts("dsrlv    ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
         break;
        
         case 0x17:
          pspDebugScreenPuts("dsrav    ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
         break;
        
        case 0x18:
          pspDebugScreenPuts("mult     ");
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
          pspDebugScreenPuts("div      ");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*DIV -- Divide
Description: Divides $s by $t and stores the quotient in $LO and the remainder in $HI
Operation: $LO = $s / $t; $HI = $s % $t; advance_pc (4);
Syntax: div $s, $t
Encoding: 0000 00ss ssst tttt 0000 0000 0001 1010*/
          break;
          
        case 0x1B:
          pspDebugScreenPuts("divu     ");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
/*DIVU -- Divide unsigned
Description: Divides $s by $t and stores the quotient in $LO and the remainder in $HI
Operation: $LO = $s / $t; $HI = $s % $t; advance_pc (4);
Syntax: divu $s, $t
Encoding: 0000 00ss ssst tttt 0000 0000 0001 1011*/
          break;
          
         case 0x1C:
          pspDebugScreenPuts("dmult    ");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
          
        case 0x1D:
          pspDebugScreenPuts("dmultu   ");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
          
         case 0x1E:
          pspDebugScreenPuts("ddiv     ");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
         break;
          
        case 0x1F:
          pspDebugScreenPuts("ddivu    ");
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
         break;
          
        case 0x20:
          pspDebugScreenPuts("add      ");
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
          pspDebugScreenPuts("addu     ");
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
          pspDebugScreenPuts("sub      ");
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
          pspDebugScreenPuts("subu     ");
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
          pspDebugScreenPuts("and      ");
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
          pspDebugScreenPuts("or       ");
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
          pspDebugScreenPuts("xor      ");
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
          pspDebugScreenPuts("nor      ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
          
          case 0x28:
          pspDebugScreenPuts("mfsa     ");
          mipsRegister(a_opcode, 2, 0);
          break;
          
          case 0x29:
          pspDebugScreenPuts("msta     ");
          mipsRegister(a_opcode, 2, 0);
          break;
          
        case 0x2A:
          pspDebugScreenPuts("slt      ");
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
          pspDebugScreenPuts("sltu     ");
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
          pspDebugScreenPuts("dadd     ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
          
          case 0x2d:
          pspDebugScreenPuts("daddu    ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
          
          case 0x2e:
          pspDebugScreenPuts("dsub     ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
          
          case 0x2F:
          pspDebugScreenPuts("dsubu    ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
          
          case 0x30:
           pspDebugScreenPuts("tge      ");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x31:
           pspDebugScreenPuts("tgeu     ");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x32:
           pspDebugScreenPuts("tlt      ");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x33:
           pspDebugScreenPuts("tltu     ");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x34:
           pspDebugScreenPuts("teq      ");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x36:
           pspDebugScreenPuts("tne      ");
           //mipsRegister(a_opcode, 0, 1);
           //mipsRegister(a_opcode, 1, 1);
           //mipsImmOther(a_opcode, 2, 0);
          break;
          
          case 0x38:
           pspDebugScreenPuts("dsll     ");
          break;
          
          case 0x3A:
           pspDebugScreenPuts("dsrl     ");
          break;
          
          case 0x3B:
           pspDebugScreenPuts("dsra     ");
          break;
          
          case 0x3C:
           pspDebugScreenPuts("dsll32   ");
          break;
          
          case 0x3E:
           pspDebugScreenPuts("dsrl32   ");
          break;
          
          case 0x3F:
           pspDebugScreenPuts("dsra32   ");
          break;
          
          
        default:
          pspDebugScreenPuts("???      ");
      }
      break;
      
    case 0x04:
      switch((a_opcode & 0x1F0000) >> 16)
      {
          case 0x00:
            pspDebugScreenPuts("bltz     ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
/*BLTZ -- Branch on less than zero
Description: Branches if the register is less than zero
Operation: if $s < 0 advance_pc (offset << 2)); else advance_pc (4);
Syntax: bltz $s, offset
Encoding: 0000 01ss sss0 0000 iiii iiii iiii iiii*/
          break;
            
          case 0x01:
            pspDebugScreenPuts("bgez     ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
/*BGEZ -- Branch on greater than or equal to zero
Description: Branches if the register is greater than or equal to zero
Operation: if $s >= 0 advance_pc (offset << 2)); else advance_pc (4);
Syntax: bgez $s, offset
Encoding: 0000 01ss sss0 0001 iiii iiii iiii iiii*/
          break;
          
           case 0x02:
            pspDebugScreenPuts("bltzl    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
            
           case 0x03:
            pspDebugScreenPuts("bgezl    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
           
           case 0x08:
            pspDebugScreenPuts("tgei     ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
          
           case 0x09:
            pspDebugScreenPuts("tgeiu    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
          
           case 0x0A:
            pspDebugScreenPuts("tlti     ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
           
           case 0x0B:
            pspDebugScreenPuts("tltiu    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
           
           case 0x0C:
            pspDebugScreenPuts("teqi     ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
           
           case 0x0E:
            pspDebugScreenPuts("tnei     ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
          
          case 0x10:
            pspDebugScreenPuts("bltzal   ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
/*BLTZAL -- Branch on less than zero and link
Description: Branches if the register is less than zero and saves the return address in $31
Operation: if $s < 0 $31 = PC + 8 (or nPC + 4); advance_pc (offset << 2)); else advance_pc (4);
Syntax: bltzal $s, offset
Encoding: 0000 01ss sss1 0000 iiii iiii iiii iiii*/
            break;
            
           case 0x11:
            pspDebugScreenPuts("bgezal   ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
/*BGEZAL -- Branch on greater than or equal to zero and link
Description: Branches if the register is greater than or equal to zero and saves the return address in $31
Operation: if $s >= 0 $31 = PC + 8 (or nPC + 4); advance_pc (offset << 2)); else advance_pc (4);
Syntax: bgezal $s, offset
Encoding: 0000 01ss sss1 0001 iiii iiii iiii iiii*/
           break;
            
           case 0x12:
            pspDebugScreenPuts("bltzall  ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
             
           case 0x13:
            pspDebugScreenPuts("bgezall  ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
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
            pspDebugScreenPuts("???      ");
      }
      break;
      
    case 0x08:
	if((a_opcode >= 0x08000000) && (a_opcode < 0x8800000)){
	pspDebugScreenPuts("kernelram");}
	else if((a_opcode >= 0x8800000) && (a_opcode < 0xA000000)){
	pspDebugScreenPuts("userram");	}
	else{
      pspDebugScreenPuts("j        ");
      mipsImm(a_opcode, 1, 0);}
/*J -- Jump
Description: Jumps to the calculated address
Operation: PC = nPC; nPC = (PC & 0xf0000000) | (target << 2);
Syntax: j target
Encoding: 0000 10ii iiii iiii iiii iiii iiii iiii*/
      break;
      
    case 0x0C:
      pspDebugScreenPuts("jal      ");
      mipsImm(a_opcode, 1, 0);
/*JAL -- Jump and link
Description: Jumps to the calculated address and stores the return address in $31
Operation: $31 = PC + 8 (or nPC + 4); PC = nPC; nPC = (PC & 0xf0000000) | (target << 2);
Syntax: jal target
Encoding: 0000 11ii iiii iiii iiii iiii iiii iiii*/
      break;
      
    case 0x10:
      pspDebugScreenPuts("beq      ");
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
      pspDebugScreenPuts("bne      ");
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
      pspDebugScreenPuts("blez     ");  
      mipsRegister(a_opcode, S, 1);
      mipsDec(a_opcode, 0, 0);
/*BLEZ -- Branch on less than or equal to zero
Description: Branches if the register is less than or equal to zero
Operation: if $s <= 0 advance_pc (offset << 2)); else advance_pc (4);
Syntax: blez $s, offset
Encoding: 0001 10ss sss0 0000 iiii iiii iiii iiii*/
      break;
      
    case 0x1C:
      pspDebugScreenPuts("bgtz     ");
      mipsRegister(a_opcode, S, 1);
      mipsDec(a_opcode, 0, 0);
/*BGTZ -- Branch on greater than zero
Description: Branches if the register is greater than zero
Operation: if $s > 0 advance_pc (offset << 2)); else advance_pc (4);
Syntax: bgtz $s, offset
Encoding: 0001 11ss sss0 0000 iiii iiii iiii iiii*/
      break;
      
    case 0x20:
      pspDebugScreenPuts("addi     ");
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
      pspDebugScreenPuts("slti     ");
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
      pspDebugScreenPuts("andi     ");
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
      pspDebugScreenPuts("ori      ");
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
      pspDebugScreenPuts("xori     ");
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
      pspDebugScreenPuts("lui      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
/*LUI -- Load upper immediate
Description: The immediate value is shifted left 16 bits and stored in the register. The lower 16 bits are zeroes.
Operation: $t = (imm << 16); advance_pc (4);
Syntax: lui $t, imm
Encoding: 0011 11-- ---t tttt iiii iiii iiii iiii*/
      break;

//FPU–½—ß
     case 0x44:
      switch(a_opcode >>24){

      //case 0x45:
      case 0x46:
		switch(a_opcode & 0x3F){

		case 0x00:
		pspDebugScreenPuts("add.s    ");
		mipsNFloat(a_opcode, 3, 1);
		mipsNFloat(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;
		
		case 0x01:
		pspDebugScreenPuts("sub.s    ");
		mipsNFloat(a_opcode, 3, 1);
		mipsNFloat(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x02:
		pspDebugScreenPuts("mul.s    ");
		mipsNFloat(a_opcode, 3, 1);
		mipsNFloat(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x03:
		pspDebugScreenPuts("div.s    ");
		mipsNFloat(a_opcode, 3, 1);
		mipsNFloat(a_opcode, 2, 1);
		floatRegister(a_opcode, T, 0);
		break;

		case 0x04:
		pspDebugScreenPuts("sqrt.s   ");
		mipsNFloat(a_opcode, 3, 1);
		mipsNFloat(a_opcode, 2, 0);
		break;

		case 0x05:
		pspDebugScreenPuts("abs.s    ");
		mipsNFloat(a_opcode, 3, 1);
		mipsNFloat(a_opcode, 2, 0);
		break;

		case 0x06:
		pspDebugScreenPuts("mov.s    ");
		mipsNFloat(a_opcode, 3, 1);
		mipsNFloat(a_opcode, 2, 0);
		break;
		}
     }break;

     case 0x50:
      pspDebugScreenPuts("beql     ");
      mipsRegister(a_opcode, S, 1);
      mipsRegister(a_opcode, T, 1);
      mipsDec(a_opcode, 0, 0);
     break;
      
     case 0x54:
      pspDebugScreenPuts("bnel     ");
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

		case 0x80:		
	        pspDebugScreenPuts("vsub.s   ");
     	 	vectors(a_opcode, 2, 1);
     	 	vectors(a_opcode, 1, 1);
     	 	vectors(a_opcode, 0, 0);
	        break;
        }break;
//    { "vadd.s",      0x60000000, 0xFF808080, "%zs, %yz, %xs" },
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
      pspDebugScreenPuts("ldl      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
     break;
     
     case 0x6C:
      pspDebugScreenPuts("ldr      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
     break;
     
     case 0x70:
      pspDebugScreenPuts("mmi      ");
      mipsImm(a_opcode, 1, 0);
     break;
     
      //0x1d is empty
      
     /*
      case 0x78:
      pspDebugScreenPuts("lq       ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
	 */     

     case 0x7C:
	if(a_opcode >>24 == 0x7C){
      switch(a_opcode & 0x7FF){
	case 0x420:
      	pspDebugScreenPuts("seb      ");
        mipsRegister(a_opcode, 2, 1);
        mipsRegister(a_opcode, 1, 0);
	break;
	
	case 0x620:
      	pspDebugScreenPuts("seh      ");
        mipsRegister(a_opcode, 2, 1);
        mipsRegister(a_opcode, 1, 0);
	break;

	case 0xA0:
      	pspDebugScreenPuts("wsbbn    ");
        mipsRegister(a_opcode, 2, 1);
        mipsRegister(a_opcode, 1, 0);
	break;
	
	case 0xE0:
      	pspDebugScreenPuts("wsbw     ");
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
	pspDebugScreenPuts("ext      ");
	mipsRegister(a_opcode, T, 1);
	mipsRegister(a_opcode, S, 1);
      	mipsNibble(a_opcode, 3, 1);
	a_opcode+=0x800;
      	mipsNibble(a_opcode, 2, 0);
	break;
	
	case 0x4:
	pspDebugScreenPuts("ins      ");
	mipsRegister(a_opcode, T, 1);
	mipsRegister(a_opcode, S, 1);
	mipsNibble(a_opcode, 3, 1);
	mipsins(a_opcode, 0, 0);
     	break;
	}
	break;

    case 0x80:
      pspDebugScreenPuts("lb       ");
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
      pspDebugScreenPuts("lh       ");
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
      pspDebugScreenPuts("lwl      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
    case 0x8C:
      pspDebugScreenPuts("lw       ");
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
      pspDebugScreenPuts("lbu      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0x94:
      pspDebugScreenPuts("lhu      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0x98:
      pspDebugScreenPuts("lwr      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
     case 0x9C:
      pspDebugScreenPuts("lwu      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
    case 0xA0:
      pspDebugScreenPuts("sb       ");
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
      pspDebugScreenPuts("sh       ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xA8:
      pspDebugScreenPuts("swl      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
    case 0xAC:
      pspDebugScreenPuts("sw       ");
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
      
      case 0xB0:
      pspDebugScreenPuts("sdl      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xB4:
      pspDebugScreenPuts("sdr      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xB8:
      pspDebugScreenPuts("swr      ");
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
      pspDebugScreenPuts("ll       ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xC4:
      pspDebugScreenPuts("lwc1     ");
      floatRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xC8:
      pspDebugScreenPuts("lv.s     ");
      vectorfloatRegister(a_opcode, T, 1);
      mipsImm2(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xCC:
      pspDebugScreenPuts("pref     ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xD0:
      pspDebugScreenPuts("lld      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xD4:
      pspDebugScreenPuts("ldc1     ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xD8:
      pspDebugScreenPuts("lqc2     ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xDC:
      pspDebugScreenPuts("ld       ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xE0:
      pspDebugScreenPuts("sc       ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xE4:
      pspDebugScreenPuts("swc1     ");
      floatRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xE8:
      pspDebugScreenPuts("sv.s     ");
      vectorfloatRegister(a_opcode, T, 1);
      mipsImm2(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      //case 0x3b:
      //break;
      
      case 0xF0:
      pspDebugScreenPuts("sdc      ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
      case 0xF4:
      pspDebugScreenPuts("sdc1     ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
     case 0xF8:
      pspDebugScreenPuts("sqc2     ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
      
     case 0xFC:
      pspDebugScreenPuts("sd       ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
     
    default:
      pspDebugScreenPuts("???      ");
  }

  pspDebugScreenSetTextColor(color02);
}

//MODED BY HAROTUBO
//reffer to PSPLINK/PRXTOOL SRC
/*
        WebSVN  
psp - Rev 2457
Subversion Repositories:
Rev:
(root)/trunk/prxtool/disasm.C @ 2457
Rev 2206 - Blame - Compare with Previous - Last modification - View Log - RSS feed

/***************************************************************
 * PRXTool : Utility for PSP executables.
 * (c) TyRaNiD 2k6
 *
 * disasm.C - Implementation of a MIPS disassembler
 ***************************************************************/
 
//Mips.h
#define S 0
#define T 1
#define D 2
unsigned int  b_opcode=0;
unsigned char VFMODE=0;
unsigned char VFR=0;
unsigned char VNUM=0;
unsigned char VMT=0;
unsigned char vmatrix=0;
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
unsigned char *unknown[]={
" //unknown opcode"," //unknown fpu opcode"," //unknown vfpu opcode"
};


void floatRegister(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  sprintf(buffer, "f%d" ,a_opcode); pspDebugScreenPuts(buffer);
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
  else{ //VFMODE=0
	unsigned char vectorflag=0;
	if(((a_opcode>>(8*(2-a_slot))) & 0x20) == 0x20){
	vectorflag=1;}
	else{
	vectorflag=0;}
	if(VMT==2){
	  if (vectorflag==1){
	  vmatrix=0x4D;vectorflag=0;}//M
	  else{
	  vmatrix=0x45;vectorflag=1;}//E
	}
	if(VMT==1){
	  if (vectorflag==1){
	  vmatrix=0x45;}//E
	  else{
	  vmatrix=0x4D;}//M
	}
	if(VMT==0){
	 if (vectorflag==1){
	 vmatrix=0x52;}//R
	 else{
	 vmatrix=0x43;}//C
	}
        if(VFR==1){ //lvq,svq
        if(((a_opcode & 0x1) == 1) && ((a_opcode >>24 == 0xD4) || (a_opcode >>24 == 0xD8) || (a_opcode >>24 == 0xF4) || (a_opcode >>24 == 0xF8))){
        sprintf(buffer, "R%d0%d",((a_opcode>>(8*(2-a_slot)))&0x1F)/4 , (a_opcode>>(8*(2-a_slot)))& 0x3 );
        a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F) + (a_opcode & 0x3);
                }
        else{
        sprintf(buffer, "C%d%d0",((a_opcode>>(8*(2-a_slot)))&0x1F)/4 , (a_opcode>>(8*(2-a_slot)))& 0x3 );
        a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F);
                }
        }
        else if(VFR==4){ //pair vector
        if (vectorflag==1){
        sprintf(buffer, "%c%d%d%d",vmatrix,((a_opcode>>(8*(2-a_slot)))&0x1F)/4 ,((a_opcode>>(8*(2-a_slot)))& 0x40)/0x20 ,(a_opcode>>(8*(2-a_slot)))& 0x3);
        a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1C)+ 8*(((a_opcode>>(8*(2-a_slot)))& 0x40)/0x20) +((a_opcode>>(8*(2-a_slot)))& 0x3);
                }
        else{
        sprintf(buffer, "%c%d%d%d",vmatrix,((a_opcode>>(8*(2-a_slot)))&0x1F)/4 , (a_opcode>>(8*(2-a_slot)))& 0x3,((a_opcode>>(8*(2-a_slot))& 0x40)/0x20));
        a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F) +((a_opcode>>(8*(2-a_slot)))& 0x40)/0x20;
                }
        }
        else if(VFR==5){ //triple vector
        if (vectorflag ==1){
        sprintf(buffer, "%c%d%d%d",vmatrix,((a_opcode>>(8*(2-a_slot)))&0x1F)/4 ,((a_opcode>>(8*(2-a_slot)))& 0x40)/0x40 ,(a_opcode>>(8*(2-a_slot)))& 0x3);
        a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1C)+ 4*(((a_opcode>>(8*(2-a_slot)))& 0x40)/0x40) +((a_opcode>>(8*(2-a_slot)))& 0x3);
                }
        else{
        sprintf(buffer, "%c%d%d%d",vmatrix,((a_opcode>>(8*(2-a_slot)))&0x1F)/4 , (a_opcode>>(8*(2-a_slot)))& 0x3,((a_opcode>>(8*(2-a_slot))& 0x40)/0x40));
        a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F) +((a_opcode>>(8*(2-a_slot)))& 0x40)/0x40;
                }
        }
        else if(VFR==6){ //quad vector
        if (vectorflag==1){
        sprintf(buffer, "%c%d0%d",vmatrix,((a_opcode>>(8*(2-a_slot)))&0x1F)/4 ,(a_opcode>>(8*(2-a_slot)))& 0x3);
        a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1C)+((a_opcode>>(8*(2-a_slot)))& 0x3);
                }
        else{
        sprintf(buffer, "%c%d%d0",vmatrix,((a_opcode>>(8*(2-a_slot)))&0x1F)/4 , (a_opcode>>(8*(2-a_slot)))& 0x3);
        a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F);
                }
        }
        else if(VFR==3){//lv.s sv.s
        sprintf(buffer, "S%d%d%d" , (a_opcode >> 18) & 7, (a_opcode >> 16) & 3, a_opcode & 3);
        a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F) + (a_opcode & 0x3);
        }
        else{ //VFR==0
        sprintf(buffer, "S%d%d%d",((a_opcode>>(8*(2-a_slot)))>>2)& 0x7 , (a_opcode>>(8*(2-a_slot)))& 0x3 , (a_opcode>>(8*(2-a_slot))&0x7F)>>5);
        a_opcode=4*((a_opcode>>(8*(2-a_slot)))& 0x1F) + ((a_opcode>>(8*(2-a_slot))&0x7F)>>5);
        }
  //colorRegisters(a_opcode);
  }
  VFR=0;
  VFMODE=0;
  VMT=0;
  pspDebugScreenPuts(buffer);
  if(a_more) pspDebugScreenPuts(", ");
}

void specialRegister(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  
  if(a_opcode != 0x03 || 0x09 || 0x0D || 0x0F || 0x15 || 0x17 || 0x19 || 0x1B || 0x1D || 0x1E || 0x1F){
        pspDebugScreenPuts(specialRegisterArray[a_opcode]);
  }
  else{
        pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$");
        sprintf(buffer, "%2d", a_opcode); pspDebugScreenPuts(buffer);
  }
  if(a_more) pspDebugScreenPuts(", ");
}

void VectorCMP(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode&=0xF;
  pspDebugScreenPuts(VFPUCMPArray[a_opcode]);
  if(a_more) pspDebugScreenPuts(", ");
}

void mipsRegister(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  pspDebugScreenPuts(mipsRegisterArray[a_opcode]);
  if(a_more) pspDebugScreenPuts(", ");
}

void cop0Register(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  pspDebugScreenPuts("COP0_");
  pspDebugScreenPuts(cop0Array[a_opcode]);
  if(a_more) pspDebugScreenPuts(", ");
}

void DrRegister(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x0F;
  pspDebugScreenPuts("DEBUG_");
  pspDebugScreenPuts(DrArray[a_opcode]);
  if(a_more) pspDebugScreenPuts(", ");
}


void mipsNibble(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=(a_opcode>>(6+(5*(3-a_slot)))) & 0x1F;
  pspDebugScreenSetTextColor(0xFF999999);
  sprintf(mipsNum, "$%D", a_opcode);
  pspDebugScreenPuts(mipsNum);
  if(a_more) pspDebugScreenPuts(", ");
}


void mipsins(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  a_opcode=((a_opcode>>11)& 0x1F )- ((a_opcode>>6)  & 0x1F) +1;
  pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$");
  sprintf(mipsNum, "%D", a_opcode); pspDebugScreenPuts(mipsNum);
  if(a_more) pspDebugScreenPuts(", ");
}

void mipsImm(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  unsigned char sign=0;
  if(a_slot== 0xA){ //dec
    a_opcode&=0xFFFF;
    if(a_opcode > 0x7FFF){
    	a_opcode=0x10000 - a_opcode;
    	sign=0x2D;//-
    	}
    else{
    	sign=0x2B;//+
    	}
    sprintf(mipsNum, "%c%D", sign, a_opcode);
  }
  else{
   pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$");
  if(a_slot==1)
  {
    a_opcode&=0x3FFFFFF;
    sprintf(mipsNum, "%08X", ((a_opcode<<2)));
  }
  else if(VFR==3){ //sv.q,lv.q,lv.s,sv.s
    a_opcode&=0xFFFC;
    sprintf(mipsNum, "%04X", a_opcode);
  }
  else 
  {
    a_opcode&=0xFFFF;
    sprintf(mipsNum, "%04X", a_opcode);
  }
  }
  pspDebugScreenPuts(mipsNum);
  VFR=0;
  if(a_more) pspDebugScreenPuts(", ");
}
/*
void mipsDec(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
  if(a_slot==1)
  {
    a_opcode&=0x3FFFFFF;
    pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$");
    sprintf(mipsNum, "%010lu", ((a_opcode<<2))); pspDebugScreenPuts(mipsNum);
  }
  else if(a_slot==2){ //DEC
    a_opcode&=0xFF;pspDebugScreenSetTextColor(0xFF999999); pspDebugScreenPuts("$");
    sprintf(mipsNum, "%02D", a_opcode); pspDebugScreenPuts(mipsNum);
  }
  if(a_more) pspDebugScreenPuts(", ");
}*/

void vrot(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{

//Special handling of the vrot instructions.
#define VFPU_MASK_OP_SIZE       0x8080  //Masks the operand size (pair, triple, quad).
#define VFPU_OP_SIZE_PAIR       0x80
#define VFPU_OP_SIZE_TRIPLE     0x8000
#define VFPU_OP_SIZE_QUAD       0x8080

// Note that these are within the rotators field, and not the full opcode.
#define VFPU_SH_ROT_HI          2
#define VFPU_MASK_ROT_HI        0x3
#define VFPU_SH_ROT_LO          0
#define VFPU_MASK_ROT_LO        0x3
#define VFPU_SH_ROT_NEG         4       //Negation.
#define VFPU_MASK_ROT_NEG       0x1

        const char *elements[4];
		
        unsigned int opcode = a_opcode & VFPU_MASK_OP_SIZE;
        unsigned int rotators = (a_opcode >> 16) & 0x1f;
        unsigned int opsize, rothi, rotlo, negation, i;

        //Determine the operand size so we'll know how many elements to output.
        if (opcode == VFPU_OP_SIZE_PAIR){
                opsize = 2;}
        else if (opcode == VFPU_OP_SIZE_TRIPLE){
                opsize = 3;}
        else{
                opsize = (opcode == VFPU_OP_SIZE_QUAD) * 4;}     //Sanity check. 

        rothi = (rotators >> VFPU_SH_ROT_HI) & VFPU_MASK_ROT_HI;
        rotlo = (rotators >> VFPU_SH_ROT_LO) & VFPU_MASK_ROT_LO;
        negation = (rotators >> VFPU_SH_ROT_NEG) & VFPU_MASK_ROT_NEG;

        pspDebugScreenPuts("[");

        for (i = 0;;)
        {
        
        if (rothi == rotlo)
        {
                if (negation)
                {
                        elements[0] = 0x73;//-s
                        elements[1] = 0x73;
                        elements[2] = 0x73;
                        elements[3] = 0x73;
                }
                else
                {
                        elements[0] = 0x73;
                        elements[1] = 0x73;
                        elements[2] = 0x73;
                        elements[3] = 0x73;
                }
        }
        else
        {
                elements[0] = 0x30;//0
                elements[1] = 0x30;
                elements[2] = 0x30;
                elements[3] = 0x30;
        }
        if (negation){
                elements[rothi] =0x73;//-s
                }
        else{
                elements[rothi] =0x73;//s
		        elements[rotlo] =0x63;//c
		}
			   if(negation){
			   pspDebugScreenPuts("-");
			   }
    		   sprintf(buffer, "%c", elements[i++]); pspDebugScreenPuts(buffer);
                if (i >= opsize)
                        break;
               pspDebugScreenPuts(" ,");
        }
		
        pspDebugScreenPuts("]");
}

// [hlide] added print_vfpu_prefix
void vprefix(unsigned int a_opcode, unsigned char pos, unsigned char a_more)
{
// VFPU prefix instruction operands.  The *_SH_* values really specify where
//   the bitfield begins, as VFPU prefix instructions have four operands
//   encoded within the immediate field. 
#define VFPU_SH_PFX_NEG         16
#define VFPU_MASK_PFX_NEG       0x1     // Negation.
#define VFPU_SH_PFX_CST         12
#define VFPU_MASK_PFX_CST       0x1     // Constant.
#define VFPU_SH_PFX_ABS_CSTHI   8
#define VFPU_MASK_PFX_ABS_CSTHI 0x1     // Abs/Constant (bit 2).
#define VFPU_SH_PFX_SWZ_CSTLO   0
#define VFPU_MASK_PFX_SWZ_CSTLO 0x3     // Swizzle/Constant (bits 0-1).
#define VFPU_SH_PFX_MASK        8
#define VFPU_MASK_PFX_MASK      0x1     // Mask.
#define VFPU_SH_PFX_SAT         0
#define VFPU_MASK_PFX_SAT       0x3     // Saturation.

unsigned char *pfx_cst[8] =
{
  "0",  "1",  "2",  "1/2",  "3",  "1/3",  "1/4",  "1/6"
};
unsigned char *pfx_swz[4] ={
  "x",  "y",  "z",  "w" 
};
unsigned char *pfx_sat[4] ={
  "",  "[0:1]",  "",  "[-1:1]"
};

        switch (pos)
        {
        case 0x30:
        case 0x31:
        case 0x32:
        case 0x33:
                {
                        unsigned int base = 0x30;
                        unsigned int negation = (a_opcode >> (pos - (base - VFPU_SH_PFX_NEG))) & VFPU_MASK_PFX_NEG;
                        unsigned int constant = (a_opcode >> (pos - (base - VFPU_SH_PFX_CST))) & VFPU_MASK_PFX_CST;
                        unsigned int abs_consthi = (a_opcode >> (pos - (base - VFPU_SH_PFX_ABS_CSTHI))) & VFPU_MASK_PFX_ABS_CSTHI;
                        unsigned int swz_constlo = (a_opcode >> ((pos - base) * 2)) & VFPU_MASK_PFX_SWZ_CSTLO;

                        if (negation)
                                 pspDebugScreenPuts("-");
                        if (constant)
                        {
                                pspDebugScreenPuts(pfx_cst[(abs_consthi << 2) | swz_constlo]);
                        }
                        else
                        {
                                if (abs_consthi){
                                pspDebugScreenPuts("|");
                                pspDebugScreenPuts(pfx_swz[swz_constlo]);
                                pspDebugScreenPuts("|");
                                }
                                else{
                                pspDebugScreenPuts(pfx_swz[swz_constlo]);
                                }
                        }
                }
                break;
                
        case 0x34:
        case 0x35:
        case 0x36:
        case 0x37:
                {
                        unsigned int base = 0x34;
                        unsigned int mask = (a_opcode >> (pos - (base - VFPU_SH_PFX_MASK))) & VFPU_MASK_PFX_MASK;
                        unsigned int saturation = (a_opcode >> ((pos - base) * 2)) & VFPU_MASK_PFX_SAT;

                        if (mask){
                                pspDebugScreenPuts("m");
                                }
                        else{
                                pspDebugScreenPuts(pfx_sat[saturation]);
                                }
                }
                break;
        }
  if(a_more) pspDebugScreenPuts(", ");
}


//VECTOR SELECTOR
void vsel(unsigned int a_opcode, unsigned char a_slot, unsigned char a_more)
{
unsigned char i; 
  switch(a_opcode & 0x8080)
  {
    case 0x00:
        pspDebugScreenPuts("s");
        for(i=0; i < a_more; i++){
        pspDebugScreenPuts(" ");
        }
        if(VNUM==1){
        vectors(a_opcode, 2, 0);
        }
        else{
        vectors(a_opcode, 2, 1);
        if(VNUM == 3){
        vectors(a_opcode, 1, 1);
        vectors(a_opcode, 0, 0);
        }
        else{
        vectors(a_opcode, 1, 0);
        }
        }
        VNUM=0;
    break;

    case 0x0080:
        pspDebugScreenPuts("p");
        for(i=0; i < a_more; i++){
        pspDebugScreenPuts(" ");
        }
        if(((a_opcode  & 0xFF800000) != 0x64800000) &&
	((a_opcode  & 0xFF800000) != 0x66000000) &&
	((a_opcode  & 0xFF800000) != 0x67000000)){
	VFR=4;}
        if(VNUM==1){
        vectors(a_opcode, 2, 0);
        }
        else{
        vectors(a_opcode, 2, 1);
        if(VNUM == 3){
        VFR=4;
        if((a_opcode  & 0xFF800000) == 0xF0000000){ 
	VMT=2;
	}
        vectors(a_opcode, 1, 1);
        VFR=4;
        if((a_opcode  & 0xFF800000) == 0xF0000000){
	VMT=1;
	}
        vectors(a_opcode, 0, 0);
        }
        else{
        VFR=4;
	    if((a_opcode  & 0xFFE00000) == 0xF3A00000){
        VFR=0;
		}
        else if(((a_opcode  & 0xFF800000) == 0xF3800000) || 
	((a_opcode  & 0xFF800000) == 0xF2000000)){
	VMT=1;
	}
        vectors(a_opcode, 1, 0);
        }
        }
        VNUM=0;
    break;

    case 0x8000:
        pspDebugScreenPuts("t");
        for(i=0; i < a_more; i++){
        pspDebugScreenPuts(" ");
        }
        if(((a_opcode  & 0xFF800000) != 0x64800000) &&
	((a_opcode  & 0xFF800000) != 0x66000000) &&
	((a_opcode  & 0xFF800000) != 0x67000000)){
	VFR=5;}
        if(VNUM==1){
        vectors(a_opcode, 2, 0);
        }
        else{
        vectors(a_opcode, 2, 1);
        if(VNUM == 3){
        VFR=5;
        if((a_opcode  & 0xFF800000) == 0xF0000000){ 
	VMT=2;
	}
        vectors(a_opcode, 1, 1);
        VFR=5;
        if((a_opcode  & 0xFF800000) == 0xF0000000){
	VMT=1;
	}
        vectors(a_opcode, 0, 0);
        }
        else{
        VFR=5;
	    if((a_opcode  & 0xFFE00000) == 0xF3A00000){
	    VFR=0;
		}
        else if(((a_opcode  & 0xFF800000) == 0xF3800000) || 
	((a_opcode  & 0xFF800000) == 0xF2000000)){
	VMT=1;
	}
        vectors(a_opcode, 1, 0);
        }
        }
        VNUM=0;
    break;

    case 0x8080:
        pspDebugScreenPuts("q");
        for(i=0; i < a_more; i++){
        pspDebugScreenPuts(" ");
        }
        if(((a_opcode  & 0xFF800000) != 0x64800000) &&
	((a_opcode  & 0xFF800000) != 0x66000000) &&
	((a_opcode  & 0xFF800000) != 0x67000000)){
	VFR=6;}
        if(VNUM==1){
        vectors(a_opcode, 2, 0);
        }
        else{
        vectors(a_opcode, 2, 1);
        if(VNUM == 3){
        VFR=6;
        if((a_opcode  & 0xFF800000) == 0xF0000000){ 
	VMT=2;
	}
        vectors(a_opcode, 1, 1);
        VFR=6;
        if((a_opcode  & 0xFF800000) == 0xF0000000){
	VMT=1;
	}
        vectors(a_opcode, 0, 0);
        }
        else{
        VFR=6;        
	    if((a_opcode  & 0xFFE00000) == 0xF3A00000){
	    VFR=0;
		}
        else if(((a_opcode  & 0xFF800000) == 0xF3800000) || 
	((a_opcode  & 0xFF800000) == 0xF2000000)){
	VMT=1;
	}	
        vectors(a_opcode, 1, 0);
        }
        }
        VNUM=0;
    break;
  }
}

void mipsDecode(unsigned int a_opcode)
{

  //Handle opcode
  switch((a_opcode >>24)& 0xFC)
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
        if(((a_opcode >>16) & 0xE0) == 0x20){
          pspDebugScreenPuts("rotr     ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 1, 1);
          mipsNibble(a_opcode, 3, 0);
        }
//        { "rotr",               0x00200002, 0xFFE0003F, "%d, %t, %a"},
        else{
          pspDebugScreenPuts("srl      ");
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
          pspDebugScreenPuts("sync      ");
          break;
          
/*          case 0x28:
          pspDebugScreenPuts("mfsa      ");
          mipsRegister(a_opcode, 2, 0);
          break;
          
          case 0x29:
          pspDebugScreenPuts("msta      ");
          mipsRegister(a_opcode, 2, 0);
          break;*/

/*          case 0x30:
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
          break;*/
        }

        switch(a_opcode & 0x7FF){
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
          
        case 0x10:
          pspDebugScreenPuts("mfhi     ");
          mipsRegister(a_opcode, 2, 0);
/*MFHI -- Move from HI
Description: The contents of register HI are moved to the specified register.
Operation: $d = $HI; advance_pc (4);
Syntax: mfhi $d
Encoding: 0000 0000 0000 0000 dddd d000 0001 0000*/
        break;

        case 0x11:
          pspDebugScreenPuts("mthi     ");
          mipsRegister(a_opcode, 2, 0);
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

        case 0x16:
          pspDebugScreenPuts("clz      ");
          mipsRegister(a_opcode, 1, 1);
          mipsRegister(a_opcode, 2, 0);
         break;
//        { "clo",                0x00000017, 0xFC1F07FF, "%d, %s"},
//        { "clz",                0x00000016, 0xFC1F07FF, "%d, %s"},

         case 0x17:
          pspDebugScreenPuts("clo      ");
          mipsRegister(a_opcode, 1, 1);
          mipsRegister(a_opcode, 2, 0);
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
          pspDebugScreenPuts("max      ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
//        { "max",                0x0000002C, 0xFC0007FF, "%d, %s, %t"},
//        { "min",                0x0000002D, 0xFC0007FF, "%d, %s, %t"},
          
          case 0x2d:
          pspDebugScreenPuts("min      ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;

        case 0x46:
          pspDebugScreenPuts("rotv     ");
          mipsRegister(a_opcode, 2, 1);
          mipsRegister(a_opcode, 0, 1);
          mipsRegister(a_opcode, 1, 0);
          break;
//        { "rotv",               0x00000046, 0xFC0007FF, "%d, %t, %s"},
        }

        switch(a_opcode & 0xFFFF){
        case 0x08:
          pspDebugScreenPuts("jr       ");
          mipsRegister(a_opcode, 0, 0);
/*JR -- Jump register
Description: Jump to the address contained in register $s
Operation: PC = nPC; nPC = $s;
Syntax: jr $s
Encoding: 0000 00ss sss0 0000 0000 0000 0000 1000*/
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
          pspDebugScreenPuts("madd     ");
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
          pspDebugScreenPuts("msub     ");
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
            pspDebugScreenPuts("bltz     ");
            mipsRegister(a_opcode, S, 1);
            //mipsDec(a_opcode,0,0);
/*BLTZ -- Branch on less than zero
Description: Branches if the register is less than zero
Operation: if $s < 0 advance_pc (offset << 2)); else advance_pc (4);
Syntax: bltz $s, offset
Encoding: 0000 01ss sss0 0000 iiii iiii iiii iiii*/
          break;
            
          case 0x01:
            pspDebugScreenPuts("bgez     ");
            mipsRegister(a_opcode, S, 1);
            //mipsDec(a_opcode,0,0);
/*BGEZ -- Branch on greater than or equal to zero
Description: Branches if the register is greater than or equal to zero
Operation: if $s >= 0 advance_pc (offset << 2)); else advance_pc (4);
Syntax: bgez $s, offset
Encoding: 0000 01ss sss0 0001 iiii iiii iiii iiii*/
          break;
          
           case 0x02:
            pspDebugScreenPuts("bltzl    ");
            mipsRegister(a_opcode, S, 1);
            //mipsDec(a_opcode,0,0);
           break;
            
           case 0x03:
            pspDebugScreenPuts("bgezl    ");
            mipsRegister(a_opcode, S, 1);
            //mipsDec(a_opcode,0,0);
           break;
           
           case 0x08:
            pspDebugScreenPuts("tgei    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
          
           case 0x09:
            pspDebugScreenPuts("tgeiu    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
          
           case 0x0A:
            pspDebugScreenPuts("tlti    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
           
           case 0x0B:
            pspDebugScreenPuts("tltiu    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
           
           case 0x0C:
            pspDebugScreenPuts("teqi    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
           
           case 0x0E:
            pspDebugScreenPuts("tnei    ");
            mipsRegister(a_opcode, S, 1);
            mipsImm(a_opcode, 0, 0);
           break;
          
          case 0x10:
            pspDebugScreenPuts("bltzal   ");
            mipsRegister(a_opcode, S, 1);
            //mipsDec(a_opcode,0,0);
/*BLTZAL -- Branch on less than zero and link
Description: Branches if the register is less than zero and saves the return address in $31
Operation: if $s < 0 $31 = PC + 8 (or nPC + 4); advance_pc (offset << 2)); else advance_pc (4);
Syntax: bltzal $s, offset
Encoding: 0000 01ss sss1 0000 iiii iiii iiii iiii*/
            break;
            
           case 0x11:
            pspDebugScreenPuts("bgezal   ");
            mipsRegister(a_opcode, S, 1);
            //mipsDec(a_opcode,0,0);
/*BGEZAL -- Branch on greater than or equal to zero and link
Description: Branches if the register is greater than or equal to zero and saves the return address in $31
Operation: if $s >= 0 $31 = PC + 8 (or nPC + 4); advance_pc (offset << 2)); else advance_pc (4);
Syntax: bgezal $s, offset
Encoding: 0000 01ss sss1 0001 iiii iiii iiii iiii*/
           break;
            
           case 0x12:
            pspDebugScreenPuts("bltzall  ");
            mipsRegister(a_opcode, S, 1);
            //mipsDec(a_opcode,0,0);
           break;
             
           case 0x13:
            pspDebugScreenPuts("bgezall  ");
            mipsRegister(a_opcode, S, 1);
            //mipsDec(a_opcode,0,0);
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
            pspDebugScreenPuts("???     ");
      }
      break;
      
    case 0x08:
        if((a_opcode >= 0x08000000) && (a_opcode < 0x8800000)){
        pspDebugScreenPuts("kernelram");}
        else if((a_opcode >= 0x8800000) && (a_opcode < 0xA000000)){
        pspDebugScreenPuts("userram");  }
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
      //mipsDec(a_opcode, 0, 0);
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
      //mipsDec(a_opcode, 0, 0);
/*BNE -- Branch on not equal
Description: Branches if the two registers are not equal
Operation: if $s != $t advance_pc (offset << 2)); else advance_pc (4);
Syntax: bne $s, $t, offset
Encoding: 0001 01ss ssst tttt iiii iiii iiii iiii*/
      break;
      
    case 0x18:
      pspDebugScreenPuts("blez     ");  
      mipsRegister(a_opcode, S, 1);
      //mipsDec(a_opcode, 0, 0);
/*BLEZ -- Branch on less than or equal to zero
Description: Branches if the register is less than or equal to zero
Operation: if $s <= 0 advance_pc (offset << 2)); else advance_pc (4);
Syntax: blez $s, offset
Encoding: 0001 10ss sss0 0000 iiii iiii iiii iiii*/
      break;
      
    case 0x1C:
      pspDebugScreenPuts("bgtz     ");
      mipsRegister(a_opcode, S, 1);
      //mipsDec(a_opcode, 0, 0);
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
      
      case 0x40:
        switch(a_opcode >> 20 & 0xFFE){
        case 0x400:
                pspDebugScreenPuts("mfc0     ");
                mipsRegister(a_opcode, T, 1);
                cop0Register(a_opcode, 2, 0);
        break;

        case 0x408:
                pspDebugScreenPuts("mtc0     ");
                mipsRegister(a_opcode, T, 1);
                cop0Register(a_opcode, 2, 0);
        break;

        case 0x404:
                pspDebugScreenPuts("cfc0     ");
                mipsRegister(a_opcode, T, 1);
                mipsNibble(a_opcode, 2, 0);
        break;


        case 0x40C:
                pspDebugScreenPuts("ctc0     ");
                mipsRegister(a_opcode, T, 1);
                mipsNibble(a_opcode, 2, 0);
        break;
//"cfc0",               0x40400000, 0xFFE007FF, "%t, %p"
//"ctc0",               0x40C00000, 0xFFE007FF, "%t, %p"
//"mfc0",               0x40000000, 0xFFE007FF, "%t, %0"
//"mtc0",               0x40800000, 0xFFE007FF, "%t, %0"

        case 0x420:
        if((a_opcode & 0xFFFFFF)  == 0x18){
                pspDebugScreenPuts("eret     ");
                }
        break;
        }
      break;


//FPU???s
     case 0x44:
      switch(a_opcode >>24){
                case 0x44:
                switch(a_opcode >> 16 & 0xE0){
                case 0x40:
                pspDebugScreenPuts("cfc1     ");
                mipsRegister(a_opcode, T, 1);
                mipsNibble(a_opcode, 2, 0);
                break;

                case 0xC0:
                pspDebugScreenPuts("ctc1     ");
                mipsRegister(a_opcode, T, 1);
                mipsNibble(a_opcode, 2, 0);
                break;

                case 0x00:
                pspDebugScreenPuts("mfc1     ");
                mipsRegister(a_opcode, T, 1);
                floatRegister(a_opcode,2,0);
                break;

                case 0x80:
                pspDebugScreenPuts("mtc1     ");
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
                pspDebugScreenPuts("bc1f     ");
                //mipsDec(a_opcode, 0 ,0 );
                break;

                case 0x01:
                pspDebugScreenPuts("bc1t     ");
                //mipsDec(a_opcode, 0 ,0 );
                break;

                case 0x02:
                pspDebugScreenPuts("bc1fl    ");
                //mipsDec(a_opcode, 0 ,0 );
                break;

                case 0x03:
                pspDebugScreenPuts("bc1tl    ");
                //mipsDec(a_opcode, 0 ,0 );
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
                pspDebugScreenPuts("mfv      ");
                mipsRegister(a_opcode, T, 1);
                vectors(a_opcode , 2 , 0);
                }
                else if((a_opcode & 0xFF00) == 0){
                pspDebugScreenPuts("mfvc     ");
                mipsRegister(a_opcode, T, 1);
                VFMODE=1;
                vectors(a_opcode, 2, 0);
                }
        break;

        case 0xE0:
                if((a_opcode & 0xFF80) == 0){
                pspDebugScreenPuts("mtv      ");
                mipsRegister(a_opcode, T, 1);
                vectors(a_opcode , 2 , 0);
                }
                else if((a_opcode & 0xFF00) == 0){
                pspDebugScreenPuts("mtvc     ");
                mipsRegister(a_opcode, T, 1);
                VFMODE=1;
                vectors(a_opcode, 2, 0);
                }
        break;
        }break;
/*
{ "mfv", 0x48600000, 0xFFE0FF80, "%t, %zs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%t, %zs"
{ "mfvc",0x48600000, 0xFFE0FF00, "%t, %2d" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%t, %2d"
{ "mtv", 0x48E00000, 0xFFE0FF80, "%t, %zs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%t, %zs"
{ "mtvc",0x48E00000, 0xFFE0FF00, "%t, %2d" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%t, %2d"
*/
      case 0x49:
                switch(a_opcode >>16 & 0xE3){
                case 0x00:
                pspDebugScreenPuts("bvf      ");
                VFMODE=2;
                vectors(a_opcode, 0, 1);
                //mipsDec(a_opcode, 0, 0);
                break;

                case 0x01:
                pspDebugScreenPuts("bvt      ");
                VFMODE=2;
                vectors(a_opcode, 0, 1);
                //mipsDec(a_opcode, 0, 0);
                break;

                case 0x02:
                pspDebugScreenPuts("bvfl     ");
                VFMODE=2;
                vectors(a_opcode, 0, 1);
                //mipsDec(a_opcode, 0, 0);
                break;

                case 0x03:
                pspDebugScreenPuts("bvtl     ");
                VFMODE=2;
                vectors(a_opcode, 0, 1);
                //mipsDec(a_opcode, 0, 0);
                break;
        }break;
/*
{ "bvf",         0x49000000, 0xFFE30000, "%Zc, %O" , ADDR_TYPE_16, INSTR_TYPE_PSP | INSTR_TYPE_B }, // [hlide] %Z -> %Zc
{ "bvfl",        0x49020000, 0xFFE30000, "%Zc, %O" , ADDR_TYPE_16, INSTR_TYPE_PSP | INSTR_TYPE_B }, // [hlide] %Z -> %Zc
{ "bvt",         0x49010000, 0xFFE30000, "%Zc, %O" , ADDR_TYPE_16, INSTR_TYPE_PSP | INSTR_TYPE_B }, // [hlide] %Z -> %Zc
{ "bvtl",        0x49030000, 0xFFE30000, "%Zc, %O" , ADDR_TYPE_16, INSTR_TYPE_PSP | INSTR_TYPE_B }, // [hlide] %Z -> %Zc
*/
     }
     break;

     case 0x50:
      pspDebugScreenPuts("beql     ");
      mipsRegister(a_opcode, S, 1);
      mipsRegister(a_opcode, T, 1);
      //mipsDec(a_opcode, 0, 0);
     break;
      
     case 0x54:
      pspDebugScreenPuts("bnel     ");
      mipsRegister(a_opcode, S, 1);
      mipsRegister(a_opcode, T, 1);
      //mipsDec(a_opcode, 0, 0);
     break;
      
     case 0x58:
      pspDebugScreenPuts("blezl    ");
      mipsRegister(a_opcode, S, 1);
      mipsRegister(a_opcode, T, 1);
      //mipsDec(a_opcode, 0, 0);
     break;
     
     case 0x5C:
      pspDebugScreenPuts("bgtzl    ");
      mipsRegister(a_opcode, S, 1);
      mipsRegister(a_opcode, T, 1);
      //mipsDec(a_opcode, 0, 0);
     break;

     case 0x60:
     switch(a_opcode >>24 & 0xFF){
        case 0x60:
        switch(a_opcode >>16 & 0x80){
                case 0x00:
                pspDebugScreenPuts("vadd.");
                VNUM=3;
                vsel(a_opcode, 0, 3);
                break;
//{ "vadd.p",      0x60000080, 0xFF808080, "%zp, %yp, %xp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vadd.q",      0x60008080, 0xFF808080, "%zq, %yq, %xq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vadd.s",      0x60000000, 0xFF808080, "%zs, %ys, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %yz -> %ys
//{ "vadd.t",      0x60008000, 0xFF808080, "%zt, %yt, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },


                case 0x80:              
                pspDebugScreenPuts("vsub.");
                VNUM=3;
                vsel(a_opcode,0,3);
                break;
        }break;
//{ "vsub.p",      0x60800080, 0xFF808080, "%zp, %yp, %xp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsub.q",      0x60808080, 0xFF808080, "%zq, %yq, %xq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsub.s",      0x60800000, 0xFF808080, "%zs, %ys, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsub.t",      0x60808000, 0xFF808080, "%zt, %yt, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },


      case 0x61:
                pspDebugScreenPuts("vsbn.s   ");
                vectors(a_opcode, 2, 1);
                vectors(a_opcode, 1, 1);
                vectors(a_opcode, 0, 0);
                break;

      case 0x63:
        if(a_opcode >> 16 & 0x80){
                pspDebugScreenPuts("vdiv.");
		VNUM=3;
                vsel(a_opcode, 0 , 3);
                }break;
//{ "vdiv.p",  0x63800080, 0xFF808080, "%zp, %yp, %xp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vdiv.q",  0x63808080, 0xFF808080, "%zq, %yq, %xq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vdiv.s",  0x63800000, 0xFF808080, "%zs, %ys, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %yz -> %ys
//{ "vdiv.t",  0x63808000, 0xFF808080, "%zt, %yt, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },


    }break;

    case 0x64:
        switch(a_opcode >> 24){
                case 0x64:
                if(((a_opcode>>16) &0x80) == 0){
                pspDebugScreenPuts("vmul.");
                }
                else if(((a_opcode>>16) &0x80) == 0x80){
                pspDebugScreenPuts("vdot.");
                }
                VNUM=3;
                vsel(a_opcode,0,3);
                break;
//{ "vmul.p",  0x64000080, 0xFF808080, "%zp, %yp, %xp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmul.q",  0x64008080, 0xFF808080, "%zq, %yq, %xq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmul.s",  0x64000000, 0xFF808080, "%zs, %ys, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmul.t",  0x64008000, 0xFF808080, "%zt, %yt, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vdot.p",  0x64800080, 0xFF808080, "%zs, %yp, %xp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vdot.q",  0x64808080, 0xFF808080, "%zs, %yq, %xq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vdot.t",  0x64808000, 0xFF808080, "%zs, %yt, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

		case 0x65:
	        pspDebugScreenPuts("vscl.");
			vsel(a_opcode, 0 ,3);
			pspDebugScreenPuts(", ");
			vectors(a_opcode,0,0);
             break;
//        { "vscl.p",      0x65000080, 0xFF808080, "%zp, %yp, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %xp -> %xs
//        { "vscl.q",      0x65008080, 0xFF808080, "%zq, %yq, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %xq -> %xs
 //       { "vscl.t",      0x65008000, 0xFF808080, "%zt, %yt, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %xt -> %xs

                case 0x66:
                if (((a_opcode>>16)&0x80) == 0x80){
                pspDebugScreenPuts("vcrs.");}
                else if (((a_opcode>>16)&0x80) == 0){
                pspDebugScreenPuts("vhdp.");}
                VNUM=3;
                vsel(a_opcode, 0 ,3);
                break;
//{ "vcrs.t",  0x66808000, 0xFF808080, "%zt, %yt, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vhdp.p",  0x66000080, 0xFF808080, "%zs, %yp, %xp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %yp, %xp"
//{ "vhdp.q",  0x66008080, 0xFF808080, "%zs, %yq, %xq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %yq, %xq"
//{ "vhdp.t",  0x66008000, 0xFF808080, "%zs, %yt, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %yt, %xt"

                case 0x67:
                pspDebugScreenPuts("vdet.");
                VNUM=3;
                vsel(a_opcode, 0 ,3);
                break;
//{ "vdet.p",  0x67000080, 0xFF808080, "%zs, %yp, %xp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
        }break;

     case 0x68:
     pspDebugScreenPuts("mfvme    ");
     mipsRegister(a_opcode, T, 1);
     mipsImm(a_opcode,0,0);
     break;
     
	case 0x6C:
	switch(a_opcode >>24){
	case 0x6C:
	if((a_opcode  & 0x8080F0) == 0){
		pspDebugScreenPuts("vcmp.s   ");
		VectorCMP(a_opcode,0,1);
	     	vectors(a_opcode, 1, 1);
	     	vectors(a_opcode, 0, 0);
	}
	else if((a_opcode  & 0xFF80F0) == 0){
	pspDebugScreenPuts("vcmp.s   ");
	VectorCMP(a_opcode,0,1);
     	vectors(a_opcode, 1, 0);
	}
	else if((a_opcode  & 0xFFFFF0) == 0){
	pspDebugScreenPuts("vcmp.s   ");
	VectorCMP(a_opcode,0,0);
	}
	else if((a_opcode  & 0x8080F0) == 0x80){
	pspDebugScreenPuts("vcmp.p   ");
	VectorCMP(a_opcode,0,1);
	VFR=4;
     	vectors(a_opcode, 1, 1);
	VFR=4;
     	vectors(a_opcode, 0, 0);
	}
	else if((a_opcode  & 0xFF80F0) == 0x80){
	pspDebugScreenPuts("vcmp.p   ");
	VectorCMP(a_opcode,0,1);
	VFR=4;
     	vectors(a_opcode, 1, 0);
	}
	else if((a_opcode  & 0xFFFFF0) == 0x80){
	pspDebugScreenPuts("vcmp.p   ");
	VectorCMP(a_opcode,0,0);
	}
	else if((a_opcode  & 0x8080F0) == 0x8000){
	pspDebugScreenPuts("vcmp.t   ");
	VectorCMP(a_opcode,0,1);
	VFR=5;
     	vectors(a_opcode, 1, 1);
	VFR=5;
     	vectors(a_opcode, 0, 0);
	}
	else if((a_opcode  & 0xFF80F0) == 0x8000){
	pspDebugScreenPuts("vcmp.t   ");
	VectorCMP(a_opcode,0,1);
	VFR=5;
     	vectors(a_opcode, 1, 0);
	}
	else if((a_opcode  & 0xFFFFF0) == 0x8000){
	pspDebugScreenPuts("vcmp.t   ");
	VectorCMP(a_opcode,0,0);
	}
	else if((a_opcode  & 0x8080F0) == 0x8080){
	pspDebugScreenPuts("vcmp.q   ");
	VectorCMP(a_opcode,0,1);
	VFR=6;
     	vectors(a_opcode, 1, 1);
	VFR=6;
     	vectors(a_opcode, 0, 0);
	}
	else if((a_opcode  & 0xFF80F0) == 0x8080){
	pspDebugScreenPuts("vcmp.q   ");
	VectorCMP(a_opcode,0,1);
	VFR=6;
     	vectors(a_opcode, 1, 0);
	}
	else if((a_opcode  & 0xFFFFF0) == 0x8080){
	pspDebugScreenPuts("vcmp.q   ");
	VectorCMP(a_opcode,0,0);
	}
	break;
	}

		switch((a_opcode >>16) & 0xFF80){
        case 0x6D00:
        pspDebugScreenPuts("vmin.");
        VNUM=3;
        vsel(a_opcode, 0, 3);
        break;
//{ "vmin.p",  0x6D000080, 0xFF808080, "%zp, %yp, %xp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmin.q",  0x6D008080, 0xFF808080, "%zq, %yq, %xq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmin.s",  0x6D000000, 0xFF808080, "%zs, %ys, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmin.t",  0x6D008000, 0xFF808080, "%zt, %yt, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },


        case 0x6D80:
        pspDebugScreenPuts("vmax.");
        VNUM=3;
        vsel(a_opcode,0,3);
        break;
//{ "vmax.p",  0x6D800080, 0xFF808080, "%zp, %yp, %xp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmax.q",  0x6D808080, 0xFF808080, "%zq, %yq, %xq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmax.s",  0x6D800000, 0xFF808080, "%zs, %ys, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmax.t",  0x6D808000, 0xFF808080, "%zt, %yt, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },


        case 0x6E80:
        pspDebugScreenPuts("vscmp.");
                VFR=3;VNUM=3;
                vsel(a_opcode, 0, 2);
        break;

        case 0x6F00:
        pspDebugScreenPuts("vsge.");
                VFR=3;VNUM=3;
                vsel(a_opcode, 0, 3);
        break;

        case 0x6F80:
        pspDebugScreenPuts("vslt.");
                VFR=3;VNUM=3;
                vsel(a_opcode, 0, 3);
        break;
        }
     break;
     
     case 0x70:
        if(a_opcode >> 24 == 0x70){
                switch(a_opcode & 0xE007FF){
                case 0x24:
                pspDebugScreenPuts("mfic     ");
                mipsRegister(a_opcode, T, 1);
                mipsNibble(a_opcode, 2, 0);
                break;

                case 0x26:
                pspDebugScreenPuts("mtic     ");
                mipsRegister(a_opcode, T, 1);
                mipsNibble(a_opcode, 2, 0);
                break;

                case 0x3D:
                pspDebugScreenPuts("mfdr     ");
                mipsRegister(a_opcode, T, 1);
                DrRegister(a_opcode,2,0);
                break;

                case 0x80003D:
                pspDebugScreenPuts("mtdr     ");
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
                pspDebugScreenPuts("halt     ");
                break;

                case 0x3E:
                pspDebugScreenPuts("dret     ");
                break;

                case 0x3F:
                pspDebugScreenPuts("dbreak    ");
                break;
                }
        }
   break;
//"dbreak",            0x7000003F, 0xFFFFFFFF, ""},
//"dret",              0x7000003E, 0xFFFFFFFF, ""},
//"halt",            0x70000000, 0xFFFFFFFF, "" },
     
      //0x1d is empty
      
//      case 0x78:
//      break;

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
     pspDebugScreenPuts("mtvme    ");
     mipsRegister(a_opcode, T, 1);
     mipsImm(a_opcode,0,0);
//        { "mtvme", 0xb0000000, 0xFC000000, "%t, %i", ADDR_TYPE_NONE, 0 },
      break;
      
/*      case 0xB4:
      pspDebugScreenPuts("sdr   ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;*/
      
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
      VFR=3;
      vectors(a_opcode, 0, 1);
      VFR=3;
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
//{ "lv.s",0xC8000000, 0xFC000000, "%Xs, %Y" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },      

      case 0xCC:
      pspDebugScreenPuts("pref  ");
      mipsRegister(a_opcode, T, 1);
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;

      case 0xD0:
        switch(a_opcode >>16){
        case 0xD000:
                pspDebugScreenPuts("vmov.");
                vsel(a_opcode,0,3);
                break;
//{ "vmov.p",  0xD0000080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmov.q",  0xD0008080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmov.s",  0xD0000000, 0xFFFF8080, "%zs, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmov.t",  0xD0008000, 0xFFFF8080, "%zt, %yt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD001:
                pspDebugScreenPuts("vabs.");
                vsel(a_opcode, 0, 3);
        break;
//{ "vabs.p",      0xD0010080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vabs.q",      0xD0018080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vabs.s",      0xD0010000, 0xFFFF8080, "%zs, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vabs.t",      0xD0018000, 0xFFFF8080, "%zt, %yt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD002:
                pspDebugScreenPuts("vneg.");
                vsel(a_opcode,0,3);
        break;
//{ "vneg.p",      0xD0020080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vneg.q",      0xD0028080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vneg.s",      0xD0020000, 0xFFFF8080, "%zs, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vneg.t",      0xD0028000, 0xFFFF8080, "%zt, %yt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD003:
                if((a_opcode & 0xFF80) == 0x80){
                pspDebugScreenPuts("vidt.p   ");
                VFR=6;
                vectors(a_opcode, 2, 0);
                }
                else if((a_opcode & 0xFF80) == 0x8080){
                pspDebugScreenPuts("vidt.q   ");
                VFR=6;
                vectors(a_opcode, 2, 0);
                }
        break;
//{ "vidt.p",  0xD0030080, 0xFFFFFF80, "%zp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vidt.q",  0xD0038080, 0xFFFFFF80, "%zq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD004:
                pspDebugScreenPuts("vsat0.");
                vsel(a_opcode, 0, 2);
        break;
//        { "vsat0.s", 0xD0040000, 0xFFFF8080, "%zs, %ys" },

        case 0xD005:
                pspDebugScreenPuts("vsat1.");
                vsel(a_opcode, 0, 2);
        break;
//        { "vsat1.s", 0xD0050000, 0xFFFF8080, "%zs, %ys" },

        case 0xD006:
                pspDebugScreenPuts("vzero.");
				VNUM=1;
				vsel(a_opcode, 0, 2);
        break;
//{ "vzero.p", 0xD0060080, 0xFFFFFF80, "%zp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vzero.q", 0xD0068080, 0xFFFFFF80, "%zq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vzero.s", 0xD0060000, 0xFFFFFF80, "%zs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vzero.t", 0xD0068000, 0xFFFFFF80, "%zt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD007:
                pspDebugScreenPuts("vone.");
		VNUM=1;
		vsel(a_opcode, 0, 3);
        break;
//        { "vone.s",      0xD0070000, 0xFFFFFF80, "%zs" },

        case 0xD010:
                pspDebugScreenPuts("vrcp.");
		vsel(a_opcode, 0, 3);
        break;
//        { "vrcp.s",      0xD0100000, 0xFFFF8080, "%zs, %ys" },

        case 0xD011:
                pspDebugScreenPuts("vrsq.");
		vsel(a_opcode, 0, 3);
        break;
//        { "vrsq.s",      0xD0110000, 0xFFFF8080, "%zs, %ys" },


        case 0xD012:
                pspDebugScreenPuts("vsin.");
		vsel(a_opcode, 0, 3);
        break;
//         { "vsin.s",      0xD0120000, 0xFFFF8080, "%zs, %ys" },

        case 0xD013:
                pspDebugScreenPuts("vcos.");
                vsel(a_opcode, 0 ,3);
        break;
//{ "vcos.p",      0xD0130080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vcos.q",      0xD0138080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vcos.s",      0xD0130000, 0xFFFF8080, "%zs, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vcos.t",      0xD0138000, 0xFFFF8080, "%zt, %yt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },


        case 0xD014:
                pspDebugScreenPuts("vexp2.");
                vsel(a_opcode,0,2);
        break;
//{ "vexp2.p", 0xD0140080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vexp2.q", 0xD0148080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vexp2.s", 0xD0140000, 0xFFFF8080, "%zs, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vexp2.t", 0xD0148000, 0xFFFF8080, "%zt, %yt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },


        case 0xD015:
                pspDebugScreenPuts("vlog2.");
                vsel(a_opcode,0,2);
        break;
//{ "vlog2.p", 0xD0150080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vlog2.q", 0xD0158080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vlog2.s", 0xD0150000, 0xFFFF8080, "%zs, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vlog2.t", 0xD0158000, 0xFFFF8080, "%zt, %yt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD016:
                pspDebugScreenPuts("vsqrt.");
                vsel(a_opcode,0,2);
        break;
//{ "vsqrt.p", 0xD0160080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsqrt.q", 0xD0168080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsqrt.s", 0xD0160000, 0xFFFF8080, "%zs, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsqrt.t", 0xD0168000, 0xFFFF8080, "%zt, %yt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },


        case 0xD017:
                pspDebugScreenPuts("vasin.");
                vsel(a_opcode, 0, 2);
        break;
//{ "vasin.p", 0xD0170080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vasin.q", 0xD0178080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vasin.s", 0xD0170000, 0xFFFF8080, "%zs, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vasin.t", 0xD0178000, 0xFFFF8080, "%zt, %yt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD018:
                pspDebugScreenPuts("vnrcp.");
		vsel(a_opcode, 0, 2);
        break;
//        { "vnrcp.s", 0xD0180000, 0xFFFF8080, "%zs, %ys" },

        case 0xD01A:
                pspDebugScreenPuts("vnsin.");
		vsel(a_opcode, 0, 2);
        break;
//        { "vnsin.s", 0xD01A0000, 0xFFFF8080, "%zs, %ys" },

        case 0xD01C:
                pspDebugScreenPuts("vrexp2.");
		vsel(a_opcode, 0, 1);
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
                pspDebugScreenPuts("vrndi.");
		VFR=1;VNUM=1;
		vsel(a_opcode, 0, 2);
        break;
//        { "vrndi.s", 0xD0210000, 0xFFFFFF80, "%zs" },

        case 0xD022:
                pspDebugScreenPuts("vrndf1.");
		VFR=1;VNUM=1;
		vsel(a_opcode, 0, 1);
        break;
//        { "vrndf1.s", 0xD0220000, 0xFFFFFF80, "%zs" },

        case 0xD023:
                pspDebugScreenPuts("vrndf2.");
		VFR=1;VNUM=1;
		vsel(a_opcode, 0, 1);
        break;
//        { "vrndf2.s", 0xD0230000, 0xFFFFFF80, "%zs" },

        case 0xD032:
                if((a_opcode & 0xFF80) == 0x80){
                pspDebugScreenPuts("vf2h.p   ");
                vectors(a_opcode, 2, 1);
                VFR=4;
                vectors(a_opcode, 1, 0);
                }
                else if((a_opcode & 0xFF80) == 0x8080){
                pspDebugScreenPuts("vf2h.q   ");
                VFR=4;
                vectors(a_opcode, 2, 1);
                VFR=6;
                vectors(a_opcode, 1, 0);
                }
        break;
//{ "vf2h.p",  0xD0320080, 0xFFFF8080, "%zs, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zp -> %zs
//{ "vf2h.q",  0xD0328080, 0xFFFF8080, "%zp, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zq -> %zp

        case 0xD033:
                if((a_opcode & 0x8080) == 0){
                pspDebugScreenPuts("vh2f.s   ");
                VFR=4;
                vectors(a_opcode, 2, 1);
                vectors(a_opcode, 1, 0);
                }
                else if((a_opcode & 0x8080) == 0x80){
                pspDebugScreenPuts("vh2f.p   ");
                VFR=6;
                vectors(a_opcode, 2, 1);
                VFR=4;
                vectors(a_opcode, 1, 0);}
        break;
//{ "vh2f.p",  0xD0330080, 0xFFFF8080, "%zq, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zp -> %zq
//{ "vh2f.s",  0xD0330000, 0xFFFF8080, "%zp, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zs -> %zp

        case 0xD036:
                pspDebugScreenPuts("vsbz.s   ");
                vectors(a_opcode, 2, 1);
                vectors(a_opcode, 1, 0);
        break;
//        { "vsbz.s",      0xD0360000, 0xFFFF8080, "%zs, %ys" },

        case 0xD037:
                pspDebugScreenPuts("vlgb.s   ");
                vectors(a_opcode, 2, 1);
                vectors(a_opcode, 1, 0);
        break;
//{ "vlgb.s",  0xD0370000, 0xFFFF8080, "%zs, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD038:
                pspDebugScreenPuts("v???.s   ");
                vectors(a_opcode, 2, 1);
                vectors(a_opcode, 1, 0);
                pspDebugScreenPuts(unknown[2]);
        break;
        case 0xD039:
                pspDebugScreenPuts("v???.s   ");
                vectors(a_opcode, 2, 1);
                vectors(a_opcode, 1, 0);
                pspDebugScreenPuts(unknown[2]);
        break;
//unknown vector command

        case 0xD03A:
		if((a_opcode & 0x8080) == 0){
		pspDebugScreenPuts("vus2i.s  ");
		VFR=4;
    	 	vectors(a_opcode, 2, 1);
		vectors(a_opcode, 1, 0);
		}
		else if((a_opcode & 0x8080) == 0x80){
	        pspDebugScreenPuts("vus2i.p  ");
		VFR=6;
     	 	vectors(a_opcode, 2, 1);
		VFR=4;
     	 	vectors(a_opcode, 1, 0);
		}
        break;
//        { "vus2i.s", 0xD03A0000, 0xFFFF8080, "" },

        case 0xD03B:
			if((a_opcode & 0x8080) == 0){
		        pspDebugScreenPuts("vs2i.s   ");
			VFR=4;
	     	 	vectors(a_opcode, 2, 1);
	     	 	vectors(a_opcode, 1, 0);
			}
			else if((a_opcode & 0x8080) == 0x80){
		        pspDebugScreenPuts("vs2i.p   ");
			VFR=6;
	     	 	vectors(a_opcode, 2, 1);
			VFR=4;
	     	 	vectors(a_opcode, 1, 0);
			}
        break;       
 // { "vs2i.p",      0xD03B0080, 0xFFFF8080, "%zq, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zp -> %zq
 //{ "vs2i.s",      0xD03B0000, 0xFFFF8080, "%zp, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zs -> %zp
  

        case 0xD03C:
                pspDebugScreenPuts("vi2uc.q  ");
                VFR=6;
                vectors(a_opcode, 2, 1);
                VFR=6;
                vectors(a_opcode, 1, 0);
        break;
//{ "vi2uc.q", 0xD03C8080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zp -> %zq

        case 0xD03D:
                pspDebugScreenPuts("vs2c.q   ");
                vectors(a_opcode, 2, 1);
                VFR=6;
                vectors(a_opcode, 1, 0);
        break;
//{ "vi2c.q",  0xD03D8080, 0xFFFF8080, "%zs, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %yq"

        case 0xD03E:
                if((a_opcode & 0x8080) == 0x80){
                pspDebugScreenPuts("vi2us.p  ");
                VFR=6;
                vectors(a_opcode, 2, 1);
                VFR=6;
                vectors(a_opcode, 1, 0);
                }
                else if((a_opcode & 0x8080) == 0x8080){
                pspDebugScreenPuts("vi2us.q  ");
                VFR=6;
                vectors(a_opcode, 2, 1);
                VFR=6;
                vectors(a_opcode, 1, 0);
                }
        break;//?????
//{ "vi2us.p", 0xD03E0080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zp -> %zq
//{ "vi2us.q", 0xD03E8080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zp -> %zq

        case 0xD03F:
                if((a_opcode & 0x8080) == 0x80){
                pspDebugScreenPuts("vi2s.p   ");
                vectors(a_opcode, 2, 1);
                VFR=4;
                vectors(a_opcode, 1, 0);
                }
                else if((a_opcode & 0x8080) == 0x8080){
                pspDebugScreenPuts("vi2s.q   ");
                VFR=4;
                vectors(a_opcode, 2, 1);
                VFR=6;
                vectors(a_opcode, 1, 0);
                }
        break;
//{ "vi2s.p",  0xD03F0080, 0xFFFF8080, "%zs, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %yp"
//{ "vi2s.q",  0xD03F8080, 0xFFFF8080, "%zp, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zp, %yq"

        case 0xD040:
                pspDebugScreenPuts("vsrt1.q  ");
                VFR=6;
                vectors(a_opcode, 2, 1);
                VFR=6;
                vectors(a_opcode, 1, 0);
        break;

        case 0xD041:
                pspDebugScreenPuts("vsrt2.q  ");
                VFR=6;
                vectors(a_opcode, 2, 1);
                VFR=6;
                vectors(a_opcode, 1, 0);
        break;
//{ "vsrt1.q", 0xD0408080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsrt2.q", 0xD0418080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD042:
                pspDebugScreenPuts("vbfy1.");
                vsel(a_opcode, 0, 2);
        break;
//{ "vbfy1.p", 0xD0420080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vbfy1.q", 0xD0428080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD043:
                pspDebugScreenPuts("vbfy2.");
                vsel(a_opcode, 0, 2);
        break;
//{ "vbfy2.q", 0xD0438080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vbfy2.q", 0xD0438080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD044:
                pspDebugScreenPuts("vocp.");
		vsel(a_opcode, 0, 3);
        break;
//        { "vocp.s",      0xD0440000, 0xFFFF8080, "%zs, %ys" },

        case 0xD045:
	if((a_opcode & 0x8080) == 0){
		        pspDebugScreenPuts("vsocp.s  ");
			VFR=4;
	     	 	vectors(a_opcode, 2, 1);
	     	 	vectors(a_opcode, 1, 0);
			}
			else if((a_opcode & 0x8080) == 0x80){
		        pspDebugScreenPuts("vsocp.s  ");
			VFR=6;
	     	 	vectors(a_opcode, 2, 1);
			VFR=4;
	     	 	vectors(a_opcode, 1, 0);
			}
        break;
//        { "vsocp.s", 0xD0450000, 0xFFFF8080, "%zs, %ys" },


        case 0xD046:
                pspDebugScreenPuts("vfad.");
                vsel(a_opcode, 0, 3);
        break;
//{ "vfad.p",  0xD0460080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vfad.q",  0xD0468080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vfad.t",  0xD0468000, 0xFFFF8080, "%zt, %yt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
        case 0xD047:
                pspDebugScreenPuts("vavg.");
                vsel(a_opcode, 0, 3);
        break;
//{ "vavg.p",  0xD0470080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vavg.q",  0xD0478080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vavg.t",  0xD0478000, 0xFFFF8080, "%zt, %yt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
	

	case 0xD048:
	pspDebugScreenPuts("vsrt3.q  ");
	VFR=6;
	vectors(a_opcode, 2, 1);
	VFR=6;
	vectors(a_opcode, 1, 0);
	break;

	case 0xD049:
	        pspDebugScreenPuts("vsrt4.q  ");
		VFR=6;
     	 	vectors(a_opcode, 2, 1);
		VFR=6;
     	 	vectors(a_opcode, 1, 0);
	break;
//{ "vsrt3.q", 0xD0488080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsrt4.q", 0xD0498080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

	case 0xD04A:
	        pspDebugScreenPuts("vsgn.");
     	 	vsel(a_opcode, 0, 3);
	break;
//{ "vsgn.p",      0xD04A0080, 0xFFFF8080, "%zp, %yp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsgn.q",      0xD04A8080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsgn.s",      0xD04A0000, 0xFFFF8080, "%zs, %ys" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsgn.t",      0xD04A8000, 0xFFFF8080, "%zt, %yt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        case 0xD050:
                pspDebugScreenPuts("vmfvc    ");
                vectors(a_opcode, 1, 1);
                a_opcode&=0xFF;
                mipsImm(a_opcode, 0, 0);
        break;
//{ "vmfvc",   0xD0500000, 0xFFFF0080, "%zs, %2s" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %2s"

        case 0xD051:
                pspDebugScreenPuts("vmtvc    ");
				b_opcode=a_opcode;
                a_opcode&=0xFF;
                mipsImm(a_opcode, 0, 1);
				a_opcode=b_opcode;
                vectors(a_opcode, 1, 0);
        break;

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
//{ "vt4444.q",0xD0598080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zq -> %zp
//{ "vt5551.q",0xD05A8080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zq -> %zp
//{ "vt5650.q",0xD05B8080, 0xFFFF8080, "%zq, %yq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zq -> %zp

        }

        switch(a_opcode >>16 & 0xFFE0){
        case 0xD060:
                if((a_opcode & 0xFF80) == 0){
                pspDebugScreenPuts("vcst.s   ");
                vectors(a_opcode, 2, 1);
                }
                else if((a_opcode & 0xFF80) == 0x80){
                pspDebugScreenPuts("vcst.p   ");
                VFR=4;
                vectors(a_opcode, 2, 1);
                }
                else if((a_opcode & 0xFF80) == 0x8000){
                pspDebugScreenPuts("vcst.t   ");
                VFR=5;
                vectors(a_opcode, 2, 1);
                }
                else if((a_opcode & 0xFF80) == 0x8080){
                pspDebugScreenPuts("vcst.q   ");
                VFR=6;
                vectors(a_opcode, 2, 1);
                }
                VFMODE=3;
                vectors(a_opcode, 0, 0);
        break;
//{ "vcst.p",  0xD0600080, 0xFFE0FF80, "%zp, %vk" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] "%zp, %yp, %xp" -> "%zp, %vk"
//{ "vcst.q",  0xD0608080, 0xFFE0FF80, "%zq, %vk" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] "%zq, %yq, %xq" -> "%zq, %vk"
//{ "vcst.s",  0xD0600000, 0xFFE0FF80, "%zs, %vk" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] "%zs, %ys, %xs" -> "%zs, %vk"
//{ "vcst.t",  0xD0608000, 0xFFE0FF80, "%zt, %vk" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] "%zt, %yt, %xt" -> "%zt, %vk"

        case 0xD260:
        pspDebugScreenPuts("vf2id.");
        vsel(a_opcode,0,2);
        pspDebugScreenPuts(", ");
        a_opcode=(a_opcode>>16)&0x1F;
        mipsImm(a_opcode,0,0);
        break;
//{ "vf2id.p", 0xD2600080, 0xFFE08080, "%zp, %yp, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zp, %yp, %v5"
//{ "vf2id.q", 0xD2608080, 0xFFE08080, "%zq, %yq, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zq, %yq, %v5"
//{ "vf2id.s", 0xD2600000, 0xFFE08080, "%zs, %ys, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %ys, %v5"
//{ "vf2id.t", 0xD2608000, 0xFFE08080, "%zt, %yt, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zt, %yt, %v5"

        case 0xD200:
        pspDebugScreenPuts("vf2idn");
        vsel(a_opcode,0,2);
        pspDebugScreenPuts(", ");
        a_opcode=(a_opcode>>16)&0x1F;
        mipsImm(a_opcode,0,0);
        break;
//{ "vf2in.p", 0xD2000080, 0xFFE08080, "%zp, %yp, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zp, %yp, %v5"
//{ "vf2in.q", 0xD2008080, 0xFFE08080, "%zq, %yq, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zq, %yq, %v5"
//{ "vf2in.s", 0xD2000000, 0xFFE08080, "%zs, %ys, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %ys, %v5"
//{ "vf2in.t", 0xD2008000, 0xFFE08080, "%zt, %yt, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zt, %yt, %v5"

        case 0xD240:
        pspDebugScreenPuts("vf2iu.");
        vsel(a_opcode,0,2);
        pspDebugScreenPuts(", ");
        a_opcode=(a_opcode>>16)&0x1F;
        mipsImm(a_opcode,0,0);
        break;
//{ "vf2iu.p", 0xD2400080, 0xFFE08080, "%zp, %yp, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zp, %yp, %v5"
//{ "vf2iu.q", 0xD2408080, 0xFFE08080, "%zq, %yq, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zq, %yq, %v5"
//{ "vf2iu.s", 0xD2400000, 0xFFE08080, "%zs, %ys, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %ys, %v5"
//{ "vf2iu.t", 0xD2408000, 0xFFE08080, "%zt, %yt, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zt, %yt, %v5"

        case 0xD220:
        pspDebugScreenPuts("vf2iz.");
        vsel(a_opcode,0,2);
        pspDebugScreenPuts(", ");
        a_opcode=(a_opcode>>16)&0x1F;
        mipsImm(a_opcode,0,0);
        break;
//{ "vf2iz.p", 0xD2200080, 0xFFE08080, "%zp, %yp, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zp, %yp, %v5"
//{ "vf2iz.q", 0xD2208080, 0xFFE08080, "%zq, %yq, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zq, %yq, %v5"
//{ "vf2iz.s", 0xD2200000, 0xFFE08080, "%zs, %ys, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %ys, %v5"
//{ "vf2iz.t", 0xD2208000, 0xFFE08080, "%zt, %yt, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zt, %yt, %v5"

        case 0xD280:
        pspDebugScreenPuts("vi2f.");
        vsel(a_opcode,0,3);
        pspDebugScreenPuts(", ");
        a_opcode=(a_opcode>>16)&0x1F;
        mipsImm(a_opcode,0,0);
        break;}
//{ "vi2f.p",  0xD2800080, 0xFFE08080, "%zp, %yp, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zp, %yp, %v5"
//{ "vi2f.q",  0xD2808080, 0xFFE08080, "%zq, %yq, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zq, %yq, %v5"
//{ "vi2f.s",  0xD2800000, 0xFFE08080, "%zs, %ys, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %ys, %v5"
//{ "vi2f.t",  0xD2808000, 0xFFE08080, "%zt, %yt, %v5" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zt, %yt, %v5"


        switch((a_opcode>>16)&0xFFF8){
        case 0xD2A0:
        pspDebugScreenPuts("vcmovt.");
        vsel(a_opcode,0,1);
        pspDebugScreenPuts(", ");
        a_opcode=(a_opcode>>16) & 0x7;
        mipsImm(a_opcode, 0 , 0);
        break;

        case 0xD2A8:
        pspDebugScreenPuts("vcmovf.");
        vsel(a_opcode,0,1);
        pspDebugScreenPuts(", ");
        a_opcode=(a_opcode>>16) & 0x7;
        mipsImm(a_opcode, 0 ,0);
        break;
        }
//{ "vcmovf.p",0xD2A80080, 0xFFF88080, "%zp, %yp, %v3" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zp, %yp, %v3"
//{ "vcmovf.q",0xD2A88080, 0xFFF88080, "%zq, %yq, %v3" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zq, %yq, %v3"
//{ "vcmovf.s",0xD2A80000, 0xFFF88080, "%zs, %ys, %v3" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %ys, %v3"
//{ "vcmovf.t",0xD2A88000, 0xFFF88080, "%zt, %yt, %v3" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zt, %yt, %v3"
//{ "vcmovt.p",0xD2A00080, 0xFFF88080, "%zp, %yp, %v3" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zp, %yp, %v3"
//{ "vcmovt.q",0xD2A08080, 0xFFF88080, "%zq, %yq, %v3" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zq, %yq, %v3"
//{ "vcmovt.s",0xD2A00000, 0xFFF88080, "%zs, %ys, %v3" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zs, %ys, %v3"
//{ "vcmovt.t",0xD2A08000, 0xFFF88080, "%zt, %yt, %v3" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zt, %yt, %v3"
        
        switch(a_opcode>>24){
        case 0xD3:
        pspDebugScreenPuts("vwbn.s   ");
                vectors(a_opcode, 2, 1);
                vectors(a_opcode, 1, 1);
        mipsImm(a_opcode,0,0);
//        { "vwbn.s",      0xD3000000, 0xFF008080, "" },
        break;
        }break;
      
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
//{ "lvl.q",       0xD4000000, 0xFC000002, "%Xq, %Y" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "lvr.q",       0xD4000002, 0xFC000002, "%Xq, %Y" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

      case 0xD8:
        if((a_opcode & 0x2) ==0){
        pspDebugScreenPuts("lv.q     ");
        VFR=1;
        vectors(a_opcode,0,1);
        VFR=3;
        mipsImm(a_opcode,0,0);
        pspDebugScreenPuts("(");
        mipsRegister(a_opcode, S, 0);
        pspDebugScreenPuts(")");
        }
      break;
//{ "lv.q",0xD8000000, 0xFC000002, "%Xq, %Y" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
      
      case 0xDC:
        switch(a_opcode >>24){
        case 0xDE:
        pspDebugScreenPuts("vpfxd    [");
        vprefix(a_opcode,0x34,1);
        vprefix(a_opcode,0x35,1);
        vprefix(a_opcode,0x36,1);
        vprefix(a_opcode,0x37,0);
        pspDebugScreenPuts("]");
        break;

        case 0xDC:
        pspDebugScreenPuts("vpfxs    [");
        vprefix(a_opcode,0x30,1);
        vprefix(a_opcode,0x31,1);
        vprefix(a_opcode,0x32,1);
        vprefix(a_opcode,0x33,0);
        pspDebugScreenPuts("]");
        break;

        case 0xDD:
        pspDebugScreenPuts("vpfxt    [");
        vprefix(a_opcode,0x30,1);
        vprefix(a_opcode,0x31,1);
        vprefix(a_opcode,0x32,1);
        vprefix(a_opcode,0x33,0);
        pspDebugScreenPuts("]");
        break;
//{ "vpfxd",       0xDE000000, 0xFF000000, "[%vp4, %vp5, %vp6, %vp7]" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "[%vp4, %vp5, %vp6, %vp7]"
//{ "vpfxs",       0xDC000000, 0xFF000000, "[%vp0, %vp1, %vp2, %vp3]" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "[%vp0, %vp1, %vp2, %vp3]"
//{ "vpfxt",       0xDD000000, 0xFF000000, "[%vp0, %vp1, %vp2, %vp3]" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "[%vp0, %vp1, %vp2, %vp3]"

        
        case 0xDF:
        switch(a_opcode >>16 & 0x80){
        case 0x00:
        pspDebugScreenPuts("viim.s   ");
        vectors(a_opcode,0,1);
        mipsImm(a_opcode,0xA,0);
        break;

        case 0x80:
        pspDebugScreenPuts("vfim.s   ");
        vectors(a_opcode,0,1);
        //mipsImm(a_opcode,0,0);
        break;
//{ "vfim.s",  0xDF800000, 0xFF800000, "%xs, %vh" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%xs, %vh"

        }
        break;
        }
      break;
      
      case 0xE0:
      pspDebugScreenPuts("sc    ");
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
      VFR=3;
      vectors(a_opcode, 0, 1);
      VFR=3;
      mipsImm(a_opcode, 0, 0);
      pspDebugScreenPuts("(");
      mipsRegister(a_opcode, S, 0);
      pspDebugScreenPuts(")");
      break;
//{ "sv.s",0xE8000000, 0xFC000000, "%Xs, %Y" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

      //case 0xEC:
      //break;

        case 0xF0:
        switch(a_opcode>>16 & 0xFF80){
	case 0xF080:
	if((a_opcode & 0x8080) == 0){
        pspDebugScreenPuts("vhtfm2.p ");}
	else if((a_opcode & 0x8080) == 0x80){
        pspDebugScreenPuts("vtfm2.p  ");}
	VFR=4;vectors(a_opcode,2,1);
	VFR=4;VMT=1;vectors(a_opcode,1,1);
	VFR=4;vectors(a_opcode,0,0);
        break;
//{ "vhtfm2.p",0xF0800000, 0xFF808080, "%zp, %ym, %xp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zp, %ym, %xp"
//{ "vtfm2.p", 0xF0800080, 0xFF808080, "%zp, %ym, %xp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zp, %ym, %xp"

        case 0xF100:
	if((a_opcode & 0x8080) == 0x80){
        pspDebugScreenPuts("vhtfm3.t ");}
	else if((a_opcode & 0x8080) == 0x8000){
        pspDebugScreenPuts("vtfm3.t  ");}
	VFR=5;vectors(a_opcode,2,1);
	VFR=5;VMT=1;vectors(a_opcode,1,1);
	VFR=5;vectors(a_opcode,0,0);
        break;
//{ "vtfm3.t", 0xF1008000, 0xFF808080, "%zt, %yn, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zt, %yn, %xt"
//{ "vhtfm3.t",0xF1000080, 0xFF808080, "%zt, %yn, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zt, %yn, %xt"

        case 0xF180:
	if((a_opcode & 0x8080) == 0x8000){
        pspDebugScreenPuts("vhtfm4.q ");}
	else if((a_opcode & 0x8080) == 0x8080){
        pspDebugScreenPuts("vtfm4.q  ");}
	VFR=6;vectors(a_opcode,2,1);
	VFR=6;VMT=1;vectors(a_opcode,1,1);
	VFR=6;vectors(a_opcode,0,0);
        break;
//{ "vtfm4.q", 0xF1808080, 0xFF808080, "%zq, %yo, %xq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zq, %yo, %xq"
//{ "vhtfm4.q",0xF1808000, 0xFF808080, "%zq, %yo, %xq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zq, %yo, %xq"

	case 0xF000:
        pspDebugScreenPuts("vmmul.");
	VNUM=3;VMT=1;
	vsel(a_opcode,0,2);
        break;
//{ "vmmul.p", 0xF0000080, 0xFF808080, "%zm, %ym, %xm" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%?%zm, %ym, %xm"
//{ "vmmul.q", 0xF0008080, 0xFF808080, "%zo, %yo, %xo" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmmul.t", 0xF0008000, 0xFF808080, "%zn, %yn, %xn" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%?%zn, %yn, %xn"

	case 0xF200:
        pspDebugScreenPuts("vmscl.");
	VMT=1;
	vsel(a_opcode,0,2);
        pspDebugScreenPuts(", ");
	vectors(a_opcode,0,0);
        break;
//{ "vmscl.p", 0xF2000080, 0xFF808080, "%zm, %ym, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zp, %yp, %xp -> %zm, %ym, %xs
//{ "vmscl.q", 0xF2008080, 0xFF808080, "%zo, %yo, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zq, %yq, %xp -> %zo, %yo, %xs
//{ "vmscl.t", 0xF2008000, 0xFF808080, "%zn, %yn, %xs" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zt, %yt, %xp -> %zn, %yn, %xs

        case 0xF280:
        if ((a_opcode &0x8080) == 0x8000){
        pspDebugScreenPuts("vcrsp.");
        VFR=5;
        }
        else if ((a_opcode &0x8080) == 0x8080){
        pspDebugScreenPuts("vqmul.");
        VFR=6;
        }
        VNUM=3;
        vsel(a_opcode, 0, 2);
        break;
//{ "vcrsp.t", 0xF2808000, 0xFF808080, "%zt, %yt, %xt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vqmul.q", 0xF2808080, 0xFF808080, "%zq, %yq, %xq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zq, %yq, %xq"
        }

        switch(a_opcode >> 16){
        case 0xF387:
        pspDebugScreenPuts("vmone.");
        VNUM=1;
        vsel(a_opcode, 0, 2);
        break;
//{ "vmone.p", 0xF3870080, 0xFFFFFF80, "%zp" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmone.q", 0xF3878080, 0xFFFFFF80, "%zq" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmone.t", 0xF3878000, 0xFFFFFF80, "%zt" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

	case 0xF383:
        pspDebugScreenPuts("vmidt.");
        VNUM=1;VMT=1;
        vsel(a_opcode, 0, 2);
        break;
//{ "vmidt.p", 0xF3830080, 0xFFFFFF80, "%zm" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zp -> %zm
//{ "vmidt.q", 0xF3838080, 0xFFFFFF80, "%zo" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zq -> %zo
//{ "vmidt.t", 0xF3838000, 0xFFFFFF80, "%zn" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zt -> %zn

	case 0xF380:
        pspDebugScreenPuts("vmmov.");
        VMT=1;
        vsel(a_opcode, 0, 2);
        break;
//{ "vmmov.p", 0xF3800080, 0xFFFF8080, "%zm, %ym" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zm, %ym"
//{ "vmmov.q", 0xF3808080, 0xFFFF8080, "%zo, %yo" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vmmov.t", 0xF3808000, 0xFFFF8080, "%zn, %yn" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zn, %yn"

	case 0xF386:
        pspDebugScreenPuts("vmzero.");
        VNUM=1;VMT=1;
        vsel(a_opcode, 0, 1);
        break;
	}
//{ "vmzero.p", 0xF3860080, 0xFFFFFF80, "%zm" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zp -> %zm
//{ "vmzero.q", 0xF3868080, 0xFFFFFF80, "%zo" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zq -> %zo
//{ "vmzero.t", 0xF3868000, 0xFFFFFF80, "%zn" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] %zt -> %zn
	
	switch((a_opcode>>16)& 0xFFE0){
	case 0xF3A0:
        pspDebugScreenPuts("vrot.");
		vsel(a_opcode,0,3);
		pspDebugScreenPuts(", ");
		vrot(a_opcode,0,0);
	break;
//{ "vrot.p",      0xF3A00080, 0xFFE08080, "%zp, %ys, %vr" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zp, %ys, %vr"
//{ "vrot.q",      0xF3A08080, 0xFFE08080, "%zq, %ys, %vr" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zq, %ys, %vr"
//{ "vrot.t",      0xF3A08000, 0xFFE08080, "%zt, %ys, %vr" , ADDR_TYPE_NONE, INSTR_TYPE_PSP }, // [hlide] added "%zt, %ys, %vr"
	}
        break;

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
//{ "svl.q",       0xF4000000, 0xFC000002, "%Xq, %Y" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "svr.q",       0xF4000002, 0xFC000002, "%Xq, %Y" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
      

     case 0xF8:
        switch(a_opcode & 0x2){
        case 0x00:
        pspDebugScreenPuts("sv.q     ");
        VFR=1;
        vectors(a_opcode,0,1);
        VFR=3;
        mipsImm(a_opcode,0,0);
        pspDebugScreenPuts("(");
        mipsRegister(a_opcode, S, 0);
        pspDebugScreenPuts(")");
        break;
//{ "sv.q",0xF8000000, 0xFC000002, "%Xq, %Y" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

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
        pspDebugScreenPuts("vnop     ");
        break;

        case 0xFFFF040D:
        pspDebugScreenPuts("vflush");
        break;
//{ "vflush",  0xFFFF040D, 0xFFFFFFFF, "" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vnop",0xFFFF0000, 0xFFFFFFFF, "" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },

        default:
        pspDebugScreenPuts("vsync    ");
        mipsImm(a_opcode,0,0);
//{ "vsync",       0xFFFF0000, 0xFFFF0000, "%I" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
//{ "vsync",       0xFFFF0320, 0xFFFFFFFF, "" , ADDR_TYPE_NONE, INSTR_TYPE_PSP },
        }
      break;
 }

  //pspDebugScreenSetTextColor(color02);
}


void mipsSpecial(unsigned int addresscode,unsigned int addresstmp,unsigned int counteraddress){
							if(((addresscode >= 0x10000000) && (addresscode <= 0x1FFFFFFF)) || ((addresscode >= 0x50000000) && (addresscode <= 0x5FFFFFFF))
							|| ((addresscode >= 0x45000000) && (addresscode <= 0x4503FFFF)) || ((addresscode >= 0x49000000) && (addresscode <= 0x491FFFFF))
							|| (((addresscode & 0xFC1F0000) >= 0x04000000) && ((addresscode & 0xFC1F0000) <= 0x04030000))
							||  (((addresscode & 0xFC1F0000) >= 0x04100000) && ((addresscode & 0xFC1F0000) <= 0x04130000)) )
							{
							addresstmp=addresscode & 0xFFFF;
							addresscode=4*(addresstmp + 1);
							if(addresstmp > 0x7FFF){
							addresscode=(-0x40000+addresscode)+counteraddress;}
							else{
							addresscode=addresscode+counteraddress;}
							sprintf(buffer, "$%X", addresscode);pspDebugScreenPuts(buffer);
							if(extMenu==2){
							pspDebugScreenPuts("+0FFSET");
							}
							if(addresstmp > 0x7FFF){
							 sprintf(buffer, "(-%d)", (0x10000-(addresstmp+1)));}
							else{
							 sprintf(buffer, "(+%d)", (addresstmp+1));}
							 pspDebugScreenPuts(buffer);
							}
							else if( (addresscode>>24 == 0x3C) && (((addresscode & 0x7FFF) > 0x38D1) && ((addresscode & 0x7FFF) < 0x4B19) ||((addresscode & 0x7FFF) > 0x7F7F))){
							addresscode=addresscode <<16;
							pspDebugScreenPuts("  UFLOAT:");//UPPER IEEE754 FLOAT
							f_cvt(&addresscode, buffer, sizeof(buffer), 6, MODE_GENERIC);
							pspDebugScreenPuts(buffer);
							}
							else if( (((addresscode>>16)&0xFF80) == 0xDF80)&& ((addresscode & 0x7FFF) > 0x68E) ){
							pspDebugScreenPuts(" VFLOAT:");//16BIT VFPU HALF FLOAT
/* VFPU 16-bit floating-point format. */
#define VFPU_FLOAT16_EXP_MAX    0x1f
#define VFPU_SH_FLOAT16_SIGN    15
#define VFPU_MASK_FLOAT16_SIGN  0x1
#define VFPU_SH_FLOAT16_EXP     10
#define VFPU_MASK_FLOAT16_EXP   0x1f
#define VFPU_SH_FLOAT16_FRAC    0
#define VFPU_MASK_FLOAT16_FRAC  0x3ff
        /* Convert a VFPU 16-bit floating-point number to IEEE754. */
        unsigned int float2int=0;
        unsigned short float16 = addresscode & 0xFFFF;
        unsigned int sign = (float16 >> VFPU_SH_FLOAT16_SIGN) & VFPU_MASK_FLOAT16_SIGN;
        int exponent = (float16 >> VFPU_SH_FLOAT16_EXP) & VFPU_MASK_FLOAT16_EXP;
        unsigned int fraction = float16 & VFPU_MASK_FLOAT16_FRAC;
        char signchar = '+' + ((sign == 1) * 2);

        if (exponent == VFPU_FLOAT16_EXP_MAX)
        {
                if (fraction == 0){
                        sprintf(buffer, "%cInf", signchar);
		pspDebugScreenPuts(buffer);
		}
                else{
                        sprintf(buffer, "%cNaN", signchar);
		pspDebugScreenPuts(buffer);
		}
        }
        else if (exponent == 0 && fraction == 0)
        {
                sprintf(buffer, "%c0", signchar);
		pspDebugScreenPuts(buffer);
        }
        else
        {
                if (exponent == 0)
                {
                        do

                        {
                                fraction <<= 1;
                                exponent--;
                        }
                        while (!(fraction & (VFPU_MASK_FLOAT16_FRAC + 1)));

                        fraction &= VFPU_MASK_FLOAT16_FRAC;
                }
                /* Convert to 32-bit single-precision IEEE754. */
                float2int = (sign << 31) + ((exponent + 112) << 23) + (fraction << 13);
		//sprintf(buffer, "%X" ,float2int); pspDebugScreenPuts(buffer);
		f_cvt(&float2int, buffer, sizeof(buffer), 6, MODE_GENERIC);
		pspDebugScreenPuts(buffer);
        }
							}

}
#ifdef _PSID_
#include <pspiofilemgr.h>
//thanks to nofx
char fx_ScramByte(int key, int input){
	char counter;
	int dummy_1;
	for(counter=0; counter<3; counter++){
		int dummy_0 = input*(10*counter);
		dummy_1 = dummy_0^dummy_1;
		dummy_0 |= dummy_0^dummy_1/(key^input);
		if(dummy_0 > 0x7f) dummy_1 = (dummy_0*key);
		else dummy_1 = (dummy_0^key);
		//dummy_1 = dummy_0^(dummy_1/10);
	}
	int dummy_2 = dummy_1;
	while(dummy_2 > 0xFF){
		dummy_2 = dummy_2/dummy_2;
	}
	char dummy_3 = dummy_2&0xFF;
	if(dummy_3 == 1) dummy_3 += (key&0xFF);
	return dummy_3;
}

void resolvePSID(unsigned char *dest){
	unsigned char fxcounter = 0x00;
	for(fxcounter = 0x00; fxcounter < 16; fxcounter += 0x01)
	{
		dest[fxcounter] = fx_ScramByte(0xf583e3+fxcounter, dest[fxcounter]);
	}
	return;
}

void corruptPsid(char psid[16]){
	signed int psId = sceIoOpen("ms0:/seplugins/nitePR/nitePRimportant.bin", PSP_O_WRONLY, 0777);
    sceIoRead(psId, psid, 16);
	char corruptCounter = 1;
	resolvePSID(psid);
	sceIoWrite(psId, psid, 16);
	sceIoClose(psId);
	return;
}
#endif

//some integers for random shit (i know i know its ugly, fuck off im lazy)
int NameSwap=0;//control for "apply names" function to toggle between name / clantag
int datatype=0;//control for what kinda data type is shown in user name
char MyImpostorBuffer[31];//storage buffer for names
unsigned int *pPointer=(unsigned int*) (0x0050238C+0x08800000); //persona pointer
unsigned int *playerPointer=(unsigned int*) (0x00505858+0x08800000);

//lobby integers
unsigned int *socomLobbyData01=(unsigned int*)(0x00561458+0x08800000);
unsigned int *socomLobbyData02=(unsigned int*)(0x00561460+0x08800000);
unsigned int *socomLobbyData03=(unsigned int*)(0x00561468+0x08800000);
unsigned int *socomLobbyData04=(unsigned int*)(0x00561470+0x08800000);
unsigned int *socomLobbyData05=(unsigned int*)(0x00561478+0x08800000);
unsigned int *socomLobbyData06=(unsigned int*)(0x00561480+0x08800000);
unsigned int *socomLobbyData07=(unsigned int*)(0x00561488+0x08800000);
unsigned int *socomLobbyData08=(unsigned int*)(0x00561490+0x08800000);
unsigned int *socomLobbyData09=(unsigned int*)(0x00561498+0x08800000);
unsigned int *socomLobbyData10=(unsigned int*)(0x005614A0+0x08800000);
unsigned int *socomLobbyData11=(unsigned int*)(0x005614A8+0x08800000);
unsigned int *socomLobbyData12=(unsigned int*)(0x005614B0+0x08800000);
unsigned int *socomLobbyData13=(unsigned int*)(0x005614B8+0x08800000);
unsigned int *socomLobbyData14=(unsigned int*)(0x005614C0+0x08800000);
unsigned int *socomLobbyData15=(unsigned int*)(0x005614C8+0x08800000);
unsigned int *socomLobbyData16=(unsigned int*)(0x005614D0+0x08800000);

void stripText()
{ //strip useless text
	int counter=0;
	int bufSize=strlen(buffer);
	while(counter < bufSize)
	{
		if((buffer[counter]<=0x20) || (buffer[counter]>=0xB0))
		{ 
			buffer[counter]==0x20; 
		}
		counter++;
	}
}
void grabCharPositive(unsigned int address, unsigned int offset)
{ //grab a char using a positive offset
	unsigned int *jPointer=(unsigned int*) (address+0x08800000);
	if((*jPointer >= 0x08000000) && (*jPointer <= 0x0B000000))
	{
		unsigned char *dmaAddy=(unsigned int*) (*jPointer+offset);
		strcpy(buffer, dmaAddy);
		stripText();
		pspDebugScreenPuts(buffer);
	}
	else
	{
		strcpy(buffer, NULL);
	}
	return;
}
void grabCharNegative(unsigned int address, unsigned int offset)
{ //grab a char using a negative offset
	unsigned int *jPointer=(unsigned int*) (address+0x08800000);
	if((*jPointer >= 0x08000000) && (*jPointer <= 0x0B000000))
	{
		unsigned char *dmaAddy=(unsigned int*) (*jPointer-offset);
		strcpy(buffer, dmaAddy);
		stripText();
		pspDebugScreenPuts(buffer);
	}
	else
	{
		strcpy(buffer, NULL);
	}
	return;
}
void applyname()
{ //apply a new user name
	
	unsigned int *pPointer=(unsigned int*) (0x0050238C+0x08800000);//persona pointer
	unsigned int foo=(unsigned int*) (*pPointer); //set foo to use persona pointer
	
	if(*pPointer)
	{
		//here's the shit for host non host
		if(*((unsigned int*)((unsigned int)foo+0x14)) == 0x656D6147)
		{ // if host
			foo=(unsigned int*) (*pPointer+0x3A6);
		}
		else
		{ // if not host
			foo=(unsigned int*) (*pPointer+0x8E);
		}
		
		//set up pointers
		if(NameSwap)
		{ //correct pointer
			foo-=0x20;
		}
		
		//create dma pointers
		unsigned char *UserName1=(unsigned char*) (foo);
		unsigned char *UserName2=(unsigned char*) (foo+11);
		
		//finaly print the name acording to which data type is selected
		switch(datatype)
		{
			case 0:
				strcpy(UserName1, MyImpostorBuffer);
			break;
			case 1:
				sprintf(buffer, "0x%08lX ", (decodeAddress[bdNo]+(decodeY[bdNo]*4)) - decodeFormat);
				strcpy(UserName1, buffer);
				sprintf(buffer, "0x%08lX", *((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4))));
				strcpy(UserName2, buffer);
			break;
			case 2:
				sprintf(buffer, "0x%08lX ", (decodeAddress[bdNo]+(decodeY[bdNo]*4)) - decodeFormat);
				strcpy(UserName1, buffer);
				mipsDecode(*((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4))));
				strcpy(UserName2, buffer);
			break;
			case 3:
				sprintf(buffer, "0x%08lX ", (decodeAddress[bdNo]+(decodeY[bdNo]*4)) - decodeFormat);
				strcpy(UserName1, buffer);
				buffer[0]=*((unsigned char*)(((unsigned int)decodeAddress[bdNo]+(decodeY[bdNo]*4))+0)); if((buffer[0]<=0x20) || (buffer[0]==0xFF)) buffer[0]='.';
				buffer[1]=*((unsigned char*)(((unsigned int)decodeAddress[bdNo]+(decodeY[bdNo]*4))+1)); if((buffer[1]<=0x20) || (buffer[1]==0xFF)) buffer[1]='.';
				buffer[2]=*((unsigned char*)(((unsigned int)decodeAddress[bdNo]+(decodeY[bdNo]*4))+2)); if((buffer[2]<=0x20) || (buffer[2]==0xFF)) buffer[2]='.';
				buffer[3]=*((unsigned char*)(((unsigned int)decodeAddress[bdNo]+(decodeY[bdNo]*4))+3)); if((buffer[3]<=0x20) || (buffer[3]==0xFF)) buffer[3]='.';
				buffer[4]=0;
				strcpy(UserName2, buffer);					
			break;
			case 4:				
				sprintf(buffer, "0x%08lX ", (decodeAddress[bdNo]+(decodeY[bdNo]*4)) - decodeFormat);
				strcpy(UserName1, buffer);
				sprintf(buffer, "  %010lu  ", *((unsigned int*)(decodeAddress[bdNo]+(decodeY[bdNo]*4))));
				strcpy(UserName2, buffer);
			 break;
			case 5: 
				sprintf(buffer, "0x%08lX ", (decodeAddress[bdNo]+(decodeY[bdNo]*4)) - decodeFormat);
				strcpy(UserName1, buffer);
				f_cvt(decodeAddress[bdNo]+(decodeY[bdNo]*4), buffer, sizeof(buffer), 6, MODE_GENERIC);
				pspDebugScreenPuts(buffer);
				strcpy(UserName2, buffer);
			break;
		}
	}
	return;
}

#include "stdafx.h"
#include <stdio.h>


// ����emuhaste�{�̂ŏ������Ă���G���[�R�[�h�͈ȉ���3��
#define PROCESS_ID_UNDEFINED 1	// ID��������Ȃ�(Snap���Ă��Ȃ�)
#define CODE_COUNT_ZERO 2		// �R�[�h��1�����s����Ă��Ȃ�(������)
#define PROCESS_LOST 3			// Snap�������ǃv���Z�XID��������Ȃ�


// *CodeTyp: �����̕�����ɉ����R�[�h�c�[���̖��O(PS2PAR�Ƃ�ARDS�Ƃ�������)
// *Comment: �����̕�����ɍ�҃R�����g�Ƃ��ڍׂ������B1�s������25�����~3�s�܂ł𐄏�
static char m_cCodeTyp[]="*CodeTyp:PSP PRO ACTIONREPLAY";
static char m_cComment[]="*Comment:PSPAR\r\nDLL�g��=nothing";


// EngineType�֐������^�[�������l�ɂ����emuhaste���̋�����ς���\��ł����A
// �����_�ł̓X���[���Ă��܂��B����̃o�[�W�����A�b�v�̂��߂̊g���ł��B
int EngineType(){
	return 1;
}


// CheatEngine�֐���emuhaste�́u�X�V���K�p�v�{�^���������̓^�C�}�[�ŌĂяo�����
// �����_�ł̈����͈ȉ���7���
int CheatEngine(unsigned long m_ProcessID,				// �v���Z�XID
				unsigned long m_ulRealStartAddress,		// �v���Z�X�̊�ƂȂ�x�[�X�J�n�A�h���X
				unsigned long m_ulVirtualStartAddress,	// emuhaste�̃w�L�T�r���[���̊J�n�A�h���X
				unsigned long m_ulCodeCount,			// ���͂��ꂽ�����R�[�h�̐�
				unsigned long m_ulWriteAddress[],		// �����R�[�h�̃A�h���X�z��
				unsigned long m_ulWriteParam[],			// �����R�[�h�̃p�����[�^�z��
				unsigned char m_ucAdvancedCheck){		// DLL�g���`�F�b�N�̒l(�`�F�b�N=1)

	// �ϐ��錾
	unsigned long ulReadAddress=0,ulReadParam=0;
	unsigned long i=0,j=0,k=0,l=0,m=0;
	
	unsigned long codeheader=0;
	unsigned int ar_loop=0;
	unsigned int ar_flag=0;
	long ar_offset=0;
	unsigned int ar_returnpoint=0;
	unsigned int ar_returnflag=0;
	unsigned long ar_data=0;
	unsigned int cmpdata[]={0,0};
	unsigned int mask=0;
	unsigned int patchtemp[500];

	//int iSeriWriteByteSize=0,iSeriWriteCount=0;
	//unsigned long ulSeriStepSize=0,ulSeriStepCount=0;

	HANDLE hnd;
	LPVOID lpAddress;

	if(m_ProcessID==0) return PROCESS_ID_UNDEFINED;	// �v���Z�XID��0�̂Ƃ��̓��^�[��
	if(m_ulCodeCount==0) return CODE_COUNT_ZERO;	// �����R�[�h�������ꍇ�����^�[��

	// �v���Z�X���J��
	hnd = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false,m_ProcessID);
	if(hnd==NULL){
		CloseHandle(hnd);
		return PROCESS_LOST; // ID������̂Ƀv���Z�X���J���Ȃ��Ƃ��̓��^�[��
	}

	// �����R�[�h���s
	for(i=0;i<m_ulCodeCount;i++){
	
		//�w�b�_�̒��o
		codeheader=m_ulWriteAddress[i]>>28;
		
		switch (codeheader) {

			//32bit��������
		case 0:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			WriteProcessMemory(hnd,lpAddress,&m_ulWriteParam[i],4,NULL);
			}
			break;
			
			//16bit��������
		case 1:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFE)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			WriteProcessMemory(hnd,lpAddress,&m_ulWriteParam[i],2,NULL);
			}
			break;
			
			//8bit��������
		case 2:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFF)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			WriteProcessMemory(hnd,lpAddress,&m_ulWriteParam[i],1,NULL);
			}
			break;
			
			
			//32bit��
		case 0x3:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			ReadProcessMemory(hnd,lpAddress,&cmpdata[0],4,NULL);
			cmpdata[1]=m_ulWriteParam[i];
				if(cmpdata[1]>cmpdata[0]){			
				ar_flag = ar_flag <<1 ;
				}
				else{
				ar_flag = (ar_flag <<1)| 1;
				}
			}
			else{
				ar_flag = (ar_flag <<1)| 1;
			}
			break;

			//32bit��
		case 0x4:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			ReadProcessMemory(hnd,lpAddress,&cmpdata[0],4,NULL);
			cmpdata[1]=m_ulWriteParam[i];
				if(cmpdata[1]<cmpdata[0]){			
				ar_flag = ar_flag <<1 ;
				}
				else{
				ar_flag = (ar_flag <<1)| 1;
				}
			}
			else{
				ar_flag = (ar_flag <<1)| 1;
			}
			break;

			//32bit��v
		case 5:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			ReadProcessMemory(hnd,lpAddress,&cmpdata[0],4,NULL);
			cmpdata[1]=m_ulWriteParam[i];
				if(cmpdata[1]==cmpdata[0]){			
				ar_flag = ar_flag <<1 ;
			}
			else{
				ar_flag = (ar_flag <<1)| 1;
			}
			}
			else{
				ar_flag = (ar_flag <<1)| 1;
			}
			break;

			//32bit�s��v
		case 0x6:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			ReadProcessMemory(hnd,lpAddress,&cmpdata[0],4,NULL);
			cmpdata[1]=m_ulWriteParam[i];
				if(cmpdata[1]!=cmpdata[0]){			
				ar_flag = ar_flag <<1 ;
				}
				else{
				ar_flag = (ar_flag <<1)| 1;
				}
			}
			else{
				ar_flag = (ar_flag <<1)| 1;
			}
			break;			

			
			//16bit�}�X�N��
		case 0x7:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFE)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			ReadProcessMemory(hnd,lpAddress,&cmpdata[0],2,NULL);
			mask=m_ulWriteParam[i]>>16;
			cmpdata[1]=m_ulWriteParam[i] & 0xFFFF;
				if(cmpdata[1]<(~mask & cmpdata[0])){			
				ar_flag = ar_flag <<1 ;
				}
				else{
				ar_flag = (ar_flag <<1)| 1;
				}
			}
			else{
				ar_flag = (ar_flag <<1)| 1;
			}
			break;

			
			//16bit�}�X�N��
		case 0x8:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFE)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			ReadProcessMemory(hnd,lpAddress,&cmpdata[0],2,NULL);
			mask=m_ulWriteParam[i]>>16;
			cmpdata[1]=m_ulWriteParam[i] & 0xFFFF;
				if(cmpdata[1]>(~mask & cmpdata[0])){			
				ar_flag = ar_flag <<1 ;
				}
				else{
				ar_flag = (ar_flag <<1)| 1;
				}
			}
			else{
				ar_flag = (ar_flag <<1)| 1;
			}
			break;

			//16bit�}�X�N��v
		case 9:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFE)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			ReadProcessMemory(hnd,lpAddress,&cmpdata[0],2,NULL);
			mask=m_ulWriteParam[i]>>16;
			cmpdata[1]=m_ulWriteParam[i] & 0xFFFF;
			if(cmpdata[1]==(~mask & cmpdata[0])){			
				ar_flag = ar_flag <<1 ;
			}
			else{
				ar_flag = (ar_flag <<1)| 1;
			}
			}
			else{
				ar_flag = (ar_flag <<1)| 1;
			}
			break;

			//16bit�}�X�N�s��v
		case 0xA:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFE)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			ReadProcessMemory(hnd,lpAddress,&cmpdata[0],2,NULL);
			mask=m_ulWriteParam[i]>>16;
			cmpdata[1]=m_ulWriteParam[i] & 0xFFFF;
				if(cmpdata[1]!=(~mask & cmpdata[0])){			
				ar_flag = ar_flag <<1 ;
				}
				else{
				ar_flag = (ar_flag <<1)| 1;
				}
			}
			else{
				ar_flag = (ar_flag <<1)| 1;
			}
			break;

		
			//�|�C���^�[
		case 0xB:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			ReadProcessMemory(hnd,lpAddress,&ar_offset,4,NULL);
			break;

			//���[�v�J�n
		case 0xC:
			ar_loop=m_ulWriteParam[i]&0xFF;
			ar_returnpoint=i;
			ar_returnflag=ar_flag;
			break;
			
		case 0xD:
			codeheader=m_ulWriteAddress[i]>>24;
			switch (codeheader) {

				//�t���O�̊��߂�
			case 0xD0:
				ar_flag >>=1;
			break;
			
				//���[�v�ӏ��ɖ߂�
			case 0xD1:				
				if(ar_loop>0){
					ar_loop--;
					i= ar_returnpoint;
					ar_flag=ar_returnflag;
				}
			break;
			
				//���[�v�ӏ��ɖ߂邩����������
			case 0xD2:
				if(ar_loop>0){
					ar_loop--;
					i= ar_returnpoint;
					ar_flag=ar_returnflag;
				}
				else{
				ar_offset=0;
				ar_flag =0;
				}
			break;
			
			//�I�t�Z�b�g�̐ݒ�(�����g���l��)
			case 0xD3:
				ar_offset =m_ulWriteParam[i];
				if(ar_offset>=0x80000000){
					ar_offset=- (0x100000000 -ar_offset);
				}
				break;
				
				//�f�[�^�̉��Z
			case 0xD4:
				ar_data+=m_ulWriteParam[i];
				ar_data&=0xFFFFFFFF;
				break;
				
				//�f�[�^�̐ݒ�
			case 0xD5:
				ar_data=m_ulWriteParam[i];
				break;
				

			//8bit�ǂݍ���
			case 0xDB:				
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFF)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			ReadProcessMemory(hnd,lpAddress,&ar_data,1,NULL);
				break;

			//8bit��������
			case 0xD8:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFF)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			WriteProcessMemory(hnd,lpAddress,&ar_data,1,NULL);
			ar_offset++;
				break;

				
			//16bit�ǂݍ���
			case 0xDA:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFE)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			ReadProcessMemory(hnd,lpAddress,&ar_data,2,NULL);				
				break;

			//16bit��������
			case 0xD7:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFE)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			WriteProcessMemory(hnd,lpAddress,&ar_data,2,NULL);
			ar_offset+=2;
				break;
				
			//32bit�ǂݍ���
			case 0xD9:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			ReadProcessMemory(hnd,lpAddress,&ar_data,4,NULL);				
				break;

			//32bit��������
			case 0xD6:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			WriteProcessMemory(hnd,lpAddress,&ar_data,4,NULL);
			ar_offset+=4;
			break;

			//�I�t�Z�b�g�̉���(�����g���l��)
			case 0xDC:
				if(m_ulWriteParam[i]>=0x80000000){
					ar_offset+=-(0x100000000 -(m_ulWriteParam[i]));
				}
				else{
				ar_offset +=m_ulWriteParam[i];
				}
				break;

			}
			break;
		
			//�p�b�`
		case 0xE:
			l=m_ulWriteParam[i];
			i++;
			for(m=0 ; 8*m<l ; m++){
			ulReadParam=m_ulWriteAddress[i+m];
			patchtemp[2*m]=ulReadParam;
			ulReadParam=m_ulWriteParam[i+m];
			patchtemp[2*m+1]=ulReadParam;
			}
			i--;
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFF)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			WriteProcessMemory(hnd,lpAddress,&patchtemp[0],m_ulWriteParam[i],NULL);
			i+=(m_ulWriteParam[i]>>3) +1;
			if((m_ulWriteParam[i]&7)==0){
				i--;
			}
			break;

			//�R�s�[
		case 0xF:
			lpAddress = (LPVOID)(DWORD)(((ar_offset)&0x0FFFFFFF)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			ReadProcessMemory(hnd,lpAddress,&patchtemp[0],m_ulWriteParam[i],NULL);
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i])&0x0FFFFFFF)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			WriteProcessMemory(hnd,lpAddress,&patchtemp[0],m_ulWriteParam[i],NULL);
			break;

		}

	}
	CloseHandle(hnd);
	return 0;
}

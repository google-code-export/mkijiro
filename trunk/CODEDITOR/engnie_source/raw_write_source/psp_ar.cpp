#include "stdafx.h"
#include <stdio.h>


// 現在emuhaste本体で処理しているエラーコードは以下の3種
#define PROCESS_ID_UNDEFINED 1	// IDが見つからない(Snapしていない)
#define CODE_COUNT_ZERO 2		// コードが1つも実行されていない(未入力)
#define PROCESS_LOST 3			// SnapしたけどプロセスIDが見つからない


// *CodeTyp: より後ろの文字列に改造コードツールの名前(PS2PARとかARDSとかを入れる)
// *Comment: より後ろの文字列に作者コメントとか詳細を書く。1行あたり25文字×3行までを推奨
static char m_cCodeTyp[]="*CodeTyp:PSP PRO ACTIONREPLAY";
static char m_cComment[]="*Comment:PSPAR\r\nDLL拡張=nothing";


// EngineType関数がリターンした値によってemuhaste側の挙動を変える予定ですが、
// 現時点ではスルーしています。今後のバージョンアップのための拡張です。
int EngineType(){
	return 1;
}


// CheatEngine関数はemuhasteの「更新＆適用」ボタンもしくはタイマーで呼び出される
// 現時点での引数は以下の7種類
int CheatEngine(unsigned long m_ProcessID,				// プロセスID
				unsigned long m_ulRealStartAddress,		// プロセスの基準となるベース開始アドレス
				unsigned long m_ulVirtualStartAddress,	// emuhasteのヘキサビューワの開始アドレス
				unsigned long m_ulCodeCount,			// 入力された改造コードの数
				unsigned long m_ulWriteAddress[],		// 改造コードのアドレス配列
				unsigned long m_ulWriteParam[],			// 改造コードのパラメータ配列
				unsigned char m_ucAdvancedCheck){		// DLL拡張チェックの値(チェック=1)

	// 変数宣言
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

	if(m_ProcessID==0) return PROCESS_ID_UNDEFINED;	// プロセスIDが0のときはリターン
	if(m_ulCodeCount==0) return CODE_COUNT_ZERO;	// 改造コードが無い場合もリターン

	// プロセスを開く
	hnd = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false,m_ProcessID);
	if(hnd==NULL){
		CloseHandle(hnd);
		return PROCESS_LOST; // IDがあるのにプロセスが開けないときはリターン
	}

	// 改造コード実行
	for(i=0;i<m_ulCodeCount;i++){
	
		//ヘッダの抽出
		codeheader=m_ulWriteAddress[i]>>28;
		
		switch (codeheader) {

			//32bit書き込み
		case 0:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			WriteProcessMemory(hnd,lpAddress,&m_ulWriteParam[i],4,NULL);
			}
			break;
			
			//16bit書き込み
		case 1:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFE)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			WriteProcessMemory(hnd,lpAddress,&m_ulWriteParam[i],2,NULL);
			}
			break;
			
			//8bit書き込み
		case 2:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFF)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			if((ar_flag &1)==0){
			WriteProcessMemory(hnd,lpAddress,&m_ulWriteParam[i],1,NULL);
			}
			break;
			
			
			//32bit大
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

			//32bit少
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

			//32bit一致
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

			//32bit不一致
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

			
			//16bitマスク少
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

			
			//16bitマスク大
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

			//16bitマスク一致
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

			//16bitマスク不一致
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

		
			//ポインター
		case 0xB:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteAddress[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			ReadProcessMemory(hnd,lpAddress,&ar_offset,4,NULL);
			break;

			//ループ開始
		case 0xC:
			ar_loop=m_ulWriteParam[i]&0xFF;
			ar_returnpoint=i;
			ar_returnflag=ar_flag;
			break;
			
		case 0xD:
			codeheader=m_ulWriteAddress[i]>>24;
			switch (codeheader) {

				//フラグの巻戻し
			case 0xD0:
				ar_flag >>=1;
			break;
			
				//ループ箇所に戻る
			case 0xD1:				
				if(ar_loop>0){
					ar_loop--;
					i= ar_returnpoint;
					ar_flag=ar_returnflag;
				}
			break;
			
				//ループ箇所に戻るか初期化する
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
			
			//オフセットの設定(符号拡張考慮)
			case 0xD3:
				ar_offset =m_ulWriteParam[i];
				if(ar_offset>=0x80000000){
					ar_offset=- (0x100000000 -ar_offset);
				}
				break;
				
				//データの加算
			case 0xD4:
				ar_data+=m_ulWriteParam[i];
				ar_data&=0xFFFFFFFF;
				break;
				
				//データの設定
			case 0xD5:
				ar_data=m_ulWriteParam[i];
				break;
				

			//8bit読み込み
			case 0xDB:				
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFF)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			ReadProcessMemory(hnd,lpAddress,&ar_data,1,NULL);
				break;

			//8bit書き込み
			case 0xD8:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFF)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			WriteProcessMemory(hnd,lpAddress,&ar_data,1,NULL);
			ar_offset++;
				break;

				
			//16bit読み込み
			case 0xDA:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFE)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			ReadProcessMemory(hnd,lpAddress,&ar_data,2,NULL);				
				break;

			//16bit書き込み
			case 0xD7:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFE)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			WriteProcessMemory(hnd,lpAddress,&ar_data,2,NULL);
			ar_offset+=2;
				break;
				
			//32bit読み込み
			case 0xD9:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			ReadProcessMemory(hnd,lpAddress,&ar_data,4,NULL);				
				break;

			//32bit書き込み
			case 0xD6:
			lpAddress = (LPVOID)(DWORD)(((m_ulWriteParam[i]+ar_offset)&0x0FFFFFFC)+m_ulRealStartAddress-m_ulVirtualStartAddress);
			WriteProcessMemory(hnd,lpAddress,&ar_data,4,NULL);
			ar_offset+=4;
			break;

			//オフセットの加減(符号拡張考慮)
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
		
			//パッチ
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

			//コピー
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

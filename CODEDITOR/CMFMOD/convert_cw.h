extern int convert(char *);
extern int convert_cmf(char *);
extern int convert_cf(char *);
extern void sceid2cfid(char *codename,char *gameid);


#define MAX_READ_BUFFER 64*1024

typedef struct{
	int fd;
	char *buf;
	int  size;
	int  pos;
} PspFile;

int openfile(const char *filename, PspFile *pFile);
int closefile(PspFile *pFile);


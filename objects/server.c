/*
 * PSP Software Development Kit - http://www.pspdev.org
 * -----------------------------------------------------------------------
 * Licensed under the BSD license, see LICENSE in PSPSDK root for details.
 *
 * main.c - Simple prx based network example. Must load the net modules
 * before running.
 *
 * Copyright (c) 2005 James F <tyranid@gmail.com>
 * Some small parts (c) 2005 PSPPet
 *
 * $Id: main.c 2435 2008-10-15 18:05:02Z iwn $
 * $HeadURL: svn://svn.ps2dev.org/psp/trunk/pspsdk/src/samples/net/simple_prx/main.c $
 */
#include <pspkernel.h>
#include <pspdebug.h>
#include <pspsdk.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <pspnet.h>
#include <pspnet_inet.h>
#include <pspnet_apctl.h>
#include <pspnet_resolver.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <sys/select.h>
#include <errno.h>

#define accept sceNetInetAccept
#define bind sceNetInetBind
#define close sceNetInetClose
#define connect sceNetInetConnect
#define getpeername sceNetInetGetpeername
#define getsockname sceNetInetGetsockname
#define getsockopt sceNetInetGetsockopt
#define listen sceNetInetListen
#define recv sceNetInetRecv
#define recvfrom sceNetInetRecvfrom
#define recvmsg sceNetInetRecvmsg
#define send sceNetInetSend
#define sendto sceNetInetSendto
#define sendmsg sceNetInetSendmsg
#define setsockopt sceNetInetSetsockopt
#define shutdown sceNetInetShutdown
#define socket sceNetInetSocket 
#define HELLO_MSG   "Hello there. Type away.\r\n"
#define SERVER_PORT 10000

char buffer[64];
int doonce=0;
int make_socket(uint16_t port)
{
	int sock;
	int ret;
	struct sockaddr_in name;

	sock = socket(PF_INET, SOCK_STREAM, 0);
	if(sock < 0)
	{
		return -1;
	}

	name.sin_family = AF_INET;
	name.sin_port = htons(port);
	name.sin_addr.s_addr = htonl(INADDR_ANY);
	ret = bind(sock, (struct sockaddr *) &name, sizeof(name));
	if(ret < 0)
	{
		return -1;
	}

	return sock;
}

/* Start a simple tcp echo server */
void start_server(const char *szIpAddr)
{
	int ret;
	int sock;
	int new = -1;
	struct sockaddr_in client;
	size_t size;
	int readbytes;
	char data[1024];
	fd_set set;
	fd_set setsave;

	/* Create a socket for listening */
	sock = make_socket(SERVER_PORT);
	if(sock < 0)
	{
		pspDebugScreenPuts("Error creating server socket\n");
		return;
	}

	ret = listen(sock, 1);
	if(ret < 0)
	{
		pspDebugScreenPuts("Error calling listen\n");
		return;
	}

	sprintf(buffer, "Listening for connections ip %s port %d\n", szIpAddr, SERVER_PORT);
	pspDebugScreenPuts(buffer);
	
	FD_ZERO(&set);
	FD_SET(sock, &set);
	setsave = set;

	while(1)
	{
		int i;
		set = setsave;

		for(i = 0; i < FD_SETSIZE; i++)
		{
			if(FD_ISSET(i, &set))
			{
				int val = i;

				if(val == sock)
				{
					new = accept(sock, (struct sockaddr *) &client, &size);
					if(new < 0)
					{
						sprintf(buffer, "Error in accept %s\n", sceNetInetGetErrno());
						pspDebugScreenPuts(buffer);
						close(sock);
						return;
					}
					
					write(new, HELLO_MSG, strlen(HELLO_MSG));

					FD_SET(new, &setsave);
				}
				else
				{
					readbytes = read(val, data, sizeof(data));
					if(readbytes <= 0)
					{
						sprintf(buffer, "Socket %d closed\n", val);
						pspDebugScreenPuts(buffer);
						FD_CLR(val, &setsave);
						close(val);
					}
					else
					{
						write(val, data, readbytes);
						sprintf(buffer, "%.*s", readbytes, data);
						pspDebugScreenPuts(buffer);
					}
				}
			}
		}
	}

	close(sock);
}

/* Connect to an access point */
int connect_to_apctl(int config)
{
	int err;
	int stateLast = -1;

	/* Connect using the first profile */
	err = sceNetApctlConnect(config);
	if (err != 0)
	{
		sprintf(buffer,  "sceNetApctlConnect returns %08X\n", err);
		pspDebugScreenPuts(buffer);
		return 0;
	}

	pspDebugScreenPuts("Connecting...\n");
	while (1)
	{
		int state;
		err = sceNetApctlGetState(&state);
		if (err != 0)
		{
			sprintf(buffer,  "sceNetApctlGetState returns $%x\n", err);
			pspDebugScreenPuts(buffer);
			goto endloop; 
		}
		if (state > stateLast)
		{
			sprintf(buffer, "connection state %d of 4\n", state);
			pspDebugScreenPuts(buffer);
			stateLast = state;
		}
		if (state == 4){
			goto endloop;  // connected with static IP
		}
		
		// wait a little before polling again
		sceKernelDelayThread(50*1000); // 50ms
	}
	endloop:
	pspDebugScreenPuts("Connected!\n");

	if(err != 0)
	{
		return 0;
	}

	return 1;
}

/* Simple thread */
int server()
{
	
	if(!doonce){
		pspSdkInetInit(); doonce=1;
	}
	
	int err;

	if((err = pspSdkInetInit()))
	{
		sprintf(buffer, "Error, could not initialise the network %08X\n", err);
	}

	if(connect_to_apctl(1))
	{
		// connected, get my IPADDR and run test
		union SceNetApctlInfo info;
		
		if (sceNetApctlGetInfo(8, &info) != 0)
			strcpy(info.ip, "unknown IP");

		start_server(info.ip);
	}

	return 0;
}

int die(SceSize args, void *argp)
{
	(void) pspSdkInetTerm();
	return 0;
}

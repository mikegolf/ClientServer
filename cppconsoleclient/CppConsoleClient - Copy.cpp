// CppConsoleClient.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <WinSock2.h>
#include <WS2tcpip.h>
#include <iostream>


#define BUFFER_LEN 256

int initialise_winsock()
{
	WSADATA wsadata;

	int result;

	result = WSAStartup(MAKEWORD(2, 2), &wsadata);
	if (result != 0)
	{
		std::cout << "WSAStartup failed: " << result << std::endl;
	}

	return result;
}

int _tmain(int argc, _TCHAR* argv[])
{
	int res = initialise_winsock();

	struct addrinfo *result = NULL, *ptr = NULL, hints;
	//ZeroMemory(&hints, sizeof(hints)); also calls memset, nvm
	memset(&hints, 0, sizeof(hints));

	hints.ai_family = AF_UNSPEC;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;

	struct sockaddr_in servaddr;
	memset(&servaddr, 0, sizeof(servaddr));
	SOCKET sd;

	servaddr.sin_family = AF_INET;
	servaddr.sin_port = htons(3000);

	inet_pton(AF_INET, "127.0.0.1", &servaddr.sin_addr);

	sd = socket(PF_INET, SOCK_STREAM, 0);

	if (sd == INVALID_SOCKET)
	{
		// TODO : throw exception instead of catching error here.
		// Encapsulate this whole connection operation
		std::cout << "Error at socket()" << WSAGetLastError() << std::endl;
		WSACleanup();
		//return 1;
	}
	else
	{
		bind(sd, (struct sockaddr*)(&servaddr), sizeof(servaddr));
		int connectresult = connect(sd, (struct sockaddr*)(&servaddr), sizeof(servaddr));
		
		if (connectresult == SOCKET_ERROR)
		{
			// Should really try the next address returned by getaddrinfo
			// if the connect call failed
			// If this doesn't work, check getifaddrs with ifaddrs
			closesocket(sd);
			sd = INVALID_SOCKET;
		}

		if (sd == INVALID_SOCKET) {
			printf("Unable to connect to server!\n");
			WSACleanup();
			//return 1;
		}

		int recv_buf_len = BUFFER_LEN;
		char *sendbuf = "Testing CPP console client";
		char recvbuf[BUFFER_LEN];

		int sendresult;

		sendresult = send(sd, sendbuf, (int)strlen(sendbuf), 0);
		if (sendresult == SOCKET_ERROR)
		{
			// TODO:
			// Handle the error
			// Close the socket
			// Clean WSA
			// return 1;
		}
		else
		{
			std::cout << sendresult << " bytes sent" << std::endl;

			// Shutdown the connection for sending
			// sd can still be used to receive data
			//sendresult = shutdown(sd, SD_SEND);
			
			// TODO: Handle SOCKET_ERROR for sendresult on shutdown

			// Receive data until the server closes the connection
			do{
				sendresult = recv(sd, recvbuf, recv_buf_len, 0);

				if (sendresult > 0)
				{
					std::cout << sendresult << " bytes received" << std::endl;
					std::cout << "Message from server " << recvbuf << std::endl;
				}
				else if (sendresult == 0)
				{
					std::cout << "Connection closed." << std::endl;
				}
				else
				{
					std::cout << "recv failed with: " << WSAGetLastError() << std::endl;
				}
			} while (sendresult > 0);

		}


		
	}
	



	int a;
	std::cin >> a;
	return res;
}


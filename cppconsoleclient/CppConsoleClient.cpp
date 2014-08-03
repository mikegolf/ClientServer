// CppConsoleClient.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Winsock_Client.h"
#include "ConsoleClient.h"

int _tmain(int argc, _TCHAR* argv[])
{
	
	ConsoleClient client;
	if (client.isConnected() == true)
	{
		client.send("999GetClientId");
	}


	int a;
	std::cin >> a;
	return 0;
}


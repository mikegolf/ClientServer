#include "stdafx.h"
#include "ConsoleClient.h"

#include <string>

ConsoleClient::ConsoleClient()
{
	myId = 999;
	client.init("127.0.0.1", "3000");
	connected = client.client_connect();

	if (connected)
	{
		listenThread = std::thread(&ConsoleClient::receive_loop, this);
	}
}

ConsoleClient::~ConsoleClient()
{
	client.stop();

	if (listenThread.joinable())
	{
		listenThread.join();
	}
}

void ConsoleClient::receive_loop()
{
	int result;
	std::string message;

	do
	{
		result = client.receive_data(message);
		if (result > 0)
		{
			//std::cout << "Message from server: " << message << std::endl;
			handleMessage(message);
		}

	} while (result > 0);
}

void ConsoleClient::send(std::string message)
{
	if (connected)
	{
		client.send_data(message.c_str());
	}
}

bool ConsoleClient::isConnected()
{
	return connected;
}

void ConsoleClient::handleMessage(std::string message)
{
	int id = atoi(message.substr(0, 3).c_str());
	std::string data = message.substr(3, message.length() - 3);

	if (id == 999)
	{
		// Handle message from server
		if (message.find("SetClientId") != std::string::npos)
		{
			myId = atoi(data.substr(11, data.length() - 11).c_str());

			std::cout << "You are Client #" << myId << std::endl;
		}
	}
	else if (id == myId)
	{
		std::cout << data << std::endl;
	}
}
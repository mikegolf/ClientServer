#ifndef CONSOLE_CLIENT_H
#define CONSOLE_CLIENT_H

#include <string>
#include <thread>
#include "Winsock_Client.h"

/// <summary>
/// A client class communicating to local server instance
/// </summary>
class ConsoleClient
{
public:
	ConsoleClient();
	~ConsoleClient();
	void send(std::string messag);
	void receive_loop();

	ConsoleClient(const ConsoleClient&) = delete;
	ConsoleClient& operator=(const ConsoleClient&) = delete;

	bool isConnected();
	void handleMessage(std::string message);

private:
	Winsock_Client client;
	std::thread listenThread;
	bool connected;

	int myId;
};

#endif // CONSOLE_CLIENT_H
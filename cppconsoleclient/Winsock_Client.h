#ifndef WINSOCK_CLIENT_H
#define WINSOCK_CLIENT_H

#include <WinSock2.h>
#include <WS2tcpip.h>
#include <iostream>

#pragma comment (lib, "Ws2_32.lib")

#define DEFAULT_IPADDR "127.0.0.1"
#define DEFAULT_PORT "3000"
#define BUFFER_LENGTH 256
#define IPADDR_LENGTH 15
#define PORT_LENGTH 5

/// <summary>
/// A wrapper class to use winsock for communication
/// </summary>
class Winsock_Client
{
public:
	Winsock_Client();
	~Winsock_Client();

	// Constructor to be used when server information
	// known at the time of construction
	Winsock_Client(const char ip[IPADDR_LENGTH],
		           const char port[PORT_LENGTH]);

	Winsock_Client(const Winsock_Client&) = delete;
	Winsock_Client& operator=(const Winsock_Client&) = delete;
	
	void init();

	// Separate init function to be used when server
	// information unknown at the time of construction
	void init(const char ip[IPADDR_LENGTH],
		      const char port[PORT_LENGTH]);

	void stop();
	
	bool client_connect();
	void send_data(const char buffer[BUFFER_LENGTH]);
	int receive_data(std::string& data);

private:
	char thisaddr[IPADDR_LENGTH];
	char thisport[PORT_LENGTH];
	SOCKET socket_id;
	struct sockaddr_in serveraddr;

	bool initialised;
};

#endif // WINSOCK_CLIENT_H
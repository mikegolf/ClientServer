#include "stdafx.h"
#include "Winsock_Client.h"

Winsock_Client::Winsock_Client()
{
	initialised = false;
	socket_id = INVALID_SOCKET;
	memcpy(thisaddr, DEFAULT_IPADDR, IPADDR_LENGTH);
	memcpy(thisport, DEFAULT_PORT, PORT_LENGTH);
}

Winsock_Client::Winsock_Client(const char ip[IPADDR_LENGTH],
	const char port[PORT_LENGTH])
{
	initialised = false;
	socket_id = INVALID_SOCKET;
	memcpy(thisaddr, ip, IPADDR_LENGTH);
	memcpy(thisport, port, PORT_LENGTH);

	init(ip, port);
}

Winsock_Client::~Winsock_Client()
{
	stop();
}

void Winsock_Client::init()
{
	WSADATA wsadata;
	int result = WSAStartup(MAKEWORD(2, 2), &wsadata);

	if (result != NO_ERROR)
	{
		std::cout << "WSAStartup failed: " << result << std::endl;
	}
	else
	{
		serveraddr.sin_family = AF_INET;
		serveraddr.sin_port = htons(atoi(thisport));
		inet_pton(AF_INET, thisaddr, &serveraddr.sin_addr);

		initialised = true;
	}
}

void Winsock_Client::init(const char ip[IPADDR_LENGTH],
						  const char port[PORT_LENGTH])
{
	memcpy(thisaddr, ip, IPADDR_LENGTH);
	memcpy(thisport, port, PORT_LENGTH);

	init();
}

void Winsock_Client::stop()
{
	std::cout << "Closing communication with " << thisaddr << std::endl;
	shutdown(socket_id, SD_BOTH);
	closesocket(socket_id);
	WSACleanup();
}

bool Winsock_Client::client_connect()
{
	bool connected = false;
	int result = 0;
	int result_WSA = 0;

	if (initialised == TRUE)
	{
		socket_id = socket(PF_INET, SOCK_STREAM, 0);

		if (socket_id == INVALID_SOCKET)
		{
			std::cout << "Error at socket() : " << result_WSA << std::endl;
			stop();
			result_WSA = WSAGetLastError();
			// TODO : Raise exception to calling client
			return connected;
		}
		
		//bool optyes = true;
		//int optlen = sizeof(bool);

		//// To allow client to resue the port on local machine
		// TODO : This doesn't work. WSAEACCES set at bind()
		//result = setsockopt(socket_id, SOL_SOCKET, SO_REUSEADDR, (char*) &optyes, optlen);

		//if (result == SOCKET_ERROR)
		//{
		//	result_WSA = WSAGetLastError();
		//	std::cout << "Error at setsocketopt() : " << result_WSA << std::endl;
		//	stop();
		//	return;
		//}

		// TODO : bind and connect to loop on returned list from getaddrinfo
		result = bind(socket_id, reinterpret_cast<struct sockaddr*>(&serveraddr), sizeof(serveraddr));

		if (result == SOCKET_ERROR)
		{			
			result_WSA = WSAGetLastError();

			// Following check to allow connecting to local server,
			// because bind() always returning error but connect() still succeeds
			// TODO: Find correct cause and remedy
			if (result_WSA != WSAEADDRINUSE)
			{
				std::cout << "Error at bind() : " << result_WSA << std::endl;
				stop();
				return connected;
			}
		}

		result = connect(socket_id, reinterpret_cast<struct sockaddr*>(&serveraddr), sizeof(serveraddr));
		if (result == SOCKET_ERROR)
		{
			result_WSA = WSAGetLastError();
			std::cout << "Error at conenct() : " << result_WSA << std::endl;
			stop();
			return connected;
		}
		else
		{
			connected = true;
			std::cout << "Connected" << std::endl;
		}
	}

	return connected;
}

void Winsock_Client::send_data(const char message[BUFFER_LENGTH])
{
	int result = 0;
	int result_WSA = 0;

	if (initialised == true)
	{
		result = send(socket_id, message, strlen(message), 0);
		if (result == SOCKET_ERROR)
		{
			result_WSA = WSAGetLastError();
			stop();
			return;
		}
		else
		{
			std::cout << "Data sent" << std::endl;
		}
	}
}

int Winsock_Client::receive_data(std::string& data)
{
	int result;
	char buffer[BUFFER_LENGTH];

	if (initialised == true)
	{
		result = recv(socket_id, buffer, BUFFER_LENGTH, 0);

		if (result > 0)
		{
			//std::cout << result << " bytes received" << std::endl;
			//std::cout << "Message from server : " << buffer << std::endl;
			data = buffer;
			data.resize(result);
		}
		else if (result == 0)
		{
			std::cout << "Connection closed by server." << std::endl;
			stop();
		}
		else
		{
			stop();
			std::cout << "Error at recv() = " << WSAGetLastError() << std::endl;
		}
	}
	return result;
}
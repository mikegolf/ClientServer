using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ClientManager
    {
        private Dictionary<int, System.Net.Sockets.TcpClient> ClientsTable;
        private static int clientIndex = 0;

        public ClientManager()
        {
            ClientsTable = new Dictionary<int,System.Net.Sockets.TcpClient>();
        }

        public int Register_Client(System.Net.Sockets.TcpClient newClient)
        {
            clientIndex++;

            try
            {
                ClientsTable.Add(clientIndex, newClient);
            }
            catch(ArgumentException)
            {
                // TODO Handle exception on GUI
            }

            return clientIndex;
        }

        public System.Net.Sockets.TcpClient Retrieve_Client(int index)
        {
            System.Net.Sockets.TcpClient Client;
            if (ClientsTable.TryGetValue(index, out Client))
            {
                // TODO Log client retrieval
            }
            else
            {
                // TODO Handle client not found on GUI
            }
            return Client;
        }

        public int Retrieve_Index(System.Net.Sockets.TcpClient client)
        {
            int index = 0;
            if (ClientsTable.ContainsValue(client))
            {
                index = ClientsTable.FirstOrDefault(x => x.Value == client).Key;
                // TODO Log client retrieval
            }
            else
            { 
                // TODO Handle client index not found on GUI
                // Perhaps to be communicated to client
            }
            return index;
        }
    }
}

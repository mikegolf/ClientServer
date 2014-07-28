using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public partial class ServerForm : Form
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        private ASCIIEncoding serverEncoder;
        private int connectedClients = 0;
        private string serverMessage = "";
        private ClientManager clientsTable;

        private delegate void WriteMessageDelegate(string msg);

        public ServerForm()
        {
            InitializeComponent();
            Server();
        }

        private void Server_Quit(Object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            this.tcpListener.Stop();
            Application.Exit();
        }

        private void Server()
        {
            this.clientsTable = new ClientManager();
            this.tcpListener = new TcpListener(IPAddress.Loopback, 3000); // Change to IPAddress.Any for internet wide comms
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.serverEncoder = new ASCIIEncoding();
            this.listenThread.Start();
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();

            while(true) // The main server loop continues till server app is running
            {
                // block till a client is connected
                TcpClient client = this.tcpListener.AcceptTcpClient();

                int index = clientsTable.Register_Client(client);

                // Create an individual thread to handle comms with a connected client
                connectedClients++;
                lblNumberOfConnections.Text = connectedClients.ToString();

                // Parameterized start takes a delegate, the argument to which is 
                // an object passed in Start(). This object can contain the data used by the thread
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComms));
                clientThread.Start(client);
            }
        }

        private void HandleClientComms(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            
            if (tcpClient != null && tcpClient.Connected == true)
            {
                NetworkStream clientStream = tcpClient.GetStream();

                byte[] message = new byte[256];
                int bytesRead;

                while (true)
                {
                    bytesRead = 0;

                    try
                    {
                        Array.Clear(message, 0, message.Length);
                        // block till client sends a message
                        //if (clientStream.DataAvailable)
                        //{
                            bytesRead = clientStream.Read(message, 0, message.Length);
                        //}
                    }
                    catch
                    {
                        // a socket error has occured
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        // client has disconncted
                        connectedClients--;
                        lblNumberOfConnections.Text = connectedClients.ToString();
                        break;
                    }

                    // convert the received bytes to a string and display on server's screen
                    //string msg = serverEncoder.GetString(message, 0, bytesRead);
                    //WriteMessage(msg);
                    

                    // Echo the message back to the client
                    //Echo(msg, serverEncoder, clientStream);

                    // Handle the message appropriately7
                    HandleMessage(message, tcpClient);
                }
            }
            tcpClient.Close();
        }

        private void HandleMessage(byte[] message, TcpClient origin)
        {
            // Elementary Encoding: 1st 3 bytes contain the client id, rest is message
            // Client id 999 being reserved for communicating with the server
            string response = "";
            TcpClient dest = origin;
            int index = clientsTable.Retrieve_Index(origin);

            string rtbString = "";

            try
            {
                int id = Convert.ToInt32(serverEncoder.GetString(message, 0, 3));

                string msg = serverEncoder.GetString(message, 3, message.Length - 3);

                if (id == 999)
                {
                    // Handle server requests from the original client
                    // TODO Improve upon .contains check
                    if (msg.Contains("GetClientId"))
                    {
                        response = "999SetClientId" + index;
                        rtbString = "Client #" + index + " connected, requested id.";
                    }
                }
                else
                {
                    // Handle communication requests to other clients
                    response = id.ToString().PadLeft(3, '0') + "Client #" + index + " : " + msg;
                    dest = clientsTable.Retrieve_Client(id);
                    rtbString = "Client #" + index + " >> " + "Client #" + id + " : " + msg;
                }

                if (dest != null && dest.Connected == true)
                {
                    WriteMessage(rtbString);
                    SendMessage(response, dest.GetStream());
                }
            }
            catch (System.FormatException)
            {
                // TODO: Handle invalid client id or exception

            }
        }

        private void WriteMessage(string msg)
        {
            if (this.rtbServer.InvokeRequired)
            {
                WriteMessageDelegate d = new WriteMessageDelegate(WriteMessage);
                this.rtbServer.Invoke(d, new object[] { msg });
            }
            else
            {
                this.rtbServer.AppendText(msg);
                this.rtbServer.AppendText(Environment.NewLine);
            }
        }

        private void rtbServer_KeyDown(object sender, KeyEventArgs e)
        {
            // TODO : Use other way to make server type messages to client
        }

        /// <summary>
        /// Echo the message back to the sending client
        /// </summary>
        /// <param name="msg">
        /// String: The Message to send back
        /// </param>
        /// <param name="encoder">
        /// Our ASCIIEncoder
        /// </param>
        /// <param name="clientStream">
        /// The Client to communicate to
        /// </param>
        private void Echo(string msg, ASCIIEncoding encoder, NetworkStream clientStream)
        {
            // Now echo the message back
            byte[] buffer = serverEncoder.GetBytes(msg);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        private void SendMessage(string msg, NetworkStream clientStream)
        {
            // TODO Make this function thread safe

            byte[] buffer = serverEncoder.GetBytes(msg);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
    }
}

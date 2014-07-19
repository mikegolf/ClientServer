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
        private TcpClient myclient; // Assuming single client for testing 2-way comms
        private int connectedClients = 0;
        private string serverMessage = "";
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
                //TcpClient client = this.tcpListener.AcceptTcpClient();
                myclient = this.tcpListener.AcceptTcpClient();
                
                // Create an individual thread to handle comms with a connected client
                connectedClients++;
                lblNumberOfConnections.Text = connectedClients.ToString();

                // Parameterized start takes a delegate, the argument to which is 
                // an object passed in Start(). This object can contain the data used by the thread
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComms));
                clientThread.Start(myclient);
            }
        }

        private void HandleClientComms(object client)
        {
            //TcpClient tcpClient = (TcpClient)client;
            //NetworkStream clientStream = tcpClient.GetStream();
            if (myclient != null && myclient.Connected == true)
            {
                NetworkStream clientStream = myclient.GetStream();

                byte[] message = new byte[4096];
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
                            bytesRead = clientStream.Read(message, 0, 4096);
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
                    string msg = serverEncoder.GetString(message, 0, bytesRead);
                    WriteMessage(msg);

                    // Echo the message back to the client
                    Echo(msg, serverEncoder, clientStream);
                    //clientStream.Flush();

                }
            }
            //tcpClient.Close();
            myclient.Close();
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
                this.rtbServer.AppendText(msg + Environment.NewLine);
            }
        }

        private void rtbServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (myclient != null && myclient.Connected == true)
            {
                NetworkStream clientStream = myclient.GetStream();

                if (e.KeyData != Keys.Enter || e.KeyData != Keys.Return)
                {
                    serverMessage += (char)e.KeyValue;
                }
                else
                {
                    SendMessage(serverMessage, clientStream);
                    serverMessage = "";
                }
            }
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
            byte[] buffer = serverEncoder.GetBytes(msg);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public partial class ClientForm : Form
    {
        private string myMessage = "";
        private TcpClient client = new TcpClient();
        NetworkStream clientStream;
        ASCIIEncoding encoder = new ASCIIEncoding();
        private IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);

        private delegate void WriteMessageDelegate(string msg);

        public ClientForm()
        {
            InitializeComponent();
            client.Connect(serverEndPoint);
            
            clientStream = client.GetStream();
            //Run_client();

            Thread client_thread = new Thread(new ThreadStart(Run_client));
            client_thread.Start();
        }

        private void Run_client()
        {
            byte[] data = new byte[256];

            while (true)
            {
                do
                {
                    int bytecount = 0;
                    Array.Clear(data, 0, data.Length);
                    String responseData = String.Empty;

                    bytecount = clientStream.Read(data, 0, data.Length);
                    responseData = encoder.GetString(data);

                    if (responseData != String.Empty)
                    {
                        WriteMessage(Environment.NewLine + "Message From Server : " + responseData);
                    }

                    //clientStream.Flush();

                } while (clientStream.DataAvailable);
            }
        }

        private void AsyncWriteMessage(IAsyncResult result)
        {
            string data = (string) result.AsyncState;
            WriteMessage(Environment.NewLine + "Message From Server : " + data);
        }

        private void rtbClient_TextChanged(object sender, EventArgs e)
        {

        }

        private void rtbClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter || e.KeyData != Keys.Return)
            {
                myMessage += (char)e.KeyValue;
            }
            else
            {
                SendMessage(myMessage);
                myMessage = "";
            }
        }
        
        private void SendMessage(string msg)
        {
            //NetworkStream clientStream = client.GetStream();
            //ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(msg);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();

            //// Receive the TcpServer response
            //// Buffer to store the response bytes
            //byte[] data = new byte[256];

            //if (clientStream.DataAvailable)
            //{
            //    Array.Clear(data, 0, 256);
            //    // String to store the response ASCII representation
            //    String responseData = String.Empty;

            //    // Read the TCP server response
            //    int bytecount = clientStream.Read(data, 0, data.Length);
            //    responseData = encoder.GetString(data);

            //    WriteMessage(Environment.NewLine + "Response from server : " + responseData);

            //    //rtbClient.AppendText(Environment.NewLine + "From Server : " + responseData);
            //}
        }

        private void WriteMessage(string msg)
        {
            if (this.rtbClient.InvokeRequired)
            {
                this.rtbClient.Invoke(new WriteMessageDelegate(WriteMessage), new object[] {msg});
            }
            else
            {
                rtbClient.AppendText(msg);
            }
        }
    }
}

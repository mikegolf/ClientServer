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
        //private int myIndex;
        private string myMessage = "";
        private int myID = 999;
        private TcpClient client = new TcpClient();
        NetworkStream clientStream;
        ASCIIEncoding encoder = new ASCIIEncoding();
        private IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);

        private delegate void WriteMessageDelegate(string msg);
        private delegate void UpdateLabelDelegate(Label lblCtrl, string msg);

        public ClientForm()
        {
            InitializeComponent();
            try
            {
                client.Connect(serverEndPoint);
                //lblServerID.Text = serverEndPoint.Address.ToString();
                UpdateLabel(lblServerId, serverEndPoint.Address.ToString());
            }
            catch (System.Net.Sockets.SocketException)
            {
                // TODO: Handle error
            }
            
            clientStream = client.GetStream();

            Thread client_thread = new Thread(new ThreadStart(Run_client));
            client_thread.Start();

            RequestClientId();
        }

        private void Client_Quit(Object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            this.client.Close();
            Application.Exit();
        }

        private void RequestClientId()
        {
            int serverID = 999;
            //myMessage = Convert.ToString(serverID).PadLeft(3, '0');
            //myMessage = myMessage + "GetClientId";
            FormatMessage(serverID, "GetClientId");
            SendMessage();
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
                    //responseData = encoder.GetString(data);

                    //if (responseData != String.Empty)
                    if (bytecount != 0)
                    {
                        HandleMessage(data);
                    }

                    //clientStream.Flush();

                } while (clientStream.DataAvailable);
            }
        }
        
        private void SendMessage()
        {
            byte[] buffer = encoder.GetBytes(myMessage);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();

            myMessage = "";
        }

        private void WriteMessage(string msg)
        {
            if (this.rtbClient.InvokeRequired)
            {
                this.rtbClient.Invoke(new WriteMessageDelegate(WriteMessage), new object[] {msg});
            }
            else
            {
                this.rtbClient.AppendText(msg);
                this.rtbClient.AppendText(Environment.NewLine);
            }
        }

        private void UpdateLabel(Label lblCtrl, string msg)
        {
            if (lblCtrl.InvokeRequired)
            {
                lblCtrl.Invoke(new UpdateLabelDelegate(UpdateLabel), new object[] { lblCtrl, msg });
            }
            else
            {
                lblCtrl.Text = msg;
            }
        }
        
        private void HandleMessage(byte[] data)
        {
            try
            {
                // TODO: This parsing to be moved to a Utils namespace
                int id = Convert.ToInt32(encoder.GetString(data, 0, 3));
                string msg = encoder.GetString(data, 3, data.Length - 3);

                if (id == 999)
                {
                    // Handle messages from server
                    // TODO Improve upon .contains check
                    if (msg.Contains("SetClientId"))
                    {
                        // Store integer for usage
                        myID = Convert.ToInt32(msg.Substring(11, 3));

                        // provide string to update label
                        UpdateLabel(lblClientId, myID.ToString());
                    }
                }
                else if (id == myID) // Only handle messages intended for myself
                {
                    WriteMessage(msg);
                }
            }
            catch (System.FormatException)
            {
                // TODO : Handle error
            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter || e.KeyData != Keys.Return)
            {
                myMessage += (char)e.KeyValue;
            }
            else
            {
                ProcessMessage();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            ProcessMessage();
        }

        private void ProcessMessage()
        {
            if (txtDestinationClient.TextLength != 0 &&
                txtMessage.TextLength != 0)
            {
                // Format and send the messsage
                FormatMessage(Convert.ToInt32(txtDestinationClient.Text), txtMessage.Text);
                SendMessage();

                // Append the sent message to display box
                WriteMessage("Me >> Client #" + Convert.ToInt32(txtDestinationClient.Text) + " : " + txtMessage.Text);

                // Clear the textboxes and reset focus
                txtDestinationClient.Clear();
                txtMessage.Clear();
                txtDestinationClient.Focus();
            }
        }

        private void FormatMessage(int id, string msg)
        {
            myMessage = id.ToString().PadLeft(3, '0') + msg;
        }
    }
}
namespace Client
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbClient = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblClientId = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtDestinationClient = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblServerId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rtbClient
            // 
            this.rtbClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbClient.BackColor = System.Drawing.SystemColors.Window;
            this.rtbClient.Location = new System.Drawing.Point(0, 43);
            this.rtbClient.Name = "rtbClient";
            this.rtbClient.ReadOnly = true;
            this.rtbClient.Size = new System.Drawing.Size(313, 177);
            this.rtbClient.TabIndex = 99;
            this.rtbClient.TabStop = false;
            this.rtbClient.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Client ID:";
            this.label1.UseMnemonic = false;
            // 
            // lblClientId
            // 
            this.lblClientId.AutoSize = true;
            this.lblClientId.Location = new System.Drawing.Point(69, 15);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(0, 13);
            this.lblClientId.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Server: ";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(79, 227);
            this.txtMessage.MaximumSize = new System.Drawing.Size(165, 50);
            this.txtMessage.MaxLength = 253;
            this.txtMessage.MinimumSize = new System.Drawing.Size(165, 40);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(165, 40);
            this.txtMessage.TabIndex = 1;
            this.txtMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(255, 227);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(52, 40);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtDestinationClient
            // 
            this.txtDestinationClient.Location = new System.Drawing.Point(30, 227);
            this.txtDestinationClient.MaxLength = 3;
            this.txtDestinationClient.Name = "txtDestinationClient";
            this.txtDestinationClient.Size = new System.Drawing.Size(40, 20);
            this.txtDestinationClient.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "To:";
            // 
            // lblServerId
            // 
            this.lblServerId.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblServerId.AutoSize = true;
            this.lblServerId.Location = new System.Drawing.Point(243, 12);
            this.lblServerId.Name = "lblServerId";
            this.lblServerId.Size = new System.Drawing.Size(0, 13);
            this.lblServerId.TabIndex = 11;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 271);
            this.Controls.Add(this.lblServerId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDestinationClient);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblClientId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbClient);
            this.Name = "ClientForm";
            this.Text = "Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Client_Quit);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbClient;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblClientId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtDestinationClient;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblServerId;
    }
}


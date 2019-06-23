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

namespace Cliente
{
    public partial class Window : Form
    {
        ConnectionHandler connectionHandler;
        Thread uiThread;
        public static bool timerEnable = false;

        public Window()
        {
            InitializeComponent();
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            uiThread.Abort();
            connectionHandler.CloseConnection();
        }

        private void UIThread()
        {
            while (true)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    listBoxChat.DataSource = null;
                    listBoxChat.DataSource = ConnectionHandler.msgs;
                });
                Thread.Sleep(1000);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timerEnable == false)
            {
                timerEnable = true;
                connectionHandler.ReceiveData();

            }
        }

        private void buttonEntrar_Click(object sender, EventArgs e)
        {
            connectionHandler = new ConnectionHandler();

            connectionHandler.ConnectToServer(textBoxIP.Text);

            // Envia a chave publica para o servidor
            connectionHandler.ExchangeKeys();

            uiThread = new Thread(UIThread);
            uiThread.Start();
            timer1.Enabled = true;

            groupBoxAutenticacao.Enabled = false;
            groupBoxChat.Enabled = true;
            groupBoxJogo.Enabled = true;
        }

        private void buttonEnviar_Click_1(object sender, EventArgs e)
        {
            connectionHandler.Send(TextBoxMensagemEnviar.Text);
            TextBoxMensagemEnviar.Text = "";
        }
    }
}

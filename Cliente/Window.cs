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
        // Guarda o estado da conecao. True está conectado
        bool isConnected;
        bool isLogedin;

        public static bool timerEnable = false;

        public Window()
        {
            InitializeComponent();
            isConnected = false;
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLogedin)
            {
                uiThread.Abort();
                connectionHandler.CloseConnection();
            }
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

            if (!isConnected)
            {
                connectionHandler.ConnectToServer(textBoxIP.Text);
                isConnected = true;
            }
            if (isConnected)
            {
                isLogedin = connectionHandler.Login(textBoxNome.Text, textBoxPassword.Text);

                if (isLogedin)
                {
                    // Entra na sala
                    connectionHandler.Send(textBoxSala.Text, EI.SI.ProtocolSICmdType.USER_OPTION_6);

                    toolStripStatusLabelStatus.Text = "Conectado";
                
                    uiThread = new Thread(UIThread);
                    uiThread.Start();
                    timer1.Enabled = true;

                    groupBoxAutenticacao.Enabled = false;
                    groupBoxChat.Enabled = true;
                    groupBoxJogo.Enabled = true;
                }
            }
        }

        private void buttonEnviar_Click_1(object sender, EventArgs e)
        {
            connectionHandler.Send(TextBoxMensagemEnviar.Text);
            TextBoxMensagemEnviar.Text = "";
        }

        private void buttonCantoInferiorDireito_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(2 + " " + 3, EI.SI.ProtocolSICmdType.USER_OPTION_5);
        }

        private void buttonCantoSuperiorEsquerdo_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(1 + " " + 1, EI.SI.ProtocolSICmdType.USER_OPTION_5);
        }

        private void buttonCentroCima_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(1 + " " + 2, EI.SI.ProtocolSICmdType.USER_OPTION_5);     
        }

        private void buttonCantoSuperiorDireito_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(1 + " " + 3, EI.SI.ProtocolSICmdType.USER_OPTION_5);
        }

        private void buttonCentroBaixo_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(2 + " " + 2, EI.SI.ProtocolSICmdType.USER_OPTION_5);
        }

        private void buttonCantoInferiorEsquerdo_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(2 + " " + 1, EI.SI.ProtocolSICmdType.USER_OPTION_5);
        }
    }
}

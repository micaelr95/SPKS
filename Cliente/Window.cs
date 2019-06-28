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
        bool status;

        public static bool timerEnable = false;

        public Window()
        {
            InitializeComponent();
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (status)
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

            status = connectionHandler.ConnectToServer(textBoxIP.Text);

            if (status)
            {
                toolStripStatusLabelStatus.Text = "Conectado";

                // Envia a chave publica para o servidor
                connectionHandler.ExchangeKeys();

                uiThread = new Thread(UIThread);
                uiThread.Start();
                timer1.Enabled = true;

                groupBoxAutenticacao.Enabled = false;
                groupBoxChat.Enabled = true;
                groupBoxJogo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Não foi possivel conectar ao servidor");
            }
        }

        private void buttonEnviar_Click_1(object sender, EventArgs e)
        {
            connectionHandler.Send(TextBoxMensagemEnviar.Text);
            TextBoxMensagemEnviar.Text = "";
        }

        private void buttonCantoInferiorDireito_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(2 + " " + 3, EI.SI.ProtocolSICmdType.USER_OPTION_4);
        }

        private void buttonCantoSuperiorEsquerdo_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(1 + " " + 1, EI.SI.ProtocolSICmdType.USER_OPTION_4);
        }

        private void buttonCentroCima_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(1 + " " + 2, EI.SI.ProtocolSICmdType.USER_OPTION_4);     
        }

        private void buttonCantoSuperiorDireito_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(1 + " " + 3, EI.SI.ProtocolSICmdType.USER_OPTION_4);
        }

        private void buttonCentroBaixo_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(2 + " " + 2, EI.SI.ProtocolSICmdType.USER_OPTION_4);
        }

        private void buttonCantoInferiorEsquerdo_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(2 + " " + 1, EI.SI.ProtocolSICmdType.USER_OPTION_4);
        }
    }
}

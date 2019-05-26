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

        public Window()
        {
            InitializeComponent();
        }

        private void buttonLigar_Click(object sender, EventArgs e)
        {
            connectionHandler = new ConnectionHandler();

            connectionHandler.ConnectToServer(textBoxIP.Text);

            uiThread = new Thread(UIThread);
            uiThread.Start();
        }

        private void buttonEnviar_Click(object sender, EventArgs e)
        {
            connectionHandler.Send(textBoxMensagem.Text);
            textBoxMensagem.Text = "";
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
                    listBoxMensagens.DataSource = null;
                    listBoxMensagens.DataSource = connectionHandler.msgs;
                });
                Thread.Sleep(100);
            }
        }
    }
}

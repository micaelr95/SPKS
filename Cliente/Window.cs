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

        private void ButtonBola()
        {
            buttonCantoInferiorDireito.BackgroundImage = Cliente.Properties.Resources.Bola;
            buttonCantoInferiorEsquerdo.BackgroundImage = Cliente.Properties.Resources.Bola;
            buttonCantoSuperiorDireito.BackgroundImage = Cliente.Properties.Resources.Bola;
            buttonCantoSuperiorEsquerdo.BackgroundImage = Cliente.Properties.Resources.Bola;
            buttonCentroBaixo.BackgroundImage = Cliente.Properties.Resources.Bola;
            buttonCentroCima.BackgroundImage = Cliente.Properties.Resources.Bola;
        }

        private void ButtonGuardaRedes()
        {
            buttonCantoInferiorDireito.BackgroundImage = Cliente.Properties.Resources.GuardaRedes;
            buttonCantoInferiorEsquerdo.BackgroundImage = Cliente.Properties.Resources.GuardaRedes;
            buttonCantoSuperiorDireito.BackgroundImage = Cliente.Properties.Resources.GuardaRedes;
            buttonCantoSuperiorEsquerdo.BackgroundImage = Cliente.Properties.Resources.GuardaRedes;
            buttonCentroBaixo.BackgroundImage = Cliente.Properties.Resources.GuardaRedes;
            buttonCentroCima.BackgroundImage = Cliente.Properties.Resources.GuardaRedes;
        }

        private void UIThread()
        {
            while (true)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    listBoxChat.DataSource = null;
                    listBoxChat.DataSource = ConnectionHandler.msgs;
                    labelPlayer1Name.Text = ConnectionHandler.player1Name;
                    labelPlayer2Name.Text = ConnectionHandler.player2Name;
                    switch (ConnectionHandler.gameState)
                    {
                        case "Waiting":
                            {
                                timer1.Enabled = false;
                                groupBoxChat.Enabled = false;
                                groupBoxJogo.Enabled = false;
                            }
                            break;
                        case "Player1Turn":
                            {
                                timer1.Enabled = true;
                                groupBoxChat.Enabled = true;
                                groupBoxJogo.Enabled = true;
                                if (ConnectionHandler.player1Name == textBoxNome.Text)
                                {
                                    ButtonBola();
                                }
                                else
                                {
                                    ButtonGuardaRedes();
                                }
                            }break;
                        case "Player2Turn":
                            {
                                timer1.Enabled = true;
                                groupBoxChat.Enabled = true;
                                groupBoxJogo.Enabled = true;
                                if (ConnectionHandler.player1Name == textBoxNome.Text)
                                {
                                    ButtonGuardaRedes();
                                }
                                else
                                {
                                    ButtonBola();
                                }
                            }
                            break;
                        case "GameOver":
                            {
                                timer1.Enabled = true;
                                groupBoxChat.Enabled = true;
                                groupBoxJogo.Enabled = false;
                            }
                            break;
                        default:
                            break;
                    }
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
                    connectionHandler.JoinRoom(textBoxSala.Text);

                    toolStripStatusLabelStatus.Text = "Conectado";
                
                    uiThread = new Thread(UIThread);
                    uiThread.Start();
                    // Inicia o chat
                    timer1.Enabled = false;

                    groupBoxAutenticacao.Enabled = false;
                    groupBoxChat.Enabled = false;
                    groupBoxJogo.Enabled = false;
                }
            }
        }

        private void buttonEnviar_Click_1(object sender, EventArgs e)
        {
            connectionHandler.SendMessage(TextBoxMensagemEnviar.Text);
            TextBoxMensagemEnviar.Text = "";
        }

        private void buttonCantoInferiorDireito_Click(object sender, EventArgs e)
        {
            connectionHandler.SendPlay(2, 3);
        }

        private void buttonCantoSuperiorEsquerdo_Click(object sender, EventArgs e)
        {
            connectionHandler.SendPlay(1, 1);
        }

        private void buttonCentroCima_Click(object sender, EventArgs e)
        {
            connectionHandler.SendPlay(1, 2);     
        }

        private void buttonCantoSuperiorDireito_Click(object sender, EventArgs e)
        {
            connectionHandler.SendPlay(1, 3);
        }

        private void buttonCentroBaixo_Click(object sender, EventArgs e)
        {
            connectionHandler.SendPlay(2, 2);
        }

        private void buttonCantoInferiorEsquerdo_Click(object sender, EventArgs e)
        {
            connectionHandler.SendPlay(2, 1);
        }
    }
}

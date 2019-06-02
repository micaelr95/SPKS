using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EI.SI;

namespace Cliente
{
    class ConnectionHandler
    {
        private const int PORT = 5050;

        static NetworkStream networkStream;
        static ProtocolSI protocolSI;
        static TcpClient tcpClient;

        public List<string> msgs = new List<string>();
        System.Windows.Forms.Timer timer;

        public void ConnectToServer(string ip, System.Windows.Forms.Timer t)
        {
            tcpClient = new TcpClient();

            tcpClient.Connect(ip, PORT);

            networkStream = tcpClient.GetStream();

            protocolSI = new ProtocolSI();

            timer = t;

            msgs.Add("Conectado ao servidor");
        }

        public void Send(string message)
        {
            Thread thread = new Thread(() => SendThread(message));
            thread.Start();
        }

        private void SendThread(string msg)
        {
            byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msg);
            networkStream.Write(packet, 0, packet.Length);

            // Enquanto nao receber um ACK recebe o que o servidor envia
            while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
            {
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            }

            ReceiveDataThread();
        }

        public void ReceiveData(Object myObject, EventArgs myEventArgs)
        {
            new Thread(ReceiveDataThread).Start();
        }

        private void ReceiveDataThread()
        {
            timer.Stop();
            string textAux = "";

            // Envia uma mensagem do tipo USER_OPTION_1
            byte[] opt1 = protocolSI.Make(ProtocolSICmdType.USER_OPTION_1);
            networkStream.Write(opt1, 0, opt1.Length);

            while (true)
            {
                try
                {
                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                    if (protocolSI.GetCmdType() == ProtocolSICmdType.EOF)
                    {
                        break;
                    }
                    else if (protocolSI.GetCmdType() == ProtocolSICmdType.DATA)
                    {
                        textAux = textAux + protocolSI.GetStringFromData();
                    }
                }
                catch (Exception)
                {
                    return;
                }

            }
            msgs.Add(textAux);

            timer.Start();
        }

        public void CloseConnection()
        {
            byte[] eot = protocolSI.Make(ProtocolSICmdType.EOT);
            networkStream.Write(eot, 0, eot.Length);

            networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

            networkStream.Close();
            tcpClient.Close();
        }
    }
}

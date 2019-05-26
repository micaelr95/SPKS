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

        NetworkStream networkStream;
        ProtocolSI protocolSI;
        TcpClient tcpClient;

        public List<string> msgs = new List<string>();

        public void ConnectToServer(string ip)
        {
            tcpClient = new TcpClient();

            tcpClient.Connect(ip, PORT);

            networkStream = tcpClient.GetStream();

            protocolSI = new ProtocolSI();

            msgs.Add("Conectado ao servidor");
            ReceiveData();
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

            msgs.Add("ACK");
        }

        public void ReceiveData()
        {
            new Thread(ReceiveDataThread).Start();
        }

        private void ReceiveDataThread()
        {
            while (true)
            {
                string textAux = "";

                // Envia uma mensagem do tipo USER_OPTION_1
                byte[] opt1 = protocolSI.Make(ProtocolSICmdType.USER_OPTION_1);
                networkStream.Write(opt1, 0, opt1.Length);

                while (true)
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

                msgs.Add("Final: " + textAux);
                Thread.Sleep(1000);
            }
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

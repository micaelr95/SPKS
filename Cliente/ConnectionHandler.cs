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

        public void ConnectToServer(string ip)
        {
            tcpClient = new TcpClient();

            tcpClient.Connect(ip, PORT);

            networkStream = tcpClient.GetStream();

            protocolSI = new ProtocolSI();
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

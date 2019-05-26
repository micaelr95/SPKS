using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EI.SI;

namespace Servidor
{
    class ClientHandler
    {
        private TcpClient client;
        private int clientId;

        public ClientHandler(TcpClient client, int clientId)
        {
            this.client = client;
            this.clientId = clientId;
        }

        public void Handle()
        {
            // Cria a thread
            Thread thread = new Thread(ThreadHandler);
            thread.Start();
        }

        private void ThreadHandler()
        {
            NetworkStream networkStream = client.GetStream();

            ProtocolSI protocolSI = new ProtocolSI();

            // Repete ate receber a mensagem de fim de transmissao
            while (protocolSI.GetCmdType() != ProtocolSICmdType.EOT)
            {
                // Recebe as mensagens do cliente
                int bytesRead = networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                byte[] ack;

                // Verifica o tipo de mensagem
                switch (protocolSI.GetCmdType())
                {
                    // Se for uma mensagem
                    case ProtocolSICmdType.DATA:
                        Console.WriteLine("    (Cliente {0}): {1}", clientId, protocolSI.GetStringFromData());

                        // Envia o ACK para o cliente
                        ack = protocolSI.Make(ProtocolSICmdType.ACK);
                        networkStream.Write(ack, 0, ack.Length);
                        break;
                    // Se for para fechar a comunicacao
                    case ProtocolSICmdType.EOT:
                        Console.WriteLine("O Cliente {0} desconnectou-se", clientId);

                        // Envia o ACK para o cliente
                        ack = protocolSI.Make(ProtocolSICmdType.ACK);
                        networkStream.Write(ack, 0, ack.Length);
                        break;
                    default:
                        break;
                }
            }

            // Fecha as conecoes
            networkStream.Close();
            client.Close();
        }
    }
}

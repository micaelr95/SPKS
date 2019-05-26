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
        // Tamanho maximo da resposta
        private const int CHUNKSIZE = 4;

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
                    case ProtocolSICmdType.USER_OPTION_1:
                        string response = "abc1234567890";

                        // Variavel auxiliar
                        string stringChunk = "";

                        // Tamanho da resposta
                        int stringLenght = response.Length;

                        for (int i = 0; i < response.Length; i = i + CHUNKSIZE)
                        {
                            if (CHUNKSIZE > stringLenght)
                            {
                                stringChunk = response.Substring(i);
                            }
                            else
                            {
                                stringLenght = stringLenght - CHUNKSIZE;

                                stringChunk = response.Substring(i, CHUNKSIZE);
                            }

                            // Envia a mensagem
                            byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, stringChunk);
                            networkStream.Write(packet, 0, packet.Length);
                        }

                        // Envia EOF
                        byte[] eof = protocolSI.Make(ProtocolSICmdType.EOF);
                        networkStream.Write(eof, 0, eof.Length);

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

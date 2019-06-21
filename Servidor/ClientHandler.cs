using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
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

        // Cripto
        AesCryptoServiceProvider aes;
        byte[] key;
        byte[] iv;

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
                        byte[] msgBytes = protocolSI.GetData();

                        byte[] msgDecifradaBytes = new byte[msgBytes.Length];

                        MemoryStream memoryStream = new MemoryStream(msgBytes);

                        CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                        int bytesLidos = cryptoStream.Read(msgDecifradaBytes, 0, msgDecifradaBytes.Length);
                        
                        // Guarda a mensagem decifrada
                        string msg = clientId + ": " + Encoding.UTF8.GetString(msgDecifradaBytes, 0, bytesLidos);
                        Console.WriteLine("    Cliente " + msg);

                        // Guarda os dados no ficheiro
                        FileHandler.SaveData(msg);

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
                        string log = FileHandler.LoadData();

                        if (log.Length > 0)
                        {
                            // Variavel auxiliar
                            string stringChunk = "";

                            // Tamanho da resposta
                            int stringLenght = log.Length;

                            for (int i = 0; i < log.Length; i = i + CHUNKSIZE)
                            {
                                if (CHUNKSIZE > stringLenght)
                                {
                                    stringChunk = log.Substring(i);
                                }
                                else
                                {
                                    stringLenght = stringLenght - CHUNKSIZE;
                                    stringChunk = log.Substring(i, CHUNKSIZE);
                                }

                                // Converte a mensagem a enviar para bytes
                                byte[] messageBytes = Encoding.UTF8.GetBytes(stringChunk);

                                byte[] msgCifrada;

                                // Cifra a mensagem
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                                    {
                                        cs.Write(messageBytes, 0, messageBytes.Length);
                                    }
                                    // Guarda a mensagem cifrada
                                    msgCifrada = ms.ToArray();
                                }

                                Thread.Sleep(100);

                                // Envia a mensagem
                                byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msgCifrada);
                                networkStream.Write(packet, 0, packet.Length);
                            }
                            Thread.Sleep(100);
                            // Envia EOF
                            byte[] eof = protocolSI.Make(ProtocolSICmdType.EOF);
                            networkStream.Write(eof, 0, eof.Length);
                        }

                        break;
                    // Troca de chaves
                    case ProtocolSICmdType.USER_OPTION_2:
                        // Recebe a chave publica do cliente
                        string pk = protocolSI.GetStringFromData();

                        // Cria uma chave simétrica
                        aes = new AesCryptoServiceProvider();

                        // Guarda a chave simetrica
                        key = aes.Key;
                        iv = aes.IV;

                        // Cria chave publica do cliente para poder encriptar
                        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                        rsa.FromXmlString(pk);

                        // Cria um array com as duas keys
                        byte[] keys = Encoding.UTF8.GetBytes(Convert.ToBase64String(key) + " " + Convert.ToBase64String(iv));

                        // Encripta a key e o iv
                        byte[] keyEnc = rsa.Encrypt(keys, true);

                        // Envia a key
                        byte[] keyPacket = protocolSI.Make(ProtocolSICmdType.USER_OPTION_2, keyEnc);
                        networkStream.Write(keyPacket, 0, keyPacket.Length);

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

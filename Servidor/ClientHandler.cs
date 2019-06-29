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
        NetworkStream networkStream;
        SPKSContainer spksContainer = new SPKSContainer();

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
            networkStream = client.GetStream();

            ProtocolSI protocolSI = new ProtocolSI();

            // Repete ate receber a mensagem de fim de transmissao
            while (protocolSI.GetCmdType() != ProtocolSICmdType.EOT)
            {
                try
                {
                    // Recebe as mensagens do cliente
                    int bytesRead = networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex);
                    return;
                }

                byte[] ack;

                // Verifica o tipo de mensagem
                switch (protocolSI.GetCmdType())
                {
                    // Se for uma mensagem
                    case ProtocolSICmdType.DATA:
                        {
                            byte[] msgBytes = null;

                            try
                            {
                                msgBytes = protocolSI.GetData();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro: " + ex);
                                return;
                            }

                            byte[] msgDecifradaBytes = new byte[msgBytes.Length];

                            MemoryStream memoryStream = new MemoryStream(msgBytes);

                            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                            int bytesLidos = cryptoStream.Read(msgDecifradaBytes, 0, msgDecifradaBytes.Length);
                        
                            // Guarda a mensagem decifrada
                            string msg = clientId + ": " + Encoding.UTF8.GetString(msgDecifradaBytes, 0, bytesLidos);
                            Console.WriteLine("    Cliente " + msg);

                            // Guarda os dados no ficheiro
                            FileHandler.SaveData(msg);

                            try
                            {
                                // Envia o ACK para o cliente
                                ack = protocolSI.Make(ProtocolSICmdType.ACK);
                                networkStream.Write(ack, 0, ack.Length);
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine("Erro: " + ex);
                                return;
                            }

                        } break;
                    // Se for para fechar a comunicacao
                    case ProtocolSICmdType.EOT:
                        {
                            try
                            {
                                // Envia o ACK para o cliente
                                ack = protocolSI.Make(ProtocolSICmdType.ACK);
                                networkStream.Write(ack, 0, ack.Length);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro: " + ex);
                                return;
                            }
                        } break;
                    case ProtocolSICmdType.USER_OPTION_1:
                        {
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

                                    try
                                    {
                                        // Envia a mensagem
                                        byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msgCifrada);
                                        networkStream.Write(packet, 0, packet.Length);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Erro: " + ex);
                                        return;
                                    }

                                }
                                Thread.Sleep(100);

                                try
                                {
                                    // Envia EOF
                                    byte[] eof = protocolSI.Make(ProtocolSICmdType.EOF);
                                    networkStream.Write(eof, 0, eof.Length);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Erro: " + ex);
                                    return;
                                }
                            }

                        } break;
                    // Troca de chaves
                    case ProtocolSICmdType.USER_OPTION_2:
                        {
                            string pk = "";
                            try
                            {
                                // Recebe a chave publica do cliente
                                pk = protocolSI.GetStringFromData();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro: " + ex);
                                return;
                            }

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

                            try
                            {
                                // Envia a key
                                byte[] keyPacket = protocolSI.Make(ProtocolSICmdType.USER_OPTION_2, keyEnc);
                                networkStream.Write(keyPacket, 0, keyPacket.Length);

                                // Envia o ACK para o cliente
                                ack = protocolSI.Make(ProtocolSICmdType.ACK);
                                networkStream.Write(ack, 0, ack.Length);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro: " + ex);
                                return;
                            }

                        } break;
                    case ProtocolSICmdType.USER_OPTION_3:
                        {
                            // Recebe os dados do cliente
                            byte[] credenciaisBytes = protocolSI.GetData();

                            byte[] credenciaisDecifradaBytes = new byte[credenciaisBytes.Length];

                            MemoryStream memoryStream = new MemoryStream(credenciaisBytes);

                            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                            int bytesLidos = cryptoStream.Read(credenciaisDecifradaBytes, 0, credenciaisDecifradaBytes.Length);

                            // Guarda as credenciais decifradas
                            string credenciais = Encoding.UTF8.GetString(credenciaisDecifradaBytes, 0, bytesLidos);

                            string username = credenciais.Substring(0, credenciais.IndexOf(" "));
                            string password = credenciais.Substring(credenciais.IndexOf(" ") + 1);

                            Console.WriteLine(username);
                            Console.WriteLine(password);

                            // Verifica se o utilizador existe na base de dados
                            User utilizador = (from User in spksContainer.Users
                                               where User.Username.Equals(username)
                                               select User).FirstOrDefault();

                            int state;

                            // Utilizador nao existe ou nome de utilizador errado
                            if (utilizador == null)
                            {
                                state = 2;
                            }
                            // Password errada
                            else if (utilizador.Password != Common.HashPassword(password, utilizador.Salt))
                            {
                                state = 1;
                            }
                            // Utilizador existe e passowrd está certa
                            else
                            {
                                state = 0;
                            }

                            // Converte a mensagem a enviar para bytes
                            byte[] messageBytes = Encoding.UTF8.GetBytes(state.ToString());

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

                            try
                            {
                                // Envia a mensagem
                                byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_3, msgCifrada);
                                networkStream.Write(packet, 0, packet.Length);

                                // Envia o ACK para o cliente
                                ack = protocolSI.Make(ProtocolSICmdType.ACK);
                                networkStream.Write(ack, 0, ack.Length);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro: " + ex);
                                return;
                            }
                        }
                        break;
                    // Cria conta
                    case ProtocolSICmdType.USER_OPTION_4:
                        {
                            byte[] credenciaisBytes;
                            try
                            {
                                // Recebe os dados do cliente
                                credenciaisBytes = protocolSI.GetData();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro: " + ex);
                                return;
                            }

                            byte[] credenciaisDecifradaBytes = new byte[credenciaisBytes.Length];

                            MemoryStream memoryStream = new MemoryStream(credenciaisBytes);

                            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                            int bytesLidos = cryptoStream.Read(credenciaisDecifradaBytes, 0, credenciaisDecifradaBytes.Length);

                            // Guarda as credenciais decifradas
                            string credenciais = Encoding.UTF8.GetString(credenciaisDecifradaBytes, 0, bytesLidos);

                            string username = credenciais.Substring(0, credenciais.IndexOf(" "));
                            string password = credenciais.Substring(credenciais.IndexOf(" ") + 1);
                            
                            User newUser = new User(username, password);
                            spksContainer.Users.Add(newUser);
                            spksContainer.SaveChanges();

                            Console.WriteLine("Utilizador " + username + " criado");
                        }
                        break;
                    default:
                        break;
                }
            }

            // Fecha as conecoes
            networkStream.Close();
            client.Close();

            Console.WriteLine("O Cliente {0} desconnectou-se", clientId);
        }
    }
}

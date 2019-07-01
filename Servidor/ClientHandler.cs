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
        User user;
        NetworkStream networkStream;
        SPKSContainer spksContainer = new SPKSContainer();
        ProtocolSI protocolSI;
        Room currentRoom;
        State lastState;

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

            protocolSI = new ProtocolSI();

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
                    // Se for uma mensagem do chat
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

                            string msgRecebida = Decifra(msgBytes);

                            string hash = msgRecebida.Substring(0, msgRecebida.IndexOf(" "));
                            msgRecebida = msgRecebida.Substring(msgRecebida.IndexOf(" ") + 1);

                            if (Common.ValidacaoDados(msgRecebida, hash))
                            {
                                string msg = user.Username + ": " + msgRecebida;
                                Console.WriteLine("    Cliente " + msg);

                                // Guarda os dados no ficheiro
                                FileHandler.SaveData(currentRoom.ToString(), msg);

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
                            }
                            else
                            {
                                Console.WriteLine("Hash não é igual");
                            }


                        }
                        break;
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
                    // Envia log do chat
                    case ProtocolSICmdType.USER_OPTION_1:
                        {
                            string log = FileHandler.LoadData(currentRoom.ToString());
                            
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

                                // Cifra a mensagem
                                byte[] msgCifrada = Cifra(stringChunk);
                                
                                Thread.Sleep(100);

                                Send(msgCifrada, ProtocolSICmdType.DATA);
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

                            string hash = pk.Substring(0, pk.IndexOf(" "));
                            pk = pk.Substring(pk.IndexOf(" ") + 1);

                            if (Common.ValidacaoDados(pk, hash))
                            {
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

                                Send(keyEnc, ProtocolSICmdType.USER_OPTION_2);
                            }
                            else
                            {
                                Console.WriteLine("Hash não é igual");
                            }

                        } break;
                    // Login
                    case ProtocolSICmdType.USER_OPTION_3:
                        {
                            // Recebe os dados do cliente
                            byte[] credenciaisBytes = protocolSI.GetData();

                            string credenciais = Decifra(credenciaisBytes);

                            string hash = credenciais.Substring(0, credenciais.IndexOf(" "));
                            credenciais = credenciais.Substring(credenciais.IndexOf(" ") + 1);
                            string username = credenciais.Substring(0, credenciais.IndexOf(" "));
                            string password = credenciais.Substring(credenciais.IndexOf(" ") + 1);

                            if (Common.ValidacaoDados(username + " " + password, hash))
                            {
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
                                    user = utilizador;
                                    state = 0;
                                }
                                

                                byte[] msgCifrada = Cifra(state.ToString());

                                Send(msgCifrada, ProtocolSICmdType.USER_OPTION_3);
                            }
                            else
                            {
                                Console.WriteLine("Hash não é igual");
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

                            string hash = credenciais.Substring(0, credenciais.IndexOf(" "));
                            credenciais = credenciais.Substring(credenciais.IndexOf(" ") + 1);
                            string username = credenciais.Substring(0, credenciais.IndexOf(" "));
                            string password = credenciais.Substring(credenciais.IndexOf(" ") + 1);

                            if (Common.ValidacaoDados(username + " " + password, hash))
                            {
                                User newUser = new User(username, password);
                                spksContainer.Users.Add(newUser);
                                spksContainer.SaveChanges();

                                Console.WriteLine("Utilizador " + username + " criado");
                            }
                            else
                            {
                                Console.WriteLine("Hash não é igual");
                            }
                        }
                        break;
                    // Jogo
                    case ProtocolSICmdType.USER_OPTION_5:
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

                            string jogada = Encoding.UTF8.GetString(msgDecifradaBytes, 0, bytesLidos);
                            string hash = jogada.Substring(0, jogada.IndexOf(" "));
                            jogada = jogada.Substring(jogada.IndexOf(" ") + 1);

                            if (Common.ValidacaoDados(jogada, hash))
                            {
                                // Guarda a mensagem decifrada
                                Console.WriteLine("    Cliente " + user.Username + " jogou: " + jogada);

                                if (currentRoom.GetPlayer1Name() == user.Username)
                                {
                                    currentRoom.Player1Play = jogada;
                                }
                                else
                                {
                                    currentRoom.Player2Play = jogada;
                                }

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

                                while (true)
                                {
                                    if (lastState != currentRoom.GameState)
                                    {
                                        string msg = currentRoom.Player1Pontos.ToString() + " " + currentRoom.Player2Pontos.ToString() + " " + currentRoom.GameState;
                                        Send(Cifra(msg), ProtocolSICmdType.USER_OPTION_5);
                                        lastState = currentRoom.GameState;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Hash não é igual");
                            }
                        } break;
                    // Adiciona o jogador a sala
                    case ProtocolSICmdType.USER_OPTION_6:
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
                            string sala = Encoding.UTF8.GetString(msgDecifradaBytes, 0, bytesLidos);

                            string hash = sala.Substring(0, sala.IndexOf(" "));
                            sala = sala.Substring(sala.IndexOf(" ") + 1);

                            if (Common.ValidacaoDados(sala, hash))
                            {
                                // Verifica existem salas
                                if (Game.rooms.Count == 0)
                                {
                                    CriaSala(sala);
                                }
                                else
                                {
                                    try
                                    {
                                        foreach (Room room in Game.rooms)
                                        {
                                            if (room.ToString() == sala)
                                            {
                                                JuntaSala(room);
                                            }
                                            else if (room == Game.rooms.Last() && room.ToString() == sala)
                                            {
                                                JuntaSala(room);
                                                break;
                                            }
                                            else if (room == Game.rooms.Last())
                                            {
                                                CriaSala(sala);
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Hash não é igual");
                            }
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

        private string Decifra(byte[] msgBytes)
        {
            byte[] msgDecifradaBytes = new byte[msgBytes.Length];

            MemoryStream memoryStream = new MemoryStream(msgBytes);

            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            int bytesLidos = cryptoStream.Read(msgDecifradaBytes, 0, msgDecifradaBytes.Length);

            // Guarda a mensagem decifrada
            return Encoding.UTF8.GetString(msgDecifradaBytes, 0, bytesLidos);
        }

        private void JuntaSala(Room room)
        {
            currentRoom = room;
            room.AddPlayer2(user);
            lastState = currentRoom.GameState;

            // Envia os dados da sala para o jogador
            string dadosSala = room.GetPlayer1Name() + " " + room.GetPlayer2Name() + " " + room.GameState;

            // Cifra a mensagem
            byte[] msgCifrada = Cifra(dadosSala);

            Send(msgCifrada, ProtocolSICmdType.USER_OPTION_7);
        }

        private void CriaSala(string sala)
        {
            // Cria a sala
            currentRoom = new Room(sala, user);
            Game.rooms.Add(currentRoom);

            // Envia os dados da sala para o jogador
            string dadosSala = currentRoom.GetPlayer1Name() + " " + currentRoom.GameState;

            // Cifra a mensagem
            byte[] msgCifrada = Cifra(dadosSala);

            Send(msgCifrada, ProtocolSICmdType.USER_OPTION_6);

            while (true)
            {
                if (currentRoom.GetPlayer2Name() != null)
                {
                    lastState = currentRoom.GameState;

                    // Envia os dados da sala para o jogador
                    dadosSala = currentRoom.GetPlayer1Name() + " " + currentRoom.GetPlayer2Name() + " " + currentRoom.GameState;

                    // Cifra a mensagem
                    byte[] msg = Cifra(dadosSala);

                    Send(msg, ProtocolSICmdType.USER_OPTION_7);

                    break;
                }
            }
        }

        /// <summary>
        /// Cifra as mensagens
        /// </summary>
        /// <param name="mensagem"></param>
        /// <returns></returns>
        private byte[] Cifra(string mensagem)
        {
            // Converte a mensagem a enviar para bytes
            byte[] messageBytes = Encoding.UTF8.GetBytes(Common.GeraHash(mensagem) + " " + mensagem);

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

            return msgCifrada;
        }

        /// <summary>
        /// Envia a mensagem para o cliente
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mensagem"></param>
        private void Send(byte[] mensagem, ProtocolSICmdType type)
        {
            try
            {
                // Envia a mensagem
                byte[] packet = protocolSI.Make(type, mensagem);
                networkStream.Write(packet, 0, packet.Length);

                // Envia o ACK para o cliente
                byte[] ack = protocolSI.Make(ProtocolSICmdType.ACK);
                networkStream.Write(ack, 0, ack.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex);
                return;
            }
        }
    }
}

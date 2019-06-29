using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EI.SI;

namespace Cliente
{
    class ConnectionHandler
    {
        private const int PORT = 5050;

        static NetworkStream networkStream;
        static ProtocolSI protocolSI;
        static TcpClient tcpClient;

        // Guarda os dados da chave simetrica
        AesCryptoServiceProvider aes;

        public static List<string> msgs = new List<string>();

        public void ConnectToServer(string ip)
        {
            try
            {
                tcpClient = new TcpClient();

                tcpClient.Connect(ip, PORT);

                networkStream = tcpClient.GetStream();

                protocolSI = new ProtocolSI();
                
            }
            catch (Exception)
            {
                return;
            }
        }

        public bool Login(string username, string password)
        {
            // Converte a mensagem a enviar para bytes
            byte[] msgBytes = Encoding.UTF8.GetBytes(username + " " + password);

            byte[] msgCifrada;

            // Cifra a mensagem
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(msgBytes, 0, msgBytes.Length);
                }
                // Guarda a mensagem cifrada
                msgCifrada = memoryStream.ToArray();
            }

            try
            {
                // Envia os dados para o servidor
                byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_3, msgCifrada);
                networkStream.Write(packet, 0, packet.Length);

                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                int state = -1;

                // Enquanto nao receber um ACK recebe o que o servidor envia
                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                {
                    
                    if (protocolSI.GetCmdType() == ProtocolSICmdType.USER_OPTION_3)
                    {
                        byte[] receivedData = protocolSI.GetData();

                        // Cria o array para guardar a mensagem decifrada
                        byte[] msgDecifradaBytes = new byte[receivedData.Length];

                        // Decifra a mensagem
                        MemoryStream memoryStream = new MemoryStream(receivedData);
                        CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                        int bytesLidos = cryptoStream.Read(msgDecifradaBytes, 0, msgDecifradaBytes.Length);

                        // Guarda a mensagem decifrada
                        state = int.Parse(Encoding.UTF8.GetString(msgDecifradaBytes, 0, bytesLidos));

                        networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                    }
                }

                switch (state)
                {
                    // Faz login
                    case 0:
                        return true;
                    // Password errada
                    case 1:
                        MessageBox.Show("Password errada");
                        break;
                    // Utilizador nao existe ou nome de utilizador errado
                    case 2:
                        MessageBox.Show("Utilizador nao existe");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public void Send(string message, ProtocolSICmdType type = ProtocolSICmdType.DATA)
        {
            Thread thread = new Thread(() => SendThread(message, type));
            thread.Start();
        }

        private void SendThread(string msg, ProtocolSICmdType type)
        {
            // Converte a mensagem a enviar para bytes
            byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
            
            byte[] msgCifrada;

            // Cifra a mensagem
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(msgBytes, 0, msgBytes.Length);
                }
                // Guarda a mensagem cifrada
                msgCifrada = memoryStream.ToArray();
            }
           
            try
            {
                // Envia os dados para o servidor
                byte[] packet = protocolSI.Make(type, msgCifrada);
                networkStream.Write(packet, 0, packet.Length);

                // Enquanto nao receber um ACK recebe o que o servidor envia
                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                {
                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                }
            }
            catch(Exception)
            {
                return;
            }
            
        }

        public void ExchangeKeys()
        {
            // Guarda as chaves (Assimetrica)
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            // Chave publica
            string publicKey = rsa.ToXmlString(false);

            try
            {
                byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_2, publicKey);
                networkStream.Write(packet, 0, packet.Length);
            }
            catch(Exception)
            {
                return;
            }

            byte[] receivedData = new byte[1024];

            try
            {
                // Enquanto nao receber um ACK recebe o que o servidor envia
                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                {
                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                    if (protocolSI.GetCmdType() == ProtocolSICmdType.USER_OPTION_2)
                    {
                        receivedData = protocolSI.GetData();
                    }
                }
            }
            catch(Exception)
            {
                return;
            }

            // Decifra as keys usando a chave privada
            byte[] decryptedKeys = rsa.Decrypt(receivedData, true);

            string keys = Encoding.UTF8.GetString(decryptedKeys);
            // Subdivide a string recebida e guarda a key e o iv
            string key = keys.Substring(0, keys.IndexOf(" "));
            string iv = keys.Substring(keys.IndexOf(" ") + 1);

            // Guarda as chaves de encriptacao do servidor
            aes = new AesCryptoServiceProvider();
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);
        }

        public void ReceiveData()
        {
            new Thread(ReceiveDataThread).Start();
        }

        private void ReceiveDataThread()
        {
            string msg = "";

            try
            {
                // Envia uma mensagem do tipo USER_OPTION_1
                byte[] opt1 = protocolSI.Make(ProtocolSICmdType.USER_OPTION_1);
                networkStream.Write(opt1, 0, opt1.Length);
            }
            catch(Exception)
            {
                return;
            }

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
                        // Recebe a mensagem do servidor
                        byte[] msgBytes = protocolSI.GetData();

                        // Cria o array para guardar a mensagem decifrada
                        byte[] msgDecifradaBytes = new byte[msgBytes.Length];

                        // Decifra a mensagem
                        MemoryStream memoryStream = new MemoryStream(msgBytes);
                        CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                        int bytesLidos = cryptoStream.Read(msgDecifradaBytes, 0, msgDecifradaBytes.Length);

                        // Guarda a mensagem decifrada
                        msg = msg + Encoding.UTF8.GetString(msgDecifradaBytes, 0, bytesLidos);
                    }
                }
                catch (Exception)
                {
                    return;
                }

            }

            Console.WriteLine("Mensagem Recebida: " + msg);

            // Limpa as mensagem
            msgs.Clear();

            // Tamanho da resposta
            int stringLenght = msg.Length;

            int chunksize = msg.IndexOf(";");

            string stringChunk = "";

            for (int i = 0; i < msg.Length; i += chunksize)
            {
                chunksize = msg.IndexOf(";", chunksize + 1);

                if (chunksize > stringLenght)
                {
                    stringChunk = msg.Substring(i);
                }
                else
                {
                    stringLenght -= chunksize;
                    stringChunk = msg.Substring(i, chunksize);
                }

                Console.WriteLine("Mensagem: " + stringChunk + 1);

                // Adiciona a mensagem recebida à caixa de mensagens
                msgs.Add(stringChunk);
            }
            
            Window.timerEnable = false;
        }

        public void CloseConnection()
        {
            try
            {
                byte[] eot = protocolSI.Make(ProtocolSICmdType.EOT);
                networkStream.Write(eot, 0, eot.Length);

                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                networkStream.Close();
                tcpClient.Close();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}

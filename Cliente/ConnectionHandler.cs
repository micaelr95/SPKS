﻿using System;
using System.Collections.Generic;
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

        //
        AesCryptoServiceProvider aes;

        public static List<string> msgs = new List<string>();

        public void ConnectToServer(string ip)
        {
            tcpClient = new TcpClient();

            tcpClient.Connect(ip, PORT);

            networkStream = tcpClient.GetStream();

            protocolSI = new ProtocolSI();

            msgs.Add("Conectado ao servidor");
        }

        public void Send(string message, ProtocolSICmdType type = ProtocolSICmdType.DATA)
        {
            Thread thread = new Thread(() => SendThread(message, type));
            thread.Start();
        }

        private void SendThread(string msg, ProtocolSICmdType type)
        {
            byte[] packet = protocolSI.Make(type, msg);
            networkStream.Write(packet, 0, packet.Length);

            // Enquanto nao receber um ACK recebe o que o servidor envia
            while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
            {
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            }

            ReceiveDataThread();
        }

        public void ExchangeKeys()
        {
            // Guarda as chaves (Assimetrica)
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            // Chave publica
            string publicKey = rsa.ToXmlString(false);

            byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_2, publicKey);
            networkStream.Write(packet, 0, packet.Length);

            byte[] receivedData = new byte[1024];

            // Enquanto nao receber um ACK recebe o que o servidor envia
            while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
            {
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                if (protocolSI.GetCmdType() == ProtocolSICmdType.USER_OPTION_2)
                {
                    receivedData = protocolSI.GetData();
                }
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
            Console.WriteLine("Output consola: " + textAux);
            msgs.Add(textAux);

            Window.timerEnable = false ;
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

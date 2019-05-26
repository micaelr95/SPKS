using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using EI.SI;

namespace Servidor
{
    class Program
    {
        private const int PORT = 5050;

        static void Main(string[] args)
        {
            // Total de clientes ligados
            int clientCounter = 0;

            // Ouve por licações tcp
            TcpListener tcpListener = new TcpListener(IPAddress.Any, PORT);
            tcpListener.Start();

            Console.WriteLine("Servidor Iniciado...");

            while (true)
            {
                // Recebe ligacao do cliente
                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                // Incrementa o contador de clientes
                clientCounter++;

                Console.WriteLine("Clientes Ligados: " + clientCounter.ToString());

                // Instancia uma nova thread para o cliente
                ClientHandler clientHandler = new ClientHandler(tcpClient, clientCounter);

                clientHandler.Handle();
            }
        }
    }
}

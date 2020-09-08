using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using KashkeshetClient;

namespace KashkeshetSenser
{
    class Sender
    {
        public List<Client> Clients { get; set; }

        public Sender(Dictionary<int, Client> clients)
        {
            Clients = new List<Client>();
        }

        public void AddANewClient(Client client)
        {
            Clients.Add(client);
        }

        public void Broadcast()
        {
            Console.Write("Enter text: ");
            string textToSend = Console.ReadLine();

            TcpClient client = new TcpClient("10.1.0.20", 11000);
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

            //---send the text---
            Console.WriteLine("Sending : " + textToSend);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            //---read back the text---
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            Console.ReadLine();
            client.Close();
        }
    }
}

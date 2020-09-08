
﻿using System;
using System.Collections.Generic;
using System.Text;
﻿using KashkeshetClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace KashkeshetServer
{
    class Server
    {

        static readonly object _lock = new object();
        static readonly Dictionary<int, TcpClient> TcpClients = new Dictionary<int, TcpClient>();
        static readonly Dictionary<int, UserData> ClientsDetail = new Dictionary<int, UserData>();
        public TcpClient client { get; set; }
        public int count { get; set; }

        public UserData userDataRecived { get; set; }


        public Server()
        {
            count = 1;
        }

        public void StrartServer()
        {
            

            TcpListener ServerSocket = new TcpListener(IPAddress.Any, 11000);
            ServerSocket.Start();

            while (true)
            {
                client = ServerSocket.AcceptTcpClient();
                lock (_lock) TcpClients.Add(count, client);
                Console.WriteLine("Someone connected!!");

                Thread t = new Thread(handle_clients);
                t.Start(count);
                count++;
            }

        }


        public void reciveObject(TcpClient client)
        {

            NetworkStream strm = client.GetStream();
            IFormatter formatter = new BinaryFormatter();
            userDataRecived = (UserData)formatter.Deserialize(strm);
            strm.Close();
            lock (_lock) ClientsDetail.Add(count, userDataRecived);

        }


        public void handle_clients(object o)
        {
            int id = (int)o;
            TcpClient client;

            lock (_lock) client = TcpClients[id];

            while (true)
            {
                NetworkStream stream = client.GetStream();
                if (!TcpClients.ContainsKey(count))
                {
                    IFormatter formatter = new BinaryFormatter();
                    UserData userDataRecived = (UserData)formatter.Deserialize(stream);
                    lock (_lock) ClientsDetail.Add(count, userDataRecived);


                }
                byte[] buffer = new byte[1024];
                int byte_count = stream.Read(buffer, 0, buffer.Length);
                stream.Close();
                if (byte_count == 0)
                {
                    break;
                }

                string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
                broadcast(data, client);
                Console.WriteLine(data);

                
            }
            broadcast(ClientsDetail[id].Name + " Leave the chat", client);
            lock (_lock) TcpClients.Remove(id);
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
            
        }

        public void ChooseClientForPrivateChat()
        {

            foreach (var client in ClientsDetail)
            {
                Console.WriteLine("{key}.{value}",client.Key,client.Value.Name);
            }
        }



        public void broadcast(string data, TcpClient client)

        {
            byte[] buffer = Encoding.ASCII.GetBytes(data + Environment.NewLine);

            lock (_lock)
            {
                foreach (TcpClient c in TcpClients.Values)
                {
                    if (client != c)
                    {
                        NetworkStream stream = c.GetStream();

                        stream.Write(buffer, 0, buffer.Length);

                        stream.Close();
                    }

                }
            }
        }


    }
}

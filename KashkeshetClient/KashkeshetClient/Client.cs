using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KashkeshetClient
{
    public class Client
    {
        public UserData userData { get; set; }
        public Client(UserData userData1)
        {
            userData = userData1 ;
        }

        public TcpClient StartSession()
        {
            Console.Write("Please enter user name: ");
            userData.Name = Console.ReadLine();
            int port = 11000;
            TcpClient client = new TcpClient("10.1.0.20",port);
            SendObject(userData, client);
            Console.WriteLine("Connected To Server, For Exit Please Press Enter");
            return client;
            
           

        }
        public void SendObject(UserData userData, TcpClient client)
        {
            IFormatter formatter = new BinaryFormatter();
            NetworkStream strm = client.GetStream();
            formatter.Serialize(strm, userData);
        }

        public void SendData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            Thread thread = new Thread(o => ReceiveData((TcpClient)o));

            thread.Start(client);

            string Input;
            while (!string.IsNullOrEmpty(Input = UserInput()))
            {
                byte[] buffer = Encoding.ASCII.GetBytes(userData.Name + ": " + Input);
                ns.Write(buffer, 0, buffer.Length);
            }

            client.Client.Shutdown(SocketShutdown.Send);
            thread.Join();
            ns.Close();
            client.Close();
            Console.WriteLine(userData.Name + " disconnect from chat");
            Console.ReadKey();
        }

        static string UserInput()
        {
            string userInput;
            Console.Write("Enter Message: ");
            userInput = Console.ReadLine();
            return userInput;

        }
        static void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[1024];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                Console.Write("\n"+Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
            }
        }


    }
}

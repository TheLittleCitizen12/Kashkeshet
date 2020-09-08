using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KashkeshetClient
{
    public class Client
    {
        public string UserName { get; set; }

        public void StartSession()
        {
            Console.Write("Please enter user name: ");
            UserName = Console.ReadLine();
            IPAddress ip = IPAddress.Parse("10.1.0.20");
            int port = 11000;
            TcpClient client = new TcpClient();
            client.Connect(ip, port);
            Console.WriteLine("Connected To Server, For Exit Please Press Enter");
            NetworkStream ns = client.GetStream();
            Thread thread = new Thread(o => ReceiveData((TcpClient)o));

            thread.Start(client);

            string Input;
            while (!string.IsNullOrEmpty(Input = UserInput()))
            {
                byte[] buffer = Encoding.ASCII.GetBytes(UserName+": "+ Input);
                ns.Write(buffer, 0, buffer.Length);
            }

            client.Client.Shutdown(SocketShutdown.Send);
            thread.Join();
            ns.Close();
            client.Close();
            Console.WriteLine("disconnect from server!!");
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
                Console.Write(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
            }
        }


    }
}

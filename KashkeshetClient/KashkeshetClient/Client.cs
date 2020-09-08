using System;
using System.Collections.Generic;
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
        public Client()
        {
            userData = new UserData();
        }

        public void StartSession()
        {
            Console.Write("Please enter user name: ");
            userData.Name = Console.ReadLine();
            IPAddress ip = IPAddress.Parse("10.1.0.20");
            int port = 11000;
            TcpClient client = new TcpClient();
            client.Connect(ip, port);
            Console.WriteLine("Connected To Server, For Exit Please Press Enter");

            NetworkStream ns = client.GetStream();
            Thread thread = new Thread(o => ReceiveData((TcpClient)o));

            thread.Start(client);

            IFormatter formatter = new BinaryFormatter();
            NetworkStream strm = client.GetStream();
            formatter.Serialize(strm, userData);
            strm.Close();

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
                Console.Write(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
            }
        }


    }
}

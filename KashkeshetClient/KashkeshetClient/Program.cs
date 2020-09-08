using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KashkeshetClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            client.StartSession();
        }
        
    }
    
}

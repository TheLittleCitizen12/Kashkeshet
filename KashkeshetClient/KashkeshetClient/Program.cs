using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KashkeshetClient
{
    class Program
    {
        static Dictionary<int, UserData> clientsConnected = new Dictionary<int, UserData>();
        
        static void Main(string[] args)
        {
            UserData userData = new UserData();
            Client client = new Client();
            Menu menu = new Menu(client,userData,clientsConnected);
            menu.MainMenu();
            
        }
        
    }
    
}

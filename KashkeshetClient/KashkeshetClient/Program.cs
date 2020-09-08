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
            UserData userData = new UserData();
            Client client = new Client(userData);
            Menu menu = new Menu(client,userData);
            menu.MainMenu();
            
        }
        
    }
    
}

using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetClient
{
    class Menu
    {
        public Client client { get; set; }
        public UserData userData { get; set; }

        public Menu(Client client1, UserData userData1)
        {
            client = client1;
            userData = userData1;
        }

        public void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("1. Broadcast Chat.");
                Console.WriteLine("2. Private Chat.");
                Console.WriteLine("3. Exit");
                int choice = UserInput();

                switch (choice)
                {
                    case 1:
                        userData.Input = 1;
                        client.SendData(client.StartSession());
                        break;
                    case 2:
                        userData.Input = 2;
                        client.StartSession();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("You don't have this option");
                        MainMenu();
                        break;


                }
            }

        }

        static int UserInput()
        {
            int result;
            bool parsedSuccessfully = int.TryParse(Console.ReadLine(), out result);
            while (!parsedSuccessfully)
            {
                Console.WriteLine("Numbers Only");
                parsedSuccessfully = int.TryParse(Console.ReadLine(), out result);
            }
            return result;
        }
    }
}

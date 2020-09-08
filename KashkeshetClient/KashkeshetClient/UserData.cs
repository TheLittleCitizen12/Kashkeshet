using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshetClient
{
    [Serializable]
    public class UserData
    {
        public string Name { get; set; }
        public int Input { get; set; }
        
    }
}

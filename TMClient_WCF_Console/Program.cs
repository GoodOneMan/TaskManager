using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient_WCF_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            Console.WriteLine("Client connect ...");

            Console.ReadKey();

            client.Disconnect();
            Console.WriteLine("Client disconnect ...");

            Console.ReadKey();
        }
    }
}

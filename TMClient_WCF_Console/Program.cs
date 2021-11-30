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

            client.Init();
            Console.WriteLine("Client init ...");

            bool flag = client.Connect();
            Console.WriteLine("Client connect ...");

            if (flag)
            {
                Console.WriteLine("Client some work ...");
                client.SomeWork();


                Console.ReadKey();

                client.Disconnect();
                Console.WriteLine("Client disconnect ...");
            }
            

            Console.ReadKey();
        }
    }
}

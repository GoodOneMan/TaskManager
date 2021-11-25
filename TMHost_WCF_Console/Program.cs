using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TMHost_WCF_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new Host();

            host.StartHost();

            Console.WriteLine("Host start ...");

            Console.ReadKey();

            host.StopHost();
        }
    }
}

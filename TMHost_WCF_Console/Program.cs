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

            host.InitHost();
            Console.WriteLine("Host init ...");
            //Console.ReadKey();

            host.StartHost();
            Console.WriteLine("Host start ...");
            Console.ReadKey();

            host.StopHost();
            Console.WriteLine("Host stop ...");
            Console.ReadKey();
        }
    }
}

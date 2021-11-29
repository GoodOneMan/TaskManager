using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace TaskManagerWCF_Host_console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(TaskManagerWCF_Lib.TMService)))
            {
                host.Open();
                Console.WriteLine("Хост стартовал!");
                Console.ReadLine();
            }
        }
    }
}

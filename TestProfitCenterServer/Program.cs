using System;
using System.Net;
using TestProfitCenterServer;

namespace TestProfitCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 0;
            IPAddress mcastAddress;
            Console.WriteLine("Enter mcast IP(224.168.100.2)");
            count = 0;
            while (!IPAddress.TryParse(Console.ReadLine(), out mcastAddress))
            {
                Console.WriteLine("Incorect format");
                count++;
                if (count > 3)
                {
                    IPAddress.TryParse("224.168.100.2", out mcastAddress);
                    Console.WriteLine("Usint mcast ip 224.168.100.2");
                    break;
                }
            }
            FinServer server = new FinServer(mcastAddress);
            server.StartServer();
            Console.WriteLine("Press eny key for stop...");
            Console.ReadKey();
            server.StopServer();
        }
    }
}

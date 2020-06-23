using System;
using System.Net;
using TestProfitCenterServer;

namespace TestProfitCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter server IP(0.0.0.0)");
            IPAddress localIPAddr;
            int count = 0;
            while (!IPAddress.TryParse(Console.ReadLine(), out localIPAddr))
            {
                Console.WriteLine("Incorect format");
                count++;
                if (count > 3)
                {
                    IPAddress.TryParse("0.0.0.0", out localIPAddr);
                    Console.WriteLine("Usint server ip 0.0.0.0");
                    break;
                }
            }
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
            FinServer server = new FinServer(localIPAddr, mcastAddress);
            server.StartServer();
            Console.WriteLine("Press eny key for stop...");
            Console.ReadKey();
            server.StopServer();
        }
    }
}

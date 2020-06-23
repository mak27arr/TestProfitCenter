using System;
using System.Net;

namespace TestProfitCenterClient
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
            FinClient client = new FinClient(localIPAddr, mcastAddress);
            client.Start();
            ConsoleKeyInfo key = new ConsoleKeyInfo();

            while (key.Key != ConsoleKey.Escape)
            {
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                    Console.WriteLine(client.GetStatistic());
            }
            client.Stop();
        }
    }
}

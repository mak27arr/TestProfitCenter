using System;
using System.Net;

namespace TestProfitCenterClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SettingsLoad settingsLoad = SettingsLoad.getInstance();
            FinClient client = new FinClient(IPAddress.Parse(settingsLoad.setting.localIPAddr), IPAddress.Parse(settingsLoad.setting.mcastAddress), settingsLoad.setting.mcrtPort);
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

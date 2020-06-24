using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TestProfitCenterClient
{
    class FinClient
    {
        static int _mcastPort = 3000;
        static Socket mcastSocket;
        private IPAddress _addres;
        private IPAddress _mcastAddress;

        private object locker = new object();
        private bool rec_stop = false;
        private Thread thread;
        StatisticMsg statisticMsg = new StatisticMsg();

        public FinClient(IPAddress address, IPAddress mcastAddress, int mcastPort = 3000)
        {
            _addres = address;
            _mcastAddress = mcastAddress;
            _mcastPort = mcastPort;
        }
        public void Start()
        {
            Init();
            thread = new Thread(ClientThread);
            thread.Start();
        }
        public void Stop()
        {
            lock (locker)
            {
                rec_stop = true;
            }
        }
        public string GetStatistic()
        {
            string rez;
            lock (locker)
            {
                rez = statisticMsg.ToString();
            }
            return rez;
        }
        private void ClientThread()
        {
            while (!rec_stop)
                ReceiveBroadcastMessages();
            lock (locker)
            {
                rec_stop = false;
            }
        }
        private void ReceiveBroadcastMessages()
        {
            
            while (!rec_stop)
            {
                try
                {
                    byte[] bytes = new Byte[8096];
                    mcastSocket.Receive(bytes);
                    var msg_str = Encoding.UTF32.GetString(bytes, 0, bytes.Length);
                    if (msg_str != string.Empty)
                    {
                        var msg = Newtonsoft.Json.JsonConvert.DeserializeObject<Msg>(msg_str);
                        ProcesMsg(msg);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Init();
                }
            }
            mcastSocket.Close();    
        }
        private void Init()
        {
            try
            {
                mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, _mcastPort);
                mcastSocket.Bind(ipep);
                mcastSocket.SetSocketOption(SocketOptionLevel.IP,SocketOptionName.AddMembership,
                                            new MulticastOption(_mcastAddress, IPAddress.Any));
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.ToString());
            }
        }
        private void ProcesMsg(Msg msg)
        {
            lock (locker)
            {
                statisticMsg.AddMsg(msg);
            }
        }
    }
}

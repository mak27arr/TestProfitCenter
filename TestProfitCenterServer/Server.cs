﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TestProfitCenterServer
{
    class FinServer
    {
        static int _mcastPort = 11000;
        static Socket mcastSocket;
        private IPAddress _mcastAddress;

        private object locker = new object();
        private bool rec_stop = false;
        private Thread thread;
        /// <summary>
        /// Init server
        /// </summary>
        /// <param name="address">Server ip adress like localhost or 0.0.0.0</param>
        public FinServer(IPAddress mcastAddress,int mcastPort=11000)
        {
            _mcastAddress = mcastAddress;
            _mcastPort = mcastPort;
        }
        public void StartServer()
        {
            InitServer();
            thread = new Thread(ServerThread);
            thread.Start();
        }
        private void ServerThread()
        {
            MessageGenerator messageGenerator = new MessageGenerator();
            while (!rec_stop)
            {     
                var msg = messageGenerator.GetNextMSG();
                while (!BroadcastMessage(msg))
                {
                    Thread.Sleep(1000);
                }
                Thread.Sleep(10);
            }
            try
            {
                mcastSocket.Close();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            lock (locker)
            {
                rec_stop = false;
            }
        }
        public void StopServer()
        {
            lock (locker)
            {
                rec_stop = true;
            }
        }
        private void InitServer()
        {
            try
            {
                // Create a multicast socket.
                mcastSocket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Dgram,
                                         ProtocolType.Udp);
                mcastSocket.SetSocketOption(SocketOptionLevel.IP,
                    SocketOptionName.AddMembership, new MulticastOption(_mcastAddress));
                mcastSocket.SetSocketOption(SocketOptionLevel.IP,
                    SocketOptionName.MulticastTimeToLive, 3);
                IPEndPoint IPlocal_m = new IPEndPoint(_mcastAddress, _mcastPort);
                mcastSocket.Connect(IPlocal_m);
            }
            catch(Exception ex)
            {
                Console.WriteLine("\n" + ex.ToString());
            }
        }
        private bool BroadcastMessage(string message)
        {
            IPEndPoint endPoint;
            try
            {
                var byte_size = ASCIIEncoding.UTF32.GetBytes(message);
                mcastSocket.Send(byte_size);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
                return false;
            }
            return true;
        }
    }
}

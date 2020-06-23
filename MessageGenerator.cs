using System;
using System.Collections.Generic;
using System.Text;

namespace TestProfitCenterServer
{
    class MessageGenerator
    {
        private static long curent_id = 0;
        private static Random generator = new Random();
        public string GetNextMSG()
        {
            Msg msg = new Msg();
            msg.msg_id = curent_id;
            curent_id++;
            msg.value = generator.Next(0, 10000);
            return Newtonsoft.Json.JsonConvert.SerializeObject(msg);
        }
    }
    class Msg
    {
        public long msg_id;
        public int value;
    }
}

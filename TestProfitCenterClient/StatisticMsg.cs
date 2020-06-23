using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace TestProfitCenterClient
{
    class StatisticMsg
    {
        private long last_id = -1;
        private long msg_count = 0;
        private long lost_count = 0;
        private int avrValue=0;
        private double sDeviation=0;
        private Dictionary<int, int> mode_dic = new Dictionary<int, int>();
        private int mode = 0;
        private int mediant = 0;
        public void AddMsg(Msg msg)
        {
            if (last_id != -1)
            {
                if (msg.msg_id - 1 != last_id)
                    lost_count++;
            }
            last_id = msg.msg_id;

            if (msg_count == 0)
            {
                avrValue = msg.value;
                mode = msg.value;
            }
            else
            {
                avrValue = (int)((((long)avrValue * msg_count) + (long)msg.value) / (msg_count + 1));
                sDeviation = Math.Sqrt((((long)Math.Pow(sDeviation,2) * msg_count)+Math.Pow(msg.value- avrValue,2))/msg_count+1);
                if (mode_dic.ContainsKey(msg.value))
                {
                    mode_dic[msg.value] = mode_dic[msg.value] + 1;
                }
                else
                {
                    mode_dic.Add(msg.value, 1);
                }
                mode = mode_dic.FirstOrDefault(x=> x.Value == mode_dic.Values.Max()).Value;
                mediant = mode_dic.OrderBy(x => x.Value).ToList()[(int)mode_dic.Count/2].Value;
            }

            msg_count++;
        }

        public override string ToString() 
        {
            return "Medium: " + avrValue + Environment.NewLine +
                "Standard deviation: " + sDeviation + Environment.NewLine +
                "Mode: " + mode + Environment.NewLine +
                "Median: " + mediant + Environment.NewLine +
                "Lost pkg: " + lost_count + Environment.NewLine;
        }
    }
}

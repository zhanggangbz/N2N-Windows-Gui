using CommonTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServerGui
{
    public delegate void IpListChangeDelegate(Dictionary<String, DateTime> curIpList);
    class LanIpManager
    {
        #region 类的单例实现
        private static readonly Lazy<LanIpManager> instance = new Lazy<LanIpManager>(() => new LanIpManager());
        public static LanIpManager Instance
        {
            get
            {
                return instance.Value;
            }
        }
        #endregion

        private static readonly object obj = new object();

        private string ip_format;

        private Dictionary<String, DateTime> AllUsedIps;

        private Dictionary<int, bool> ipPool;

        private System.Timers.Timer guardTimer;

        public event IpListChangeDelegate IpListChangeEvent;

        LanIpManager()
        {
            AllUsedIps = new Dictionary<string, DateTime>();
            ip_format = "192.168.88.{0}";
            ipPool = new Dictionary<int, bool>();

            for (int i = 3; i < 200; ++i)
            {
                ipPool.Add(i, false);
            }

            //初始化并启动定时器
            guardTimer = new System.Timers.Timer();
            guardTimer.Enabled = true;
            guardTimer.Interval = 120000;
            guardTimer.Elapsed += guardTimer_Elapsed;
            guardTimer.Start();
        }

        private void guardTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //定时清理客户端在线列表
            lock (obj)
            {
                DateTime curTime = DateTime.Now;

                List<string> predele = new List<string>();

                foreach (var item in AllUsedIps)
                {
                    if ((curTime - item.Value).TotalSeconds >= 110)
                    {
                        predele.Add(item.Key);
                    }
                }

                foreach (var item in predele)
                {
                    AllUsedIps.Remove(item);

                    int index = int.Parse(item.Substring(item.LastIndexOf('.')+1));
                    ipPool[index] = false;
                    LogHelper.Instance.WriteInfo("清理并释放IP：{0}", item);
                }

                if (IpListChangeEvent != null)
                {
                    IpListChangeEvent.Invoke(AllUsedIps);
                }
            }
        }

        public string GetIp()
        {
            lock (obj)
            {
                var ip = ipPool.First(x => !x.Value);
                ipPool[ip.Key] = true;

                string iplan = string.Format(ip_format, ip.Key);
                AllUsedIps.Add(iplan,DateTime.Now);
                if (IpListChangeEvent != null)
                {
                    IpListChangeEvent.Invoke(AllUsedIps);
                }
                LogHelper.Instance.WriteInfo("获取IP：{0}", iplan);

                return iplan;
            }
        }

        public void UpdateIp(string ip)
        {
            lock (obj)
            {
                if (AllUsedIps.ContainsKey(ip))
                {
                    AllUsedIps[ip] = DateTime.Now;
                    LogHelper.Instance.WriteInfo("更新IP：{0}", ip);
                }
            }
        }

        public void ReleaseIp(string ip)
        {
            lock (obj)
            {
                if (AllUsedIps.ContainsKey(ip))
                {
                    AllUsedIps.Remove(ip);
                    if (IpListChangeEvent != null)
                    {
                        IpListChangeEvent.Invoke(AllUsedIps);
                    }
                    LogHelper.Instance.WriteInfo("释放IP：{0}", ip);
                }
                int index = int.Parse(ip.Substring(ip.LastIndexOf('.')));
                ipPool[index] = false;
            }
        }
    }
}

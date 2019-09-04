using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGui
{
    class EdgeParameter
    {
        /// <summary>
        /// 服务端信息（IP:Port）
        /// </summary>
        public String Server { get; set; }

        /// <summary>
        /// 局域网IP
        /// </summary>
        public String LanIP { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public String Group { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public String Key { get; set; }

        public String GetCommand()
        {
            return string.Format("-a {0} -c {1} -k {2} -l {3}", LanIP, Group, Key, Server);
        }
    }
}

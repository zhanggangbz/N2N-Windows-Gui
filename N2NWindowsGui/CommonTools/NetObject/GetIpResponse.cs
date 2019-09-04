using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools.NetObject
{
    [Serializable]
    public class GetIpResponse
    {
        public N2nConfig N2NConfig { get; set; }

        public String LanIp { get; set; }
    }
}

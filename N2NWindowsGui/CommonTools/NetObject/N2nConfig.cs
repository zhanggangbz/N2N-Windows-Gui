using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools.NetObject
{
    [Serializable]
    public class N2nConfig
    {
        public string Server { get; set; }

        public string Group { get; set; }

        public string Key { get; set; }

        public int KeepInterval { get; set; }
    }
}

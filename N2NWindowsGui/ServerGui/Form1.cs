using Sodao.FastSocket.Server;
using Sodao.FastSocket.Server.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SocketServerManager.Init();
            SocketServerManager.Start();
            this.button1.Enabled = false;
            this.button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SocketServerManager.Stop();
            this.button2.Enabled = false;
            this.button1.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LanIpManager.Instance.IpListChangeEvent += Instance_IpListChangeEvent;
        }

        private void Instance_IpListChangeEvent(Dictionary<string, DateTime> curIpList)
        {
            this.dataGridView1.Invoke(new Action(()=> {
                this.dataGridView1.DataSource = (from v in curIpList
                                                 select new
                                                 {
                                                     IP = v.Key,
                                                     更新时间 = v.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                                 }).ToArray();
            }));
        }
    }
}

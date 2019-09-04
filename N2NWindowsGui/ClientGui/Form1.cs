using ClientGui.API;
using CommonTools;
using CommonTools.NetObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientGui
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 服务端基础信息
        /// </summary>
        BaseServerAPI baseServer = new BaseServerAPI("127.0.0.1", 8401);

        /// <summary>
        /// N2n客户端进程变量
        /// </summary>
        Process edgeProcess;

        EdgeParameter edgeParameter = new EdgeParameter();

        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "TAP");

            string command = Path.Combine(path, "tapinstall");

            string arga = "install \"OemVista.inf\" tap0901";

            string reStr = "Drivers installed successfully";

            string msg = "安装失败";

            if (CommonTools.ExeHelper.RunCmdCommand(command, arga, path, reStr))
            {
                msg = "安装成功";
            }
            MessageBox.Show(msg);
        }

        /// <summary>
        /// 执行Edge客户端程序
        /// </summary>
        /// <param name="parameter">执行参数</param>
        void RunEdge(EdgeParameter parameter)
        {
            string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "N2N");

            string command = Path.Combine(path, "edge.exe");

            //删除当前正在运行的进程
            KillProcessByName("edge.exe", command);

            string arga = parameter.GetCommand();

            MessageBox.Show("开始执行N2N客户端");

            Task.Run(() =>
            {
                try
                {
                    edgeProcess = CommonTools.ExeHelper.CreateCmdProcess(command, arga, path);
                    edgeProcess.StartInfo.RedirectStandardOutput = true;
                    edgeProcess.StartInfo.RedirectStandardError = true;

                    edgeProcess.OutputDataReceived += (sender1, e1) =>
                    {
                        if (!string.IsNullOrEmpty(e1.Data))
                        {
                            LogHelper.Instance.WriteInfo(e1.Data);
                        }
                    };

                    edgeProcess.ErrorDataReceived += (sender1, e1) =>
                    {
                        if (!string.IsNullOrEmpty(e1.Data))
                        {
                            LogHelper.Instance.WriteError(e1.Data);
                        }
                    };

                    edgeProcess.Start();
                    edgeProcess.BeginOutputReadLine();
                    edgeProcess.BeginErrorReadLine();
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.WriteError(ex);
                }
            });
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(edgeProcess!=null && !edgeProcess.HasExited)
            {
                edgeProcess.Kill();

                baseServer.ReleaseIp(edgeParameter.LanIP);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var data = baseServer.GetIp();

                if (data != null)
                {
                    edgeParameter.Server = this.textBox1.Text = data.N2NConfig.Server;
                    edgeParameter.LanIP = this.textBox2.Text = data.LanIp;
                    edgeParameter.Group = this.textBox3.Text = data.N2NConfig.Group;
                    edgeParameter.Key = this.textBox4.Text = data.N2NConfig.Key;

                    RunEdge(edgeParameter);

                    timer1.Start();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void KillProcessByName(string name, string filePath)
        {
            var allprocess = System.Diagnostics.Process.GetProcessesByName(name);

            foreach (System.Diagnostics.Process p in allprocess)
            {
                if (filePath == p.MainModule.FileName)
                {
                    p.Kill();
                    p.Close();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                baseServer.KeepIp(edgeParameter.LanIP);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.WriteError(ex);
            }
        }
    }
}

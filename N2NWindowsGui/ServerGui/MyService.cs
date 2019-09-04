using CommonTools;
using CommonTools.NetObject;
using Sodao.FastSocket.Server;
using Sodao.FastSocket.Server.Command;
using Sodao.FastSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerGui
{
    internal class MyService : CommandSocketService<AsyncBinaryCommandInfo>
    {
        protected override void HandleUnKnowCommand(IConnection connection, AsyncBinaryCommandInfo commandInfo)
        {
            LogHelper.Instance.WriteWain("UnKnowCommand:{0}", commandInfo.CmdName);
            commandInfo.Reply(connection, null);
        }

        public override void OnReceived(IConnection connection, AsyncBinaryCommandInfo cmdInfo)
        {
            switch (cmdInfo.CmdName)
            {
                case BaseAction.GetIp:
                    var n2NConfig = GetIp();
                    cmdInfo.Reply(connection, BinarySeria.GetBytes(n2NConfig));
                    break;
                case BaseAction.ReleaseIp:
                    var ripResult = ReleaseIp(cmdInfo.Buffer);
                    cmdInfo.Reply(connection, BinarySeria.GetBytes(ripResult));
                    break;
                case BaseAction.KeepIp:
                    var kipResult = KeepIp(cmdInfo.Buffer);
                    cmdInfo.Reply(connection, BinarySeria.GetBytes(kipResult));
                    break;
                default:
                    base.OnReceived(connection, cmdInfo);
                    break;
            }
        }

        private GetIpResponse GetIp()
        {
            GetIpResponse ipRequest = new GetIpResponse();

            ipRequest.N2NConfig = new CommonTools.NetObject.N2nConfig()
            {
                Server = "xxx.xxx.xxx.xxx:5678",
                Group = "aaaa",
                Key = "aaaa",
                KeepInterval = 60000
            };

            ipRequest.LanIp = LanIpManager.Instance.GetIp();

            return ipRequest;
        }

        private UpdateIpResponse KeepIp(Byte[] data)
        {
            try
            {
                var ip = BinarySeria.GetObject(data) as UpdateIpRequest;

                LanIpManager.Instance.UpdateIp(ip.LanIp);

                return new UpdateIpResponse() { Result = true };
            }
            catch(Exception ex)
            {
                LogHelper.Instance.WriteError(ex);
                return new UpdateIpResponse() { Result = false };
            }
        }

        private UpdateIpResponse ReleaseIp(Byte[] data)
        {
            try
            {
                var ip = BinarySeria.GetObject(data) as UpdateIpRequest;

                LanIpManager.Instance.ReleaseIp(ip.LanIp);

                return new UpdateIpResponse() { Result = true };
            }
            catch (Exception ex)
            {
                LogHelper.Instance.WriteError(ex);
                return new UpdateIpResponse() { Result = false };
            }
        }
    }
}

using CommonTools;
using CommonTools.NetObject;
using Sodao.FastSocket.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGui.API
{
    class BaseServerAPI
    {
        string ServerIp { get; set; }

        int ServerPort { get; set; }

        AsyncBinarySocketClient Client;

        string ClientRegName = "test";
        public BaseServerAPI(string ip,int port)
        {
            Client = new AsyncBinarySocketClient(8192, 8192, 3000, 3000);

            ServerIp = ip;

            ServerPort = port;

            Client.RegisterServerNode(ClientRegName, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ServerIp), ServerPort));
        }

        public void ReSetServerInfo(string ip, int port)
        {
            ServerIp = ip;

            ServerPort = port;

            Client.UnRegisterServerNode(ClientRegName);

            Client.RegisterServerNode(ClientRegName, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ServerIp), ServerPort));
        }

        public GetIpResponse GetIp()
        {
            try
            {
                var data = Client.Send(BaseAction.GetIp, null, (res) =>
                {
                    return BinarySeria.GetObject(res.Buffer) as CommonTools.NetObject.GetIpResponse;
                }).Result;

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("请求服务时发生错误!", ex);
            }
        }

        public void KeepIp(string ip)
        {
            try
            {
                UpdateIpRequest request = new UpdateIpRequest() { LanIp = ip };

                Client.Send(BaseAction.KeepIp, BinarySeria.GetBytes(request), (res) =>
                {
                    return BinarySeria.GetObject(res.Buffer) as CommonTools.NetObject.GetIpResponse;
                }).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("请求服务时发生错误!", ex);
            }
        }

        public void ReleaseIp(string ip)
        {
            try
            {
                UpdateIpRequest request = new UpdateIpRequest() { LanIp = ip };

                Client.Send(BaseAction.ReleaseIp, BinarySeria.GetBytes(request), (res) =>
                {
                    return BinarySeria.GetObject(res.Buffer) as CommonTools.NetObject.GetIpResponse;
                }).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("请求服务时发生错误!", ex);
            }
        }
    }
}

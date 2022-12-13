using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace UdpChatExample
{
    /// <summary>
    /// UDP服务器对象
    /// </summary>
    public class UDPServerClass
    {
        public delegate void MessageHandler(string Message);//定义委托事件
        public event MessageHandler MessageArrived;
        public UDPServerClass()
        {
            //获取本机可用IP地址
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ipa in ips)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    MyIPAddress = ipa;//获取本地IP地址
                  
                }
            }
          
           
          
            Note_StringBuilder = new StringBuilder();
            PortName = 9998;

        }
        /// <summary>  
        /// ip地址，网络字节序，long类型 
        /// </summary>  
        /// <param name="ipmsg">文件名</param>  
        public long IpChange(string ipmsg)
        {
            char[] separator = new char[] { '.' };
            string[] items = ipmsg.Split(separator);
            long dreamduip = long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);
            long sun = System.Net.IPAddress.HostToNetworkOrder(dreamduip);
            return sun;
        }

        public UdpClient ReceiveUdpClient;

        /// <summary>
        /// 侦听端口名称
        /// </summary>
        public int PortName;

        /// <summary>
        /// 本地地址
        /// </summary>
        public IPEndPoint LocalIPEndPoint;

        /// <summary>
        /// 日志记录
        /// </summary>
        public StringBuilder Note_StringBuilder;
        /// <summary>
        /// 本地IP地址
        /// </summary>
        public IPAddress MyIPAddress;

        public void Thread_Listen()
        {
            //创建一个线程接收远程主机发来的信息
            Thread myThread = new Thread(ReceiveData);
            myThread.IsBackground = true;
            myThread.Start();
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        private void ReceiveData()
        {
            IPEndPoint local = new IPEndPoint(MyIPAddress, PortName);
            ReceiveUdpClient = new UdpClient(local);
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    
                    //关闭udpClient 时此句会产生异常
                    byte[] receiveBytes = ReceiveUdpClient.Receive(ref remote);
                    string receiveMessage = Encoding.Default.GetString(receiveBytes, 0, receiveBytes.Length);
                    //  receiveMessage = ASCIIEncoding.ASCII.GetString(receiveBytes, 0, receiveBytes.Length);
                    MessageArrived(string.Format("{0}来自{1}:{2}", DateTime.Now.ToString(), remote, receiveMessage));
                    //try
                    //{
                    //    Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");
                    //    ReceiveUdpClient.Send(sendBytes, sendBytes.Length, local);
                    //}
                    //catch (Exception e)
                    //{
                    //}
                    //break;

                }
                catch
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 添加日志信息到Note_StringBuilder
        /// </summary>
        public void AddMessage_Note_StringBuilder()
        {

        }
    }
}
namespace XTXK.Common
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    #region Delegates

    /// <summary>
    /// 消息接收委托
    /// </summary>
    public delegate void MessageReceivedHandler(object sender, MessageReceivedEventArgs e);

    #endregion Delegates

    /// <summary>
    /// Udp消息接收事件参数对象
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        #region Fields

        private readonly string message;

        #endregion Fields

        #region Constructors

        public MessageReceivedEventArgs(string message)
        {
            this.message = message;
        }

        #endregion Constructors

        #region Properties

        public string ReceivedMessage
        {
            get
            {
                return this.message;
            }
        }

        #endregion Properties
    }

    /// <summary> 
    /// 消息接收对象
    /// </summary>
    public class UdpReceiver
    {
        #region Fields

        private UdpClient client;
        private IPEndPoint endPoint;

        #endregion Fields

        #region Constructors

        public UdpReceiver(int port)
        {
            endPoint = new IPEndPoint(IPAddress.Any, port);
            client = new UdpClient(endPoint);
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when [on message received].
        /// </summary>
        public event MessageReceivedHandler MessageReceived;

        #endregion Events

        #region Methods

        public void OnReceived(IAsyncResult result)
        {
            try
            {
                Byte[] receiveBytes = client.EndReceive(result, ref endPoint);
                string messageReceived = Encoding.UTF8.GetString(receiveBytes);

                if (receiveBytes.Length > 0)
                {
                    if (this.MessageReceived != null)
                    {
                        MessageReceivedEventArgs e = new MessageReceivedEventArgs(string.Format("{0}来自{1}:{2}", DateTime.Now.ToString(), endPoint, messageReceived));
                        this.MessageReceived(this, e);
                    }
                }

                client.BeginReceive(new AsyncCallback(OnReceived), endPoint);
            }
            catch
            {

            }
        }

        public void StartReceive()
        {
            client.BeginReceive(new AsyncCallback(OnReceived), endPoint);

            //Thread.Sleep(Timeout.Infinite);
        }

        #endregion Methods
    }
}
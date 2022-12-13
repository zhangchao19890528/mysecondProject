using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using UdpChatExample;
using XTXK.Common;
using System.IO;

namespace WindowsFormsApplication6
{
    public partial class Form1 : Form
    {
        private int VoiceSec = 20;
        private int RefreshTime = 0;//声音计时器默认清0 
        public Form1()
        {
            InitializeComponent();

   
            UdpReceiver test = new UdpReceiver(3081);
            test.MessageReceived += new MessageReceivedHandler(test_MessageReceived);
            test.StartReceive();
          
            listBox1.Items.Add("开启UDP3081端口监听报警成功！");
            string[] strList = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "设备表.txt", System.Text.Encoding.Default);
             for (int i = 0; i < strList.Count(); i++)
             {   
                 checkedListBox1.Items.Add(strList[i]);
             }


               
           

           
        }

        void test_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
          
            this.Invoke(new DelegateChangeText(ChangeTxt), e.ReceivedMessage);
        }
        bool IsStartCount = false;
        public delegate void DelegateChangeText(string Messages);
        void ChangeTxt(string Messages)
        {

           // listBox1.Items.Add(Messages);
    
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                string msg= checkedListBox1.CheckedItems[i].ToString();
                if (Messages.Contains(msg.Split(',')[0]))
                {
                    IsStartCount = true;
                    listBox1.Items.Add(Messages + "找到了匹配的，播放音乐");
                    PlayVoice(AppDomain.CurrentDomain.BaseDirectory + "jingdi.wav");
                
                    return;
                }
               

 
            }
            listBox1.Items.Add("不播放音乐"+Messages);
        


           
        }

        /// <summary>
        /// 播放声音文件
        /// </summary>
        /// <param name="filename">声音文件</param>
        public static void PlayVoice(string filename)
        {
            try
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.SoundLocation = filename;
                player.LoadAsync();
                player.PlayLooping();
            }
            catch (Exception ex)
            {
                
            }
        }

        StringBuilder SB = new StringBuilder();


        void UDPServerClass1_MessageArrived(string Message)
        {
            try
            {
             //   richTextBox1.Invoke(new DelegateChangeText(ChangeTxt), Message);
                
               
            //    MessageBox.Show("<script type='text/javascript'>alert('发送成功！');history.go(-1)</script>");//发送成功则提示返回当前页面；
            }
            catch (Exception ex)
            {
              //  MessageBox.Show("<script type='text/javascript'>alert('" + ex + "！');history.go(-1)</script>");//打印错误
            }
         
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (IsStartCount)
            {
                RefreshTime += 1;
                if (RefreshTime >= VoiceSec)
                {
                    IsStartCount = false;
                    RefreshTime = 0;
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                    player.Stop(); //停止播放声音
                    listBox1.Items.Add("停止播放");

                }
            }
        }

        private void 开始播放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayVoice(AppDomain.CurrentDomain.BaseDirectory + "jingdi.wav");

        }

        private void 停止播放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.Stop(); //停止播放声音
            listBox1.Items.Add("停止播放");


        }

    }
}

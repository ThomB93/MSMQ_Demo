using System;
using System.Messaging;
using System.Windows.Forms;
using MSMQ_Demo.MyMessage;

namespace MSMQ_Demo.MessageTo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            #region Create My Own Queue 

            MessageQueue messageQueue = null;
            if (MessageQueue.Exists(@".\Private$\demo"))
            {
                messageQueue = new MessageQueue(@".\Private$\demo");
                messageQueue.Label = "MyQueueLabel";
            }
            else
            {
                // Create the Queue
                MessageQueue.Create(@".\Private$\demo");
                messageQueue = new MessageQueue(@".\Private$\demo");
                messageQueue.Label = "MyQueueLabel";
            }

            #endregion

            MyMessage.MyMessage m1 = new MyMessage.MyMessage();
            m1.BornPoint = DateTime.Now;
            m1.LifeInterval = TimeSpan.FromSeconds(5);
            m1.Text = "Command Start: " + DateTime.Now.ToString();

            messageQueue.Send(m1);
        }
    }
}

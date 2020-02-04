using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Messaging;
using MSMQ_Demo.MyMessage;

namespace MSMQ_Demo.MessageFrom
{
    public partial class Form1 : Form
    {
        Timer t = new Timer();
        BackgroundWorker bw1 = new BackgroundWorker();
        string text = string.Empty;

        public Form1()
        {
            InitializeComponent();

            t.Interval = 1000;
            t.Tick += T_Tick;
            t.Start();

            bw1.DoWork += (sender, args) => args.Result = Operation1();
            bw1.RunWorkerCompleted += (sender, args) =>
            {
                if ((bool)args.Result)
                {
                    richTextBox1.Text += text;
                }
            };
        }

        private object Operation1()
        {
            try
            {
                if (MessageQueue.Exists(@".\Private$\demo"))
                {
                    MessageQueue messageQueue = new MessageQueue(@".\Private$\demo");
                    messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(MyMessage.MyMessage) });


                    System.Messaging.Message[] messages = messageQueue.GetAllMessages();

                    var isOK = messages.Count() > 0 ? true : false;
                    foreach (System.Messaging.Message m in messages)
                    {
                        try
                        {
                            var command = (MyMessage.MyMessage)m.Body;
                            text = command.Text + Environment.NewLine;
                        }
                        catch (MessageQueueException ex)
                        {
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    messageQueue.Purge(); // after all processing, delete all the messages
                    return isOK;
                }
            }
            catch (MessageQueueException ex)
            {
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            t.Enabled = false;

            if (!bw1.IsBusy)
                bw1.RunWorkerAsync();

            t.Enabled = true;
        }
    }
}

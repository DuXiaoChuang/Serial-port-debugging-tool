using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.IO;


namespace 串口调节小助手_MJ版
{
    public partial class Form1 : Form
    {
        public int ok = 0;
        public int ce = 0;
        public int cb = 0;
        public Form1()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "DiamondBlue.ssk";
        }
        private string m_portname;
        public string Portname
        {
            get { return m_portname; }
            set { m_portname = value; }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                this.comboBox1.Items.Add(port);//存放串口名称
            }
            this.Portname = this.serialPort1.PortName;
            this.comboBox1.Text = Portname;
            comboBox2.Text = "9600";
            comboBox3.Text = "8";//数据位
            //comboBox4.Items.AddRange(Enum.GetNames(typeof(StopBits)));
            comboBox4.Items.AddRange(Enum.GetNames(typeof(StopBits)));
            comboBox4.Items.Remove(comboBox4.Items[0]);
            comboBox4.Text = comboBox4.Items[0].ToString();//停止位
            comboBox5.Items.AddRange(Enum.GetNames(typeof(Parity)));
            comboBox5.Text = comboBox5.Items[0].ToString();//校验位

            this.pictureBox1.Image = Properties.Resources.Red1;
            this.pictureBox2.Image = Properties.Resources.Red1;

            this.label6.Text = "串口已经关闭......";


        }
        /// <summary>
        /// 启动串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (ok == 0)
            {
                this.serialPort1.Close();
                this.serialPort1.PortName = this.comboBox1.Text;
                this.serialPort1.BaudRate = Convert.ToInt32(this.comboBox2.Text);
                this.serialPort1.DataBits = Convert.ToInt32(this.comboBox3.Text);
                this.serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), this.comboBox4.Text);
                this.serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), this.comboBox5.Text);
                try
                {
                    try
                    {
                        this.serialPort1.Open();
                        this.button1.Text = "关闭串口";
                        this.pictureBox1.Image = Properties.Resources.Green1;
                        this.pictureBox2.Image = Properties.Resources.Green1;
                        this.timer1.Enabled = true;
                        this.timer2.Enabled = false;
                        this.timer3.Enabled = true;
                        this.comboBox1.Enabled = false;
                        this.comboBox2.Enabled = false;
                        this.comboBox3.Enabled = false;
                        this.comboBox4.Enabled = false;
                        this.comboBox5.Enabled = false;
                        this.label6.Text = "串口" + serialPort1.PortName.ToString() + "已经打开......";
                        ok = 1;

                    }
                    catch (UnauthorizedAccessException ue)
                    {
                        MessageBox.Show(ue.Message.ToString(), "错误", MessageBoxButtons.OK);
                    }
                }
                catch (System.IO.IOException ee)
                {
                    MessageBox.Show(ee.Message.ToString(), "错误", MessageBoxButtons.OK);
                }
                return;
            }
            else
            {
                try
                {
                    this.serialPort1.Close();
                    this.timer3.Stop();
                    this.timer3.Enabled = false;
                    MessageBox.Show("串口关闭成功！");
                    this.button1.Text = "打开串口";
                    this.pictureBox1.Image = Properties.Resources.Red1;
                    timer1.Enabled = false;
                    this.timer2.Enabled = true;
                    this.comboBox1.Enabled = true;
                    this.comboBox2.Enabled = true;
                    this.comboBox3.Enabled = true;
                    this.comboBox4.Enabled = true;
                    this.comboBox5.Enabled = true;
                    ok = 0;
                    ce = 0;
                    cb = 0;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());

                }

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ce == 0)
            {
                this.pictureBox2.Image = Properties.Resources.Green1;
                this.pictureBox1.Image = Properties.Resources.Yellow;
                ce = 1;
                return;
            }
            if (ce == 1)
            {
                this.pictureBox2.Image = Properties.Resources.Yellow;
                this.pictureBox1.Image = Properties.Resources.Green1;
                ce = 0;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (cb == 0)
            {
                this.pictureBox2.Image = Properties.Resources.Yellow;
                this.pictureBox1.Image = Properties.Resources.Red1;
                cb = 1;
                return;
            }
            if (cb == 1)
            {
                this.pictureBox2.Image = Properties.Resources.Red1;
                this.pictureBox1.Image = Properties.Resources.Yellow;
                cb = 0;
            }

        }
        /// <summary>
        /// 实时接收字符串数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            byte[] buffer = new byte[serialPort1.BytesToRead];
            CheckForIllegalCrossThreadCalls = false;
            try
            {
                serialPort1.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (radioButton3.Checked == true)//十六进制
                        this.richTextBox1.Text += buffer[i].ToString("X2") + " ";
                    if (radioButton5.Checked == true)//字符型
                        this.richTextBox1.Text += (char)buffer[i];
                    if (radioButton4.Checked == true)//十进制
                        this.richTextBox1.Text += buffer[i].ToString() + " ";
                }
                int cout = Convert.ToInt32(this.textBox2.Text) + buffer.Length;
                this.textBox2.Text = cout.ToString();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString()); ;
                //    return;
            }

        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            button3_Click(sender, e);
        }
        /// <summary>
        /// 清空计数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "0";
            this.textBox1.Text = "0";
            this.richTextBox1.Text = "";
            this.richTextBox2.Text = "";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked == true)
            {
                this.button3.Enabled = true;
                this.textBox3.Enabled = false;
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked == true)
            {
                this.button3.Enabled = false;
                this.textBox3.Enabled = true;
                this.timer4.Interval = Convert.ToInt32(this.textBox3.Text);
                this.timer4.Start();

            }
        }
        /// <summary>
        /// 向串口发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {

            if (serialPort1.IsOpen)
            {
                int i = 0;
                byte[] value;
                string[] hexValuesSplit = this.richTextBox2.Text.Split(' ');
                value = new byte[hexValuesSplit.Length];

                char[] cbuff = new char[this.richTextBox2.Text.Length];
                cbuff = this.richTextBox2.Text.ToCharArray();
                try
                {
                    if (radioButton6.Checked == true || radioButton7.Checked == true)
                    {
                        foreach (string hex in hexValuesSplit)
                        {
                            if (radioButton6.Checked == true)//发送十六进制
                                value[i] = (byte)Convert.ToInt32(hex, 16);
                            if (radioButton7.Checked == true)//发送十进制
                                value[i] = (byte)Convert.ToInt32(hex, 10);
                            i++;
                        }

                        this.serialPort1.Write(value, 0, value.Length);
                        int cout = Convert.ToInt32(this.textBox1.Text) + value.Length;
                        this.textBox1.Text = cout.ToString();
                    }
                    else
                    {
                        //this.serialPort1.Write(cbuff, 0, cbuff.Length);
                        this.serialPort1.Write(this.richTextBox2.Text.ToString());
                        int coutA = Convert.ToInt32(this.textBox1.Text) + cbuff.Length;
                        this.textBox1.Text = coutA.ToString();
                    }
                }
                catch
                {
                    string s = this.richTextBox2.Text;
                    char[] buffe = s.ToCharArray(0, s.Length);
                    char[] bbuffe = new char[buffe.Length];
                    bbuffe = richTextBox2.Text.ToCharArray(0, bbuffe.Length);
                    if (radioButton6.Checked == true)
                    {
                        foreach (char c in buffe)
                        {
                            int value1 = Convert.ToInt32(c);
                            string st = string.Format("{0:X}", value1) + " ";
                            serialPort1.Write(st);
                            int coutAB = Convert.ToInt32(this.textBox1.Text) + st.Length;
                            this.textBox1.Text = coutAB.ToString();
                        }
                    }
                    if (radioButton7.Checked == true)
                    {
                        foreach (char c in buffe)
                        {
                            string ss = Convert.ToInt32(c).ToString() + " ";
                            serialPort1.Write(ss);
                        }
                        int coutAA = Convert.ToInt32(this.textBox1.Text) + value.Length;
                        this.textBox1.Text = coutAA.ToString();
                    }
                    //this.richTextBox1.Text += "出错了，可能是发送的类型不对";
                    return;
                }
            }
        }

        /// <summary>
        /// 停止取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            this.timer4.Stop();
            this.button3.Enabled = true;

        }
        /// <summary>
        /// 保存接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            if (this.richTextBox1.Text == "")
                return;
            string filename = DateTime.Now.ToString() + "接收数据";
            saveFileDialog1.Filter = "文本文件 (*.txt)|*.txt";
            saveFileDialog1.Title = "保存文件";
            saveFileDialog1.FileName = filename;
            if (DialogResult.Cancel == saveFileDialog1.ShowDialog())
                return;
            string strpath = saveFileDialog1.FileName;
            ///创建文件
            FileStream fs = File.Create(strpath);
            fs.Close();
            ///向文件中写入
            StreamWriter sw = new StreamWriter(strpath);
            sw.Write(this.richTextBox1.Text);
            sw.Flush();
            sw.Close();
            MessageBox.Show(this, "成功创建文件！", "提示对话框", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.richTextBox2.Text == "")
                return;
            string filename = DateTime.Now.Ticks.ToString() + "接收数据";
            saveFileDialog1.Filter = "文本文件 (*.txt)|*.txt";
            saveFileDialog1.Title = "保存文件";
            saveFileDialog1.FileName = filename;
            if (DialogResult.Cancel == saveFileDialog1.ShowDialog())
                return;
            string strpath = saveFileDialog1.FileName;
            ///创建文件
            FileStream fs = File.Create(strpath);
            fs.Close();
            ///向文件中写入
            StreamWriter sw = new StreamWriter(strpath);
            sw.Write(this.richTextBox2.Text);
            sw.Flush();
            sw.Close();
            MessageBox.Show(this, "成功创建文件！", "提示对话框", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }
        int j;
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            //byte[] buffer = new byte[serialPort1.BytesToRead];
            //CheckForIllegalCrossThreadCalls = false;
            //string ReceiveString = "";
            ////ReceiveString = serialPort1.ReadLine();
            ////ReceiveString = ReceiveString.Trim('\r');
            //try
            //{
            //    serialPort1.Read(buffer, 0, buffer.Length);
            //    for (int i = 0; i < buffer.Length; i++)
            //    {
            //        ReceiveString += (char)buffer[i];
            //    }
            //    richTextBox1.Text = ReceiveString.ToString();
            //}
            //catch (Exception ee)
            //{
            //    throw ee;
            //}
        }
    }
}
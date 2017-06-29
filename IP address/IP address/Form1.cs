using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace IP_address
{
    public partial class Form1 : Form
    {
        List<string> allIP = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            maskedTextBox1.Mask = @"999\.999\.999\.999";
            maskedTextBox2.Mask = @"999\.999\.999\.999";
            maskedTextBox1.PromptChar = ' ';
            maskedTextBox2.PromptChar = ' ';
            maskedTextBox1.MaskInputRejected += new MaskInputRejectedEventHandler(maskedTextBox_MaskInputRejected);
            maskedTextBox2.MaskInputRejected += new MaskInputRejectedEventHandler(maskedTextBox_MaskInputRejected);

            openFile();
        }

        private void maskedTextBox_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            toolTip1.ToolTipTitle = "Ошибка ввода";
            toolTip1.Show("Допустимые значения: (0-9)", maskedTextBox2, 0, 30, 3000);
        }

        private void searchIP(string startIP, string endIP, List<string> allIP)
        {
            string[] splitStartrIP = startIP.Split('.');
            string[] splitEndIP = endIP.Split('.');
            string[] splitAllIP;
            int[] start = new int[4];
            int[] end = new int[4];
            int[] all = new int [4];
            bool b = true;

            string sSIP, sEIP;

            for (int i = 0; i < 4; i++)
            {
                sSIP = splitStartrIP[i].Replace(" ", string.Empty);
                sEIP = splitEndIP[i].Replace(" ", string.Empty);

                if (sSIP == string.Empty) sSIP = "0";
                if (sEIP == string.Empty) sEIP = "255";

                start[i] = Convert.ToInt32(sSIP);
                end[i] = Convert.ToInt32(sEIP);

                if (start[i] > end[i] || start[i] < 0 || end[i] > 255)
                {
                    toolTip1.ToolTipTitle = "Ошибка ввода";
                    toolTip1.Show("Значение 'от' не может превышать значение 'до' " +
                        "\n Значения 'от' не могут быть меньше 0" +
                        "\n Значения 'до' не могут быть больше 255", maskedTextBox2, 0, 30, 3000);

                    return;
                }
            }

            foreach (var ip in allIP)
            {
                splitAllIP = ip.Split('.');

                b = true;

                for (int i = 0; i < 4; i++)
                {
                    all[i] = Convert.ToInt32(splitAllIP[i]);

                    if (all[i] < start[i] || all[i] > end[i])
                    {
                        b = false;
                        break;
                    }
                }

                if (b) richTextBox1.Text += ip + "\n";
            }
        }

        private void openFile()
        {
            string line = string.Empty;

            try
            {   
                using (StreamReader sr = new StreamReader("IP.txt"))
                {
                    while ((line = sr.ReadLine()) != null) allIP.Add(line);
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            searchIP(maskedTextBox1.Text, maskedTextBox2.Text, allIP);
        }
    }
}
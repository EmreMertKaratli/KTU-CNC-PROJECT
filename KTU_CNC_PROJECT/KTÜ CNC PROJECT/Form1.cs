using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTÜ_CNC_PROJECT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox2.Items.AddRange(new object[] { 9600, 19200, 38400, 57600, 115200 });
            button3.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.DataSource = SerialPort.GetPortNames();
            serialPort1.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            serialPort1.PortName = comboBox1.SelectedItem.ToString();
            serialPort1.BaudRate = Convert.ToInt32(comboBox2.SelectedItem.ToString());
            serialPort1.DataBits = 8;
            serialPort1.Parity = Parity.None;
            serialPort1.StopBits = StopBits.One;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.Open();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Metin Dosyası |*.txt";
            file.FilterIndex = 1;
            file.RestoreDirectory = true;
            file.CheckFileExists = false;
            file.Title = "Gcode Dosyası Seçiniz..";

            if (file.ShowDialog() == DialogResult.OK)
            {
                StreamReader oku = new StreamReader(file.FileName);
                string satir;
                while (true)
                {
                    satir = oku.ReadLine();
                    if (satir == null) break;
                    serialPort1.Write("%"); textBox1.AppendText("%");
                    while (serialPort1.ReadByte() != '%') { }
                    //listBox1.Items.Add(satir);
                    char[] veriler1 = satir.ToCharArray();

                    for (int i = 0; i < veriler1.Length; i++)
                    {
                        for (; veriler1[i] != ' ';)
                        {
                            if (veriler1[i] == '(')
                            {
                                for (; veriler1[i] != ')';) { i++; }
                                if (veriler1.Length == (i + 1)) { break; } else { i++; }
                            }
                            if (veriler1[i] == null) break;
                            serialPort1.Write(Convert.ToString(veriler1[i]));
                            textBox1.AppendText(Convert.ToString(veriler1[i]));
                            break;
                        }
                        if (veriler1[i] == null) break;
                        textBox2.AppendText(Convert.ToString(veriler1[i]));
                    }
                    serialPort1.Write("%"); textBox1.AppendText("%");
                }
            }
        }
    }
}

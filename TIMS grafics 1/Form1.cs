using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS_grafics_1
{
    public partial class Form1 : Form
    {
        Diskerete d = new Diskerete();
        Continuous c = new Continuous();
        string dfileName = "TextFile1.txt";
        string cfileName = "TextFile2.txt";
        public Form1()
        {
            InitializeComponent();
            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            runD(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            runD(2);
        }
        private void runD(int i)
        {
            bool goo = false;
            if (i == 1)
            {
                int start;
                int end;
                int size;
                double A;
                if (int.TryParse(Start.Text, out start) && int.TryParse(End.Text, out end) && int.TryParse(Size.Text, out size) && double.TryParse(AD.Text,out A) && (A < 1) && (A > 0) && (start <= end) && (size > 0))
                {   
                    d.A = A;
                    d.Generate(start, end, size);
                    goo = true;
                }
                else if(!int.TryParse(Start.Text, out start)) MessageBox.Show("Неправильний початок!!!");
                else if (!int.TryParse(End.Text, out end)) MessageBox.Show("Неправильний кінець!!!");
                else if (!int.TryParse(Size.Text, out size)) MessageBox.Show("Неправильнй розмір!!");
                else if (!(start <= end)) MessageBox.Show("Початок більший кінця!!!");
                else if (!(size > 0)) MessageBox.Show("Розмір менший одиниці!!!");
                else if (!double.TryParse(AD.Text, out A)) MessageBox.Show("Неправильна Альфа!!!");
                else if (!(A < 1)) MessageBox.Show("Альфа більша-рівна одигиці!!!");
                else if (!(A > 0)) MessageBox.Show("Альфа менша-рівна нуля!!!");
            }
            if (i == 2)
            {
                d.Read(dfileName);
                goo = true;
            }
            if (goo)
            {
                listBox1.Items.Clear();
                textBox1.Text = d.GetVibircaStr();
                listBox1.Items.Add(d.GetKeys());
                listBox1.Items.Add(d.GetValues());
                rozmah.Text = d.Rozmah.ToString();
                median.Text = d.Mediana.ToString("0.000");
                moda.Text = d.GetModaStr();
                midl.Text = d.Xmedium.ToString("0.000");
                devi.Text = d.Deviation.ToString("0.000");
                varia.Text = d.Variansa.ToString("0.000");
                standart.Text = d.Standart.ToString("0.000");
                dvariacia.Text = d.Variacia.ToString("0.000");

                chart1.Series[0].Points.Clear();
                chart2.Series[0].Points.Clear();
                chart3.Series[0].Points.Clear();

                foreach (var item in d.Unic)
                {
                    chart1.Series[0].Points.AddXY(item.Key, 0);
                    chart1.Series[0].Points.AddXY(item.Key, item.Value);
                    chart1.Series[0].Points.AddXY(item.Key, 0);
                }
                for (int it = d.Unic.Keys.Min();it < d.Unic.Keys.Max() + 1;it++)
                {
                    int val;
                    d.Unic.TryGetValue(it, out val);
                    chart2.Series[0].Points.AddXY(it, val);
                }
                
                double sum = 0;
                foreach (var item in d.Unic) sum += item.Value;
                double temp_sum = 0;
                bool poch = true;
                int j = 0;
                double last_s = 0;
                foreach (var item in d.Unic)
                {
                    if (poch)
                    {
                        for (int l = 1; l < 99; l++)
                        {
                            chart3.Series[0].Points.AddXY(item.Key - 1 + 0.01 * l, temp_sum / sum);
                        }
                        poch = false;
                    }
                    temp_sum += item.Value;
                    for (int l = 5; l < 90; l++)
                    {
                        chart3.Series[0].Points.AddXY(item.Key + 0.01*l, temp_sum / sum);
                    }
                    while ((item.Key - j) > 1)
                    {
                        j++;
                        for (int l = 4; l < 90; l++)
                        {
                            chart3.Series[0].Points.AddXY(j + 0.01 * l, last_s);
                        }
                    }
                    last_s = temp_sum / sum;
                    j = item.Key;
                }
                if(!poch)
                    chart3.Series[0].Points.AddXY(j + 1,1);
                xcrit.Text = d.XCrit.ToString();
                xemp.Text = d.XEmp.ToString();
                dataGridView1.DataSource = d.GetGridData();
                if (d.Hipotes())
                {
                    rezD.Text = "Гіпотеза підтверджена";
                }
                else
                {
                    rezD.Text = "Гіпотеза не підтверджена";
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            runС(1);
        }
        private void runС(int i)
        {
            bool goo = false;
            if (i == 1)
            {
                double start;
                double end;
                int size;
                double A;
                if (double.TryParse(CStart.Text, out start) && double.TryParse(CEnd.Text, out end) && int.TryParse(CSize.Text, out size) && double.TryParse(AD.Text, out A) && (A < 1) && (A > 0) && (start <= end) && (size > 0))
                {
                    c.A = A;
                    c.Generate(start, end, size);
                    goo = true;
                }
                else if (!double.TryParse(CStart.Text, out start)) MessageBox.Show("Неправильний початок!!!");
                else if (!double.TryParse(CEnd.Text, out end)) MessageBox.Show("Неправильний кінець!!!");
                else if (!int.TryParse(CSize.Text, out size)) MessageBox.Show("Неправильнй розмір!!");
                else if (!(start <= end)) MessageBox.Show("Початок більший кінця!!!");
                else if (!(size > 0)) MessageBox.Show("Розмір менший одиниці!!!");
                else if (!double.TryParse(AD.Text, out A)) MessageBox.Show("Неправильна Альфа!!!");
                else if (!(A < 1)) MessageBox.Show("Альфа більша-рівна одигиці!!!");
                else if (!(A > 0)) MessageBox.Show("Альфа менша-рівна нуля!!!");
            }
            if (i == 2)
            {
                c.Read(cfileName);
                goo = true;
            }
            if (goo)
            {
                listBox2.Items.Clear();
                CVib.Text = c.GetVibircaStr();
                listBox2.Items.Add(c.GetKeys());
                listBox2.Items.Add(c.GetValues());
                crozmah.Text = c.Rozmah.ToString();
                cmedian.Text = c.Mediana.ToString("0.000");
                cmoda.Text = c.GetModaStr();
                cxmid.Text = c.Xmedium.ToString("0.000");
                cdeviac.Text = c.Deviation.ToString("0.000");
                cvarians.Text = c.Variansa.ToString("0.000");
                cstandart.Text = c.Standart.ToString("0.000");
                cvariacia.Text = c.Variacia.ToString("0.000");

                chart4.Series[0].Points.Clear();
                chart5.Series[0].Points.Clear();
                chart6.Series[0].Points.Clear();

                double sum = 0;
                foreach (var item in c.Zet) sum += item.Value;
                double temp_sum = 0;
                bool poch = true;
                double j = 0;
                foreach (var item in c.Zet)
                {
                    if (poch)
                    {
                        chart4.Series[0].Points.AddXY(item.Key - 1, 0);
                        poch = false;
                    }
                    temp_sum += item.Value;
                    chart4.Series[0].Points.AddXY(item.Key, temp_sum / sum);
                    j = item.Key;
                }
                if (!poch)
                    chart4.Series[0].Points.AddXY(j + 1, 1);
                foreach (var item in c.Zet)
                {
                    chart5.Series[0].Points.AddXY(item.Key, item.Value);
                }
                foreach (var item in c.Zet)
                {
                    chart6.Series[0].Points.AddXY(item.Key, item.Value);
                }
            }
            xcritC.Text = c.XCrit.ToString();
            xempC.Text = c.XEmp.ToString();
            dataGridView2.DataSource = c.GetGridData();
            if (c.Hipotes())
            {
                rezC.Text = "Гіпотеза підтверджена";
            }
            else
            {
                rezC.Text = "Гіпотеза не підтверджена";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            runС(2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }
    }
}

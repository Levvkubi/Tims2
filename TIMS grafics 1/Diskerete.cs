using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMS_grafics_1
{
    class Diskerete
    {
        private List<int> vibirka = new List<int>();
        private Dictionary<int, int> unic = new Dictionary<int, int>();
        private List<int> moda = new List<int>();
        private double mediana;
        private double rozmah;
        private double xmedium;
        private double deviation;
        private double variansa;
        private double standart;
        private double variacia;
        private List<double> pi = new List<double>();
        private List<double> npi = new List<double>();
        private double xcrit;
        private double xemp;
        public List<int> Vibirka { get => vibirka; }
        public Dictionary<int, int> Unic { get => unic; }
        public List<int> Moda { get => moda; }
        public double Mediana { get => mediana; }
        public double Rozmah { get => rozmah; }
        public double Xmedium { get => xmedium; }
        public double Deviation { get => deviation; }
        public double Variansa { get => variansa; }
        public double Standart { get => standart; }
        public double Variacia { get => variacia; }
        public List<double> Pi { get => pi; }
        public List<double> NPi { get => npi; }
        public double XCrit { get => xcrit; }
        public double XEmp { get => xemp; }
        public double A { get; set; }
        public List<GridData> GD = new List<GridData>();

        public Diskerete() { }

        public Diskerete(string fileName)
        {
            Read(fileName);
        }
        public Diskerete(int start, int end, int size)
        {
            Generate(start, end, size);
        }
        public void Read(string fileName)
        {
            Clear();
            string str = "";
            StreamReader reader = new StreamReader(fileName);
            string[] mas;
            while ((str = reader.ReadLine()) != null)
            {
                mas = str.Split();
                for (int i = 0; i < mas.Length; i++)
                {
                    vibirka.Add(Convert.ToInt32(mas[i]));
                }
            }
            CalcHaract();
        }
        public void Generate(int start, int end, int size)
        {
            Clear();
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                vibirka.Add(rand.Next(start, end + 1));
            }
            CalcHaract();
        }

        private void CalcHaract()
        {
            vibirka.Sort();
            CalcUnic();
            CalcModa();
            CalcMediana();
            CalcRozmah();
            CalcXmedium();
            CalcDeviation();
            CalcVariansa();
            CalcStandart();
            CalcVariacia();
            CalcPuasson();
            CalcNPI();
            CalcXEmp();
            CalcXCrit();
        }
        private void CalcUnic()
        {
            for (int i = 0; i < vibirka.Count; i++)
            {
                if (unic.ContainsKey(vibirka[i])) unic[vibirka[i]]++;
                else unic.Add(vibirka[i], 1);
            }

        }
        public string GetKeys()
        {
            string rez = "";
            foreach (var item in unic.Keys)
            {
                if (item < 10 && item > -10) rez += item.ToString() + "    ";
                else if (item < 100 && item > -100) rez += item.ToString() + "  ";
                else rez += item.ToString();
            }
            return rez;
        }
        public string GetValues()
        {
            string rez = "";
            foreach (var item in unic.Values)
            {
                if (item < 10 && item > -10) rez += item.ToString() + "    ";
                else if (item < 100 && item > -100) rez += item.ToString() + "  ";
                else rez += item.ToString();
            }
            return rez;
        }

        private void CalcModa()
        {
            int max = 0;
            foreach (int i in unic.Values) if (i > max) max = i;
            foreach (var item in unic) if (item.Value == max) moda.Add(item.Key);
        }
        private void CalcVariacia()
        {
            variacia = standart / xmedium;
        }
        public string GetModaStr()
        {
            string rez = "";
            foreach (var item in moda)
            {
                rez += item.ToString() + " ";
            }
            return rez;
        }
        public string GetVibircaStr()
        {
            string rez = "";
            foreach (var item in vibirka)
            {
                rez += item.ToString() + " ";
            }
            return rez;
        }


        private void CalcMediana()
        {
            if (vibirka.Count % 2 == 0)
            {
                int ind = vibirka.Count / 2;
                mediana = (vibirka[ind] + vibirka[ind - 1]) / 2;
            }
            else
                mediana = vibirka[(vibirka.Count - 1) / 2];
        }
        private void CalcRozmah()
        {
            rozmah = vibirka[vibirka.Count - 1] - vibirka[0];
        }
        private void CalcXmedium()
        {
            double sum = 0;
            foreach (var item in vibirka) sum += item;
            xmedium = sum / vibirka.Count;
        }
        private void CalcDeviation()
        {
            double sum = 0;
            foreach (var i in unic) sum += i.Value * Math.Pow((i.Key - xmedium), 2);
            deviation = sum;
        }
        private void CalcVariansa()
        {
            double sum = 0;
            for (int i = 0; i < vibirka.Count; i++) sum += Math.Pow((vibirka[i] - xmedium), 2);
            variansa = sum / (vibirka.Count - 1);
        }
        private void CalcStandart()
        {
            standart = Math.Sqrt(variansa);
        }
        
        private void CalcPuasson()
        {
            double Factorial(int number)
            {
                if (number == 1)
                    return 1;
                else
                    return number * Factorial(number - 1);
            }
            for (int i = 1; i < unic.Keys.Count + 1; i++)
            {
                pi.Add((Math.Pow(xmedium, i) * Math.Pow(Math.E, -xmedium)) / Factorial(i));
            }
        }
        private void CalcNPI()
        {
            int n = vibirka.Count;
            foreach (var item in pi)
            {
                NPi.Add(item * n);
            }
        }
        private void CalcXEmp()
        {
            int i = 0;
            xemp = 0.0;
            foreach (var item in unic.Values)
            {
                xemp += Math.Pow(item - npi[i], 2) / npi[i];
            }
        }
        private void CalcXCrit()
        {
            int k = unic.Count;
            int x= 0, y = 0;
            List<List<double>> CritPOint = new List<List<double>>();
            StreamReader sr;
            try
            {
                using (sr = new StreamReader(@"D:\Programming\С#\TIMS grafics 1\CritPoints.txt", System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        List<double> temp = new List<double>();
                        string[] t = line.Split('\t');
                        foreach (var item in t)
                        {
                            temp.Add(double.Parse(item));
                        }
                        CritPOint.Add(temp);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (k > 120) x = 34;
            else {
                for (int i = 1; i < 34; i++)
                {
                    if (k <= CritPOint[i][0])
                    {
                        x = i;
                        break;
                    }
                }
             }
            if (A == 0) y = 2;
            else if(A <= 0.001)  y = 6;
            else
            {
                y = 1;
                while(A > CritPOint[0][y])
                {
                    y++;
                }
            }
            xcrit = CritPOint[x][y];
        }
        public bool Hipotes()//xe<xc+  
        {
            if (xemp < xcrit) return true;
            else return false;
        }
        public List<GridData> GetGridData()
        {
            
            int i = 0;
            foreach (var item in unic.Values)
            {
                GridData temp = new GridData() { ni = item,npi = npi[i],pi = pi[i]};
                GD.Add(temp);
                i++;
            }
            return GD;
        }

        public void Clear()
        {
            vibirka = new List<int>();
            unic = new Dictionary<int, int>();
            moda = new List<int>();
            mediana = new double();
            rozmah = new double();
            xmedium = new double();
            deviation = new double();
            variansa = new double();
            standart = new double();
            variacia = new double();
        }
    }
}

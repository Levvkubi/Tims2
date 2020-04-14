using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMS_grafics_1
{
    class Continuous
    {
        private List<double> vibirka = new List<double>();
        private Dictionary<double, double> unic = new Dictionary<double, double>();
        //private OrderedDictionary mag = new OrderedDictionary();
        private Dictionary<double, int> zet = new Dictionary<double, int>();
        private List<double> moda = new List<double>();
        private int r;
        private int n;
        private double step;
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
        public List<double> Vibirka { get => vibirka; }
        public Dictionary<double, double> Unic { get => unic; }
        public Dictionary<double, int> Zet { get => zet; }
        public List<double> Moda { get => moda; }
        public int R { get => r; }
        public double Step { get => step; }
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
        private List<GridData> GD = new List<GridData>();

        public Continuous() {}
        public Continuous(string fileName)
        {
            Read(fileName);
        }
        public Continuous(double start, double end, int size)
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
                    vibirka.Add(Convert.ToDouble(mas[i]));
                }
            }
            CalcHaract();
        }
        public void Generate(double start, double end, int size)
        {
            Clear();
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                double temp = Math.Round((rand.NextDouble() * (end - start) + start),3);//skhdgfhjvsdh
                vibirka.Add(temp);
            }
            CalcHaract();
        }
        private void CalcHaract()
        {
            vibirka.Sort();
            CalcR();
            CalcUnic();
            CalcRozmahStep();
            CalcZet();
            CalcModa();
            CalcMediana();
            CalcXMedium();
            CalcDeviation();
            CalcVariansa();
            CalcStandart();
            CalcVariacia();
            CalcEqual();
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
        private void CalcR()
        {
            r = 0;
            while (!((vibirka.Count > Math.Pow(2, r)) && (vibirka.Count <= Math.Pow(2, r + 1))))
            {
                r++;
            }
            r++;

        }
        private void CalcVariacia()
        {
            variacia = standart / xmedium;
        }
        private void CalcRozmahStep()
        {
            rozmah = vibirka[vibirka.Count - 1] - vibirka[0];
            step = rozmah / (r);//1
        }
        private void CalcZet()
        {
            bool bulo = false;
            for(int i = 0; i < r; i++)//1
            {
                double z = ((vibirka[0] + i * step) + (vibirka[0] + step * (i + 1))) / 2;
                z = Math.Round(z, 3);
                int count = 0;
                foreach (var item in vibirka)
                {
                    if (!bulo)
                    {
                        count++;
                        bulo = true;
                    }
                    if(((vibirka[0] + i * step) < item) && (item <= (vibirka[0] + step * (i + 1))))
                    {
                        count++;
                    }
                }
                zet.Add(z, count);
            }
            n = 0;
            foreach (var item in zet.Values)
            {
                n += item;
            }
            //foreach (var item in zet)
            //{
            //    mag.Add(item.Key, item.Value);
            //}
            
            
        }
        private void CalcModa()
        {
            int max = 0;
            int[] arr = new int[r];
            zet.Values.CopyTo(arr, 0);
            foreach (var item in zet.Values) if (item > max) max = item;
            for (int i = 0; i < r; i++)
            {
                if (arr[i] == max)
                {
                    if (i == 0)
                    {
                        moda.Add( (vibirka[0] + i * step) + step * ((arr[i]) / (arr[i] + arr[i] - arr[i + 1])));
                    }
                    else if (i == r - 1)
                    {
                        moda.Add((vibirka[0] + i * step) + step * ((arr[i] - arr[i - 1]) / (arr[i] - arr[i - 1] + arr[i])));
                    }
                    else
                    {
                        moda.Add((vibirka[0] + i * step) + step * ((arr[i] - arr[i - 1]) / (arr[i] - arr[i - 1] + arr[i] - arr[i + 1])));
                    }
                }
            }

            }
        public string GetModaStr()
        {
            string rez = "";
            foreach (var item in moda)
            {
                rez += item.ToString() + "  ";
            }
            return rez;
        }
        private void CalcMediana()
        {
            int tmp = n / 2;
            int wh = 0;
            int nazb = 0;
            int max = 0;
            int ind = 0;
            int M = 1;
            foreach (var item in zet.Values) if (item > max) max = item;
            foreach (var item in zet.Values)
            {
                wh += item;
                if((nazb <= tmp) && (tmp <= wh))
                {
                    M = item;
                    break;
                }
                else
                {
                    ind++;
                    nazb = wh;
                }
            }
            mediana = vibirka[0] + ind * step + step * ((tmp - nazb) / n);
        }
        private void CalcXMedium()
        {
            double sum = 0;
            foreach (var item in zet)
            {
                sum += item.Key * item.Value;
            }
            xmedium = sum / n;
        }
        private void CalcDeviation()
        {
            double sum = 0;
            foreach (var i in zet) sum += i.Value * Math.Pow((i.Key - xmedium), 2);
            deviation = sum;
        }
        private void CalcVariansa()
        {
            
            variansa = deviation / (vibirka.Count - 1);
        }
        private void CalcStandart()
        { 
            standart = Math.Sqrt(variansa);
        }
        public string GetKeys()
        {
            string rez = "";
            foreach (var item in Zet.Keys)
            {
                rez += item.ToString() + "\t";
            }
            return rez;
        }
        public string GetValues()
        {
            string rez = "";
            foreach (var item in Zet.Values)
            {
                rez += item.ToString() + "\t";
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
        private void CalcEqual()
        {
            double a = xmedium - Math.Sqrt(3) * standart;
            double b = xmedium + Math.Sqrt(3) * standart;
            double riz = b - a;
            for(int i = 0;i < zet.Count; i++)
            {
                pi.Add(step / riz);
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
            int k = zet.Count;
            int x = 0, y = 0;
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
            else
            {
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
            else if (A <= 0.001) y = 6;
            else
            {
                y = 1;
                while (A > CritPOint[0][y])
                {
                    y++;
                }
            }
            xcrit = CritPOint[x][y];
        }
        public List<GridData> GetGridData()
        {

            int i = 0;
            foreach (var item in zet.Values)
            {
                GridData temp = new GridData() { ni = item, npi = npi[i], pi = pi[i] };
                GD.Add(temp);
                i++;
            }
            return GD;
        }
        public bool Hipotes()
        {
            if (xemp < xcrit) return true;
            else return false;
        }
        public void Clear()
        {
            vibirka = new List<double>();
            unic = new Dictionary<double, double>();
            zet = new Dictionary<double, int>();
            moda = new List<double>();
            r = new int();
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

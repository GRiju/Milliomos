using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Milliomos
{
    class Kerdes
    {
        public string Kerd { get; }
        public string[] Valaszok { get; }
        public string Helyesvalasz { get; }
        public string Kategoria { get; }
        private string aktualisk;

        public Kerdes(bool sorkerdes, bool load)
        {
            if (load)
            {
                string sor = File.ReadAllText("mentes.sav", Encoding.UTF8);

                string decryptedload = "";

                for (int i = 0; i < sor.Length; i++)
                {
                    decryptedload += Convert.ToChar(sor[i]+1);
                }

                string[] sp = decryptedload.Split(';');

                Kerd = sp[1];

                Valaszok = new string[4];
                Valaszok[0] = sp[2];
                Valaszok[1] = sp[3];
                Valaszok[2] = sp[4];
                Valaszok[3] = sp[5];
                Helyesvalasz = sp[6];
                Kategoria = sp[7];
            }
        }

        public Kerdes(bool sorkerdes, int szint = 0)
        {
            string row = "";
            int ind = 0;
            if (!sorkerdes)
            {
                var qu = File.ReadAllLines("kerdes.txt").Where(w => w.Split(';')[0].ToString() == szint.ToString());

                int maxr = qu.Count();
                Random rand = new Random();
                int randq = rand.Next(maxr);

                row = qu.ToArray()[randq];
                ind++;
            }
            else
            {
                int maxr = File.ReadAllLines("sorkerdes.txt").Length;
                Random rand = new Random();
                int randq = rand.Next(maxr);

                row = File.ReadAllLines("sorkerdes.txt")[randq];
            }
            //Console.WriteLine(row);
            aktualisk = row;
            string[] temp = row.Split(';');

            Kerd = temp[ind++];
            Valaszok = new string[4];
            Valaszok[0] = temp[ind++];
            Valaszok[1] = temp[ind++];
            Valaszok[2] = temp[ind++];
            Valaszok[3] = temp[ind++];
            Helyesvalasz = temp[ind++];
            Kategoria = temp[ind++];
        }

        public string[] Felezes()
        {
            string[] vlszk = new string[2];
            

            for (int i = 0; i < 4; i++)
            {
                char betu = Convert.ToChar((Convert.ToInt32('A') + i));
                if (betu.ToString() == Helyesvalasz)
                {
                    vlszk[0] = betu + "\t-\t" + Valaszok[i];
                    break;
                }
            }

            Random rand = new Random();
            do
            {
                int szam = rand.Next(0, 4);
                char betu = Convert.ToChar((Convert.ToInt32('A') + szam));
                vlszk[1] = betu + "\t-\t" + Valaszok[szam];
            }
            while (vlszk[1] == vlszk[0]);
            
            return vlszk;
        }

        public string[] Kozonseg()
        {
            string[] vlszk = new string[4];

            Random rand = new Random();
            int jo = rand.Next(30, 71); //60

            int[] rossz = new int[3];
            rossz[0] = rand.Next(0, (100 - jo)); //100-40 = 60, 30
            rossz[1] = rand.Next(0, (100 - jo - rossz[0]));
            rossz[2] = 100 - jo - rossz[0] - rossz[1];

            int r = 0;
            for (int i = 0; i < 4; i++)
            {
                char betu = Convert.ToChar((Convert.ToInt32('A') + i));
                if(betu.ToString() == Helyesvalasz) vlszk[i] = Valaszok[i] + " [" + jo + "%]";
                else
                {
                    vlszk[i] = Valaszok[i] + " [" + rossz[r] + "%]";
                    r++;
                }
            }
            return vlszk;
        }

        public override string ToString()
        {
            return aktualisk;
        }
    }
}

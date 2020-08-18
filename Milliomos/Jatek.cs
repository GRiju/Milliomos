using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Milliomos
{
    class Jatek
    {
        private string nev;
        private int kerdesszam;
        private bool nyer = true;
        const int MAXKERDES = 15;
        private string segitsegek = "kft";
        private string segitseg = "";
        TimeSpan start;

        public Jatek(bool load = false)
        {
            Console.Clear();
            Kerdes s = new Kerdes(false, 1);
            bool kovkerdes = true;
            kerdesszam = 0;

            if (!load)
            {
                do
                {
                    Console.Clear();
                    Console.Write("Kérlek add meg a neved: ");
                    nev = Console.ReadLine();
                }
                while (nev.Length == 0);
                start = DateTime.Now.TimeOfDay;
            }
            else
            {
                s = new Kerdes(false, true);
                string sor = File.ReadAllText("mentes.sav", Encoding.UTF8);

                string decryptedload = "";
                for (int i = 0; i < sor.Length; i++)
                {
                    decryptedload += Convert.ToChar(sor[i] + 1);
                }

                string[] sp = decryptedload.Split(';');
                start = DateTime.Now.TimeOfDay - TimeSpan.Parse($"00:{Convert.ToInt32(sp[8])}:{Convert.ToInt32(sp[9])}");
                nev = sp[10];
                kerdesszam = Convert.ToInt32(sp[11]);

                kovkerdes = false;
                //+ p + ";" + mp + ";" + nev + ";" + kerdesszam;
            }
            
            while (kerdesszam <= MAXKERDES && nyer == true)
            {
                if (kovkerdes)
                {
                    if (kerdesszam == 0)
                    {
                        s = new Kerdes(true);
                    }
                    else
                    {
                        s = new Kerdes(false, kerdesszam);
                    }
                    kovkerdes = false;
                }

                string valasz = "";
                string alchars = "abcdkftmj";
                do
                {
                    Kijelzo(s);
                    valasz = Console.ReadLine();
                }
                while (valasz.Length == 0 || !alchars.Contains(valasz[0]));
                segitseg = "";
                if (valasz.ToLower() == s.Helyesvalasz.ToLower())
                {
                    ColorWrite(ConsoleColor.Yellow, "A válasz helyes.");
                    ColorWrite(ConsoleColor.Yellow, "Tovább ugorhatunk a következő kérdésre.");
                    
                    double totalsec = (int)(DateTime.Now.TimeOfDay - start).TotalSeconds;
                    double p = (int)TimeSpan.FromSeconds(totalsec).TotalMinutes;
                    double mp = totalsec - p * 60;

                    Console.WriteLine(p + " perc " + mp + " másodperc alatt");

                    System.Threading.Thread.Sleep(2000);
                    kerdesszam++;
                    kovkerdes = true;
                }
                else if(valasz.ToLower() == "f" && kerdesszam != 0)
                {
                    if(segitsegek.Contains("f")) segitseg = "f";
                }
                else if(valasz.ToLower() == "k" && kerdesszam != 0)
                {
                    if (segitsegek.Contains("k")) segitseg = "k";
                }

                else if(valasz.ToLower() == "m" && kerdesszam != 0)
                {
                    Console.WriteLine("A megállást választottad.");
                    
                    GameOver();

                    Console.WriteLine("Az enter megnyomásával visszaléphetsz a menübe.");
                    nyer = false;
                    Console.ReadLine();
                }

                else if(valasz.ToLower() == "j" && kerdesszam != 0)
                {
                    double totalsec = (int)(DateTime.Now.TimeOfDay - start).TotalSeconds;
                    double p = (int)TimeSpan.FromSeconds(totalsec).TotalMinutes;
                    double mp = totalsec - p * 60;
                    string ment = s.ToString() + ";" + p + ";" + mp + ";" + nev + ";" + kerdesszam;

                    string encryptedment = "";

                    for(int i = 0; i < ment.Length; i++)
                    {
                        encryptedment += Convert.ToChar(ment[i]-1);
                    }

                    File.WriteAllText("mentes.sav", encryptedment);

                    Console.WriteLine("Elmentetted az állást. Ha volt mentett állás, akkor azt felülírtad.");
                    Console.WriteLine("Nyomj entert a menübe való visszalépéshez.");
                    Console.ReadLine();
                    nyer = false;
                }

                else
                {
                    ColorWrite(ConsoleColor.Red, "A válasz helytelen.");
                    if (kerdesszam != 0)
                    {
                        Console.WriteLine("Eljutottál a(z) " + (kerdesszam - 1) + " kérdésig.");
                        GameOver();
                    }

                    Console.WriteLine("Az enter megnyomásával visszaléphetsz a menübe.");
                    Console.ReadLine();
                    nyer = false;
                }
            }
            if(!nyer)
            {

                Menu m = new Menu();
            }
            else
            {
                Console.Clear();
                GameOver();
                Console.WriteLine("Az enter megnyomásával visszaléphetsz a menübe.");
                Console.ReadLine();
                Console.WriteLine();
            }
        }

        public void GameOver()
        {
            int total = 0;
            if (kerdesszam > 5) total += 5 * 2000000;
            if (kerdesszam > 10) total += 5 * 2000000;
            if(kerdesszam > 15) total = 25000000;
            ColorWrite(ConsoleColor.Yellow, "A nyereményed " + total + " Ft!");

            double totalsec = (int)(DateTime.Now.TimeOfDay - start).TotalSeconds;
            double p = (int)TimeSpan.FromSeconds(totalsec).TotalMinutes;
            double mp = totalsec - p * 60;

            Console.WriteLine(p + " perc " + mp + " másodperc alatt");

            RankListWrite(nev, total, p, mp);
        }

        public void RankListWrite(string nev, int osszeg, double p, double mp)
        {
            string filestr = nev + " " + osszeg + " " + p + " " + mp + Environment.NewLine;
            File.AppendAllText("ranklist.txt", filestr);
            
            string[] file = File.ReadAllLines("ranklist.txt", Encoding.UTF8);
            var ordered = file.OrderByDescending(o => Convert.ToInt32(o.Split(' ')[1])).ThenBy(o => Convert.ToInt32(o.Split(' ')[2])).ThenBy(o => Convert.ToInt32(o.Split(' ')[3])).Take(10);

            filestr = "";
            foreach (var sor in ordered)
            {
                string[] sp = sor.Split(' ');
                filestr += $"{sp[0]} {sp[1]} {sp[2]} {sp[3]}" + Environment.NewLine;
            }

            File.WriteAllText("ranklist.txt", filestr);

        }

        private void ColorWrite(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void Kijelzo(Kerdes k)
        {
            Console.Clear();
            Console.WriteLine("Név: " + nev);
            ColorWrite(ConsoleColor.Cyan, (kerdesszam == 0 ? "Belépő" : kerdesszam + ".") + " kérdés");

            Console.WriteLine(k.Kategoria);
            Console.WriteLine();
            ColorWrite(ConsoleColor.White, k.Kerd);

            if (segitseg.Length != 0) //Segítséget kért
            {
                if (segitseg == "f") //Felezés
                {
                    string[] vlszk = k.Felezes();
                    for (int i = 0; i < vlszk.Length; i++)
                    {
                        char betu = Convert.ToChar((Convert.ToInt32('A') + i));
                        Console.WriteLine(vlszk[i]);
                    }
                }
                else if (segitseg == "k") // Közönség segítsége
                {
                    string[] vlszk = k.Kozonseg();
                    for (int i = 0; i < vlszk.Length; i++)
                    {
                        char betu = Convert.ToChar((Convert.ToInt32('A') + i));
                        Console.WriteLine(betu + "\t-\t" + vlszk[i]);
                    }
                }
                segitsegek = segitsegek.Replace(Convert.ToChar(segitseg), '-');
                //segitseg = "";
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    char betu = Convert.ToChar((Convert.ToInt32('A') + i));
                    Console.WriteLine(betu + "\t-\t" + k.Valaszok[i]);
                }
            }

            if (kerdesszam != 0)
            {
                Segits();
                Console.WriteLine("M\t-\tMegállok és elviszem a nyereményt");
                Console.WriteLine("J\t-\tJáték mentés, majd kilépés");
            }
            Console.WriteLine("Helyes válasz: " + k.Helyesvalasz);
            Console.Write("Add meg a helyes " + (kerdesszam == 0 ? "sorrendet: " : "választ: "));
        }

        private void Segits()
        {
            Console.WriteLine();
            if (segitsegek.Contains("k")) ColorWrite(ConsoleColor.Green, "K\t-\tKözönség segítsége");
            if (segitsegek.Contains("f")) ColorWrite(ConsoleColor.Green, "F\t-\tFelezés");
            if (segitsegek.Contains("t")) ColorWrite(ConsoleColor.Green, "T\t-\tTelefonos segítség");
            Console.WriteLine();

        }
    }
}

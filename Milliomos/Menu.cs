using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Milliomos
{
    class Menu
    {
        enum menu
        {
            start,
            load,
            ranks,
            info,
            quit
        }
        public Menu()
        {
            int v = 0;

            while (true)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Menü");
                    Menupont((int)menu.start, "Játék indítása");
                    Menupont((int)menu.load, "Betöltés");
                    Menupont((int)menu.ranks, "Dicsőséglista");
                    Menupont((int)menu.info, "Információ");
                    Menupont((int)menu.quit, "Kilépés");
                    Console.WriteLine();
                    Console.Write("Válassz a lehetőségek közül: ");

                    try
                    {
                        v = Convert.ToInt32(Console.ReadLine()) - 1;
                    }
                    catch(Exception)
                    {
                        v = -1;
                    }
                }
                while (v > (int)menu.quit || v < (int)menu.start);

                if (v == (int)menu.start)
                {
                    Jatek j = new Jatek();
                }
                else if(v == (int)menu.load)
                {
                    if (File.Exists("mentes.sav"))
                    {
                        Jatek j = new Jatek(true);
                    }
                    else
                    {
                        Console.WriteLine("Nincs elmentett állás.");
                        System.Threading.Thread.Sleep(2000);
                    }
                }
                else if (v == (int)menu.ranks)
                {
                    Console.Clear();
                    
                    if (!File.Exists("ranklist.txt"))
                    {
                        Console.WriteLine("Még nem játszott senki.");
                    }
                    else
                    {
                        string[] file = File.ReadAllLines("ranklist.txt", Encoding.UTF8);
                        Console.WriteLine("Helyezés\tNév\t\tNyeremény\tPerc\tMásodperc");

                        int i = 1;

                        foreach (var sor in file.OrderByDescending(o => Convert.ToInt32(o.Split(' ')[1])).ThenBy(o => Convert.ToInt32(o.Split(' ')[2])).ThenBy(o => Convert.ToInt32(o.Split(' ')[3])))
                        {
                            string[] sp = sor.Split(' ');
                            Console.WriteLine("{4,-16}{0,-16}{1,-16}{2,-8}{3,-8}", sp[0], sp[1], sp[2], sp[3], i++);
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("Az enter megnyomásával léphetsz vissza a menübe.");
                    Console.ReadLine();

                }
                else if(v == (int)menu.info)
                {
                    Console.Clear();
                    Console.WriteLine("A játék a következőképpen zajlik:");
                    Console.WriteLine("A játékot egy sorkérdéssel kezded, majd ha helyesen sorrendbe rakod a válaszokat, bekerülsz a játékba.");
                    Console.WriteLine("Ezután 15 kérdést kell megválaszolnod, melyekért egyre nagyobb nyereményt szerezhetsz.");
                    Console.WriteLine("Ha minden kérdést helyesen megválaszolsz, akkor 25 millió forintot nyersz.");
                    Console.WriteLine("Az 5. és a 10. kérdések megválaszolását követően biztos nyereménnyel fogsz távozni.");
                    Console.WriteLine();
                    Console.WriteLine("Az enter megnyomásával léphetsz vissza a menübe.");
                    Console.ReadLine();
                }
                else if (v == (int)menu.quit) Environment.Exit(0);
            }
        }

        private void Menupont(int index, string name)
        {
            Console.WriteLine($"\t{index+1}\t-\t{name}");
        }
    }
}

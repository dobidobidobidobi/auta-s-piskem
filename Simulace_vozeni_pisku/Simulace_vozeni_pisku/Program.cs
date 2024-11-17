using System.Collections;
using System.Collections.Generic;

namespace Simulace_vozeni_pisku
{
    class Program
    {
        static void Main(string[] args)
        {    
            //input zakladnich dat
            Console.WriteLine("Napište kolik tun písku mají auta převézt");
            Stav.zbyvajici_pisek = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Napište počet aut");
            int pocet_aut = Int32.Parse(Console.ReadLine());

            //input a seřazení aut
            Console.WriteLine("Napište vlastnosti jednotlivých aut ve tvaru: Nosnost DobaNaloze DobaCesty DobaVyloze");
            for (int jmeno_auta = 1; jmeno_auta <= pocet_aut; jmeno_auta++)
            {
                int[] auto = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                Car auticko = new Car( jmeno_auta, auto[0], auto[1],  auto[2], auto[3]);
                Serad_auto(auticko);
            }

            PriorityQueue<Udalost, int> kalendar = new PriorityQueue<Udalost, int>();
            Udalost prvniUdalost = new Udalost(Udalost.autaCekajiciNaNaloz[0], TypUdalosti.NalozZacat, 0).Proved();
            Udalost.autaCekajiciNaNaloz.RemoveAt(0);
            kalendar.Enqueue(prvniUdalost, prvniUdalost.cas);
           
            while (kalendar.Count > 0)
            {
                //pokud zadne auto nic nenakladá a existuje nevyuzite auto, zacne nakladat
                         
                if (Stav.zbyvajici_pisek == 0 && kalendar.Peek().typ == TypUdalosti.NalozZacat)
                {
                    kalendar.Dequeue();
                }
                //else if (Udalost.probihaNaloz == false && Udalost.autaCekajiciNaNaloz.Count != 0)
                //{
                //    kalendar.Enqueue(new Udalost(Udalost.autaCekajiciNaNaloz[0], TypUdalosti.NalozZacat, Stav.cas), Stav.cas);
                //    Udalost.autaCekajiciNaNaloz.RemoveAt(0);
                //}
                else
                {
                    Udalost NasledujiciUdalost = kalendar.Dequeue().Proved();
                    if (NasledujiciUdalost != null)
                    {
                        kalendar.Enqueue(NasledujiciUdalost, NasledujiciUdalost.cas);
                    }
                }
                if (Udalost.probihaNaloz == false && Udalost.autaCekajiciNaNaloz.Count != 0)
                {
                    kalendar.Enqueue(new Udalost(Udalost.autaCekajiciNaNaloz[0], TypUdalosti.NalozZacat, Stav.cas), Stav.cas);
                    Udalost.autaCekajiciNaNaloz.RemoveAt(0);
                }
            }
        }
        // Pro zajištění správné posloupnosti dle zadání
        public static void Serad_auto(Car auticko)
        {
            if (Udalost.autaCekajiciNaNaloz.Any())
            {
                int i = 0;
                while (Udalost.autaCekajiciNaNaloz[i].nosnost > auticko.nosnost && i < Udalost.autaCekajiciNaNaloz.Count)
                {
                    i++;
                }
                while (Udalost.autaCekajiciNaNaloz[i].nosnost == auticko.nosnost && Udalost.autaCekajiciNaNaloz[i].cesta < auticko.cesta && i < Udalost.autaCekajiciNaNaloz.Count)
                {
                    i++;
                }
                while (Udalost.autaCekajiciNaNaloz[i].nosnost == auticko.nosnost && Udalost.autaCekajiciNaNaloz[i].cesta == auticko.cesta && Udalost.autaCekajiciNaNaloz[i].nalozdoba < auticko.nalozdoba && i < Udalost.autaCekajiciNaNaloz.Count)
                {
                    i++;
                }
                while (Udalost.autaCekajiciNaNaloz[i].nosnost == auticko.nosnost && Udalost.autaCekajiciNaNaloz[i].cesta == auticko.cesta && Udalost.autaCekajiciNaNaloz[i].nalozdoba == auticko.nalozdoba && Udalost.autaCekajiciNaNaloz[i].nalozdoba < auticko.nalozdoba && i < Udalost.autaCekajiciNaNaloz.Count)
                {
                    i++;
                }

                if (i == Udalost.autaCekajiciNaNaloz.Count)
                {
                    Udalost.autaCekajiciNaNaloz.Add(auticko);
                }

                else
                {
                    Udalost.autaCekajiciNaNaloz.Insert(i,auticko);
                }
            }
            else
            {
                Udalost.autaCekajiciNaNaloz.Add(auticko);
            }
        }
    }
    class Car
    {
        public int jmeno { get;  }
        public int nosnost { get; }
        public int nalozdoba { get; }
        public int cesta { get; }
        public int vylozdoba { get; }

        public Car(int jmenoName, int nosnostNAME, int nalozdobaNAME, int cestaNAME, int vylozdobaNAME)
        {
            jmeno = jmenoName;
            nosnost = nosnostNAME;
            nalozdoba = nalozdobaNAME;
            cesta = cestaNAME;
            vylozdoba = vylozdobaNAME;
        }

    }
    public enum TypUdalosti { PrijezdDoM, PrijezdDoN, NalozZacat, /*VylozZacat,*/ Nalozeno, Vylozeno }

    class Udalost
    {

        public Car auto { get; }
        public TypUdalosti typ { get; }
        public int cas { get; }
        
        public static bool probihaNaloz = false;
        
        public static List<Car> autaCekajiciNaNaloz = new List<Car>();

        public Udalost(Car auticko, TypUdalosti typUdalosti, int c)
        { 
            auto = auticko;
            typ = typUdalosti;   
            cas = c;
        }
        // tuple s vecmi co pridame do kalendare --> udalost a v jaky cas
        // pro vetsi abstrakci by se dal zmenit na nejakou classu jako treba pristi udalost
        public  Udalost Proved()
        {
            Stav.cas = cas;

            switch(typ)
            {
                case TypUdalosti.PrijezdDoM:
                    Console.WriteLine("v case "  + Stav.cas + " auto " + auto.jmeno + " prijelo do M" + " a zacalo vykladat");
                    return new Udalost(auto, TypUdalosti.Vylozeno, auto.vylozdoba+Stav.cas);

                case TypUdalosti.PrijezdDoN:
                    Console.WriteLine("v case " + Stav.cas + " auto " + auto.jmeno + " prijelo do N");
                    Program.Serad_auto(auto);
                    return null;
                
                case TypUdalosti.NalozZacat:

                    Console.WriteLine("v case " + Stav.cas + " auto " + auto.jmeno + " zacalo nakladat");
                    Udalost.probihaNaloz = true;
                    return new Udalost(auto, TypUdalosti.Nalozeno, auto.nalozdoba + Stav.cas);
                    
                
                //case TypUdalosti.VylozZacat:
                //    Console.WriteLine("v case "  + Stav.cas + " auto " + auto.jmeno + " zacalo vykladat");
                //    return new Udalost(auto, TypUdalosti.Vylozeno, auto.vylozdoba+Stav.cas);

                case TypUdalosti.Nalozeno:
                    Udalost.probihaNaloz = false;
                    if (auto.nosnost<Stav.zbyvajici_pisek)
                    {
                        Stav.zbyvajici_pisek -= auto.nosnost;
                        Console.WriteLine("v case " + Stav.cas + " auto " + auto.jmeno + " nalozilo " + auto.nosnost + " tun pisku, zbyva " + Stav.zbyvajici_pisek + " tun pisku");
                    }
                    else
                    {
                        Console.WriteLine("v case " + Stav.cas + " auto " + auto.jmeno + " nalozilo " + Stav.zbyvajici_pisek + " tun pisku, zbyva 0 tun pisku");
                        Stav.zbyvajici_pisek = 0;
                    }
                    return new Udalost(auto, TypUdalosti.PrijezdDoM, auto.cesta + Stav.cas);

                case TypUdalosti.Vylozeno:
                    Console.WriteLine("v case " + Stav.cas + " auto " + auto.jmeno + " vylozilo");
                    return new Udalost(auto, TypUdalosti.PrijezdDoN, auto.cesta + Stav.cas);
                default:
                    Console.WriteLine("Udalost.Proved() nefunguje");
                    return null;
            }
        }
    }

    //bude vyjadrovat jaky je cas pri dane udalosti a kolik písku zbyvá převézt
    static class Stav
    {
        public static int cas = 0;
        public static int zbyvajici_pisek;
    }

}

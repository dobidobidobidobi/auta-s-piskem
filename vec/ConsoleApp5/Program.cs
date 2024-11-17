using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Udalost.probihaNaloz = false;
            PriorityQueue<Udalost, int> kalendar = new PriorityQueue<Udalost, int>();
     
            //input zakladnich dat
            Console.WriteLine("Napište kolik tun písku mají auta převézt");
            Stav.zbyvajici_pisek = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Napište počet aut");
            int pocet_aut = Int32.Parse(Console.ReadLine());

            //input aut do Queue
            Console.WriteLine("Napište vlastnosti jednotlivých aut ve tvaru: Nosnost DobaNaloze DobaCesty DobaVyloze");
            for (int jmeno_auta = 1; jmeno_auta <= pocet_aut; jmeno_auta++)
            {
                int[] auto = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                Car auticko = new Car( jmeno_auta, auto[0], auto[1],  auto[2], auto[3]);
                Udalost.autaCekajiciNaNaloz.Enqueue(auticko);
            }

            Udalost prvniUdalost = new Udalost(Udalost.autaCekajiciNaNaloz.Dequeue(), TypUdalosti.NalozZacat, 0).Proved();
            kalendar.Enqueue(prvniUdalost, prvniUdalost.cas);
           
            while (kalendar.Count > 0)
            {
                //pokud zadne auto nic nenakladá a existuje nevyuzite auto, zacne nakladat
                //to jak je smycka serazena take znamena, ze simulace pri nakladani uprednostnuje auta, ktera jeste nic nedelala
                //da se to pripadne zmenit
                //bool probehla prvni udalost je kvuli spatne implemetaci Udalost.konec_naloze, jinak by to na zacatku nacetlo vsechna auta 
                Console.WriteLine(Udalost.probihaNaloz);
              
                if (Stav.zbyvajici_pisek == 0 && kalendar.Peek().typ == TypUdalosti.NalozZacat)
                {
                    kalendar.Dequeue();
                }
                else if (Udalost.probihaNaloz == false && Stav.zbyvajici_pisek > 0 && Udalost.autaCekajiciNaNaloz.Count != 0)
                {
                    kalendar.Enqueue(new Udalost(Udalost.autaCekajiciNaNaloz.Dequeue(), TypUdalosti.NalozZacat, Stav.cas), Stav.cas);
                }
                else
                {
                    Udalost NasledujiciUdalost = kalendar.Dequeue().Proved();
                    if (NasledujiciUdalost != null)
                    {

                    }
                }
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
    public enum TypUdalosti { PrijezdDoM, PrijezdDoN, NalozZacat, VylozZacat, Nalozeno, Vylozeno }

    class Udalost
    {

        public Car auto { get; }
        public TypUdalosti typ { get; }
        public int cas { get; }
        //kdyz je 0 => neprobiha naloz, jinak vyjadruje cas konce probihajici naloze
        public static bool probihaNaloz;

        public static Queue<Car> autaCekajiciNaNaloz = new Queue<Car>();

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
                    Console.WriteLine("v case "  + Stav.cas + " auto " + auto.jmeno + " prijelo do M");
                    return new Udalost(auto, TypUdalosti.VylozZacat, Stav.cas);

                case TypUdalosti.PrijezdDoN:
                    Console.WriteLine("v case " + Stav.cas + " auto " + auto.jmeno + " prijelo do N");
                    return new Udalost(auto, TypUdalosti.NalozZacat, Stav.cas);
                
                case TypUdalosti.NalozZacat:
                    if (Udalost.probihaNaloz==false)
                    {
                        Console.WriteLine("v case " +Stav.cas + " auto " + auto.jmeno + " zacalo nakladat");
                        Udalost.probihaNaloz = true;
                        return new Udalost(auto, TypUdalosti.Nalozeno, auto.nalozdoba + Stav.cas);
                    }
                    else
                    {
                        autaCekajiciNaNaloz.Enqueue(auto);
                        return null; 
                    }
                
                case TypUdalosti.VylozZacat:
                    Console.WriteLine("v case "  + Stav.cas + " auto " + auto.jmeno + " zacalo vykladat");
                    return new Udalost(auto, TypUdalosti.Vylozeno, auto.vylozdoba+Stav.cas);

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

using System.Collections;

namespace ConsoleApp5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Naloz.cas = 0;
            PriorityQueue<Udalost, int> kalendar = new PriorityQueue<Udalost, int>();
            // auta jsou potreba jenom jednou a poté budou v kalendari
            //kvuli tomu fronta --> lepsi casova slozitost nez list
            Queue<Car> auta = new Queue<Car>();

            //input zakladnich dat
            Console.WriteLine("Napište kolik tun písku mají auta převézt");
            Stav.zbyvajici_pisek = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Napište počet aut");
            int pocet_aut = Int32.Parse(Console.ReadLine());

            //input aut do listu
            Console.WriteLine("Napište vlastnosti jednotlivých aut ve tvaru: Nosnost DobaNaloze DobaCesty DobaVyloze");
            for (int jmeno_auta = 1; jmeno_auta <= pocet_aut; jmeno_auta++)
            {
                int[] auto = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                Car auticko = new Car( jmeno_auta, auto[0], auto[1],  auto[2], auto[3]);
                auta.Enqueue(auticko);
            }

            kalendar.Enqueue(new Udalost(auta.Dequeue(), TypUdalosti.NalozZacat), 0);
           
            while (kalendar.Count > 0)
            {
                if (Naloz.cas==0 && auta.Count > 0 && Stav.zbyvajici_pisek > 0)
                {
                    kalendar.Enqueue(new Udalost(auta.Dequeue(), TypUdalosti.NalozZacat), 0);
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
    //je jako samostatna classa => myslím, že musí být mimo Udalost
    public enum TypUdalosti { PrijezdDoM, PrijezdDoN, NalozZacat, VylozZacat, Nalozeno, Vylozeno }

    class Udalost
    {
        public Car auto { get; }
        public TypUdalosti typ { get; }

        public Udalost(Car auticko, TypUdalosti typUdalosti)
        { 
            auto = auticko;
            typ = typUdalosti;   
        }
        //vraci navazujici udalost dale cas, ktery se bude pricitat a nakonec kolik písku tato akce odebrala(vetsinou 0)
        // ty dva int by se pro lepsi prehlednost dali predelat na jednotlivou classu --> treba class zmena
        public Tuple<Udalost, int, int> Proved()
        {
            switch(typ)
            {
                case TypUdalosti.PrijezdDoM:
                    Console.WriteLine("v case" + Stav.cas + " auto " + auto.jmeno + " prijelo do M");
                    return new Tuple<Udalost, int, int>(new Udalost(auto, TypUdalosti.VylozZacat), 0 , 0);

                case TypUdalosti.PrijezdDoN:
                    Console.WriteLine("v case" + Stav.cas + " auto " + auto.jmeno + " prijelo do N");
                    return new Tuple<Udalost, int, int>(new Udalost(auto, TypUdalosti.NalozZacat), 0, 0);
                
                case TypUdalosti.NalozZacat:
                    if (Naloz.cas==0)
                    {
                        Console.WriteLine("v case" +Stav.cas + " auto " + auto.jmeno + " zacalo nakladat");
                        Naloz.cas = auto.nalozdoba;
                        return new Tuple<Udalost, int, int>(new Udalost(auto, TypUdalosti.Nalozeno), auto.nalozdoba, auto.nosnost);
                    }
                    else
                    {
                       return new Tuple<Udalost, int, int>(new Udalost(auto, typ), Naloz.cas, 0); 
                    }
                
                case TypUdalosti.VylozZacat:
                    Console.WriteLine("v case" + Stav.cas + " auto " + auto.jmeno + " zacalo vykladat");
                    return new Tuple<Udalost, int, int>(new Udalost(auto, TypUdalosti.Vylozeno), auto.vylozdoba, 0);

                case TypUdalosti.Nalozeno:
                    Naloz.cas = 0;
                    if (auto.nosnost>=Stav.zbyvajici_pisek)
                    {
                        Stav.zbyvajici_pisek -= auto.nosnost;
                        Console.WriteLine("v case" + Stav.cas + " auto " + auto.jmeno + " nalozilo " + auto.nosnost + " tun pisku, zbyva " + Stav.zbyvajici_pisek + " tun pisku");
                    }
                    else
                    {
                        Console.WriteLine("v case" + Stav.cas + "auto " + auto.jmeno + " nalozilo " + Stav.zbyvajici_pisek + " tun pisku, zbyva 0 tun pisku");
                        Stav.zbyvajici_pisek = 0;
                    }
                    return new Tuple<Udalost, int, int>(new Udalost(auto, TypUdalosti.PrijezdDoM), auto.cesta, 0);
                case TypUdalosti.Vylozeno:
                    Console.WriteLine("v case" + Stav.cas + " auto " + auto.jmeno + " vylozilo");
                    return new Tuple<Udalost, int, int>(new Udalost(auto, TypUdalosti.PrijezdDoN), auto.cesta, 0);
                default:
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
    //globalni variable -_-
    static class Naloz
    {
        public static int cas;
    }
}

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
            bool probehlaPrvniUdalost = false;
           
            while (kalendar.Count > 0)
            {
                //pokud zadne auto nic nenakladá a existuje nevyuzite auto zacne nakladat
                //to jak je smycka serazena take znamena, ze simulace pri nakladani uprednostnuje auta, ktera jeste nic nedelala
                //da se to pripadne zmenit
                //bool probehla prvni udalost je kvuli spatne implemetaci Naloz.cas, jinak by to na zacatku nacetlo vsechna auta 
                if (Naloz.cas==0 && auta.Count > 0 && Stav.zbyvajici_pisek > 0 && probehlaPrvniUdalost==true)
                {
                    kalendar.Enqueue(new Udalost(auta.Dequeue(), TypUdalosti.NalozZacat), Stav.cas);
                }
                //kdyz neni pisek nezacne dalsi smycka udalosti
                else if (Stav.zbyvajici_pisek <= 0 && kalendar.Peek().typ == TypUdalosti.NalozZacat)
                { 
                    kalendar.Dequeue();
                }
                else
                {
                    //Ziska element v halde a podle ceho halda radi, coz je pro nas cas 
                    kalendar.TryDequeue(out Udalost momentalni_udalost, out int cas);
                    
                    Stav.cas = cas;
                    Tuple<Udalost, int> dalsi_udalost = momentalni_udalost.Proved();

                    kalendar.Enqueue(dalsi_udalost.Item1, dalsi_udalost.Item2);
                    probehlaPrvniUdalost = true;
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
        // tuple s vecmi co pridame do kalendare --> udalost a v jaky cas
        // pro vetsi abstrakci by se dal zmenit na nejakou classu jako treba pristi udalost
        public Tuple<Udalost, int> Proved()
        {
            switch(typ)
            {
                case TypUdalosti.PrijezdDoM:
                    Console.WriteLine("v case "  + Stav.cas + " auto " + auto.jmeno + " prijelo do M");
                    return new Tuple<Udalost, int>(new Udalost(auto, TypUdalosti.VylozZacat), Stav.cas);

                case TypUdalosti.PrijezdDoN:
                    Console.WriteLine("v case " + Stav.cas + " auto " + auto.jmeno + " prijelo do N");
                    return new Tuple<Udalost, int>(new Udalost(auto, TypUdalosti.NalozZacat), Stav.cas);
                
                case TypUdalosti.NalozZacat:
                    if (Naloz.cas==0)
                    {
                        Console.WriteLine("v case " +Stav.cas + " auto " + auto.jmeno + " zacalo nakladat");
                        Naloz.cas = auto.nalozdoba + Stav.cas;
                        return new Tuple<Udalost, int>(new Udalost(auto, TypUdalosti.Nalozeno), auto.nalozdoba + Stav.cas);
                    }
                    else
                    {
                        //naloz.cas musi byt + 1 jelikoz jinak by se halda mohla zacyklit
                        return new Tuple<Udalost, int>(new Udalost(auto, typ), Naloz.cas + 1); 
                    }
                
                case TypUdalosti.VylozZacat:
                    Console.WriteLine("v case "  + Stav.cas + " auto " + auto.jmeno + " zacalo vykladat");
                    return new Tuple<Udalost, int>(new Udalost(auto, TypUdalosti.Vylozeno), auto.vylozdoba+Stav.cas);

                case TypUdalosti.Nalozeno:
                    Naloz.cas = 0;
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
                    return new Tuple<Udalost, int>(new Udalost(auto, TypUdalosti.PrijezdDoM), auto.cesta + Stav.cas);

                case TypUdalosti.Vylozeno:
                    Console.WriteLine("v case " + Stav.cas + " auto " + auto.jmeno + " vylozilo");
                    return new Tuple<Udalost, int>(new Udalost(auto, TypUdalosti.PrijezdDoN), auto.cesta + Stav.cas);
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
    //globalni variable -_-
    static class Naloz
    {
        public static int cas;
    }
}

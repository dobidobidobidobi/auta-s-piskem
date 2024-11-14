namespace ConsoleApp5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Naloz.cas = 0;
            PriorityQueue<Udalost, int> kalendar = new PriorityQueue<Udalost, int>();
            List<Car> auta = new List<Car>();

            Console.WriteLine("Napište kolik tun písku mají auta převézt");
            int pisek = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Napište počet aut");
            int pocet_aut = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Napište vlastnosti jednotlivých aut ve tvaru: Nosnost DobaNaloze DobaCesty DobaVyloze");

            //input aut do listu
            for (int jmeno_auta = 1; jmeno_auta <= pocet_aut; jmeno_auta++)
            {
                int[] auto = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                Car auticko = new Car( jmeno_auta, auto[0], auto[1],  auto[2], auto[3]);
                auta.Add(auticko);
            }
            
            
            while (kalendar.Count > 0);
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
        //prvni je dana udalost pote cas, ktery se bude pricitat a nakonec kolik písku tato akce odebrala(vetsinou 0)
        public Tuple<Udalost, int, int> Proved()
        {
            switch(typ)
            {
                case TypUdalosti.PrijezdDoM:
                    return new Tuple<Udalost, int, int>(new Udalost(auto, TypUdalosti.VylozZacat), 0 , 0);
                case TypUdalosti.PrijezdDoN:
                    return 
                case TypUdalosti.NalozZacat:
                    if (Naloz.cas==0)
                    {
                        Naloz.cas = auto.nalozdoba;
                        return new Tuple<Udalost, int, int>(new Udalost(auto, TypUdalosti.Nalozeno), auto.nalozdoba, auto.nosnost);
                    }
                    else
                    {
                       return new Tuple<Udalost, int, int>(new Udalost(auto, typ), Naloz.cas, 0); 
                    }
                case TypUdalosti.VylozZacat:
                    return new Tuple<Udalost, int, int>(new Udalost(auto, TypUdalosti.Vylozeno), auto.vylozdoba, 0);
                case TypUdalosti.Nalozeno:
                    return new Tuple<Udalost, int, int>(new Udalost(auto, TypUdalosti.PrijezdDoM), auto.cesta, 0);
                case TypUdalosti.Vylozeno:
                    return null;
                default:
                    return null;
            }
        }
            

        


    }
    //globalni variable -_-
    static class Naloz
    {
        public static int cas;
    }
}

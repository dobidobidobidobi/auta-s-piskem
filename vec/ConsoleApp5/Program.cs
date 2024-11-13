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


            for (int jmeno_auta = 1; jmeno_auta <= pocet_aut; jmeno_auta++)
            {
                int[] auto = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                Car auticko = new Car( jmeno_auta, auto[0], auto[1],  auto[2], auto[3]);
                auta.Add(auticko);
            }
            foreach (Car aut in auta)
            {
                Console.WriteLine(aut.jmeno);
            }

            while (kalendar.Count > 0 || pisek>0);
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

    class Udalost
    {
        public Car auto { get; }
        enum TypUdalosti { PrijezdDoM, PrijezdDoN, NalozZacat, VylozZacat, Nalozeno, Vylozeno }
        TypUdalosti udalost { get; }

        public Udalost(Car auticko, int Typ_udalosti)
        {
            auto = auticko;
            udalost = Typ_Udalosti;
        }
        


    }
    //globalni variable -_-
    static class Naloz
    {
        public static int cas;
    }
}

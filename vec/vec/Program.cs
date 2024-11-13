using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class Car
    {
        int jmeno {  get;  }
        int nosnost { get; }
        int nalozdoba { get; }
        int cesta { get; }
        int vylozdoba { get; }

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
        Car auto;
        enum TypUdalosti {PrijezdDoM, PrijezdDoN, NalozZacat, VylozZacat, Nalozeno, Vylozeno };
        TypUdalosti udalost { get; }
        

    }
    
    static class Naloz
    {
        public static int cas;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Naloz.cas = 0;
            PriorityQueue<Udalost,int> kalendar = new PriorityQueue<Udalost,int>();
            List<Car> auta = new List<Car>();

            Console.WriteLine("Napište kolik tun písku mají auta převézt")
            int pisek = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Napište počet aut")
            int pocet_aut = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Napište vlastnosti jednotlivých aut ve tvaru: Nosnost DobaNaloze DobaCesty DobaVyloze");


            for (int jmeno_auta = 1; jmeno_auta <= pocet_aut; jmeno_auta++)
            {
                int[] auto = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                Car auticko = new Car(int jmeno_auta, int[] auto[0], int[] auto[1], int[] auto[2], int[] auto[3]);
                auta.Add(Car auticko);
            }
            foreach (Car aut in auta)
            {
                Console.WriteLine(Car aut.jmeno);
            }
        }
    }
}

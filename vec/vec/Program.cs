﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class Car
    {
        int jmeno { get; }
        int nosnost { get; }
        int nalozdoba { get; }
        int cesta { get; }
        int vylozdoba { get; }
    }

    class Udalost
    {
        Car auto;
        enum TypUdalosti { };
        TypUdalosti udalost { get; }
    }

    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}

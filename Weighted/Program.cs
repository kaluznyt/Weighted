using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Weighted
{
    class Program
    {


        static void Main(string[] args)
        {
            var union = new WeightedQuickUnionUFWrapper(120);

            //union.open(0, 0);
            //union.open(1, 0);
            //union.open(2, 0);
            //union.open(3, 0);
            //union.open(4, 0);
            //union.open(5, 0);
            //union.open(6, 0);

            //union.open(6, 1);
            //union.open(7, 1);
            //union.open(8, 1);

            //union.open(8, 0);
            //union.open(9, 0);

            union.StartSimulation();

            Console.ReadKey();
        }
    }

    
}

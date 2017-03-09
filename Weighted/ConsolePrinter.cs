using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weighted
{
    public class ConsolePrinter : IPrinter
    {
        private WeightedQuickUnionUFWrapper _algo;


        #region Console Printers
        private void PrintStatistics(bool cls = false)
        {
            if (cls) Console.Clear();

            Console.WriteLine("Percolates ? " + _algo.Percolates());
            Console.WriteLine("Number of compontents: " + _algo.ComponentsCount);

            Console.WriteLine(
                "Sites: {0} / Opened: {1} / Closed: {2}",
                _algo.SitesCount,
                _algo.OpenedSitesCount,
                _algo.ClosedSitesCount);
        }

        public void PrintLargeSiteOverview()
        {
            Console.Clear();

            for (int i = 0; i < _algo.N; i++)
            {
                for (int x = 0; x < _algo.N; x++)
                {
                    if (x == 0) Console.Write(i.ToString().PadLeft(3) + ": ");

                    if (_algo.IsInPercolationPath(i * _algo.N + x))
                    {
                        var c = i < _algo.N * 0.33 ? ConsoleColor.Yellow : 
                                         i < _algo.N * 0.66 ? ConsoleColor.Red : 
                                         ConsoleColor.DarkRed;

                        //Console.BackgroundColor = (ConsoleColor)new Random(new object().GetHashCode()).Next(12, 15);
                        PrintColoredSpace(c);
                    }
                    //else if (_sites[i, x] == true)
                    else if (_algo.IsSiteOpen(i, x))
                    {
                        //Console.BackgroundColor = (ConsoleColor)new Random(new object().GetHashCode()).Next(1, 3);
                        PrintColoredSpace(ConsoleColor.DarkCyan);
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }

            PrintStatistics();
        }

        private void PrintColoredSpace(ConsoleColor color = ConsoleColor.Black)
        {
            Console.BackgroundColor = color;
            Console.Write(" ");
            Console.ResetColor();
        }

        public void SetAlgorithm(WeightedQuickUnionUFWrapper algo)
        {
            _algo = algo;
        }

        //private void PrintOpenSites()
        //{
        //    Console.Clear();

        //    for (int i = 0; i < N; i++)
        //    {
        //        Console.Write("    {0}", i);
        //    }

        //    Console.WriteLine();

        //    for (int i = 0; i < N; i++)
        //    {
        //        for (int x = 0; x < N; x++)
        //        {
        //            if (x == 0) Console.Write(i + ":");
        //            Console.Write(_sites[i, x] == true ? " [x] " : " [ ] ");
        //        }

        //        Console.WriteLine();
        //    }

        //    Console.WriteLine("Number of compontents: " + _weightedUnionUF.count);
        //    Console.WriteLine("Percolates ? " + Percolates());
        //}

        #endregion Console Printers
    }
}

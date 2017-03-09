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
            var union = new WeightedQuickUnionUFWrapper(150);

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

            union.openRandomSite();

            while (!union.percolates())
            {
                union.openRandomSite();

            }

            union.printLargeSiteOverview();
            //union.printStatistics();
            Console.ReadKey();
            //union.printOpenSites();
        }
    }

    public class WeightedQuickUnionUFWrapper
    {
        private WeightedQuickUnionUF _weightedUnionUF;

        private bool[,] _sites;

        private int openedSites = 0;

        private int N;

        private int _virtualTopSite;

        private int _virtualBottomSite;

        public WeightedQuickUnionUFWrapper(int n)
        {
            N = n;
            _sites = new bool[n, n];
            int len = n * n;
            _virtualTopSite = len;
            _virtualBottomSite = len + 1;
            _weightedUnionUF = new WeightedQuickUnionUF(n * n + 2);
        }

        private int valueAt(int row, int col)
        {
            return _weightedUnionUF.find(row * N + col);
        }

        public void openRandomSite()
        {
            open(new Random(Guid.NewGuid().GetHashCode()).Next(0, N), new Random(Guid.NewGuid().GetHashCode()).Next(0, N));
        }
        public void open(int row, int col)
        {
            if (!_sites[row, col])
            {
                _sites[row, col] = true;
                openedSites++;
            }

            union(row, col, Position.Left);
            union(row, col, Position.Right);
            union(row, col, Position.Top);
            union(row, col, Position.Bottom);

        }

        enum Position
        {
            Top, Bottom, Left, Right
        }

        private bool checkOpen(int row, int col)
        {
            if (row < 0 || row >= N || col < 0 || col >= N) return false;

            return _sites[row, col];
        }

        public bool percolates()
        {
            return _weightedUnionUF.connected(_virtualTopSite, _virtualBottomSite);
        }

        private void union(int row, int col, Position position)
        {
            int relRow = row;
            int relCol = col;

            switch (position)
            {
                case Position.Top:
                    relRow--;
                    if (relRow == -1)
                    {
                        var relSite = _weightedUnionUF.find(_virtualTopSite);
                        var current = _weightedUnionUF.find(valueAt(row, col));

                        _weightedUnionUF.union(relSite, current);
                        return;
                    }
                    break;
                case Position.Bottom:
                    relRow++;
                    if (relRow == N)
                    {
                        var relSite = _weightedUnionUF.find(_virtualBottomSite);
                        var current = _weightedUnionUF.find(valueAt(row, col));

                        _weightedUnionUF.union(relSite, current);
                        return;
                    }
                    break;
                case Position.Left:
                    relCol++;
                    break;
                case Position.Right:
                    relCol--;
                    break;
            }

            if (checkOpen(relRow, relCol))
            {
                var relSite = _weightedUnionUF.find(valueAt(relRow, relCol));
                var current = _weightedUnionUF.find(valueAt(row, col));

                _weightedUnionUF.union(relSite, current);
            }
        }

        public void printStatistics(bool cls = false)
        {
            if (cls)
                Console.Clear();
            Console.WriteLine("Percolates ? " + percolates());
            Console.WriteLine("Number of compontents: " + _weightedUnionUF.count);

            Console.WriteLine("Sites: {0} / Opened: {1} / Closed: {2}", _sites.Length, openedSites, _sites.Length - openedSites);

        }

        public void printLargeSiteOverview()
        {
            Console.Clear();

            for (int i = 0; i < N; i++)
            {
                for (int x = 0; x < N; x++)
                {
                    if (x == 0) Console.Write(i.ToString().PadLeft(3) + ": ");

                    if (_weightedUnionUF.connected(_weightedUnionUF.parent[i * N + x], _virtualTopSite))
                    {
                        //Console.BackgroundColor = (ConsoleColor)new Random(new object().GetHashCode()).Next(12, 15);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write(" ");
                        Console.ResetColor();
                    }
                    else if (_sites[i, x] == true)
                    {
                        //Console.BackgroundColor = (ConsoleColor)new Random(new object().GetHashCode()).Next(1, 3);
                        Console.BackgroundColor = ConsoleColor.DarkBlue;

                        Console.Write(" ");
                        Console.ResetColor();

                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }

            printStatistics();
        }

        public void printOpenSites()
        {
            Console.Clear();
            for (int i = 0; i < N; i++)
            {
                Console.Write("    {0}", i);
            }

            Console.WriteLine();

            for (int i = 0; i < N; i++)
            {
                for (int x = 0; x < N; x++)
                {
                    if (x == 0) Console.Write(i + ":");
                    Console.Write(_sites[i, x] == true ? " [x] " : " [ ] ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Number of compontents: " + _weightedUnionUF.count);
            Console.WriteLine("Percolates ? " + percolates());
        }
    }
}

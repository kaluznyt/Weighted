using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weighted
{
    public class WeightedQuickUnionUFWrapper
    {
        private enum SiteRelativePosition
        {
            Top, Bottom, Left, Right
        }

        private readonly WeightedQuickUnionUF _weightedUnionUF;

        private readonly bool[,] _sites;

        private int _openedSites = 0;

        private readonly int N;

        private readonly int _virtualTopSite;

        private readonly int _virtualBottomSite;

        public WeightedQuickUnionUFWrapper(int n)
        {
            N = n;
            _sites = new bool[n, n];

            var len = n * n;
            _virtualTopSite = len;
            _virtualBottomSite = len + 1;

            _weightedUnionUF = new WeightedQuickUnionUF(len + 2);
        }

        private int ValueAt(int row, int col)
        {
            if (row == -1) return _virtualTopSite;
            else if (row == N) return _virtualBottomSite;
            else return _weightedUnionUF.find(row * N + col);
        }

        public void OpenRandomSite()
        {
            OpenSite(
                new Random(Guid.NewGuid().GetHashCode()).Next(0, N),
                new Random(Guid.NewGuid().GetHashCode()).Next(0, N));
        }

        public void OpenSite(int row, int col)
        {
            if (!_sites[row, col])
            {
                _sites[row, col] = true;
                _openedSites++;

                Union(row, col, SiteRelativePosition.Left);
                Union(row, col, SiteRelativePosition.Right);
                Union(row, col, SiteRelativePosition.Top);
                Union(row, col, SiteRelativePosition.Bottom);
            }
        }

        private bool IsSiteOpen(int row, int col)
        {
            // 'virtual' sites always open
            if (row == -1 || row == N) return true;

            if (row < 0 || row >= N || col < 0 || col >= N) return false;

            return _sites[row, col];
        }

        public bool Percolates()
        {
            return _weightedUnionUF.connected(_virtualTopSite, _virtualBottomSite);
        }

        private void Union(int row, int col, SiteRelativePosition siteRelativePosition)
        {
            int relRow = siteRelativePosition == SiteRelativePosition.Top ? row - 1 :
                          siteRelativePosition == SiteRelativePosition.Bottom ? row + 1 : row;

            int relCol = siteRelativePosition == SiteRelativePosition.Left ? col - 1 :
                          siteRelativePosition == SiteRelativePosition.Right ? col + 1 : col;

            if (IsSiteOpen(relRow, relCol))
            {
                var relSite = _weightedUnionUF.find(ValueAt(relRow, relCol));
                var current = _weightedUnionUF.find(ValueAt(row, col));

                _weightedUnionUF.union(relSite, current);
            }
        }

        public void StartSimulation()
        {
            while (!Percolates())
            {
                OpenRandomSite();
            }

            PrintLargeSiteOverview();
        }

        #region Console Printers
        private void PrintStatistics(bool cls = false)
        {
            if (cls) Console.Clear();
            Console.WriteLine("Percolates ? " + Percolates());
            Console.WriteLine("Number of compontents: " + _weightedUnionUF.count);

            Console.WriteLine(
                "Sites: {0} / Opened: {1} / Closed: {2}",
                _sites.Length,
                _openedSites,
                _sites.Length - _openedSites);
        }

        private void PrintLargeSiteOverview()
        {
            Console.Clear();

            for (int i = 0; i < N; i++)
            {
                for (int x = 0; x < N; x++)
                {
                    if (x == 0) Console.Write(i.ToString().PadLeft(3) + ": ");

                    if (_weightedUnionUF.connected(_weightedUnionUF.parent[i * N + x], _virtualTopSite))
                    {
                        ConsoleColor c = i < N * 0.3 ? ConsoleColor.Yellow : i < N * 0.5 ? ConsoleColor.Red : ConsoleColor.DarkRed;
                        //Console.BackgroundColor = (ConsoleColor)new Random(new object().GetHashCode()).Next(12, 15);
                        PrintColorSpace(c);
                    }
                    else if (_sites[i, x] == true)
                    {
                        //Console.BackgroundColor = (ConsoleColor)new Random(new object().GetHashCode()).Next(1, 3);
                        PrintColorSpace(ConsoleColor.DarkCyan);
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

        private void PrintColorSpace(ConsoleColor color = ConsoleColor.Black)
        {
            Console.BackgroundColor = color;
            Console.Write(" ");
            Console.ResetColor();
        }

        private void PrintOpenSites()
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
            Console.WriteLine("Percolates ? " + Percolates());
        }

        #endregion Console Printers
    }
}

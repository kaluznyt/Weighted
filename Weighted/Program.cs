using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weighted
{
    class Program
    {
        static void Main(string[] args)
        {
            var union = new WeightedQuickUnionUFWrapper(100);

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
            //    union.printOpenSites();
            }

            union.printOpenSites();
        }
    }

    public class WeightedQuickUnionUFWrapper
    {
        private WeightedQuickUnionUF _weightedUnionUF;

        private bool[,] _sites;

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
            open(new Random(Guid.NewGuid().GetHashCode()).Next(0, N ), new Random(Guid.NewGuid().GetHashCode()).Next(0, N ));
        }
        public void open(int row, int col)
        {
            _sites[row, col] = true;
            union(row, col, Position.Top);
            union(row, col, Position.Bottom);
            union(row, col, Position.Left);
            union(row, col, Position.Right);
        }

        private bool checkOpen(int row, int col)
        {
            if (row < 0 || row >= N || col < 0 || col >= N) return false;

            return _sites[row, col];
        }

        enum Position
        {
            Top, Bottom, Left, Right
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

        //private void unionBottom(int row, int col)
        //{
        //    if (checkOpen(row + 1, col))
        //    {
        //        var siteTop = _weightedUnionUF.find(valueAt(row + 1, col));
        //        var current = _weightedUnionUF.find(valueAt(row, col));

        //        _weightedUnionUF.union(siteTop, current);
        //    }
        //}

        //private void unionRight(int row, int col)
        //{
        //    if (checkOpen(row, col + 1))
        //    {
        //        var siteTop = _weightedUnionUF.find(valueAt(row, col + 1));
        //        var current = _weightedUnionUF.find(valueAt(row, col));

        //        _weightedUnionUF.union(siteTop, current);
        //    }
        //}

        //private void unionLeft(int row, int col)
        //{
        //    if (checkOpen(row, col - 1))
        //    {
        //        var siteTop = _weightedUnionUF.find(valueAt(row, col - 1));
        //        var current = _weightedUnionUF.find(valueAt(row, col));

        //        _weightedUnionUF.union(siteTop, current);
        //    }
        //}

        public class WeightedQuickUnionUF
        {
            private int[] parent; // parent[i] = parent of i

            private int[] size; // size[i] = number of sites in subtree rooted at i

            public int count; // number of components

            /**
             * Initializes an empty union–find data structure with {@code n} sites
             * {@code 0} through {@code n-1}. Each site is initially in its own 
             * component.
             *
             * @param  n the number of sites
             * @throws IllegalArgumentException if {@code n < 0}
             */

            public WeightedQuickUnionUF(int n)
            {
                count = n;
                parent = new int[n];
                size = new int[n];
                for (int i = 0; i < n; i++)
                {
                    parent[i] = i;
                    size[i] = 1;
                }
            }

            /**
             * Returns the number of components.
             *
             * @return the number of components (between {@code 1} and {@code n})
             */

            /**
             * Returns the component identifier for the component containing site {@code p}.
             *
             * @param  p the integer representing one object
             * @return the component identifier for the component containing site {@code p}
             * @throws IndexOutOfBoundsException unless {@code 0 <= p < n}
             */

            public int find(int p)
            {
                validate(p);
                while (p != parent[p]) p = parent[p];
                return p;
            }

            // validate that p is a valid index
            private void validate(int p)
            {
                int n = parent.Length;
                if (p < 0 || p >= n)
                {
                    throw new IndexOutOfRangeException();
                }
            }

            /**
             * Returns true if the the two sites are in the same component.
             *
             * @param  p the integer representing one site
             * @param  q the integer representing the other site
             * @return {@code true} if the two sites {@code p} and {@code q} are in the same component;
             *         {@code false} otherwise
             * @throws IndexOutOfBoundsException unless
             *         both {@code 0 <= p < n} and {@code 0 <= q < n}
             */

            public bool connected(int p, int q)
            {
                return find(p) == find(q);
            }

            /**
             * Merges the component containing site {@code p} with the 
             * the component containing site {@code q}.
             *
             * @param  p the integer representing one site
             * @param  q the integer representing the other site
             * @throws IndexOutOfBoundsException unless
             *         both {@code 0 <= p < n} and {@code 0 <= q < n}
             */

            public void union(int p, int q)
            {
                int rootP = find(p);
                int rootQ = find(q);
                if (rootP == rootQ) return;

                // make smaller root point to larger one
                if (size[rootP] < size[rootQ])
                {
                    parent[rootP] = rootQ;
                    size[rootQ] += size[rootP];
                }
                else
                {
                    parent[rootQ] = rootP;
                    size[rootP] += size[rootQ];
                }
                count--;
            }


            /**
             * Reads in a sequence of pairs of integers (between 0 and n-1) from standard input, 
             * where each integer represents some object;
             * if the sites are in different components, merge the two components
             * and print the pair to standard output.
             *
             * @param args the command-line arguments
             */


        }

    }
}

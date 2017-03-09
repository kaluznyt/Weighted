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

        private IPrinter _printer;

        private readonly WeightedQuickUnionUF _weightedUnionUF;

        public int ComponentsCount => this._weightedUnionUF.count;
        public int SitesCount => this._sites.Length;
        public int OpenedSitesCount => _openedSites;
        public int ClosedSitesCount => SitesCount - OpenedSitesCount;

        private readonly bool[,] _sites;

        private int _openedSites = 0;

        public readonly int N;

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
            _printer = new ConsolePrinter(this);
        }

        public bool IsInPercolationPath(int x)
        {
            return _weightedUnionUF.connected(_weightedUnionUF.parent[x], _virtualTopSite);
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

        public bool IsSiteOpen(int row, int col)
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

            _printer.PrintLargeSiteOverview();
        }
    }
}

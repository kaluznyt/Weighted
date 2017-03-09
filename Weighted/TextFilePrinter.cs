using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weighted
{
    using System.IO;

    public class TextFilePrinter : IPrinter
    {
        private  WeightedQuickUnionUFWrapper _algo;

        private  string _outputFileName;

        public TextFilePrinter(string outputFileName)
        {
            _outputFileName = outputFileName;
        }

        private string PrintStatistics()
        {
            var strb = new StringBuilder();
            strb.AppendLine("Percolates ? " + _algo.Percolates());
            strb.AppendLine("Number of compontents: " + _algo.ComponentsCount);

            strb.AppendLine(
                $"Sites: {_algo.SitesCount} / Opened: {_algo.OpenedSitesCount} / Closed: {_algo.ClosedSitesCount}");

            return strb.ToString();
        }

        public void PrintLargeSiteOverview()
        {
            var strb = new StringBuilder();

            for (int i = 0; i < _algo.N; i++)
            {
                for (int x = 0; x < _algo.N; x++)
                {
                    if (x == 0) strb.Append(i.ToString().PadLeft(3) + ": ");

                    if (_algo.IsInPercolationPath(i * _algo.N + x))
                    {
                        strb.Append("x");
                    }
                    else if (_algo.IsSiteOpen(i, x))
                    {
                        strb.Append(".");
                    }
                    else
                    {
                        strb.Append(" ");
                    }
                }

                strb.AppendLine();
            }

            strb.Append(PrintStatistics());

            File.AppendAllText(_outputFileName, strb.ToString());
        }

        public void SetAlgorithm(WeightedQuickUnionUFWrapper algo)
        {
            _algo = algo;
        }
    }
}

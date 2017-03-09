using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weighted
{
    public interface IPrinter
    {
        void PrintLargeSiteOverview();

        void SetAlgorithm(WeightedQuickUnionUFWrapper algo);
    }
}

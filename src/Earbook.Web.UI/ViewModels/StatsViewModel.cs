using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Earbook.ViewModels
{
    public class StatsViewModel
    {
        public int TrueCount { get; set; }
        public int FalseCount { get; set; }

        public StatsViewModel((int trueCount, int falseCount) data)
        {
            TrueCount = data.trueCount;
            FalseCount = data.falseCount;
        }
    }
}

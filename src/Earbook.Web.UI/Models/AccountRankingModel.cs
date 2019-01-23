using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Earbook.Models
{
    public class AccountRankingModel
    {
        public AccountModel Player { get; set; }
        public int TrueCount { get; set; }
        public int FalseCount { get; set; }
    }
}

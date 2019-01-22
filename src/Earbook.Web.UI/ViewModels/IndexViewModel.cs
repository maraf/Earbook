using Earbook.Models;
using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Earbook.ViewModels
{
    public class IndexViewModel
    {
        public QuizModel Quiz { get; }
        public StatsViewModel Stats { get; }

        public IndexViewModel(QuizModel quiz, StatsViewModel stats)
        {
            Ensure.NotNull(stats, "stats");
            Quiz = quiz;
            Stats = stats;
        }
    }
}

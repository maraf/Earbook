using Neptuo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Earbook.Models
{
    public class QuizOptionModel
    {
        [Key]
        public Guid Id { get; set; }
        public EarModel Option { get; set; }

        public QuizOptionModel()
        { }

        public QuizOptionModel(EarModel option)
        {
            Ensure.NotNull(option, "option");
            Option = option;
        }
    }
}

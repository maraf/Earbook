using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Earbook.Models
{
    public class QuizModel
    {
        [Key]
        public Guid Id { get; set; }

        public AccountModel Player { get; set; }
        public EarModel Answer { get; set; }

        public DateTime When { get; set; }
        public bool? IsSuccess { get; set; }

        public List<QuizOptionModel> Options { get; set; }
    }
}

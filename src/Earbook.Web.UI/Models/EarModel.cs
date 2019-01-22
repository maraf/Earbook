using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Earbook.Models
{
    public class EarModel
    {
        [Key]
        public Guid Id { get; set; }

        public AccountModel Owner { get; set; }

        public string Name { get; set; }
        public string FileName { get; set; }
    }
}

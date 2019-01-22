using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Earbook.Models.Options
{
    public class Storage
    {
        public string Path { get; set; }
        public long MaxLength { get; set; }
        public List<string> SupportedExtensions { get; } = new List<string>();
    }
}

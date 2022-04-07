using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Models
{
    public class About : BaseEntity
    {
        public string Image { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public ICollection<AboutAdvantage> Advantages { get; set; }
    }
}

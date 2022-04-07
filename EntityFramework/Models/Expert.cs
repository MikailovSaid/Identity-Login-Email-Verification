using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Models
{
    public class Expert:BaseEntity
    {
        public ICollection<ExpertImage> Image { get; set; }
        public string Name { get; set; }
        public string Duty { get; set; }
        public string Thought { get; set; }
    }
}

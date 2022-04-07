using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Models
{
    public class ExpertImage:BaseEntity
    {
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public int ExpertId { get; set; }
        public Expert Expert { get; set; }
    }
}

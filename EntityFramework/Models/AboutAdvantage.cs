using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Models
{
    public class AboutAdvantage:BaseEntity
    {
        public string Advantage { get; set; }
        public About About { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Models
{
    public class Blog:BaseEntity
    {
        public string Image { get; set; }
        public DateTime DateTime { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }

    }
}

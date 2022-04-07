using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Models
{
    public class ExpertTable:BaseEntity
    {
        public string Header { get; set; }
        public string Text { get; set; }
    }
}

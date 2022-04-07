using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Models
{
    public class Category:BaseEntity
    { 
        [Required(ErrorMessage = "Burani Doldurun")]
        [StringLength(20, ErrorMessage ="Uzunluq Chox Ola Bilmez")]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

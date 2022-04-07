using EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public SliderDetail Detail { get; set; }
        public List<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public About Abouts { get; set; }
        public List<Expert> Experts { get; set; }
        public List<Expert> ExpertsThoughts { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Instagram> Instagrams { get; set; }
        public ExpertSection ExpertSection { get; set; }



    }
}

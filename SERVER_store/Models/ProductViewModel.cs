using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVER_store.Models
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}

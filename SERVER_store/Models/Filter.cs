using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVER_store.Models
{
    public class Filter
    {
        public IEnumerable<Atribute_in_product> Atribute_In_Products { get; set; }
        public SelectList Atributes { get; set; }
        public SelectList Products { get; set; }
    }
}

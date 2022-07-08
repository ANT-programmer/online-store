using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVER_store.Models
{
    public class Atribute_in_product
    {
        [PrimaryKey]
        public int id { get; set; }
        public string meaning { get; set; }
        public int? AtributeId { get; set; }
        public Atribute Atribute { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }

    }
}

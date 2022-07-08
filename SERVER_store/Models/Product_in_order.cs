using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVER_store.Models
{
    public class Product_in_order
    {
        [PrimaryKey]
        public int id { get; set; }
        public int quantity { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
    }
}

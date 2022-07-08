using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVER_store.Models
{
    public class Category_has_Category
    {
        [PrimaryKey]
        public int id { get; set; }
        public int? Categoryid { get; set; }
        public Category Category { get; set; }
        public string image { get; set; }
        public string name { get; set; }
    }
}

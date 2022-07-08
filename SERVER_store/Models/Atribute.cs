using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVER_store.Models
{
    public class Atribute
    {
        [PrimaryKey]
        public int id { get; set; }
        public string name { get; set; }
    }
}

using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVER_store.Models
{
    public class Cart
    {
        [PrimaryKey]
        public int id{ get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}


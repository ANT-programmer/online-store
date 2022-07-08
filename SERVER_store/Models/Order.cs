using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DataTypeAttribute = System.ComponentModel.DataAnnotations.DataTypeAttribute;

namespace SERVER_store.Models
{
    public class Order 
    {
        [PrimaryKey]
        public int id { get; set; }
        public string opisanye { get; set; }
        public DateTime date { get; set; }
        [Required(ErrorMessage = "Адрес должен быть введён")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [DataType(DataType.Text)]
        [Display(Name = "adres")]
        public string adres { get; set; }
        [Required(ErrorMessage = "выберите способ оплаты")]
        [DataType(DataType.Currency)]
        [Display(Name = "oplata")]
        public string oplata { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}

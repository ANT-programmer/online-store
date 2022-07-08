using LinqToDB.Mapping;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DataTypeAttribute = System.ComponentModel.DataAnnotations.DataTypeAttribute;

namespace SERVER_store.Models
{
    public class Product
    {
        [PrimaryKey]
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }

        [Display(Name = "Название")]
        public string name { get; set; }
        [Display(Name = "Цена (BYN)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public double price { get; set; }
        [Display(Name = "Картинка (URL)")]
        public string img { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int? Category_has_CategoryId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Category_has_Category Category_has_Category { get; set; }
        [Display(Name = "Количество товара")]
        public  int quantity_in_stock { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string description { get; set; }
    }
}

using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SERVER_store.Models;
using SERVER_store.Models.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SERVER_store.Controllers
{
    public class HomeController : Controller
    {
        EFDbContext db;


        public HomeController(EFDbContext context)
        {
            db = context;
        }
        public IActionResult Index(int? id)
        {
            ViewBag.prod = db.Products.Take(2);
            ViewBag.carusel = db.Products.Take(5);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            return View();

        }

        public IActionResult ProducthasCategory(int id)
        {
            ViewBag.atrinprod = db.Atribute_In_Products;
            ViewBag.atribut = db.Atributes;
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            return View(db.Products.Where(x => x.Category_has_Category.id == id));

        }
        public IActionResult ProductShow(int id)
        {
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            var productedition = db.Products;
            ViewBag.Prod = productedition.Where(Product => Product.id == id);
            return View(db.Atribute_In_Products.Where(x => x.Product.id == id).Include(x => x.Atribute).ToList());
        }
        public IActionResult Cart( IFormCollection form)
        {
            int quantity = Int32.Parse(form["qty"]);
            int prodid = Int32.Parse(form["productid"]);
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            var user = User.Claims.ElementAt(0).Value;
            var cart = db.Carts.FirstOrDefault(x => x.UserId == user);
            db.Product_In_Carts.Add(new Product_in_cart { Cart = cart, ProductId = prodid, Quantity = quantity });
            db.SaveChanges();
            return RedirectToAction("ProductShow", "Home", new { id = prodid });
        }

        public IActionResult ShowCart() 
        {
            ViewBag.prodincart = db.Product_In_Carts;
            ViewBag.ord = db.Orders;
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            var user = User.Claims.ElementAt(0).Value;
            var cart = db.Carts.FirstOrDefault(x => x.UserId == user);
            var productcart = db.Product_In_Carts.Include(x=>x.Product).Where(x => x.Cart == cart).ToList();
            return View(productcart);
        }

        public IActionResult RemoveFromCart(int id)
        {
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            var userId = User.Claims.ElementAt(0).Value;
            var cart = db.Carts.FirstOrDefault(x => x.UserId == userId);
            var product_in_cart =
                db.Product_In_Carts.FirstOrDefault(x => x.CartId == cart.id && x.Product.id == id);
            if (product_in_cart != null) db.Product_In_Carts.Remove(product_in_cart);
            db.SaveChanges();
            return RedirectToAction("ShowCart", "Home");
        }
        public IActionResult MoneyFilter(IFormCollection form) 
        {
            ViewBag.atribut = db.Atributes;
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            ViewBag.atrinprod = db.Atribute_In_Products;
            double min = Double.Parse(form["min"]);
            double max = Double.Parse(form["max"]);
            return View(db.Products.Where(x=>x.price > min && x.price < max).ToList());
        }


        public IActionResult AddOrder(IFormCollection form) 
        {
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            ViewBag.or = db.Orders;
            var user = User.Claims.ElementAt(0).Value;
            string adresss = form["adres"];
            string oplataa = form["oplata"];
            var order = new Order { UserId = user, date = DateTime.Now, opisanye = "Обрабатывается", adres=adresss, oplata=oplataa};
            db.Orders.Add(order);
            db.SaveChanges();
            var productcart = db.Product_In_Carts.Include(x => x.Product).Where(x => x.Cart.UserId==user).ToList();
            List<Product_in_order> productorders = new List<Product_in_order>();
            foreach (var item in productcart) 
            {
              var q =  db.Products.Find(item.ProductId);
                q.quantity_in_stock = q.quantity_in_stock - item.Quantity;
                db.Products.Update(q);
                productorders.Add(new Product_in_order { quantity = item.Quantity, ProductId = item.ProductId, OrderId = order.id });
            }
            db.Product_In_Orders.AddRange(productorders);
            db.SaveChanges();
            db.Product_In_Carts.RemoveRange(productcart);
            db.SaveChanges();

     
            return RedirectToAction("ShowOrders", "Home", new { id = order.id });
        }


        public IActionResult ShowOrders()
        {
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            var user = User.Claims.ElementAt(0).Value;
            var orders = db.Orders.Where(x => x.UserId == user).ToList();
            return View(orders);
        }

        public IActionResult Order(int id)
        {
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            return View(db.Product_In_Orders.Where(x => x.OrderId == id).Include(x => x.Product).ToList());
        }

        public IActionResult Search(string searchString)
        {
            ViewBag.atrinprod = db.Atribute_In_Products;
            ViewBag.atribut = db.Atributes;
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            var products = from m in db.Products
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.name!.Contains(searchString));
            }

            return View(products);
        }

        public ActionResult PriceList()
        {
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            var prod = db.Products;
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Прайс лист");
                worksheet.Cell("A1").Value = "Название товара";
                worksheet.Cell("B1").Value = "Стоимость";
                worksheet.Cell("C1").Value = "Количество товара";
                worksheet.Cell("D1").Value = "Описание товара";
                worksheet.Row(1).Style.Font.Bold = true;
                worksheet.Cell("A1").Style.Fill.BackgroundColor = XLColor.LightGreen;
                worksheet.Cell("B1").Style.Fill.BackgroundColor = XLColor.LightGreen;
                worksheet.Cell("C1").Style.Fill.BackgroundColor = XLColor.LightGreen;
                worksheet.Cell("D1").Style.Fill.BackgroundColor = XLColor.LightGreen;
                worksheet.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.ColumnWidth = 45;
                var i = 0;
                foreach (var item in prod)
                {
                    worksheet.Cell(i + 2, 1).Value = string.Join(", ", item.name);
                    worksheet.Cell(i + 2, 2).Value = item.price;
                    worksheet.Cell(i + 2, 3).Value = item.quantity_in_stock;
                    worksheet.Cell(i + 2, 4).Value = string.Join(", ", item.description);
                    i++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Прайс_лист_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }

        }

        [HttpGet]
        public async Task<ActionResult> ProductAsync(int page=1)
        {           
            
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            int pageSize = 6;   // количество элементов на странице

            IQueryable<Product> source = db.Products;
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            ProductViewModel viewModel = new ProductViewModel
            {
                PageViewModel = pageViewModel,
                Products = items
            };
            return View(viewModel);
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVER_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERVER_store.Models.Repository;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using System.IO;

namespace SERVER_store.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly EFDbContext _database;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(EFDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager)
        {
            _database = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> AddAdmin(string[] selectedUsersId)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            if (selectedUsersId != null)
            {
                foreach (var userId in selectedUsersId)
                {
                    var user = _database.Users.FirstOrDefault(u => u.Id == userId);
                    await _userManager.AddToRolesAsync(user, new[] { "Admin" });
                    await _signInManager.RefreshSignInAsync(await _userManager.FindByIdAsync(userId));
                }
            }
            return RedirectToAction("AdminPanel", "Admin");
        }

        public async Task<IActionResult> DeleteAdmin(string[] selectedUsersId)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            if (selectedUsersId != null)
            {
                foreach (var userId in selectedUsersId)
                {
                    var user = _database.Users.FirstOrDefault(u => u.Id == userId);
                    await _userManager.RemoveFromRolesAsync(user, new[] { "Admin" });
                    await _signInManager.RefreshSignInAsync(await _userManager.FindByIdAsync(userId));
                }
            }
            return RedirectToAction("AdminPanel", "Admin");
        }

        public IActionResult DeleteUser(string[] selectedUsersId)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            if (selectedUsersId != null)
                foreach (var userId in selectedUsersId)
                {
                    var user = _database.Users.FirstOrDefault(u => u.Id == userId);
                    _database.Remove(user);
                    _database.SaveChanges();
                }

            return RedirectToAction("AdminPanel", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(string[] selectedUsersId)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            if (selectedUsersId != null)
                foreach (var userId in selectedUsersId)
                {
                    var user = _database.Users.FirstOrDefault(u => u.Id == userId);
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.Today.AddYears(100));
                }

            return RedirectToAction("AdminPanel", "Admin");
        }

        public async Task<IActionResult> UnBlockUser(string[] selectedUsersId)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            if (selectedUsersId != null)
                foreach (var userId in selectedUsersId)
                {
                    var user = _database.Users.FirstOrDefault(u => u.Id == userId);
                    await _userManager.SetLockoutEndDateAsync(user, null);
                }

            return RedirectToAction("AdminPanel", "Admin");
        }

        public async Task<IActionResult> AdminPanel()
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            ViewBag.AllProfiles = await _database.Users
                .ToListAsync();
            ViewBag.AllOrders = await _database.Orders.ToListAsync();
            return View();
        }

       

        [HttpGet]
        public ActionResult CreateProdAtr(int? id)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            ViewBag.atr = _database.Atributes;
            ViewBag.productid = id;
            return View();
        }

        [HttpPost]
        public ActionResult CreateProdAtr(IFormCollection form)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            var atrinprod = new Atribute_in_product();
            string prodid = form["productid"];
            string atribut = form["atribut"];
            string mean = form["meaning"];
            atrinprod.ProductId = Int32.Parse(prodid);
            atrinprod.AtributeId = Int32.Parse(atribut);
            atrinprod.meaning = mean;

            _database.Add(atrinprod);
            _database.SaveChanges();

            return RedirectToAction("CreateProdAtr");
        }

        [HttpGet]
        public ActionResult DeleteProdAtr(int? id)
        {
        
            Atribute_in_product b = _database.Atribute_In_Products.Find(id);
            if (b != null)
            {
                _database.Atribute_In_Products.Remove(b);
                _database.SaveChanges();
            }
            return RedirectToAction("Product", "Home");
        }


        [HttpGet]
        public ActionResult CreateProd(int? id)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            ViewBag.cathas = _database.Category_Has_Categories;
            ViewBag.ProductId = id;
            return View();
        }
        [HttpPost]
        public ActionResult CreateProd(IFormCollection form)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            var produ = new Product();
            string ProductId = form["ProductId"];
            produ.Category_has_CategoryId = Int32.Parse(ProductId);
            string img = form["img"];
            string name = form["name"];
            string price = form["price"];
            string quantity_in_stock = form["quantity_in_stock"];
            string description = form["description"];
            produ.img = img;
            produ.name = name;
            produ.price = Double.Parse(price);
            produ.quantity_in_stock = Int32.Parse(quantity_in_stock);
            produ.description = name;
            _database.Add(produ);
            _database.SaveChanges();
            return RedirectToAction("Product", "Home");
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory(Category cat)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            _database.Add(cat);
            _database.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CreateCatHasCat(int? id)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            ViewBag.Cat = _database.Categories;
            ViewBag.catid = id;
            return View();
        }
        [HttpPost]
        public ActionResult CreateCatHasCat(IFormCollection form)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            var cats = new Category_has_Category();
            string catid = form["catid"];
            cats.Categoryid = Int32.Parse(catid);
            string image = form["image"];
            string name = form["name"];
            cats.image = image;
            cats.name = name;
            _database.Add(cats);
            _database.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditCatHasCat(int id)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            ViewBag.Cat = _database.Categories;
            Category_has_Category cathascat = await _database.Category_Has_Categories.FindAsync(id);
            if (cathascat == null)
            {
                return NotFound();
            }
            Category_has_Category cats = new Category_has_Category { id = cathascat.id, name = cathascat.name, Categoryid = cathascat.Categoryid, Category = cathascat.Category};
            return View(cats);
        }

        [HttpPost]
        public async Task<IActionResult> EditCatHasCat(Category_has_Category cats)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            if (ModelState.IsValid)
            {
                Category_has_Category cathascat = await _database.Category_Has_Categories.FindAsync(cats.id);
                if (cathascat != null)
                {
                    cathascat.name = cats.name;
                    cathascat.Categoryid = cats.Categoryid;

                    var result = _database.Category_Has_Categories.Update(cathascat);
                    _database.SaveChanges();
                    if (result != null)
                    {
                        return RedirectToAction("Product", "Home");
                    }
                }
            }
            return View(cats);
        }

        [HttpGet]
        public ActionResult DeleteCatHasCat(int? id)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            Category_has_Category b = _database.Category_Has_Categories.Find(id);
            if (b != null)
            {
                _database.Category_Has_Categories.Remove(b);
                _database.SaveChanges();
            }
            return RedirectToAction("Product", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            Category category = await _database.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            Category cat = new Category { id = category.id, name = category.name };
            return View(cat);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category cat)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            if (ModelState.IsValid)
            {
                Category category = await _database.Categories.FindAsync(cat.id);
                if (category != null)
                {
                    category.name = cat.name;

                    var result = _database.Categories.Update(category);
                    _database.SaveChanges();
                    if (result != null)
                    {
                        return RedirectToAction("Product", "Home");
                    }
                }
            }
            return View(cat);
        }

        [HttpGet]
        public ActionResult DeleteCategory(int? id)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            Category b = _database.Categories.Find(id);
            if (b != null)
            {
                _database.Categories.Remove(b);
                _database.SaveChanges();
            }
            return RedirectToAction("Product", "Home");
        }  

        [HttpGet]
        public async Task<ActionResult> EditProdsAsync(int id)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            Product prod = await _database.Products.FindAsync(id);
            if (prod == null)
            {
                return NotFound();
            }
            Product prods = new Product { id = prod.id, name = prod.name, img = prod.img, price = prod.price, description = prod.description, quantity_in_stock = prod.quantity_in_stock };
                return View(prods);
          
        }

        [HttpPost]
        public async Task<ActionResult> EditProdsAsync(Product prod)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            if (ModelState.IsValid)
            {
                Product product = await _database.Products.FindAsync(prod.id);
                if (product != null)
                {
                    product.name = prod.name;
                    product.img = prod.img;
                    product.price = prod.price;
                    product.description = prod.description;
                    product.quantity_in_stock = prod.quantity_in_stock;

                    var result = _database.Products.Update(product);
                    _database.SaveChanges();
                    if (result != null)
                    {
                        return RedirectToAction("Product", "Home");
                    }
                }
            }
            return View(prod);
        }

        [HttpGet]
        public ActionResult DeleteProd(int? id)
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            Product b = _database.Products.Find(id);
            if (b != null)
            {
                _database.Products.Remove(b);
                _database.SaveChanges();
            }
            return RedirectToAction("Product", "Home");
        }

        public ActionResult Export()
        {
            ViewBag.prod = _database.Products.Take(2);
            ViewBag.CategoryStruct = _database.Categories;
            ViewBag.SubCategoryStruct = _database.Category_Has_Categories;
            var ord = _database.Orders;
            var prod = _database.Product_In_Orders.Include(x=>x.Product);
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Заказы");
                worksheet.Cell("A1").Value = "Номер заказа";
                worksheet.Cell("B1").Value = "Название товара";
                worksheet.Cell("C1").Value = "количество товара";
                worksheet.Cell("D1").Value = "Дата";
                worksheet.Cell("E1").Value = "Способ оплаты";
                worksheet.Cell("F1").Value = "Статус";
                worksheet.Row(1).Style.Font.Bold = true;
                worksheet.Cell("A1").Style.Fill.BackgroundColor = XLColor.LightGreen;
                worksheet.Cell("B1").Style.Fill.BackgroundColor = XLColor.LightGreen;
                worksheet.Cell("C1").Style.Fill.BackgroundColor = XLColor.LightGreen;
                worksheet.Cell("D1").Style.Fill.BackgroundColor = XLColor.LightGreen;
                worksheet.Cell("E1").Style.Fill.BackgroundColor = XLColor.LightGreen;
                worksheet.Cell("F1").Style.Fill.BackgroundColor = XLColor.LightGreen;
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
                var j = 0;
                foreach (var it in prod)
                {
                    worksheet.Cell(j + 2, 2).Value = string.Join(", ", it.Product.name);
                    worksheet.Cell(j + 2, 3).Value = it.quantity;
                    j++;
                }
                foreach (var item in ord) 
                {
                    worksheet.Cell(i + 2, 1).Value = item.id;
                    worksheet.Cell(i + 2, 4).Value = item.date;
                    worksheet.Cell(i + 2, 5).Value = string.Join(", ", item.oplata);
                    worksheet.Cell(i + 2, 6).Value = string.Join(", ", item.opisanye);
                    i++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Заказы_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
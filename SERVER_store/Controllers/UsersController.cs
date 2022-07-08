using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SERVER_store.Models;
using SERVER_store.ViewModels;
using SERVER_store.Models.Repository;

namespace SERVER_store.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;

        EFDbContext db;

        public UsersController(UserManager<User> userManager, EFDbContext context)
        {
            _userManager = userManager;
            db = context;
        }

        public IActionResult Index()
        {
            ViewBag.atrinprod = db.Atribute_In_Products;
            ViewBag.atribut = db.Atributes;
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            return View(_userManager.Users.ToList()); 
        }

        public IActionResult Create()
        {
            ViewBag.atrinprod = db.Atribute_In_Products;
            ViewBag.atribut = db.Atributes;
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            ViewBag.atrinprod = db.Atribute_In_Products;
            ViewBag.atribut = db.Atributes;
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.atrinprod = db.Atribute_In_Products;
            ViewBag.atribut = db.Atributes;
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            ViewBag.atrinprod = db.Atribute_In_Products;
            ViewBag.atribut = db.Atributes;
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            ViewBag.atrinprod = db.Atribute_In_Products;
            ViewBag.atribut = db.Atributes;
            ViewBag.prod = db.Products.Take(2);
            ViewBag.CategoryStruct = db.Categories;
            ViewBag.SubCategoryStruct = db.Category_Has_Categories;
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}
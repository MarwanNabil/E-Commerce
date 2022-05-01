using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Software_Project.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext db;
        public CategoryController(ApplicationDbContext DBconnection)
        {
            db = DBconnection;
        }
        public List<Category>  getCategory()
        {
            List<Category> category = db.Categories.ToList();
            return category;
        }
        public IActionResult DisplayAllCategory()
        {
            List<Category> category = getCategory();
            return View(category);
        }

        public IActionResult Create(Category model)
        {
            db.Categories.Add(model);
            db.SaveChanges();
            return View();
        }
        public IActionResult getAllProducts(int categoryId)
        {
            var products = db.Products.Where(p => p.CategoryID == categoryId);
            return View(products);
        }
    }
}

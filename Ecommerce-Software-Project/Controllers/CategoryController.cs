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
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Models.Category ca)
        {

            if (!ModelState.IsValid)
            {
                return View("AddCategory",ca);
            }
            var category = new Category
            {
                CategoryName = ca.CategoryName
            };
            db.Categories.Add(category);
            db.SaveChanges();
         
            return RedirectToAction("DisplayAllCategory");
        }
    }
}

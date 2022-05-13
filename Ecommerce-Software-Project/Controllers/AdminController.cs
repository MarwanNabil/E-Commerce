using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Software_Project.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db;
        private readonly IWebHostEnvironment Ih;

        public AdminController(ApplicationDbContext _db, IWebHostEnvironment _Ih)
        {
            db = _db;
            Ih = _Ih;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowCategories()
        {
            return View(db.Categories);
        }

        public IActionResult ShowSellers()
        {
            return View(db.Users);
        }

        public IActionResult ShowProducts()
        {
            return View(db.Products);
        }


        public IActionResult DeleteCategory(int id)
        {
            Category tmpCat = db.Categories.Where(r => r.Id.Equals(id)).First();
            db.Categories.Remove(tmpCat);
            db.SaveChanges();

            TempData[Toaster.Success] = "Deleted Successfully!";

            return View("ShowCategories", db.Categories);
        }

        public IActionResult ViewCategory(int id)
        {
            ProductController tmpP = new ProductController(db, Ih);
            return tmpP.DisplayProductsViaCategory(id);
        }

        public IActionResult AddCategory()
        {
            return View("AddCategory");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveCategory(Models.Category ca)
        {

            if (!ModelState.IsValid)
            {
                return View("AddCategory", ca);
            }
            var category = new Category
            {
                CategoryName = ca.CategoryName
            };
            db.Categories.Add(category);
            db.SaveChanges();

            TempData[Toaster.Success] = "Added Successfully!";

            return View("ShowCategories" , db.Categories);
        }

    }
}

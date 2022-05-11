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
    }
}

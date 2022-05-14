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
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);
            var user = Authentication.LoggedInUser;

            if (user.Name != "Admin")
            {
                TempData[Toaster.Warning] = "Admin Only!";
                return View("~/Views/Home/Index.cshtml");
            }

            return View();
        }

        public IActionResult ShowCategories()
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);
            var user = Authentication.LoggedInUser;

            if (user.Name != "Admin")
            {
                TempData[Toaster.Warning] = "Admin Only!";
                return View("~/Views/Home/Index.cshtml");
            }

            return View(db.Categories);
        }

        public IActionResult ShowSellers()
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);
            var user = Authentication.LoggedInUser;

            if (user.Name != "Admin")
            {
                TempData[Toaster.Warning] = "Admin Only!";
                return View("~/Views/Home/Index.cshtml");
            }

            return View(db.Users);
        }

        public IActionResult ShowProducts()
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);
            var user = Authentication.LoggedInUser;

            if (user.Name != "Admin")
            {
                TempData[Toaster.Warning] = "Admin Only!";
                return View("~/Views/Home/Index.cshtml");
            }

            return View(db.Products);
        }


        public IActionResult DeleteCategory(int id)
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);
            var user = Authentication.LoggedInUser;

            if (user.Name != "Admin")
            {
                TempData[Toaster.Warning] = "Admin Only!";
                return View("~/Views/Home/Index.cshtml");
            }

            Category tmpCat = db.Categories.Where(r => r.Id.Equals(id)).Include(r => r.Products).First();
            
            foreach(Product product in tmpCat.Products)
            {
                Product withReview = db.Products.Where(r => r.Id == product.Id).Include(product => product.Reviews).First();
                foreach (Review r in product.Reviews)
                {
                    db.Reviews.Remove(r);
                }
                db.Products.Remove(product);
            }

            db.Categories.Remove(tmpCat);
            db.SaveChanges();

            TempData[Toaster.Success] = "Deleted Successfully!";

            return View("ShowCategories", db.Categories);
        }

        public IActionResult ViewCategory(int id)
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);
            var user = Authentication.LoggedInUser;

            if (user.Name != "Admin")
            {
                TempData[Toaster.Warning] = "Admin Only!";
                return View("~/Views/Home/Index.cshtml");
            }

            ProductController tmpP = new ProductController(db, Ih);
            return tmpP.DisplayProductsViaCategory(id);
        }

        public IActionResult AddCategory()
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);
            var user = Authentication.LoggedInUser;

            if (user.Name != "Admin")
            {
                TempData[Toaster.Warning] = "Admin Only!";
                return View("~/Views/Home/Index.cshtml");
            }

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

        public IActionResult PurchaseMoney()
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);
            var user = Authentication.LoggedInUser;

            if (user.Name != "Admin")
            {
                TempData[Toaster.Warning] = "Admin Only!";
                return View("~/Views/Home/Index.cshtml");
            }


            return View("PurchaseMoney" , db.Users);
        }

        [HttpPost]
        public IActionResult PurchaseMoneyRequest(int UserId , int Money)
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);
            var user = Authentication.LoggedInUser;

            if(user.Name != "Admin")
            {
                TempData[Toaster.Warning] = "Admin Only!";
                return View("~/Views/Home/Index.cshtml");
            }


            if (Money < 0 || Money >= 10000)
            {
                TempData[Toaster.Warning] = "Please Add Suitable Money!";
            } else
            {
                User tmpUser = db.Users.Where(r => r.Id == UserId).First();
                tmpUser.Money += Money;
                db.Users.Update(tmpUser);
                db.SaveChanges();
                TempData[Toaster.Success] = "Added " + Money.ToString() + " $ to his account!";
            }

            return PurchaseMoney();
        }

    }
}

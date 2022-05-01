using Microsoft.AspNetCore.Mvc;
using Ecommerce_Software_Project.Models;
namespace Ecommerce_Software_Project.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment Ih;

        public UserController(ApplicationDbContext _db, IWebHostEnvironment _Ih)
        {
            db = _db;
            Ih = _Ih;
        }

        public IActionResult Index()
        {
            return View();
        }

        public static void AddUser(ApplicationDbContext db, User newUser)
        {
            db.Add(newUser);
            db.SaveChanges();
        }

        public IActionResult Profile()
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);

            var user = Authentication.LoggedInUser;
            user.Products = db.Products.Where(p => p.SellerId == user.Id).ToList();
            foreach (var product in user.Products)
            {
                product.Category = db.Categories.SingleOrDefault(c => c.Id == product.CategoryID);
            }
            return View(Authentication.LoggedInUser);
        }

        public IActionResult ProfileWithId(int id)
        {
            var user = db.Users.SingleOrDefault(u => u.Id == id);
            user.Products = db.Products.Where(p => p.SellerId == user.Id).ToList();
            foreach (var product in user.Products)
            {
                product.Category = db.Categories.SingleOrDefault(c => c.Id == product.CategoryID);
            }
            return View("Profile", user);
        }

        public IActionResult SearchUsers()
        {
            return View(db.Users.Include(p => p.Products).Where(e => e.Products.Count > 0));
        }

        public IActionResult SearchUsersWithString(string searchName)
        {
            if (string.IsNullOrEmpty(searchName))
            {
                return View("SearchUsers", db.Users.Include(p => p.Products).Where(e => e.Products.Count > 0));
            }

            return View("SearchUsers", db.Users.Include(p => p.Products).Where(e => e.Products.Count > 0 && e.Name.Contains(searchName)));
        }

        public void ChargeMoney(int money)
        {
            User user = Authentication.LoggedInUser;
            user.Money = user.Money + money;
            db.SaveChanges();
        }
        public bool CanBuy(int PriceTobeSubtracted)
        {
            User user = Authentication.LoggedInUser;
            if (PriceTobeSubtracted > user.Money)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public int buyProduct(int priceToBeSubtracted)
        {
            User user = Authentication.LoggedInUser;
            try
            {
                if (CanBuy(priceToBeSubtracted))
                {
                    user.Money = user.Money - priceToBeSubtracted;
                    db.SaveChanges();
                    return (user.Money - priceToBeSubtracted);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }
    }
}

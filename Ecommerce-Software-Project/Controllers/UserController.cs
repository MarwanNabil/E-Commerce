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

        public IActionResult DeleteUser(int id)
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);

            var user = Authentication.LoggedInUser;

            try
            {
                User target = db.Users.Where(r => r.Id == id).First();

                if (user.Name != "Admin")
                    throw new Exception("Not Authorized!");
                if (target.Name == "Admin")
                    throw new Exception("You can't Delete An Admin!");

                User targetUser = db.Users.Where(r => r.Id == id).Include(r => r.Products).First();

                foreach(var product in targetUser.Products)
                {
                        Product p = db.Products.Where(r => r.Id == product.Id).Include(r => r.Reviews).First();
                        foreach (Review r in product.Reviews)
                        {
                            db.Reviews.Remove(r);
                        }
                        db.Products.Remove(product);
                }

                db.Users.Remove(target);
                db.SaveChanges();

                TempData[Toaster.Success] = "Deleted Successfully.";

            } catch(Exception e)
            {
                TempData[Toaster.Warning] = e.Message;
            }


            return View("~/Views/Home/Index.cshtml");
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

using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Software_Project.Controllers
{
    public class ChargeMoneyController : Controller
    {
        private ApplicationDbContext db;
        public ChargeMoneyController(ApplicationDbContext DBconnection)
        {
            db = DBconnection;
        }
        public IActionResult charge_money()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddMoney(string money)
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);

            var user = Authentication.LoggedInUser;
            Console.WriteLine(money);

            if (money==null|| user == null)
            {
                TempData[Toaster.Error] = " You should enter  amount of money";
                return View("charge_money", money);
            }
            int m = int.Parse(money);
            if (m <= 100 || m > 1000000)
            {
                TempData[Toaster.Error] = "Sorry, we accept the amount of money from 100 to 1000000 only";
                return View("charge_money", money);
            }
            var special_user = db.Users.Where(x => x.Id == user.Id).FirstOrDefault();
            special_user.Money += m;
            db.SaveChanges();
            TempData[Toaster.Success] = "Done ";
            return RedirectToAction("charge_money");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Software_Project.Controllers;

public class AuthController : Controller
{
    private readonly ApplicationDbContext db;
    private readonly IWebHostEnvironment Ih;

    public AuthController(ApplicationDbContext _db, IWebHostEnvironment _Ih)
    {
        db = _db;
        Ih = _Ih;
    }

    #region Login
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Login(User user, bool rememberMe)
    {
        var userInDb = db.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
        if (userInDb == null)
        {
            TempData[Toaster.Error] = "User Not Found";
            return RedirectToAction("Index");
        }

        Services.Authentication.SetUser(userInDb);
        TempData[Toaster.Success] = "You have logged in";
        return RedirectToAction("Index", "Home");
    }
    public IActionResult LogOut()
    {
        Services.Authentication.LogUserOut();
        return RedirectToAction("Index", "Home");
    }

    #endregion
    #region Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(User userForm, IFormFile userImage)
    {
        if (userImage == null || userForm == null)
        {
            TempData[Toaster.Error] = "Please Enter The Your Data";
            return View();
        }

        if (!Service.IsValidImage(userImage))
        {
            TempData[Toaster.Error] = "Please Enter The Valid Image (png, jpg, jpeg, gif)";
            return View();
        }

        var imagePath = Service.SaveImageAndGetPath(userImage, Ih);
        if (imagePath == string.Empty)
        {
            TempData[Toaster.Error] = "Error Happened, Please Try Again.";
            return View();
        }

        userForm.UserImageUrl = imagePath;

        UserController.AddUser(db, userForm);
        TempData[Toaster.Success] = "You have succeded in registration, Please login to continue.";
        return RedirectToAction("Index");
    }
    #endregion
}

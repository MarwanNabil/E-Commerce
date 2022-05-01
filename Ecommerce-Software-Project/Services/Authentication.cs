using Microsoft.AspNetCore.Authorization;

namespace Ecommerce_Software_Project.Services;

public class Authentication
{
    public static User? LoggedInUser { get; private set; }

    public static bool IsLoggedIn { get => LoggedInUser != null; }

    public static void SetUser(User loggedInUser)
    {
        LoggedInUser = loggedInUser;
    }

    public static IActionResult CheckAuthAndRouteLogin(object controllerObj)
    {
        if (!IsLoggedIn)
        {
            var controller = (Controller)controllerObj;
            controller.TempData[Toaster.Warning] = "You Have To Be Logged In.";
            return controller.RedirectToAction("Index", "Auth");
        }
        return null;
    }
}

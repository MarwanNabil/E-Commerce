using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Ecommerce_Software_Project.Controllers
{
    public class ProductController : Controller
    {

        static bool did = false; 
        private static ApplicationDbContext db;
        private readonly IWebHostEnvironment Ih;

        public ProductController(ApplicationDbContext _db, IWebHostEnvironment _Ih)
        {
            did = true;
            db = _db;
            List<Category> A = db.Categories.Where(r => true).ToList();
            Ih = _Ih;
        }

        //Add Product
        [HttpGet]
        public IActionResult Addproduct()
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);

            AddProductView prodView = new AddProductView();
            prodView.categories = db.Categories.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.CategoryName }).ToList();
            return View(prodView);
        }

        [HttpPost]
        public IActionResult Addproduct(AddProductView productForm, IFormFile productImage, int categoryValue)
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);

            var user = Authentication.LoggedInUser;
            //var userInDb = db.Users.Where(x => x.Id == user.Id);

            if (productImage == null || user == null)
            {
                TempData[Toaster.Error] = "Please Enter The Product's Info";
                return Addproduct();
            }

            var imagePath = Service.SaveImageAndGetPath(productImage, Ih);
            var category = db.Categories.SingleOrDefault(e => e.Id == categoryValue);

            if (category == null)
            {
                TempData[Toaster.Error] = "Please Enter Product's Category";
                return Addproduct();
            }

            //put data in db
            productForm.product.ProductImages = imagePath;
            productForm.product.SellerId = user.Id;
            productForm.product.ProductAddedDate = DateTime.Now;
            productForm.product.CategoryID = category.Id;

            db.Products.Add(productForm.product);
            db.SaveChanges();

            TempData[Toaster.Success] = "You have succeded in registration, Please login to continue.";
            return RedirectToAction("Addproduct");
        }

        //get product of User
        public IActionResult GetProductOfUser()
        {
            var user = Authentication.LoggedInUser;
            return View(GetProductsViaUserID(user.Id));
        }

        public IEnumerable<Product> GetProductsViaUserID(int userID)
        {
            return db.Products.Include(e => e.Category).Where(e => e.SellerId == userID);
        }

        //get all product of shop and search Product
        [HttpGet]
        public IActionResult GetAllProductOfShop()
        {
            return View(GetAllProductShop());
        }

        public IEnumerable<Product> GetAllProductShop()
        {
            return db.Products.Include(e => e.Category).Include(s => s.Seller);
        }

        [HttpPost]
        public IActionResult GetAllProductOfShop(string searchProduct, string categoryValue)
        {
            return View(GetAllProductShop(searchProduct, categoryValue));
        }
        public IEnumerable<Product> GetAllProductShop(string searchProduct, string categoryValue)
        {
            var prods = db.Products.Include(e => e.Category).Include(s => s.Seller).Where(e => e.ProductName.Contains(searchProduct) && e.Category.CategoryName.Contains(categoryValue)).Include(u => u.Seller);
            return prods;
        }

        //purchase products
        /*public IActionResult purchaseProduct(int id, int quantityNeeded)
        {
            Product product = new Product {Id = id};
            product.ProductQuantity = product.ProductQuantity - quantityNeeded;
            db.SaveChanges();
           
            return View();
        } */

        //show product details page

        public ViewResult ShowProductDetails(int id)
        {
            return View(ShowAllProductDetails(id));
        }
        public ShowProductDetailsView ShowAllProductDetails(int id)
        {
            ShowProductDetailsView product = new ShowProductDetailsView();
            product.product = db.Products.Include(e => e.Category).Include(s => s.Seller).Where(p => p.Id == id).First();
            product.reviews = displayReviews(id);
            return (product);
        }


        [HttpPost]
        public IActionResult GetReview(Product product, Review review)
        {
            if (!Authentication.IsLoggedIn)
                return Authentication.CheckAuthAndRouteLogin(this);
            var user = Authentication.LoggedInUser;

            ShowProductDetailsView rev = new ShowProductDetailsView();

            //rev.review = new Review();
            //rev.review.Description = review.Description;
            //rev.review.rate = review.rate;
            review.ProductId = product.Id;
            review.UserId = user.Id;
            review.Date = DateTime.Now;
            //rev.review.ProductId = product.Id;

            db.Reviews.Add(review);
            db.SaveChanges();
            return Redirect("/");
        }
        [HttpGet]
        public IEnumerable<Review> displayReviews(int id)
        {
            ShowProductDetailsView rev = new ShowProductDetailsView();
            rev.reviews = db.Reviews.Include(u => u.User).Where(p => p.ProductId == id);
            return rev.reviews;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DisplayProductsViaCategory(int id)
        {
            IEnumerable<Product> tmp = db.Products.Include(r => r.Seller).Include(r => r.Category).Where(r => r.CategoryID == id);
             
            //IEnumerable<Product> products = db.Products;
            return View("~/Views/Product/GetAllProductOfShop.cshtml", tmp);
        }
    }
}

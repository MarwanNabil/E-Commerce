namespace Ecommerce_Software_Project.Services
{
    public class Service
    {
        public static bool IsValidImage(IFormFile image)
        {
            string imageType = Path.GetExtension(image.FileName).ToLower();
            string[] validTypes = new string[] { "png", "jpg", "jpeg", "gif" };
            foreach (var type in validTypes)
            {
                if (imageType.Contains(type)) return true;
            }
            return false;
        }

        public static string SaveImageAndGetPath(IFormFile image, IWebHostEnvironment webHostEnvironment)
        {
            if (!IsValidImage(image)) return string.Empty;

            var imageName = Guid.NewGuid().ToString() + image.FileName;
            var fs = new FileStream(Path.Combine(webHostEnvironment.WebRootPath, "Images", imageName), FileMode.Create);
            image.CopyTo(fs);

            return "/images/" + imageName;
        }
    }
}

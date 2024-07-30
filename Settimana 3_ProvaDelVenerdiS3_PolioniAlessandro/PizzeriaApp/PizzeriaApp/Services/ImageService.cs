using PizzeriaApp.Services.Interfaces;

namespace PizzeriaApp.Services
{
    public class ImageService : IImageService
    {
        public async Task<string> ConvertImageToBase64Async(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }

            try
            {
                // Utilizza MemoryStream in un blocco using per garantire una corretta gestione delle risorse
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception ex)
            {
                // Gestione eccezione 
                throw new InvalidOperationException("Error converting image to Base64.", ex);
            }
        }
    }
}

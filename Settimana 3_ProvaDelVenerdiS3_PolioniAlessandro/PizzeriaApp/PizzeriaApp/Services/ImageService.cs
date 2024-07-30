using Microsoft.AspNetCore.Http;
using PizzeriaApp.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

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
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error converting image to Base64.", ex);
            }
        }

        public async Task<IFormFile> ConvertBase64ToFileAsync(string base64String, string fileName)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                throw new ArgumentException("Base64 string cannot be null or empty.", nameof(base64String));
            }

            try
            {
                byte[] fileBytes = Convert.FromBase64String(base64String);
                var stream = new MemoryStream(fileBytes);
                var file = new FormFile(stream, 0, fileBytes.Length, "file", fileName);
                return await Task.FromResult(file);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error converting Base64 to file.", ex);
            }
        }
    }
}

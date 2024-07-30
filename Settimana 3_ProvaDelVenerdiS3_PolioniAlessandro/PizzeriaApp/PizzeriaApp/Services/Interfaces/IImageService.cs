namespace PizzeriaApp.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> ConvertImageToBase64Async(IFormFile imageFile);
        Task<IFormFile> ConvertBase64ToFileAsync(string base64String, string fileName);
    }
}
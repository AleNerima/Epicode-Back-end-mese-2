namespace PizzeriaApp.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> ConvertImageToBase64Async(IFormFile imageFile);
    }
}
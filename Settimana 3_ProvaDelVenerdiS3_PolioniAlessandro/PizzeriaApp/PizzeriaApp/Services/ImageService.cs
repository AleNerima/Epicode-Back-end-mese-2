using PizzeriaApp.Services.Interfaces;


namespace PizzeriaApp.Services
{
    public class ImageService : IImageService
    {
        // Converte un file immagine in una stringa Base64
        public async Task<string> ConvertImageToBase64Async(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }

            try
            {
                // Utilizza un MemoryStream per leggere il contenuto del file
                using (var memoryStream = new MemoryStream())
                {
                    // Copia il contenuto del file nel MemoryStream
                    await imageFile.CopyToAsync(memoryStream);
                    // Converte il contenuto del MemoryStream in un array di byte
                    var imageBytes = memoryStream.ToArray();
                    // Converte l'array di byte in una stringa Base64 e restituisce il risultato
                    return Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error converting image to Base64.", ex);
            }
        }

        // Converte una stringa Base64 in un file IFormFile
        public async Task<IFormFile> ConvertBase64ToFileAsync(string base64String, string fileName)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                throw new ArgumentException("Base64 string cannot be null or empty.", nameof(base64String));
            }

            try
            {
                // Converte la stringa Base64 in un array di byte
                byte[] fileBytes = Convert.FromBase64String(base64String);
                // Crea un MemoryStream dal array di byte
                var stream = new MemoryStream(fileBytes);
                // Crea un FormFile dal MemoryStream e restituisce il risultato
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

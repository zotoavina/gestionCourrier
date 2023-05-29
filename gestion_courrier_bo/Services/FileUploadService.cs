namespace gestion_courrier_bo.Services
{
    public class FileUploadService : IFileUploadService
    {
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            // Implement the file upload logic here
            // Example: Save the file to a specific directory or process its contents

            // Generate a unique file name
            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            // Save the file to a specific directory
            string filePath = Path.Combine("Uploads", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}

namespace gestion_courrier_bo.Services
{
    public interface IFileUploadService
    {
        string UploadFileAsync(IFormFile file);
    }
}

namespace EventEase.Services;

public interface IBlobService
{
    // This is just a definition. It has no fields.
    Task<string> UploadFileAsync(IFormFile file, string containerName);
}
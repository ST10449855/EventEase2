using Azure.Storage.Blobs;

namespace EventEase.Services;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(IConfiguration configuration)
    {
        // FIX: Force the library to use a version Azurite understands
        var options = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2023_11_03);

        // This connects using the connection string from appsettings.json and the new options
        _blobServiceClient = new BlobServiceClient(
            configuration.GetConnectionString("AzureBlobStorage"),
            options);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string containerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        // This was where the error happened
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
        await blobClient.UploadAsync(file.OpenReadStream(), true);

        return blobClient.Uri.ToString();
    }
}
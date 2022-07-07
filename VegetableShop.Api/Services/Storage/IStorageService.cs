namespace VegetableShop.Api.Services.Storage
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);

        Task<bool> DeleteFileAsync(string fileName);

        Task<bool> DeleteFilePathAsync(string filePath);
    }
}

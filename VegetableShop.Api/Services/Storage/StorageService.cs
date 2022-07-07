namespace VegetableShop.Api.Services.Storage
{
    public class StorageService : IStorageService
    {
        private readonly string _contentFolder;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const string CONTENT_FOLDER_NAME = "Images/Products";

        public StorageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _contentFolder = Path.Combine(webHostEnvironment.WebRootPath, CONTENT_FOLDER_NAME);
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_contentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
                return true;
            }
            return false;
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{CONTENT_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_contentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task<bool> DeleteFilePathAsync(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            return await DeleteFileAsync(fileName);
        }
    }
}

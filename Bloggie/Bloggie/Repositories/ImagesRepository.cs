using CloudinaryDotNet;

namespace Bloggie.Repositories
{
    public class ImagesRepository : IImagesRepository
    {
        private readonly Account account;
        public ImagesRepository(IConfiguration configuration)
        {
            account = new Account(configuration.GetSection("Cloundinary")["CloundName"], configuration.GetSection("Cloundinary")["ApiKey"], configuration.GetSection("Cloundinary")["ApiSecret"]);
        }

        public IConfiguration Configuration { get; }

        public async Task<string> UploadAsynce(IFormFile file)
        {
            var client = new Cloudinary(account);
            var uploadResult = await client.UploadAsync(
                  new CloudinaryDotNet.Actions.ImageUploadParams
                  {
                      File = new FileDescription(file.FileName, file.OpenReadStream()),
                      DisplayName = file.FileName,
                      UploadPreset = "n60t718n"
                  });
            if(uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            return null;
        }
    }
}

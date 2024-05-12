namespace Bloggie.Repositories
{
    public interface IImagesRepository
    {
        Task<string> UploadAsynce(IFormFile file);
    }
}

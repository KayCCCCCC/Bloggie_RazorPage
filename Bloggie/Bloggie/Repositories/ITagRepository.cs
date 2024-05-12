using Bloggie.Models.Domain;

namespace Bloggie.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
    }
}

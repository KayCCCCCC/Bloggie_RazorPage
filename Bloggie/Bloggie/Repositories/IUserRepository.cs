using Microsoft.AspNetCore.Identity;

namespace Bloggie.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
        Task<bool> Add(IdentityUser identityUser, string password, List<string> roles);
        Task<bool> Delete(Guid id);
    }
}

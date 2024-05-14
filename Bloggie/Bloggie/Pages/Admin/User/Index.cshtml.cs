using Bloggie.Models.ViewModels;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Pages.Admin.User
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IUserRepository userRepository;

        [BindProperty]
        public List<Users> UserViewModel { get; set; }

        [BindProperty]
        public AddUser AddUser { get; set; }

        [BindProperty]
        public Guid UserIdSelected { get; set; }
        public IndexModel(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<IActionResult> OnGet()
        {
            var users = await userRepository.GetAll();
            UserViewModel = new List<Users>();
            foreach (var user in users)
            {
                UserViewModel.Add(new Models.ViewModels.Users()
                {
                    Id = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    Email = user.Email
                });
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var identityUser = new IdentityUser
            {
                UserName = AddUser.UserName,
                Email = AddUser.Email,
            };

            var roles = new List<string>() { "User" };
            if (AddUser.AdminCheckBox)
            {
                roles.Add("Admin");
            }

            var result = await userRepository.Add(identityUser, AddUser.Password, roles);
            if (result)
            {
                ViewData["Notification"] = new Notification
                {
                    Message = "New user created!",
                    Type = Enums.NotificationType.Success
                };
                return RedirectToPage("/Admin/User/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDelete()
        {
            await userRepository.Delete(UserIdSelected);
            return RedirectToPage("/Admin/User/Index");
        }
    }
}

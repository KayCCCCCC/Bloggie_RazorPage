using Bloggie.Models.Domain;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Pages.Blog
{
    public class DetailsModel : PageModel
    {
        private readonly IBlogPostRepository blogPostRepository;
        [BindProperty]
        public BlogPost BlogDetail { get; set; }
        public DetailsModel(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }
        public async Task<IActionResult> OnGet(string urlHandle)
        {
            BlogDetail = await blogPostRepository.GetAsync(urlHandle);
            return Page();
        }
    }
}

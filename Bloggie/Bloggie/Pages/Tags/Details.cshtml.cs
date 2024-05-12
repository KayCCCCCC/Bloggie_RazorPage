using Bloggie.Models.Domain;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Pages.Tags
{
    public class DetailsModel : PageModel
    {
        private readonly IBlogPostRepository blogPostRepository;

        [BindProperty]
        public List<BlogPost> BlogPosts { get; set; }

        public DetailsModel(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }
        public async Task<IActionResult> OnGet(string tagName)
        {
            BlogPosts = (await blogPostRepository.GetAllAsync(tagName)).ToList();
            return Page();
        }
    }
}

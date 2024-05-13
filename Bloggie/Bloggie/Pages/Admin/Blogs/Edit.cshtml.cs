using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Bloggie.Pages.Admin.Blogs
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IBlogPostRepository blogPostRepository;

        [BindProperty]
        public BlogPost blogPost { get; set; }

        [BindProperty]
        public IFormFile FeaturedImage { get; set; }

        [BindProperty]
        public string Tags { get; set; }

        public EditModel(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            blogPost = await blogPostRepository.GetAsync(id);
            if (blogPost.Tags != null)
            {
                Tags = string.Join(',', blogPost.Tags.Select(x => x.Name));
            }
            if (blogPost == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEdit()
        {
            try
            {
                blogPost.Tags = new List<Tag>(Tags.Split(',').Select(x => new Tag() { Name = x.Trim() }));
                await blogPostRepository.UpdateAsync(blogPost);
                ViewData["Notification"] = new Notification
                {
                    Message = "Record updated successfully",
                    Type = Enums.NotificationType.Success,
                };
            }
            catch (Exception ex)
            {
                await blogPostRepository.UpdateAsync(blogPost);
                ViewData["Notification"] = new Notification
                {
                    Message = "Something went wrong",
                    Type = Enums.NotificationType.Error,
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDelete()
        {
            var result = await blogPostRepository.DeleteAsync(blogPost.Id);
            if (result)
            {
                var notification = new Notification
                {
                    Message = "Blog post was deleted!",
                    Type = Enums.NotificationType.Success
                };
                TempData["Notification"] = JsonSerializer.Serialize(notification);
                return RedirectToPage("/Admin/Blogs/List");
            }

            return Page();
        }
    }
}

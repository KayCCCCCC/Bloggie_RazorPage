using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Pages.Blog
{
    public class DetailsModel : PageModel
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        [BindProperty]
        public BlogPost BlogDetail { get; set; }

        [BindProperty]
        public int TotalLikes { get; set; }

        [BindProperty]
        public bool Liked { get; set; }

        [BindProperty]
        public Guid BlogPostId { get; set; }

        [BindProperty]
        public string CommentDescription { get; set; }

        [BindProperty]
        public List<BlogComment> BlogComments { get; set; }
        public DetailsModel(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }

        public async Task<IActionResult> OnGet(string urlHandle)
        {
            BlogDetail = await blogPostRepository.GetAsync(urlHandle);
            if (BlogDetail != null)
            {
                BlogPostId = BlogDetail.Id;
                if (signInManager.IsSignedIn(User))
                {
                    var likes = await blogPostLikeRepository.GetLikesForBlog(BlogDetail.Id);
                    var userId = userManager.GetUserId(User);

                    //check have liked?
                    Liked = likes.Any(x => x.UserId == Guid.Parse(userId));

                    //get blog comment
                    await GetComments();
                }

                TotalLikes = await blogPostLikeRepository.GetTotalLikesForBlog(BlogDetail.Id);
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(string urlHandle)
        {
            if (signInManager.IsSignedIn(User) && !string.IsNullOrWhiteSpace(CommentDescription))
            {
                var userId = userManager.GetUserId(User);
                var comment = new BlogPostComment()
                {
                    BlogPostId = BlogPostId,
                    Description = CommentDescription,
                    DateAdded = DateTime.Now,
                    UserId = Guid.Parse(userId),
                };
                await blogPostCommentRepository.AddAsync(comment);
            }
            return RedirectToPage("/Blog/Details", new { urlHandle = urlHandle });
        }

        private async Task GetComments()
        {
            var blogPostComments = await blogPostCommentRepository.GetAllAsync(BlogDetail.Id);
            var blogCommentsViewModel = new List<BlogComment>();

            blogPostComments = blogPostComments.OrderByDescending(comment => comment.DateAdded);

            foreach (var comment in blogPostComments)
            {
                blogCommentsViewModel.Add(new BlogComment()
                {
                    DateAdded = comment.DateAdded,
                    Description = comment.Description,
                    UserName = (await userManager.FindByIdAsync(comment.UserId.ToString())).UserName
                });
            }

            // Gán danh sách ?ã s?p x?p vào thu?c tính BlogComments
            BlogComments = blogCommentsViewModel;
        }

    }
}

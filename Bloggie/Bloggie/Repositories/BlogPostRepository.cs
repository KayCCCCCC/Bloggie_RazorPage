using Bloggie.Data;
using Bloggie.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.BlogPosts.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingBlogPost = bloggieDbContext.BlogPosts.FirstOrDefault(blog => blog.Id == id);
            if (existingBlogPost != null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlogPost);
                await bloggieDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            var BlogPosts = await bloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
            return BlogPosts;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync(string tagName)
        {
            var BlogPosts = await (bloggieDbContext.BlogPosts.Include(x => x.Tags).Where(x => x.Tags.Any(x => x.Name == tagName))).ToListAsync();
            return BlogPosts;
        }

        public async Task<BlogPost> GetAsync(Guid id)
        {
            var blogPost = await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(blog => blog.Id == id);
            return blogPost;
        }

        public async Task<BlogPost> GetAsync(string urlHandle)
        {
            var blogPost = await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(blog => blog.UrlHandle == urlHandle);
            return blogPost;
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(blog => blog.Id == blogPost.Id);
            if (existingBlogPost != null)
            {
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.ShortDescripttion = blogPost.ShortDescripttion;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.UrlHandle = blogPost.UrlHandle;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.Visible = blogPost.Visible;

                if (blogPost.Tags != null && blogPost.Tags.Any())
                {
                    // Delete the existing tags
                    bloggieDbContext.Tags.RemoveRange(existingBlogPost.Tags);
                    // Add new tags
                    blogPost.Tags.ToList().ForEach(x => x.BlogPostId = existingBlogPost.Id);
                    await bloggieDbContext.Tags.AddRangeAsync(blogPost.Tags);
                }
            }
            bloggieDbContext.Update(existingBlogPost);
            await bloggieDbContext.SaveChangesAsync();
            return existingBlogPost;
        }
    }
}

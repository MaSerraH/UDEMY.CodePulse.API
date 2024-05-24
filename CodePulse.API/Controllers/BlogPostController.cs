using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository blogpostrepository;

        public BlogPostController(IBlogPostRepository blogpostrepository)
        {
            this.blogpostrepository = blogpostrepository;
        }

        //POST: {apibaseurl}/api/blogposts

        [HttpPost]
        public async Task <IActionResult> CreateBlogPost([FromBody]CreateBlogPostRequestDto request)
        {
            
            //Convert from dto to domain

            var blogpost = new BlogPost
            {
                Author = request.Author,
                Title = request.Title,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                ShortDescription = request.ShortDescription,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate
            };



           blogpost= await blogpostrepository.CreateAsync(blogpost);

            //convert domain to dto

            var response = new BlogPost
            {
                Id = blogpost.Id,
                Author = blogpost.Author,
                Content = blogpost.Content,
                FeaturedImageUrl = blogpost.FeaturedImageUrl,
                IsVisible = blogpost.IsVisible,
                PublishedDate = blogpost.PublishedDate,
                ShortDescription = blogpost.ShortDescription,
                UrlHandle = blogpost.UrlHandle,
                Title = blogpost.Title
            };
            return Ok(response);

        }

        //GET: {apibaseurl}/api/blogposts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogposts = await blogpostrepository.GetAllAsync();

            //convert domain to dto
            var response = new List<BlogPostDTO>();
            foreach (var blogpost in blogposts)
            {
                response.Add(new BlogPostDTO
                {
                    Id = blogpost.Id,
                    Author = blogpost.Author,
                    Content = blogpost.Content,
                    FeaturedImageUrl = blogpost.FeaturedImageUrl,
                    IsVisible = blogpost.IsVisible,
                    PublishedDate = blogpost.PublishedDate,
                    ShortDescription = blogpost.ShortDescription,
                    UrlHandle = blogpost.UrlHandle,
                    Title = blogpost.Title
                });
            }
            return Ok(response);
        }
    }
}

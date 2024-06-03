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
        private readonly ICategoryRepository categoryRepository;

        public BlogPostController(IBlogPostRepository blogpostrepository,
            ICategoryRepository categoryRepository)
        {
            this.blogpostrepository = blogpostrepository;
            this.categoryRepository = categoryRepository;
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
                PublishedDate = request.PublishedDate,
                Categories = new List <Category>()
            };

            foreach(var categoryGuid  in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if(existingCategory != null) 
                { 
                    blogpost.Categories.Add(existingCategory);
                }
            }

           blogpost= await blogpostrepository.CreateAsync(blogpost);

            //convert domain to dto

            var response = new BlogPostDTO
            {
                Id = blogpost.Id,
                Author = blogpost.Author,
                Content = blogpost.Content,
                FeaturedImageUrl = blogpost.FeaturedImageUrl,
                IsVisible = blogpost.IsVisible,
                PublishedDate = blogpost.PublishedDate,
                ShortDescription = blogpost.ShortDescription,
                UrlHandle = blogpost.UrlHandle,
                Title = blogpost.Title,
                Categories = blogpost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
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
                    Title = blogpost.Title,
                    Categories = blogpost.Categories.Select(x => new CategoryDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()

                });
            }
            return Ok(response);
        }

        //GET:{apibaseurl}/api/blogposts/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute]Guid id)
        {
            //Get the blogposts from repository

            var blogpost = await blogpostrepository.GetByIdAsync(id);
            if (blogpost == null)
            {
                return NotFound();
            }

            //convert domain to dto

            var response = new  BlogPostDTO
            {
                Id = blogpost.Id,
                Author = blogpost.Author,
                Content = blogpost.Content,
                FeaturedImageUrl = blogpost.FeaturedImageUrl,
                IsVisible = blogpost.IsVisible,
                PublishedDate = blogpost.PublishedDate,
                ShortDescription = blogpost.ShortDescription,
                UrlHandle = blogpost.UrlHandle,
                Title = blogpost.Title,
                Categories = blogpost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok (response);

        }

        //put:{apibaseurle}/api/blogpost/{ig}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute]Guid id, UpdateBlogPostRequestDTO request)
        {
            //convert from dto to domain

            var blogPost = new BlogPost
            {
                Id = id,
                Author = request.Author,
                Title = request.Title,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                ShortDescription = request.ShortDescription,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
              var existingCategory =  await categoryRepository.GetById(categoryGuid);

                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            var updatetBlogPost = await blogpostrepository.UpdateAsync(blogPost);

            if (updatetBlogPost == null)
            {
                return NotFound();
            }

            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Title = blogPost.Title,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);

        }
        //delete: {apibaseurl}/api/blogpost/{id}

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var deletedBlogPost = await blogpostrepository.DeleteAsync(id);

            if (deletedBlogPost == null)
            {
                return NotFound();
            }
            //convert domAIN TO DDTO

            var response = new BlogPostDTO
            {
                Id = deletedBlogPost.Id,
                Author = deletedBlogPost.Author,
                Content = deletedBlogPost.Content,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                IsVisible = deletedBlogPost.IsVisible,
                PublishedDate = deletedBlogPost.PublishedDate,
                ShortDescription = deletedBlogPost.ShortDescription,
                UrlHandle = deletedBlogPost.UrlHandle,
                Title = deletedBlogPost.Title,
            };

            return Ok(response);


        }
    }
}

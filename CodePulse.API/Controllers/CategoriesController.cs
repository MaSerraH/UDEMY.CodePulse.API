using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    //https://localhost:xxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }


        // 
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDTO request)
        {
            //map DTO to Domain Model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await categoryRepository.CreateAsync(category);


            //map From Domain to DTO

            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            return Ok(response);

        }

        //GET: https://localhost:7161/api/Categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();

            //map domain model to dto
            var response = new List<CategoryDTO>();
            foreach (var categoriedomain in categories)
            {
                response.Add(new CategoryDTO
                {
                    Id= categoriedomain.Id,
                    Name = categoriedomain.Name,
                    UrlHandle= categoriedomain.UrlHandle
                });
            }
            return Ok(response);    
        }

        //Get: https://localhost:7161/api/Categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            var response = new CategoryDTO
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };
            return Ok(response);
        }


        //PUT: https://localhost:7161/api/Categories/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, UpdateCategoryRequestDto request )
        {
            //Convert DTO to domain
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            category = await categoryRepository.UpdateAsync(category);

            if (category == null)
            {
                return NotFound();  
            }

            //Convert Domain model to DTO
            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }

        //delete: https://localhost:7161/api/Categories/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);

            if (category is null)
            {
                return NotFound();
            }
            //convert domain model to dto
            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RMS.Application.Category.DTO;
using RMS.Application.Helpers.IFileStorage;
using RMS.Application.Helpers.JwtHelper;
using RMS.Services.CategoryService;

namespace RMS.Host.Controllers.CategoryController
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly IFileStorage _fileStorage;
        public CategoryController(CategoryService categoryService, IFileStorage fileStorage)
        {
            _categoryService = categoryService;
            _fileStorage = fileStorage;
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateAsync([FromForm] CategoryDto dto)
        {
            var createdBy = JwtHelper.GetUserId(User);

            var result = await _categoryService.CreateAsync(dto, createdBy);

            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateAsync([FromForm] CategoryDto dto)
        {
            var updatedBy = JwtHelper.GetUserId(User);

            var result = await _categoryService.UpdateAsync(dto, updatedBy);

            return Ok(result);
        }
    }
}



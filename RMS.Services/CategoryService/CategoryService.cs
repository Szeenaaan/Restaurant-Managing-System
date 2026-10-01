using RMS.Application.Category.DTO;
using RMS.Application.Helpers.IFileStorage;
using RMS.Application.Helpers.ResponseHelpers;
using RMS.Persistence;
using RMS.Repositories.CategoryRepository;
namespace RMS.Services.CategoryService
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly IFileStorage _fileStorage;

        public CategoryService(CategoryRepository categoryRepository, IFileStorage fileStorage)
        {
            _categoryRepository = categoryRepository;
            _fileStorage = fileStorage;
        }

        public async Task<ResponseDto> CreateAsync(CategoryDto dto, int createdby)
        {
            try
            {
                var existname = await _categoryRepository.GetByCategoryName(dto.CategoryName);

                if (existname != null)
                {
                    return new ResponseDto
                    {
                        Success = false,
                        StatusCode = 409,
                        Message = "CategoryName already exists."
                    };
                }

                var existcode = await _categoryRepository.GetByCategoryCode(dto.CategoryCode);

                if (existcode != null)
                {
                    return new ResponseDto
                    {
                        Success = false,
                        StatusCode = 409,
                        Message = "CategoryCode already exists."
                    };
                }

                string? imagePath = null;

                if (dto.DisplayImage != null)
                {
                    imagePath = await _fileStorage.SaveAsync(dto.DisplayImage);
                }

                var category = new Category
                {
                    CategoryName = dto.CategoryName,
                    CategoryCode = dto.CategoryCode,
                    Description = dto.Description,
                    DisplayImage = imagePath,
                    SortOrder = 0,
                    IsActive = true,
                    PresentStatus = "Active",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdby,
                    Version = 1
                };

                await _categoryRepository.Create(category);

                return new ResponseDto
                {
                    Success = true,
                    StatusCode = 201,
                    Message = "Category created successfully."
                };
            }
            catch
            {
                return new ResponseDto
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "An error occurred while creating the category."
                };
            }
        }

        public async Task<ResponseDto> UpdateAsync(CategoryDto dto, int updatedBy)
        {
            try
            {
                if (dto.Id == null)
                {
                    return new ResponseDto { Success = false, StatusCode = 400, Message = "Category Id is required." };
                }

                var category = await _categoryRepository.ExistId(dto.Id.Value);

                if (category == null)
                {
                    return new ResponseDto { Success = false, StatusCode = 404, Message = "Category not found." };
                }

                var existName = await _categoryRepository.GetByCategoryName(dto.CategoryName);

                if (existName != null && existName.Id != dto.Id.Value)
                {
                    return new ResponseDto { Success = false, StatusCode = 409, Message = "CategoryName already exists." };
                }

                var existCode = await _categoryRepository.GetByCategoryCode(dto.CategoryCode);

                if (existCode != null && existCode.Id != dto.Id.Value)
                {
                    return new ResponseDto { Success = false, StatusCode = 409, Message = "CategoryCode already exists." };
                }

                var oldImagePath = category.DisplayImage;

                string? newImagePath = null;

                if (dto.DisplayImage != null)
                {
                    newImagePath = await _fileStorage.SaveAsync(dto.DisplayImage);

                    category.DisplayImage = newImagePath;
                }

                category.CategoryName = dto.CategoryName;
                category.CategoryCode = dto.CategoryCode;
                category.Description = dto.Description;
                category.UpdatedAt = DateTime.UtcNow;
                category.UpdatedBy = updatedBy;
                category.Version++;

                await _categoryRepository.Update(category);

                if (newImagePath != null && oldImagePath != null)
                {
                    _fileStorage.Delete(oldImagePath);
                }

                return new ResponseDto { Success = true, StatusCode = 200, Message = "Category updated successfully." };
            }
            catch (Exception)
            {
                return new ResponseDto { Success = false, StatusCode = 500, Message = "An error occurred while updating the category." };
            }
        }

        public async Task<ApiResponseDto<GetIdDto>> GetByIdAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetById(id);

                if (category == null)
                {
                    return new ApiResponseDto<GetIdDto> { Success = false, StatusCode = 404, Message = "Category not found." };
                }

                return new ApiResponseDto<GetIdDto>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Category retrieved successfully.",
                    Data = category
                };
            }
            catch
            {
                return new ApiResponseDto<GetIdDto> { Success = false, StatusCode = 500, Message = "An error occurred while retrieving the category." };
            }
        }

        public async Task<ApiResponseDto<List<GetAllDto>>> GetAllAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();

                return new ApiResponseDto<List<GetAllDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Categories retrieved successfully.",
                    Data = categories
                };
            }
            catch
            {
                return new ApiResponseDto<List<GetAllDto>> { Success = false, StatusCode = 500, Message = "An error occurred while retrieving the categories." };
            }
        }

        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.ExistId(id);

                if (category == null)
                {
                    return new ResponseDto { Success = false, StatusCode = 404, Message = "Category not found." };
                }

                await _categoryRepository.Delete(category);

                if (!string.IsNullOrEmpty(category.DisplayImage))
                {
                    _fileStorage.Delete(category.DisplayImage);
                }

                return new ResponseDto { Success = true, StatusCode = 200, Message = "Category deleted successfully." };
            }
            catch
            {
                return new ResponseDto { Success = false, StatusCode = 500, Message = "An error occurred while deleting the category." };
            }
        }

        public async Task<ApiResponseDto<List<GetAllDto>>> GetQueryAsync(GetQueryDto dto)
        {
            try
            {
                var categories = await _categoryRepository.GetQueryAsync(dto);

                return new ApiResponseDto<List<GetAllDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Categories retrieved successfully.",
                    Data = categories
                };
            }
            catch
            {
                return new ApiResponseDto<List<GetAllDto>> { Success = false, StatusCode = 500, Message = "An error occurred while retrieving the categories." };
            }
        }
    }
}
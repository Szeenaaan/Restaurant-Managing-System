using Microsoft.EntityFrameworkCore;
using RMS.Application.Category.DTO;
using RMS.Persistence;
namespace RMS.Repositories.CategoryRepository
{
    public class CategoryRepository
    {

        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task Create(Category category)
        {
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
        }
        public async Task Update(Category category)
        {
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
        }
        public async Task Delete(Category category)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }

        public async Task<Category?> ExistId(int id)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<GetIdDto?> GetById(int id)
        {
            return await _db.Categories.Where(x => x.Id == id)
                .Select(x => new GetIdDto
                {
                    Id = x.Id,
                    CategoryName = x.CategoryName,
                    CategoryCode = x.CategoryCode,
                    DisplayImage = x.DisplayImage
                }).FirstOrDefaultAsync();
        }


        public async Task<List<GetAllDto>> GetAllAsync()
        {
            return await _db.Categories.OrderBy(c => c.SortOrder)
                .Select(category => new GetAllDto
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                    CategoryCode = category.CategoryCode,
                    Description = category.Description,
                    DisplayImage = category.DisplayImage,
                    IsActive = category.IsActive,
                    PresentStatus = category.PresentStatus
                }).ToListAsync();
        }

        public async Task<List<GetAllDto>> GetQueryAsync(GetQueryDto dto)
        {
            var query = _db.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.SearchText))
            {
                var search = $"%{dto.SearchText}%";

                query = query.Where(c =>
                    EF.Functions.ILike(c.CategoryName, search) ||
                    EF.Functions.ILike(c.CategoryCode, search));
            }

            if (!string.IsNullOrWhiteSpace(dto.CategoryName))
            {
                var categoryName = $"%{dto.CategoryName}%";

                query = query.Where(c => EF.Functions.ILike(c.CategoryName, categoryName));
            }

            if (!string.IsNullOrWhiteSpace(dto.CategoryCode))
            {
                var categoryCode = $"%{dto.CategoryCode}%";

                query = query.Where(c => EF.Functions.ILike(c.CategoryCode, categoryCode));
            }

            return await query.OrderBy(c => c.SortOrder).ThenBy(c => c.Id).Skip(dto.Offset).Take(dto.Limit)
                .Select(c => new GetAllDto
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                    CategoryCode = c.CategoryCode,
                    Description = c.Description,
                    DisplayImage = c.DisplayImage,
                    IsActive = c.IsActive,
                    PresentStatus = c.PresentStatus
                }).ToListAsync();
        }

        public async Task<Category?> GetByCategoryName(string categoryName)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);
        }


        public async Task<Category?> GetByCategoryCode(string categoryCode)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.CategoryCode == categoryCode);
        }
    }
}
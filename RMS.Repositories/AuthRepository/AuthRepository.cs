using Microsoft.EntityFrameworkCore;
using RMS.Application.Authentication.DTO;
using RMS.Persistence;

namespace RMS.Repositories.AuthRepository
{
    public class AuthRepository
    {
        private readonly AppDbContext _db;

        public AuthRepository(AppDbContext db)
        {
            _db = db;
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _db.Users
                .FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<Roles?> GetActiveRoleAsync(int roleId)
        {
            return await _db.Roles
                .FirstOrDefaultAsync(x => x.Id == roleId && x.IsActive);
        }

        public async Task CreateAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task<List<GetAllDto>> GetAllAsync()
        {
            return await _db.Users
                .Select(user => new GetAllDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    RoleName = user.Role!.Name
                })
                .ToListAsync();
        }
    }
}



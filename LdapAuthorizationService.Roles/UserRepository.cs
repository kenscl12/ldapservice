using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LdapAuthorizationService.Common.Extensions;
using LdapAuthorizationService.Repository.DbContext;
using LdapAuthorizationService.Repository.Entities;
using LdapAuthorizationService.Users.Internal;

namespace LdapAuthorizationService.Users
{
    /// <summary>
    /// Репозиторий пользователей
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        ///  Бд контекст базы
        /// </summary>
        private readonly AuthServiceDbContext _dbContext;

        /// <summary>
        ///  Конструктор
        /// </summary>
        public UserRepository(AuthServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Поиск пользователя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public async Task<DbUser> FindUser(Guid id)
        {
            var result = await _dbContext.Users
                .Where(u => u.Id.Equals(id))
                .Include(u => u.User2Roles)
                .ThenInclude(t => t.Role)
                .FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Поиск наличия пользователя по логину
        /// </summary>
        /// <param name="login">Логин</param>
        public async Task<bool> ExistUser(string login)
        {
            var result = await _dbContext.Users
                .Where(u => u.Login.ToLower().Equals(login.ToLower()))
                .AnyAsync();

            return result;
        }

        /// <summary>
        /// Поиск пользователя по логину
        /// </summary>
        /// <param name="login">Логин</param>
        public async Task<DbUser> FindUser(string login)
        {
            var result = await _dbContext.Users
                .Include(u => u.User2Roles)
                    .ThenInclude(t => t.Role)
                .Where(u => u.Login.ToLower().Equals(login.ToLower()))
                .FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Поиск пользователей групп
        /// </summary>
        /// <param name="login">Логин</param>
        public async Task<List<DbUser>> FindUserByGroups(List<string> groups)
        {
            if (groups == null || !groups.Any())
                return null;

            groups.ForEach(g => g.ToLower());

            var result = await _dbContext.Users
                .Include(u => u.User2Roles)
                    .ThenInclude(t => t.Role)
                .Where(u => groups.Contains(u.Login.ToLower()))
                .ToListAsync();

            return result;
        }

        public async Task<List<DbUser>> FindUsers(int? page, int? pageSize)
        {
            var users = _dbContext.Users
                .Include(u => u.User2Roles)
                    .ThenInclude(t => t.Role)
                .Include(u => u.UserStatus)
                .OrderBy(u => u.Status);

            if (!page.HasValue && !pageSize.HasValue)
                return await users.ToListAsync();

            return await users.Paginate(page.Value, pageSize.Value).ToListAsync();
        }

        public async Task<int> GetUsersCount()
		{
            return await _dbContext.Users.CountAsync();
		}

        public async Task DeleteUser2Roles(ICollection<DbUser2Role> dbUser2Roles)
        {
            if (dbUser2Roles == null || !dbUser2Roles.Any())
                return;

            foreach (var dbUser2Role in dbUser2Roles.ToList())
                await DeleteUser2Role(dbUser2Role);
        }

        private async Task DeleteUser2Role(DbUser2Role dbUser2Role)
        {
            if (dbUser2Role == null)
                return;

            _dbContext.User2Roles.Remove(dbUser2Role);

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddUser2Role(Guid[] roles, Guid userId)
        {
            if (roles == null || !roles.Any())
                return;

            foreach (var role in roles.ToList())
                _dbContext.User2Roles.Add(new DbUser2Role()
                {
                    Id = Guid.NewGuid(),
                    RoleId = role,
                    UserId = userId
                });

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddUser(DbUser dbUser)
        {
            _dbContext.Users.Add(dbUser);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<DbRole>> GetRoles()
        {
            return await _dbContext.Roles
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<DbUserStatus>> GetUserStatuses()
        {
            return await _dbContext.UserStatuses
                .AsNoTracking().ToListAsync();
        }

        public async Task<DbRole> FindRole(Guid id)
        {
            return await _dbContext.Roles
                .Where(u => u.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task DeleteUserRoles(Guid id)
        {
            var userRoleToDelete = await _dbContext.Users.Include(u => u.User2Roles).SingleAsync(u => u.Id.Equals(id));

            userRoleToDelete.Status = nameof(UserStatus.Disabled);
            userRoleToDelete.ModifiedDate = DateTime.UtcNow;

            _dbContext.Users.Update(userRoleToDelete);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<DbUser> ChangeUserStatus(Guid id, string status)
        {
            var user = await _dbContext.Users.SingleAsync(u => u.Id.Equals(id));

			user.Status = status;
			user.ModifiedDate = DateTime.UtcNow;

            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}

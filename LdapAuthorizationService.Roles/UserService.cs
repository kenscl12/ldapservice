using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LdapAuthorizationService.Common.Exceptions;
using LdapAuthorizationService.Repository.Entities;
using LdapAuthorizationService.Users.Extensions;
using LdapAuthorizationService.Users.Internal;

namespace LdapAuthorizationService.Users
{
    /// <summary>
    ///  Сервис пользователей
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        ///  Репозиторий для работы с пользователями
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        ///  Конструктор
        /// </summary>
        public UserService(IUserRepository userRepository)
        {
			_userRepository = userRepository;
        }
        
        /// <summary>
        ///  Возвращает пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        public async Task<User> GetUser(string login)
        {
            var dbUser = await _userRepository.FindUser(login);

            return dbUser.ConvertToUser();
        }
        
        /// <summary>
        ///  Возвращает пользователей групп
        /// </summary>
        /// <param name="login">Логин</param>
        public async Task<List<User>> GetUserGroups(List<string> groups)
        {
            var dbUsers = await _userRepository.FindUserByGroups(groups);
            if (dbUsers == null || !dbUsers.Any())
                return null;

            return dbUsers.ConvertToUsers();
        }

        /// <summary>
        ///  Возвращает пользователей
        /// </summary>
        /// <param name="userRolesFilter">Фильтр пользователей</param>
        public async Task<UsersResponse> GetUsers(UserRolesFilter userRolesFilter)
        {
            var users = await _userRepository.FindUsers(userRolesFilter.Page, userRolesFilter.PageSize);

            return new UsersResponse
            {
                Users = users.ConvertToUsers(),
                PageCount = GetPageCount(userRolesFilter.PageSize, await _userRepository.GetUsersCount())
            };
        }

        /// <summary>
        ///  Обновление ролей пользователя
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="updateUserRoleRequest">Фильтр обновления ролей пользователя</param>
        public async Task<User> UpdateUserRole(Guid id, UpdateUserRoleRequest updateUserRoleRequest)
        {
            foreach (var role in updateUserRoleRequest.Roles)
                if ((await _userRepository.FindRole(role)) == null)
                    throw new BackendException("Unknown role");

            var dbUser = await _userRepository.FindUser(id);

            await _userRepository.DeleteUser2Roles(dbUser.User2Roles);

            if (updateUserRoleRequest.Roles == null || !updateUserRoleRequest.Roles.Any())
                return (await _userRepository.FindUser(id)).ConvertToUser();

            await _userRepository.AddUser2Role(updateUserRoleRequest.Roles, id);

            return (await _userRepository.FindUser(id)).ConvertToUser();
        }

        /// <summary>
        ///  Добавления ролей пользователя
        /// </summary>
        /// <param name="updateUserRoleRequest">Фильтр добавления ролей пользователя</param>
        public async Task<User> AddUserRole(AddAddRoleToUserRequest addUserRoleRequest)
        {
            var dbUserId = Guid.NewGuid();

            if (await _userRepository.ExistUser(addUserRoleRequest.Login))
                throw new BackendException("User already exists");

            foreach (var role in addUserRoleRequest.Roles)
                if ((await _userRepository.FindRole(role)) == null)
                    throw new BackendException("Unknown role");

            await _userRepository.AddUser(new DbUser()
            {
                Login = addUserRoleRequest.Login,
                ExternalId = addUserRoleRequest.ExternalId,
                Id = dbUserId,
                UserType = (byte)addUserRoleRequest.UserType,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            });
            await _userRepository.AddUser2Role(addUserRoleRequest.Roles, dbUserId);

            var newUser = await _userRepository.FindUser(dbUserId);
            return newUser.ConvertToUser();
        }

        /// <summary>
        ///  Получение ролей
        /// </summary>
        public async Task<List<Role>> GetRoles()
        {
            var roles = await _userRepository.GetRoles();

            return roles.ConvertToRoles();
        }

        /// <summary>
        ///  Получение статусов
        /// </summary>
        public async Task<List<CodeWithDescription>> GetUserStatuses()
        {
            var userStatuses = await _userRepository.GetUserStatuses();

            return userStatuses.ConvertToUserStatuses();
        }

        /// <summary>
        ///  Удаление ролей пользователя
        /// </summary>
        public async Task DeleteUserRole(Guid id)
        {
            await _userRepository.DeleteUserRoles(id);
        }

        /// <summary>
        ///  Изменение статуса пользователя
        /// </summary>
        public async Task<User> ChangeUserStatus(Guid userId, string status)
        {
            var user = await _userRepository.ChangeUserStatus(userId, status);

            return user.ConvertToUser();
        }

        /// <summary>
        /// Получает количество страниц
        /// </summary>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="itemsCount">Количество записей</param>
        private int GetPageCount(int? pageSize, int itemsCount)
        {
            if (!pageSize.HasValue)
                return 1;

            var pageCount = itemsCount / pageSize.Value;
            if ((pageCount * pageSize.Value) < itemsCount)
                pageCount += 1;

            return pageCount;
        }
    }
}

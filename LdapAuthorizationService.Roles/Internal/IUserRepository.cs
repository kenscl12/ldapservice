using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LdapAuthorizationService.Repository.Entities;

namespace LdapAuthorizationService.Users.Internal
{
	/// <summary>
	/// Интерфейс репозитория пользователей
	/// </summary>
	public interface IUserRepository
	{
		/// <summary>
		/// Поиск пользователя по идентификатору
		/// </summary>
		/// <param name="id">Идентификатор</param>
		Task<DbUser> FindUser(Guid id);

		/// <summary>
		/// Поиск наличия пользователя по логину
		/// </summary>
		/// <param name="login">Логин</param>
		Task<bool> ExistUser(string login);

		/// <summary>
		/// Поиск пользователя по логину
		/// </summary>
		/// <param name="login">Логин</param>
		Task<DbUser> FindUser(string login);

		/// <summary>
		/// Поиск пользователей групп
		/// </summary>
		/// <param name="login">Логин</param>
		Task<List<DbUser>> FindUserByGroups(List<string> groups);

		Task<List<DbUser>> FindUsers(int? page, int? pageSize);

		Task<int> GetUsersCount();

		Task DeleteUser2Roles(ICollection<DbUser2Role> dbUser2Roles);
		Task AddUser2Role(Guid[] roles, Guid userId);
		Task AddUser(DbUser dbUser);

		Task<List<DbRole>> GetRoles();
		Task<DbRole> FindRole(Guid id);
		Task DeleteUserRoles(Guid id);

		Task<List<DbUserStatus>> GetUserStatuses();

		Task<DbUser> ChangeUserStatus(Guid id, string status);
	}
}

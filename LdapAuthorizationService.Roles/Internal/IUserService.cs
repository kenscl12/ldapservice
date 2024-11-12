using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LdapAuthorizationService.Users.Internal
{
	/// <summary>
	///  Интерфейс сервиса пользователей
	/// </summary>
	public interface IUserService
	{
		/// <summary>
		///  Возвращает пользователя
		/// </summary>
		/// <param name="login">Логин</param>
		Task<User> GetUser(string login);

		/// <summary>
		///  Возвращает пользователей
		/// </summary>
		/// <param name="userRolesFilter">Фильтр пользователей</param>
		Task<UsersResponse> GetUsers(UserRolesFilter userRolesFilter);

		/// <summary>
		///  Добавления ролей пользователя
		/// </summary>
		/// <param name="updateUserRoleRequest">Фильтр добавления ролей пользователя</param>
		Task<User> AddUserRole(AddAddRoleToUserRequest addUserRoleRequest);

		/// <summary>
		///  Добавления ролей пользователя
		/// </summary>
		/// <param name="updateUserRoleRequest">Фильтр добавления ролей пользователя</param>
		Task DeleteUserRole(Guid id);

		/// <summary>
		///  Получение ролей
		/// </summary>
		Task<List<Role>> GetRoles();

		/// <summary>
		///  Обновление ролей пользователя
		/// </summary>
		/// <param name="id">Идентификатор</param>
		/// <param name="updateUserRoleRequest">Фильтр обновления ролей пользователя</param>
		Task<User> UpdateUserRole(Guid id, UpdateUserRoleRequest updateUserRoleRequest);

		/// <summary>
		///  Получение статусов
		/// </summary>
		Task<List<CodeWithDescription>> GetUserStatuses();

		/// <summary>
		///  Изменение статуса пользователя
		/// </summary>
		Task<User> ChangeUserStatus(Guid userId, string status);

		/// <summary>
		///  Возвращает пользователей групп
		/// </summary>
		/// <param name="login">Логин</param>
		Task<List<User>> GetUserGroups(List<string> groups);
	}
}

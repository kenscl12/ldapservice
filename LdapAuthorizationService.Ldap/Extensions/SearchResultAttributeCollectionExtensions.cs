using LdapForNet;

namespace LdapAuthorizationService.Ldap.Extensions
{
	/// <summary>
	/// Расширения для работы с результатами поиска Ldap сервиса
	/// </summary>
	public static class SearchResultAttributeCollectionExtensions
	{
		/// <summary>
		/// Аутентификация
		/// </summary>
		/// <param name="attributes">Атрибуты</param>
		/// <param name="key">Ключ</param>
		public static string GetSafeStringValue(this SearchResultAttributeCollection attributes, string key)
		{
			if (!attributes.Contains(key))
				return null;

			return attributes[key].GetValue<string>();
		}
	}
}

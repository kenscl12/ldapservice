using System;
using System.Collections.Generic;

namespace LdapAuthorizationService.Common.Exceptions
{
	/// <summary>
	/// Представление ошибки сервиса
	/// </summary>
	public class ServiceException : Exception
	{
		public ServiceException(string message) : base(message)
		{
			Data = new Dictionary<string, string>();
		}

		public ServiceException(int? code, string message) : this(message)
		{
			Code = code;
		}

		public ServiceException(string id, int? code, string message) : this(message)
		{
			Id = id;
			Code = code;
		}

		public ServiceException(string id, int? code, string message, Dictionary<string, string> data)
			: this(id, code, message)
		{
			Data = data;
		}

		/// <summary>
		/// Идентификатор
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Код
		/// </summary>
		public int? Code { get; set; }

		/// <summary>
		/// Дополнительные данные
		/// </summary>
		public new Dictionary<string, string> Data { get; set; }
	}
}

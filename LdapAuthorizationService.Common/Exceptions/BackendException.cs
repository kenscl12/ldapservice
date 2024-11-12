using System;
using System.Runtime.Serialization;

namespace LdapAuthorizationService.Common.Exceptions
{
	/// <summary>
	/// Представление общей ошибки.
	/// </summary>
	[Serializable()]
	public class BackendException : Exception
	{
		/// <summary>
		/// Инициализация экземпляра ошибки
		/// </summary>
		public BackendException()
		{
		}

		/// <summary>
		/// Инициализация экземпляра ошибки
		/// </summary>
		public BackendException(string message) : base(message)
		{
		}

		/// <summary>
		/// Инициализация экземпляра ошибки
		/// </summary>
		public BackendException(int code, string message) : this(message)
		{
			Code = code;
		}

		/// <summary>
		/// Инициализация экземпляра ошибки
		/// </summary>
		public BackendException(int code) : this(null)
		{
			Code = code;
		}

		/// <summary>
		/// Инициализация экземпляра ошибки
		/// </summary>
		protected BackendException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>
		/// Код.
		/// </summary>
		public int? Code { get; set; }
	}
}

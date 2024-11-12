using LdapAuthorizationService.Common.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace LdapAuthorizationService.Middleware
{
	/// <summary>
	/// Middleware для работы с api ошибками
	/// </summary>
	public class ApiExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ApiExceptionMiddleware> _logger;

		public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception");

				await HandleExceptionAsync(context, ex);
			}
		}

		/// <summary>
		/// Проверяет что если ошибка обработанная, то возвращает ее json объектом {code, message}
		/// </summary>
		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var response = context.Response;
			response.ContentType = "application/json";

			var backendException = exception as BackendException;
			if (backendException == null)
				return;

			response.StatusCode = (int)HttpStatusCode.BadRequest;
			await response.WriteAsync(JsonConvert.SerializeObject(new
			{
				code = backendException.Code,
				message = backendException.Message
			}));
		}
	}
}

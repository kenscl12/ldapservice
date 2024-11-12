using System;
using System.Security.Cryptography;
using System.Text;

namespace LdapAuthorizationService.Common.Helpers
{
	/// <summary>
	/// Криптография
	/// </summary>
	public static class CryptographyHelper
	{
		/// <summary>
		/// Калькуляция хэша sha256
		/// </summary>
		/// <param name="input">Значение</param>
		/// <returns>Sha256 хэш</returns>
		public static string ComputeSha256Hash(string input)
		{
			HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

			byte[] byteHash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

			return Convert.ToBase64String(byteHash);
		}
	}
}

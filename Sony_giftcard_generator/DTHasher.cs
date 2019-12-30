using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sony_giftcard_generator
{
	public sealed class DTHasher
	{
		private DTHasher()
		{
		}

		private static byte[] ConvertStringToByteArray(string data)
		{
			return new UnicodeEncoding().GetBytes(data);
		}

		private static FileStream GetFileStream(string pathName)
		{
			return new FileStream(pathName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		}

		public static string GetSHA1Hash(string code)
		{
			string result = "";
			SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			try
			{
				result = BitConverter.ToString(sHA1CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(code))).Replace("-", "");
				return result;
			}
			catch (Exception)
			{
				return result;
			}
		}

		public static string GetMD5Hash(string pathName)
		{
			string result = "";
			FileStream fileStream = null;
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			try
			{
				fileStream = GetFileStream(pathName);
				byte[] value = mD5CryptoServiceProvider.ComputeHash(fileStream);
				fileStream.Close();
				result = BitConverter.ToString(value).Replace("-", "");
				return result;
			}
			catch (Exception)
			{
				return result;
			}
		}
	}
}

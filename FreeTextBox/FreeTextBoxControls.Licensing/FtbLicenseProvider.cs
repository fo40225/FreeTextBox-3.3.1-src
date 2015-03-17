using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace FreeTextBoxControls.Licensing
{
	public class FtbLicenseProvider : LicenseProvider
	{
		private sealed class FtbLicenseCollector
		{
			private IDictionary _collectedLicenses;
			public FtbLicenseCollector()
			{
				this._collectedLicenses = new HybridDictionary();
			}
			public void AddLicense(Type objectType, FtbLicense license)
			{
				if (objectType == null)
				{
					throw new ArgumentNullException("objectType");
				}
				if (license == null)
				{
					throw new ArgumentNullException("objectType");
				}
				this._collectedLicenses[objectType] = license;
			}
			public FtbLicense GetLicense(Type objectType)
			{
				if (objectType == null)
				{
					throw new ArgumentNullException("objectType");
				}
				if (this._collectedLicenses.Count == 0)
				{
					return null;
				}
				return (FtbLicense)this._collectedLicenses[objectType];
			}
			public void RemoveLicense(Type objectType)
			{
				if (objectType == null)
				{
					throw new ArgumentNullException("objectType");
				}
				this._collectedLicenses.Remove(objectType);
			}
		}
		private static readonly byte[] encryptionKeyBytes = new byte[]
		{
			57,
			72,
			66,
			50,
			56,
			49,
			70,
			54
		};
		private static readonly FtbLicenseProvider.FtbLicenseCollector LicenseCollector = new FtbLicenseProvider.FtbLicenseCollector();
		protected virtual FtbLicense CreateLicense(Type type, string licenseData)
		{
			Match match = Regex.Match(licenseData, type.Name + " License(.|\\n)*?\\[(?<licenseType>[^\\]]+)\\](.|\\n)*?\\[(?<secondField>[^\\]]+)\\]");
			if (match.Success)
			{
				string text = this.DecryptLicenseData(match.Groups["licenseType"].Value);
				string text2 = this.DecryptLicenseData(match.Groups["secondField"].Value);
				if (text == "SingleLicense" || text == "DistributionLicense")
				{
					return new FtbLicense(type, text, text2, true);
				}
				if (!(text == "ExpiringLicense"))
				{
					return new FtbLicense(type, "BadLicenseData", text + "," + text2, false);
				}
				DateTime dateTime = default(DateTime);
				try
				{
					dateTime = Convert.ToDateTime(text2);
				}
				catch
				{
				}
				if (!(dateTime != default(DateTime)))
				{
					return new FtbLicense(type, "Invalid date", text + "," + text2, false);
				}
				if (dateTime > DateTime.Now)
				{
					return new FtbLicense(type, text, text2, true);
				}
				return new FtbLicense(type, "expired", text + "," + text2, false);
			}
			else
			{
				if (HttpContext.Current.Request.Url.AbsoluteUri.StartsWith("http://localhost/"))
				{
					return new FtbLicense(type, "LocalhostLicense", "none", true);
				}
				return new FtbLicense(type, "NoLicense", "Unlicensed", false);
			}
		}
		protected virtual FtbLicense CreateEmptyLicense(Type type)
		{
			return new FtbLicense(type, string.Empty, string.Empty);
		}
		public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
		{
			string text = null;
			FtbLicense ftbLicense;
			if (context.UsageMode == LicenseUsageMode.Designtime)
			{
				ftbLicense = this.CreateEmptyLicense(type);
			}
			else
			{
				ftbLicense = FtbLicenseProvider.LicenseCollector.GetLicense(type);
				if (ftbLicense == null)
				{
					string licenseData = this.GetLicenseData(type);
					if (licenseData != null && licenseData.Length != 0 && this.ValidateLicenseData(type, licenseData))
					{
						FtbLicense ftbLicense2 = this.CreateLicense(type, licenseData);
						if (this.ValidateLicense(ftbLicense2, out text))
						{
							ftbLicense = ftbLicense2;
							FtbLicenseProvider.LicenseCollector.AddLicense(type, ftbLicense);
						}
					}
				}
				else
				{
					if (!this.ValidateLicense(ftbLicense, out text))
					{
						ftbLicense = null;
					}
				}
			}
			if (ftbLicense == null)
			{
				ftbLicense = this.CreateLicense(type, "No data");
			}
			return ftbLicense;
		}
		protected virtual string GetLicenseData(Type type)
		{
			string result = null;
			Stream stream = null;
			try
			{
				stream = this.GetLicenseDataStream(type);
				if (stream != null)
				{
					StreamReader streamReader = new StreamReader(stream);
					result = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
					stream = null;
				}
			}
			return result;
		}
		protected virtual Stream GetLicenseDataStream(Type type)
		{
			string arg_10_0 = type.Assembly.GetName().Name;
			type.Assembly.GetName().Version.ToString();
			string path = "~/bin/" + type.Name + ".lic";
			string text = null;
			try
			{
				text = HttpContext.Current.Server.MapPath(path);
				if (!File.Exists(text))
				{
					path = "/bin/" + type.Name + ".lic";
					text = HttpContext.Current.Server.MapPath(path);
					if (!File.Exists(text))
					{
						text = null;
					}
				}
			}
			catch
			{
			}
			if (text != null)
			{
				try
				{
					Stream stream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read);
					Stream result;
					if (stream == null)
					{
						result = null;
						return result;
					}
					result = stream;
					return result;
				}
				catch
				{
					Stream result = null;
					return result;
				}
			}
			return null;
		}
		private string DecryptLicenseData(string input)
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			ICryptoTransform transform = dESCryptoServiceProvider.CreateDecryptor(FtbLicenseProvider.encryptionKeyBytes, FtbLicenseProvider.encryptionKeyBytes);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			byte[] array = new byte[input.Length];
			string result;
			try
			{
				array = Convert.FromBase64CharArray(input.ToCharArray(), 0, input.Length);
			}
			catch (Exception)
			{
				string text = "Error. Input Data is not base64 encoded.";
				result = text;
				return result;
			}
			long num = 0L;
			long num2 = (long)input.Length;
			try
			{
				while (num2 >= num)
				{
					cryptoStream.Write(array, 0, array.Length);
					num = memoryStream.Length + (long)((ulong)Convert.ToUInt32(array.Length / dESCryptoServiceProvider.BlockSize * dESCryptoServiceProvider.BlockSize));
				}
				ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
				string text = aSCIIEncoding.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
				string value = text.Substring(0, 5);
				int length = Convert.ToInt32(value);
				text = text.Substring(5, length);
				result = text;
			}
			catch (Exception)
			{
				string text = "";
				result = text;
			}
			return result;
		}
		protected virtual bool ValidateLicense(FtbLicense license, out string errorMessage)
		{
			errorMessage = null;
			return true;
		}
		protected virtual bool ValidateLicenseData(Type type, string licenseData)
		{
			Match match = Regex.Match(licenseData, type.Name + " License(.|\\n)*?\\[(?<type>[^\\]]+)\\](.|\\n)*?\\[(?<secondfield>[^\\]]+)\\]");
			return match.Success;
		}
	}
}

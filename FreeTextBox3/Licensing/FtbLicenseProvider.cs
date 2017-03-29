// ServerLicenseProvider.cs
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace FreeTextBoxControls.Licensing {
	/// <summary>
	/// Creates and processes FreeTextBox.lic
	/// </summary>
	/// <exclude />
	public class FtbLicenseProvider : LicenseProvider {
		
		#region Private properties
		// This is a 64-bit key generated from the string
		//
		private static readonly byte[] encryptionKeyBytes =
			new byte[] { 0x39, 0x48, 0x42, 0x32, 0x38, 0x31, 0x46, 0x36 };

		private static readonly FtbLicenseCollector LicenseCollector = new FtbLicenseCollector();
		#endregion

		#region License Creation
		//protected virtual FtbLicense CreateLicense(Type type, string key, bool isPro) {
		//	return new FtbLicense(type, key, isPro);
		//}

		protected virtual FtbLicense CreateLicense(Type type, string licenseData) {
			Match m = Regex.Match(licenseData, type.Name + " License" +
                @"(.|\n)*?" +
				@"\[(?<licenseType>[^\]]+)\]" +
                @"(.|\n)*?" +
				@"\[(?<secondField>[^\]]+)\]");
			
			if (m.Success) {
				
				string licenseType = DecryptLicenseData(m.Groups["licenseType"].Value);
				string secondField = DecryptLicenseData(m.Groups["secondField"].Value);

				// Decrypt!!

				if (licenseType == "SingleLicense" || licenseType == "DistributionLicense") {			
					
					return new FtbLicense(type, licenseType, secondField, true);			
				} else if (licenseType == "ExpiringLicense") {
					DateTime expDate = new DateTime();
					try { expDate = Convert.ToDateTime(secondField); } catch {}
                    if (expDate != new DateTime())
                    {

                        if (expDate > DateTime.Now)
                        {
                            return new FtbLicense(type, licenseType, secondField, true);
                        }
                        else
                        {
                            return new FtbLicense(type, "expired", licenseType + "," + secondField, false);
                        }
                    }
                    else
                    {
                        return new FtbLicense(type, "Invalid date", licenseType + "," + secondField, false);
                    }
				} else {
					return new FtbLicense(type, "BadLicenseData",licenseType + "," + secondField, false);
				}
			}
			
			// if here, then check for localhost			
			if (System.Web.HttpContext.Current.Request.Url.AbsoluteUri.StartsWith("http://localhost/")) {
				return new FtbLicense(type, "LocalhostLicense","none", true);
			} else {
				return new FtbLicense(type, "NoLicense", "Unlicensed", false);
			}
		}

		protected virtual FtbLicense CreateEmptyLicense(Type type) {
			return new FtbLicense(type, String.Empty, String.Empty);
		}
		#endregion


		// main method that we have to inherit
		public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions) {
			FtbLicense license = null;
			string errorMessage = null;
			
			// in VS.NET DesignMode, just create a blank license
			if (context.UsageMode == LicenseUsageMode.Designtime) {
				license = CreateEmptyLicense(type);
			} else {
				
				// attempt to get cached license
				license = LicenseCollector.GetLicense(type);

				if (license == null) {
					// get license from disk
					string licenseData = GetLicenseData(type);

					// if the license exists then set the properties
					if ((licenseData != null) && (licenseData.Length != 0)) {

						if (ValidateLicenseData(type, licenseData)) {
							FtbLicense newLicense = CreateLicense(type, licenseData);

							if (ValidateLicense(newLicense, out errorMessage)) {
								license = newLicense;
								LicenseCollector.AddLicense(type, license);
							}
						}	
					}
				} else {
					// USING license from disk

					if (ValidateLicense(license, out errorMessage) == false) {
						license = null;
					}
				}
			}

            if (license == null)
            {
                // create a license with no data

                license = CreateLicense(type, "No data");
            }
			

			return license;
		}

		protected virtual string GetLicenseData(Type type) {
			string licenseData = null;
			Stream licenseStream = null;

			try {
				licenseStream = GetLicenseDataStream(type);

				if (licenseStream != null) {
					StreamReader sr = new StreamReader(licenseStream);

					licenseData = sr.ReadToEnd();

					sr.Close();
				} else {
					// got no data
				}
			} finally {
				if (licenseStream != null) {
					licenseStream.Close();
					licenseStream = null;
				}
			}

			return licenseData;
		}

		protected virtual Stream GetLicenseDataStream(Type type) {
			string assemblyPart = type.Assembly.GetName().Name;
			string versionPart = type.Assembly.GetName().Version.ToString();
			// string relativePath = "~/licenses/" + assemblyPart + "/" + versionPart + "/" + type.FullName + ".lic";
			
			string relativePath = "~/bin/" + type.Name + ".lic";
			string licensesFile = null;
			try {
				licensesFile = HttpContext.Current.Server.MapPath(relativePath);
				if (!File.Exists(licensesFile)) {
					relativePath = "/bin/" + type.Name + ".lic";
					licensesFile = HttpContext.Current.Server.MapPath(relativePath);
					if (!File.Exists(licensesFile)) {
						licensesFile = null;
					}
				}
			} catch {}

			if (licensesFile != null) {
				try {
					Stream baseStream = new FileStream(licensesFile, FileMode.Open, FileAccess.Read, FileShare.Read);

					if (baseStream == null) {
						return null;
					}
					
					return baseStream;
				} catch {
					return null;
				}
				
		
			} else {
				return null;
			}
		}
		
		private string DecryptLicenseData(string input) {

			string result;

			
			//int return = 0;
			// setup decryptor
			DESCryptoServiceProvider decryptionProvider = new DESCryptoServiceProvider(); 
			ICryptoTransform decryptor = decryptionProvider.CreateDecryptor(encryptionKeyBytes, encryptionKeyBytes); 
		
			// setup streams:
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write);        
		
			// back to base64 byte array
			byte[] plain = new byte[input.Length];
			try {
				plain = Convert.FromBase64CharArray(input.ToCharArray(), 0, input.Length);
			}
			catch (Exception) { 
				result = "Error. Input Data is not base64 encoded.";
				return result;
			}
		
			long read = 0;
			long total = input.Length;
		
			try {
				//5. Perform the actual decryption
				while (total >= read) { 
					cryptoStream.Write(plain,0,(int)plain.Length); 
					//descsp.BlockSize=64
					read = memoryStream.Length + Convert.ToUInt32(((plain.Length / decryptionProvider.BlockSize) * decryptionProvider.BlockSize));
				};
			
				ASCIIEncoding aEnc = new ASCIIEncoding();
				result = aEnc.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			
				//6. Trim the string to return only the meaningful data
				//	Remember that in the encrypt function, the first 5 character holds the length of the actual data
				//	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
				String strLen = result.Substring(0,5);
				int nLen = Convert.ToInt32(strLen);
				result = result.Substring(5, nLen);
				//nReturn = (int)mOut.Length;
			
				return result;
			}
			catch (Exception) {
				result = "";
				return result;
			}			
		}

		
		/// <summary>
		/// Sets up the lisence
		/// </summary>
		/// <param name="license"></param>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		protected virtual bool ValidateLicense(FtbLicense license, out string errorMessage) {				
			errorMessage = null;
			return true;
		}
			
		/// <summary>
		/// Checks if the license data is in a valid form
		/// </summary>
		protected virtual bool ValidateLicenseData(Type type, string licenseData) {
			/* Should look like:			
			-- START --
			FreeTextBox License	
			[license type]
			[data field]
			-- END ----
			*/

			Match m = Regex.Match(licenseData, type.Name + " License" +
				@"(.|\n)*?" +
				@"\[(?<type>[^\]]+)\]" +
                @"(.|\n)*?" +
				@"\[(?<secondfield>[^\]]+)\]");
			
			return m.Success;


		}
	
		#region License Collector
		/// <summary>
		/// Stores licenses so they are not read from disk every time
		/// </summary>
		/// <exclude />
		private sealed class FtbLicenseCollector {

			private IDictionary _collectedLicenses;

			public FtbLicenseCollector() {
				_collectedLicenses = new HybridDictionary();
			}

			public void AddLicense(Type objectType, FtbLicense license) {
				if (objectType == null) {
					throw new ArgumentNullException("objectType");
				}
				if (license == null) {
					throw new ArgumentNullException("objectType");
				}

				_collectedLicenses[objectType] = license;
			}

			public FtbLicense GetLicense(Type objectType) {
				if (objectType == null) {
					throw new ArgumentNullException("objectType");
				}

				if (_collectedLicenses.Count == 0) {
					return null;
				}

				return (FtbLicense)_collectedLicenses[objectType];
			}

			public void RemoveLicense(Type objectType) {
				if (objectType == null) {
					throw new ArgumentNullException("objectType");
				}

				_collectedLicenses.Remove(objectType);
			}
		}
		#endregion 
	}
}

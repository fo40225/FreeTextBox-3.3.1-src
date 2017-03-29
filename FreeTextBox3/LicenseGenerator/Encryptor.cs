using System;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;


namespace LicenseGenerator {


	public class Encryptor {
		#region Private properties
		// This is a 64-bit key generated from the string
		//
		private static readonly byte[] encryptionKeyBytes =
			new byte[] { 0x39, 0x48, 0x42, 0x32, 0x38, 0x31, 0x46, 0x36 };

		#endregion	

		public string EncryptData(string input) {
			string result = null;

			//  3. Prepare the String
			//	The first 5 character of the string is formatted to store the actual length of the data.
			//	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
			//	If anyone figure a good way to 'remember' the original length to facilite the decryption without having to use additional function parameters, pls let me know.
			input = String.Format("{0,5:00000}"+input, input.Length);

			//4. Encrypt the Data
			byte[] rbData = new byte[input.Length];
			ASCIIEncoding aEnc = new ASCIIEncoding();
			aEnc.GetBytes(input, 0, input.Length, rbData, 0);
		
			DESCryptoServiceProvider descsp = new DESCryptoServiceProvider(); 
		
			ICryptoTransform desEncrypt = descsp.CreateEncryptor(encryptionKeyBytes, encryptionKeyBytes); 


			//5. Perpare the streams:
			//	mOut is the output stream. 
			//	mStream is the input stream.
			//	cs is the transformation stream.
			MemoryStream mStream = new MemoryStream(rbData); 
			CryptoStream cs = new CryptoStream(mStream, desEncrypt, CryptoStreamMode.Read);        
			MemoryStream mOut = new MemoryStream();
		
			//6. Start performing the encryption
			int bytesRead; 
			byte[] output = new byte[1024];
			do { 
				bytesRead = cs.Read(output,0,1024);
				if (bytesRead != 0) 
					mOut.Write(output,0,bytesRead); 
			} while (bytesRead > 0); 
		
			//7. Returns the encrypted result after it is base64 encoded
			//	In this case, the actual result is converted to base64 so that it can be transported over the HTTP protocol without deformation.
			if (mOut.Length == 0)		
				result = "";
			else
				result = Convert.ToBase64String(mOut.GetBuffer(), 0, (int)mOut.Length);
	
			return result;
		}
		public string DecryptData(string input) {

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
	}
}

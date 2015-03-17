using System;
namespace FreeTextBoxControls.Support.Sgml
{
	internal class Ucs4DecoderBigEngian : Ucs4Decoder
	{
		internal override int GetFullChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			byteCount += byteIndex;
			int num = byteIndex;
			int num2 = charIndex;
			while (num + 3 < byteCount)
			{
				uint num3 = (uint)((int)bytes[num + 3] << 24 | (int)bytes[num + 2] << 16 | (int)bytes[num + 1] << 8 | (int)bytes[num]);
				if (num3 > 1114111u)
				{
					throw new Exception("Invalid character 0x" + num3.ToString("x") + " in encoding");
				}
				if (num3 > 65535u)
				{
					chars[num2] = base.UnicodeToUTF16(num3);
					num2++;
				}
				else
				{
					if (num3 >= 55296u && num3 <= 57343u)
					{
						throw new Exception("Invalid character 0x" + num3.ToString("x") + " in encoding");
					}
					chars[num2] = (char)num3;
				}
				num2++;
				num += 4;
			}
			return num2 - charIndex;
		}
	}
}

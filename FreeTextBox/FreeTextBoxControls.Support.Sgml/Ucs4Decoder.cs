using System;
using System.Text;
namespace FreeTextBoxControls.Support.Sgml
{
	internal abstract class Ucs4Decoder : Decoder
	{
		internal byte[] temp = new byte[4];
		internal int tempBytes;
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return (count + this.tempBytes) / 4;
		}
		internal abstract int GetFullChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			int i = this.tempBytes;
			if (this.tempBytes > 0)
			{
				while (i < 4)
				{
					this.temp[i] = bytes[byteIndex];
					byteIndex++;
					byteCount--;
					i++;
				}
				i = 1;
				this.GetFullChars(this.temp, 0, 4, chars, charIndex);
				charIndex++;
			}
			else
			{
				i = 0;
			}
			i = this.GetFullChars(bytes, byteIndex, byteCount, chars, charIndex) + i;
			int num = (this.tempBytes + byteCount) % 4;
			byteCount += byteIndex;
			byteIndex = byteCount - num;
			this.tempBytes = 0;
			if (byteIndex >= 0)
			{
				while (byteIndex < byteCount)
				{
					this.temp[this.tempBytes] = bytes[byteIndex];
					this.tempBytes++;
					byteIndex++;
				}
			}
			return i;
		}
		internal char UnicodeToUTF16(uint code)
		{
			byte b = (byte)(55232u + (code >> 10));
			byte b2 = (byte)(56320u | (code & 1023u));
			return (char)((int)b2 << 8 | (int)b);
		}
	}
}

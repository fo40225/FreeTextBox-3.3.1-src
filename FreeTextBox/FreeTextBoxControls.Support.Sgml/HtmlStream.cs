using System;
using System.IO;
using System.Text;
namespace FreeTextBoxControls.Support.Sgml
{
	internal class HtmlStream : TextReader
	{
		private const int BUFSIZE = 16384;
		private const int EOF = -1;
		private Stream stm;
		private byte[] rawBuffer;
		private int rawPos;
		private int rawUsed;
		private Encoding encoding;
		private Decoder decoder;
		private char[] buffer;
		private int used;
		private int pos;
		public Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
		}
		public HtmlStream(Stream stm, Encoding defaultEncoding)
		{
			if (defaultEncoding == null)
			{
				defaultEncoding = Encoding.UTF8;
			}
			if (!stm.CanSeek)
			{
				stm = this.CopyToMemoryStream(stm);
			}
			this.stm = stm;
			this.rawBuffer = new byte[16384];
			this.rawUsed = stm.Read(this.rawBuffer, 0, 4);
			this.buffer = new char[16384];
			this.decoder = HtmlStream.AutoDetectEncoding(this.rawBuffer, ref this.rawPos, this.rawUsed);
			int num = this.rawPos;
			if (this.decoder == null)
			{
				this.decoder = defaultEncoding.GetDecoder();
				this.rawUsed += stm.Read(this.rawBuffer, 4, 16380);
				this.DecodeBlock();
				Decoder decoder = this.SniffEncoding();
				if (decoder != null)
				{
					this.decoder = decoder;
				}
			}
			this.stm.Seek(0L, SeekOrigin.Begin);
			this.pos = (this.used = 0);
			if (num > 0)
			{
				stm.Read(this.rawBuffer, 0, num);
			}
			this.rawPos = (this.rawUsed = 0);
		}
		private Stream CopyToMemoryStream(Stream s)
		{
			int num = 100000;
			byte[] array = new byte[num];
			MemoryStream memoryStream = new MemoryStream();
			int count;
			while ((count = s.Read(array, 0, num)) > 0)
			{
				memoryStream.Write(array, 0, count);
			}
			memoryStream.Seek(0L, SeekOrigin.Begin);
			s.Close();
			return memoryStream;
		}
		internal void DecodeBlock()
		{
			if (this.pos > 0)
			{
				if (this.pos < this.used)
				{
					Array.Copy(this.buffer, this.pos, this.buffer, 0, this.used - this.pos);
				}
				this.used -= this.pos;
				this.pos = 0;
			}
			int charCount = this.decoder.GetCharCount(this.rawBuffer, this.rawPos, this.rawUsed - this.rawPos);
			int num = this.buffer.Length - this.used;
			if (num < charCount)
			{
				char[] destinationArray = new char[this.buffer.Length + charCount];
				Array.Copy(this.buffer, this.pos, destinationArray, 0, this.used - this.pos);
				this.buffer = destinationArray;
			}
			this.used = this.pos + this.decoder.GetChars(this.rawBuffer, this.rawPos, this.rawUsed - this.rawPos, this.buffer, this.pos);
			this.rawPos = this.rawUsed;
		}
		internal static Decoder AutoDetectEncoding(byte[] buffer, ref int index, int length)
		{
			if (4 <= length - index)
			{
				uint num = (uint)((int)buffer[index] << 24 | (int)buffer[index + 1] << 16 | (int)buffer[index + 2] << 8 | (int)buffer[index + 3]);
				uint num2 = num;
				if (num2 <= 1006632960u)
				{
					if (num2 == 60u)
					{
						goto IL_63;
					}
					if (num2 != 1006632960u)
					{
						goto IL_6F;
					}
				}
				else
				{
					if (num2 != 4278189823u)
					{
						if (num2 != 4294901758u)
						{
							goto IL_6F;
						}
						goto IL_63;
					}
				}
				index += 4;
				return new Ucs4DecoderBigEngian();
				IL_63:
				index += 4;
				return new Ucs4DecoderLittleEndian();
				IL_6F:
				num >>= 8;
				if (num == 15711167u)
				{
					index += 3;
					return Encoding.UTF8.GetDecoder();
				}
				num >>= 8;
				uint num3 = num;
				if (num3 <= 15360u)
				{
					if (num3 == 60u)
					{
						goto IL_CC;
					}
					if (num3 != 15360u)
					{
						goto IL_DF;
					}
				}
				else
				{
					if (num3 != 65279u)
					{
						if (num3 != 65534u)
						{
							goto IL_DF;
						}
						goto IL_CC;
					}
				}
				index += 2;
				return Encoding.BigEndianUnicode.GetDecoder();
				IL_CC:
				index += 2;
				return new UnicodeEncoding(false, false).GetDecoder();
			}
			IL_DF:
			return null;
		}
		private int ReadChar()
		{
			if (this.pos < this.used)
			{
				return (int)this.buffer[this.pos++];
			}
			return -1;
		}
		private int PeekChar()
		{
			int num = this.ReadChar();
			if (num != -1)
			{
				this.pos--;
			}
			return num;
		}
		private bool SniffPattern(string pattern)
		{
			int num = this.PeekChar();
			if (num != (int)pattern[0])
			{
				return false;
			}
			int num2 = 0;
			int length = pattern.Length;
			while (num != -1 && num2 < length)
			{
				num = this.ReadChar();
				char c = pattern[num2];
				if (num != (int)c)
				{
					return false;
				}
				num2++;
			}
			return true;
		}
		private void SniffWhitespace()
		{
			char c = (char)this.PeekChar();
			while (c == ' ' || c == '\r' || c == '\r' || c == '\n')
			{
				int num = this.pos;
				c = (char)this.ReadChar();
				if (c != ' ' && c != '\r' && c != '\r' && c != '\n')
				{
					this.pos = num;
				}
			}
		}
		private string SniffLiteral()
		{
			int num = this.PeekChar();
			if (num != 39 && num != 34)
			{
				return null;
			}
			this.ReadChar();
			int num2 = this.pos;
			int num3 = this.ReadChar();
			while (num3 != -1 && num3 != num)
			{
				num3 = this.ReadChar();
			}
			if (this.pos <= num2)
			{
				return "";
			}
			return new string(this.buffer, num2, this.pos - num2 - 1);
		}
		private string SniffAttribute(string name)
		{
			this.SniffWhitespace();
			string b = this.SniffName();
			if (name == b)
			{
				this.SniffWhitespace();
				if (this.SniffPattern("="))
				{
					this.SniffWhitespace();
					return this.SniffLiteral();
				}
			}
			return null;
		}
		private string SniffAttribute(out string name)
		{
			this.SniffWhitespace();
			name = this.SniffName();
			if (name != null)
			{
				this.SniffWhitespace();
				if (this.SniffPattern("="))
				{
					this.SniffWhitespace();
					return this.SniffLiteral();
				}
			}
			return null;
		}
		private void SniffTerminator(string term)
		{
			int num = this.ReadChar();
			int num2 = 0;
			int length = term.Length;
			while (num2 < length && num != -1)
			{
				if ((int)term[num2] == num)
				{
					num2++;
					if (num2 == length)
					{
						return;
					}
				}
				else
				{
					num2 = 0;
				}
				num = this.ReadChar();
			}
		}
		internal Decoder SniffEncoding()
		{
			Decoder decoder = null;
			if (this.SniffPattern("<?xml"))
			{
				string text = this.SniffAttribute("version");
				if (text != null)
				{
					string text2 = this.SniffAttribute("encoding");
					if (text2 != null)
					{
						try
						{
							Encoding encoding = Encoding.GetEncoding(text2);
							if (encoding != null)
							{
								this.encoding = encoding;
								return encoding.GetDecoder();
							}
						}
						catch (Exception)
						{
						}
					}
					this.SniffTerminator(">");
				}
			}
			if (decoder == null)
			{
				return this.SniffMeta();
			}
			return null;
		}
		internal Decoder SniffMeta()
		{
			for (int num = this.ReadChar(); num != -1; num = this.ReadChar())
			{
				char c = (char)num;
				if (c == '<')
				{
					string text = this.SniffName();
					if (text != null && text.ToLower() == "meta")
					{
						string text2 = null;
						string text3 = null;
						while (true)
						{
							string text4 = this.SniffAttribute(out text);
							if (text == null)
							{
								break;
							}
							text = text.ToLower();
							if (text == "http-equiv")
							{
								text2 = text4;
							}
							else
							{
								if (text == "content")
								{
									text3 = text4;
								}
							}
						}
						if (text2 != null && text2.ToLower() == "content-type" && text3 != null)
						{
							int num2 = text3.IndexOf("charset");
							if (num2 >= 0)
							{
								num2 = text3.IndexOf("=", num2);
								if (num2 >= 0)
								{
									num2++;
									int num3 = text3.IndexOf(";", num2);
									if (num3 < 0)
									{
										num3 = text3.Length;
									}
									string name = text3.Substring(num2, num3 - num2).Trim();
									try
									{
										Encoding encoding = Encoding.GetEncoding(name);
										this.encoding = encoding;
										return encoding.GetDecoder();
									}
									catch
									{
									}
								}
							}
						}
					}
				}
			}
			return null;
		}
		internal string SniffName()
		{
			if (this.pos == this.used)
			{
				return null;
			}
			char c = this.buffer[this.pos];
			int num = this.pos;
			while (this.pos < this.used && (char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == ':'))
			{
				c = this.buffer[++this.pos];
			}
			if (num == this.pos)
			{
				return null;
			}
			return new string(this.buffer, num, this.pos - num);
		}
		internal void SkipWhitespace()
		{
			char c = this.buffer[this.pos];
			while (this.pos < this.used && (c == ' ' || c == '\r' || c == '\n'))
			{
				c = this.buffer[++this.pos];
			}
		}
		internal void SkipTo(char what)
		{
			char c = this.buffer[this.pos];
			while (this.pos < this.used && c != what)
			{
				c = this.buffer[++this.pos];
			}
		}
		internal string ParseAttribute()
		{
			this.SkipTo('=');
			if (this.pos < this.used)
			{
				this.pos++;
				this.SkipWhitespace();
				if (this.pos < this.used)
				{
					char what = this.buffer[this.pos];
					this.pos++;
					int num = this.pos;
					this.SkipTo(what);
					if (this.pos < this.used)
					{
						string result = new string(this.buffer, num, this.pos - num);
						this.pos++;
						return result;
					}
				}
			}
			return null;
		}
		public override int Peek()
		{
			int num = this.Read();
			if (num != -1)
			{
				this.pos--;
			}
			return num;
		}
		public override int Read()
		{
			if (this.pos == this.used)
			{
				this.rawUsed = this.stm.Read(this.rawBuffer, 0, this.rawBuffer.Length);
				this.rawPos = 0;
				if (this.rawUsed == 0)
				{
					return -1;
				}
				this.DecodeBlock();
			}
			if (this.pos < this.used)
			{
				return (int)this.buffer[this.pos++];
			}
			return -1;
		}
		public override int Read(char[] buffer, int start, int length)
		{
			if (this.pos == this.used)
			{
				this.rawUsed = this.stm.Read(this.rawBuffer, 0, this.rawBuffer.Length);
				this.rawPos = 0;
				if (this.rawUsed == 0)
				{
					return -1;
				}
				this.DecodeBlock();
			}
			if (this.pos < this.used)
			{
				length = Math.Min(this.used - this.pos, length);
				Array.Copy(this.buffer, this.pos, buffer, start, length);
				this.pos += length;
				return length;
			}
			return 0;
		}
		public override int ReadBlock(char[] buffer, int index, int count)
		{
			return this.Read(buffer, index, count);
		}
		public int ReadLine(char[] buffer, int start, int length)
		{
			int num = 0;
			int num2 = this.ReadChar();
			while (num2 != -1)
			{
				buffer[num + start] = (char)num2;
				num++;
				if (num + start == length)
				{
					break;
				}
				if (num2 == 13)
				{
					if (this.PeekChar() == 10)
					{
						num2 = this.ReadChar();
						buffer[num + start] = (char)num2;
						num++;
						break;
					}
					break;
				}
				else
				{
					if (num2 == 10)
					{
						break;
					}
					num2 = this.ReadChar();
				}
			}
			return num;
		}
		public override string ReadToEnd()
		{
			char[] array = new char[100000];
			StringBuilder stringBuilder = new StringBuilder();
			int charCount;
			while ((charCount = this.Read(array, 0, array.Length)) > 0)
			{
				stringBuilder.Append(array, 0, charCount);
			}
			return stringBuilder.ToString();
		}
		public override void Close()
		{
			this.stm.Close();
		}
	}
}

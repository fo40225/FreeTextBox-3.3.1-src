using System;
using System.IO;
using System.Net;
using System.Text;
namespace FreeTextBoxControls.Support.Sgml
{
	public class Entity
	{
		public const char EOF = '￿';
		public string Proxy;
		public string Name;
		public bool Internal;
		public string PublicId;
		public string Uri;
		public string Literal;
		public LiteralType LiteralType;
		public Entity Parent;
		public bool Html;
		public int Line;
		public char Lastchar;
		public bool IsWhitespace;
		private Encoding encoding;
		private Uri resolvedUri;
		private TextReader stm;
		private bool weOwnTheStream;
		private int lineStart;
		private int absolutePos;
		private static int[] CtrlMap = new int[]
		{
			8364,
			129,
			8218,
			402,
			8222,
			8230,
			8224,
			8225,
			710,
			8240,
			352,
			8249,
			338,
			141,
			381,
			143,
			144,
			8216,
			8217,
			8220,
			8221,
			8226,
			8211,
			8212,
			732,
			8482,
			353,
			8250,
			339,
			157,
			382,
			376
		};
		public Uri ResolvedUri
		{
			get
			{
				if (this.resolvedUri != null)
				{
					return this.resolvedUri;
				}
				if (this.Parent != null)
				{
					return this.Parent.ResolvedUri;
				}
				return null;
			}
		}
		public int LinePosition
		{
			get
			{
				return this.absolutePos - this.lineStart + 1;
			}
		}
		public Entity(string name, string pubid, string uri, string proxy)
		{
			this.Name = name;
			this.PublicId = pubid;
			this.Uri = uri;
			this.Proxy = proxy;
			this.Html = (name != null && name.ToLower() == "html");
		}
		public Entity(string name, string literal)
		{
			this.Name = name;
			this.Literal = literal;
			this.Internal = true;
		}
		public Entity(string name, Uri baseUri, TextReader stm, string proxy)
		{
			this.Name = name;
			this.Internal = true;
			this.stm = stm;
			this.resolvedUri = baseUri;
			this.Proxy = proxy;
			this.Html = (name.ToLower() == "html");
		}
		public char ReadChar()
		{
			char c = (char)this.stm.Read();
			if (c == '\0')
			{
				c = ' ';
			}
			this.absolutePos++;
			if (c == '\n')
			{
				this.IsWhitespace = true;
				this.lineStart = this.absolutePos + 1;
				this.Line++;
			}
			else
			{
				if (c == ' ' || c == '\t')
				{
					this.IsWhitespace = true;
					if (this.Lastchar == '\r')
					{
						this.lineStart = this.absolutePos;
						this.Line++;
					}
				}
				else
				{
					if (c == '\r')
					{
						this.IsWhitespace = true;
					}
					else
					{
						this.IsWhitespace = false;
						if (this.Lastchar == '\r')
						{
							this.Line++;
							this.lineStart = this.absolutePos;
						}
					}
				}
			}
			this.Lastchar = c;
			return c;
		}
		public void Open(Entity parent, Uri baseUri)
		{
			this.Parent = parent;
			if (parent != null)
			{
				this.Html = parent.Html;
			}
			this.Line = 1;
			if (this.Internal)
			{
				if (this.Literal != null)
				{
					this.stm = new StringReader(this.Literal);
					return;
				}
			}
			else
			{
				if (this.Uri == null)
				{
					this.Error("Unresolvable entity '{0}'", this.Name);
					return;
				}
				if (baseUri != null)
				{
					this.resolvedUri = new Uri(baseUri, this.Uri);
				}
				else
				{
					this.resolvedUri = new Uri(this.Uri);
				}
				Encoding @default = Encoding.Default;
				string scheme;
				Stream stream;
				if ((scheme = this.resolvedUri.Scheme) != null && scheme == "file")
				{
					string localPath = this.resolvedUri.LocalPath;
					stream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
				}
				else
				{
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this.ResolvedUri);
					httpWebRequest.UserAgent = "Mozilla/4.0 (compatible;);";
					httpWebRequest.Timeout = 10000;
					if (this.Proxy != null)
					{
						httpWebRequest.Proxy = new WebProxy(this.Proxy);
					}
					httpWebRequest.PreAuthenticate = false;
					httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
					WebResponse response = httpWebRequest.GetResponse();
					Uri responseUri = response.ResponseUri;
					if (responseUri.AbsoluteUri != this.resolvedUri.AbsoluteUri)
					{
						this.resolvedUri = responseUri;
					}
					string text = response.ContentType.ToLower();
					int num = text.IndexOf("charset");
					@default = Encoding.Default;
					if (num >= 0)
					{
						int num2 = text.IndexOf("=", num);
						int num3 = text.IndexOf(";", num2);
						if (num3 < 0)
						{
							num3 = text.Length;
						}
						if (num2 > 0)
						{
							num2++;
							string name = text.Substring(num2, num3 - num2).Trim();
							try
							{
								@default = Encoding.GetEncoding(name);
							}
							catch (Exception)
							{
							}
						}
					}
					stream = response.GetResponseStream();
				}
				this.weOwnTheStream = true;
				HtmlStream htmlStream = new HtmlStream(stream, @default);
				this.encoding = htmlStream.Encoding;
				this.stm = htmlStream;
			}
		}
		public Encoding GetEncoding()
		{
			return this.encoding;
		}
		public void Close()
		{
			if (this.weOwnTheStream)
			{
				this.stm.Close();
			}
		}
		public char SkipWhitespace()
		{
			char c = this.Lastchar;
			while (c != '￿' && (c == ' ' || c == '\r' || c == '\n' || c == '\t'))
			{
				c = this.ReadChar();
			}
			return c;
		}
		public string ScanToken(StringBuilder sb, string term, bool nmtoken)
		{
			sb.Length = 0;
			char c = this.Lastchar;
			if (nmtoken && c != '_' && !char.IsLetter(c))
			{
				throw new Exception(string.Format("Invalid name start character '{0}'", c));
			}
			while (c != '￿' && term.IndexOf(c) < 0)
			{
				if (nmtoken && c != '_' && c != '.' && c != '-' && c != ':' && !char.IsLetterOrDigit(c))
				{
					throw new Exception(string.Format("Invalid name character '{0}'", c));
				}
				sb.Append(c);
				c = this.ReadChar();
			}
			return sb.ToString();
		}
		public string ScanLiteral(StringBuilder sb, char quote)
		{
			sb.Length = 0;
			char c = this.ReadChar();
			while (c != '￿' && c != quote)
			{
				if (c == '&')
				{
					c = this.ReadChar();
					if (c == '#')
					{
						string value = this.ExpandCharEntity();
						sb.Append(value);
					}
					else
					{
						sb.Append('&');
						sb.Append(c);
					}
				}
				else
				{
					sb.Append(c);
				}
				c = this.ReadChar();
			}
			this.ReadChar();
			return sb.ToString();
		}
		public string ScanToEnd(StringBuilder sb, string type, string terminators)
		{
			if (sb != null)
			{
				sb.Length = 0;
			}
			int line = this.Line;
			char c = this.ReadChar();
			int num = 0;
			char c2 = terminators[num];
			while (c != '￿')
			{
				if (c == c2)
				{
					num++;
					if (num >= terminators.Length)
					{
						break;
					}
					c2 = terminators[num];
				}
				else
				{
					if (num > 0)
					{
						int num2 = num - 1;
						int num3 = 0;
						while (num2 >= 0 && num3 == 0)
						{
							if (terminators[num2] == c)
							{
								int num4 = 1;
								while (num2 - num4 >= 0 && terminators[num2 - num4] == terminators[num - num4])
								{
									num4++;
								}
								if (num4 > num2)
								{
									num3 = num2 + 1;
								}
							}
							else
							{
								num2--;
							}
						}
						if (sb != null)
						{
							num2 = ((num2 < 0) ? 1 : 0);
							for (int i = 0; i <= num - num3 - num2; i++)
							{
								sb.Append(terminators[i]);
							}
							if (num2 > 0)
							{
								sb.Append(c);
							}
						}
						num = num3;
						c2 = terminators[num3];
					}
					else
					{
						if (sb != null)
						{
							sb.Append(c);
						}
					}
				}
				c = this.ReadChar();
			}
			if (c == '\0')
			{
				this.Error(type + " starting on line {0} was never closed", line);
			}
			this.ReadChar();
			if (sb != null)
			{
				return sb.ToString();
			}
			return "";
		}
		public string ExpandCharEntity()
		{
			char c = this.ReadChar();
			int num = 0;
			if (c == 'x')
			{
				while (c != '￿')
				{
					if (c == ';')
					{
						break;
					}
					int num2;
					if (c >= '0' && c <= '9')
					{
						num2 = (int)(c - '0');
					}
					else
					{
						if (c >= 'a' && c <= 'f')
						{
							num2 = (int)(c - 'a' + '\n');
						}
						else
						{
							if (c < 'A' || c > 'F')
							{
								break;
							}
							num2 = (int)(c - 'A' + '\n');
						}
					}
					num = num * 16 + num2;
					c = this.ReadChar();
				}
			}
			else
			{
				while (c != '￿' && c != ';' && c >= '0' && c <= '9')
				{
					num = num * 10 + (int)(c - '0');
					c = this.ReadChar();
				}
			}
			if (c == '\0')
			{
				this.Error("Premature {0} parsing entity reference", c);
			}
			if (this.Html && (num >= 128 & num <= 159))
			{
				int[] arg_C3_0 = Entity.CtrlMap;
				int num3 = num - 128;
				int value = Entity.CtrlMap[num3];
				return Convert.ToChar(value).ToString();
			}
			return Convert.ToChar(num).ToString();
		}
		public void Error(string msg)
		{
			throw new Exception(msg);
		}
		public void Error(string msg, char ch)
		{
			string arg = (ch == '￿') ? "EOF" : char.ToString(ch);
			throw new Exception(string.Format(msg, arg));
		}
		public void Error(string msg, int x)
		{
			throw new Exception(string.Format(msg, x));
		}
		public void Error(string msg, string arg)
		{
			throw new Exception(string.Format(msg, arg));
		}
		public string Context()
		{
			Entity entity = this;
			StringBuilder stringBuilder = new StringBuilder();
			while (entity != null)
			{
				string value;
				if (entity.Internal)
				{
					value = string.Format("\nReferenced on line {0}, position {1} of internal entity '{2}'", entity.Line, entity.LinePosition, entity.Name);
				}
				else
				{
					value = string.Format("\nReferenced on line {0}, position {1} of '{2}' entity at [{3}]", new object[]
					{
						entity.Line,
						entity.LinePosition,
						entity.Name,
						entity.ResolvedUri.AbsolutePath
					});
				}
				stringBuilder.Append(value);
				entity = entity.Parent;
			}
			return stringBuilder.ToString();
		}
		public static bool IsLiteralType(string token)
		{
			return token == "CDATA" || token == "SDATA" || token == "PI";
		}
		public void SetLiteralType(string token)
		{
			if (token != null)
			{
				if (token == "CDATA")
				{
					this.LiteralType = LiteralType.CDATA;
					return;
				}
				if (token == "SDATA")
				{
					this.LiteralType = LiteralType.SDATA;
					return;
				}
				if (!(token == "PI"))
				{
					return;
				}
				this.LiteralType = LiteralType.PI;
			}
		}
	}
}

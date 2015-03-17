using System;
using System.Web;
namespace FreeTextBoxControls.Support
{
	public class BrowserInfo
	{
		private bool isRichCapable;
		private bool isMac;
		private bool isWindows;
		private bool isLinux;
		private bool isPalm;
		private bool isOpera;
		private bool isIE;
		private bool isGecko;
		private bool isWebkit;
		private bool isiOS;
		public bool IsRichCapable
		{
			get
			{
				return this.isRichCapable;
			}
		}
		public bool IsPalm
		{
			get
			{
				return this.isPalm;
			}
		}
		public bool IsMac
		{
			get
			{
				return this.isMac;
			}
		}
		public bool IsWindows
		{
			get
			{
				return this.isWindows;
			}
		}
		public bool IsLinux
		{
			get
			{
				return this.isLinux;
			}
		}
		public bool IsOpera
		{
			get
			{
				return this.isOpera;
			}
		}
		public bool IsIE
		{
			get
			{
				return this.isIE;
			}
		}
		public bool IsGecko
		{
			get
			{
				return this.isGecko;
			}
		}
		public bool IsWebkit
		{
			get
			{
				return this.isWebkit;
			}
		}
		public bool IsiOS
		{
			get
			{
				return this.isiOS;
			}
		}
		public static BrowserInfo GetBrowserInfo()
		{
			return BrowserInfo.GetBrowserInfo(HttpContext.Current);
		}
		public static BrowserInfo GetBrowserInfo(HttpContext context)
		{
			BrowserInfo browserInfo = new BrowserInfo();
			HttpBrowserCapabilities arg_11_0 = context.Request.Browser;
			string text = (context.Request.UserAgent ?? "").ToLower();
			browserInfo.isPalm = (text.IndexOf("palm") > -1);
			browserInfo.isMac = (text.IndexOf("mac") > -1);
			browserInfo.isLinux = (text.IndexOf("linux") > -1);
			browserInfo.isWindows = (text.IndexOf("win") > -1);
			browserInfo.isOpera = (text.IndexOf("opera") > -1);
			browserInfo.isIE = (text.IndexOf("msie") > -1);
			browserInfo.isWebkit = (text.IndexOf("webkit") > -1);
			browserInfo.isGecko = (text.IndexOf("gecko") > -1 && !browserInfo.isWebkit);
			browserInfo.isiOS = (text.IndexOf("iphone") > -1 || text.IndexOf("ipad") > -1);
			if (browserInfo.IsIE && !browserInfo.isMac)
			{
				if (context.Request.Browser.MajorVersion >= 6)
				{
					browserInfo.isRichCapable = true;
				}
			}
			else
			{
				if (browserInfo.IsGecko)
				{
					try
					{
						string text2 = text.Substring(text.IndexOf("gecko/") + 6, 8);
						DateTime t = new DateTime(Convert.ToInt32(text2.Substring(0, 4)), Convert.ToInt32(text2.Substring(4, 2)), Convert.ToInt32(text2.Substring(6, 2)));
						if (t >= new DateTime(2003, 6, 24))
						{
							browserInfo.isRichCapable = true;
						}
						return browserInfo;
					}
					catch
					{
						return browserInfo;
					}
				}
				if (browserInfo.IsOpera)
				{
					string text3 = "opera/";
					int num = text.IndexOf(text3) + text3.Length;
					string value = text.Substring(num, text.IndexOf("(", num) - num);
					try
					{
						double num2 = Convert.ToDouble(value);
						if (num2 > 9.0)
						{
							browserInfo.isRichCapable = true;
						}
						return browserInfo;
					}
					catch
					{
						return browserInfo;
					}
				}
				if (browserInfo.isWebkit)
				{
					string text4 = "webkit/";
					int num3 = text.IndexOf(text4) + text4.Length;
					string value2 = text.Substring(num3, text.IndexOf(".", num3) - num3);
					try
					{
						double num4 = Convert.ToDouble(value2);
						if (num4 > 412.0)
						{
							browserInfo.isRichCapable = true;
						}
					}
					catch
					{
					}
					if (browserInfo.IsiOS)
					{
						browserInfo.isRichCapable = false;
					}
				}
			}
			return browserInfo;
		}
	}
}

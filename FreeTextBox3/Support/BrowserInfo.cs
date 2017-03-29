using System;
using System.Web;

namespace FreeTextBoxControls.Support {
	/// <summary>
	/// Detects browser information for FreeTextBox
	/// </summary>
	public class BrowserInfo {
		public BrowserInfo() {}

		#region Private Properties
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


		/*
		public bool isRichCapable;

		public bool isMac;
		public bool isWindows;
		public bool isLinux;
		public bool isPalm;

		public bool isOpera;
		public bool isIE;
		public bool isGecko;
		public bool isWebkit;		 
		*/
		#endregion

		#region Public Properties
		/// <summary>
        /// Gets whether the browser can Rich edit.
        /// </summary>	
        public bool IsRichCapable
        {
            get { return isRichCapable; }
        }

        /// <summary>
        /// Gets whether a browser is Palm-based
        /// </summary>		
        public bool IsPalm
        {
            get { return isPalm; }
        }
        /// <summary>
        /// Gets whether the OS is Mac.
        /// </summary>		
        public bool IsMac
        {
            get { return isMac; }
        }
        /// <summary>
        /// Gets whether the OS is Windows.
        /// </summary>		
        public bool IsWindows
        {
            get { return isWindows; }
        }
        /// <summary>
        /// Gets whether the OS is Linux.
        /// </summary>	
        public bool IsLinux
        {
            get { return isLinux; }
        }
        
        
        /// <summary>
		/// Gets whether a browser is Opera or greater.
		/// </summary>		
		public bool IsOpera {
			get {return isOpera; }
		}
		/// <summary>
		/// Gets whether a browser is IE 5 or greater.
		/// </summary>		
		public bool IsIE {
			get {return isIE; }
		}

		/// <summary>
		/// Gets whether thebrowser has a Gecko rendering engine.
		/// </summary>	
		public bool IsGecko {
			get {return isGecko; }
		}
        /// <summary>
        /// Gets whether thebrowser has a Gecko rendering engine.
        /// </summary>	
        public bool IsWebkit
        {
            get { return isWebkit; }
        }

		/// <summary>
		/// Gets whether the browsser 
		/// </summary>		
		public bool IsiOS {
			get { return isiOS; }
		}

		#endregion 

		/// <summary>
		/// Returns a BrowerInfo object with <see cref="FreeTextBox"/> related information.
		/// </summary>
		public static BrowserInfo GetBrowserInfo() {
			return GetBrowserInfo(System.Web.HttpContext.Current);
		}
		/// <summary>
		/// Returns a BrowerInfo object with <see cref="FreeTextBox"/> related information.
		/// </summary>
		public static BrowserInfo GetBrowserInfo(HttpContext context) {
			BrowserInfo browserInfo = new BrowserInfo();

			HttpBrowserCapabilities browser = context.Request.Browser;
			string userAgent = (context.Request.UserAgent + "").ToLower();
			
            // OS			
            browserInfo.isPalm = (userAgent.IndexOf("palm") > -1);
            browserInfo.isMac = (userAgent.IndexOf("mac") > -1);
            browserInfo.isLinux = (userAgent.IndexOf("linux") > -1);
            browserInfo.isWindows = (userAgent.IndexOf("win") > -1);


            // browser
            browserInfo.isOpera = (userAgent.IndexOf("opera") > -1);
            browserInfo.isIE = (userAgent.IndexOf("msie") > -1);
            browserInfo.isWebkit = (userAgent.IndexOf("webkit") > -1);
            browserInfo.isGecko = (userAgent.IndexOf("gecko") > -1 && !browserInfo.isWebkit);
			browserInfo.isiOS = userAgent.IndexOf("iphone") > -1 || userAgent.IndexOf("ipad") > -1;
            
			
            // (1) IE version
            if (browserInfo.IsIE && !browserInfo.isMac) {
                string identifier = "msie";

                //int ieStart = userAgent.IndexOf(identifier) + identifier.Length;
                //string ieInfo = userAgent.Substring(ieStart, userAgent.IndexOf(";", ieStart) - ieStart);
                
                //Double ieVersion = Convert.ToDouble(context.Request.Browser.MajorVersion + context.Request.Browser.MinorVersionString);
				//context.Request.Browser.

                //if (ieVersion > 5.5)
				if (context.Request.Browser.MajorVersion >= 6)
                    browserInfo.isRichCapable = true;
            } else if (browserInfo.IsGecko) {
            
            	try {
					string dateString = userAgent.Substring(userAgent.IndexOf("gecko/")+6,8);
					DateTime mozillaDate = new DateTime(Convert.ToInt32(dateString.Substring(0,4)),Convert.ToInt32(dateString.Substring(4,2)),Convert.ToInt32(dateString.Substring(6,2)));
					
                    if (mozillaDate >= new DateTime(2003,6,24))
                        browserInfo.isRichCapable = true;

				} catch {}
			} else if (browserInfo.IsOpera) {

                string identifier = "opera/";
                int operaStart = userAgent.IndexOf(identifier) + identifier.Length;
                string operaInfo = userAgent.Substring(operaStart, userAgent.IndexOf("(", operaStart) - operaStart);

                try
                {
                    Double operaVersion = Convert.ToDouble(operaInfo);

                    if (operaVersion > 9)
                        browserInfo.isRichCapable = true;

                }
                catch { }
            }
            else if (browserInfo.isWebkit)
            {
                string identifier = "webkit/";
                int webkitStart = userAgent.IndexOf(identifier) + identifier.Length;
                string webkitInfo = userAgent.Substring(webkitStart, userAgent.IndexOf(".", webkitStart) - webkitStart);

                try
                {
                    Double webkitVersion = Convert.ToDouble(webkitInfo);

                    if (webkitVersion > 412)
                        browserInfo.isRichCapable = true;
                }
                catch { }

				if (browserInfo.IsiOS)
					browserInfo.isRichCapable = false;
            }


			return browserInfo;
		}
	}
}

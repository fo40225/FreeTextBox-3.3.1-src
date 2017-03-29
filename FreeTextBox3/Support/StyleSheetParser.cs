using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using FreeTextBoxControls;

namespace FreeTextBoxControls.Support {
	/// <summary>
	/// Reads stylesheets for FreeTextBox
	/// </summary>	
	public class StyleSheetParser {
		/// <summary>
		/// Finds the stylesheet and reads in correctly formatted styles
		/// </summary>			
		public static string[] ParseStyleSheet(string sheetLocation) {			
			sheetLocation = (System.Web.HttpContext.Current.Request.PhysicalApplicationPath + sheetLocation.Replace("/","\\")).Replace("\\\\","\\");

			if (File.Exists(sheetLocation)) {
				ArrayList stylesList = new ArrayList();	

				StreamReader reader = File.OpenText(sheetLocation);
				string text = reader.ReadToEnd();
				reader.Close();
			
				string rp = @"\.(?<className>[^}]+)\s*{[^}]*}";
				Regex regex = new Regex(rp, RegexOptions.IgnoreCase);
				MatchCollection mc = regex.Matches(text);	

				if (mc.Count > 0) {
					for (int i = 0; i < mc.Count; i++)  {		
						stylesList.Add(mc[i].Groups["className"].Value);
					}
				}
				
				return (string[]) stylesList.ToArray(typeof(string));

			} else {
				return new string[] {"file not found.",sheetLocation};
			}
		}
	}
}

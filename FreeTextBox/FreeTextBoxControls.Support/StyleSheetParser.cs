using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
namespace FreeTextBoxControls.Support
{
	public class StyleSheetParser
	{
		public static string[] ParseStyleSheet(string sheetLocation)
		{
			sheetLocation = (HttpContext.Current.Request.PhysicalApplicationPath + sheetLocation.Replace("/", "\\")).Replace("\\\\", "\\");
			if (File.Exists(sheetLocation))
			{
				ArrayList arrayList = new ArrayList();
				StreamReader streamReader = File.OpenText(sheetLocation);
				string input = streamReader.ReadToEnd();
				streamReader.Close();
				string pattern = "\\.(?<className>[^}]+)\\s*{[^}]*}";
				Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
				MatchCollection matchCollection = regex.Matches(input);
				if (matchCollection.Count > 0)
				{
					for (int i = 0; i < matchCollection.Count; i++)
					{
						arrayList.Add(matchCollection[i].Groups["className"].Value);
					}
				}
				return (string[])arrayList.ToArray(typeof(string));
			}
			return new string[]
			{
				"file not found.",
				sheetLocation
			};
		}
	}
}

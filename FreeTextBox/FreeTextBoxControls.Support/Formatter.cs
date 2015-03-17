using FreeTextBoxControls.Support.Sgml;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
namespace FreeTextBoxControls.Support
{
	public class Formatter
	{
		private ArrayList events = new ArrayList();
		public Formatter()
		{
			this.events.Add("onclick");
			this.events.Add("onmousedown");
			this.events.Add("onmouseover");
			this.events.Add("onmouseout");
			this.events.Add("onmouseup");
		}
		public string HtmlSymbolsToHtmlCodes(string input)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < input.Length; i++)
			{
				char c = input[i];
				if (c > '\u0080')
				{
					stringBuilder.Append("&#");
					StringBuilder arg_33_0 = stringBuilder;
					int num = (int)c;
					arg_33_0.Append(num.ToString());
					stringBuilder.Append(";");
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}
		public string HtmlCodesToHtmlSymbols(string input)
		{
			bool flag = false;
			string text = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < input.Length; i++)
			{
				char c = input[i];
				char c2 = c;
				if (c2 != '&')
				{
					if (c2 != ';')
					{
						if (flag)
						{
							text += c;
						}
						else
						{
							stringBuilder.Append(c);
						}
					}
					else
					{
						if (!flag)
						{
							stringBuilder.Append(c);
						}
						else
						{
							text += c;
							int length = text.Length;
							if (length >= 2 && text[1] == '#' && text[2] != ';')
							{
								text = ((char)int.Parse(text.Substring(2, text.Length - 3))).ToString();
							}
							stringBuilder.Append(text);
							text = string.Empty;
							flag = false;
						}
					}
				}
				else
				{
					stringBuilder.Append(text);
					text = "&";
					flag = true;
				}
			}
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}
		public string RemoveServerNameFromUrls(string input, string serverPath)
		{
			input = Regex.Replace(input, "href=" + serverPath, "href=", RegexOptions.IgnoreCase);
			input = Regex.Replace(input, "href=\"" + serverPath, "href=\"", RegexOptions.IgnoreCase);
			input = Regex.Replace(input, "src=" + serverPath, "src=", RegexOptions.IgnoreCase);
			input = Regex.Replace(input, "src=\"" + serverPath, "src=\"", RegexOptions.IgnoreCase);
			return input;
		}
		public string RemoveScriptNameFromBookmarks(string input, string url)
		{
			input = input.Replace("href=\"" + url, "href=\"");
			input = input.Replace("href=" + url, "href=");
			return input;
		}
		public string RemoveScriptTags(string input)
		{
			input = Regex.Replace(input, "<script(.|\\n)+</script>", "", RegexOptions.IgnoreCase);
			return input;
		}
		public string RemoveWhiteSpace(string input)
		{
			input = input.Replace("\n", " ");
			input = input.Replace("\r", " ");
			input = input.Replace("\t", " ");
			while (input.IndexOf("  ") != -1)
			{
				input = input.Replace("  ", " ");
			}
			return input;
		}
		public string RemoveJavaScriptEventsFromTags(string input)
		{
			Regex regex = new Regex("</?([-\\w]+)( [^>]+)?>");
			input = regex.Replace(input, new MatchEvaluator(this.FixTag));
			return input;
		}
		private string FixTag(Match tagMatch)
		{
			bool flag = true;
			Regex regex = new Regex(" ([-\\w]+)(=(\"[^\"]*\"|'[^']*'|(#|_)?\\w+))?");
			string value = tagMatch.Value;
			string text = "";
			if (value.IndexOf("</") == 0)
			{
				text = value.ToLower();
			}
			else
			{
				string str = tagMatch.Groups[1].Value.ToLower();
				text = "<" + str;
				MatchCollection matchCollection = regex.Matches(value);
				foreach (Match match in matchCollection)
				{
					string arg_84_0 = match.Value;
					string value2 = match.Groups[1].Value;
					string text2 = match.Groups[3].Value;
					text2 = text2.TrimStart(new char[]
					{
						'"'
					});
					text2 = text2.TrimEnd(new char[]
					{
						'"'
					});
					if (!flag || !this.events.Contains(value2))
					{
						string text3 = text;
						text = string.Concat(new string[]
						{
							text3,
							" ",
							value2.ToLower(),
							"=\"",
							text2,
							"\""
						});
					}
				}
				text += ">";
			}
			return text;
		}
		public string HtmlToXhtml(string input)
		{
			string text = string.Empty;
			if (input != null)
			{
				try
				{
					input = "<html>" + input + "</html>";
					SgmlReader sgmlReader = new SgmlReader();
					sgmlReader.DocType = "HTML";
					sgmlReader.InputStream = new StringReader(input);
					sgmlReader.CaseFolding = CaseFolding.ToLower;
					sgmlReader.WhitespaceHandling = WhitespaceHandling.None;
					StringWriter stringWriter = new StringWriter();
					XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
					xmlTextWriter.Formatting = Formatting.Indented;
					xmlTextWriter.IndentChar = '\t';
					while (!sgmlReader.EOF)
					{
						xmlTextWriter.WriteNode(sgmlReader, true);
					}
					xmlTextWriter.Close();
					text = stringWriter.ToString();
					text = text.Substring(6, text.Length - 13);
				}
				catch (Exception ex)
				{
					text = "Error convertoring HTML to XHTML: " + ex.ToString();
				}
			}
			return text;
		}
	}
}

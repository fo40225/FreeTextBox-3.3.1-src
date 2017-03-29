using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Web;

namespace FreeTextBoxControls.Support 
{
	/// <summary>
	/// Formats HTML text.
	/// </summary>	
	public class Formatter {
		private ArrayList events = new ArrayList();
		
		public Formatter() {
			events.Add("onclick");
			events.Add("onmousedown");
			events.Add("onmouseover");
			events.Add("onmouseout");
			events.Add("onmouseup");
		}

		/// <summary>
		/// Converts Html Symbols to their respective Html Codes
		/// </summary>	
		public string HtmlSymbolsToHtmlCodes(string input) {
			StringBuilder sb = new StringBuilder();
			foreach (char c in input) {		
				if ((int) c > 128) {
					sb.Append ("&#");
					sb.Append (((int) c).ToString ());
					sb.Append (";");
				} else {
					sb.Append(c);
				}
			}
			return sb.ToString();
		}
		/// <summary>
		/// Converts Html Codes to Html Characters
		/// </summary>	
		public string HtmlCodesToHtmlSymbols(string input) {
			bool insideEntity = false; // used to indicate that we are in a potential entity
			string entity = String.Empty;
			StringBuilder output = new StringBuilder ();
	
			foreach (char c in input) {
				switch (c) {
					case '&' :
						output.Append (entity);
						entity = "&";
						insideEntity = true;
						break;
					case ';' :
						if (!insideEntity) {
							output.Append (c);
							break;
						}

						entity += c;
						int length = entity.Length;
						if (length >= 2 && entity[1] == '#' && entity[2] != ';')
							entity = ((char) Int32.Parse (entity.Substring (2, entity.Length - 3))).ToString();
					
						output.Append (entity);
						entity = String.Empty;
						insideEntity = false;
						break;
					default :
						if (insideEntity)
							entity += c;
						else
							output.Append (c);
						break;
				}
			}
			output.Append (entity);
			return output.ToString ();
		}
		/// <summary>
		/// Removes the local servername from A and IMG tags
		/// </summary>
		public string RemoveServerNameFromUrls(string input, string serverPath) {

			input = Regex.Replace(input,"href=" + serverPath,"href=",RegexOptions.IgnoreCase);
			input = Regex.Replace(input,"href=\"" + serverPath,"href=\"",RegexOptions.IgnoreCase);
			input = Regex.Replace(input,"src=" + serverPath,"src=",RegexOptions.IgnoreCase);
			input = Regex.Replace(input,"src=\"" + serverPath,"src=\"",RegexOptions.IgnoreCase);
			return input;
		}
		/// <summary>
		/// Removes the scriptname from bookmarks (#mark).
		/// </summary>
		public string RemoveScriptNameFromBookmarks(string input, string url) {
			input = input.Replace("href=\"" + url,"href=\"");
			input = input.Replace("href=" + url,"href=");
			return input;
		}


		/// <summary>
		/// Removes &lt;script&gt; tags
		/// </summary>
		public string RemoveScriptTags(string input) {

			input = Regex.Replace(input,@"<script(.|\n)+</script>","",RegexOptions.IgnoreCase);
			
			return input;
		}

		/// <summary>
		/// Removes unneeded whites space (spaces and breaks)
		/// </summary>
		public string RemoveWhiteSpace(string input) {

			
	
			// white space cleaning
			input = input.Replace("\n", " ");
			input = input.Replace("\r", " ");
			input = input.Replace("\t", " ");
			while(input.IndexOf("  ") != -1) {
				input = input.Replace("  ", " ");
			}
			return input;
		}

		/// <summary>
		/// Removes &lt;script&gt; tags
		/// </summary>
		public string RemoveJavaScriptEventsFromTags(string input) {


			Regex regexTags = new Regex(@"</?([-\w]+)( [^>]+)?>");
			
			input = regexTags.Replace(input,new MatchEvaluator(FixTag));
		
			return input;
		}

		private string FixTag(Match tagMatch) {
			bool removeScripting = true;
			
			Regex regexAttributes = new Regex(@" ([-\w]+)(=(""[^""]*""|'[^']*'|(#|_)?\w+))?");

			// full tag
			string tag = tagMatch.Value;
			string returnTag = "";

			// closing tag
			if (tag.IndexOf("</") == 0) {
				returnTag = tag.ToLower();

			} else {
				string tagName = tagMatch.Groups[1].Value.ToLower();
				returnTag = "<" + tagName;

				MatchCollection attributeMatches = regexAttributes.Matches(tag);

				//Loop through parameters
				foreach(Match attributeMatch in attributeMatches) {
					string fullAttribute = attributeMatch.Value;
			
					string attribute = attributeMatch.Groups[1].Value;
					string value = attributeMatch.Groups[3].Value;
			
					value = value.TrimStart('\"');
					value = value.TrimEnd('\"');

					if (!removeScripting || !events.Contains(attribute)) 
						returnTag += " " + attribute.ToLower() + "=\"" + value + "\"";
				}				
				returnTag += ">";
			}
	
			return returnTag;	
		}
		/// <summary>
		/// Uses <see cref="Sgml.SgmlReader"/> to convert HTML to well-formed XHTML
		/// </summary>
		/// <param name="input">The text to convert</param>
		/// <returns></returns>
		public string HtmlToXhtml(string input) {
			string returnValue = string.Empty;

			if (input != null) {
				try {
					// add HTML tags to ensure at least one element
					input = "<html>" + input + "</html>";
					Sgml.SgmlReader sgmlReader = new Sgml.SgmlReader();
					sgmlReader.DocType = "HTML";
					sgmlReader.InputStream = new StringReader( input );
					sgmlReader.CaseFolding = Sgml.CaseFolding.ToLower;
					sgmlReader.WhitespaceHandling = WhitespaceHandling.None;

					StringWriter stringWriter = new StringWriter();
					XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
					xmlTextWriter.Formatting = Formatting.Indented;
					xmlTextWriter.IndentChar = '\t';

					while (!sgmlReader.EOF) {
						xmlTextWriter.WriteNode(sgmlReader, true);
					}

					xmlTextWriter.Close();

					returnValue = stringWriter.ToString();

					// Finally remove the <html> and </html> tags. 
					returnValue = returnValue.Substring(6, returnValue.Length - 13);
				} catch (Exception e) {
					returnValue = "Error convertoring HTML to XHTML: " + e.ToString();
				}
			}

			return returnValue;
		}
	}
}

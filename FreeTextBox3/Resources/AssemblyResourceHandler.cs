using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

using System.Globalization;

namespace FreeTextBoxControls {
	/// <summary>
	/// The ContentType enumeration has several constant strings that represent the various
	/// MIME content types used for the response stream of ASP.NET. These values are 
	/// particularly useful when specifying <see cref="WebResource"/> attributes.
	/// </summary>
	public struct ContentType {
		/// <summary>
		/// Content type for "image/gif"
		/// </summary>
		public const string ImageGif = "image/gif";
		/// <summary>
		/// Content type for "image/bmp"
		/// </summary>
		public const string ImageBmp = "image/bmp";
		/// <summary>
		/// Content type for "image/jpeg"
		/// </summary>
		public const string ImageJpeg = "image/jpeg";
		/// <summary>
		/// Content type for "image/png"
		/// </summary>
		public const string ImagePng = "image/png";
		/// <summary>
		/// Content type for "text/html"
		/// </summary>
		public const string TextHtml = "text/html";
		/// <summary>
		/// Content type for "text/xml"
		/// </summary>
		public const string TextXml = "text/xml";
		/// <summary>
		/// Content type for "text/x-component"
		/// </summary>
		public const string TextComponent = "text/x-component";

		/// <summary>
		/// Content type for "text/javascript"
		/// </summary>
		public const string JavaScript = "text/javascript";
	}

	/// <summary>
	/// <para>
	/// The AssemblyResourceHandler class is an http handler that is able to retrieve
	/// embedded resources from a particular assembly. It will stream this resource back to the 
	/// requesting client. The embedded resource must be reachable for this handler. For this
	/// an assembly-level attribute <see cref="WebResourceAttribute"/> must be specified.
	/// This attribute specifies the name of the resource file with extension and the content
	/// type. The latter can be specified as a literal string or as one of the 
	/// <see cref="ContentType"/> static values. 
	/// </para>
	/// <para>
	/// If you do not specify the <see cref="WebResourceAttribute"/>attribute, an 
	/// <see cref="System.ArgumentException"/> will be thrown when requesting the resource.
	/// The resource can be retrieved through a special URL, which can be build using the
	/// <see cref="AssemblyResourceHandler.GetWebResourceUrl"/> method.
	/// </para>
	/// <seealso cref="WebResourceAttribute"/>
	/// </summary>
	/// <example>
	/// This example shows how to specify the ScrollablePanel.bmp file, which has a Build Action 
	/// of Embedded Resource, as being reachable for 
	/// <code>
	/// [assembly: WebResource("ScrollablePanel.bmp", ContentType.TextComponent)]
	/// 
	/// //Inside MyPage.aspx.cs
	/// private void Page_Load(object sender, System.EventArgs e)
	/// {
	///     Image1.ImageUrl = AssemblyResourceHandler.GetWebResourceUrl(
	///             typeof(ScrollablePanel), "ScrollablePanel.bmp");
	/// }
	/// </code>
	/// </example>
	public class AssemblyResourceHandler: IHttpHandler {
		#region IHttpHandler Members

		/// <summary>
		/// Process the request from the user
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context) {
			string assembly = context.Request.QueryString["a"];
			string resourceName = context.Request.QueryString["r"];
			string timeStamp = context.Request.QueryString["t"];

			WriteResourceToOutputStream(assembly, resourceName, timeStamp, true);
		}
		#endregion

		/// <summary>
		/// This handler is marked as reusable and always return true.
		/// </summary>
		public bool IsReusable {
			get { return true ; }
		}

		/// <summary>
		/// Load an image from a given assembly
		/// </summary>
		/// <param name="assembly">The name of the assembly to load</param>
		/// <param name="resourceName">The name of the resource to load</param>
		/// <param name="contentType">Will be set to the content type as specified by the
		/// <see cref="WebResource"/></param>
		/// <returns>A <see cref="System.IO.Stream"/> for the embedded resource.</returns>
		public static Stream Load(string assembly, string resourceName, string timeStamp, out string contentType, out bool substitute) {
			Debug.WriteLine(string.Format("Assembly {0}, resource {1}", assembly , resourceName));
			Assembly resourceAssembly;
			try {
				// Attempt to load the assembly
				resourceAssembly = Assembly.Load(assembly);

				//Validate the timestamp

				Uri url = new Uri(resourceAssembly.EscapedCodeBase);
				long time = File.GetLastWriteTimeUtc(url.LocalPath).Ticks;
				if (time.ToString() != timeStamp) {
					throw new ArgumentException("Specified resource timestamp is invalid.");
				}

				// Lookup cached names
				string[] names = HttpContext.Current.Application[assembly] as string[];

				if (null == names) {
					// Get the names of all the resources in the assembly
					names = resourceAssembly.GetManifestResourceNames();
					Array.Sort(names, CaseInsensitiveComparer.Default);
					HttpContext.Current.Application[assembly] = names;
				}

				object[] atts = resourceAssembly.GetCustomAttributes(typeof(WebResourceAttribute), false);
				contentType = string.Empty;
				substitute=false;
				foreach (WebResourceAttribute att in atts) {
					if (string.Compare(att.WebResource, resourceName, true) == 0) {
						contentType = att.ContentType;
						substitute = att.PerformSubstitution;
						break;
					}
				}
				if (contentType.Length == 0)
					throw new ArgumentException("Specified resource is not marked as a WebResource.", resourceName);

				// Check for resource requested
				if (names.Length > 0) {
					// Find the resource in the names array
					string fullResourceName = resourceName;
					int index = Array.BinarySearch(names, fullResourceName, 
						CaseInsensitiveComparer.Default);

					#region Removed Code
					/*
					string resourceAssembly.GetName().Name + "." + resourceName;
					int index = Array.BinarySearch(names, fullResourceName, 
						CaseInsensitiveComparer.Default);
						
					//If the resourceName does not have the assembly name prepended
					if (index == -1)
					{
						fullResourceName = resourceName;
						index = Array.BinarySearch(names, fullResourceName, 
							CaseInsensitiveComparer.Default);
					}
					*/
					#endregion
                    
					if (index > -1) {
						Stream resourceStream = resourceAssembly.GetManifestResourceStream(fullResourceName);
						return resourceStream;
					}
					else {
						throw new ArgumentException(string.Format(
							"The assembly '{0}' does not contain a resource called '{1}'", 
							assembly, resourceName), resourceName);
					}
				}
				else
					throw new ArgumentException(string.Format(
						"The assembly '{0}' contains no resources", assembly), assembly);
			}
			catch (Exception ex) {
				Debug.WriteLine(ex.ToString());
				throw;
			}
			finally {

			}
		}

		/// <summary>
		/// <para>
		/// Composes a full url to be used when referencing a resource of this type
		/// and with the specified name.
		/// </para>
		/// </summary>
		/// <param name="type">Type of the resource.</param>
		/// <param name="resourceName">Name of the embedded resource.</param>
		/// <param name="resourceHandlerPath">Path to the resource handler.</param>
		/// <returns>Url to retrieve this particular resource.</returns>
		public static string GetWebResourceUrl(Type type, string resourceName, string resourceHandlerPath) {
			string assemblyName = type.Assembly.FullName;
			// TODO: implement t to serve as a real time-stamp
			Uri url = new Uri(type.Assembly.EscapedCodeBase);
			long timeStamp = File.GetLastWriteTimeUtc(url.LocalPath).Ticks;
			return CreateWebResourceUrl(assemblyName,resourceName,timeStamp.ToString(), resourceHandlerPath);
		}
		public static string GetWebResourceUrl(Type type, string resourceName) 
		{
			return GetWebResourceUrl(type, resourceName, string.Empty);
		}
		internal static string CreateWebResourceUrl(String assemblyName, string resourceName, string timeStamp, string resourceHandlerPath) 
		{
			return resourceHandlerPath + "FtbWebResource.axd" + string.Format("?a={0}&r={1}&t={2}",
				HttpUtility.UrlEncode(assemblyName),
				HttpUtility.UrlEncode(resourceName), 
				timeStamp);
		}

		private void WriteResourceToOutputStream(string assembly, string resourceName, string timeStamp, bool cache) {
			// Get the resource
			string contentType;
			bool performSubstitution;
			using (Stream resourceStream = Load(assembly, resourceName, timeStamp, out contentType, out performSubstitution)) {
				HttpResponse response = HttpContext.Current.Response;
				//response.Clear();
				if (cache) {
#if DEBUG
                    response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
#else
					response.Cache.SetExpires(DateTime.Now.AddDays(1));
					response.Cache.SetCacheability(HttpCacheability.Public);
#endif
					response.Cache.VaryByParams["a;r;t"]=true;
				}

				response.ContentType = contentType;

				int bytesRead, bufferSize = 1024;
				byte[] buffer = new byte[bufferSize];
                
				if (performSubstitution && contentType.ToLower().StartsWith("text")) {
					//We *could* use a filter to do the text replacement.
					byte[] incomingBuffer=new byte[resourceStream.Length];
					MemoryStream ms = new MemoryStream(incomingBuffer);
					//Read the bytes into a memory stream
					while (true) {
						bytesRead = resourceStream.Read(buffer, 0, bufferSize);
						if (bytesRead <=0) {
							break;
						}
						ms.Write(buffer,0,bytesRead);
					}

					//Search the bytes for replacement text and replace
					String msgText = HttpContext.Current.Response.ContentEncoding.GetString( ms.ToArray());
					ms.Close();
					ms=null;
					Regex regex = new Regex(@"<%\s*=\s*WebResource\(\s*(?<fileName>[^)]+)\)\s*%>",RegexOptions.ExplicitCapture|RegexOptions.IgnoreCase|RegexOptions.Multiline);
					msgText = regex.Replace(msgText,new MatchEvaluator(new WebResourceEvaluator(assembly,timeStamp).Evaluator));
					ms = new MemoryStream(HttpContext.Current.Response.ContentEncoding.GetBytes(msgText));              
                                
					//Reset the stream to the beginning
					ms.Seek(0,SeekOrigin.Begin);

					//Copy the revised stream to the response
					do {
						bytesRead = ms.Read(buffer, 0, bufferSize);
						response.OutputStream.Write(buffer, 0, bytesRead);
					} 
					while (bytesRead > 0);
					ms.Close(); 
				}
				else {
					//Directly copy the byte stream
					do {
						bytesRead = resourceStream.Read(buffer, 0, bufferSize);
						response.OutputStream.Write(buffer, 0, bytesRead);
					} 
					while (bytesRead > 0);
				}

				response.Flush();
				response.End();
			}
		}
	}

	public class WebResourceEvaluator {
		private String assemblyName;
		private String timeStamp;

		public WebResourceEvaluator(string assembly, string time) {
			this.assemblyName=assembly;
			this.timeStamp=time;
		}

		public String Evaluator(Match m) {
			return AssemblyResourceHandler.CreateWebResourceUrl(this.assemblyName,m.Groups["fileName"].Value,this.timeStamp,string.Empty);
		}
	}

	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
	public class WebResourceAttribute: Attribute {
		private string resourceName, contentType;

		//TODO: Implement substitution of resources that have resources
		bool performSubstitution;
		public bool PerformSubstitution {
			get { return performSubstitution; }
			set { performSubstitution = value; }
		}

		public string WebResource {
			get { return resourceName; }
			set { resourceName = value; }
		}
		public string ContentType {
			get { return contentType; }
			set { contentType = value; }
		}

		public WebResourceAttribute(string resourceName, string contentType) {
			this.resourceName = resourceName;
			this.contentType = contentType;
		}
		public WebResourceAttribute(string resourceName, string contentType,bool substitute) {
			this.resourceName = resourceName;
			this.contentType = contentType;
			this.performSubstitution=substitute;
		}

	}
}
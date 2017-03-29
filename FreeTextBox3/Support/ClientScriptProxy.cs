// FROM http://www.west-wind.com/WebLog/posts/10246.aspx


using System;
using System.Web;
using System.Web.UI;
using System.Reflection;

namespace FreeTextBoxControls.Support
{

	/// <summary>
	/// This is a proxy object for the Page.ClientScript and MS Ajax ScriptManager
	/// object that can operate when MS Ajax is not present. Because MS Ajax
	/// may not be available accessing the methods directly is not possible
	/// and we are required to indirectly reference client script methods through
	/// this class.
	///
	/// This class should be invoked at the Control's start up and be used
	/// to replace all calls Page.ClientScript. Scriptmanager calls are made
	/// through Reflection
	/// </summary>
	public class ClientScriptProxy
	{
		private static Type scriptManagerType = null;
	 
		// *** Register proxied methods of ScriptManager
		private static MethodInfo RegisterClientScriptBlockMethod;
		private static MethodInfo RegisterStartupScriptMethod;
		private static MethodInfo RegisterClientScriptIncludeMethod;
		private static MethodInfo RegisterClientScriptResourceMethod;
		private static MethodInfo RegisterHiddenFieldMethod;
		private static MethodInfo GetCurrentMethod;
	 
		//private static MethodInfo RegisterPostBackControlMethod;
		//private static MethodInfo GetWebResourceUrlMethod;
	 
	 
		/// <summary>
		/// Internal global static that gets set when IsMsAjax() is
		/// called. The result is cached once per application so
		/// we don't have keep making reflection calls for each access
		/// </summary>
		private static bool _IsMsAjax = false;
	 
		/// <summary>
		/// Flag that determines whether check was previously done
		/// </summary>
		private static bool _CheckedForMsAjax = false;
	 
		/// <summary>
		/// Cached value to see whether the script manager is
		/// on the page. This value caches here once per page.
		/// </summary>
		private bool _IsScriptManagerOnPage = false;
		private bool _CheckedForScriptManager = false;
	           
		/// <summary>
		/// Current instance of this class which should always be used to
		/// access this object. There are no public constructors to
		/// ensure the reference is used as a Singleton to further
		/// ensure that all scripts are written to the same clientscript
		/// manager.
		/// </summary>
		public static ClientScriptProxy Current
		{
			get
			{
				return
					(HttpContext.Current.Items["__ClientScriptProxy"] ??
					(HttpContext.Current.Items["__ClientScriptProxy"] =
						new ClientScriptProxy()))
					as ClientScriptProxy;
			}
		}
	 
		/// <summary>
		/// No public constructor - use ClientScriptProxy.Current to
		/// get an instance to ensure you once have one instance per
		/// page active.
		/// </summary>
		protected ClientScriptProxy()
		{
		}
	 
		/// <summary>
		/// Checks to see if MS Ajax is registered with the current
		/// Web application.
		///
		/// Note: Method is static so it can be directly accessed from
		/// anywhere. If you use the IsMsAjax property to check the
		/// value this method fires only once per application.
		/// </summary>
		/// <returns></returns>
		public static bool IsMsAjax()
		{
			if (_CheckedForMsAjax)
				return _IsMsAjax;
	 
			// *** Easiest but we don't want to hardcode the version here
			// scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", false);
	 
			// *** To be safe and compliant we need to look through all loaded assemblies           
			Assembly ScriptAssembly = null; // Assembly.LoadWithPartialName("System.Web.Extensions");
			foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
			{
				string fn = ass.FullName;
				if (fn.StartsWith("System.Web.Extensions"))
				{
					ScriptAssembly = ass;
					break;
				}
			}
	 
			if (ScriptAssembly == null)
				return false;
	 
			scriptManagerType = ScriptAssembly.GetType("System.Web.UI.ScriptManager");
	 
			if (scriptManagerType == null)
			{
				_IsMsAjax = false;
				_CheckedForMsAjax = true;
				return false;
	 
			}
	 
			// *** Method to check for current instance on a page - cache
			// *** since we might call this frequently
			GetCurrentMethod = scriptManagerType.GetMethod("GetCurrent");
	       
			_IsMsAjax = true;
			_CheckedForMsAjax = true;
	 
			return true;
		}
	 
		/// <summary>
		/// Checks to see if a script manager is on the page
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
		public bool IsScriptManagerOnPage(Page page)
		{            
			// *** Check is done only once per page
			if (this._CheckedForScriptManager)
				return _IsScriptManagerOnPage;
	       
			// *** Must check whether MS Ajax is available
			// *** at all first. Method sets up scriptManager
			// *** and GetCurrentMethod on success.
			if (!IsMsAjax())
			{
				this._CheckedForScriptManager = true;
				this._IsScriptManagerOnPage = false;
				return false;
			}
	 
			// *** Now check and see if we can get a ref to the script manager
			object sm = GetCurrentMethod.Invoke(null, new object[1] { page });
			if (sm == null)
				this._IsScriptManagerOnPage = false;
			else
				this._IsScriptManagerOnPage = true;
	 
			this._CheckedForScriptManager = true;
			return this._IsScriptManagerOnPage;
		}

		public bool IsScriptManagerInAsyncPostBack(Page page) {
			bool isScriptManagerOnPage = this.IsScriptManagerOnPage(page);

			if (!isScriptManagerOnPage)
				return false;

			object sm = GetCurrentMethod.Invoke(null, new object[1] { page });
			Type smType = sm.GetType();
			PropertyInfo isInAsyncPostBackProperty = smType.GetProperty("IsInAsyncPostBack");

			return (bool) isInAsyncPostBackProperty.GetValue(sm, null);
		}
	   
		/// <summary>
		/// Returns a WebResource or ScriptResource URL for script resources that are to be
		/// embedded as script includes.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="type"></param>
		/// <param name="resourceName"></param>
		public void RegisterClientScriptResource(Control control, Type type, string resourceName)
		{
			if ( this.IsScriptManagerOnPage(control.Page) )
			{
				// *** NOTE: If MS Ajax is referenced, but no scriptmanager is on the page
				//           script no compression will occur. With a script manager
				//           on the page compression will be handled by MS Ajax.
				if (RegisterClientScriptResourceMethod == null)
					RegisterClientScriptResourceMethod = scriptManagerType.GetMethod("RegisterClientScriptResource",
																 new Type[3] { typeof(Control), typeof(Type), typeof(string) });
	 
				RegisterClientScriptResourceMethod.Invoke(null, new object[3] { control, type, resourceName });
			}
	 
			//// *** If wwScriptCompression Module through Web.config is loaded use it to compress
			//// *** script resources by using wcSC.axd Url the module intercepts
			//if (wwScriptCompressionModule.wwScriptCompressionModuleActive)
			//{
			//    if (type.Assembly == this.GetType().Assembly)
	 
			//        RegisterClientScriptInclude(control, type, resourceName, "wwSC.axd?r=" +
			//                                    Convert.ToBase64String(Encoding.ASCII.GetBytes(resourceName)));
			//    else
			//        RegisterClientScriptInclude(control, type, resourceName, "wwSC.axd?r=" +
			//                                    Convert.ToBase64String(Encoding.ASCII.GetBytes(resourceName)) +
			//                                    "&t=" +
			//                                    Convert.ToBase64String(Encoding.ASCII.GetBytes(type.Assembly.FullName)));
			//}       
	  else
				// *** Otherwise just embed a script reference into the page
				control.Page.ClientScript.RegisterClientScriptResource(type, resourceName);
		}
	   
		/// <summary>
		/// Registers a client script block in the page.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="type"></param>
		/// <param name="key"></param>
		/// <param name="script"></param>
		/// <param name="addScriptTags"></param>
		public void RegisterClientScriptBlock(Control control, Type type, string key, string script, bool addScriptTags)
		{
			if (IsMsAjax())
			{
				if (RegisterClientScriptBlockMethod == null)
					RegisterClientScriptBlockMethod = scriptManagerType.GetMethod("RegisterClientScriptBlock", new Type[5] { typeof(Control), typeof(Type), typeof(string), typeof(string), typeof(bool) });
	 
				RegisterClientScriptBlockMethod.Invoke(null, new object[5] { control, type, key, script, addScriptTags });
			}
			else
				control.Page.ClientScript.RegisterClientScriptBlock(type, key, script, addScriptTags);
		}
	 
		/// <summary>
		/// Registers a startup code snippet that gets placed at the bottom of the page
		/// </summary>
		/// <param name="control"></param>
		/// <param name="type"></param>
		/// <param name="key"></param>
		/// <param name="script"></param>
		/// <param name="addStartupTags"></param>
		public void RegisterStartupScript(Control control, Type type, string key, string script, bool addStartupTags)
		{
			if (IsMsAjax())
			{
				if (RegisterStartupScriptMethod == null)
					RegisterStartupScriptMethod = scriptManagerType.GetMethod("RegisterStartupScript", new Type[5] { typeof(Control), typeof(Type), typeof(string), typeof(string), typeof(bool) });
	 
				RegisterStartupScriptMethod.Invoke(null, new object[5] { control, type, key, script, addStartupTags });
			}
			else
				control.Page.ClientScript.RegisterStartupScript(type, key, script, addStartupTags);
	 
		}
	 
		/// <summary>
		/// Registers a script include tag into the page for an external script url
		/// </summary>
		/// <param name="control"></param>
		/// <param name="type"></param>
		/// <param name="key"></param>
		/// <param name="url"></param>
		public void RegisterClientScriptInclude(Control control, Type type, string key, string url)
		{
			if (IsMsAjax())
			{
				if (RegisterClientScriptIncludeMethod == null)
					RegisterClientScriptIncludeMethod = scriptManagerType.GetMethod("RegisterClientScriptInclude", new Type[4] { typeof(Control), typeof(Type), typeof(string), typeof(string) });
	 
				RegisterClientScriptIncludeMethod.Invoke(null, new object[4] { control, type, key, url });
			}
			else
				control.Page.ClientScript.RegisterClientScriptInclude(type, key, url);
		}
	 
		/// <summary>
		/// Returns a WebResource URL for non script resources
		/// </summary>
		/// <param name="control"></param>
		/// <param name="type"></param>
		/// <param name="resourceName"></param>
		/// <returns></returns>
		public string GetWebResourceUrl(Control control, Type type, string resourceName)
		{
			return control.Page.ClientScript.GetWebResourceUrl(type, resourceName);
		}
	 
		/// <summary>
		/// Injects a hidden field into the page
		/// </summary>
		/// <param name="control"></param>
		/// <param name="hiddenFieldName"></param>
		/// <param name="hiddenFieldInitialValue"></param>
		public void RegisterHiddenField(Control control, string hiddenFieldName, string hiddenFieldInitialValue)
		{
			if (IsMsAjax())
			{
				if (RegisterHiddenFieldMethod == null)
					RegisterHiddenFieldMethod = scriptManagerType.GetMethod("RegisterHiddenField", new Type[3] { typeof(Control), typeof(string), typeof(string) });
	 
				RegisterHiddenFieldMethod.Invoke(null, new object[3] { control, hiddenFieldName, hiddenFieldInitialValue });
			}
			else
				control.Page.ClientScript.RegisterHiddenField(hiddenFieldName, hiddenFieldInitialValue);
		}
	 
	}
}
using System;
using System.Web;
using System.Web.UI;

namespace FreeTextBoxControls.Support
{
	/// <summary>
	/// Wrapper for methods that differ from .NET 1.x to 2.0
	/// </summary>
	public class ClientScriptWrapper
	{
		public static string GetWebResourceUrl(Control control, string resourceName,string assemblyResourceHandlerPath) 
		{
			return AssemblyResourceHandler.GetWebResourceUrl(typeof(FreeTextBox), resourceName, assemblyResourceHandlerPath);
		}

		public static void RegisterRequiresPostBack(Page page, Control control)
		{
			page.GetPostBackEventReference(control, "");
		}

		public static bool IsClientScriptBlockRegistered(Page page, string key)
		{
			return page.IsClientScriptBlockRegistered(key);
		}

		public static void RegisterOnSubmitStatement(Page page, Type type, string key, string script)
		{
			page.RegisterOnSubmitStatement(key, script);
		}

		public static void RegisterClientScriptBlock(Page page, Control control, string key, string script)
		{
			page.RegisterClientScriptBlock(key, script);
		}

		public static bool IsStartupScriptRegistered(Page page, string key)
		{
			return page.IsStartupScriptRegistered(key);
		}

		public static void RegisterStartupScript(Page page, Type type, string key, string script)
		{
			page.RegisterStartupScript(key, script);
		}
	}
}

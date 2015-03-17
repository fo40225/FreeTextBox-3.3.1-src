using System;
using System.Web.UI;
namespace FreeTextBoxControls.Support
{
	public class ClientScriptWrapper
	{
		public static string GetWebResourceUrl(Control control, string resourceName)
		{
			return MsAjaxProxy.Current.GetWebResourceUrl(control, control.GetType(), resourceName);
		}
		public static string GetWebResourceUrl(Control control, string resourceName, string assemblyResourceHandlerPath)
		{
			return MsAjaxProxy.Current.GetWebResourceUrl(control, control.GetType(), resourceName);
		}
		public static void RegisterRequiresPostBack(Page page, Control control)
		{
			page.RegisterRequiresPostBack(control);
		}
		public static bool IsClientScriptBlockRegistered(Page page, string key)
		{
			return page.ClientScript.IsClientScriptBlockRegistered(key);
		}
		public static void RegisterOnSubmitStatement(Page page, Type type, string key, string script)
		{
			page.ClientScript.RegisterOnSubmitStatement(type, key, script);
		}
		public static void RegisterClientScriptBlock(Page page, Control control, string key, string script)
		{
			MsAjaxProxy.Current.RegisterClientScriptBlock(control, control.GetType(), key, script, false);
		}
		public static bool IsStartupScriptRegistered(Page page, string key)
		{
			return page.ClientScript.IsStartupScriptRegistered(key);
		}
		public static void RegisterStartupScript(Page page, Type type, string key, string script)
		{
			MsAjaxProxy.Current.RegisterStartupScript(page, typeof(Page), key, script, false);
		}
	}
}

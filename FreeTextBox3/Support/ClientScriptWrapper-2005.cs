using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace FreeTextBoxControls.Support
{
	/// <summary>
	/// Wrapper for methods that differ from .NET 1.x to 2.0
	/// </summary>
	public class ClientScriptWrapper
	{
        public static string GetWebResourceUrl(Control control, string resourceName)
        {
            return MsAjaxProxy.Current.GetWebResourceUrl(control, control.GetType(), resourceName);
            //return control.Page.ClientScript.GetWebResourceUrl(typeof(FreeTextBox), resourceName);
        }
        
        public static string GetWebResourceUrl(Control control, string resourceName, string assemblyResourceHandlerPath)
        {
            return MsAjaxProxy.Current.GetWebResourceUrl(control, control.GetType(), resourceName);
            //return control.Page.ClientScript.GetWebResourceUrl(typeof(FreeTextBox), resourceName);
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
            //page.ClientScript.RegisterClientScriptBlock(control.GetType(), key, script);
        }

        public static bool IsStartupScriptRegistered(Page page, string key)
        {
            return page.ClientScript.IsStartupScriptRegistered(key);
        }

        public static void RegisterStartupScript(Page page, Type type, string key, string script)
        {
            MsAjaxProxy.Current.RegisterStartupScript(page, typeof(Page), key, script, false);
            //page.ClientScript.RegisterStartupScript(type, key, script);
        }
	}
}

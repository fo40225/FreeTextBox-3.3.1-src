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
    public class MsAjaxProxy
    {
        /// <summary>
        /// Current instance of this class which should always be used to
        /// access this object. There are no public constructors to
        /// ensure the reference is used as a Singleton to further
        /// ensure that all scripts are written to the same clientscript
        /// manager.
        /// </summary>
        public static MsAjaxProxy Current
        {
            get
            {
                return new MsAjaxProxy();
            }
        }

        /// <summary>
        /// No public constructor - use ClientScriptProxy.Current to
        /// get an instance to ensure you once have one instance per
        /// page active.
        /// </summary>
        protected MsAjaxProxy()
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
           return false;
        }

        /// <summary>
        /// Checks to see if a script manager is on the page
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public bool IsScriptManagerOnPage(Page page)
        {
            return false;
        }

        public bool IsScriptManagerInAsyncPostBack(Page page)
        {
            return false;
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
            control.Page.RegisterClientScriptBlock(key, script);
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
            control.Page.RegisterStartupScript(key, script);
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
            return "";
        }

        /// <summary>
        /// Injects a hidden field into the page
        /// </summary>
        /// <param name="control"></param>
        /// <param name="hiddenFieldName"></param>
        /// <param name="hiddenFieldInitialValue"></param>
        public void RegisterHiddenField(Control control, string hiddenFieldName, string hiddenFieldInitialValue)
        {
			
       }

    }
}
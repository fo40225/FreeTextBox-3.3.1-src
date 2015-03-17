using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
namespace FreeTextBoxControls.Support
{
	public class MsAjaxProxy
	{
		private static Type scriptManagerType = null;
		private static MethodInfo RegisterClientScriptBlockMethod;
		private static MethodInfo RegisterStartupScriptMethod;
		private static MethodInfo RegisterClientScriptIncludeMethod;
		private static MethodInfo RegisterClientScriptResourceMethod;
		private static MethodInfo RegisterHiddenFieldMethod;
		private static MethodInfo GetCurrentMethod;
		private static bool _IsMsAjax = false;
		private static bool _CheckedForMsAjax = false;
		private bool _IsScriptManagerOnPage;
		private bool _CheckedForScriptManager;
		public static MsAjaxProxy Current
		{
			get
			{
				object arg_34_0;
				if ((arg_34_0 = HttpContext.Current.Items["__ClientScriptProxy"]) == null)
				{
					arg_34_0 = (HttpContext.Current.Items["__ClientScriptProxy"] = new MsAjaxProxy());
				}
				return arg_34_0 as MsAjaxProxy;
			}
		}
		protected MsAjaxProxy()
		{
		}
		public static bool IsMsAjax()
		{
			if (MsAjaxProxy._CheckedForMsAjax)
			{
				return MsAjaxProxy._IsMsAjax;
			}
			MsAjaxProxy.scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", false);
			if (MsAjaxProxy.scriptManagerType == null)
			{
				MsAjaxProxy._IsMsAjax = false;
				MsAjaxProxy._CheckedForMsAjax = true;
				return false;
			}
			MsAjaxProxy.GetCurrentMethod = MsAjaxProxy.scriptManagerType.GetMethod("GetCurrent");
			MsAjaxProxy._IsMsAjax = true;
			MsAjaxProxy._CheckedForMsAjax = true;
			return true;
		}
		public bool IsScriptManagerOnPage(Page page)
		{
			if (this._CheckedForScriptManager)
			{
				return this._IsScriptManagerOnPage;
			}
			if (!MsAjaxProxy.IsMsAjax())
			{
				this._CheckedForScriptManager = true;
				this._IsScriptManagerOnPage = false;
				return false;
			}
			if (MsAjaxProxy.GetCurrentMethod.Invoke(null, new object[]
			{
				page
			}) == null)
			{
				this._IsScriptManagerOnPage = false;
			}
			else
			{
				this._IsScriptManagerOnPage = true;
			}
			this._CheckedForScriptManager = true;
			return this._IsScriptManagerOnPage;
		}
		public bool IsScriptManagerInAsyncPostBack(Page page)
		{
			if (!this.IsScriptManagerOnPage(page))
			{
				return false;
			}
			object obj = MsAjaxProxy.GetCurrentMethod.Invoke(null, new object[]
			{
				page
			});
			Type type = obj.GetType();
			PropertyInfo property = type.GetProperty("IsInAsyncPostBack");
			return (bool)property.GetValue(obj, null);
		}
		public void RegisterClientScriptResource(Control control, Type type, string resourceName)
		{
			if (this.IsScriptManagerOnPage(control.Page))
			{
				if (MsAjaxProxy.RegisterClientScriptResourceMethod == null)
				{
					MsAjaxProxy.RegisterClientScriptResourceMethod = MsAjaxProxy.scriptManagerType.GetMethod("RegisterClientScriptResource", new Type[]
					{
						typeof(Control),
						typeof(Type),
						typeof(string)
					});
				}
				MsAjaxProxy.RegisterClientScriptResourceMethod.Invoke(null, new object[]
				{
					control,
					type,
					resourceName
				});
				return;
			}
			control.Page.ClientScript.RegisterClientScriptResource(type, resourceName);
		}
		public void RegisterClientScriptBlock(Control control, Type type, string key, string script, bool addScriptTags)
		{
			if (MsAjaxProxy.IsMsAjax())
			{
				if (MsAjaxProxy.RegisterClientScriptBlockMethod == null)
				{
					MsAjaxProxy.RegisterClientScriptBlockMethod = MsAjaxProxy.scriptManagerType.GetMethod("RegisterClientScriptBlock", new Type[]
					{
						typeof(Control),
						typeof(Type),
						typeof(string),
						typeof(string),
						typeof(bool)
					});
				}
				MsAjaxProxy.RegisterClientScriptBlockMethod.Invoke(null, new object[]
				{
					control,
					type,
					key,
					script,
					addScriptTags
				});
				return;
			}
			control.Page.ClientScript.RegisterClientScriptBlock(type, key, script, addScriptTags);
		}
		public void RegisterStartupScript(Control control, Type type, string key, string script, bool addStartupTags)
		{
			if (MsAjaxProxy.IsMsAjax())
			{
				if (MsAjaxProxy.RegisterStartupScriptMethod == null)
				{
					MsAjaxProxy.RegisterStartupScriptMethod = MsAjaxProxy.scriptManagerType.GetMethod("RegisterStartupScript", new Type[]
					{
						typeof(Control),
						typeof(Type),
						typeof(string),
						typeof(string),
						typeof(bool)
					});
				}
				MsAjaxProxy.RegisterStartupScriptMethod.Invoke(null, new object[]
				{
					control,
					type,
					key,
					script,
					addStartupTags
				});
				return;
			}
			control.Page.ClientScript.RegisterStartupScript(type, key, script, addStartupTags);
		}
		public void RegisterClientScriptInclude(Control control, Type type, string key, string url)
		{
			if (MsAjaxProxy.IsMsAjax())
			{
				if (MsAjaxProxy.RegisterClientScriptIncludeMethod == null)
				{
					MsAjaxProxy.RegisterClientScriptIncludeMethod = MsAjaxProxy.scriptManagerType.GetMethod("RegisterClientScriptInclude", new Type[]
					{
						typeof(Control),
						typeof(Type),
						typeof(string),
						typeof(string)
					});
				}
				MsAjaxProxy.RegisterClientScriptIncludeMethod.Invoke(null, new object[]
				{
					control,
					type,
					key,
					url
				});
				return;
			}
			control.Page.ClientScript.RegisterClientScriptInclude(type, key, url);
		}
		public string GetWebResourceUrl(Control control, Type type, string resourceName)
		{
			return control.Page.ClientScript.GetWebResourceUrl(type, resourceName);
		}
		public void RegisterHiddenField(Control control, string hiddenFieldName, string hiddenFieldInitialValue)
		{
			if (MsAjaxProxy.IsMsAjax())
			{
				if (MsAjaxProxy.RegisterHiddenFieldMethod == null)
				{
					MsAjaxProxy.RegisterHiddenFieldMethod = MsAjaxProxy.scriptManagerType.GetMethod("RegisterHiddenField", new Type[]
					{
						typeof(Control),
						typeof(string),
						typeof(string)
					});
				}
				MsAjaxProxy.RegisterHiddenFieldMethod.Invoke(null, new object[]
				{
					control,
					hiddenFieldName,
					hiddenFieldInitialValue
				});
				return;
			}
			control.Page.ClientScript.RegisterHiddenField(hiddenFieldName, hiddenFieldInitialValue);
		}
	}
}

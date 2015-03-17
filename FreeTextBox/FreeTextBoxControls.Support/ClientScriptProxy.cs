using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
namespace FreeTextBoxControls.Support
{
	public class ClientScriptProxy
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
		public static ClientScriptProxy Current
		{
			get
			{
				object arg_34_0;
				if ((arg_34_0 = HttpContext.Current.Items["__ClientScriptProxy"]) == null)
				{
					arg_34_0 = (HttpContext.Current.Items["__ClientScriptProxy"] = new ClientScriptProxy());
				}
				return arg_34_0 as ClientScriptProxy;
			}
		}
		protected ClientScriptProxy()
		{
		}
		public static bool IsMsAjax()
		{
			if (ClientScriptProxy._CheckedForMsAjax)
			{
				return ClientScriptProxy._IsMsAjax;
			}
			Assembly assembly = null;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Assembly assembly2 = assemblies[i];
				string fullName = assembly2.FullName;
				if (fullName.StartsWith("System.Web.Extensions"))
				{
					assembly = assembly2;
					break;
				}
			}
			if (assembly == null)
			{
				return false;
			}
			ClientScriptProxy.scriptManagerType = assembly.GetType("System.Web.UI.ScriptManager");
			if (ClientScriptProxy.scriptManagerType == null)
			{
				ClientScriptProxy._IsMsAjax = false;
				ClientScriptProxy._CheckedForMsAjax = true;
				return false;
			}
			ClientScriptProxy.GetCurrentMethod = ClientScriptProxy.scriptManagerType.GetMethod("GetCurrent");
			ClientScriptProxy._IsMsAjax = true;
			ClientScriptProxy._CheckedForMsAjax = true;
			return true;
		}
		public bool IsScriptManagerOnPage(Page page)
		{
			if (this._CheckedForScriptManager)
			{
				return this._IsScriptManagerOnPage;
			}
			if (!ClientScriptProxy.IsMsAjax())
			{
				this._CheckedForScriptManager = true;
				this._IsScriptManagerOnPage = false;
				return false;
			}
			if (ClientScriptProxy.GetCurrentMethod.Invoke(null, new object[]
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
			object obj = ClientScriptProxy.GetCurrentMethod.Invoke(null, new object[]
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
				if (ClientScriptProxy.RegisterClientScriptResourceMethod == null)
				{
					ClientScriptProxy.RegisterClientScriptResourceMethod = ClientScriptProxy.scriptManagerType.GetMethod("RegisterClientScriptResource", new Type[]
					{
						typeof(Control),
						typeof(Type),
						typeof(string)
					});
				}
				ClientScriptProxy.RegisterClientScriptResourceMethod.Invoke(null, new object[]
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
			if (ClientScriptProxy.IsMsAjax())
			{
				if (ClientScriptProxy.RegisterClientScriptBlockMethod == null)
				{
					ClientScriptProxy.RegisterClientScriptBlockMethod = ClientScriptProxy.scriptManagerType.GetMethod("RegisterClientScriptBlock", new Type[]
					{
						typeof(Control),
						typeof(Type),
						typeof(string),
						typeof(string),
						typeof(bool)
					});
				}
				ClientScriptProxy.RegisterClientScriptBlockMethod.Invoke(null, new object[]
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
			if (ClientScriptProxy.IsMsAjax())
			{
				if (ClientScriptProxy.RegisterStartupScriptMethod == null)
				{
					ClientScriptProxy.RegisterStartupScriptMethod = ClientScriptProxy.scriptManagerType.GetMethod("RegisterStartupScript", new Type[]
					{
						typeof(Control),
						typeof(Type),
						typeof(string),
						typeof(string),
						typeof(bool)
					});
				}
				ClientScriptProxy.RegisterStartupScriptMethod.Invoke(null, new object[]
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
			if (ClientScriptProxy.IsMsAjax())
			{
				if (ClientScriptProxy.RegisterClientScriptIncludeMethod == null)
				{
					ClientScriptProxy.RegisterClientScriptIncludeMethod = ClientScriptProxy.scriptManagerType.GetMethod("RegisterClientScriptInclude", new Type[]
					{
						typeof(Control),
						typeof(Type),
						typeof(string),
						typeof(string)
					});
				}
				ClientScriptProxy.RegisterClientScriptIncludeMethod.Invoke(null, new object[]
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
			if (ClientScriptProxy.IsMsAjax())
			{
				if (ClientScriptProxy.RegisterHiddenFieldMethod == null)
				{
					ClientScriptProxy.RegisterHiddenFieldMethod = ClientScriptProxy.scriptManagerType.GetMethod("RegisterHiddenField", new Type[]
					{
						typeof(Control),
						typeof(string),
						typeof(string)
					});
				}
				ClientScriptProxy.RegisterHiddenFieldMethod.Invoke(null, new object[]
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

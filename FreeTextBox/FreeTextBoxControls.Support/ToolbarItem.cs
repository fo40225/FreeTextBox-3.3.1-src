using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
namespace FreeTextBoxControls.Support
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public abstract class ToolbarItem : IStateManager
	{
		protected bool g_isTrackingViewState;
		protected StateBag g_viewState;
		private string functionName;
		public string FunctionName
		{
			get
			{
				return this.functionName;
			}
			set
			{
				this.functionName = value;
			}
		}
		internal string className
		{
			get
			{
				object obj = this.ViewState["className"];
				if (obj != null)
				{
					return (string)this.ViewState["className"];
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["className"] = value;
			}
		}
		internal string builtInScript
		{
			get
			{
				object obj = this.ViewState["builtInScript"];
				if (obj != null)
				{
					return (string)this.ViewState["builtInScript"];
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["builtInScript"] = value;
			}
		}
		internal string builtInStateScript
		{
			get
			{
				object obj = this.ViewState["builtInStateScript"];
				if (obj != null)
				{
					return (string)this.ViewState["builtInStateScript"];
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["builtInStateScript"] = value;
			}
		}
		internal string builtInEnabledScript
		{
			get
			{
				object obj = this.ViewState["builtInEnabledScript"];
				if (obj != null)
				{
					return (string)this.ViewState["builtInEnabledScript"];
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["builtInEnabledScript"] = value;
			}
		}
		internal bool isBuiltIn
		{
			get
			{
				object obj = this.ViewState["isBuiltIn"];
				return obj != null && (bool)this.ViewState["isBuiltIn"];
			}
			set
			{
				this.ViewState["isBuiltIn"] = value;
			}
		}
		internal bool isProFeature
		{
			get
			{
				object obj = this.ViewState["isProFeature"];
				return obj != null && (bool)this.ViewState["isProFeature"];
			}
			set
			{
				this.ViewState["isProFeature"] = value;
			}
		}
		internal bool TitleHasBeenSet
		{
			get
			{
				object obj = this.ViewState["TitleHasBeenSet"];
				return obj != null && (bool)this.ViewState["TitleHasBeenSet"];
			}
		}
		public bool IsProFeature
		{
			get
			{
				return this.isProFeature;
			}
		}
		public bool IsBuiltIn
		{
			get
			{
				return this.isBuiltIn;
			}
		}
		[Browsable(false)]
		public string BuiltInScript
		{
			get
			{
				return this.builtInScript;
			}
		}
		[Browsable(false)]
		public string BuiltInStateScript
		{
			get
			{
				return this.builtInStateScript;
			}
		}
		[Browsable(false)]
		public string BuiltInEnabledScript
		{
			get
			{
				return this.builtInEnabledScript;
			}
		}
		internal string ClientID
		{
			get
			{
				object obj = this.ViewState["ClientID"];
				if (obj != null)
				{
					return (string)this.ViewState["ClientID"];
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ClientID"] = value;
			}
		}
		[Category("Behavior"), Description("The alt text for the ToolbarButton"), NotifyParentProperty(true)]
		public string Title
		{
			get
			{
				object obj = this.ViewState["Title"];
				if (obj != null)
				{
					return (string)this.ViewState["Title"];
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["Title"] = value;
				if (this.ClientID == "")
				{
					this.ClientID = value.Replace(" ", "");
				}
				this.ViewState["TitleHasBeenSet"] = true;
				HttpContext.Current.Trace.Write("Title", "set: " + value);
			}
		}
		[Category("Behavior"), DefaultValue(""), Description("The command identifier used for dynamic updating through the queryCommandState method"), NotifyParentProperty(true)]
		public string CommandIdentifier
		{
			get
			{
				object obj = this.ViewState["CommandIdentifier"];
				if (obj != null)
				{
					return (string)this.ViewState["CommandIdentifier"];
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CommandIdentifier"] = value;
			}
		}
		[Category("Behavior"), DefaultValue(""), Description("The JavaScript code for the function"), NotifyParentProperty(true), TypeConverter(typeof(string)), PersistenceMode(PersistenceMode.InnerProperty)]
		public string ScriptBlock
		{
			get
			{
				object obj = this.ViewState["ScriptBlock"];
				if (obj != null)
				{
					return (string)this.ViewState["ScriptBlock"];
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ScriptBlock"] = value;
			}
		}
		[Category("Behavior"), DefaultValue(""), Description("The JavaScript code to determine the state of the button or list"), NotifyParentProperty(true), TypeConverter(typeof(string)), PersistenceMode(PersistenceMode.InnerProperty)]
		public string StateScriptBlock
		{
			get
			{
				object obj = this.ViewState["StateScriptBlock"];
				if (obj != null)
				{
					return (string)this.ViewState["StateScriptBlock"];
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["StateScriptBlock"] = value;
			}
		}
		[Category("Behavior"), DefaultValue(""), Description("The JavaScript code to determine if the button or list is enabled"), NotifyParentProperty(true), TypeConverter(typeof(string)), PersistenceMode(PersistenceMode.InnerProperty)]
		public string EnabledScriptBlock
		{
			get
			{
				object obj = this.ViewState["EnabledScriptBlock"];
				if (obj != null)
				{
					return (string)this.ViewState["EnabledScriptBlock"];
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["EnabledScriptBlock"] = value;
			}
		}
		internal string ID
		{
			get
			{
				return this.Title.Replace(" ", "");
			}
		}
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected StateBag ViewState
		{
			get
			{
				if (this.g_viewState == null)
				{
					this.g_viewState = new StateBag(false);
					if (this.g_isTrackingViewState)
					{
						((IStateManager)this.g_viewState).TrackViewState();
					}
				}
				return this.g_viewState;
			}
		}
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this.g_isTrackingViewState;
			}
		}
		internal void SetTitleLanguage(string title)
		{
			this.ViewState["Title"] = title;
			if (this.ClientID == "")
			{
				this.ClientID = title.Replace(" ", "");
			}
		}
		internal void SetDirty()
		{
			if (this.g_viewState != null)
			{
				ICollection keys = this.g_viewState.Keys;
				foreach (string key in keys)
				{
					this.g_viewState.SetItemDirty(key, true);
				}
			}
		}
		void IStateManager.LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				((IStateManager)this.g_viewState).LoadViewState(savedState);
			}
		}
		object IStateManager.SaveViewState()
		{
			if (this.g_viewState != null)
			{
				return ((IStateManager)this.g_viewState).SaveViewState();
			}
			return null;
		}
		void IStateManager.TrackViewState()
		{
			this.g_isTrackingViewState = true;
			if (this.g_viewState != null)
			{
				((IStateManager)this.g_viewState).TrackViewState();
			}
		}
	}
}

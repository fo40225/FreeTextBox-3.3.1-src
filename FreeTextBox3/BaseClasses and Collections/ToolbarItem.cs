using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;

namespace FreeTextBoxControls.Support {
	/// <summary>
	/// Base class for all toolbar items (<see cref="ToolbarButton"/>, <see cref="ToolbarDropDownList"/>, etc.)
	/// </summary>
	[
	TypeConverter(typeof(ExpandableObjectConverter))
	]
	public abstract class ToolbarItem: IStateManager {

		#region Private Properties
		/*
		internal bool isBuiltIn = false;	
		internal bool isProFeature = false;	
		internal string builtInScript = string.Empty;
		internal string builtInStateScript = string.Empty;
		internal string builtInEnabledScript = string.Empty;
		*/
        protected bool g_isTrackingViewState;
        protected StateBag g_viewState;
		#endregion
		
		#region Depreciated
		string functionName;
		public string FunctionName {
			get { return functionName; }
			set { functionName = value; }
		}
		#endregion

		#region Internal Properties
		internal string className {
			get {
				object o = ViewState["className"];
				return (o==null) ? string.Empty : (string) ViewState["className"];
			}		
			set {
				ViewState["className"] = value;
			}
		}
		internal string builtInScript {
			get {
				object o = ViewState["builtInScript"];
				return (o==null) ? string.Empty : (string) ViewState["builtInScript"];
			}		
			set {
				ViewState["builtInScript"] = value;
			}
		}
		internal string builtInStateScript {
			get {
				object o = ViewState["builtInStateScript"];
				return (o==null) ? string.Empty : (string) ViewState["builtInStateScript"];
			}		
			set {
				ViewState["builtInStateScript"] = value;
			}
		}
		internal string builtInEnabledScript {
			get {
				object o = ViewState["builtInEnabledScript"];
				return (o==null) ? string.Empty : (string) ViewState["builtInEnabledScript"];
			}		
			set {
				ViewState["builtInEnabledScript"] = value;
			}
		}
		internal bool isBuiltIn {
			get {
				object o = ViewState["isBuiltIn"];
				return (o==null) ? false : (bool) ViewState["isBuiltIn"];
			}		
			set {
				ViewState["isBuiltIn"] = value;
			}
		}	
		internal bool isProFeature {
			get {
				object o = ViewState["isProFeature"];
				return (o==null) ? false : (bool) ViewState["isProFeature"];
			}		
			set {
				ViewState["isProFeature"] = value;
			}
		}

		/// <summary>
		/// Gets whether the Title property has been manually set
		/// </summary>
		internal bool TitleHasBeenSet {
			get {
				object o = ViewState["TitleHasBeenSet"];
				return (o==null) ? false : (bool) ViewState["TitleHasBeenSet"];
			}		
		}
		internal void SetTitleLanguage(string title) {
			ViewState["Title"] = title; 
			if (ClientID == "") ClientID = title.Replace(" ","");
		}
		#endregion

		#region Public Properties

		
		/// <summary>
		/// Gets whether the ToolbarItem requires a Pro license
		/// </summary>
		public bool IsProFeature {
			get { return isProFeature; }
		}
		/// <summary>
		/// Gets whether the ToolbarItem is built into the FreeTextBox namespace
		/// </summary>
		public bool IsBuiltIn {
			get { return isBuiltIn; }
		}
		[Browsable(false)]
		/// <summary>
		/// Gets the script run when the ToolbarItem is used
		/// </summary>
		public string BuiltInScript{
			get { return builtInScript; }
		}
		[Browsable(false)]
		/// <summary>
		/// Gets the script run to determine the client state of the button
		/// </summary>
		public string BuiltInStateScript{
			get { return builtInStateScript; }
		}
		[Browsable(false)]
		/// <summary>
		/// Gets the script run to determine if the ToolbarItem should be active on the client
		/// </summary>
		public string BuiltInEnabledScript{
			get { return builtInEnabledScript; }
		}
		/// <summary>
		/// Gets or sets the string used to identify the ToolbarItem for Toolbar config updates
		/// </summary>
		internal string ClientID {
			get {
				object o = ViewState["ClientID"];
				return (o==null) ? String.Empty : (string) ViewState["ClientID"];
			}
			set { ViewState["ClientID"] = value; }
		}
	
		/// <summary>
		/// Gets or sets the alt tile for buttons and first item of a dropdownlist
		/// </summary>
		[
		Category("Behavior"),
		Description("The alt text for the ToolbarButton"),
		NotifyParentProperty(true)
		]
		public string Title {
			get {
				object o = ViewState["Title"];
				return (o==null) ? String.Empty : (string) ViewState["Title"];
			}
			set { 
				ViewState["Title"] = value; 
				if (ClientID == "") ClientID = value.Replace(" ","");

				// make sure we know that the title has been manually set so that the 
				// resource language is NOT used
				ViewState["TitleHasBeenSet"] = true;
				System.Web.HttpContext.Current.Trace.Write("Title","set: " + value);
			}
		}
		/// <summary>
		/// Gets or sets the command identifier used for dynamic updating through the queryCommandState method
		/// </summary>		
		[
		Category("Behavior"),
		DefaultValue(""),
		Description("The command identifier used for dynamic updating through the queryCommandState method"),
		NotifyParentProperty(true)
		]
		public string CommandIdentifier {
			get {
				object o = ViewState["CommandIdentifier"];
				return (o==null) ? String.Empty : (string) ViewState["CommandIdentifier"];
			}
			set { ViewState["CommandIdentifier"] = value; }
		}
		/// <summary>
		/// Gets or sets the JavaScript code for the function
		/// </summary>					
		[
		Category("Behavior"),
		DefaultValue(""),
		Description("The JavaScript code for the function"),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerProperty),
		TypeConverter(typeof(string))
		]
		public string ScriptBlock {
			get {
				object o = ViewState["ScriptBlock"];
				return (o==null) ? String.Empty: (string) ViewState["ScriptBlock"];
			}
			set { ViewState["ScriptBlock"] = value; }
		}
		/// <summary>
		/// Gets or sets the JavaScript code to determine the state of the button or list
		/// </summary>					
		[
		Category("Behavior"),
		DefaultValue(""),
		Description("The JavaScript code to determine the state of the button or list"),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerProperty),
		TypeConverter(typeof(string))
		]
		public string StateScriptBlock {
			get {
				object o = ViewState["StateScriptBlock"];
				return (o==null) ? String.Empty : (string) ViewState["StateScriptBlock"];
			}
			set { ViewState["StateScriptBlock"] = value; }
		}

		/// <summary>
		/// Gets or sets the JavaScript code to determine if the button or list is enabled
		/// </summary>					
		[
		Category("Behavior"),
		DefaultValue(""),
		Description("The JavaScript code to determine if the button or list is enabled"),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerProperty),
		TypeConverter(typeof(string))
		]
		public string EnabledScriptBlock {
			get {
				object o = ViewState["EnabledScriptBlock"];
				return (o==null) ? String.Empty : (string) ViewState["EnabledScriptBlock"];
			}
			set { ViewState["EnabledScriptBlock"] = value; }
		}

		internal string ID {
			get { return Title.Replace(" ",""); }
		}

		#endregion

		#region ViewState
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		protected StateBag ViewState {
			get {
				if (g_viewState == null) {
					g_viewState = new StateBag(false);
					if (g_isTrackingViewState) {
						((IStateManager)g_viewState).TrackViewState();
					}
				}
				return g_viewState;
			}
		}
		internal void SetDirty() {
			if (g_viewState != null) {
				ICollection Keys = g_viewState.Keys;
				foreach (string key in Keys) {
					g_viewState.SetItemDirty(key, true);
				}
			}
			
		}
		#endregion
		
		#region IStateManger implemenation
		bool IStateManager.IsTrackingViewState {
			get {
				return g_isTrackingViewState;
			}
		}

		void IStateManager.LoadViewState(object savedState) {
			if (savedState != null) {
				((IStateManager)g_viewState).LoadViewState(savedState);
			}
		}

		object IStateManager.SaveViewState() {
			if (g_viewState != null) {
				return ((IStateManager)g_viewState).SaveViewState();
			}
			return null;
		}
		
		void IStateManager.TrackViewState() {
			
			g_isTrackingViewState = true;

			if (g_viewState != null) {
				((IStateManager)g_viewState).TrackViewState();
			}
		}
		#endregion

	}
}

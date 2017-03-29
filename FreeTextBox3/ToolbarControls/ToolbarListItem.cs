using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing;

namespace FreeTextBoxControls {
	/// <summary>
	/// ListItem replacement for FTB dropdownlists
	/// </summary>
	[
	TypeConverter(typeof(ExpandableObjectConverter))
	]
	public class ToolbarListItem : IStateManager, IParserAccessor {
		
		#region Contructors
		public ToolbarListItem() : this("") {
		}
		public ToolbarListItem(string text) {
			this.Text = text;
			this.Value = text;
		}
		public ToolbarListItem(string text, string value) {
			this.Text = text;
			this.Value = value;
		}
		public ToolbarListItem(string text, string value, Color color) {
			this.Text = text;
			this.Value = value;
			this.BackColor = color;
		}
		public ToolbarListItem(string text, Color color) {
			this.Text = text;
			this.Value = text;
			this.BackColor = color;
		}
		#endregion

		#region Private Properties
		private bool isTrackingViewState;
		private StateBag viewState;
		#endregion
		
		#region Public Properties
		/// <summary>
		/// Gets or sets the value property of the DropDownList option.
		/// </summary>
		[
		Category("Behavior"),
		Description("The value used in the JavaScript function"),
		NotifyParentProperty(true)
		]
		public string Value {
			get {
				object o = ViewState["Value"];
				return (o==null) ? "" : (string) o;
			}
			set { ViewState["Value"] = value; }
		}
		/// <summary>
		/// Gets or sets the the background color of the DropDownList option
		/// </summary>	
		[
		Category("Behavior"),
		Description("The background color of the dropdownlistitem"),
		NotifyParentProperty(true)
		]
		public Color BackColor {
			get {
				object o = ViewState["BackColor"];
				return (o==null) ? Color.Transparent : (Color) o;
			}
			set { ViewState["BackColor"] = value; }
		}
		
		/// <summary>
		/// Gets or sets the text property of the DropDownList
		/// </summary>	
		[
		Category("Behavior"),
		DefaultValue(""),
		Description("The display text for the item"),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)
		]
		public string Text {
			get {
				object o = ViewState["Text"];
				return (o==null) ? "" : (string) o;
			}
			set { ViewState["Text"] = value; }
		}
		#endregion
		
		#region IParserAccessor
		/// <remarks>
		/// Use text between ToolbarItem tags as the ScriptBlock property
		/// </remarks>
		void IParserAccessor.AddParsedSubObject(object obj) {
			if(obj is LiteralControl) {
				Text = ((LiteralControl)obj).Text;
				return;
			}
		}
		#endregion

		#region ViewState
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		protected StateBag ViewState {
			get {
				if (viewState == null) {
					viewState = new StateBag(false);
					if (isTrackingViewState) {
						((IStateManager)viewState).TrackViewState();
					}
				}
				return viewState;
			}
		}
		internal void SetDirty() {
			if (viewState != null) {
				ICollection Keys = viewState.Keys;
				foreach (string key in Keys) {
					viewState.SetItemDirty(key, true);
				}
			}
		}
		#endregion
		
		#region IStateManger implemenation
		bool IStateManager.IsTrackingViewState {
			get {
				return isTrackingViewState;
			}
		}

		void IStateManager.LoadViewState(object savedState) {
			if (savedState != null) {
				((IStateManager)viewState).LoadViewState(savedState);
			}
		}

		object IStateManager.SaveViewState() {
			if (viewState != null) {
				return ((IStateManager)viewState).SaveViewState();
			}
			return null;
		}
		
		void IStateManager.TrackViewState() {
			
			isTrackingViewState = true;

			if (viewState != null) {
				((IStateManager)viewState).TrackViewState();
			}
		}
		#endregion
	}
}

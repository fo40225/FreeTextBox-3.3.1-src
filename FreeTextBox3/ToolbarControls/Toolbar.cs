using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Text;

using FreeTextBoxControls.Support;

namespace FreeTextBoxControls {
	/// <summary>
	/// Encapsulates all the style information for ToolbarButtons
	/// </summary>
	[
	DefaultProperty("Items"),
	ParseChildren(true,"Items"),
	PersistChildren(true)
	]
	public class Toolbar : IStateManager {
		public Toolbar(){
			viewState = new StateBag();
		}
		
		#region Private Properties
		// view state properties
		private bool isTrackingViewState;
		private StateBag viewState;

		// collection properties
		private ToolbarItemCollection items;
		private ToolbarButtonStyle buttonStyle;

		#endregion 

		#region Public Properties
		/// <summary>
		/// Gets or sets the repeat direction of <seealso cref="ToolbarItem"/>s in the Toolbar
		/// </summary>
		[
		Category("Behavior"),
		Description("The direction the Toolbar will draw the ToolbarItems"),
		NotifyParentProperty(true)
		]
		public RepeatDirection RepeatDirection {
			get {
				object o = ViewState["RepeatDirection"];
				return (o==null) ? RepeatDirection.Horizontal : (RepeatDirection) ViewState["RepeatDirection"];
			}
			set {
				ViewState["RepeatDirection"] = value;
			}
		}
	
		#region ViewState Enabled Properties
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerDefaultProperty)
		]
		public ToolbarItemCollection Items {
			get {
				if (items == null) {
					items = new ToolbarItemCollection();
					if (this.isTrackingViewState) {
						((IStateManager)items).TrackViewState();
					}
				}
				return items;
			}
		}
		/// <summary>
		/// Style parameters for ToolbarButtons. 
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.InnerProperty),
		NotifyParentProperty(true),
		CategoryAttribute("Appearance")
		]
		public virtual ToolbarButtonStyle ButtonStyle {
			get {
				if (buttonStyle == null) {
					buttonStyle = new ToolbarButtonStyle();
					if (isTrackingViewState)
						((IStateManager)buttonStyle).TrackViewState();
				}
				return buttonStyle;
			}
		}
		#endregion 

		#endregion

		#region State Management
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
#if DEBUG
			System.Web.HttpContext.Current.Trace.Write("Toolbar:Dirty","setting");
#endif
			if (viewState != null) {
				ICollection Keys = viewState.Keys;
				foreach (string key in Keys) {
					viewState.SetItemDirty(key, true);
				}
#if DEBUG
				System.Web.HttpContext.Current.Trace.Write("Toolbar:Dirty","dirty");
#endif
			}
		}
		bool IStateManager.IsTrackingViewState {
			get {
				return isTrackingViewState;
			}
		}

		void IStateManager.LoadViewState(object savedState) {
#if DEBUG			
			System.Web.HttpContext.Current.Trace.Write("Toolbar","Loading ViewState");
#endif			
			
			object[] statesArray = null;
			object baseState = null;
			object itemsState = null;

			if (savedState != null) {
				statesArray = (object[]) savedState;
				// currently there is are 2 things to keep track of:
				// 1) this, 2) Items
				if (statesArray.Length != 2) throw new ArgumentException("Invalid view state");
				
				baseState = statesArray[0];
				itemsState = statesArray[1];
			}
			// load this state
			if (baseState != null) {
				((IStateManager)viewState).LoadViewState(baseState);
			}

			// load Items state
			if (itemsState != null) {
				((IStateManager)Items).LoadViewState(itemsState);
			}

#if DEBUG			
			System.Web.HttpContext.Current.Trace.Write("Toolbar items?",((itemsState == null) ? "null" : "got something"));
#endif	
		}
		object IStateManager.SaveViewState() {
#if DEBUG
			System.Web.HttpContext.Current.Trace.Write("Toolbar","Saving ViewState");
#endif	
			object baseState = ((IStateManager)viewState).SaveViewState();
			object itemsState = null;

			if (items != null) {
				itemsState = ((IStateManager)items).SaveViewState();
			}
#if DEBUG
			System.Web.HttpContext.Current.Trace.Write("Toolbar:SavingItems",((itemsState == null) ? "null" : "got something"));
#endif	

			if ((baseState != null) || (itemsState != null)) {
				object[] savedState = new object[2];
				savedState[0] = baseState;
				savedState[1] = itemsState;

				return savedState;
			}
			return null;			
		}
		
		void IStateManager.TrackViewState() {
			((IStateManager)ViewState).TrackViewState();
			if (items != null)
				((IStateManager)items).TrackViewState();

		}

		#endregion


	}
}

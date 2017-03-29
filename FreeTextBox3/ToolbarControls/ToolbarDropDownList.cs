using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using FreeTextBoxControls.Support;

namespace FreeTextBoxControls {
	/// <summary>
	/// A DropDownList in a <seealso cref="Toolbar"/> in a <seealso cref="FreeTextBox"/>.
	/// </summary>
	public class ToolbarDropDownList : ToolbarItem, IParserAccessor, IStateManager {
		#region Contructors
		public ToolbarDropDownList() : this("","") {
		}
		public ToolbarDropDownList(string title) {
			this.SetTitleLanguage(title);
			//this.Title = title;
		}
		public ToolbarDropDownList(string title, string nothing) {
			this.SetTitleLanguage(title);
			//this.Title = title;
		}
		#endregion	

		#region Private Properties
		private ToolbarDropDownListItemCollection items;
		#endregion

		#region Public Properties
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerDefaultProperty)
		]
		public ToolbarDropDownListItemCollection Items {
			get {
				if (items == null) {
					items = new ToolbarDropDownListItemCollection();
					if (this.g_isTrackingViewState) {
						((IStateManager)items).TrackViewState();
					}
				}
				return items;
			}
		}

		#endregion	

		#region State Management

		void IStateManager.TrackViewState() {
            ((IStateManager)this.g_viewState).TrackViewState();
			if (items != null) {
				((IStateManager)items).TrackViewState();
			}
		}
	
		object IStateManager.SaveViewState() {
			object[] state = new object[2];
		
			state[0] = ((IStateManager) g_viewState).SaveViewState();
			if (this.items != null) 
				state[1] = ((IStateManager) this.items).SaveViewState();
#if DEBUG
			System.Web.HttpContext.Current.Trace.Write("ToolbarDDL","Save ViewState:" + ((state[1]==null) ? "null":"good"));
#endif
			return state;
		}

		void IStateManager.LoadViewState(object savedState) {
	
			object[] state = null;

			if (savedState != null) {
				state = (object[]) savedState;

                ((IStateManager)g_viewState).LoadViewState(state[0]);
				if (state[1] != null) 
					((IStateManager) this.Items).LoadViewState(state[1]);

#if DEBUG
				System.Web.HttpContext.Current.Trace.Write("ToolbarDDL","Loading ViewState:" + ((state[1]==null) ? "null":"good"));
#endif
			}
		}
	
		#endregion

		#region IParserAccessor
		public void AddParsedSubObject(object obj) {
			if(obj is ToolbarListItem) {
				Items.Add((ToolbarListItem)obj);
				return;
			}

		}
		#endregion
	}
}

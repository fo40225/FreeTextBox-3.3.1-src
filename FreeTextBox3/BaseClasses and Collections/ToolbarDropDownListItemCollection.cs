using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;

namespace FreeTextBoxControls.Support {
	/// <exclude />
	public class ToolbarDropDownListItemCollection : ICollection, IStateManager {
		
		#region Private Properties
		private ArrayList dropDownListItems;
		private bool isTrackingViewState;
		private bool saveAll;
		#endregion
		
		internal ToolbarDropDownListItemCollection() {
			dropDownListItems = new ArrayList();
		}
		
		public ToolbarListItem this[int index] {
			get { return (ToolbarListItem) dropDownListItems[index];}
		}

		#region List Methods
		public int Add(ToolbarListItem item) {
			if (item == null) throw new ArgumentNullException("item");

			return dropDownListItems.Add(item);
		}
		public bool Contains(ToolbarListItem item) {
			if (item == null) {
				return false;
			}
			return dropDownListItems.Contains(item);
		}

		public int IndexOf(ToolbarListItem item) {
			if (item == null) {
				throw new ArgumentNullException("item");
			}
			return dropDownListItems.IndexOf(item);
		}
		public void Clear() {
			dropDownListItems.Clear();
		}
		public void Insert(int index, ToolbarListItem item) {
			if (item == null) throw new ArgumentNullException("item");
			
			dropDownListItems.Insert(index,item);
		}
		public void Remove(ToolbarListItem item) {
			if (item == null) throw new ArgumentNullException("item");

			dropDownListItems.Remove(item);
		}
		public void RemoveAt(int index) {
			dropDownListItems.RemoveAt(index);
		}
		#endregion

		#region IEnumerable Implementation
		public IEnumerator GetEnumerator() {
			return dropDownListItems.GetEnumerator();
		}
		#endregion IEnumerable Implementation

		#region ICollection Implementation
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int Count {
			get {
				return dropDownListItems.Count;
			}
		}

		public void CopyTo(Array array, int index) {
			dropDownListItems.CopyTo(array,index);
		}

		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool IsSynchronized {
			get {
				return false;
			}
		}

		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public object SyncRoot {
			get {
				return this;
			}
		}
		#endregion ICollection Implementation
   
		#region IStateManager Implimentation
		bool IStateManager.IsTrackingViewState {
			get { return this.isTrackingViewState; }
		}

		void IStateManager.TrackViewState() {
			isTrackingViewState = true;
			foreach (ToolbarListItem toolbarListItem in dropDownListItems) {
				((IStateManager)toolbarListItem).TrackViewState();
			}
		}

		object IStateManager.SaveViewState() {
#if DEBUG
			System.Web.HttpContext.Current.Trace.Write("DDLList","Saving:" +  this.dropDownListItems.Count.ToString());
#endif

			bool isAllNulls = true;	
			object [] state = new object[this.dropDownListItems.Count];
			for (int i = 0; i < this.dropDownListItems.Count; i++) {
				// Save each item's viewstate...
				((ToolbarListItem)this.dropDownListItems[i]).SetDirty();
				state[i] = ((IStateManager) this.dropDownListItems[i]).SaveViewState();
				if (state[i] != null)
					isAllNulls = false;
			}
#if DEBUG
			System.Web.HttpContext.Current.Trace.Write("DDLList","isAllNulls:" + isAllNulls.ToString());
#endif
			// If all items returned null, simply return a null rather than the object array
			if (isAllNulls)
				return null;
			else
				return state;
		}

		void IStateManager.LoadViewState(object savedState) {
			if (savedState != null) {
				object [] state = (object[]) savedState;

				// Create an ArrayList of the precise size
				dropDownListItems = new ArrayList(state.Length);

				for (int i = 0; i < state.Length; i++) {
					ToolbarListItem toolbarListItem  = new ToolbarListItem();		// create MenuItem
					((IStateManager) toolbarListItem ).TrackViewState();	// Indicate that it needs to track its view state

					// Add the MenuItem to the collection
					dropDownListItems.Add(toolbarListItem  );

					if (state[i] != null) {
						// Load its state via LoadViewState()
						((IStateManager) dropDownListItems[i]).LoadViewState(state[i]);
					}
				}
			}
		}
		#endregion
	}
}

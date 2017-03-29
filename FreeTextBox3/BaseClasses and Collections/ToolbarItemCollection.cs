using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;

namespace FreeTextBoxControls.Support {
	/// <exclude />
	public class ToolbarItemCollection : ICollection, IStateManager {
		
		private ArrayList toolbarItems;
		private bool isTrackingViewState;
		private bool saveAll;
		
		internal ToolbarItemCollection() {
			toolbarItems = new ArrayList();
		}
		
		public ToolbarItem this[int index] {
			get { return (ToolbarItem) toolbarItems[index];}
		}

		#region Add/Remove Functions

		public int Add(ToolbarItem item) {
			if (item == null) 
				throw new ArgumentNullException("item");

			return toolbarItems.Add(item);
		}
		public bool Contains(ToolbarItem item) {
			if (item == null) return false;

			return toolbarItems.Contains(item);
		}

		public int IndexOf(ToolbarItem item) {
			if (item == null)
				throw new ArgumentNullException("item");

			return toolbarItems.IndexOf(item);
		}
		public void Clear() {
			toolbarItems.Clear();
		}
		public void Insert(int index, ToolbarItem item) {
			if (item == null) throw new ArgumentNullException("item");
			
			toolbarItems.Insert(index,item);
		}
		public void Remove(ToolbarItem item) {
			if (item == null) throw new ArgumentNullException("item");
			toolbarItems.Remove(item);
		}
		public void RemoveAt(int index) {
			toolbarItems.RemoveAt(index);
		}
		#endregion

		#region IEnumerable Implementation
		public IEnumerator GetEnumerator() {
			return toolbarItems.GetEnumerator();
		}
		#endregion IEnumerable Implementation

		#region ICollection Implementation
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int Count {
			get {
				return toolbarItems.Count;
			}
		}

		public void CopyTo(Array array, int index) {
			toolbarItems.CopyTo(array,index);
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
			foreach (ToolbarItem toolbarItem in toolbarItems) {
				((IStateManager)toolbarItem).TrackViewState();
			}
		}

		object IStateManager.SaveViewState() {
#if DEBUG	
			System.Web.HttpContext.Current.Trace.Write("ToolbarItem Collection","Saving ViewState");
#endif
			
			bool isAllNulls = true;
	
			ArrayList types = new ArrayList(this.Count);
			ArrayList states = new ArrayList(this.Count);
				
			for (int i=0; i<this.Count; i++) {
				ToolbarItem toolbarItem = (ToolbarItem) toolbarItems[i];
				toolbarItem.SetDirty();
					
				if (toolbarItem is ToolbarButton) {
					types.Add('b');
				} else if (toolbarItem is ToolbarDropDownList) {
					types.Add('d');
				} else {
					types.Add('s');
				}
				object state = ((IStateManager)toolbarItem).SaveViewState();

				states.Add(state);

				if (state != null) 
					isAllNulls = false;
			}

			// If all items returned null, simply return a null rather than the object array
			if (isAllNulls)
				return null;
			else
				return new Pair(types,states);

		}

		void IStateManager.LoadViewState(object savedState) {
#if DEBUG	
			System.Web.HttpContext.Current.Trace.Write("ToolbarItem Collection","Loading ViewState");
#endif
			if (savedState == null) return;

			if (savedState is Pair) {
				// all items were saved
				// create a new collection using view state
				saveAll = true;
				Pair p = (Pair)savedState;
				ArrayList types = (ArrayList) p.First;
				ArrayList states = (ArrayList) p.Second;
				int count = types.Count;

				toolbarItems = new ArrayList(count);

				for (int i=0; i<count; i++) {
					ToolbarItem toolbarItem = null;
					if (((char)types[i]).Equals('b')) {
						toolbarItem = new ToolbarButton();
					} else if (((char)types[i]).Equals('d')) {
						toolbarItem = new ToolbarDropDownList();
					} else if (((char)types[i]).Equals('s')) {
						toolbarItem = new ToolbarSeparator();
					}
					Add(toolbarItem);
					((IStateManager)toolbarItem).LoadViewState(states[i]);
				}
			}
		}
		#endregion
	}
}

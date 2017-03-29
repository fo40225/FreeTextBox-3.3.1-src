using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;

namespace FreeTextBoxControls.Support {
	/// <exclude />
	public class ToolbarCollection : ICollection, IStateManager {
		
		#region Private Properties
		private ArrayList toolbars;
		private bool isTrackingViewState;
		private bool saveAll;
		#endregion
		
		internal ToolbarCollection() {
			toolbars = new ArrayList();
		}
		
		public Toolbar this[int index] {
			get { return (Toolbar) toolbars[index];}
		}

		#region IList Implementation

		public int Add(Toolbar item) {
			if (item == null) 
				throw new ArgumentNullException("item");

			return toolbars.Add(item);
		}
		public bool Contains(Toolbar item) {
			if (item == null) {
				return false;
			}
			return toolbars.Contains(item);
		}

		public int IndexOf(Toolbar item) {
			if (item == null) {
				throw new ArgumentNullException("item");
			}
			return toolbars.IndexOf(item);
		}
		public void Clear() {
			toolbars.Clear();
		}
		public void Insert(int index, Toolbar item) {
			if (item == null) 
				throw new ArgumentNullException("item");
			
			toolbars.Insert(index,item);
		}
		public void Remove(Toolbar item) {
			if (item == null) 
				throw new ArgumentNullException("item");

			toolbars.Remove(item);
		}
		public void RemoveAt(int index) {
			toolbars.RemoveAt(index);
		}
		#endregion

		#region IEnumerable Implementation
		public IEnumerator GetEnumerator() {
			return toolbars.GetEnumerator();
		}
		#endregion IEnumerable Implementation

		#region ICollection Implementation
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int Count {
			get {
				return toolbars.Count;
			}
		}

		public void CopyTo(Array array, int index) {
			toolbars.CopyTo(array,index);
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
			foreach (Toolbar Toolbar in toolbars) {
				((IStateManager)Toolbar).TrackViewState();
			}
		}

		object IStateManager.SaveViewState() {
#if DEBUG
			System.Web.HttpContext.Current.Trace.Write("ToolbarCollection","Save");
#endif	
			bool isAllNulls = true;
			object [] state = new object[this.toolbars.Count];
			for (int i = 0; i < this.toolbars.Count; i++) {
				// Save each item's viewstate...
				state[i] = ((IStateManager) this.toolbars[i]).SaveViewState();
				if (state[i] != null)
					isAllNulls = false;
			}

			// If all items returned null, simply return a null rather than the object array
			if (isAllNulls)
				return null;
			else
				return state;
		}

		void IStateManager.LoadViewState(object savedState) {
#if DEBUG
			System.Web.HttpContext.Current.Trace.Write("ToolbarCollection","Loading ViewState");
#endif
			if (savedState != null) {
				object [] state = (object[]) savedState;

				// Create an ArrayList of the precise size
				toolbars = new ArrayList(state.Length);

				for (int i = 0; i < state.Length; i++) {
					Toolbar toolbar = new Toolbar();		// create Toolbar
					((IStateManager) toolbar).TrackViewState();	// Indicate that it needs to track its view state

					// Add the MenuItem to the collection
					toolbars.Add(toolbar);

					if (state[i] != null) {
						// Load its state via LoadViewState()
						((IStateManager) toolbars[i]).LoadViewState(state[i]);
					}
				}
			}
		}
		#endregion
	}
}

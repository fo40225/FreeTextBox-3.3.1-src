using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
namespace FreeTextBoxControls.Support
{
	public class ToolbarDropDownListItemCollection : ICollection, IEnumerable, IStateManager
	{
		private ArrayList dropDownListItems;
		private bool isTrackingViewState;
		private bool saveAll;
		public ToolbarListItem this[int index]
		{
			get
			{
				return (ToolbarListItem)this.dropDownListItems[index];
			}
		}
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Count
		{
			get
			{
				return this.dropDownListItems.Count;
			}
		}
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this.isTrackingViewState;
			}
		}
		internal ToolbarDropDownListItemCollection()
		{
			this.dropDownListItems = new ArrayList();
		}
		public int Add(ToolbarListItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.dropDownListItems.Add(item);
		}
		public bool Contains(ToolbarListItem item)
		{
			return item != null && this.dropDownListItems.Contains(item);
		}
		public int IndexOf(ToolbarListItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.dropDownListItems.IndexOf(item);
		}
		public void Clear()
		{
			this.dropDownListItems.Clear();
		}
		public void Insert(int index, ToolbarListItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.dropDownListItems.Insert(index, item);
		}
		public void Remove(ToolbarListItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.dropDownListItems.Remove(item);
		}
		public void RemoveAt(int index)
		{
			this.dropDownListItems.RemoveAt(index);
		}
		public IEnumerator GetEnumerator()
		{
			return this.dropDownListItems.GetEnumerator();
		}
		public void CopyTo(Array array, int index)
		{
			this.dropDownListItems.CopyTo(array, index);
		}
		void IStateManager.TrackViewState()
		{
			this.isTrackingViewState = true;
			foreach (ToolbarListItem toolbarListItem in this.dropDownListItems)
			{
				((IStateManager)toolbarListItem).TrackViewState();
			}
		}
		object IStateManager.SaveViewState()
		{
			bool flag = true;
			object[] array = new object[this.dropDownListItems.Count];
			for (int i = 0; i < this.dropDownListItems.Count; i++)
			{
				((ToolbarListItem)this.dropDownListItems[i]).SetDirty();
				array[i] = ((IStateManager)this.dropDownListItems[i]).SaveViewState();
				if (array[i] != null)
				{
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return array;
		}
		void IStateManager.LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] array = (object[])savedState;
				this.dropDownListItems = new ArrayList(array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					ToolbarListItem toolbarListItem = new ToolbarListItem();
					((IStateManager)toolbarListItem).TrackViewState();
					this.dropDownListItems.Add(toolbarListItem);
					if (array[i] != null)
					{
						((IStateManager)this.dropDownListItems[i]).LoadViewState(array[i]);
					}
				}
			}
		}
	}
}

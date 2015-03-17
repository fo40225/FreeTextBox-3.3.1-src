using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
namespace FreeTextBoxControls.Support
{
	public class ToolbarItemCollection : ICollection, IEnumerable, IStateManager
	{
		private ArrayList toolbarItems;
		private bool isTrackingViewState;
		private bool saveAll;
		public ToolbarItem this[int index]
		{
			get
			{
				return (ToolbarItem)this.toolbarItems[index];
			}
		}
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Count
		{
			get
			{
				return this.toolbarItems.Count;
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
		internal ToolbarItemCollection()
		{
			this.toolbarItems = new ArrayList();
		}
		public int Add(ToolbarItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.toolbarItems.Add(item);
		}
		public bool Contains(ToolbarItem item)
		{
			return item != null && this.toolbarItems.Contains(item);
		}
		public int IndexOf(ToolbarItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.toolbarItems.IndexOf(item);
		}
		public void Clear()
		{
			this.toolbarItems.Clear();
		}
		public void Insert(int index, ToolbarItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.toolbarItems.Insert(index, item);
		}
		public void Remove(ToolbarItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.toolbarItems.Remove(item);
		}
		public void RemoveAt(int index)
		{
			this.toolbarItems.RemoveAt(index);
		}
		public IEnumerator GetEnumerator()
		{
			return this.toolbarItems.GetEnumerator();
		}
		public void CopyTo(Array array, int index)
		{
			this.toolbarItems.CopyTo(array, index);
		}
		void IStateManager.TrackViewState()
		{
			this.isTrackingViewState = true;
			foreach (ToolbarItem toolbarItem in this.toolbarItems)
			{
				((IStateManager)toolbarItem).TrackViewState();
			}
		}
		object IStateManager.SaveViewState()
		{
			bool flag = true;
			ArrayList arrayList = new ArrayList(this.Count);
			ArrayList arrayList2 = new ArrayList(this.Count);
			for (int i = 0; i < this.Count; i++)
			{
				ToolbarItem toolbarItem = (ToolbarItem)this.toolbarItems[i];
				toolbarItem.SetDirty();
				if (toolbarItem is ToolbarButton)
				{
					arrayList.Add('b');
				}
				else
				{
					if (toolbarItem is ToolbarDropDownList)
					{
						arrayList.Add('d');
					}
					else
					{
						arrayList.Add('s');
					}
				}
				object obj = ((IStateManager)toolbarItem).SaveViewState();
				arrayList2.Add(obj);
				if (obj != null)
				{
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new Pair(arrayList, arrayList2);
		}
		void IStateManager.LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				return;
			}
			if (savedState is Pair)
			{
				this.saveAll = true;
				Pair pair = (Pair)savedState;
				ArrayList arrayList = (ArrayList)pair.First;
				ArrayList arrayList2 = (ArrayList)pair.Second;
				int count = arrayList.Count;
				this.toolbarItems = new ArrayList(count);
				for (int i = 0; i < count; i++)
				{
					ToolbarItem toolbarItem = null;
					if (((char)arrayList[i]).Equals('b'))
					{
						toolbarItem = new ToolbarButton();
					}
					else
					{
						if (((char)arrayList[i]).Equals('d'))
						{
							toolbarItem = new ToolbarDropDownList();
						}
						else
						{
							if (((char)arrayList[i]).Equals('s'))
							{
								toolbarItem = new ToolbarSeparator();
							}
						}
					}
					this.Add(toolbarItem);
					((IStateManager)toolbarItem).LoadViewState(arrayList2[i]);
				}
			}
		}
	}
}

using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
namespace FreeTextBoxControls.Support
{
	public class ToolbarCollection : ICollection, IEnumerable, IStateManager
	{
		private ArrayList toolbars;
		private bool isTrackingViewState;
		private bool saveAll;
		public Toolbar this[int index]
		{
			get
			{
				return (Toolbar)this.toolbars[index];
			}
		}
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Count
		{
			get
			{
				return this.toolbars.Count;
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
		internal ToolbarCollection()
		{
			this.toolbars = new ArrayList();
		}
		public int Add(Toolbar item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.toolbars.Add(item);
		}
		public bool Contains(Toolbar item)
		{
			return item != null && this.toolbars.Contains(item);
		}
		public int IndexOf(Toolbar item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.toolbars.IndexOf(item);
		}
		public void Clear()
		{
			this.toolbars.Clear();
		}
		public void Insert(int index, Toolbar item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.toolbars.Insert(index, item);
		}
		public void Remove(Toolbar item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.toolbars.Remove(item);
		}
		public void RemoveAt(int index)
		{
			this.toolbars.RemoveAt(index);
		}
		public IEnumerator GetEnumerator()
		{
			return this.toolbars.GetEnumerator();
		}
		public void CopyTo(Array array, int index)
		{
			this.toolbars.CopyTo(array, index);
		}
		void IStateManager.TrackViewState()
		{
			this.isTrackingViewState = true;
			foreach (Toolbar toolbar in this.toolbars)
			{
				((IStateManager)toolbar).TrackViewState();
			}
		}
		object IStateManager.SaveViewState()
		{
			bool flag = true;
			object[] array = new object[this.toolbars.Count];
			for (int i = 0; i < this.toolbars.Count; i++)
			{
				array[i] = ((IStateManager)this.toolbars[i]).SaveViewState();
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
				this.toolbars = new ArrayList(array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					Toolbar toolbar = new Toolbar();
					((IStateManager)toolbar).TrackViewState();
					this.toolbars.Add(toolbar);
					if (array[i] != null)
					{
						((IStateManager)this.toolbars[i]).LoadViewState(array[i]);
					}
				}
			}
		}
	}
}

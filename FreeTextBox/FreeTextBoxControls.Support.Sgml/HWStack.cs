using System;
namespace FreeTextBoxControls.Support.Sgml
{
	internal class HWStack
	{
		private object[] items;
		private int size;
		private int count;
		private int growth;
		public int Count
		{
			get
			{
				return this.count;
			}
			set
			{
				this.count = value;
			}
		}
		public int Size
		{
			get
			{
				return this.size;
			}
		}
		public object this[int i]
		{
			get
			{
				if (i < 0 || i >= this.size)
				{
					return null;
				}
				return this.items[i];
			}
			set
			{
				this.items[i] = value;
			}
		}
		public HWStack(int growth)
		{
			this.growth = growth;
		}
		public object Pop()
		{
			this.count--;
			if (this.count > 0)
			{
				return this.items[this.count - 1];
			}
			return null;
		}
		public object Push()
		{
			if (this.count == this.size)
			{
				int num = this.size + this.growth;
				object[] destinationArray = new object[num];
				if (this.items != null)
				{
					Array.Copy(this.items, destinationArray, this.size);
				}
				this.size = num;
				this.items = destinationArray;
			}
			return this.items[this.count++];
		}
		public void RemoveAt(int i)
		{
			this.items[i] = null;
			Array.Copy(this.items, i + 1, this.items, i, this.count - i - 1);
			this.count--;
		}
	}
}

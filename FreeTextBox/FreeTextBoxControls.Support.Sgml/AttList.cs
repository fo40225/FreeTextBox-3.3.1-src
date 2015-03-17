using System;
using System.Collections;
namespace FreeTextBoxControls.Support.Sgml
{
	public class AttList : IEnumerable
	{
		private Hashtable AttDefs;
		public AttDef this[string name]
		{
			get
			{
				return (AttDef)this.AttDefs[name];
			}
		}
		public AttList()
		{
			this.AttDefs = new Hashtable();
		}
		public void Add(AttDef a)
		{
			this.AttDefs.Add(a.Name, a);
		}
		public IEnumerator GetEnumerator()
		{
			return this.AttDefs.Values.GetEnumerator();
		}
	}
}

using System;
using System.Xml;
namespace FreeTextBoxControls.Support.Sgml
{
	internal class Node
	{
		internal XmlNodeType NodeType;
		internal string Value;
		internal XmlSpace Space;
		internal string XmlLang;
		internal bool IsEmpty;
		internal string Name;
		internal ElementDecl DtdType;
		internal State CurrentState;
		private HWStack attributes = new HWStack(10);
		public int AttributeCount
		{
			get
			{
				return this.attributes.Count;
			}
		}
		public void Reset(string name, XmlNodeType nt, string value)
		{
			this.Value = value;
			this.Name = name;
			this.NodeType = nt;
			this.Space = XmlSpace.None;
			this.XmlLang = null;
			this.IsEmpty = true;
			this.attributes.Count = 0;
			this.DtdType = null;
		}
		public Attribute AddAttribute(string name, string value, char quotechar, bool caseInsensitive)
		{
			int i = 0;
			int count = this.attributes.Count;
			Attribute attribute;
			while (i < count)
			{
				attribute = (Attribute)this.attributes[i];
				if (caseInsensitive && string.Compare(attribute.Name, name, true) == 0)
				{
					return null;
				}
				if (attribute.Name == name)
				{
					return null;
				}
				i++;
			}
			attribute = (Attribute)this.attributes.Push();
			if (attribute == null)
			{
				attribute = new Attribute();
				this.attributes[this.attributes.Count - 1] = attribute;
			}
			attribute.Reset(name, value, quotechar);
			return attribute;
		}
		public void RemoveAttribute(string name)
		{
			int i = 0;
			int count = this.attributes.Count;
			while (i < count)
			{
				Attribute attribute = (Attribute)this.attributes[i];
				if (attribute.Name == name)
				{
					this.attributes.RemoveAt(i);
					return;
				}
				i++;
			}
		}
		public void CopyAttributes(Node n)
		{
			int i = 0;
			int count = n.attributes.Count;
			while (i < count)
			{
				Attribute attribute = (Attribute)n.attributes[i];
				Attribute attribute2 = this.AddAttribute(attribute.Name, attribute.Value, attribute.QuoteChar, false);
				attribute2.DtdType = attribute.DtdType;
				i++;
			}
		}
		public int GetAttribute(string name)
		{
			int i = 0;
			int count = this.attributes.Count;
			while (i < count)
			{
				Attribute attribute = (Attribute)this.attributes[i];
				if (attribute.Name == name)
				{
					return i;
				}
				i++;
			}
			return -1;
		}
		public Attribute GetAttribute(int i)
		{
			if (i >= 0 && i < this.attributes.Count)
			{
				return (Attribute)this.attributes[i];
			}
			return null;
		}
	}
}

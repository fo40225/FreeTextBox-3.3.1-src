using System;
namespace FreeTextBoxControls.Support.Sgml
{
	public class ElementDecl
	{
		public string Name;
		public bool StartTagOptional;
		public bool EndTagOptional;
		public ContentModel ContentModel;
		public string[] Inclusions;
		public string[] Exclusions;
		public AttList AttList;
		public ElementDecl(string name, bool sto, bool eto, ContentModel cm, string[] inclusions, string[] exclusions)
		{
			this.Name = name;
			this.StartTagOptional = sto;
			this.EndTagOptional = eto;
			this.ContentModel = cm;
			this.Inclusions = inclusions;
			this.Exclusions = exclusions;
		}
		public AttDef FindAttribute(string name)
		{
			return this.AttList[name.ToUpper()];
		}
		public void AddAttDefs(AttList list)
		{
			if (this.AttList == null)
			{
				this.AttList = list;
				return;
			}
			foreach (AttDef attDef in list)
			{
				if (this.AttList[attDef.Name] == null)
				{
					this.AttList.Add(attDef);
				}
			}
		}
		public bool CanContain(string name, SgmlDtd dtd)
		{
			if (this.Exclusions != null)
			{
				string[] exclusions = this.Exclusions;
				for (int i = 0; i < exclusions.Length; i++)
				{
					string text = exclusions[i];
					if (text == name)
					{
						bool result = false;
						return result;
					}
				}
			}
			if (this.Inclusions != null)
			{
				string[] inclusions = this.Inclusions;
				for (int j = 0; j < inclusions.Length; j++)
				{
					string text2 = inclusions[j];
					if (text2 == name)
					{
						bool result = true;
						return result;
					}
				}
			}
			return this.ContentModel.CanContain(name, dtd);
		}
	}
}

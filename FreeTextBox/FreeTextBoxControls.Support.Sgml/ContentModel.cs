using System;
namespace FreeTextBoxControls.Support.Sgml
{
	public class ContentModel
	{
		public DeclaredContent DeclaredContent;
		public int CurrentDepth;
		public Group Model;
		public ContentModel()
		{
			this.Model = new Group(null);
		}
		public void PushGroup()
		{
			this.Model = new Group(this.Model);
			this.CurrentDepth++;
		}
		public int PopGroup()
		{
			if (this.CurrentDepth == 0)
			{
				return -1;
			}
			this.CurrentDepth--;
			this.Model.Parent.AddGroup(this.Model);
			this.Model = this.Model.Parent;
			return this.CurrentDepth;
		}
		public void AddSymbol(string sym)
		{
			this.Model.AddSymbol(sym);
		}
		public void AddConnector(char c)
		{
			this.Model.AddConnector(c);
		}
		public void AddOccurrence(char c)
		{
			this.Model.AddOccurrence(c);
		}
		public void SetDeclaredContent(string dc)
		{
			if (dc != null)
			{
				if (dc == "EMPTY")
				{
					this.DeclaredContent = DeclaredContent.EMPTY;
					return;
				}
				if (dc == "RCDATA")
				{
					this.DeclaredContent = DeclaredContent.RCDATA;
					return;
				}
				if (dc == "CDATA")
				{
					this.DeclaredContent = DeclaredContent.CDATA;
					return;
				}
			}
			throw new Exception(string.Format("Declared content type '{0}' is not supported", dc));
		}
		public bool CanContain(string name, SgmlDtd dtd)
		{
			return this.DeclaredContent == DeclaredContent.Default && this.Model.CanContain(name, dtd);
		}
	}
}

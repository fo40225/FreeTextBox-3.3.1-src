using System;
using System.Collections;
namespace FreeTextBoxControls.Support.Sgml
{
	public class Group
	{
		public Group Parent;
		public ArrayList Members;
		public GroupType GroupType;
		public Occurrence Occurrence;
		public bool Mixed;
		public bool TextOnly
		{
			get
			{
				return this.Mixed && this.Members.Count == 0;
			}
		}
		public Group(Group parent)
		{
			this.Parent = parent;
			this.Members = new ArrayList();
			this.GroupType = GroupType.None;
			this.Occurrence = Occurrence.Required;
		}
		public void AddGroup(Group g)
		{
			this.Members.Add(g);
		}
		public void AddSymbol(string sym)
		{
			if (sym == "#PCDATA")
			{
				this.Mixed = true;
				return;
			}
			this.Members.Add(sym);
		}
		public void AddConnector(char c)
		{
			if (!this.Mixed && this.Members.Count == 0)
			{
				throw new Exception(string.Format("Missing token before connector '{0}'.", c));
			}
			GroupType groupType = GroupType.None;
			if (c != '&')
			{
				if (c != ',')
				{
					if (c == '|')
					{
						groupType = GroupType.Or;
					}
				}
				else
				{
					groupType = GroupType.Sequence;
				}
			}
			else
			{
				groupType = GroupType.And;
			}
			if (this.GroupType != GroupType.None && this.GroupType != groupType)
			{
				throw new Exception(string.Format("Connector '{0}' is inconsistent with {1} group.", c, this.GroupType.ToString()));
			}
			this.GroupType = groupType;
		}
		public void AddOccurrence(char c)
		{
			Occurrence occurrence = Occurrence.Required;
			switch (c)
			{
			case '*':
				occurrence = Occurrence.ZeroOrMore;
				break;
			case '+':
				occurrence = Occurrence.OneOrMore;
				break;
			default:
				if (c == '?')
				{
					occurrence = Occurrence.Optional;
				}
				break;
			}
			this.Occurrence = occurrence;
		}
		public bool CanContain(string name, SgmlDtd dtd)
		{
			foreach (object current in this.Members)
			{
				if (current is string && current == name)
				{
					bool result = true;
					return result;
				}
			}
			foreach (object current2 in this.Members)
			{
				if (current2 is string)
				{
					string name2 = (string)current2;
					ElementDecl elementDecl = dtd.FindElement(name2);
					if (elementDecl != null && elementDecl.StartTagOptional && elementDecl.CanContain(name, dtd))
					{
						bool result = true;
						return result;
					}
				}
				else
				{
					Group group = (Group)current2;
					if (group.CanContain(name, dtd))
					{
						bool result = true;
						return result;
					}
				}
			}
			return false;
		}
	}
}

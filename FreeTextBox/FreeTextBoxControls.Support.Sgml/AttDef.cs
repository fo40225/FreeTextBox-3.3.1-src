using System;
namespace FreeTextBoxControls.Support.Sgml
{
	public class AttDef
	{
		public string Name;
		public AttributeType Type;
		public string[] EnumValues;
		public string Default;
		public AttributePresence Presence;
		public AttDef(string name)
		{
			this.Name = name;
		}
		public void SetType(string type)
		{
			switch (type)
			{
			case "CDATA":
				this.Type = AttributeType.CDATA;
				return;
			case "ENTITY":
				this.Type = AttributeType.ENTITY;
				return;
			case "ENTITIES":
				this.Type = AttributeType.ENTITIES;
				return;
			case "ID":
				this.Type = AttributeType.ID;
				return;
			case "IDREF":
				this.Type = AttributeType.IDREF;
				return;
			case "IDREFS":
				this.Type = AttributeType.IDREFS;
				return;
			case "NAME":
				this.Type = AttributeType.NAME;
				return;
			case "NAMES":
				this.Type = AttributeType.NAMES;
				return;
			case "NMTOKEN":
				this.Type = AttributeType.NMTOKEN;
				return;
			case "NMTOKENS":
				this.Type = AttributeType.NMTOKENS;
				return;
			case "NUMBER":
				this.Type = AttributeType.NUMBER;
				return;
			case "NUMBERS":
				this.Type = AttributeType.NUMBERS;
				return;
			case "NUTOKEN":
				this.Type = AttributeType.NUTOKEN;
				return;
			case "NUTOKENS":
				this.Type = AttributeType.NUTOKENS;
				return;
			}
			throw new Exception("Attribute type '" + type + "' is not supported");
		}
		public bool SetPresence(string token)
		{
			bool result = true;
			if (token == "FIXED")
			{
				this.Presence = AttributePresence.FIXED;
			}
			else
			{
				if (token == "REQUIRED")
				{
					this.Presence = AttributePresence.REQUIRED;
					result = false;
				}
				else
				{
					if (!(token == "IMPLIED"))
					{
						throw new Exception(string.Format("Attribute value '{0}' not supported", token));
					}
					this.Presence = AttributePresence.IMPLIED;
					result = false;
				}
			}
			return result;
		}
	}
}

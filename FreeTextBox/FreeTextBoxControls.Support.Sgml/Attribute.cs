using System;
namespace FreeTextBoxControls.Support.Sgml
{
	internal class Attribute
	{
		internal string Name;
		internal AttDef DtdType;
		internal char QuoteChar;
		internal string literalValue;
		public string Value
		{
			get
			{
				if (this.literalValue != null)
				{
					return this.literalValue;
				}
				if (this.DtdType != null)
				{
					return this.DtdType.Default;
				}
				return null;
			}
			set
			{
				this.literalValue = value;
			}
		}
		public bool IsDefault
		{
			get
			{
				return this.literalValue == null;
			}
		}
		public void Reset(string name, string value, char quote)
		{
			this.Name = name;
			this.literalValue = value;
			this.QuoteChar = quote;
			this.DtdType = null;
		}
	}
}

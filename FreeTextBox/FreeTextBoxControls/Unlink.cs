using System;
namespace FreeTextBoxControls
{
	public class Unlink : ToolbarButton
	{
		public Unlink() : base("Unlink", "unlink")
		{
			base.isBuiltIn = true;
			base.CommandIdentifier = "unlink";
			base.BuiltInButtonOffset = 39;
			base.builtInScript = string.Empty;
			base.builtInEnabledScript = "var el = this.ftb.GetParentElement(); if (el.tagName && el.tagName.toLowerCase() != 'a') this.disabled = true; else this.disabled = false; ";
			base.className = "Unlink";
		}
	}
}

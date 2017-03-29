using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Unlink JavaScript functions builtin
	/// </summary>	
	public class Unlink : ToolbarButton {
		public Unlink() : base("Unlink","unlink") {
			isBuiltIn = true;
			this.CommandIdentifier = "unlink";
			this.BuiltInButtonOffset = 39;
			this.builtInScript = String.Empty;
			this.builtInEnabledScript = "var el = this.ftb.GetParentElement(); if (el.tagName && el.tagName.toLowerCase() != 'a') this.disabled = true; else this.disabled = false; ";
			this.className = "Unlink";
		}
	}
}

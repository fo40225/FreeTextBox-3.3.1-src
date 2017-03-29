using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with NetSpell JavaScript functions builtin
	/// </summary>	
	public class NetSpell : ToolbarButton {
		public NetSpell() : base("NetSpell","netspell") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 34;
			this.builtInScript = @"this.ftb.NetSpell();";
			this.className = "NetSpell";
		}
	}
}

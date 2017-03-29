using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with IE SpellCheck JavaScript functions builtin
	/// </summary>	
	public class IeSpellCheck : ToolbarButton {
		public IeSpellCheck() : base("IE SpellCheck","iespellcheck") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 34;
			this.builtInScript = @"this.ftb.IeSpellCheck();";
			this.className = "IeSpellCheck";
		}
	}
}

using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with JustifyFull JavaScript functions builtin
	/// </summary>	
	public class JustifyFull : ToolbarButton {
		public JustifyFull() : base("Justify Full","justifyfull") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 21;
			this.CommandIdentifier = "justifyfull";
			this.builtInScript = String.Empty;
			this.className = "JustifyFull";
		}
	}
}
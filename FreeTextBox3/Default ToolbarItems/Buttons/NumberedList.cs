using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// Returns a ToolbarButton with NumberedList JavaScript functions builtin
	/// </summary>	
	public class NumberedList : ToolbarButton {
		public NumberedList() : base("Numbered List","numberedlist") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 26;
			this.CommandIdentifier = "insertorderedlist";
			this.builtInScript = String.Empty;
			this.className = "NumberedList";
		}
	}
}
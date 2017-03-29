using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with RemoveFormat JavaScript functions builtin
	/// </summary>	
	public class RemoveFormat : ToolbarButton {
		public RemoveFormat() : base("Remove All Formatting","removeformat") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 31;
			this.CommandIdentifier = "removeformat";
			this.builtInScript = String.Empty;
			this.className = "RemoveFormat";
		}
	}
}
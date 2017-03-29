using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// What happens when a user tries to paste into the editor
	/// </summary>
	public enum PasteMode {
		/// <summary>
		/// Default browser functionality
		/// </summary>	
		Default = 0,
		/// <summary>
		/// Pasting is completely disabled
		/// </summary>	
		Disabled = 1,
		/// <summary>
		/// Only text. No HTML allowed.
		/// </summary>	
		Text = 2
	}
}

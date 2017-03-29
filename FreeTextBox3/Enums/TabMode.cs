using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// Determines what happens when the tab key is pressed
	/// </summary>
	public enum TabMode {
		/// <summary>
		/// Once in the editor, the tab key is disabled
		/// </summary>		
		Disabled = 0,
		/// <summary>
		/// Allow the browser to go to the next control in the HTML form
		/// </summary>		
		NextControl = 1,
		/// <summary>
		/// Inserts spaces into the editor to mimic tabs
		/// </summary>		
		InsertSpaces = 2
	}
}

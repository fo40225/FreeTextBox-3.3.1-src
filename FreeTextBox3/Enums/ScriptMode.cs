using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// Determines if ToolbarItem JavaScript is written inpage or comes from an external file
	/// </summary>
	public enum ScriptMode {
		/// <summary>
		/// Writes JavaScript code directly to the page
		/// </summary>		
		InPage = 0,
		/// <summary>
		/// Uses an externally linked JavaScript file for builtin ToolbarItems
		/// </summary>		
		External = 1
	}
}
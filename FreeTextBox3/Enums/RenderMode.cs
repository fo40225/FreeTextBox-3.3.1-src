using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// Force FreeTextBox to render as Rich or Text
	/// </summary>
	public enum RenderMode {
		/// <summary>
		/// Forces FreeTextBox to render the rich editor
		/// </summary>			
		Rich = 0,
		/// <summary>
		/// Forces FreeTextBox to render the plain TEXTAREA
		/// </summary>			
		Plain = 1,
		/// <summary>
		/// Allows FreeTextBox to choose what to render
		/// </summary>			
		NotSet = 2
	}
}

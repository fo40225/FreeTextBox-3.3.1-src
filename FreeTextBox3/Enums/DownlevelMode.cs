using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// Determines what happens when FreeTextBox renders for a client incapable of Rich editing
	/// </summary>
	public enum DownLevelMode {
		/// <summary>
		/// Renders an HTML TextArea control
		/// </summary>
		TextArea = 0,
		/// <summary>
		/// Displays the HTML inline
		/// </summary>
		InlineHtml = 1,
		/// <summary>
		/// Displays a message defined by the <see cref="FreeTextBox.DownlevelMessage"/> property
		/// </summary>
		Message = 2
	}
}

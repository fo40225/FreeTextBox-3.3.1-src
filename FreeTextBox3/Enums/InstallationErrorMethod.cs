using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// Options for displaying a message that the JavaScript has not loaded properly
	/// </summary>
	public enum InstallationErrorMessage {
		/// <summary>
		/// No error message is displayed
		/// </summary>
		None = 0,
		/// <summary>
		/// A JavaScript popup appears alerting users to the problem
		/// </summary>
		JavaScriptAlert = 1,
		/// <summary>
		/// An inline message appears alerting the user to the problem
		/// </summary>
		InlineMessage = 2	
	}
}

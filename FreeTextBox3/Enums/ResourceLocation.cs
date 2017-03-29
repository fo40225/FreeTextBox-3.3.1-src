using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// Determines where FreeTextBox looks for images
	/// </summary>
	public enum ResourceLocation {
		/// <summary>
		/// FreeTextBox will load its own internal resources
		/// </summary>
		InternalResource = 0,
		/// <summary>
		/// FreeTextBox will create HREFs to external files
		/// </summary>
		ExternalFile = 1
	}
}

using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// Determines how the toolbars and its buttons and dropdownlists are displayed
	/// </summary>
	public enum ToolbarStyleConfiguration {
		/// <summary>
		/// Allows the developer to change styles.
		/// </summary>
		NotSet = 0,		
		/// <summary>
		/// Sets images, borders and backcolors to Office XP style
		/// </summary>
		OfficeXP = 1,
		/// <summary>
		/// Sets images, borders and backcolors to Office 2000 style
		/// </summary>
		Office2000 = 2,
		/// <summary>
		/// Sets images, borders and backcolors to Office 2003 style
		/// </summary>
		Office2003 = 3,
		/// <summary>
		/// Sets images, borders and backcolors to Office Mac style
		/// </summary>
		OfficeMac = 4,
	}
}
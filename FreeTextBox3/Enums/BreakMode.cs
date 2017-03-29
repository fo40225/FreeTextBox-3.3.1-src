using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// What happens when 'Enter' is pressed 
	/// </summary>
	public enum BreakMode {
		/// <summary>
		/// P tags are added (IE only)
		/// </summary>	
		Paragraph = 0,
		/// <summary>
		/// BR tags are added
		/// </summary>	
		LineBreak = 1
	}
}

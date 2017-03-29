using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// Determines how buttons are rendered
	/// </summary>
	public enum ButtonRenderMode {
		/// <summary>
		/// Uses individual images for each 
		/// </summary>
		StyledBackgrounds = 0,
		/// <summary>
		/// Uses CSS to change images.  Recommended only for Mozilla.  IE's CSS and caching sucks.
		/// </summary>
		ImageBackgrounds = 1
	}
}

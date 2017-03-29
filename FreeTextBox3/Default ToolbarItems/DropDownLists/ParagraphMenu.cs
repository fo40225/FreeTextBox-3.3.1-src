using System;
using System.Drawing;
using System.Collections;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarDropDownList with Paragraph JavaScript functions builtin
	/// </summary>	
	public class ParagraphMenu : ToolbarDropDownList {
		public ParagraphMenu() : base("Paragraph") {
			this.isBuiltIn = true;
			this.CommandIdentifier = "formatBlock";
			this.className = "ParagraphMenu";	
			/*
			 this.builtInScript = @"function FTB_SetParagraph(ftbName,name,value) {
	if (FTB_IsHtmlMode(ftbName)) return;
	editor = FTB_GetIFrame(ftbName);
	if (value == '<body>') {
		editor.document.execCommand('formatBlock','','Normal');
		editor.document.execCommand('removeFormat');
		return;
	}
	editor.document.execCommand('formatBlock','',value);
}";
*/
		}
	}
}

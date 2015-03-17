using System;
namespace FreeTextBoxControls
{
	public class IeSpellCheck : ToolbarButton
	{
		public IeSpellCheck() : base("IE SpellCheck", "iespellcheck")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 34;
			base.builtInScript = "this.ftb.IeSpellCheck();";
			base.className = "IeSpellCheck";
		}
	}
}

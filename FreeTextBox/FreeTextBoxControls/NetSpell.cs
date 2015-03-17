using System;
namespace FreeTextBoxControls
{
	public class NetSpell : ToolbarButton
	{
		public NetSpell() : base("NetSpell", "netspell")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 34;
			base.builtInScript = "this.ftb.NetSpell();";
			base.className = "NetSpell";
		}
	}
}

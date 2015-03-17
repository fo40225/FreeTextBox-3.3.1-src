using System;
namespace FreeTextBoxControls.Support.Sgml
{
	internal enum State
	{
		Initial,
		Markup,
		EndTag,
		Attr,
		AttrValue,
		Text,
		PartialTag,
		AutoClose,
		CData,
		PartialText,
		PseudoStartTag,
		Eof
	}
}

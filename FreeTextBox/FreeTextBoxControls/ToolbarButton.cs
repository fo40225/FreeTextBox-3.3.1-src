using FreeTextBoxControls.Support;
using System;
using System.ComponentModel;
namespace FreeTextBoxControls
{
	public class ToolbarButton : ToolbarItem
	{
		internal bool htmlModeEnabled;
		[Category("Behavior"), Description("The image for the ToolbarButton"), NotifyParentProperty(true)]
		public string ButtonImage
		{
			get
			{
				object obj = base.ViewState["ButtonImage"];
				if (obj != null)
				{
					return (string)base.ViewState["ButtonImage"];
				}
				return "";
			}
			set
			{
				base.ViewState["ButtonImage"] = value;
			}
		}
		internal int BuiltInButtonOffset
		{
			get
			{
				object obj = base.ViewState["BuiltInButtonOffset"];
				if (obj != null)
				{
					return (int)base.ViewState["BuiltInButtonOffset"];
				}
				return -1;
			}
			set
			{
				base.ViewState["BuiltInButtonOffset"] = value;
			}
		}
		public ToolbarButton() : this("", "")
		{
		}
		public ToolbarButton(string title)
		{
			base.SetTitleLanguage(title);
			this.ButtonImage = "";
		}
		public ToolbarButton(string title, string buttonImage)
		{
			base.SetTitleLanguage(title);
			this.ButtonImage = buttonImage;
		}
		public ToolbarButton(string title, string nothing, string buttonImage)
		{
			base.SetTitleLanguage(title);
			this.ButtonImage = buttonImage;
		}
	}
}

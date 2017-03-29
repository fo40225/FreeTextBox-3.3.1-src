using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using FreeTextBoxControls.Support;

namespace FreeTextBoxControls {
	/// <summary>
	/// A Button in a <seealso cref="Toolbar"/> in a <seealso cref="FreeTextBox"/>.
	/// </summary>
	public class ToolbarButton : ToolbarItem {
		#region Contructors
		public ToolbarButton() : this("","") {
		}
		public ToolbarButton(string title) {
			this.SetTitleLanguage(title);
			//this.Title = title;
			this.ButtonImage = "";
		}
		public ToolbarButton(string title, string buttonImage) {
			this.SetTitleLanguage(title);
			//this.Title = title;
			this.ButtonImage = buttonImage;
		}
		public ToolbarButton(string title, string nothing, string buttonImage) {
			this.SetTitleLanguage(title);
			//this.Title = title;
			this.ButtonImage = buttonImage;
		}
		#endregion

		#region Private Properties
		internal bool htmlModeEnabled = false;
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets or sets the name of the button image. 
		/// </summary>
		[
		Category("Behavior"),
		Description("The image for the ToolbarButton"),
		NotifyParentProperty(true)
		]
		public string ButtonImage {
			get {
				object o = ViewState["ButtonImage"];
				return (o==null) ? "" : (string) ViewState["ButtonImage"];
			}
			set {
				ViewState["ButtonImage"] = value;
			}
		}	
	
		/// <summary>
		/// Gets or sets the CSS offset of the button. 
		/// </summary>
		internal int BuiltInButtonOffset {
			get {
				object o = ViewState["BuiltInButtonOffset"];
				return (o==null) ? -1 : (int) ViewState["BuiltInButtonOffset"];
			}
			set {
				ViewState["BuiltInButtonOffset"] = value;
			}
		}
		
		#endregion
		
		/*
		 * This is no longer necessary in ASP.NET 2.0
		 * 
		#region IParserAccessor
		/// <remarks>
		/// Use text between FTB:<see cref="ToolbarItem"/> tags as the ScriptBlock property
		/// </remarks>
		void IParserAccessor.AddParsedSubObject(object obj) {
			if(obj is LiteralControl) {
				ScriptBlock = ((LiteralControl)obj).Text;
				return;
			}
		}
		#endregion
		*/
	}
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.UI;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Resources;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Reflection;
using FreeTextBoxControls.Support;
using FreeTextBoxControls.Licensing;

[assembly:TagPrefix("FreeTextBoxControls", "FTB")]
[assembly:AllowPartiallyTrustedCallers]
namespace FreeTextBoxControls {
	
	/// <summary>
	/// The no. 1 free ASP.NET HTML editor for IE, Firefox, Opera, Safari, and Chrome.
	/// </summary>
	/// <remarks>
	///	Supports IE, Firefox, Opera, Safari, and Chrome. The Toolbar can be customized in several ways. 1) By setting the <see cref="ToolbarLayout"/> string 
	///	with appopriate <see cref="ToolbarButton"/> and <see cref="ToolbarDropDownList"/> names, 2) Procedurally, by setting AutoGenerateToolbarsFromString=false and
	///	adding ASP.NET code, 3) in codebehind, 4) by inherting from <see cref="ToolbarButton"/> or <see cref="ToolbarDropDownList"/>
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// -- webform.aspx ----------------------------
	/// <%@ Register TagPrefix="FTB" Assembly="FreeTextBox" Namespace="FreeTextBoxControls" %>
	/// <html>
	///	 <head>
	///	 </head>
	///	 <body>
	///		<form runat="server">
	///			<FTB:FreeTextBox id="FreeTextBox1" runat="server" />
	///		</form>
	///	 </body>
	/// </html>
	/// 
	/// -- web.config -------------------------------
	/// <?xml version="1.0" encoding="utf-8" ?>
	/// <configuration>
	///		<system.web>
	///			<httpHandlers>
	///				<add verb="GET" 
	///					path="FtbWebResource.axd" 
	///					type="FreeTextBoxControls.AssemblyResourceHandler, FreeTextBox" />		
	///			</httpHandlers>
	///		</system.web>      
	/// </configuration>
	/// ]]></code>
	/// </example>
	[
	LicenseProvider( typeof( FreeTextBoxControls.Licensing.FtbLicenseProvider) ) ,
	ToolboxData("<{0}:FreeTextBox runat=\"server\"></{0}:FreeTextBox>"),
	ValidationPropertyAttribute("Text"),
	DefaultProperty("Text"),
	ParseChildren(true),
	PersistChildren(true),
	Designer(typeof(FreeTextBoxControls.Design.FreeTextBoxDesigner))
	]	
	public class FreeTextBox : Control, IPostBackDataHandler, INamingContainer, IPostBackEventHandler, IDisposable {
		
		public FreeTextBox() {
			this.ProcessText += new EventHandler(InternalProcessText);
			try 
			{
				license = (FtbLicense) LicenseManager.Validate( typeof( FreeTextBox ), this );
			} 
			catch (Exception e) {
                license = new FtbLicense(this.GetType(),"Unlicensed: " + e.ToString(), "", false);
            }	

			// temp
			//license = new FtbLicense(typeof( FreeTextBox ), "", true);			
		}
		
		public override void Dispose() {
			base.Dispose();
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing ) {
			if( disposing ) {
				if( license != null ) {
					license.Dispose();
					license = null;
				}
			}
		}
		
		internal bool DesignMode
		{
			get
			{
				if (Context == null)
				{
					return true;
				}
				else if (this.Site != null)
				{
					return this.Site.DesignMode;
				}
				else
				{
					return false;
				}
			}
		}
		
		#region Private Properties
		private FtbLicense license	= null;
		private ToolbarCollection toolbars;
		private ToolbarButtonStyle buttonStyle;
		private ToolbarButtonStyle buttonStyleActive;
		private bool hasToolbars;
		private BrowserInfo browserInfo;
		private FreeTextBoxControls.Support.ResourceManager resourceManager;
		private string viewStateText;
		#endregion

		#region Public Properties
		
		#region Main Properties
		/// <summary>
		/// Gets or sets the HTML for the editor.
		/// </summary>
		[
		CategoryAttribute("Output"),
		Description("Contains the HTML for the editor.")
		]
		public string Text {
			set {
                ViewState["Text"] = value;           
            }
			get {
				object savedState = this.ViewState["Text"];
				return (savedState == null) ? "" : (string) savedState;
			}
		}

		/// <summary>
		/// Converts the returned HTML to XHTML
		/// </summary>
		[
		CategoryAttribute("Output"),
		Description("Converts the returned HTML to XHTML")
		]
		public string Xhtml {
			get {
				string text = this.Text;
				Formatter formatter = new Formatter();
				return formatter.HtmlToXhtml(text);
			}
		}

		/// <summary>
		/// Gets the Text data from the ViewState before it is processed
		/// </summary>
		[
		CategoryAttribute("Output"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Description("Contains the pre-processed HTML for the editor.")
		]
		public string ViewStateText {
			get {
				return viewStateText;
			}
		}	
		/// <summary>
		/// Gets the Text stripped of Html tags
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		]
		public string HtmlStrippedText {
			get {
				string text = this.Text;
				return Regex.Replace(text,"<(.|\n)+?>"," ",RegexOptions.IgnoreCase);
			}
		}
		/// <summary>
		/// Overridden ClientID field to ensure CSS and JS compatibility
		/// </summary>
		public override string ClientID {
			get {
				string id = base.ClientID;
				// remove beginning _ for CSS
				while (id.Substring(0,1) == "_") {
					id = id.Substring(1);
				}
				// remove an nested file-specific characters
				return id.Replace("\\","_").Replace("/","_").Replace(".","_");
			}
		}

		/// <summary>
		/// Style parameters for ToolbarButtons when in "normal" state. 
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		PersistenceMode(PersistenceMode.InnerProperty),
		NotifyParentProperty(true),
		CategoryAttribute("Appearance")
		]
		public virtual ToolbarButtonStyle ButtonStyle {
			get {
				if (buttonStyle == null) {
					buttonStyle = new ToolbarButtonStyle();
					// initialize to Images.Office2003
					this.SetButtonStyle(buttonStyle, ToolbarStyleConfiguration.Office2003, true);
					if (IsTrackingViewState)
						((IStateManager)buttonStyle).TrackViewState();
				}
				return buttonStyle;
			}
		}
		/// <summary>
		/// Style parameters for ToolbarButtons when in "active" state.
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		PersistenceMode(PersistenceMode.InnerProperty),
		NotifyParentProperty(true),
		CategoryAttribute("Appearance")
		]
		public virtual ToolbarButtonStyle ButtonStyleActive {
			get {
				if (buttonStyleActive == null) {
					buttonStyleActive = new ToolbarButtonStyle();
					// initialize to Images.Office2003
					this.SetButtonStyle(buttonStyleActive, ToolbarStyleConfiguration.Office2003, false);

					if (IsTrackingViewState)
						((IStateManager)buttonStyleActive).TrackViewState();
				}
				return buttonStyleActive;
			}
		}

		/// <summary>
		/// Contains all the Toolbars and ToolbarItems
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerDefaultProperty),
		CategoryAttribute("Behavior")
		]
		public ToolbarCollection Toolbars {
			get {
				if (toolbars == null) {
					toolbars = new ToolbarCollection();
					if (this.IsTrackingViewState)
						((IStateManager)toolbars).TrackViewState();
				}
				return toolbars;
			}
		}	
		#endregion

		#region Events
		/// <summary>
		/// Gets or sets a javascript method fired when the text changes
		/// </summary>
		[
		CategoryAttribute("Behavior"),
		]
		public string ClientSideTextChanged {
			set { ViewState["ClientSideTextChanged"] = value; }
			get {
				object savedState = this.ViewState["ClientSideTextChanged"];
				return (savedState == null) ? string.Empty : (string) savedState;
			}
		}		
		#endregion

		#region Resource Related
		/// <summary>
		/// Gets or sets where JS files are stored
		/// </summary>
		public ResourceLocation JavaScriptLocation {
			get { 
				object savedState = this.ViewState["JavaScriptLocation"];
				return (savedState == null) ? ResourceLocation.InternalResource : (ResourceLocation) savedState;
			}
			set {
				ViewState["JavaScriptLocation"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets where button images are stored
		/// </summary>
		public ResourceLocation ButtonImagesLocation {
			get { 
				object savedState = this.ViewState["ButtonImagesLocation"];
				return (savedState == null) ? ResourceLocation.InternalResource : (ResourceLocation) savedState;
			}
			set {
				ViewState["ButtonImagesLocation"] = value;
			}
		}
		/// <summary>
		/// Gets or sets where the images such as the toolbar background and separators are stored
		/// </summary>
		public ResourceLocation ToolbarImagesLocation {
			get { 
				object savedState = this.ViewState["ToolbarImagesLocation"];
				return (savedState == null) ? ResourceLocation.InternalResource : (ResourceLocation) savedState;
			}
			set {
				ViewState["ToolbarImagesLocation"] = value;
			}
		}
		/// <summary>
		/// Gets or sets which button set to draw from if using resources
		/// </summary>
		public ToolbarStyleConfiguration ButtonSet {
			get { 
				object savedState = this.ViewState["ToolbarStyleConfiguration"];
				return (savedState == null) ? ToolbarStyleConfiguration.Office2003 : (ToolbarStyleConfiguration) savedState;
			}
			set {
				ViewState["ToolbarStyleConfiguration"] = value;
			}
		}
		
		#endregion

		#region Display Settings
		/// <summary>
		/// Gets or sets how the user is alerted that FreeTextBox has not been installed correctly
		/// </summary>
		[
		CategoryAttribute("Behavior"),
		Description("Gets or sets how the user is alerted that FreeTextBox has not been installed correctly.")
		]		
		public InstallationErrorMessage InstallationErrorMessage {
			get { 
				object savedState = this.ViewState["InstallationErrorMessage"];
				return (savedState == null) ? InstallationErrorMessage.InlineMessage : (InstallationErrorMessage) savedState;
			}
			set {
				ViewState["InstallationErrorMessage"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the base URL of the editor
		/// </summary>
		[
		CategoryAttribute("Behavior"),
		Description("Gets or sets the URL of the editor window.")
		]		
		public string BaseUrl {
			get { 
				object savedState = this.ViewState["BaseUrl"];
				return (savedState == null) ? "" : (string) savedState;
			}
			set {
				ViewState["BaseUrl"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the URL of the style sheet used in Design mode.
		/// </summary>
		[
		CategoryAttribute("External"),
		Description("Gets or sets the URL of the style sheet used in Design mode.")
		]		
		public string DesignModeCss 
		{
			get 
			{ 
				object savedState = this.ViewState["DesignModeCss"];
				return (savedState == null) ? "" : ResolveUrl((string) savedState);
			}
			set 
			{
				ViewState["DesignModeCss"] = value;
			}
		}
		internal string DesignModeCssViewState {
			get { 
				object savedState = this.ViewState["DesignModeCss"];
				return (savedState == null) ? "" : (string) savedState;
			}
		}
		/// <summary>
		/// Gets or sets a CSS class used for the editor area
		/// </summary>
		[
		CategoryAttribute("External"),
		Description("Gets or sets a CSS class used for the editor area.")
		]		
		public string DesignModeBodyTagCssClass
		{
			get 
			{ 
				object savedState = this.ViewState["DesignModeBodyTagCssClass"];
				return (savedState == null) ? "" : (string) savedState;
			}
			set 
			{
				ViewState["DesignModeBodyTagCssClass"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the width of the editor.
		/// </summary>
		[
		CategoryAttribute("Appearance"),
		Description("Gets or sets the width of the editor.")
		]
		public Unit Width {
			get {
				object savedState = this.ViewState["Width"];
				return (savedState == null) ? new Unit("600px") : (Unit) savedState;
			}
			set {
				ViewState["Width"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the height of the editor including toolbars.
		/// </summary>
		[
		CategoryAttribute("Appearance"),
		Description("Gets or sets the height of the editor.")
		]		
		public Unit Height {
			get { 
				object savedState = this.ViewState["Height"];
				return (savedState == null) ? new Unit("350px") : (Unit) savedState;

			}
			set {
				ViewState["Height"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the back color of the entire editor area.
		/// </summary>
		[
		CategoryAttribute("Appearance"),
		Description("Gets or sets the background color of the editor.")
		]		
		public Color BackColor {
			get { 
				object savedState = this.ViewState["BackColor"];
				return (savedState == null) ? ColorTranslator.FromHtml("#9EBEF5") : (Color) savedState;
			}
			set {
				ViewState["BackColor"] = value;
			}
		}
		#endregion

		#region ToolbarItem Settings
		/// <summary>
		/// Gets or sets whether buttons change images onMouseOver.
		/// </summary>	
		[
		Category("Toolbar"),
		Description("Gets or sets whether buttons change images onMouseOver.")
		]
		public string ButtonFolder {
			get {
				object o = ViewState["ButtonFolder"];
				if (o==null) {
					return "Images";
				} else {
					string folder = (string) ViewState["ButtonFolder"];
					
					// remove starting slashes
					while (folder.StartsWith("/") || folder.StartsWith(@"\")) 
						folder = folder.Substring(1);
				
					// remove trailing slashes
					while (folder.EndsWith("/") || folder.EndsWith(@"\")) 
						folder = folder.Substring(0,folder.Length-1);
					
					return folder;
				}
			}
			set {
				ViewState["ButtonFolder"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the width in pixels of <see cref="ToolbarButton"/>s
		/// </summary>
		[
		Category("Toolbar"),
		Description("Gets or sets the width in pixels of ToolbarButtons.")
		]		
		public int ButtonWidth {
			get { 
				object savedState = this.ViewState["ButtonWidth"];
				return (savedState == null) ? 21 : (int) savedState;
			}
			set {
				ViewState["ButtonWidth"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the height in pixels of <see cref="ToolbarButton"/>s
		/// </summary>
		[
		Category("Toolbar"),
		Description("Gets or sets the height in pixels of ToolbarButtons.")
		]		
		public int ButtonHeight {
			get { 
				object savedState = this.ViewState["ButtonHeight"];
				return (savedState == null) ? 20 : (int) savedState;
			}
			set {
				ViewState["ButtonHeight"] = value;
			}
		}
		
		/*

		/// <summary>
		/// Gets or sets the height in pixels of <see cref="ToolbarButton"/>s
		/// </summary>
		[
		Category("Toolbar"),
		Description("Gets or sets the height in pixels of ToolbarButtons.")
		]		
		public ButtonRenderMode ButtonRenderMode {
			get { 
				object savedState = this.ViewState["ButtonRenderMode"];
				return (savedState == null) ? ButtonRenderMode.StyledBackgrounds : (ButtonRenderMode) savedState;
			}
			set {
				ViewState["ButtonRenderMode"] = value;
			}
		}
		*/
		#endregion

		#region Toolbar Settings
		/// <summary>
		/// Gets or sets a prebuilt Toolbar styling to emulate versions of Microsoft Office.
		/// </summary>		
		[
		Category("Toolbar"),
		DefaultValue(""),
		Description("Gets or sets a prebuilt Toolbar styling to emulate versions of Microsoft Office."),
		NotifyParentProperty(true)
		]
		public ToolbarStyleConfiguration ToolbarStyleConfiguration {
			get {
				object o = ViewState["ToolbarStyleConfiguration"];
				return (o==null) ? ToolbarStyleConfiguration.NotSet : (ToolbarStyleConfiguration) ViewState["ToolbarStyleConfiguration"];
			}
			set {
				ViewState["ToolbarStyleConfiguration"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the CSS class for DropDownLists
		/// </summary>	
		[
		Category("Toolbar"),
		DefaultValue(""),
		Description("Gets or sets the CSS class for DropDownLists."),
		NotifyParentProperty(true)
		]

		public string DropDownListCssClass {
			get {
				object o = ViewState["DropDownListCssClass"];
				return (o==null) ? "" : (string) ViewState["DropDownListCssClass"];
			}
			set {
				ViewState["DropDownListCssClass"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the CSS class for HTML Mode
		/// </summary>	
		[
		Category("Toolbar"),
		DefaultValue(""),
		Description("Gets or sets the CSS class for HTML Mode."),
		NotifyParentProperty(true)
		]
		public string HtmlModeCssClass 
		{
			get 
			{
				object o = ViewState["HtmlModeCssClass"];
				return (o==null) ? "" : (string) ViewState["HtmlModeCssClass"];
			}
			set 
			{
				ViewState["HtmlModeCssClass"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the <see cref="ToolbarButton"/>s and <see cref="ToolbarDropDownList"/>s reflect the style of the text at the cursor.
		/// </summary>	
		[
		Category("Behavior"),
		DefaultValue(""),
		Description("Gets or sets whether the ToolbarButtons and ToolbarDropDownLists reflect the style of the text at the cursor."),
		NotifyParentProperty(true)
		]
		public bool UpdateToolbar {
			get {
				object o = ViewState["UpdateToolbar"];
				return (o==null) ? true : (bool) ViewState["UpdateToolbar"];
			}
			set {
				ViewState["UpdateToolbar"] = value;
			}
		}
		/// <summary>
		/// Gets or sets whether a background image is used for the toolbar.
		/// </summary>	
		[
		Category("Behavior"),
		DefaultValue(""),
		Description("Gets or sets whether a background image is used for the toolbar."),
		NotifyParentProperty(true)
		]
		public bool ToolbarBackgroundImage {
			get {
				object o = ViewState["BackgroundImage"];
				return (o==null) ? true : (bool) ViewState["BackgroundImage"];
			}
			set {
				ViewState["BackgroundImage"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the back color of each toolbar.
		/// </summary>	
		[
		Category("Toolbar"),
		Description("The back color of each toolbar.")
		]
		public Color ToolbarBackColor {
			get {
				object o = ViewState["ToolbarBackColor"];
				return (o==null) ? Color.Transparent: (Color) ViewState["ToolbarBackColor"];
			}
			set {
				ViewState["ToolbarBackColor"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether this control display <see cref="Toolbar"/>s.
		/// </summary>
		[
		CategoryAttribute("Toolbar"),
		Description("Gets or sets whether this control display Toolbars")
		]	
		public bool EnableToolbars {
			get { 
				object savedState = this.ViewState["EnableToolbars"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["EnableToolbars"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether an image is behind a toolbar
		/// </summary>
		[
		CategoryAttribute("Toolbar"),
		Description("Gets or sets whether an image is behind a toolbar")
		]		
		public bool UseToolbarBackGroundImage {
			get { 
				object savedState = this.ViewState["UseToolbarBackGroundImage"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["UseToolbarBackGroundImage"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the toolbars are hidden when the editor is in HTML mode.
		/// </summary>
		[
		CategoryAttribute("Toolbar"),
		Description("Gets or sets whether the toolbars are hidden when the editor is in HTML mode.")
		]
		public bool AutoGenerateToolbarsFromString {
			get { 
				object savedState = this.ViewState["AutoGenerateToolbarsFromString"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["AutoGenerateToolbarsFromString"] = value;
			}
		}

		/// <summary>
		/// A string of buttons used to create the toolbar. Use commas (,) to separate items.  A pipe (|) will insert a separator and a semicolon (;) will start a new Toolbar.  Possible values are ParagraphMenu, FontFacesMenu, FontSizesMenu, FontForeColorsMenu, FontForeColorPicker, FontBackColorsMenu, FontBackColorPicker, Bold, Italic, Underline, Strikethrough, Superscript, Subscript, CreateLink, Unlink, RemoveFormat, JustifyLeft, JustifyRight, JustifyCenter, JustifyFull, BulletedList, NumberedList, Indent, Outdent, Cut, Copy, Paste, Delete, Undo, Redo, Print, Save, SymbolsMenu, StylesMenu, InsertHtmlMenu, InsertRule, InsertDate, InsertTime, ieSpellCheck, NetSpell, WordClean, InsertImageFromGallery, InsertTable, InsertTableRowBelow, InsertTableRowAbove, DeleteTableRow, InsertTableColumnBelow, InsertTableColumnAbove, DeleteTableColumn, InsertForm, InsertForm,InsertTextBox,InsertTextArea,InsertRadioButton,InsertCheckBox,InsertDropDownList,InsertButton, InsertDiv, InsertImageFromGallery
		/// </summary>
		[
		CategoryAttribute("Toolbar")
		]	
		public string ToolbarLayout {
			get { 
				object savedState = this.ViewState["ToolbarLayout"];
				return (savedState == null) ? ToolbarGenerator.DefaultConfigString : (string) savedState;
			}
			set {
				ViewState["ToolbarLayout"] = value;
			}
		}
		/// <summary>
		/// Private variable to see it ToolbarItems have been created from their string representation
		/// </summary>
		private bool ToolbarsCreated {
			get { 
				object savedState = this.ViewState["ToolbarsCreated"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set {
				ViewState["ToolbarsCreated"] = value;
			}
		}
		#endregion

		#region DropDownList arrays
		/// <summary>
		/// The list of styles in the <see cref="StylesMenu"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] StylesMenuList {
			get {
				object savedState = this.ViewState["StyleMenuList"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["StyleMenuList"] = value;
			}
		}
		/// <summary>
		/// The list of display names for the styles in <see cref="StylesMenuList"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] StylesMenuNames {
			get {
				object savedState = this.ViewState["StyleMenuNames"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["StyleMenuNames"] = value;
			}
		}

		/// <summary>
		/// The list of fonts in the <see cref="FontFacesMenu"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] FontFacesMenuList {
			get {
				object savedState = this.ViewState["FontFacesMenuList"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["FontFacesMenuList"] = value;
			}
		}
		/// <summary>
		/// The list of display names for the fonts in <see cref="FontFacesMenuList"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] FontFacesMenuNames {
			get {
				object savedState = this.ViewState["FontFacesMenuNames"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["FontFacesMenuNames"] = value;
			}
		}
		/// <summary>
		/// The list of font sizes (1,2,3) in the <see cref="FontSizesMenu"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] FontSizesMenuList {
			get {
				object savedState = this.ViewState["FontSizesMenuList"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["FontSizesMenuList"] = value;
			}
		}
		/// <summary>
		/// The list of display names for the font sizes in <see cref="FontSizesMenuList"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] FontSizesMenuNames {
			get {
				object savedState = this.ViewState["FontSizesMenuNames"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["FontSizesMenuNames"] = value;
			}
		}

		/// <summary>
		/// The list of font colors in the <see cref="FontForeColorsMenu"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public Color[] FontForeColorMenuList {
			get {
				object savedState = this.ViewState["FontForeColorMenuList"];
				return (savedState == null) ? null : (Color[]) savedState;
			}
			set {
				ViewState["FontForeColorMenuList"] = value;
			}
		}
		/// <summary>
		/// The list of display names for the colors in <see cref="FontForeColorMenuList"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] FontForeColorMenuNames {
			get {
				object savedState = this.ViewState["FontForeColorMenuNames"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["FontForeColorMenuNames"] = value;
			}
		}

		/// <summary>
		/// The list of back colors in the <see cref="FontBackColorsMenu"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public Color[] FontBackColorMenuList {
			get {
				object savedState = this.ViewState["FontBackColorMenuList"];
				return (savedState == null) ? null : (Color[]) savedState;
			}
			set {
				ViewState["FontBackColorMenuList"] = value;
			}
		}
		/// <summary>
		/// The list of display names for the back colors in <see cref="FontBackColorMenuList"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] FontBackColorMenuNames {
			get {
				object savedState = this.ViewState["FontBackColorMenuNames"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["FontBackColorMenuNames"] = value;
			}
		}
		/// <summary>
		/// The list of HTML blocks inserted by <see cref="InsertHtmlMenu"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] InsertHtmlMenuList {
			get {
				object savedState = this.ViewState["InsertHtmlMenuList"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["InsertHtmlMenuList"] = value;
			}
		}
		/// <summary>
		/// The list of display names for the HTML blocks in <see cref="InsertHtmlMenuList"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] InsertHtmlMenuNames {
			get {
				object savedState = this.ViewState["InsertHtmlMenuNames"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["InsertHtmlMenuNames"] = value;
			}
		}
		/// <summary>
		/// The list of HTML blocks inserted by <see cref="ParagraphMenu"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] ParagraphMenuList {
			get {
				object savedState = this.ViewState["ParagraphMenuList"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["ParagraphMenuList"] = value;
			}
		}
		/// <summary>
		/// The list of display names for the HTML blocks used in <see cref="ParagraphMenuList"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] ParagraphMenuNames {
			get {
				object savedState = this.ViewState["ParagraphMenuNames"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["ParagraphMenuNames"] = value;
			}
		}

		/// <summary>
		/// The list of HTML symbols inserted by <see cref="SymbolsMenu"/>
		/// </summary>
		[
		CategoryAttribute("DropDownList Arrays")
		]	
		public string[] SymbolsMenuList {
			get {
				object savedState = this.ViewState["SymbolsMenuList"];
				return (savedState == null) ? null : (string[]) savedState;
			}
			set {
				ViewState["SymbolsMenuList"] = value;
			}
		}
		#endregion

		#region Behavior
		/// <summary>
		/// Gets or sets the path to FtbWebResource.axd (if the web.config reference needs to happen elsewhere)
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]	
		public string AssemblyResourceHandlerPath 
		{
			get 
			{
				object savedState = this.ViewState["AssemblyResourceHandlerPath"];
				return (savedState == null) ? "" /*ResolveUrl("~/")*/ : (string) savedState;
			}
			set 
			{
				ViewState["AssemblyResourceHandlerPath"] = value;
			}
		}
	
		/// <summary>
		/// Gets or sets whether to show the path of tags at the cursor position
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]	
		public bool ShowTagPath {
			get {
				if (!license.IsPro) return false;
				object savedState = this.ViewState["ShowTagPath"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["ShowTagPath"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets whether this control will receive focus on startup
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]	
		public bool Focus {
			get {
				object savedState = this.ViewState["Focus"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set {
				ViewState["Focus"] = value;
			}
		}
		
		/// <summary>
		/// The Url of the image gallery to be launched
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]	
		public string ImageGalleryPath {
			get {
				object savedState = this.ViewState["ImageGalleryPath"];
				return (savedState == null) ? "~/images/" : (string) savedState;
			}
			set {
				ViewState["ImageGalleryPath"] = value;
			}
		}
		/// <summary>
		/// The Url of the gallery used when the <see cref="InsertImageFromGallery"/> button is pressed
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]	
		public string ImageGalleryUrl {
			get {
				object savedState = this.ViewState["ImageGalleryUrl"];
				if (savedState == null) {
					return "ftb.imagegallery.aspx?rif={0}&cif={0}";
					/*
					//try {
					
						// Reflection is used to check for Daniel Fisher's control
						//
						Type t = Type.GetType("StaticDust.Web.UI.Controls.UploadDialogButton, StaticDust.Web.UI.Controls.UploadDialog");
	
						if (t == null)						
							return "";
					
						// create object
						Object obj = t.InvokeMember(null, 
							BindingFlags.DeclaredOnly | 
							BindingFlags.Public | BindingFlags.NonPublic | 
							BindingFlags.Instance | BindingFlags.CreateInstance, null, null, new Object[] {} );
					
						if (obj == null) 
							return "";

						t.InvokeMember("UploadDirectory", 
							BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | 
							BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new object[] { this.ImageGalleryPath } );

						t.InvokeMember("ReturnFunction", 
							BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | 
							BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new object[] {"FTB_ReturnImageFromGallery()"} );

						string galleryLink = (string) t.InvokeMember("JavascriptLink", 
							BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | 
							BindingFlags.Instance | BindingFlags.GetProperty, null, obj, null);
						
			
						// strip off the window.open stuff
						Match m = Regex.Match(galleryLink,"'(?<link>[^']+)'",RegexOptions.IgnoreCase);
						if (m.Success) 
							return m.Groups["link"].Value;
						else 
							return "fart";

					//} catch {
						return "";	
					//}
					*/
				} else {
					return (string) savedState;
				}
			}
			set {
				ViewState["ImageGalleryUrl"] = value;
			}
		}
		/// <summary>
		/// Gets or sets whether styles will automagically be parsed from DesignModeCss into StylesMenu items
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public bool AutoParseStyles {
			get { 
				object savedState = this.ViewState["AutoParseStyles"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["AutoParseStyles"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the src url for the IFRAME when in SSL mode
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public string SslUrl {
			get { 
				object savedState = this.ViewState["SslUrl"];
				return (savedState == null) ? "/." : (string) savedState;
			}
			set {
				ViewState["SslUrl"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the direction of text within the editor
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public TextDirection TextDirection {
			get { 
				object savedState = this.ViewState["TextDirection"];
				return (savedState == null) ? TextDirection.LeftToRight : (TextDirection) savedState;
			}
			set {
				ViewState["TextDirection"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether FreeTextBox will attempt strip all scripting tags and scripting events.
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public bool StripAllScripting {
			get { 
				object savedState = this.ViewState["StripAllScripting"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set {
				ViewState["StripAllScripting"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether FreeTextBox will use <see cref="FreeTextBoxControls.Support.Sgml.SgmlReader"/> to reformat tags as XHTML
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public bool FormatHtmlTagsToXhtml {
			get { 
				object savedState = this.ViewState["FormatHtmlTagsToXhtml"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["FormatHtmlTagsToXhtml"] = value;
			}
		}		
		
		/// <summary>
		/// Gets or sets whether to disable onload code that allow the back button to function in Internet Explorer. This does not affect Mozilla.
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public bool DisableIEBackButton {
			get { 
				object savedState = this.ViewState["DisableIEBackButton"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set {
				ViewState["DisableIEBackButton"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets whether to add /. to the src attribute of the iframe in order for IE to correctly function in SSL.
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public bool EnableSsl {
			get { 
				object savedState = this.ViewState["EnableSsl"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set {
				ViewState["EnableSsl"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets whether or not to strip out the local server name from all A and IMG tags
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public bool RemoveServerNameFromUrls {
			get { 
				object savedState = this.ViewState["RemoveServerNameFromUrls"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["RemoveServerNameFromUrls"] = value;
			}
		}
		/// <summary>
		/// Gets or sets whether or not to remove the scriptname from bookmarks
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public bool RemoveScriptNameFromBookmarks {
			get { 
				object savedState = this.ViewState["RemoveScriptNameFromBookmarks"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["RemoveScriptNameFromBookmarks"] = value;
			}
		}
		/// <summary>
		/// Gets or sets whether or not to convert non-ASCII characters to HTML Hex codes.
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public bool ConvertHtmlSymbolsToHtmlCodes {
			get { 
				object savedState = this.ViewState["ConvertHtmlSymbolsToHtmlCodes"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set {
				ViewState["ConvertHtmlSymbolsToHtmlCodes"] = value;
			}
		}		
		/// <summary>
		/// Gets or sets what happens when a user tries to paste into the editor
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]		
		public PasteMode PasteMode {
			set { ViewState["PasteMode"] = value; }
			get {
				object savedState = this.ViewState["PasteMode"];
				return (savedState == null) ?  PasteMode.Default : (PasteMode) savedState;
			}
		}
		/// <summary>
		/// Gets or sets what happens when 'Enter' is pressed (IE only)
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]		
		public BreakMode BreakMode {
			set { ViewState["BreakMode"] = value; }
			get {
				object savedState = this.ViewState["BreakMode"];
				return (savedState == null) ?  BreakMode.Paragraph : (BreakMode) savedState;
			}
		}
		/// <summary>
		/// Gets or sets the TabMode of the control
		/// </summary>		
		[
		CategoryAttribute("Behavior")
		]
		public TabMode TabMode {
			set { ViewState["TabMode"] = value; }
			get {
				object savedState = this.ViewState["TabMode"];
				return (savedState == null) ?  TabMode.InsertSpaces : (TabMode) savedState;
			}
		}

		/// <summary>
		/// Gets or sets whether to allow HTML mode
		/// </summary>		
		[
		CategoryAttribute("Behavior")
		]
		public bool EnableHtmlMode {
			set { ViewState["EnableHtmlMode"] = value; }
			get {
				object savedState = this.ViewState["EnableHtmlMode"];
				return (savedState == null) ?  true : (bool) savedState;
			}
		}
		
		/// <summary>
		/// Gets or sets how the control will render.  NotSet = FreeTextBox will attempt to figure out which browser to render for, Text or Rich forces FreeTextBox to render accordingly
		/// </summary>		
		[
		CategoryAttribute("Behavior")
		]
		public RenderMode RenderMode {
			set { ViewState["RenderMode"] = value; }
			get {
				object savedState = this.ViewState["RenderMode"];
				return (savedState == null) ?  RenderMode.NotSet : (RenderMode) savedState;
			}
		}

		/// <summary>
		/// Gets or sets the lanuage used for all labels. Pulls from XML languages files in /aspnet_client/FreeTextBox/Languages folder.
		/// </summary>		
		[
		CategoryAttribute("Behavior")
		]
		public string Language {
			set { ViewState["Language"] = value; }
			get {
				object savedState = this.ViewState["Language"];
				return (savedState == null) ?  "en-US" : (string) savedState;
			}
		}
		
		/// <summary>
		/// Gets or sets the folder where FreeTextBox images and scripts are stored
		/// </summary>
		[
		CategoryAttribute("External")
		]
		public string SupportFolder {
			set { ViewState["SupportFolder"] = value; }
			get {
				object savedState = this.ViewState["SupportFolder"];
				if (savedState == null) {
					return ResolveUrl("/aspnet_client/FreeTextBox/");
				} else {
					string folder = (string) savedState;
					// ensure forward slashes
					folder.Replace(@"\","/");
					// ensure final slash
					if (!folder.EndsWith("/")) folder+= "/";
					
					return ResolveUrl(folder);
				}
			}
		}
		internal string SupportFolderViewState {
			get {
				object savedState = this.ViewState["SupportFolder"];
				return (savedState == null) ? "/aspnet_client/FreeTextBox/" : (string) savedState;
			}
		}

		/// <summary>
		/// Gets or sets if ToolbarItem JavaScript is written inpage or comes from an external file
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]	
		public ScriptMode ScriptMode {
			get { 
				object savedState = this.ViewState["ScriptMode"];
				return (savedState == null) ? ScriptMode.External : (ScriptMode) savedState;
			}
			set {
				ViewState["ScriptMode"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the TabIndex on the IFrame
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public int TabIndex {
			get { 
				object savedState = this.ViewState["TabIndex"];
				return (savedState == null) ? -1 : (int) savedState;
			}
			set {
				ViewState["TabIndex"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether content is editable.
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]
		public bool ReadOnly {
			get { 
				object savedState = this.ViewState["ReadOnly"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set {
				ViewState["ReadOnly"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the mode FreeTextBox starts in.
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]		
		public EditorMode StartMode {
			get {
				object savedState = this.ViewState["StartMode"];
				return (savedState == null) ? EditorMode.DesignMode : (EditorMode) savedState;
			}
			set {
				ViewState["StartMode"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets file type extention of buttons (gif,jpg,png)
		/// </summary>	
		[
		CategoryAttribute("External")
		]					
		public string ButtonFileExtention {
			get { 
				object savedState = this.ViewState["ButtonFileExtention"];
				return (savedState == null) ? "gif" : (string) savedState;
			}
			set {
				ViewState["ButtonFileExtention"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the URL of the style sheet used in HTML mode.
		/// </summary>
		[
		CategoryAttribute("Behavior")
		]		
		public bool HtmlModeDefaultsToMonoSpaceFont {
			get {
				object savedState = this.ViewState["HtmlModeDefaultsToMonoSpaceFont"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {

				ViewState["HtmlModeDefaultsToMonoSpaceFont"] = value;
			}
		}

		#endregion

		#region Tab Styles
		/// <summary>
		/// Gets or sets the back color of the gutter of the tab area.
		/// </summary>
		[
		CategoryAttribute("Appearance")
		]		
		public Color GutterBackColor {
			get {
				object savedState = this.ViewState["GutterBackColor"];
				return (savedState == null) ? ColorTranslator.FromHtml("#81A9E2") : (Color) savedState;
			}
			set {
				ViewState["GutterBackColor"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the light color of the border of the Editor window.
		/// </summary>
		[
		CategoryAttribute("Appearance")
		]		
		public Color EditorBorderColorLight {
			get { 
				object savedState = this.ViewState["EditorBorderColorLight"];
				return (savedState == null) ? ColorTranslator.FromHtml("#808080") : (Color) savedState;
			}
			set {
				ViewState["EditorBorderColorLight"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the dark color of the border of the Editor window.
		/// </summary>		
		[
		CategoryAttribute("Appearance")
		]
		public Color EditorBorderColorDark {
			get { 
				object savedState = this.ViewState["EditorBorderColorDark"];
				return (savedState == null) ? ColorTranslator.FromHtml("#808080") : (Color) savedState;
			}
			set {
				ViewState["EditorBorderColorDark"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the light color of the border of the Editor window.
		/// </summary>
		[
		CategoryAttribute("Appearance")
		]		
		public Color GutterBorderColorLight {
			get { 
				object savedState = this.ViewState["GutterBorderColorLight"];
				return (savedState == null) ? ColorTranslator.FromHtml("#FFFFFF") : (Color) savedState;
			}
			set {
				ViewState["GutterBorderColorLight"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the dark color of the border of the Editor window.
		/// </summary>
		[
		CategoryAttribute("Appearance")
		]		
		public Color GutterBorderColorDark {
			get { 
				object savedState = this.ViewState["GutterBorderColorDark"];
				return (savedState == null) ? ColorTranslator.FromHtml("#808080") : (Color) savedState;
			}
			set {
				ViewState["GutterBorderColorDark"] = value;
			}
		}
		#endregion

		#region Downlevel
		/// <summary>
		/// Gets or sets the cols property of the downlevel TEXTAREA
		/// </summary>
		[
		CategoryAttribute("DownLevel")
		]		
		public int DownLevelCols {
			get {
				object savedState = this.ViewState["DownLevelCols"];
				return (savedState == null) ? 50 : (int) savedState;
			}
			set {
				ViewState["DownLevelCols"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the rows property of the downlevel TEXTAREA
		/// </summary>
		[
		CategoryAttribute("DownLevel")
		]		
		public int DownLevelRows {
			get {
				object savedState = this.ViewState["DownLevelRows"];
				return (savedState == null) ? 10 : (int) savedState;
			}
			set {
				ViewState["DownLevelRows"] = value;
			}
		}
		/// <summary>
		/// Gets or sets how FreeTextBox will render for downlevel browsers
		/// </summary>
		[
		CategoryAttribute("DownLevel")
		]		
		public DownLevelMode DownLevelMode {
			get {
				object savedState = this.ViewState["DownLevelMode"];
				return (savedState == null) ? DownLevelMode.TextArea : (DownLevelMode) savedState;
			}
			set {
				ViewState["DownLevelMode"] = value;
			}
		}
		/// <summary>
		/// Gets or sets how FreeTextBox will render for downlevel browsers
		/// </summary>
		[
		CategoryAttribute("DownLevel")
		]		
		public string DownLevelMessage {
			get {
				object savedState = this.ViewState["DownLevelMessage"];
				return (savedState == null) ? string.Empty : (string) savedState;
			}
			set {
				ViewState["DownLevelMessage"] = value;
			}
		}
		#endregion

		#region Depreciated
		/// <summary>
		/// Depreciated: (FTB 1.x property)
		/// </summary>
		[
		CategoryAttribute("Depreciated"),
		Obsolete("Please use the HtmlModeCssClass property")
		]				
		public string HtmlModeCss 
		{
			get 
			{ 
				object savedState = this.ViewState["HtmlModeCss"];
				return (savedState == null) ? "" : ResolveUrl((string) savedState);
			}
			set 
			{
				ViewState["HtmlModeCss"] = value;
			}
		}

		/// <summary>
		/// Depreciated: (FTB 1.x property)
		/// </summary>	
		[
		CategoryAttribute("Depreciated"),
		Obsolete("Unused FTB 1.x property.")
		]
		public bool ButtonOverImage 
		{
			get { 
				object savedState = this.ViewState["ButtonOverImage"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set {
				ViewState["ButtonOverImage"] = value;
			}
		}

		/// <summary>
		/// Depreciated: (FTB 1.x property)
		/// </summary>				
		[
		CategoryAttribute("Depreciated"),
		Obsolete("Unused FTB 1.x property.")
		]
		public bool ButtonDownImage 
		{
			get { 
				object savedState = this.ViewState["ButtonDownImage"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set {
				ViewState["ButtonDownImage"] = value;
			}
		}	
		/// <summary>
		/// Depreciated: (FTB 1.x property)
		/// </summary>
		[
		CategoryAttribute("Depreciated"),
		Obsolete("Please use the EnableHtmlMode property.")
		]	
		public bool AllowHtmlMode {
			get {
				object savedState = this.ViewState["AllowHtmlMode"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set {
				ViewState["AllowHtmlMode"] = value;
			}
		}
		/// <summary>
		/// Depreciated: (FTB 1.x property)
		/// </summary>
		[
		CategoryAttribute("Depreciated"),
		Obsolete("Unused FTB 1.x property.")
		]	
		public string AutoConfigure {
			get {
				object savedState = this.ViewState["AutoConfigure"];
				return (savedState == null) ? "" : (string) savedState;
			}
			set {
				ViewState["AutoConfigure"] = value;
			}
		}

		/// <summary>
		/// Depreciated: (FTB 1.x property)
		/// </summary>
		[
		CategoryAttribute("Depreciated"),
		Obsolete("Unused FTB 1.x property for DNN 2.x.")
		]	
		public string HelperFilesParameters {
			get {
				object savedState = this.ViewState["HelperFilesParameters"];
				return (savedState == null) ? "" : (string) savedState;
			}
			set {
				ViewState["HelperFilesParameters"] = value;
			}
		}
		/// <summary>
		/// Depreciated: (FTB 1.x property)
		/// </summary>
		[
		CategoryAttribute("Depreciated"),
		Obsolete("Unused FTB 1.x property for DNN 2.x.")
		]	
		public string HelperFilesPath {
			get {
				object savedState = this.ViewState["HelperFilesPath"];
				return (savedState == null) ? "" : (string) savedState;
			}
			set {
				ViewState["HelperFilesPath"] = value;
			}
		}
		/// <summary>
		/// Depreciated: (FTB 1.x property)
		/// </summary>
		[
		CategoryAttribute("Depreciated"),
		Obsolete("Unused FTB 1.x property.")
		]	
		public string ButtonPath {
			get {
				object savedState = this.ViewState["ButtonPath"];
				return (savedState == null) ? "" : (string) savedState;
			}
			set {
				ViewState["ButtonPath"] = value;
			}
		}
		/// <summary>
		/// Depreciated: (FTB 1.x property)
		/// </summary>
		[
		CategoryAttribute("Depreciated"),
		Obsolete("Unused FTB 1.x property.")
		]	
		public bool AutoHideToolbar {
			get { 
				object savedState = this.ViewState["AutoHideToolbar"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["AutoHideToolbar"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether toolbar items are generated from a string
		/// </summary>
		[
		CategoryAttribute("Toolbar")
		]
		#endregion
		
		#endregion

		#region State Management
		protected override void LoadViewState(object savedState) {

#if DEBUG
			System.Web.HttpContext.Current.Trace.Write("FreeTextBox","Loading ViewState");
#endif
			object[] state = null;

			if (savedState != null) {
				state = (object[]) savedState;

				base.LoadViewState(state[0]);
				if (state[1] != null) 
					((IStateManager) this.Toolbars).LoadViewState(state[1]);
				if (state[2] != null) 
					((IStateManager) this.ButtonStyle).LoadViewState(state[2]);
				if (state[3] != null) 
					((IStateManager) this.ButtonStyleActive).LoadViewState(state[3]);
			}
		}
		protected override object SaveViewState() {
#if DEBUG
			System.Web.HttpContext.Current.Trace.Write("FreeTextBox","Saving");
#endif		
			object[] state = new object[4];
			
			state[0] = base.SaveViewState();
			if (this.toolbars != null) 
				state[1] = ((IStateManager) this.toolbars).SaveViewState();
			if (this.buttonStyle != null) 
				state[2] = ((IStateManager) this.buttonStyle).SaveViewState();
			if (this.buttonStyleActive != null) 
				state[3] = ((IStateManager) this.buttonStyleActive).SaveViewState();


			return state;
			
		}
		
		protected override void TrackViewState() {
			base.TrackViewState ();
			if (toolbars != null)
				((IStateManager)toolbars).TrackViewState();

			if (buttonStyle != null)
				((IStateManager)buttonStyle).TrackViewState();
			
			if (buttonStyleActive != null)
				((IStateManager)buttonStyleActive).TrackViewState();
			
		}
		#endregion

		#region IPostBack Implimentation and Events
		/// <summary>
		/// Occurs when a Save button is pressed.
		/// </summary>
		public event EventHandler SaveClick;

		/// <summary>
		/// Event for Save button
		/// </summary>
		protected virtual void OnSaveClick(EventArgs e) {			
			if (SaveClick != null) {
				SaveClick(this, e);
			}
		}

		/// <summary>
		/// Event for Save button
		/// </summary>
		protected virtual void OnTextChanged(EventArgs e) {			
			if (TextChanged != null) {
				TextChanged(this, e);
			}
		}

		/// <summary>
		/// The event fired on post back that processes the HTML
		/// </summary>
		public event EventHandler ProcessText;

		/// <summary>
		/// The event fired on post back when the HTML in the editor has changed
		/// </summary>
		public event EventHandler TextChanged;

		/// <summary>
		/// Event for ProcessText handler
		/// </summary>
		protected virtual void OnProcessText(EventArgs e) {			
			if (ProcessText != null) {
				ProcessText(this, e);
			}
		}

		private void InternalProcessText(object source, EventArgs args) {
			FreeTextBox ftb = (FreeTextBox) source;
			string text = ftb.Text;
			
			Formatter formatter = new Formatter();
			if (ftb.RemoveServerNameFromUrls || ftb.ConvertHtmlSymbolsToHtmlCodes || ftb.RemoveScriptNameFromBookmarks) {
				try {
					string url = Page.Request.Url.AbsoluteUri.ToString();
					string serverPath = url.Substring(0, url.IndexOf(Page.Request.ServerVariables["HTTP_HOST"]) + Page.Request.ServerVariables["HTTP_HOST"].Length);
					
					if (ftb.RemoveScriptNameFromBookmarks) 
					{
						string server = Page.Request.ServerVariables["HTTP_HOST"];
						string script = Page.Request.ServerVariables["SCRIPT_NAME"];
						string qstring = Page.Request.QueryString.ToString();
						text = formatter.RemoveScriptNameFromBookmarks(text,url);	
						text = formatter.RemoveScriptNameFromBookmarks(text,url.Replace("&","&amp;"));	
					}


					if (ftb.RemoveServerNameFromUrls) 
						text = formatter.RemoveServerNameFromUrls(text,serverPath);
					if (ftb.ConvertHtmlSymbolsToHtmlCodes) 
						text = formatter.HtmlSymbolsToHtmlCodes(text);
				} catch {
					// catch designer error
				}
			}
			if (this.StripAllScripting)
				text = formatter.RemoveScriptTags(text); 

			// automatically do XHTML by default
			if (FormatHtmlTagsToXhtml && this.license.IsPro) {
				text = formatter.HtmlToXhtml(text); 
				if (this.StripAllScripting)
					text = formatter.RemoveJavaScriptEventsFromTags(text); 
			}
					

			ftb.Text = text;
		}

		public virtual void RaisePostBackEvent(string eventArgument) { 
			System.Web.HttpContext.Current.Trace.Write("PostBackEvent",eventArgument);
			
			switch (eventArgument) {
				case "Save":
					this.OnSaveClick(EventArgs.Empty);
					break;
				default:
					break;
			}
		}
		public void RaisePostDataChangedEvent() {
            OnTextChanged(EventArgs.Empty);

            OnProcessText(EventArgs.Empty);
		}
		public bool LoadPostData(String postDataKey, NameValueCollection values) {

            string PresentValue = this.Text;
            string PostedValue = values[this.ClientID];


			if (!PresentValue.Equals(PostedValue)) {				
				
				this.Text = PostedValue;
				viewStateText = PostedValue;
                               
				return true;
            
			}
			                      
            return false;            
		}
		#endregion

		#region StartUp & Initialization
		protected override void OnInit( EventArgs e ) {

            if (!this.DesignMode)
            {
                // Browser detection
                browserInfo = BrowserInfo.GetBrowserInfo();

                if (browserInfo != null && browserInfo.IsRichCapable  || this.RenderMode == RenderMode.Rich)
                {

                    // AutoGenerated Toolbars
                    if (AutoGenerateToolbarsFromString && !ToolbarsCreated)
                    {
                        // generate toolbars from string
                        ToolbarCollection toolbars = ToolbarGenerator.ToolbarsFromString(ToolbarLayout);

                        // insert the toolbars from the string before the procedurally defined toolbars
                        for (int i = toolbars.Count - 1; i > -1; i--)
                        {
                            Toolbars.Insert(0, toolbars[i]);
                        }

                        ToolbarsCreated = true;
                    }
                }
            }
			base.OnInit(e);
		}

		protected override void OnLoad( EventArgs e ) {
					
			base.OnLoad(e);
		}

		protected override void OnPreRender( EventArgs e ) {
            if (!this.DesignMode)
            {
                if (browserInfo != null && browserInfo.IsRichCapable)
                {

                    // get resource manager
                    resourceManager = new FreeTextBoxControls.Support.ResourceManager(this.Language, this.SupportFolder + "Languages/");

                    // attempt to set the localization to the selected language (for colors)
                    // TODO:
                    // Thread.CurrentThread.CurrentCulture = new CultureInfo(this.Language, false);

                    // fill default dropdownlists
                    foreach (Toolbar toolbar in Toolbars)
                    {
                        foreach (ToolbarItem toolbarItem in toolbar.Items)
                        {
                            if (toolbarItem is ToolbarDropDownList)
                            {
                                if (((ToolbarDropDownList)toolbarItem).IsBuiltIn && ((ToolbarDropDownList)toolbarItem).Items.Count == 0)
                                {
                                    Helper.PopulateDefaultDropDownList((ToolbarDropDownList)toolbarItem, this, resourceManager);
                                }
                            }
                            else
                            {
                                System.Web.HttpContext.Current.Trace.Write("ToolbarItems", toolbarItem.Title + ":" + toolbarItem.GetType());
                            }
                        }
                    }

                    SetupToolbarButtonStyles();
                }

                Page.RegisterRequiresPostBack(this);


                ClientScriptWrapper.RegisterRequiresPostBack(this.Page, this);
                // 1.x
                //Page.RegisterRequiresPostBack(this);

                // 2.0
                //Page.ClientScript.GetPostBackEventReference(this,"");

                RegisterClientScript();
            }
			base.OnPreRender(e);
		}

		private void SetupToolbarButtonStyles() {
			// Toolbar Style setup, if NotSet is not used, then the styles are set to Office styles
			if (ToolbarStyleConfiguration != ToolbarStyleConfiguration.NotSet) {
				
				SetButtonStyle(this.ButtonStyle, this.ToolbarStyleConfiguration, true);
				SetButtonStyle(this.ButtonStyleActive, this.ToolbarStyleConfiguration, false);
				
				switch (this.ToolbarStyleConfiguration) {
					default:
					case ToolbarStyleConfiguration.Office2003:
							
						this.ButtonSet = ToolbarStyleConfiguration.Office2003;
						this.ButtonFolder = "Images";
						this.BackColor = ColorTranslator.FromHtml("#9EBEF5");
						this.GutterBackColor = ColorTranslator.FromHtml("#81A9E2");
						this.ButtonOverImage = false;
						this.ButtonDownImage = false;
						this.ToolbarBackColor = Color.Transparent;
						this.ToolbarBackgroundImage = true;
						this.ButtonWidth=21;
						this.ButtonHeight=20;

						break;
					case ToolbarStyleConfiguration.OfficeXP:
						
						this.ButtonSet = ToolbarStyleConfiguration.OfficeXP;
						this.ButtonFolder = "OfficeXP";						
						this.BackColor = ColorTranslator.FromHtml("#D4D0C8");
						this.GutterBackColor = ColorTranslator.FromHtml("#BFBCB6");
						this.ButtonOverImage = true;
						this.ButtonDownImage = false;
						this.ToolbarBackColor = ColorTranslator.FromHtml("#DEDED6");
						this.ToolbarBackgroundImage = false;
						this.ButtonWidth=21;
						this.ButtonHeight=20;
						
						break;
					case ToolbarStyleConfiguration.Office2000:
						
						this.ButtonSet = ToolbarStyleConfiguration.Office2000;
						this.ButtonFolder = "Office2000";
						this.BackColor = ColorTranslator.FromHtml("#D4D0C8");
						this.GutterBackColor = ColorTranslator.FromHtml("#BFBCB6");	
						this.ButtonOverImage = false;
						this.ButtonDownImage = false;
						this.ToolbarBackColor = Color.Transparent;
						this.ToolbarBackgroundImage = false;
						this.ButtonWidth=21;
						this.ButtonHeight=20;
						
						

						break;
					case ToolbarStyleConfiguration.OfficeMac:
							
						this.ButtonSet = ToolbarStyleConfiguration.OfficeMac;
						this.ButtonFolder = "OfficeMac";
						
						this.BackColor=ColorTranslator.FromHtml("#e0dedd");
						this.GutterBackColor=ColorTranslator.FromHtml("#cccccc");
						this.ButtonWidth=26;
						this.ButtonHeight=26;


						break;
				}
			}
		}

		private void SetButtonStyle(ToolbarButtonStyle style, ToolbarStyleConfiguration toolbarStyle, bool normal) {
			switch (toolbarStyle) {
				default:
				case ToolbarStyleConfiguration.Office2003: 
					if (normal) {
						style.UseOverBackgroundImage = true;
						style.UseBackgroundImage = true;
						style.BackColor = Color.Transparent;					
						style.BorderColorLight = Color.Transparent;
						style.BorderColorDark = Color.Transparent;

						style.OverBackColor = ColorTranslator.FromHtml("#FFF4CC");
						style.OverBackColorGradient = ColorTranslator.FromHtml("#FFD091");
						style.OverBorderColorLight = ColorTranslator.FromHtml("#000080");
						style.OverBorderColorDark = ColorTranslator.FromHtml("#000080");
						
						style.DownBackColor = Color.Transparent;				
						style.DownBorderColorLight = ColorTranslator.FromHtml("#000080");
						style.DownBorderColorDark = ColorTranslator.FromHtml("#000080");
					} else {
						style.UseOverBackgroundImage = true;
						style.BackColor = ColorTranslator.FromHtml("#FFD58C");
						style.BackColorGradient = ColorTranslator.FromHtml("#FFAD55");
						style.BorderColorLight = ColorTranslator.FromHtml("#000080");
						style.BorderColorDark = ColorTranslator.FromHtml("#000080");
						
						style.OverBackColor = ColorTranslator.FromHtml("#FE914E");
						style.OverBackColorGradient = ColorTranslator.FromHtml("#FFD38E");
						style.OverBorderColorLight = ColorTranslator.FromHtml("#000080");
						style.OverBorderColorDark = ColorTranslator.FromHtml("#000080");
						
						style.DownBackColor = Color.Transparent;					
						style.DownBorderColorLight = ColorTranslator.FromHtml("#000080");
						style.DownBorderColorDark = ColorTranslator.FromHtml("#000080");
					}
					break;
				case ToolbarStyleConfiguration.OfficeXP: 
					if (normal) {
						style.UseBackgroundImage = false;
						style.UseOverBackgroundImage = false;

						style.BackColor =  Color.Transparent;
						style.BorderColorLight = Color.Transparent;
						style.BorderColorDark = Color.Transparent;
						
						style.OverBackColor = ColorTranslator.FromHtml("#B5BDD6");
						style.OverBorderColorLight = ColorTranslator.FromHtml("#3169C6");
						style.OverBorderColorDark = ColorTranslator.FromHtml("#3169C6");
						
						style.DownBackColor = ColorTranslator.FromHtml("#8592B5");
						style.DownBorderColorLight = ColorTranslator.FromHtml("#3169C6");
						style.DownBorderColorDark = ColorTranslator.FromHtml("#3169C6");
					} else {
						style.UseBackgroundImage = false;
						style.UseOverBackgroundImage = false;

						style.BackColor = ColorTranslator.FromHtml("#D6D6DE");
						style.BorderColorLight = ColorTranslator.FromHtml("#3169C6");
						style.BorderColorDark = ColorTranslator.FromHtml("#3169C6");
						
						style.OverBackColor = ColorTranslator.FromHtml("#8494B5");
						style.OverBorderColorLight = ColorTranslator.FromHtml("#3169C6");
						style.OverBorderColorDark = ColorTranslator.FromHtml("#3169C6");
						
						style.DownBackColor = ColorTranslator.FromHtml("#8494B5");
						style.DownBorderColorLight = ColorTranslator.FromHtml("#3169C6");
						style.DownBorderColorDark = ColorTranslator.FromHtml("#3169C6");
					}
					break;		
				case ToolbarStyleConfiguration.Office2000: 
					if (normal) {
						style.UseBackgroundImage = false;
						style.UseOverBackgroundImage = false;

						style.OverBackColor = ColorTranslator.FromHtml("#D4D0C8");
						style.OverBorderColorLight = ColorTranslator.FromHtml("#FFFFFF");
						style.OverBorderColorDark = ColorTranslator.FromHtml("#808080");
						
						style.DownBackColor = ColorTranslator.FromHtml("#D4D0C8");
						style.DownBorderColorLight = ColorTranslator.FromHtml("#808080");
						style.DownBorderColorDark = ColorTranslator.FromHtml("#FFFFFF");
					
					} else {
						style.UseBackgroundImage = false;
						style.UseOverBackgroundImage = false;

						style.BackColor = Color.Transparent;
						style.BorderColorLight = ColorTranslator.FromHtml("#808080");
						style.BorderColorDark = ColorTranslator.FromHtml("#808080");

						style.OverBackColor = ColorTranslator.FromHtml("#D4D0C8");
						style.OverBorderColorLight = ColorTranslator.FromHtml("#FFFFFF");
						style.OverBorderColorDark = ColorTranslator.FromHtml("#808080");
						
						style.DownBackColor = ColorTranslator.FromHtml("#D4D0C8");
						style.DownBorderColorLight = ColorTranslator.FromHtml("#808080");
						style.DownBorderColorDark = ColorTranslator.FromHtml("#FFFFFF");					}
					break;
				case ToolbarStyleConfiguration.OfficeMac:
					if (normal) {
						style.UseBackgroundImage = false;
						style.UseOverBackgroundImage = false;

						style.UseBackgroundImage=true;
						style.UseOverBackgroundImage=true;

						style.OverBackColor=Color.Transparent;
						style.OverBackColorGradient=Color.Transparent;
						style.OverBorderColorLight=Color.Transparent;	
						style.OverBorderColorDark=Color.Transparent;
					} else {

						style.UseBackgroundImage=true;
						style.UseOverBackgroundImage=true;
						
						style.BackColor=Color.Transparent;
						style.BackColorGradient=Color.Transparent;
						style.BorderColorLight=Color.Transparent;
						style.BorderColorDark=Color.Transparent;

						style.OverBorderColorLight=Color.Transparent;
						style.OverBorderColorDark=Color.Transparent;
						style.OverBackColor=Color.Transparent;
						style.OverBackColorGradient=Color.Transparent;
					}
					break;
			}
		}

		private string RepeatString(string input, int times) {
			string output = "";
			for (int i=1; i<=times; i++) output += input;
			return output;
		}

		protected virtual void RegisterClientScript() {
			
			if (this.RenderMode == RenderMode.Rich || (this.RenderMode == RenderMode.NotSet && this.browserInfo.IsRichCapable)) {
				


				// 1.x
				//this.Page.RegisterOnSubmitStatement(this.ClientID + "_OnSubmit","FTB_API['" + this.ClientID + @"'].StoreHtml();");

				// 2.0
				//this.Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), this.ClientID + "_OnSubmit", "FTB_API['" + this.ClientID + @"'].StoreHtml();");
			
				// Main Script block
				// 1.x
				//if (!this.Page.IsClientScriptBlockRegistered("FTB-Scripts")) {
				
				// 2.0
				//if (!this.Page.ClientScript.IsClientScriptBlockRegistered("FTB-Scripts")) {

                ClientScriptWrapper.RegisterOnSubmitStatement(this.Page, this.GetType(), this.ClientID + "_OnSubmit", "FTB_API['" + this.ClientID + @"'].StoreHtml();");

                if (!ClientScriptWrapper.IsClientScriptBlockRegistered(this.Page, "FTB-Scripts"))
                {
					string ftbScripts = @"<script type=""text/javascript"" src=""" + CreateResourceString("FTB-Utility.js", ResourceType.JavaScript) + @"""></script>
<script type=""text/javascript"" src=""" + CreateResourceString("FTB-FreeTextBox.js", ResourceType.JavaScript) + @"""></script>
<script type=""text/javascript"" src=""" + CreateResourceString("FTB-ToolbarItems.js", ResourceType.JavaScript) + @"""></script>";
					
					if (this.license.IsPro) 
						ftbScripts += @"<script type=""text/javascript"" src=""" + CreateResourceString("FTB-Pro.js", ResourceType.JavaScript) + @"""></script>";

					// 1.x
					//this.Page.RegisterClientScriptBlock("FTB-Scripts",ftbScripts);

					// 2.0
					//this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FTB-Scripts", ftbScripts);
                    ClientScriptWrapper.RegisterClientScriptBlock(this.Page, this, "FTB-Scripts", ftbScripts);
				}
			
				// 1.x
				//if (!this.Page.IsClientScriptBlockRegistered("FreeTextBoxInfo")) {

				// 2.0
				//if (!this.Page.ClientScript.IsClientScriptBlockRegistered("FreeTextBoxInfo")) {

                if (!ClientScriptWrapper.IsClientScriptBlockRegistered(this.Page, "FreeTextBoxInfo"))
                {
					ArrayList ftbInfo = new ArrayList();
					
					
					string assemblyName = typeof(FreeTextBox).Assembly.FullName;
					string versionNumber = assemblyName.Substring(assemblyName.IndexOf("=")+1);
					versionNumber = versionNumber.Substring(0,versionNumber.IndexOf(","));
					
					// add the strings
					ftbInfo.Add("FreeTextBox v3 (" + versionNumber + ")");					
					ftbInfo.Add("http://www.freetextbox.com/");
					ftbInfo.Add("ASP.NET HTML editor for PC/IE & Mozilla");
					ftbInfo.Add("License Type: " + license.LicenseKey + " (To: " + license.Data + ")");
					
					// check the max length needed
					int maxLength = 0;
					foreach (string s in ftbInfo) {
						if (s.Length > maxLength) maxLength = s.Length;
					}
					maxLength += 5;
					
					// write the strings
					string info = @"
<!-- **" + this.RepeatString("*",maxLength) + @"* -->";
					foreach (string s in ftbInfo) {
						info += @"
<!-- * " + s + this.RepeatString(" ", maxLength - s.Length) + @"* -->";
					}

					info += @"
<!-- **" + this.RepeatString("*",maxLength) + @"* -->
";
					// 1.x
					//this.Page.RegisterClientScriptBlock("FreeTextBoxInfo",info);

					// 2.0
					//this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FreeTextBoxInfo", info);

					if (!MsAjaxProxy.Current.IsScriptManagerInAsyncPostBack(this.Page))
						ClientScriptWrapper.RegisterClientScriptBlock(this.Page, this, "FreeTextBoxInfo", info);
				}

		
                if (!ClientScriptWrapper.IsStartupScriptRegistered(this.Page, "FreeTextBox_" + this.ClientID + "_Startup")) // && !MsAjaxProxy.Current.IsScriptManagerInAsyncPostBack(this.Page))
                {
                    string startupScript = CreateStartupString();

                    startupScript = @"
<script type=""text/javascript"">
if (window.FTB_AddEvent) { 
	" + ((!MsAjaxProxy.Current.IsScriptManagerInAsyncPostBack(this.Page)) ? "FTB_AddEvent(window,'load',function () {" : "") + @"
    " + startupScript + @"
	" + ((!MsAjaxProxy.Current.IsScriptManagerInAsyncPostBack(this.Page)) ? "});" : "") + @"
} else {
";
            string errorMessage = @"FreeTextBox has not been correctly installed. To install FreeTextBox either:\n (1) add a reference to FtbWebResource.axd in web.config:\n<system.web>\n<httpHandlers>\n<add verb=""GET""\npath=""FtbWebResource.axd""\ntype=""FreeTextBoxControls.AssemblyResourceHandler, FreeTextBox"" />\n</httpHandlers>\n</system.web>\n\n(2) Save the FreeTextBox image and javascript files to a location on your website and set up FreeTextBox as follows \n<FTB:FreeTextBox id=""FreeTextBox1"" SupportFolder=""ftbfileslocation"" JavaScriptLocation=""ExternalFile"" ButtonImagesLocation=""ExternalFile"" ToolbarImagesLocation=""ExternalFile"" ButtonImagesLocation=""ExternalFile"" runat=""server"" />";

            switch (this.InstallationErrorMessage)
            {
                default:
                case InstallationErrorMessage.JavaScriptAlert:
                    startupScript += "alert('" + errorMessage + "');";
                    break;
                case InstallationErrorMessage.InlineMessage:
                    startupScript += ((this.browserInfo.IsIE) ? "ed = eval('" + this.ClientID + "_designEditor');" : "ed = document.getElementById('" + this.ClientID + "_designEditor').contentWindow;") + "ed.document.open(); ed.document.write('" + this.Page.Server.HtmlEncode(errorMessage).Replace(@"\n", "<br>") + "');ed.document.close();";
                    break;
                case InstallationErrorMessage.None:
                    // do nothing!
                    break;
            }

            startupScript += @"
}
</script>";


                    ClientScriptWrapper.RegisterStartupScript(this.Page, this.GetType(), "FreeTextBox_" + this.ClientID + "_Startup", startupScript);
				}		
			}
		
		}

        private string CreateStartupString()
        {

            // setup FTB_Button & FTB_DropDownList declarations
            ArrayList buttonList = new ArrayList();
            ArrayList dropdownlistList = new ArrayList();

            if (this.EnableToolbars)
            {
                foreach (Toolbar toolbar in this.Toolbars)
                {
                    foreach (ToolbarItem toolbarItem in toolbar.Items)
                    {

                        if (this.license.IsPro || !toolbarItem.isProFeature)
                        {

                            if (toolbarItem is ToolbarButton)
                            {
                                ToolbarButton button = (ToolbarButton)toolbarItem;
                                string buttonDeclaration = "new FTB_Button('" + CreateUniqueToolbarItemID(toolbar, toolbarItem) + "','" + button.CommandIdentifier + "'";

                                // action javascript
                                if (button.BuiltInScript != string.Empty)
                                    buttonDeclaration += ",function() { " + button.BuiltInScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else if (button.ScriptBlock != string.Empty)
                                    buttonDeclaration += ",function() { " + button.ScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else
                                    buttonDeclaration += ",null";

                                // state javascript
                                if (button.BuiltInStateScript != string.Empty)
                                    buttonDeclaration += ",function() { " + button.BuiltInStateScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else if (button.StateScriptBlock != string.Empty)
                                    buttonDeclaration += ",function() { " + button.StateScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else
                                    buttonDeclaration += ",null";

                                // HTML mode enabled
                                buttonDeclaration += "," + button.htmlModeEnabled.ToString().ToLower();

                                // enabled javascript
                                if (button.BuiltInEnabledScript != string.Empty)
                                    buttonDeclaration += ",function() { " + button.BuiltInEnabledScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else if (button.EnabledScriptBlock != string.Empty)
                                    buttonDeclaration += ",function() { " + button.EnabledScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else
                                    buttonDeclaration += ",null";

                                //buttonDeclaration += ",FTB_BUTTON_" + this.ButtonRenderMode.ToString().ToUpper();
                                buttonDeclaration += @")";
                                buttonList.Add(buttonDeclaration);

                            }
                            else if (toolbarItem is ToolbarDropDownList)
                            {
                                ToolbarDropDownList dropDownList = (ToolbarDropDownList)toolbarItem;
                                string ddlDeclaration = "new FTB_DropDownList('" + CreateUniqueToolbarItemID(toolbar, toolbarItem) + "','" + dropDownList.CommandIdentifier + "'";

                                if (dropDownList.BuiltInScript != string.Empty)
                                    ddlDeclaration += ",function() { " + dropDownList.BuiltInScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else if (dropDownList.ScriptBlock != string.Empty)
                                    ddlDeclaration += ",function() { " + dropDownList.ScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else
                                    ddlDeclaration += ",null";

                                // state javascript
                                if (dropDownList.BuiltInStateScript != string.Empty)
                                    ddlDeclaration += ",function() { " + dropDownList.BuiltInStateScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else if (dropDownList.StateScriptBlock != string.Empty)
                                    ddlDeclaration += ",function() { " + dropDownList.StateScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else
                                    ddlDeclaration += ",null";

                                // enabled javascript
                                if (dropDownList.BuiltInEnabledScript != string.Empty)
                                    ddlDeclaration += ",function() { " + dropDownList.BuiltInEnabledScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else if (dropDownList.EnabledScriptBlock != string.Empty)
                                    ddlDeclaration += ",function() { " + dropDownList.EnabledScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
                                else
                                    ddlDeclaration += ",null";


                                ddlDeclaration += ")";
                                dropdownlistList.Add(ddlDeclaration);

                            }
                        }
                    }
                }
            }


            string startupScript = @"
        FTB_API['" + this.ClientID + @"'] = new FTB_FreeTextBox('" + this.ClientID + @"',
					" + this.EnableToolbars.ToString().ToLower() + @",
					" + this.ReadOnly.ToString().ToLower() + @",
					" + ((this.EnableToolbars) ? @"new Array(
						" + String.Join(",\n\t\t\t", ((string[])buttonList.ToArray(typeof(string)))) + @"
					)" : "null") + @",
					" + ((this.EnableToolbars) ? @"new Array(
						" + String.Join(",\n\t\t\t", ((string[])dropdownlistList.ToArray(typeof(string)))) + @"
					)" : "null") + @",				
					" + ((this.BreakMode == BreakMode.Paragraph) ? "FTB_BREAK_P" : "FTB_BREAK_BR") + @",
					FTB_PASTE_" + this.PasteMode.ToString().ToUpper() + @",
					FTB_TAB_" + this.TabMode.ToString().ToUpper() + @",
					FTB_MODE_" + ((this.StartMode == EditorMode.DesignMode) ? "DESIGN" : "HTML") + @",									
					" + ((this.ClientSideTextChanged != string.Empty) ? this.ClientSideTextChanged : "null") + @",
					'" + this.DesignModeCss + @"',
					'" + this.DesignModeBodyTagCssClass + @"',
					'" + this.BaseUrl + @"',
					'" + ((this.TextDirection == TextDirection.RightToLeft) ? "rtl" : "") + @"',
					'',
					'" + this.ImageGalleryUrl + @"',
					'" + this.ImageGalleryPath + @"',
					" + this.Focus.ToString().ToLower() + @",
					" + this.ButtonWidth + @",
					" + this.ButtonHeight + @"
					";
            startupScript += @"
				);";

            return startupScript;
        }

		/// <summary>
		/// Static function to determine if the client's browser is capable of displaying the rich editor.
		/// </summary>
		public static bool IsRichCapable(HttpContext context) {
			BrowserInfo browserInfo = BrowserInfo.GetBrowserInfo();			
			return browserInfo.IsRichCapable;
		}
		#endregion

		#region Render Functions
		protected override void Render(HtmlTextWriter writer) {
			if (browserInfo == null)
				return;
			
			// if RenderMode == NotSet then the decision is based on IsRichCapable
			if ((browserInfo.IsRichCapable && RenderMode != RenderMode.Plain) || RenderMode == RenderMode.Rich) {
				RenderRichEditor(writer);
			} else {
				RenderDownLevel(writer);
			}
		}
		
		#region Main Rendering
		protected virtual void RenderDownLevel(HtmlTextWriter writer) {
			switch (this.DownLevelMode) {
				default:
				case DownLevelMode.TextArea:
					writer.WriteLine(@"<textarea id=""" + this.ClientID + @""" name=""" + this.ClientID + @"""" + ((this.HtmlModeCssClass != string.Empty) ? "class=\"" + this.HtmlModeCssClass + "\"" : "") + @" cols=""" + this.DownLevelCols.ToString() + @""" rows=""" + this.DownLevelRows.ToString() + @"""" + ((this.ReadOnly) ? @" readonly=""readonly"" disabled=""true"" onFocus=""this.blur();""" : "") + @">" + this.Text + "</textarea>");			
					break;
				case DownLevelMode.InlineHtml:
					writer.WriteLine(@"<input type=""hidden"" id=""" + this.ClientID + @""" name=""" + this.ClientID + @""" value=""" + HttpContext.Current.Server.HtmlEncode(this.Text) + @""" />");
					writer.WriteLine(this.Text);
					break;
				case DownLevelMode.Message:
					writer.WriteLine(@"<input type=""hidden"" id=""" + this.ClientID + @""" name=""" + this.ClientID + @""" value=""" + HttpContext.Current.Server.HtmlEncode(this.Text) + @""" />");
					writer.WriteLine(this.DownLevelMessage);
					break;
			}
		}

		protected virtual void RenderRichEditor(HtmlTextWriter writer) {
			
			hasToolbars = ((toolbars != null) && (toolbars.Count > 0));
			
			writer.WriteLine(@"<style type=""text/css"">");
			RenderEditorStyles(writer);
			RenderButtonStyles(writer);
			RenderTabStyles(writer);
			writer.WriteLine(@"</style>");
			
			// main outer table
			//
			writer.WriteLine(@"<table cellpadding=""2"" cellspacing=""0"" class=""" + this.ClientID + @"_OuterTable""><tr><td>");

			writer.WriteLine(@"<div>");
			
			// toolbararea
			//
			
			if (this.hasToolbars && this.EnableToolbars) {				
				writer.WriteLine(@"<div id=""" + this.ClientID + @"_toolbarArea"" style=""padding-bottom:2px;clear:both;"">");
				
				foreach(Toolbar toolbar in Toolbars) {
					toolbar.RepeatDirection = RepeatDirection.Horizontal;
					RenderToolbar(writer,toolbar);
				}

				writer.WriteLine(@"</div>");
			}
			
			// weird width corrections for IFRAME/TEXTAREA in IE
			string htmlWidth = this.Width.ToString();
			string htmlHeight = this.Height.ToString();
			string iframeWidth = this.Width.ToString();
			string iframeHeight = this.Height.ToString();
			
			if (this.browserInfo.IsIE) {
				if (this.Width.ToString().IndexOf("%") == -1) {
					iframeWidth = (this.Width.Value - 2).ToString() + "px";
				}
				if (this.Height.ToString().IndexOf("%") == -1) {
					iframeHeight = (this.Height.Value - 1).ToString() + "px";
				}
			}

			// editor frames
			//
			writer.WriteLine(@"
	<div id=""" + this.ClientID + @"_designEditorArea"" style=""clear:both;padding-top:1px;"">
	    <iframe id=""" + this.ClientID + @"_designEditor"" style=""padding: 0px; width:" + iframeWidth + @"; height: " + iframeHeight + @";"" src=""" + ((this.EnableSsl) ? this.SslUrl.ToString() : "about:blank") + @""" class=""" + this.ClientID + @"_DesignBox""></iframe>
	</div>
	<div id=""" + this.ClientID + @"_htmlEditorArea"" style=""clear:both;display:none;padding-bottom:1px;"">
	    <textarea id=""" + this.ClientID + @""" name=""" + this.ClientID + @""" disabled=""disabled"" style=""padding: 0px; width:" + htmlWidth + @"; height: " + htmlHeight + @";" + ( (this.TextDirection == TextDirection.RightToLeft) ? @"direction:rtl;" : "") + @""" class=""" + ((this.HtmlModeCssClass == string.Empty) ? this.ClientID + @"_HtmlBox" : this.HtmlModeCssClass) + @""">" + this.Page.Server.HtmlEncode(this.Text) + @"</textarea>
	</div>
	<div id=""" + this.ClientID + @"_previewPaneArea"" style=""clear:both;display:none;padding-bottom:1px;"">
	    <iframe id=""" + this.ClientID + @"_previewPane"" style=""padding: 0px; width:" + iframeWidth + @"; height: " + iframeHeight + @""" src=""" + ((this.EnableSsl) ? this.SslUrl.ToString() : "about:blank") + @""" class=""" + this.ClientID + @"_DesignBox""></iframe>
	</div>
");
			
			// tabs
			//
			if (EnableHtmlMode) {
				writer.WriteLine(@"<div style=""clear:both;padding-top:2px;"">");
				RenderTabs(writer);
				writer.WriteLine(@"</div>");
			}
			
			// inner main table
			writer.WriteLine("</div>");	
			
			// outer table
			writer.WriteLine("</table>");

            /*
            if (MsAjaxProxy.Current.IsScriptManagerInAsyncPostBack(this.Page))
            {
                writer.Write("<script type=\"text/javascript\">" + CreateStartupString() + "</script>");
            }

            writer.Write("ajax: " + MsAjaxProxy.Current.IsScriptManagerOnPage(this.Page).ToString() + ", ");
            writer.Write("async: " + MsAjaxProxy.Current.IsScriptManagerInAsyncPostBack(this.Page).ToString());
            */
		}

		protected virtual void RenderTabs(HtmlTextWriter writer) {
			// Status/HTML Table
			string DesignModeStyle = this.ClientID + "_TabOn";
			string HtmlModeStyle = this.ClientID + "_TabOffRight";
			string StartTab = this.ClientID + "_StartTabOn";
			
			if (this.StartMode == EditorMode.HtmlMode) {
				StartTab = this.ClientID + "_StartTabOff";
				DesignModeStyle = this.ClientID + "_TabOffLeft";
				HtmlModeStyle = this.ClientID + "_TabOn";
			}

			writer.WriteLine(@"
	<table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""border-collapse:collapse;"">
		<tr id=""" + this.ClientID + @"_TabRow"">
			<td class=""" + StartTab + @""">
				&nbsp;
			</td>
			<td class=""" + DesignModeStyle + @""" id=""" + this.ClientID + @"_designModeTab"" unselectable=""on"">
				<nobr><img unselectable=""on"" src=""" + CreateResourceString("mode.design", ResourceType.Button) + @""" align=""absmiddle"" width=""21"" height=""20"">&nbsp;" + resourceManager.GetString("DesignModeTab") + @"</nobr>
			</td>
			<td class=""" + HtmlModeStyle + @""" ID=""" + this.ClientID + @"_htmlModeTab"" unselectable=""on"">
				<nobr><img unselectable=""on"" SRC=""" + CreateResourceString("mode.html", ResourceType.Button) + @""" align=""absmiddle"" width=""21"" height=""20"">&nbsp;" + resourceManager.GetString("HtmlModeTab") + @"</nobr>
			</td>
			<td class=""" + this.ClientID + @"_EndTab"">
				<div id=""" + this.ClientID + @"_AncestorArea"" class=""" + this.ClientID + @"_AncestorArea""></div>
			</td>
		</tr>
	</table>
	");

			/*
			<td class=""" + HtmlModeStyle + @""" ID=""" + this.ClientID + @"_previewModeTab"" unselectable=""on"">
				<nobr><img unselectable=""on"" SRC=""" + CreateResourceString("mode.html", ResourceType.Button) + @""" ALIGN=""absmiddle"" width=21 height=20>&nbsp;" + resourceManager.GetString("HtmlModeTab") + @"</nobr>
			</td>
						 
			 */
		}
		
		#endregion

		#region Toolbar Rendering
		public virtual void RenderToolbar(HtmlTextWriter writer, Toolbar toolbar) {
			
			// floating div
			//
			writer.WriteLine(@"<div class=""" + this.ClientID + @"_Toolbar"">");
			
			// table to hold buttons
			//
			writer.Write(@"<table border=""0"" cellpadding=""0"" cellspacing=""0"" " + ((this.ToolbarBackgroundImage) ? @"style=""background-image:url(" + CreateResourceString("toolbar." + RepeatDirection.Horizontal.ToString().ToLower()+ ".background", ResourceType.Toolbar) + @");""" : "") + @"><tr><td border=""0"" unselectable=""on"">");
			writer.Write(@"<img src=""" + CreateResourceString("toolbar." + toolbar.RepeatDirection.ToString().ToLower() + ".start", ResourceType.Toolbar) + @""" align=""center"" />");
			if (toolbar.RepeatDirection == RepeatDirection.Horizontal) writer.Write("</td><td>");
			

			writer.Write(@"<table cellpadding=""0"" cellspacing=""0"" border=""0"" class=""" + this.ClientID + @"_items"">");				
			if (toolbar.RepeatDirection == RepeatDirection.Horizontal) writer.Write("<tr>");
			foreach(ToolbarItem toolbarItem in toolbar.Items) {
				// check for pro status
				if(this.license.IsPro || !toolbarItem.isProFeature) {
				
					if (toolbarItem is ToolbarButton) {
						ToolbarButton toolbarButton = (ToolbarButton) toolbarItem;
						RenderToolbarButton(writer, toolbar, toolbarButton);

					} else if (toolbarItem is ToolbarDropDownList) {
						ToolbarDropDownList toolbarDropDownList = (ToolbarDropDownList) toolbarItem;
						RenderToolbarDropDownList(writer,toolbar , toolbarDropDownList);
							
					} else if (toolbarItem is ToolbarSeparator) {
						RenderToolbarSeparator(writer,toolbar);
					}
				}
			}		
			if (toolbar.RepeatDirection == RepeatDirection.Horizontal) writer.Write("</tr>");
			writer.Write(@"</table>");


			if (toolbar.RepeatDirection == RepeatDirection.Horizontal) writer.Write("</td><td>");
			writer.Write(@"<img src=""" + CreateResourceString("toolbar." + toolbar.RepeatDirection.ToString().ToLower() + ".end", ResourceType.Toolbar) + @""" border=""0"" unselectable=""on"" align=""center"" />");
			writer.Write(@"</td></tr></table>");
			writer.Write(@"</div>");
		} 

		public virtual void RenderToolbarButton(HtmlTextWriter writer, Toolbar toolbar, ToolbarButton toolbarButton) {
			if (toolbarButton.IsBuiltIn) 
				SetToolbarButtonLanguage(toolbarButton);
			
			if (toolbar.RepeatDirection == RepeatDirection.Vertical) writer.WriteLine("<tr>");

			
			//if (this.ButtonRenderMode == ButtonRenderMode.StyledBackgrounds) {

				writer.Write(@"<td id=""" + CreateUniqueToolbarItemID(toolbar, toolbarButton) + @""" class=""" + this.ClientID + @"_Button_Off_Out"">");
				if (toolbarButton.isBuiltIn) {
					// resourced image				
					writer.Write(@"<img src=""" + CreateResourceString(toolbarButton.ButtonImage, ResourceType.Button) + @""" border=""0"" title=""" + toolbarButton.Title + @""" unselectable=""on"" width=""" + this.ButtonWidth + @""" height=""" + this.ButtonHeight + @""" tabindex=""-1"" align=""center"" />");
				} else {
					// use normal linked image in the case of a non-builtin button
					writer.Write(@"<img src=""" + this.SupportFolder + this.ButtonFolder + @"/" + toolbarButton.ButtonImage + "." + this.ButtonFileExtention + @""" border=""0"" title=""" + toolbarButton.Title + @""" unselectable=""on"" width=""" + this.ButtonWidth + @""" height=""" + this.ButtonHeight + @""" tabindex=""-1"" align=""center"" />");
				}
				writer.Write("</td>\n");
			/*
			} else {
			
				writer.Write(@"<td id=""" + this.ClientID + "_" + toolbarButton.ClientID.Replace(" ","") + @""" class=""" + this.ClientID + @"_Button_Off_Normal"" style=""width:" + (this.ButtonWidth+1).ToString() + @"px"">" + "\n");
				if (toolbarButton.isBuiltIn) {
					// resourced image
				
					//writer.Write(@"<img src=""" + CreateResourceString(toolbarButton.ButtonImage, ResourceType.Button) + @""" border=0 title=""" + toolbarButton.Title + @""" unselectable=""on"" width=""" + this.ButtonWidth + @""" height=""" + this.ButtonHeight + @""" tabindex=""-1"">");
				
					// attempt to use a single image with CSS. IE chokes on this!!
					writer.Write(@"<div style=""position:absolute; z-index: 0;""><img src=""" + CreateResourceString("spacer", ResourceType.Button) + @""" width=""" + (this.ButtonWidth+1).ToString() + @""" height=""" + (this.ButtonHeight+1).ToString() + @""" /></div>" + "\n");
					writer.Write(@"<div style=""position:relative; z-index: 1; padding-left:1px;"">");
					writer.Write(@"<img src=""" + CreateResourceString("spacer", ResourceType.Button) + @""" style=""background: transparent url(" + CreateResourceString("buttons", ResourceType.Button) + ") -" + (toolbarButton.BuiltInButtonOffset * this.ButtonWidth).ToString() + @"px 0 no-repeat;""  border=""0"" alt=""" + toolbarButton.Title + @""" unselectable=""on"" width=""" + this.ButtonWidth + @""" height=""" + this.ButtonHeight + @""" tabindex=""-1"">");
					writer.Write(@"</div>" + "\n");
				
				
				} else {
					// use normal linked image in the case of a non-builtin button
					writer.Write(@"<img src=""" + this.SupportFolder + this.ButtonFolder + @"/" + toolbarButton.ButtonImage + "." + this.ButtonFileExtention + @""" border=0 title=""" + toolbarButton.Title + @""" unselectable=""on"" width=""" + this.ButtonWidth + @""" height=""" + this.ButtonHeight + @""" tabindex=""-1"">");				
				}
				writer.Write("</td>\n");

			}
			*/

			if (toolbar.RepeatDirection == RepeatDirection.Vertical) writer.Write("</tr>");
		}

		public virtual void SetToolbarButtonLanguage(ToolbarButton toolbarButton) {
			if (!toolbarButton.TitleHasBeenSet) {
				toolbarButton.SetTitleLanguage(resourceManager.GetString(toolbarButton.className));
			}
		}

		public virtual void RenderToolbarDropDownList(HtmlTextWriter writer, Toolbar toolbar, ToolbarDropDownList toolbarDropDownList) {

			if (toolbarDropDownList.IsBuiltIn) 
				SetToolbarDropDownListLanguage(toolbarDropDownList);
			
			if (toolbar.RepeatDirection == RepeatDirection.Vertical) writer.WriteLine("<tr>");
			writer.Write(@"<td style=""padding-left:4px;"" unselectable=""on"">");

			writer.Write(@"<select id=""" + CreateUniqueToolbarItemID(toolbar, toolbarDropDownList) + @"""" + 
				@" TabIndex=""-1"" " + 
				( (DropDownListCssClass != string.Empty) ? " class=\"" + DropDownListCssClass + "\"" : "") +
				( (this.TextDirection == TextDirection.RightToLeft) ? @"style=""direction:rtl;""" : "") +
				">");

			writer.Write(@"<option value="""">" + toolbarDropDownList.Title + @"</option>");

			foreach (ToolbarListItem toolbarListItem in toolbarDropDownList.Items) {
				if (toolbarDropDownList is ParagraphMenu) {
					writer.Write(@"<option value=""" + toolbarListItem.Value + @"""");
				} else {
					writer.Write(@"<option value=""" + this.Page.Server.HtmlEncode(toolbarListItem.Value) + @"""");
				}
				
				if (toolbarListItem.BackColor != Color.Transparent) {
					writer.Write(" style=\"background-color: " + ColorTranslator.ToHtml(toolbarListItem.BackColor) + ";");
					if (toolbarListItem.BackColor.GetBrightness() < .4) writer.Write("color: #FFFFFF;");
					writer.Write(@"""");
				}
				
				writer.Write(">" + toolbarListItem.Text + "</option>\n");
			}

			writer.Write(@"</select>");
			writer.Write(@"</td>");
			if (toolbar.RepeatDirection == RepeatDirection.Vertical) writer.Write("</tr>");
		}

		private void SetToolbarDropDownListLanguage(ToolbarDropDownList toolbarDropDownList) {
			// TODO: see if it's already not the default language
			if (!toolbarDropDownList.TitleHasBeenSet) {
				toolbarDropDownList.SetTitleLanguage(resourceManager.GetString(toolbarDropDownList.className));
			}
		}

		public virtual void RenderToolbarSeparator(HtmlTextWriter writer, Toolbar toolbar) {
			if (toolbar.RepeatDirection == RepeatDirection.Vertical) writer.WriteLine("<tr>");
			writer.WriteLine(@"<td><img src=""" + CreateResourceString("separator." + toolbar.RepeatDirection.ToString().ToLower(), ResourceType.Toolbar) + @""" border=0 unselectable=""on""></td>");
			if (toolbar.RepeatDirection == RepeatDirection.Vertical) writer.WriteLine("</tr>");
		}
		#endregion

		#region CSS Rendering
		
		protected virtual void RenderEditorStyles(HtmlTextWriter writer) {
			writer.WriteLine(@"
." + this.ClientID + @"_OuterTable {
	width: " + this.Width.ToString() + @";
	background-color: " + ColorTranslator.ToHtml(this.BackColor) + @";
}
#" + this.ClientID + @"_toolbarArea td {
	vertical-align: middle;
	border-collapse: separate;
}
#" + this.ClientID + @"_toolbarArea select {
	margin: 0px;
	padding: 0px;
	font: 11px Tahoma,Verdana,sans-serif;
}
#" + this.ClientID + @"_toolbarArea img {
	display: block;
}
." + this.ClientID + @"_HtmlBox {
	overflow: auto;
	font-family: Courier New, Courier;
	padding: 4px;
	border-right: 1px solid " + ColorTranslator.ToHtml(this.EditorBorderColorLight) + @";
	border-left: 1px solid " + ColorTranslator.ToHtml(this.EditorBorderColorDark) + @";
	border-top: 1px solid " + ColorTranslator.ToHtml(this.EditorBorderColorDark) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(this.EditorBorderColorLight) + @";
}
." + this.ClientID + @"_DesignBox {
	" + ((!browserInfo.IsIE || DesignModeCss == "") ? "background-color: #FFFFFF;" : "") + @"
	border: 0; 
	border-right: 1px solid " + ColorTranslator.ToHtml(this.EditorBorderColorLight) + @";
	border-left: 1px solid " + ColorTranslator.ToHtml(this.EditorBorderColorDark) + @";
	border-top: 1px solid " + ColorTranslator.ToHtml(this.EditorBorderColorDark) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(this.EditorBorderColorLight) + @";
}
." + this.ClientID + @"_DesignBox body {
	background-color: black;
}
." + this.ClientID + @"_Toolbar {
	margin-bottom: 1px; 
	margin-right: 2px;
	float: left;
	" + ((ToolbarBackgroundImage && false) ? 
				"background-image: url(" + CreateResourceString("toolbar." + RepeatDirection.Horizontal.ToString().ToLower()+ ".background", ResourceType.Toolbar) + ");" : "" ) + @"
}
");
		}
		protected virtual void RenderTabStyles(HtmlTextWriter writer) {
			writer.WriteLine(@"
#" + this.ClientID + @"_TabRow td {
	vertical-align:center;
}
." + this.ClientID + @"_StartTabOn {
	font: 10pt MS Sans Serif;
	padding: 1px;
	border-left: 1px solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	border-right: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorLight) + @";
	border-top: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	background-color: " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
}
." + this.ClientID + @"_StartTabOff {
	font: 10pt MS Sans Serif;
	padding:1px;
	border-left: 1px solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	border-right: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-top: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	background-color: " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
}
." + this.ClientID + @"_TabOn {
	font: 8pt MS Sans Serif;
	padding:1px;
	padding-left:5px;
	padding-right:5px;
	border-left: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorLight) + @";
	border-right: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-top: 1px solid " + ColorTranslator.ToHtml(this.BackColor) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	background-color: " + ColorTranslator.ToHtml(this.BackColor) + @";	
}
." + this.ClientID + @"_TabOffRight {
	font: 8pt MS Sans Serif;
	padding:1px;
	padding-left:5px;
	padding-right:5px;
	border-left: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-right: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-top: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	background-color: " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
}
." + this.ClientID + @"_TabOffLeft {
	font: 8pt MS Sans Serif;
	padding:1px;
	padding-left:5px;
	padding-right:5px;
	border-left: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-right: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorLight) + @";
	border-top: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(this.BackColor) + @";
	background-color: " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
}
." + this.ClientID + @"_EndTab {
	font: 10pt MS Sans Serif;
	width: 100%;
	padding:1px;
	border-left: 1px solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	border-right: 1px solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	border-top: 1px solid " + ColorTranslator.ToHtml(this.GutterBorderColorDark) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
	background-color: " + ColorTranslator.ToHtml(this.GutterBackColor) + @";
}
." + this.ClientID + @"_AncestorArea {
	" + ((!this.ShowTagPath) ? "display:none;" : "") + @"
	margin-left: 4px;
}
." + this.ClientID + @"_AncestorArea a {
	padding: 1px;
	margin-left: 2px;
	margin-right: 2px;
	border: 1px solid #808080;	
	color: #000;
	font-family: arial;
	font-size: 11px;
}
." + this.ClientID + @"_AncestorArea a:link, ." + this.ClientID + @"_AncestorArea a:visited, ." + this.ClientID + @"_AncestorArea a:active {
	background-color: transparent;
	text-decoration: none;
}
." + this.ClientID + @"_AncestorArea a:hover {
	text-decoration: none;
	background-color: #316AC5;
	border: 1px solid #fff;
	color:#fff;
}
");
		}


		protected virtual void RenderButtonStyles(HtmlTextWriter writer) {
			
			//if (this.ButtonRenderMode == ButtonRenderMode.StyledBackgrounds) {
				writer.WriteLine(@"
." + this.ClientID + @"_items img {
	padding: 1px;
}
." + this.ClientID + @"_Button_Off_Out img {
	" + ((ButtonStyle.BorderColorLight == Color.Transparent && ButtonStyle.BorderColorDark == Color.Transparent) ? "padding: 1px;" : @"
	padding: 0px;
	border-top: 1px solid " + ColorTranslator.ToHtml(ButtonStyle.BorderColorLight) + @";	
	border-left: 1px solid " + ColorTranslator.ToHtml(ButtonStyle.BorderColorLight) + @";
	border-right: 1px solid " + ColorTranslator.ToHtml(ButtonStyle.BorderColorDark) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(ButtonStyle.BorderColorDark) + @"; ") + @" 
	background-color: " + ((ButtonStyle.BackColor != Color.Transparent) ? ColorTranslator.ToHtml(ButtonStyle.BackColor) : "transparent") + @";
}
." + this.ClientID + @"_Button_Off_Over img {
	" + ((ButtonStyle.OverBorderColorLight == Color.Transparent && ButtonStyle.OverBorderColorDark == Color.Transparent) ? "padding: 1px;" : @"
	padding: 0px;
	border-top: 1px solid " + ColorTranslator.ToHtml(ButtonStyle.OverBorderColorLight) + @";	
	border-left: 1px solid " + ColorTranslator.ToHtml(ButtonStyle.OverBorderColorLight) + @";
	border-right: 1px solid " + ColorTranslator.ToHtml(ButtonStyle.OverBorderColorDark) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(ButtonStyle.OverBorderColorDark) + @"; ") + @" 
	background-color: " + ((ButtonStyle.OverBackColor != Color.Transparent) ? ColorTranslator.ToHtml(ButtonStyle.OverBackColor) : "transparent") + @";
	" + ((ButtonStyle.UseOverBackgroundImage && (!this.browserInfo.IsIE || ButtonStyle.OverBackColorGradient == Color.Transparent)) ? "background-image: url(" + CreateResourceString("toolbarbuttoncss.off.over", ResourceType.Toolbar) + ");" : "") + @"
	" + ((ButtonStyle.UseOverBackgroundImage && (this.browserInfo.IsIE && ButtonStyle.OverBackColorGradient != Color.Transparent)) ? 
					"filter: progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='#FF" + ColorTranslator.ToHtml(ButtonStyle.OverBackColor).Replace("#","") + "', EndColorStr='#FF" + ColorTranslator.ToHtml(ButtonStyle.OverBackColorGradient).Replace("#","") + "');" : "") + @"
}
." + this.ClientID + @"_Button_On_Out img {
	" + ((ButtonStyleActive.BorderColorLight == Color.Transparent && ButtonStyleActive.BorderColorDark == Color.Transparent) ? "padding: 1px;" : @"
	padding: 0px;
	border-top: 1px solid " + ColorTranslator.ToHtml(ButtonStyleActive.BorderColorLight) + @";	
	border-left: 1px solid " + ColorTranslator.ToHtml(ButtonStyleActive.BorderColorLight) + @";
	border-right: 1px solid " + ColorTranslator.ToHtml(ButtonStyleActive.BorderColorDark) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(ButtonStyleActive.BorderColorDark) + @"; ") + @" 
	background-color: " + ((ButtonStyleActive.BackColor != Color.Transparent) ? ColorTranslator.ToHtml(ButtonStyleActive.BackColor) : "transparent") + @";
	" + ((ButtonStyleActive.UseBackgroundImage && (!this.browserInfo.IsIE || ButtonStyleActive.BackColorGradient == Color.Transparent)) ? "background-image: url(" + CreateResourceString("toolbarbuttoncss.on.out", ResourceType.Toolbar) + ");" : "") + @"
	" + ((ButtonStyleActive.UseBackgroundImage && (this.browserInfo.IsIE && ButtonStyleActive.BackColorGradient != Color.Transparent)) ? 
					"filter: progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='#FF" + ColorTranslator.ToHtml(ButtonStyleActive.BackColor).Replace("#","") + "', EndColorStr='#FF" + ColorTranslator.ToHtml(ButtonStyleActive.BackColorGradient).Replace("#","") + "');" : "") + @"
}
." + this.ClientID + @"_Button_On_Over img {
	" + ((ButtonStyleActive.OverBorderColorLight == Color.Transparent && ButtonStyleActive.OverBorderColorDark == Color.Transparent) ? "padding: 1px;" : @"
	padding: 0px;
	border-top: 1px solid " + ColorTranslator.ToHtml(ButtonStyleActive.OverBorderColorLight) + @";	
	border-left: 1px solid " + ColorTranslator.ToHtml(ButtonStyleActive.OverBorderColorLight) + @";
	border-right: 1px solid " + ColorTranslator.ToHtml(ButtonStyleActive.OverBorderColorDark) + @";
	border-bottom: 1px solid " + ColorTranslator.ToHtml(ButtonStyleActive.OverBorderColorDark) + @"; ") + @" 
	background-color: " + ((ButtonStyleActive.OverBackColor != Color.Transparent) ? ColorTranslator.ToHtml(ButtonStyleActive.OverBackColor) : "transparent") + @";
	" + ((ButtonStyleActive.UseOverBackgroundImage && (!this.browserInfo.IsIE || ButtonStyleActive.BackColorGradient == Color.Transparent)) ? "background-image: url(" + CreateResourceString("toolbarbuttoncss.on.over", ResourceType.Toolbar) + ");" : "") + @"
	" + ((ButtonStyleActive.UseOverBackgroundImage && (this.browserInfo.IsIE && ButtonStyleActive.BackColorGradient != Color.Transparent)) ? 
	"filter: progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='#FF" + ColorTranslator.ToHtml(ButtonStyleActive.OverBackColor).Replace("#","") + "', EndColorStr='#FF" + ColorTranslator.ToHtml(ButtonStyleActive.OverBackColorGradient).Replace("#","") + "');" : "") + @"
}
");
			//}
		}
		#endregion

		#endregion

		#region Helper Functions
		private string CreateUniqueToolbarItemID(Toolbar toolbar, ToolbarItem toolbarItem) 
		{
			return this.ClientID + "_" + this.Toolbars.IndexOf(toolbar).ToString() + "_" + toolbar.Items.IndexOf(toolbarItem).ToString();
		}

		private string CreateResourceString(string filename, ResourceType resourceType) {
			// this allows buttons and toolbar background images to be handled separately so 
			// a developer can still have a Green Office 2003 toolbar
			// and custom JS files
			
			// if NotSet, then default to Office2003
			ToolbarStyleConfiguration style = this.ButtonSet;
			if (style == ToolbarStyleConfiguration.NotSet) style = ToolbarStyleConfiguration.Office2003;
			
			switch (resourceType) {
				default:
					return string.Empty;
				case ResourceType.Button:
					if (this.ButtonImagesLocation == ResourceLocation.InternalResource) {				
						
						return ClientScriptWrapper.GetWebResourceUrl(this,"FreeTextBoxControls.Resources.Images." + style + "." + filename + ".gif", this.AssemblyResourceHandlerPath);						
						
					} else {
						return (this.SupportFolder + this.ButtonFolder + "/" + filename + "." + this.ButtonFileExtention).ToLower();
					}
				case ResourceType.Toolbar:
					if (this.ToolbarImagesLocation == ResourceLocation.InternalResource) {
						
						return ClientScriptWrapper.GetWebResourceUrl(this,"FreeTextBoxControls.Resources.Images." + style + "." + filename + ".gif", this.AssemblyResourceHandlerPath);					
					} else {
						return (this.SupportFolder + this.ButtonFolder + "/" + filename + "." + this.ButtonFileExtention).ToLower();
					}
				case ResourceType.JavaScript:
					if (this.JavaScriptLocation == ResourceLocation.InternalResource) {
						
						return ClientScriptWrapper.GetWebResourceUrl(this,"FreeTextBoxControls.Resources.JavaScript." + filename, this.AssemblyResourceHandlerPath);						
												
					} else {
						return this.SupportFolder + filename;
					}

			}
		}
		#endregion
	}
}

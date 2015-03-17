using FreeTextBoxControls.Design;
using FreeTextBoxControls.Licensing;
using FreeTextBoxControls.Support;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace FreeTextBoxControls
{
	[DefaultProperty("Text"), Designer(typeof(FreeTextBoxDesigner)), LicenseProvider(typeof(FtbLicenseProvider)), ParseChildren(true), PersistChildren(true), ToolboxData("<{0}:FreeTextBox runat=\"server\"></{0}:FreeTextBox>"), ValidationProperty("Text")]
	public class FreeTextBox : Control, IPostBackDataHandler, INamingContainer, IPostBackEventHandler, IDisposable
	{
		private FtbLicense license;
		private ToolbarCollection toolbars;
		private ToolbarButtonStyle buttonStyle;
		private ToolbarButtonStyle buttonStyleActive;
		private bool hasToolbars;
		private BrowserInfo browserInfo;
		private ResourceManager resourceManager;
		private string viewStateText;
		public event EventHandler SaveClick;
		public event EventHandler ProcessText;
		public event EventHandler TextChanged;
		internal new bool DesignMode
		{
			get
			{
				return this.Context == null || (base.Site != null && base.Site.DesignMode);
			}
		}
		[Category("Output"), Description("Contains the HTML for the editor.")]
		public string Text
		{
			get
			{
				object obj = this.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
			set
			{
				this.ViewState["Text"] = value;
			}
		}
		[Category("Output"), Description("Converts the returned HTML to XHTML")]
		public string Xhtml
		{
			get
			{
				string text = this.Text;
				Formatter formatter = new Formatter();
				return formatter.HtmlToXhtml(text);
			}
		}
		[Category("Output"), Description("Contains the pre-processed HTML for the editor."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ViewStateText
		{
			get
			{
				return this.viewStateText;
			}
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string HtmlStrippedText
		{
			get
			{
				string text = this.Text;
				return Regex.Replace(text, "<(.|\n)+?>", " ", RegexOptions.IgnoreCase);
			}
		}
		public override string ClientID
		{
			get
			{
				string text = base.ClientID;
				while (text.Substring(0, 1) == "_")
				{
					text = text.Substring(1);
				}
				return text.Replace("\\", "_").Replace("/", "_").Replace(".", "_");
			}
		}
		[Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ToolbarButtonStyle ButtonStyle
		{
			get
			{
				if (this.buttonStyle == null)
				{
					this.buttonStyle = new ToolbarButtonStyle();
					this.SetButtonStyle(this.buttonStyle, ToolbarStyleConfiguration.Office2003, true);
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.buttonStyle).TrackViewState();
					}
				}
				return this.buttonStyle;
			}
		}
		[Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ToolbarButtonStyle ButtonStyleActive
		{
			get
			{
				if (this.buttonStyleActive == null)
				{
					this.buttonStyleActive = new ToolbarButtonStyle();
					this.SetButtonStyle(this.buttonStyleActive, ToolbarStyleConfiguration.Office2003, false);
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.buttonStyleActive).TrackViewState();
					}
				}
				return this.buttonStyleActive;
			}
		}
		[Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		public ToolbarCollection Toolbars
		{
			get
			{
				if (this.toolbars == null)
				{
					this.toolbars = new ToolbarCollection();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.toolbars).TrackViewState();
					}
				}
				return this.toolbars;
			}
		}
		[Category("Behavior")]
		public string ClientSideTextChanged
		{
			get
			{
				object obj = this.ViewState["ClientSideTextChanged"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ClientSideTextChanged"] = value;
			}
		}
		public ResourceLocation JavaScriptLocation
		{
			get
			{
				object obj = this.ViewState["JavaScriptLocation"];
				if (obj != null)
				{
					return (ResourceLocation)obj;
				}
				return ResourceLocation.InternalResource;
			}
			set
			{
				this.ViewState["JavaScriptLocation"] = value;
			}
		}
		public ResourceLocation ButtonImagesLocation
		{
			get
			{
				object obj = this.ViewState["ButtonImagesLocation"];
				if (obj != null)
				{
					return (ResourceLocation)obj;
				}
				return ResourceLocation.InternalResource;
			}
			set
			{
				this.ViewState["ButtonImagesLocation"] = value;
			}
		}
		public ResourceLocation ToolbarImagesLocation
		{
			get
			{
				object obj = this.ViewState["ToolbarImagesLocation"];
				if (obj != null)
				{
					return (ResourceLocation)obj;
				}
				return ResourceLocation.InternalResource;
			}
			set
			{
				this.ViewState["ToolbarImagesLocation"] = value;
			}
		}
		public ToolbarStyleConfiguration ButtonSet
		{
			get
			{
				object obj = this.ViewState["ToolbarStyleConfiguration"];
				if (obj != null)
				{
					return (ToolbarStyleConfiguration)obj;
				}
				return ToolbarStyleConfiguration.Office2003;
			}
			set
			{
				this.ViewState["ToolbarStyleConfiguration"] = value;
			}
		}
		[Category("Behavior"), Description("Gets or sets how the user is alerted that FreeTextBox has not been installed correctly.")]
		public InstallationErrorMessage InstallationErrorMessage
		{
			get
			{
				object obj = this.ViewState["InstallationErrorMessage"];
				if (obj != null)
				{
					return (InstallationErrorMessage)obj;
				}
				return InstallationErrorMessage.InlineMessage;
			}
			set
			{
				this.ViewState["InstallationErrorMessage"] = value;
			}
		}
		[Category("Behavior"), Description("Gets or sets the URL of the editor window.")]
		public string BaseUrl
		{
			get
			{
				object obj = this.ViewState["BaseUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
			set
			{
				this.ViewState["BaseUrl"] = value;
			}
		}
		[Category("External"), Description("Gets or sets the URL of the style sheet used in Design mode.")]
		public string DesignModeCss
		{
			get
			{
				object obj = this.ViewState["DesignModeCss"];
				if (obj != null)
				{
					return base.ResolveUrl((string)obj);
				}
				return "";
			}
			set
			{
				this.ViewState["DesignModeCss"] = value;
			}
		}
		internal string DesignModeCssViewState
		{
			get
			{
				object obj = this.ViewState["DesignModeCss"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
		}
		[Category("External"), Description("Gets or sets a CSS class used for the editor area.")]
		public string DesignModeBodyTagCssClass
		{
			get
			{
				object obj = this.ViewState["DesignModeBodyTagCssClass"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
			set
			{
				this.ViewState["DesignModeBodyTagCssClass"] = value;
			}
		}
		[Category("Appearance"), Description("Gets or sets the width of the editor.")]
		public Unit Width
		{
			get
			{
				object obj = this.ViewState["Width"];
				if (obj != null)
				{
					return (Unit)obj;
				}
				return new Unit("600px");
			}
			set
			{
				this.ViewState["Width"] = value;
			}
		}
		[Category("Appearance"), Description("Gets or sets the height of the editor.")]
		public Unit Height
		{
			get
			{
				object obj = this.ViewState["Height"];
				if (obj != null)
				{
					return (Unit)obj;
				}
				return new Unit("350px");
			}
			set
			{
				this.ViewState["Height"] = value;
			}
		}
		[Category("Appearance"), Description("Gets or sets the background color of the editor.")]
		public Color BackColor
		{
			get
			{
				object obj = this.ViewState["BackColor"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return ColorTranslator.FromHtml("#9EBEF5");
			}
			set
			{
				this.ViewState["BackColor"] = value;
			}
		}
		[Category("Toolbar"), Description("Gets or sets whether buttons change images onMouseOver.")]
		public string ButtonFolder
		{
			get
			{
				if (this.ViewState["ButtonFolder"] == null)
				{
					return "Images";
				}
				string text = (string)this.ViewState["ButtonFolder"];
				while (text.StartsWith("/") || text.StartsWith("\\"))
				{
					text = text.Substring(1);
				}
				while (text.EndsWith("/") || text.EndsWith("\\"))
				{
					text = text.Substring(0, text.Length - 1);
				}
				return text;
			}
			set
			{
				this.ViewState["ButtonFolder"] = value;
			}
		}
		[Category("Toolbar"), Description("Gets or sets the width in pixels of ToolbarButtons.")]
		public int ButtonWidth
		{
			get
			{
				object obj = this.ViewState["ButtonWidth"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 21;
			}
			set
			{
				this.ViewState["ButtonWidth"] = value;
			}
		}
		[Category("Toolbar"), Description("Gets or sets the height in pixels of ToolbarButtons.")]
		public int ButtonHeight
		{
			get
			{
				object obj = this.ViewState["ButtonHeight"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 20;
			}
			set
			{
				this.ViewState["ButtonHeight"] = value;
			}
		}
		[Category("Toolbar"), DefaultValue(""), Description("Gets or sets a prebuilt Toolbar styling to emulate versions of Microsoft Office."), NotifyParentProperty(true)]
		public ToolbarStyleConfiguration ToolbarStyleConfiguration
		{
			get
			{
				object obj = this.ViewState["ToolbarStyleConfiguration"];
				if (obj != null)
				{
					return (ToolbarStyleConfiguration)this.ViewState["ToolbarStyleConfiguration"];
				}
				return ToolbarStyleConfiguration.NotSet;
			}
			set
			{
				this.ViewState["ToolbarStyleConfiguration"] = value;
			}
		}
		[Category("Toolbar"), DefaultValue(""), Description("Gets or sets the CSS class for DropDownLists."), NotifyParentProperty(true)]
		public string DropDownListCssClass
		{
			get
			{
				object obj = this.ViewState["DropDownListCssClass"];
				if (obj != null)
				{
					return (string)this.ViewState["DropDownListCssClass"];
				}
				return "";
			}
			set
			{
				this.ViewState["DropDownListCssClass"] = value;
			}
		}
		[Category("Toolbar"), DefaultValue(""), Description("Gets or sets the CSS class for HTML Mode."), NotifyParentProperty(true)]
		public string HtmlModeCssClass
		{
			get
			{
				object obj = this.ViewState["HtmlModeCssClass"];
				if (obj != null)
				{
					return (string)this.ViewState["HtmlModeCssClass"];
				}
				return "";
			}
			set
			{
				this.ViewState["HtmlModeCssClass"] = value;
			}
		}
		[Category("Behavior"), DefaultValue(""), Description("Gets or sets whether the ToolbarButtons and ToolbarDropDownLists reflect the style of the text at the cursor."), NotifyParentProperty(true)]
		public bool UpdateToolbar
		{
			get
			{
				object obj = this.ViewState["UpdateToolbar"];
				return obj == null || (bool)this.ViewState["UpdateToolbar"];
			}
			set
			{
				this.ViewState["UpdateToolbar"] = value;
			}
		}
		[Category("Behavior"), DefaultValue(""), Description("Gets or sets whether a background image is used for the toolbar."), NotifyParentProperty(true)]
		public bool ToolbarBackgroundImage
		{
			get
			{
				object obj = this.ViewState["BackgroundImage"];
				return obj == null || (bool)this.ViewState["BackgroundImage"];
			}
			set
			{
				this.ViewState["BackgroundImage"] = value;
			}
		}
		[Category("Toolbar"), Description("The back color of each toolbar.")]
		public Color ToolbarBackColor
		{
			get
			{
				object obj = this.ViewState["ToolbarBackColor"];
				if (obj != null)
				{
					return (Color)this.ViewState["ToolbarBackColor"];
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["ToolbarBackColor"] = value;
			}
		}
		[Category("Toolbar"), Description("Gets or sets whether this control display Toolbars")]
		public bool EnableToolbars
		{
			get
			{
				object obj = this.ViewState["EnableToolbars"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["EnableToolbars"] = value;
			}
		}
		[Category("Toolbar"), Description("Gets or sets whether an image is behind a toolbar")]
		public bool UseToolbarBackGroundImage
		{
			get
			{
				object obj = this.ViewState["UseToolbarBackGroundImage"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["UseToolbarBackGroundImage"] = value;
			}
		}
		[Category("Toolbar"), Description("Gets or sets whether the toolbars are hidden when the editor is in HTML mode.")]
		public bool AutoGenerateToolbarsFromString
		{
			get
			{
				object obj = this.ViewState["AutoGenerateToolbarsFromString"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AutoGenerateToolbarsFromString"] = value;
			}
		}
		[Category("Toolbar")]
		public string ToolbarLayout
		{
			get
			{
				object obj = this.ViewState["ToolbarLayout"];
				if (obj != null)
				{
					return (string)obj;
				}
				return ToolbarGenerator.DefaultConfigString;
			}
			set
			{
				this.ViewState["ToolbarLayout"] = value;
			}
		}
		private bool ToolbarsCreated
		{
			get
			{
				object obj = this.ViewState["ToolbarsCreated"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ToolbarsCreated"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] StylesMenuList
		{
			get
			{
				object obj = this.ViewState["StyleMenuList"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["StyleMenuList"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] StylesMenuNames
		{
			get
			{
				object obj = this.ViewState["StyleMenuNames"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["StyleMenuNames"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] FontFacesMenuList
		{
			get
			{
				object obj = this.ViewState["FontFacesMenuList"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["FontFacesMenuList"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] FontFacesMenuNames
		{
			get
			{
				object obj = this.ViewState["FontFacesMenuNames"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["FontFacesMenuNames"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] FontSizesMenuList
		{
			get
			{
				object obj = this.ViewState["FontSizesMenuList"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["FontSizesMenuList"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] FontSizesMenuNames
		{
			get
			{
				object obj = this.ViewState["FontSizesMenuNames"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["FontSizesMenuNames"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public Color[] FontForeColorMenuList
		{
			get
			{
				object obj = this.ViewState["FontForeColorMenuList"];
				if (obj != null)
				{
					return (Color[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["FontForeColorMenuList"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] FontForeColorMenuNames
		{
			get
			{
				object obj = this.ViewState["FontForeColorMenuNames"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["FontForeColorMenuNames"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public Color[] FontBackColorMenuList
		{
			get
			{
				object obj = this.ViewState["FontBackColorMenuList"];
				if (obj != null)
				{
					return (Color[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["FontBackColorMenuList"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] FontBackColorMenuNames
		{
			get
			{
				object obj = this.ViewState["FontBackColorMenuNames"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["FontBackColorMenuNames"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] InsertHtmlMenuList
		{
			get
			{
				object obj = this.ViewState["InsertHtmlMenuList"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["InsertHtmlMenuList"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] InsertHtmlMenuNames
		{
			get
			{
				object obj = this.ViewState["InsertHtmlMenuNames"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["InsertHtmlMenuNames"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] ParagraphMenuList
		{
			get
			{
				object obj = this.ViewState["ParagraphMenuList"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["ParagraphMenuList"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] ParagraphMenuNames
		{
			get
			{
				object obj = this.ViewState["ParagraphMenuNames"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["ParagraphMenuNames"] = value;
			}
		}
		[Category("DropDownList Arrays")]
		public string[] SymbolsMenuList
		{
			get
			{
				object obj = this.ViewState["SymbolsMenuList"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return null;
			}
			set
			{
				this.ViewState["SymbolsMenuList"] = value;
			}
		}
		[Category("Behavior")]
		public string AssemblyResourceHandlerPath
		{
			get
			{
				object obj = this.ViewState["AssemblyResourceHandlerPath"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
			set
			{
				this.ViewState["AssemblyResourceHandlerPath"] = value;
			}
		}
		[Category("Behavior")]
		public bool ShowTagPath
		{
			get
			{
				if (!this.license.IsPro)
				{
					return false;
				}
				object obj = this.ViewState["ShowTagPath"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowTagPath"] = value;
			}
		}
		[Category("Behavior")]
		public new bool Focus
		{
			get
			{
				object obj = this.ViewState["Focus"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["Focus"] = value;
			}
		}
		[Category("Behavior")]
		public string ImageGalleryPath
		{
			get
			{
				object obj = this.ViewState["ImageGalleryPath"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "~/images/";
			}
			set
			{
				this.ViewState["ImageGalleryPath"] = value;
			}
		}
		[Category("Behavior")]
		public string ImageGalleryUrl
		{
			get
			{
				object obj = this.ViewState["ImageGalleryUrl"];
				if (obj == null)
				{
					return "ftb.imagegallery.aspx?rif={0}&cif={0}";
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["ImageGalleryUrl"] = value;
			}
		}
		[Category("Behavior")]
		public bool AutoParseStyles
		{
			get
			{
				object obj = this.ViewState["AutoParseStyles"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AutoParseStyles"] = value;
			}
		}
		[Category("Behavior")]
		public string SslUrl
		{
			get
			{
				object obj = this.ViewState["SslUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "/.";
			}
			set
			{
				this.ViewState["SslUrl"] = value;
			}
		}
		[Category("Behavior")]
		public TextDirection TextDirection
		{
			get
			{
				object obj = this.ViewState["TextDirection"];
				if (obj != null)
				{
					return (TextDirection)obj;
				}
				return TextDirection.LeftToRight;
			}
			set
			{
				this.ViewState["TextDirection"] = value;
			}
		}
		[Category("Behavior")]
		public bool StripAllScripting
		{
			get
			{
				object obj = this.ViewState["StripAllScripting"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["StripAllScripting"] = value;
			}
		}
		[Category("Behavior")]
		public bool FormatHtmlTagsToXhtml
		{
			get
			{
				object obj = this.ViewState["FormatHtmlTagsToXhtml"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["FormatHtmlTagsToXhtml"] = value;
			}
		}
		[Category("Behavior")]
		public bool DisableIEBackButton
		{
			get
			{
				object obj = this.ViewState["DisableIEBackButton"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["DisableIEBackButton"] = value;
			}
		}
		[Category("Behavior")]
		public bool EnableSsl
		{
			get
			{
				object obj = this.ViewState["EnableSsl"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["EnableSsl"] = value;
			}
		}
		[Category("Behavior")]
		public bool RemoveServerNameFromUrls
		{
			get
			{
				object obj = this.ViewState["RemoveServerNameFromUrls"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["RemoveServerNameFromUrls"] = value;
			}
		}
		[Category("Behavior")]
		public bool RemoveScriptNameFromBookmarks
		{
			get
			{
				object obj = this.ViewState["RemoveScriptNameFromBookmarks"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["RemoveScriptNameFromBookmarks"] = value;
			}
		}
		[Category("Behavior")]
		public bool ConvertHtmlSymbolsToHtmlCodes
		{
			get
			{
				object obj = this.ViewState["ConvertHtmlSymbolsToHtmlCodes"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ConvertHtmlSymbolsToHtmlCodes"] = value;
			}
		}
		[Category("Behavior")]
		public PasteMode PasteMode
		{
			get
			{
				object obj = this.ViewState["PasteMode"];
				if (obj != null)
				{
					return (PasteMode)obj;
				}
				return PasteMode.Default;
			}
			set
			{
				this.ViewState["PasteMode"] = value;
			}
		}
		[Category("Behavior")]
		public BreakMode BreakMode
		{
			get
			{
				object obj = this.ViewState["BreakMode"];
				if (obj != null)
				{
					return (BreakMode)obj;
				}
				return BreakMode.Paragraph;
			}
			set
			{
				this.ViewState["BreakMode"] = value;
			}
		}
		[Category("Behavior")]
		public TabMode TabMode
		{
			get
			{
				object obj = this.ViewState["TabMode"];
				if (obj != null)
				{
					return (TabMode)obj;
				}
				return TabMode.InsertSpaces;
			}
			set
			{
				this.ViewState["TabMode"] = value;
			}
		}
		[Category("Behavior")]
		public bool EnableHtmlMode
		{
			get
			{
				object obj = this.ViewState["EnableHtmlMode"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["EnableHtmlMode"] = value;
			}
		}
		[Category("Behavior")]
		public RenderMode RenderMode
		{
			get
			{
				object obj = this.ViewState["RenderMode"];
				if (obj != null)
				{
					return (RenderMode)obj;
				}
				return RenderMode.NotSet;
			}
			set
			{
				this.ViewState["RenderMode"] = value;
			}
		}
		[Category("Behavior")]
		public string Language
		{
			get
			{
				object obj = this.ViewState["Language"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "en-US";
			}
			set
			{
				this.ViewState["Language"] = value;
			}
		}
		[Category("External")]
		public string SupportFolder
		{
			get
			{
				object obj = this.ViewState["SupportFolder"];
				if (obj == null)
				{
					return base.ResolveUrl("/aspnet_client/FreeTextBox/");
				}
				string text = (string)obj;
				text.Replace("\\", "/");
				if (!text.EndsWith("/"))
				{
					text += "/";
				}
				return base.ResolveUrl(text);
			}
			set
			{
				this.ViewState["SupportFolder"] = value;
			}
		}
		internal string SupportFolderViewState
		{
			get
			{
				object obj = this.ViewState["SupportFolder"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "/aspnet_client/FreeTextBox/";
			}
		}
		[Category("Behavior")]
		public ScriptMode ScriptMode
		{
			get
			{
				object obj = this.ViewState["ScriptMode"];
				if (obj != null)
				{
					return (ScriptMode)obj;
				}
				return ScriptMode.External;
			}
			set
			{
				this.ViewState["ScriptMode"] = value;
			}
		}
		[Category("Behavior")]
		public int TabIndex
		{
			get
			{
				object obj = this.ViewState["TabIndex"];
				if (obj != null)
				{
					return (int)obj;
				}
				return -1;
			}
			set
			{
				this.ViewState["TabIndex"] = value;
			}
		}
		[Category("Behavior")]
		public bool ReadOnly
		{
			get
			{
				object obj = this.ViewState["ReadOnly"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ReadOnly"] = value;
			}
		}
		[Category("Behavior")]
		public EditorMode StartMode
		{
			get
			{
				object obj = this.ViewState["StartMode"];
				if (obj != null)
				{
					return (EditorMode)obj;
				}
				return EditorMode.DesignMode;
			}
			set
			{
				this.ViewState["StartMode"] = value;
			}
		}
		[Category("External")]
		public string ButtonFileExtention
		{
			get
			{
				object obj = this.ViewState["ButtonFileExtention"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "gif";
			}
			set
			{
				this.ViewState["ButtonFileExtention"] = value;
			}
		}
		[Category("Behavior")]
		public bool HtmlModeDefaultsToMonoSpaceFont
		{
			get
			{
				object obj = this.ViewState["HtmlModeDefaultsToMonoSpaceFont"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["HtmlModeDefaultsToMonoSpaceFont"] = value;
			}
		}
		[Category("Appearance")]
		public Color GutterBackColor
		{
			get
			{
				object obj = this.ViewState["GutterBackColor"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return ColorTranslator.FromHtml("#81A9E2");
			}
			set
			{
				this.ViewState["GutterBackColor"] = value;
			}
		}
		[Category("Appearance")]
		public Color EditorBorderColorLight
		{
			get
			{
				object obj = this.ViewState["EditorBorderColorLight"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return ColorTranslator.FromHtml("#808080");
			}
			set
			{
				this.ViewState["EditorBorderColorLight"] = value;
			}
		}
		[Category("Appearance")]
		public Color EditorBorderColorDark
		{
			get
			{
				object obj = this.ViewState["EditorBorderColorDark"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return ColorTranslator.FromHtml("#808080");
			}
			set
			{
				this.ViewState["EditorBorderColorDark"] = value;
			}
		}
		[Category("Appearance")]
		public Color GutterBorderColorLight
		{
			get
			{
				object obj = this.ViewState["GutterBorderColorLight"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return ColorTranslator.FromHtml("#FFFFFF");
			}
			set
			{
				this.ViewState["GutterBorderColorLight"] = value;
			}
		}
		[Category("Appearance")]
		public Color GutterBorderColorDark
		{
			get
			{
				object obj = this.ViewState["GutterBorderColorDark"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return ColorTranslator.FromHtml("#808080");
			}
			set
			{
				this.ViewState["GutterBorderColorDark"] = value;
			}
		}
		[Category("DownLevel")]
		public int DownLevelCols
		{
			get
			{
				object obj = this.ViewState["DownLevelCols"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 50;
			}
			set
			{
				this.ViewState["DownLevelCols"] = value;
			}
		}
		[Category("DownLevel")]
		public int DownLevelRows
		{
			get
			{
				object obj = this.ViewState["DownLevelRows"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 10;
			}
			set
			{
				this.ViewState["DownLevelRows"] = value;
			}
		}
		[Category("DownLevel")]
		public DownLevelMode DownLevelMode
		{
			get
			{
				object obj = this.ViewState["DownLevelMode"];
				if (obj != null)
				{
					return (DownLevelMode)obj;
				}
				return DownLevelMode.TextArea;
			}
			set
			{
				this.ViewState["DownLevelMode"] = value;
			}
		}
		[Category("DownLevel")]
		public string DownLevelMessage
		{
			get
			{
				object obj = this.ViewState["DownLevelMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["DownLevelMessage"] = value;
			}
		}
		[Category("Depreciated"), Obsolete("Please use the HtmlModeCssClass property")]
		public string HtmlModeCss
		{
			get
			{
				object obj = this.ViewState["HtmlModeCss"];
				if (obj != null)
				{
					return base.ResolveUrl((string)obj);
				}
				return "";
			}
			set
			{
				this.ViewState["HtmlModeCss"] = value;
			}
		}
		[Category("Depreciated"), Obsolete("Unused FTB 1.x property.")]
		public bool ButtonOverImage
		{
			get
			{
				object obj = this.ViewState["ButtonOverImage"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ButtonOverImage"] = value;
			}
		}
		[Category("Depreciated"), Obsolete("Unused FTB 1.x property.")]
		public bool ButtonDownImage
		{
			get
			{
				object obj = this.ViewState["ButtonDownImage"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ButtonDownImage"] = value;
			}
		}
		[Category("Depreciated"), Obsolete("Please use the EnableHtmlMode property.")]
		public bool AllowHtmlMode
		{
			get
			{
				object obj = this.ViewState["AllowHtmlMode"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["AllowHtmlMode"] = value;
			}
		}
		[Category("Depreciated"), Obsolete("Unused FTB 1.x property.")]
		public string AutoConfigure
		{
			get
			{
				object obj = this.ViewState["AutoConfigure"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
			set
			{
				this.ViewState["AutoConfigure"] = value;
			}
		}
		[Category("Depreciated"), Obsolete("Unused FTB 1.x property for DNN 2.x.")]
		public string HelperFilesParameters
		{
			get
			{
				object obj = this.ViewState["HelperFilesParameters"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
			set
			{
				this.ViewState["HelperFilesParameters"] = value;
			}
		}
		[Category("Depreciated"), Obsolete("Unused FTB 1.x property for DNN 2.x.")]
		public string HelperFilesPath
		{
			get
			{
				object obj = this.ViewState["HelperFilesPath"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
			set
			{
				this.ViewState["HelperFilesPath"] = value;
			}
		}
		[Category("Depreciated"), Obsolete("Unused FTB 1.x property.")]
		public string ButtonPath
		{
			get
			{
				object obj = this.ViewState["ButtonPath"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
			set
			{
				this.ViewState["ButtonPath"] = value;
			}
		}
		[Category("Depreciated"), Obsolete("Unused FTB 1.x property.")]
		public bool AutoHideToolbar
		{
			get
			{
				object obj = this.ViewState["AutoHideToolbar"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AutoHideToolbar"] = value;
			}
		}
		public FreeTextBox()
		{
			this.ProcessText += new EventHandler(this.InternalProcessText);
			try
			{
				this.license = (FtbLicense)LicenseManager.Validate(typeof(FreeTextBox), this);
			}
			catch (Exception ex)
			{
				this.license = new FtbLicense(base.GetType(), "Unlicensed: " + ex.ToString(), "", false);
			}
		}
		public override void Dispose()
		{
			base.Dispose();
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.license != null)
			{
				this.license.Dispose();
				this.license = null;
			}
		}
		[Category("Toolbar")]
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] array = (object[])savedState;
				base.LoadViewState(array[0]);
				if (array[1] != null)
				{
					((IStateManager)this.Toolbars).LoadViewState(array[1]);
				}
				if (array[2] != null)
				{
					((IStateManager)this.ButtonStyle).LoadViewState(array[2]);
				}
				if (array[3] != null)
				{
					((IStateManager)this.ButtonStyleActive).LoadViewState(array[3]);
				}
			}
		}
		protected override object SaveViewState()
		{
			object[] array = new object[4];
			array[0] = base.SaveViewState();
			if (this.toolbars != null)
			{
				array[1] = ((IStateManager)this.toolbars).SaveViewState();
			}
			if (this.buttonStyle != null)
			{
				array[2] = ((IStateManager)this.buttonStyle).SaveViewState();
			}
			if (this.buttonStyleActive != null)
			{
				array[3] = ((IStateManager)this.buttonStyleActive).SaveViewState();
			}
			return array;
		}
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this.toolbars != null)
			{
				((IStateManager)this.toolbars).TrackViewState();
			}
			if (this.buttonStyle != null)
			{
				((IStateManager)this.buttonStyle).TrackViewState();
			}
			if (this.buttonStyleActive != null)
			{
				((IStateManager)this.buttonStyleActive).TrackViewState();
			}
		}
		protected virtual void OnSaveClick(EventArgs e)
		{
			if (this.SaveClick != null)
			{
				this.SaveClick(this, e);
			}
		}
		protected virtual void OnTextChanged(EventArgs e)
		{
			if (this.TextChanged != null)
			{
				this.TextChanged(this, e);
			}
		}
		protected virtual void OnProcessText(EventArgs e)
		{
			if (this.ProcessText != null)
			{
				this.ProcessText(this, e);
			}
		}
		private void InternalProcessText(object source, EventArgs args)
		{
			FreeTextBox freeTextBox = (FreeTextBox)source;
			string text = freeTextBox.Text;
			Formatter formatter = new Formatter();
			if (!freeTextBox.RemoveServerNameFromUrls && !freeTextBox.ConvertHtmlSymbolsToHtmlCodes)
			{
				if (!freeTextBox.RemoveScriptNameFromBookmarks)
				{
					goto IL_12F;
				}
			}
			try
			{
				string text2 = this.Page.Request.Url.AbsoluteUri.ToString();
				string serverPath = text2.Substring(0, text2.IndexOf(this.Page.Request.ServerVariables["HTTP_HOST"]) + this.Page.Request.ServerVariables["HTTP_HOST"].Length);
				if (freeTextBox.RemoveScriptNameFromBookmarks)
				{
					string arg_B5_0 = this.Page.Request.ServerVariables["HTTP_HOST"];
					string arg_D0_0 = this.Page.Request.ServerVariables["SCRIPT_NAME"];
					this.Page.Request.QueryString.ToString();
					text = formatter.RemoveScriptNameFromBookmarks(text, text2);
					text = formatter.RemoveScriptNameFromBookmarks(text, text2.Replace("&", "&amp;"));
				}
				if (freeTextBox.RemoveServerNameFromUrls)
				{
					text = formatter.RemoveServerNameFromUrls(text, serverPath);
				}
				if (freeTextBox.ConvertHtmlSymbolsToHtmlCodes)
				{
					text = formatter.HtmlSymbolsToHtmlCodes(text);
				}
			}
			catch
			{
			}
			IL_12F:
			if (this.StripAllScripting)
			{
				text = formatter.RemoveScriptTags(text);
			}
			if (this.FormatHtmlTagsToXhtml && this.license.IsPro)
			{
				text = formatter.HtmlToXhtml(text);
				if (this.StripAllScripting)
				{
					text = formatter.RemoveJavaScriptEventsFromTags(text);
				}
			}
			freeTextBox.Text = text;
		}
		public virtual void RaisePostBackEvent(string eventArgument)
		{
			HttpContext.Current.Trace.Write("PostBackEvent", eventArgument);
			if (eventArgument != null)
			{
				if (!(eventArgument == "Save"))
				{
					return;
				}
				this.OnSaveClick(EventArgs.Empty);
			}
		}
		public void RaisePostDataChangedEvent()
		{
			this.OnTextChanged(EventArgs.Empty);
			this.OnProcessText(EventArgs.Empty);
		}
		public bool LoadPostData(string postDataKey, NameValueCollection values)
		{
			string text = this.Text;
			string text2 = values[this.ClientID];
			if (!text.Equals(text2))
			{
				this.Text = text2;
				this.viewStateText = text2;
				return true;
			}
			return false;
		}
		protected override void OnInit(EventArgs e)
		{
			if (!this.DesignMode)
			{
				this.browserInfo = BrowserInfo.GetBrowserInfo();
				if (((this.browserInfo != null && this.browserInfo.IsRichCapable) || this.RenderMode == RenderMode.Rich) && this.AutoGenerateToolbarsFromString && !this.ToolbarsCreated)
				{
					ToolbarCollection toolbarCollection = ToolbarGenerator.ToolbarsFromString(this.ToolbarLayout);
					for (int i = toolbarCollection.Count - 1; i > -1; i--)
					{
						this.Toolbars.Insert(0, toolbarCollection[i]);
					}
					this.ToolbarsCreated = true;
				}
			}
			base.OnInit(e);
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}
		protected override void OnPreRender(EventArgs e)
		{
			if (!this.DesignMode)
			{
				if (this.browserInfo != null && this.browserInfo.IsRichCapable)
				{
					this.resourceManager = new ResourceManager(this.Language, this.SupportFolder + "Languages/");
					foreach (Toolbar toolbar in this.Toolbars)
					{
						foreach (ToolbarItem toolbarItem in toolbar.Items)
						{
							if (toolbarItem is ToolbarDropDownList)
							{
								if (((ToolbarDropDownList)toolbarItem).IsBuiltIn && ((ToolbarDropDownList)toolbarItem).Items.Count == 0)
								{
									Helper.PopulateDefaultDropDownList((ToolbarDropDownList)toolbarItem, this, this.resourceManager);
								}
							}
							else
							{
								HttpContext.Current.Trace.Write("ToolbarItems", toolbarItem.Title + ":" + toolbarItem.GetType());
							}
						}
					}
					this.SetupToolbarButtonStyles();
				}
				this.Page.RegisterRequiresPostBack(this);
				ClientScriptWrapper.RegisterRequiresPostBack(this.Page, this);
				this.RegisterClientScript();
			}
			base.OnPreRender(e);
		}
		private void SetupToolbarButtonStyles()
		{
			if (this.ToolbarStyleConfiguration != ToolbarStyleConfiguration.NotSet)
			{
				this.SetButtonStyle(this.ButtonStyle, this.ToolbarStyleConfiguration, true);
				this.SetButtonStyle(this.ButtonStyleActive, this.ToolbarStyleConfiguration, false);
				switch (this.ToolbarStyleConfiguration)
				{
				case ToolbarStyleConfiguration.OfficeXP:
					this.ButtonSet = ToolbarStyleConfiguration.OfficeXP;
					this.ButtonFolder = "OfficeXP";
					this.BackColor = ColorTranslator.FromHtml("#D4D0C8");
					this.GutterBackColor = ColorTranslator.FromHtml("#BFBCB6");
					this.ButtonOverImage = true;
					this.ButtonDownImage = false;
					this.ToolbarBackColor = ColorTranslator.FromHtml("#DEDED6");
					this.ToolbarBackgroundImage = false;
					this.ButtonWidth = 21;
					this.ButtonHeight = 20;
					return;
				case ToolbarStyleConfiguration.Office2000:
					this.ButtonSet = ToolbarStyleConfiguration.Office2000;
					this.ButtonFolder = "Office2000";
					this.BackColor = ColorTranslator.FromHtml("#D4D0C8");
					this.GutterBackColor = ColorTranslator.FromHtml("#BFBCB6");
					this.ButtonOverImage = false;
					this.ButtonDownImage = false;
					this.ToolbarBackColor = Color.Transparent;
					this.ToolbarBackgroundImage = false;
					this.ButtonWidth = 21;
					this.ButtonHeight = 20;
					return;
				case ToolbarStyleConfiguration.Office2003:
					IL_50:
					this.ButtonSet = ToolbarStyleConfiguration.Office2003;
					this.ButtonFolder = "Images";
					this.BackColor = ColorTranslator.FromHtml("#9EBEF5");
					this.GutterBackColor = ColorTranslator.FromHtml("#81A9E2");
					this.ButtonOverImage = false;
					this.ButtonDownImage = false;
					this.ToolbarBackColor = Color.Transparent;
					this.ToolbarBackgroundImage = true;
					this.ButtonWidth = 21;
					this.ButtonHeight = 20;
					return;
				case ToolbarStyleConfiguration.OfficeMac:
					this.ButtonSet = ToolbarStyleConfiguration.OfficeMac;
					this.ButtonFolder = "OfficeMac";
					this.BackColor = ColorTranslator.FromHtml("#e0dedd");
					this.GutterBackColor = ColorTranslator.FromHtml("#cccccc");
					this.ButtonWidth = 26;
					this.ButtonHeight = 26;
					return;
				}
				goto IL_50;
			}
		}
		private void SetButtonStyle(ToolbarButtonStyle style, ToolbarStyleConfiguration toolbarStyle, bool normal)
		{
			switch (toolbarStyle)
			{
			case ToolbarStyleConfiguration.OfficeXP:
				if (normal)
				{
					style.UseBackgroundImage = false;
					style.UseOverBackgroundImage = false;
					style.BackColor = Color.Transparent;
					style.BorderColorLight = Color.Transparent;
					style.BorderColorDark = Color.Transparent;
					style.OverBackColor = ColorTranslator.FromHtml("#B5BDD6");
					style.OverBorderColorLight = ColorTranslator.FromHtml("#3169C6");
					style.OverBorderColorDark = ColorTranslator.FromHtml("#3169C6");
					style.DownBackColor = ColorTranslator.FromHtml("#8592B5");
					style.DownBorderColorLight = ColorTranslator.FromHtml("#3169C6");
					style.DownBorderColorDark = ColorTranslator.FromHtml("#3169C6");
					return;
				}
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
				return;
			case ToolbarStyleConfiguration.Office2000:
				if (normal)
				{
					style.UseBackgroundImage = false;
					style.UseOverBackgroundImage = false;
					style.OverBackColor = ColorTranslator.FromHtml("#D4D0C8");
					style.OverBorderColorLight = ColorTranslator.FromHtml("#FFFFFF");
					style.OverBorderColorDark = ColorTranslator.FromHtml("#808080");
					style.DownBackColor = ColorTranslator.FromHtml("#D4D0C8");
					style.DownBorderColorLight = ColorTranslator.FromHtml("#808080");
					style.DownBorderColorDark = ColorTranslator.FromHtml("#FFFFFF");
					return;
				}
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
				style.DownBorderColorDark = ColorTranslator.FromHtml("#FFFFFF");
				return;
			case ToolbarStyleConfiguration.Office2003:
				IL_1A:
				if (normal)
				{
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
					return;
				}
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
				return;
			case ToolbarStyleConfiguration.OfficeMac:
				if (normal)
				{
					style.UseBackgroundImage = false;
					style.UseOverBackgroundImage = false;
					style.UseBackgroundImage = true;
					style.UseOverBackgroundImage = true;
					style.OverBackColor = Color.Transparent;
					style.OverBackColorGradient = Color.Transparent;
					style.OverBorderColorLight = Color.Transparent;
					style.OverBorderColorDark = Color.Transparent;
					return;
				}
				style.UseBackgroundImage = true;
				style.UseOverBackgroundImage = true;
				style.BackColor = Color.Transparent;
				style.BackColorGradient = Color.Transparent;
				style.BorderColorLight = Color.Transparent;
				style.BorderColorDark = Color.Transparent;
				style.OverBorderColorLight = Color.Transparent;
				style.OverBorderColorDark = Color.Transparent;
				style.OverBackColor = Color.Transparent;
				style.OverBackColorGradient = Color.Transparent;
				return;
			}
			goto IL_1A;
		}
		private string RepeatString(string input, int times)
		{
			string text = "";
			for (int i = 1; i <= times; i++)
			{
				text += input;
			}
			return text;
		}
		protected virtual void RegisterClientScript()
		{
			if (this.RenderMode == RenderMode.Rich || (this.RenderMode == RenderMode.NotSet && this.browserInfo.IsRichCapable))
			{
				ClientScriptWrapper.RegisterOnSubmitStatement(this.Page, base.GetType(), this.ClientID + "_OnSubmit", "FTB_API['" + this.ClientID + "'].StoreHtml();");
				if (!ClientScriptWrapper.IsClientScriptBlockRegistered(this.Page, "FTB-Scripts"))
				{
					string text = string.Concat(new string[]
					{
						"<script type=\"text/javascript\" src=\"",
						this.CreateResourceString("FTB-Utility.js", ResourceType.JavaScript),
						"\"></script>\r\n<script type=\"text/javascript\" src=\"",
						this.CreateResourceString("FTB-FreeTextBox.js", ResourceType.JavaScript),
						"\"></script>\r\n<script type=\"text/javascript\" src=\"",
						this.CreateResourceString("FTB-ToolbarItems.js", ResourceType.JavaScript),
						"\"></script>"
					});
					if (this.license.IsPro)
					{
						text = text + "<script type=\"text/javascript\" src=\"" + this.CreateResourceString("FTB-Pro.js", ResourceType.JavaScript) + "\"></script>";
					}
					ClientScriptWrapper.RegisterClientScriptBlock(this.Page, this, "FTB-Scripts", text);
				}
				if (!ClientScriptWrapper.IsClientScriptBlockRegistered(this.Page, "FreeTextBoxInfo"))
				{
					ArrayList arrayList = new ArrayList();
					string fullName = typeof(FreeTextBox).Assembly.FullName;
					string text2 = fullName.Substring(fullName.IndexOf("=") + 1);
					text2 = text2.Substring(0, text2.IndexOf(","));
					arrayList.Add("FreeTextBox v3 (" + text2 + ")");
					arrayList.Add("http://www.freetextbox.com/");
					arrayList.Add("ASP.NET HTML editor for PC/IE & Mozilla");
					arrayList.Add(string.Concat(new string[]
					{
						"License Type: ",
						this.license.LicenseKey,
						" (To: ",
						this.license.Data,
						")"
					}));
					int num = 0;
					foreach (string text3 in arrayList)
					{
						if (text3.Length > num)
						{
							num = text3.Length;
						}
					}
					num += 5;
					string text4 = "\r\n<!-- **" + this.RepeatString("*", num) + "* -->";
					foreach (string text5 in arrayList)
					{
						string text6 = text4;
						text4 = string.Concat(new string[]
						{
							text6,
							"\r\n<!-- * ",
							text5,
							this.RepeatString(" ", num - text5.Length),
							"* -->"
						});
					}
					text4 = text4 + "\r\n<!-- **" + this.RepeatString("*", num) + "* -->\r\n";
					if (!MsAjaxProxy.Current.IsScriptManagerInAsyncPostBack(this.Page))
					{
						ClientScriptWrapper.RegisterClientScriptBlock(this.Page, this, "FreeTextBoxInfo", text4);
					}
				}
				if (!ClientScriptWrapper.IsStartupScriptRegistered(this.Page, "FreeTextBox_" + this.ClientID + "_Startup"))
				{
					string text7 = this.CreateStartupString();
					text7 = string.Concat(new string[]
					{
						"\r\n<script type=\"text/javascript\">\r\nif (window.FTB_AddEvent) { \r\n\t",
						(!MsAjaxProxy.Current.IsScriptManagerInAsyncPostBack(this.Page)) ? "FTB_AddEvent(window,'load',function () {" : "",
						"\r\n    ",
						text7,
						"\r\n\t",
						(!MsAjaxProxy.Current.IsScriptManagerInAsyncPostBack(this.Page)) ? "});" : "",
						"\r\n} else {\r\n"
					});
					string text8 = "FreeTextBox has not been correctly installed. To install FreeTextBox either:\\n (1) add a reference to FtbWebResource.axd in web.config:\\n<system.web>\\n<httpHandlers>\\n<add verb=\"GET\"\\npath=\"FtbWebResource.axd\"\\ntype=\"FreeTextBoxControls.AssemblyResourceHandler, FreeTextBox\" />\\n</httpHandlers>\\n</system.web>\\n\\n(2) Save the FreeTextBox image and javascript files to a location on your website and set up FreeTextBox as follows \\n<FTB:FreeTextBox id=\"FreeTextBox1\" SupportFolder=\"ftbfileslocation\" JavaScriptLocation=\"ExternalFile\" ButtonImagesLocation=\"ExternalFile\" ToolbarImagesLocation=\"ExternalFile\" ButtonImagesLocation=\"ExternalFile\" runat=\"server\" />";
					switch (this.InstallationErrorMessage)
					{
					case InstallationErrorMessage.None:
						goto IL_497;
					case InstallationErrorMessage.JavaScriptAlert:
						IL_3EE:
						text7 = text7 + "alert('" + text8 + "');";
						goto IL_497;
					case InstallationErrorMessage.InlineMessage:
					{
						string text9 = text7;
						text7 = string.Concat(new string[]
						{
							text9,
							this.browserInfo.IsIE ? ("ed = eval('" + this.ClientID + "_designEditor');") : ("ed = document.getElementById('" + this.ClientID + "_designEditor').contentWindow;"),
							"ed.document.open(); ed.document.write('",
							this.Page.Server.HtmlEncode(text8).Replace("\\n", "<br>"),
							"');ed.document.close();"
						});
						goto IL_497;
					}
					}
					goto IL_3EE;
					IL_497:
					text7 += "\r\n}\r\n</script>";
					ClientScriptWrapper.RegisterStartupScript(this.Page, base.GetType(), "FreeTextBox_" + this.ClientID + "_Startup", text7);
				}
			}
		}
		private string CreateStartupString()
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
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
								ToolbarButton toolbarButton = (ToolbarButton)toolbarItem;
								string text = string.Concat(new string[]
								{
									"new FTB_Button('",
									this.CreateUniqueToolbarItemID(toolbar, toolbarItem),
									"','",
									toolbarButton.CommandIdentifier,
									"'"
								});
								if (toolbarButton.BuiltInScript != string.Empty)
								{
									text = text + ",function() { " + toolbarButton.BuiltInScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
								}
								else
								{
									if (toolbarButton.ScriptBlock != string.Empty)
									{
										text = text + ",function() { " + toolbarButton.ScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
									}
									else
									{
										text += ",null";
									}
								}
								if (toolbarButton.BuiltInStateScript != string.Empty)
								{
									text = text + ",function() { " + toolbarButton.BuiltInStateScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
								}
								else
								{
									if (toolbarButton.StateScriptBlock != string.Empty)
									{
										text = text + ",function() { " + toolbarButton.StateScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
									}
									else
									{
										text += ",null";
									}
								}
								text = text + "," + toolbarButton.htmlModeEnabled.ToString().ToLower();
								if (toolbarButton.BuiltInEnabledScript != string.Empty)
								{
									text = text + ",function() { " + toolbarButton.BuiltInEnabledScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
								}
								else
								{
									if (toolbarButton.EnabledScriptBlock != string.Empty)
									{
										text = text + ",function() { " + toolbarButton.EnabledScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
									}
									else
									{
										text += ",null";
									}
								}
								text += ")";
								arrayList.Add(text);
							}
							else
							{
								if (toolbarItem is ToolbarDropDownList)
								{
									ToolbarDropDownList toolbarDropDownList = (ToolbarDropDownList)toolbarItem;
									string text2 = string.Concat(new string[]
									{
										"new FTB_DropDownList('",
										this.CreateUniqueToolbarItemID(toolbar, toolbarItem),
										"','",
										toolbarDropDownList.CommandIdentifier,
										"'"
									});
									if (toolbarDropDownList.BuiltInScript != string.Empty)
									{
										text2 = text2 + ",function() { " + toolbarDropDownList.BuiltInScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
									}
									else
									{
										if (toolbarDropDownList.ScriptBlock != string.Empty)
										{
											text2 = text2 + ",function() { " + toolbarDropDownList.ScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
										}
										else
										{
											text2 += ",null";
										}
									}
									if (toolbarDropDownList.BuiltInStateScript != string.Empty)
									{
										text2 = text2 + ",function() { " + toolbarDropDownList.BuiltInStateScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
									}
									else
									{
										if (toolbarDropDownList.StateScriptBlock != string.Empty)
										{
											text2 = text2 + ",function() { " + toolbarDropDownList.StateScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
										}
										else
										{
											text2 += ",null";
										}
									}
									if (toolbarDropDownList.BuiltInEnabledScript != string.Empty)
									{
										text2 = text2 + ",function() { " + toolbarDropDownList.BuiltInEnabledScript.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
									}
									else
									{
										if (toolbarDropDownList.EnabledScriptBlock != string.Empty)
										{
											text2 = text2 + ",function() { " + toolbarDropDownList.EnabledScriptBlock.Replace("\r", "").Replace("\n", "").Replace("\t", "") + " }";
										}
										else
										{
											text2 += ",null";
										}
									}
									text2 += ")";
									arrayList2.Add(text2);
								}
							}
						}
					}
				}
			}
			string str = string.Concat(new object[]
			{
				"\r\n        FTB_API['",
				this.ClientID,
				"'] = new FTB_FreeTextBox('",
				this.ClientID,
				"',\r\n\t\t\t\t\t",
				this.EnableToolbars.ToString().ToLower(),
				",\r\n\t\t\t\t\t",
				this.ReadOnly.ToString().ToLower(),
				",\r\n\t\t\t\t\t",
				this.EnableToolbars ? ("new Array(\r\n\t\t\t\t\t\t" + string.Join(",\n\t\t\t", (string[])arrayList.ToArray(typeof(string))) + "\r\n\t\t\t\t\t)") : "null",
				",\r\n\t\t\t\t\t",
				this.EnableToolbars ? ("new Array(\r\n\t\t\t\t\t\t" + string.Join(",\n\t\t\t", (string[])arrayList2.ToArray(typeof(string))) + "\r\n\t\t\t\t\t)") : "null",
				",\t\t\t\t\r\n\t\t\t\t\t",
				(this.BreakMode == BreakMode.Paragraph) ? "FTB_BREAK_P" : "FTB_BREAK_BR",
				",\r\n\t\t\t\t\tFTB_PASTE_",
				this.PasteMode.ToString().ToUpper(),
				",\r\n\t\t\t\t\tFTB_TAB_",
				this.TabMode.ToString().ToUpper(),
				",\r\n\t\t\t\t\tFTB_MODE_",
				(this.StartMode == EditorMode.DesignMode) ? "DESIGN" : "HTML",
				",\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t",
				(this.ClientSideTextChanged != string.Empty) ? this.ClientSideTextChanged : "null",
				",\r\n\t\t\t\t\t'",
				this.DesignModeCss,
				"',\r\n\t\t\t\t\t'",
				this.DesignModeBodyTagCssClass,
				"',\r\n\t\t\t\t\t'",
				this.BaseUrl,
				"',\r\n\t\t\t\t\t'",
				(this.TextDirection == TextDirection.RightToLeft) ? "rtl" : "",
				"',\r\n\t\t\t\t\t'',\r\n\t\t\t\t\t'",
				this.ImageGalleryUrl,
				"',\r\n\t\t\t\t\t'",
				this.ImageGalleryPath,
				"',\r\n\t\t\t\t\t",
				this.Focus.ToString().ToLower(),
				",\r\n\t\t\t\t\t",
				this.ButtonWidth,
				",\r\n\t\t\t\t\t",
				this.ButtonHeight,
				"\r\n\t\t\t\t\t"
			});
			return str + "\r\n\t\t\t\t);";
		}
		public static bool IsRichCapable(HttpContext context)
		{
			BrowserInfo browserInfo = BrowserInfo.GetBrowserInfo();
			return browserInfo.IsRichCapable;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (this.browserInfo == null)
			{
				return;
			}
			if ((this.browserInfo.IsRichCapable && this.RenderMode != RenderMode.Plain) || this.RenderMode == RenderMode.Rich)
			{
				this.RenderRichEditor(writer);
				return;
			}
			this.RenderDownLevel(writer);
		}
		protected virtual void RenderDownLevel(HtmlTextWriter writer)
		{
			switch (this.DownLevelMode)
			{
			case DownLevelMode.TextArea:
				IL_19:
				writer.WriteLine(string.Concat(new string[]
				{
					"<textarea id=\"",
					this.ClientID,
					"\" name=\"",
					this.ClientID,
					"\"",
					(this.HtmlModeCssClass != string.Empty) ? ("class=\"" + this.HtmlModeCssClass + "\"") : "",
					" cols=\"",
					this.DownLevelCols.ToString(),
					"\" rows=\"",
					this.DownLevelRows.ToString(),
					"\"",
					this.ReadOnly ? " readonly=\"readonly\" disabled=\"true\" onFocus=\"this.blur();\"" : "",
					">",
					this.Text,
					"</textarea>"
				}));
				return;
			case DownLevelMode.InlineHtml:
				writer.WriteLine(string.Concat(new string[]
				{
					"<input type=\"hidden\" id=\"",
					this.ClientID,
					"\" name=\"",
					this.ClientID,
					"\" value=\"",
					HttpContext.Current.Server.HtmlEncode(this.Text),
					"\" />"
				}));
				writer.WriteLine(this.Text);
				return;
			case DownLevelMode.Message:
				writer.WriteLine(string.Concat(new string[]
				{
					"<input type=\"hidden\" id=\"",
					this.ClientID,
					"\" name=\"",
					this.ClientID,
					"\" value=\"",
					HttpContext.Current.Server.HtmlEncode(this.Text),
					"\" />"
				}));
				writer.WriteLine(this.DownLevelMessage);
				return;
			}
			goto IL_19;
		}
		protected virtual void RenderRichEditor(HtmlTextWriter writer)
		{
			this.hasToolbars = (this.toolbars != null && this.toolbars.Count > 0);
			writer.WriteLine("<style type=\"text/css\">");
			this.RenderEditorStyles(writer);
			this.RenderButtonStyles(writer);
			this.RenderTabStyles(writer);
			writer.WriteLine("</style>");
			writer.WriteLine("<table cellpadding=\"2\" cellspacing=\"0\" class=\"" + this.ClientID + "_OuterTable\"><tr><td>");
			writer.WriteLine("<div>");
			if (this.hasToolbars && this.EnableToolbars)
			{
				writer.WriteLine("<div id=\"" + this.ClientID + "_toolbarArea\" style=\"padding-bottom:2px;clear:both;\">");
				foreach (Toolbar toolbar in this.Toolbars)
				{
					toolbar.RepeatDirection = RepeatDirection.Horizontal;
					this.RenderToolbar(writer, toolbar);
				}
				writer.WriteLine("</div>");
			}
			string text = this.Width.ToString();
			string text2 = this.Height.ToString();
			string text3 = this.Width.ToString();
			string text4 = this.Height.ToString();
			if (this.browserInfo.IsIE)
			{
				if (this.Width.ToString().IndexOf("%") == -1)
				{
					text3 = (this.Width.Value - 2.0).ToString() + "px";
				}
				if (this.Height.ToString().IndexOf("%") == -1)
				{
					text4 = (this.Height.Value - 1.0).ToString() + "px";
				}
			}
			writer.WriteLine(string.Concat(new string[]
			{
				"\r\n\t<div id=\"",
				this.ClientID,
				"_designEditorArea\" style=\"clear:both;padding-top:1px;\">\r\n\t    <iframe id=\"",
				this.ClientID,
				"_designEditor\" style=\"padding: 0px; width:",
				text3,
				"; height: ",
				text4,
				";\" src=\"",
				this.EnableSsl ? this.SslUrl.ToString() : "about:blank",
				"\" class=\"",
				this.ClientID,
				"_DesignBox\"></iframe>\r\n\t</div>\r\n\t<div id=\"",
				this.ClientID,
				"_htmlEditorArea\" style=\"clear:both;display:none;padding-bottom:1px;\">\r\n\t    <textarea id=\"",
				this.ClientID,
				"\" name=\"",
				this.ClientID,
				"\" disabled=\"disabled\" style=\"padding: 0px; width:",
				text,
				"; height: ",
				text2,
				";",
				(this.TextDirection == TextDirection.RightToLeft) ? "direction:rtl;" : "",
				"\" class=\"",
				(this.HtmlModeCssClass == string.Empty) ? (this.ClientID + "_HtmlBox") : this.HtmlModeCssClass,
				"\">",
				this.Page.Server.HtmlEncode(this.Text),
				"</textarea>\r\n\t</div>\r\n\t<div id=\"",
				this.ClientID,
				"_previewPaneArea\" style=\"clear:both;display:none;padding-bottom:1px;\">\r\n\t    <iframe id=\"",
				this.ClientID,
				"_previewPane\" style=\"padding: 0px; width:",
				text3,
				"; height: ",
				text4,
				"\" src=\"",
				this.EnableSsl ? this.SslUrl.ToString() : "about:blank",
				"\" class=\"",
				this.ClientID,
				"_DesignBox\"></iframe>\r\n\t</div>\r\n"
			}));
			if (this.EnableHtmlMode)
			{
				writer.WriteLine("<div style=\"clear:both;padding-top:2px;\">");
				this.RenderTabs(writer);
				writer.WriteLine("</div>");
			}
			writer.WriteLine("</div>");
			writer.WriteLine("</table>");
		}
		protected virtual void RenderTabs(HtmlTextWriter writer)
		{
			string text = this.ClientID + "_TabOn";
			string text2 = this.ClientID + "_TabOffRight";
			string text3 = this.ClientID + "_StartTabOn";
			if (this.StartMode == EditorMode.HtmlMode)
			{
				text3 = this.ClientID + "_StartTabOff";
				text = this.ClientID + "_TabOffLeft";
				text2 = this.ClientID + "_TabOn";
			}
			writer.WriteLine(string.Concat(new string[]
			{
				"\r\n\t<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"border-collapse:collapse;\">\r\n\t\t<tr id=\"",
				this.ClientID,
				"_TabRow\">\r\n\t\t\t<td class=\"",
				text3,
				"\">\r\n\t\t\t\t&nbsp;\r\n\t\t\t</td>\r\n\t\t\t<td class=\"",
				text,
				"\" id=\"",
				this.ClientID,
				"_designModeTab\" unselectable=\"on\">\r\n\t\t\t\t<nobr><img unselectable=\"on\" src=\"",
				this.CreateResourceString("mode.design", ResourceType.Button),
				"\" align=\"absmiddle\" width=\"21\" height=\"20\">&nbsp;",
				this.resourceManager.GetString("DesignModeTab"),
				"</nobr>\r\n\t\t\t</td>\r\n\t\t\t<td class=\"",
				text2,
				"\" ID=\"",
				this.ClientID,
				"_htmlModeTab\" unselectable=\"on\">\r\n\t\t\t\t<nobr><img unselectable=\"on\" SRC=\"",
				this.CreateResourceString("mode.html", ResourceType.Button),
				"\" align=\"absmiddle\" width=\"21\" height=\"20\">&nbsp;",
				this.resourceManager.GetString("HtmlModeTab"),
				"</nobr>\r\n\t\t\t</td>\r\n\t\t\t<td class=\"",
				this.ClientID,
				"_EndTab\">\r\n\t\t\t\t<div id=\"",
				this.ClientID,
				"_AncestorArea\" class=\"",
				this.ClientID,
				"_AncestorArea\"></div>\r\n\t\t\t</td>\r\n\t\t</tr>\r\n\t</table>\r\n\t"
			}));
		}
		public virtual void RenderToolbar(HtmlTextWriter writer, Toolbar toolbar)
		{
			writer.WriteLine("<div class=\"" + this.ClientID + "_Toolbar\">");
			writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" " + (this.ToolbarBackgroundImage ? ("style=\"background-image:url(" + this.CreateResourceString("toolbar." + RepeatDirection.Horizontal.ToString().ToLower() + ".background", ResourceType.Toolbar) + ");\"") : "") + "><tr><td border=\"0\" unselectable=\"on\">");
			writer.Write("<img src=\"" + this.CreateResourceString("toolbar." + toolbar.RepeatDirection.ToString().ToLower() + ".start", ResourceType.Toolbar) + "\" align=\"center\" />");
			if (toolbar.RepeatDirection == RepeatDirection.Horizontal)
			{
				writer.Write("</td><td>");
			}
			writer.Write("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"" + this.ClientID + "_items\">");
			if (toolbar.RepeatDirection == RepeatDirection.Horizontal)
			{
				writer.Write("<tr>");
			}
			foreach (ToolbarItem toolbarItem in toolbar.Items)
			{
				if (this.license.IsPro || !toolbarItem.isProFeature)
				{
					if (toolbarItem is ToolbarButton)
					{
						ToolbarButton toolbarButton = (ToolbarButton)toolbarItem;
						this.RenderToolbarButton(writer, toolbar, toolbarButton);
					}
					else
					{
						if (toolbarItem is ToolbarDropDownList)
						{
							ToolbarDropDownList toolbarDropDownList = (ToolbarDropDownList)toolbarItem;
							this.RenderToolbarDropDownList(writer, toolbar, toolbarDropDownList);
						}
						else
						{
							if (toolbarItem is ToolbarSeparator)
							{
								this.RenderToolbarSeparator(writer, toolbar);
							}
						}
					}
				}
			}
			if (toolbar.RepeatDirection == RepeatDirection.Horizontal)
			{
				writer.Write("</tr>");
			}
			writer.Write("</table>");
			if (toolbar.RepeatDirection == RepeatDirection.Horizontal)
			{
				writer.Write("</td><td>");
			}
			writer.Write("<img src=\"" + this.CreateResourceString("toolbar." + toolbar.RepeatDirection.ToString().ToLower() + ".end", ResourceType.Toolbar) + "\" border=\"0\" unselectable=\"on\" align=\"center\" />");
			writer.Write("</td></tr></table>");
			writer.Write("</div>");
		}
		public virtual void RenderToolbarButton(HtmlTextWriter writer, Toolbar toolbar, ToolbarButton toolbarButton)
		{
			if (toolbarButton.IsBuiltIn)
			{
				this.SetToolbarButtonLanguage(toolbarButton);
			}
			if (toolbar.RepeatDirection == RepeatDirection.Vertical)
			{
				writer.WriteLine("<tr>");
			}
			writer.Write(string.Concat(new string[]
			{
				"<td id=\"",
				this.CreateUniqueToolbarItemID(toolbar, toolbarButton),
				"\" class=\"",
				this.ClientID,
				"_Button_Off_Out\">"
			}));
			if (toolbarButton.isBuiltIn)
			{
				writer.Write(string.Concat(new object[]
				{
					"<img src=\"",
					this.CreateResourceString(toolbarButton.ButtonImage, ResourceType.Button),
					"\" border=\"0\" title=\"",
					toolbarButton.Title,
					"\" unselectable=\"on\" width=\"",
					this.ButtonWidth,
					"\" height=\"",
					this.ButtonHeight,
					"\" tabindex=\"-1\" align=\"center\" />"
				}));
			}
			else
			{
				writer.Write(string.Concat(new object[]
				{
					"<img src=\"",
					this.SupportFolder,
					this.ButtonFolder,
					"/",
					toolbarButton.ButtonImage,
					".",
					this.ButtonFileExtention,
					"\" border=\"0\" title=\"",
					toolbarButton.Title,
					"\" unselectable=\"on\" width=\"",
					this.ButtonWidth,
					"\" height=\"",
					this.ButtonHeight,
					"\" tabindex=\"-1\" align=\"center\" />"
				}));
			}
			writer.Write("</td>\n");
			if (toolbar.RepeatDirection == RepeatDirection.Vertical)
			{
				writer.Write("</tr>");
			}
		}
		public virtual void SetToolbarButtonLanguage(ToolbarButton toolbarButton)
		{
			if (!toolbarButton.TitleHasBeenSet)
			{
				toolbarButton.SetTitleLanguage(this.resourceManager.GetString(toolbarButton.className));
			}
		}
		public virtual void RenderToolbarDropDownList(HtmlTextWriter writer, Toolbar toolbar, ToolbarDropDownList toolbarDropDownList)
		{
			if (toolbarDropDownList.IsBuiltIn)
			{
				this.SetToolbarDropDownListLanguage(toolbarDropDownList);
			}
			if (toolbar.RepeatDirection == RepeatDirection.Vertical)
			{
				writer.WriteLine("<tr>");
			}
			writer.Write("<td style=\"padding-left:4px;\" unselectable=\"on\">");
			writer.Write(string.Concat(new string[]
			{
				"<select id=\"",
				this.CreateUniqueToolbarItemID(toolbar, toolbarDropDownList),
				"\" TabIndex=\"-1\" ",
				(this.DropDownListCssClass != string.Empty) ? (" class=\"" + this.DropDownListCssClass + "\"") : "",
				(this.TextDirection == TextDirection.RightToLeft) ? "style=\"direction:rtl;\"" : "",
				">"
			}));
			writer.Write("<option value=\"\">" + toolbarDropDownList.Title + "</option>");
			foreach (ToolbarListItem toolbarListItem in toolbarDropDownList.Items)
			{
				if (toolbarDropDownList is ParagraphMenu)
				{
					writer.Write("<option value=\"" + toolbarListItem.Value + "\"");
				}
				else
				{
					writer.Write("<option value=\"" + this.Page.Server.HtmlEncode(toolbarListItem.Value) + "\"");
				}
				if (toolbarListItem.BackColor != Color.Transparent)
				{
					writer.Write(" style=\"background-color: " + ColorTranslator.ToHtml(toolbarListItem.BackColor) + ";");
					if ((double)toolbarListItem.BackColor.GetBrightness() < 0.4)
					{
						writer.Write("color: #FFFFFF;");
					}
					writer.Write("\"");
				}
				writer.Write(">" + toolbarListItem.Text + "</option>\n");
			}
			writer.Write("</select>");
			writer.Write("</td>");
			if (toolbar.RepeatDirection == RepeatDirection.Vertical)
			{
				writer.Write("</tr>");
			}
		}
		private void SetToolbarDropDownListLanguage(ToolbarDropDownList toolbarDropDownList)
		{
			if (!toolbarDropDownList.TitleHasBeenSet)
			{
				toolbarDropDownList.SetTitleLanguage(this.resourceManager.GetString(toolbarDropDownList.className));
			}
		}
		public virtual void RenderToolbarSeparator(HtmlTextWriter writer, Toolbar toolbar)
		{
			if (toolbar.RepeatDirection == RepeatDirection.Vertical)
			{
				writer.WriteLine("<tr>");
			}
			writer.WriteLine("<td><img src=\"" + this.CreateResourceString("separator." + toolbar.RepeatDirection.ToString().ToLower(), ResourceType.Toolbar) + "\" border=0 unselectable=\"on\"></td>");
			if (toolbar.RepeatDirection == RepeatDirection.Vertical)
			{
				writer.WriteLine("</tr>");
			}
		}
		protected virtual void RenderEditorStyles(HtmlTextWriter writer)
		{
			string[] array = new string[41];
			array[0] = "\r\n.";
			array[1] = this.ClientID;
			array[2] = "_OuterTable {\r\n\twidth: ";
			array[3] = this.Width.ToString();
			array[4] = ";\r\n\tbackground-color: ";
			array[5] = ColorTranslator.ToHtml(this.BackColor);
			array[6] = ";\r\n}\r\n#";
			array[7] = this.ClientID;
			array[8] = "_toolbarArea td {\r\n\tvertical-align: middle;\r\n\tborder-collapse: separate;\r\n}\r\n#";
			array[9] = this.ClientID;
			array[10] = "_toolbarArea select {\r\n\tmargin: 0px;\r\n\tpadding: 0px;\r\n\tfont: 11px Tahoma,Verdana,sans-serif;\r\n}\r\n#";
			array[11] = this.ClientID;
			array[12] = "_toolbarArea img {\r\n\tdisplay: block;\r\n}\r\n.";
			array[13] = this.ClientID;
			array[14] = "_HtmlBox {\r\n\toverflow: auto;\r\n\tfont-family: Courier New, Courier;\r\n\tpadding: 4px;\r\n\tborder-right: 1px solid ";
			array[15] = ColorTranslator.ToHtml(this.EditorBorderColorLight);
			array[16] = ";\r\n\tborder-left: 1px solid ";
			array[17] = ColorTranslator.ToHtml(this.EditorBorderColorDark);
			array[18] = ";\r\n\tborder-top: 1px solid ";
			array[19] = ColorTranslator.ToHtml(this.EditorBorderColorDark);
			array[20] = ";\r\n\tborder-bottom: 1px solid ";
			array[21] = ColorTranslator.ToHtml(this.EditorBorderColorLight);
			array[22] = ";\r\n}\r\n.";
			array[23] = this.ClientID;
			array[24] = "_DesignBox {\r\n\t";
			array[25] = ((!this.browserInfo.IsIE || this.DesignModeCss == "") ? "background-color: #FFFFFF;" : "");
			array[26] = "\r\n\tborder: 0; \r\n\tborder-right: 1px solid ";
			array[27] = ColorTranslator.ToHtml(this.EditorBorderColorLight);
			array[28] = ";\r\n\tborder-left: 1px solid ";
			array[29] = ColorTranslator.ToHtml(this.EditorBorderColorDark);
			array[30] = ";\r\n\tborder-top: 1px solid ";
			array[31] = ColorTranslator.ToHtml(this.EditorBorderColorDark);
			array[32] = ";\r\n\tborder-bottom: 1px solid ";
			array[33] = ColorTranslator.ToHtml(this.EditorBorderColorLight);
			array[34] = ";\r\n}\r\n.";
			array[35] = this.ClientID;
			array[36] = "_DesignBox body {\r\n\tbackground-color: black;\r\n}\r\n.";
			array[37] = this.ClientID;
			array[38] = "_Toolbar {\r\n\tmargin-bottom: 1px; \r\n\tmargin-right: 2px;\r\n\tfloat: left;\r\n\t";
			string[] arg_1E1_0 = array;
			int arg_1E1_1 = 39;
			bool arg_1DB_0 = this.ToolbarBackgroundImage;
			arg_1E1_0[arg_1E1_1] = "";
			array[40] = "\r\n}\r\n";
			writer.WriteLine(string.Concat(array));
		}
		protected virtual void RenderTabStyles(HtmlTextWriter writer)
		{
			writer.WriteLine(string.Concat(new string[]
			{
				"\r\n#",
				this.ClientID,
				"_TabRow td {\r\n\tvertical-align:center;\r\n}\r\n.",
				this.ClientID,
				"_StartTabOn {\r\n\tfont: 10pt MS Sans Serif;\r\n\tpadding: 1px;\r\n\tborder-left: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n\tborder-right: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorLight),
				";\r\n\tborder-top: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorDark),
				";\r\n\tborder-bottom: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n\tbackground-color: ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n}\r\n.",
				this.ClientID,
				"_StartTabOff {\r\n\tfont: 10pt MS Sans Serif;\r\n\tpadding:1px;\r\n\tborder-left: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n\tborder-right: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorDark),
				";\r\n\tborder-top: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorDark),
				";\r\n\tborder-bottom: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n\tbackground-color: ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n}\r\n.",
				this.ClientID,
				"_TabOn {\r\n\tfont: 8pt MS Sans Serif;\r\n\tpadding:1px;\r\n\tpadding-left:5px;\r\n\tpadding-right:5px;\r\n\tborder-left: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorLight),
				";\r\n\tborder-right: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorDark),
				";\r\n\tborder-top: 1px solid ",
				ColorTranslator.ToHtml(this.BackColor),
				";\r\n\tborder-bottom: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorDark),
				";\r\n\tbackground-color: ",
				ColorTranslator.ToHtml(this.BackColor),
				";\t\r\n}\r\n.",
				this.ClientID,
				"_TabOffRight {\r\n\tfont: 8pt MS Sans Serif;\r\n\tpadding:1px;\r\n\tpadding-left:5px;\r\n\tpadding-right:5px;\r\n\tborder-left: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorDark),
				";\r\n\tborder-right: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorDark),
				";\r\n\tborder-top: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorDark),
				";\r\n\tborder-bottom: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n\tbackground-color: ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n}\r\n.",
				this.ClientID,
				"_TabOffLeft {\r\n\tfont: 8pt MS Sans Serif;\r\n\tpadding:1px;\r\n\tpadding-left:5px;\r\n\tpadding-right:5px;\r\n\tborder-left: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorDark),
				";\r\n\tborder-right: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorLight),
				";\r\n\tborder-top: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorDark),
				";\r\n\tborder-bottom: 1px solid ",
				ColorTranslator.ToHtml(this.BackColor),
				";\r\n\tbackground-color: ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n}\r\n.",
				this.ClientID,
				"_EndTab {\r\n\tfont: 10pt MS Sans Serif;\r\n\twidth: 100%;\r\n\tpadding:1px;\r\n\tborder-left: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n\tborder-right: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n\tborder-top: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBorderColorDark),
				";\r\n\tborder-bottom: 1px solid ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n\tbackground-color: ",
				ColorTranslator.ToHtml(this.GutterBackColor),
				";\r\n}\r\n.",
				this.ClientID,
				"_AncestorArea {\r\n\t",
				(!this.ShowTagPath) ? "display:none;" : "",
				"\r\n\tmargin-left: 4px;\r\n}\r\n.",
				this.ClientID,
				"_AncestorArea a {\r\n\tpadding: 1px;\r\n\tmargin-left: 2px;\r\n\tmargin-right: 2px;\r\n\tborder: 1px solid #808080;\t\r\n\tcolor: #000;\r\n\tfont-family: arial;\r\n\tfont-size: 11px;\r\n}\r\n.",
				this.ClientID,
				"_AncestorArea a:link, .",
				this.ClientID,
				"_AncestorArea a:visited, .",
				this.ClientID,
				"_AncestorArea a:active {\r\n\tbackground-color: transparent;\r\n\ttext-decoration: none;\r\n}\r\n.",
				this.ClientID,
				"_AncestorArea a:hover {\r\n\ttext-decoration: none;\r\n\tbackground-color: #316AC5;\r\n\tborder: 1px solid #fff;\r\n\tcolor:#fff;\r\n}\r\n"
			}));
		}
		protected virtual void RenderButtonStyles(HtmlTextWriter writer)
		{
			writer.WriteLine(string.Concat(new string[]
			{
				"\r\n.",
				this.ClientID,
				"_items img {\r\n\tpadding: 1px;\r\n}\r\n.",
				this.ClientID,
				"_Button_Off_Out img {\r\n\t",
				(this.ButtonStyle.BorderColorLight == Color.Transparent && this.ButtonStyle.BorderColorDark == Color.Transparent) ? "padding: 1px;" : string.Concat(new string[]
				{
					"\r\n\tpadding: 0px;\r\n\tborder-top: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyle.BorderColorLight),
					";\t\r\n\tborder-left: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyle.BorderColorLight),
					";\r\n\tborder-right: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyle.BorderColorDark),
					";\r\n\tborder-bottom: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyle.BorderColorDark),
					"; "
				}),
				" \r\n\tbackground-color: ",
				(this.ButtonStyle.BackColor != Color.Transparent) ? ColorTranslator.ToHtml(this.ButtonStyle.BackColor) : "transparent",
				";\r\n}\r\n.",
				this.ClientID,
				"_Button_Off_Over img {\r\n\t",
				(this.ButtonStyle.OverBorderColorLight == Color.Transparent && this.ButtonStyle.OverBorderColorDark == Color.Transparent) ? "padding: 1px;" : string.Concat(new string[]
				{
					"\r\n\tpadding: 0px;\r\n\tborder-top: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyle.OverBorderColorLight),
					";\t\r\n\tborder-left: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyle.OverBorderColorLight),
					";\r\n\tborder-right: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyle.OverBorderColorDark),
					";\r\n\tborder-bottom: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyle.OverBorderColorDark),
					"; "
				}),
				" \r\n\tbackground-color: ",
				(this.ButtonStyle.OverBackColor != Color.Transparent) ? ColorTranslator.ToHtml(this.ButtonStyle.OverBackColor) : "transparent",
				";\r\n\t",
				(this.ButtonStyle.UseOverBackgroundImage && (!this.browserInfo.IsIE || this.ButtonStyle.OverBackColorGradient == Color.Transparent)) ? ("background-image: url(" + this.CreateResourceString("toolbarbuttoncss.off.over", ResourceType.Toolbar) + ");") : "",
				"\r\n\t",
				(this.ButtonStyle.UseOverBackgroundImage && this.browserInfo.IsIE && this.ButtonStyle.OverBackColorGradient != Color.Transparent) ? string.Concat(new string[]
				{
					"filter: progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='#FF",
					ColorTranslator.ToHtml(this.ButtonStyle.OverBackColor).Replace("#", ""),
					"', EndColorStr='#FF",
					ColorTranslator.ToHtml(this.ButtonStyle.OverBackColorGradient).Replace("#", ""),
					"');"
				}) : "",
				"\r\n}\r\n.",
				this.ClientID,
				"_Button_On_Out img {\r\n\t",
				(this.ButtonStyleActive.BorderColorLight == Color.Transparent && this.ButtonStyleActive.BorderColorDark == Color.Transparent) ? "padding: 1px;" : string.Concat(new string[]
				{
					"\r\n\tpadding: 0px;\r\n\tborder-top: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyleActive.BorderColorLight),
					";\t\r\n\tborder-left: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyleActive.BorderColorLight),
					";\r\n\tborder-right: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyleActive.BorderColorDark),
					";\r\n\tborder-bottom: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyleActive.BorderColorDark),
					"; "
				}),
				" \r\n\tbackground-color: ",
				(this.ButtonStyleActive.BackColor != Color.Transparent) ? ColorTranslator.ToHtml(this.ButtonStyleActive.BackColor) : "transparent",
				";\r\n\t",
				(this.ButtonStyleActive.UseBackgroundImage && (!this.browserInfo.IsIE || this.ButtonStyleActive.BackColorGradient == Color.Transparent)) ? ("background-image: url(" + this.CreateResourceString("toolbarbuttoncss.on.out", ResourceType.Toolbar) + ");") : "",
				"\r\n\t",
				(this.ButtonStyleActive.UseBackgroundImage && this.browserInfo.IsIE && this.ButtonStyleActive.BackColorGradient != Color.Transparent) ? string.Concat(new string[]
				{
					"filter: progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='#FF",
					ColorTranslator.ToHtml(this.ButtonStyleActive.BackColor).Replace("#", ""),
					"', EndColorStr='#FF",
					ColorTranslator.ToHtml(this.ButtonStyleActive.BackColorGradient).Replace("#", ""),
					"');"
				}) : "",
				"\r\n}\r\n.",
				this.ClientID,
				"_Button_On_Over img {\r\n\t",
				(this.ButtonStyleActive.OverBorderColorLight == Color.Transparent && this.ButtonStyleActive.OverBorderColorDark == Color.Transparent) ? "padding: 1px;" : string.Concat(new string[]
				{
					"\r\n\tpadding: 0px;\r\n\tborder-top: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyleActive.OverBorderColorLight),
					";\t\r\n\tborder-left: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyleActive.OverBorderColorLight),
					";\r\n\tborder-right: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyleActive.OverBorderColorDark),
					";\r\n\tborder-bottom: 1px solid ",
					ColorTranslator.ToHtml(this.ButtonStyleActive.OverBorderColorDark),
					"; "
				}),
				" \r\n\tbackground-color: ",
				(this.ButtonStyleActive.OverBackColor != Color.Transparent) ? ColorTranslator.ToHtml(this.ButtonStyleActive.OverBackColor) : "transparent",
				";\r\n\t",
				(this.ButtonStyleActive.UseOverBackgroundImage && (!this.browserInfo.IsIE || this.ButtonStyleActive.BackColorGradient == Color.Transparent)) ? ("background-image: url(" + this.CreateResourceString("toolbarbuttoncss.on.over", ResourceType.Toolbar) + ");") : "",
				"\r\n\t",
				(this.ButtonStyleActive.UseOverBackgroundImage && this.browserInfo.IsIE && this.ButtonStyleActive.BackColorGradient != Color.Transparent) ? string.Concat(new string[]
				{
					"filter: progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='#FF",
					ColorTranslator.ToHtml(this.ButtonStyleActive.OverBackColor).Replace("#", ""),
					"', EndColorStr='#FF",
					ColorTranslator.ToHtml(this.ButtonStyleActive.OverBackColorGradient).Replace("#", ""),
					"');"
				}) : "",
				"\r\n}\r\n"
			}));
		}
		private string CreateUniqueToolbarItemID(Toolbar toolbar, ToolbarItem toolbarItem)
		{
			return string.Concat(new string[]
			{
				this.ClientID,
				"_",
				this.Toolbars.IndexOf(toolbar).ToString(),
				"_",
				toolbar.Items.IndexOf(toolbarItem).ToString()
			});
		}
		private string CreateResourceString(string filename, ResourceType resourceType)
		{
			ToolbarStyleConfiguration toolbarStyleConfiguration = this.ButtonSet;
			if (toolbarStyleConfiguration == ToolbarStyleConfiguration.NotSet)
			{
				toolbarStyleConfiguration = ToolbarStyleConfiguration.Office2003;
			}
			switch (resourceType)
			{
			case ResourceType.Button:
				if (this.ButtonImagesLocation == ResourceLocation.InternalResource)
				{
					return ClientScriptWrapper.GetWebResourceUrl(this, string.Concat(new object[]
					{
						"FreeTextBoxControls.Resources.Images.",
						toolbarStyleConfiguration,
						".",
						filename,
						".gif"
					}), this.AssemblyResourceHandlerPath);
				}
				return string.Concat(new string[]
				{
					this.SupportFolder,
					this.ButtonFolder,
					"/",
					filename,
					".",
					this.ButtonFileExtention
				}).ToLower();
			case ResourceType.Toolbar:
				if (this.ToolbarImagesLocation == ResourceLocation.InternalResource)
				{
					return ClientScriptWrapper.GetWebResourceUrl(this, string.Concat(new object[]
					{
						"FreeTextBoxControls.Resources.Images.",
						toolbarStyleConfiguration,
						".",
						filename,
						".gif"
					}), this.AssemblyResourceHandlerPath);
				}
				return string.Concat(new string[]
				{
					this.SupportFolder,
					this.ButtonFolder,
					"/",
					filename,
					".",
					this.ButtonFileExtention
				}).ToLower();
			case ResourceType.JavaScript:
				if (this.JavaScriptLocation == ResourceLocation.InternalResource)
				{
					return ClientScriptWrapper.GetWebResourceUrl(this, "FreeTextBoxControls.Resources.JavaScript." + filename, this.AssemblyResourceHandlerPath);
				}
				return this.SupportFolder + filename;
			default:
				return string.Empty;
			}
		}
	}
}

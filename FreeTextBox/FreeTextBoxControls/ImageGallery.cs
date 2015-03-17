using FreeTextBoxControls.Support;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace FreeTextBoxControls
{
	[ToolboxData("<{0}:ImageGallery runat=\"server\"></{0}:ImageGallery>")]
	public class ImageGallery : Control, IPostBackDataHandler, IPostBackEventHandler
	{
		private HttpRequest request;
		private BrowserInfo browserInfo;
		private HttpResponse response;
		private HttpContext context;
		protected int MaxThumbnailWidth = 100;
		protected int MaxThumbnailHeight = 80;
		protected string returnMessage = "";
		protected HtmlInputFile inputFile;
		private string[] _currentDirectories;
		private FileInfo[] _currentImages;
		[Category("Behavior"), Description("Sets the current folders used (overrides the default read).")]
		public string[] CurrentDirectories
		{
			set
			{
				this._currentDirectories = value;
			}
		}
		[Category("Behavior"), Description("Sets the current images used (overrides the default read).")]
		public FileInfo[] CurrentImages
		{
			set
			{
				this._currentImages = value;
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
				return base.ResolveUrl("~/");
			}
			set
			{
				this.ViewState["AssemblyResourceHandlerPath"] = value;
			}
		}
		[Category("Behavior"), Description("Gets or sets the FreeTextBox to insert images into.")]
		public string TargetFreeTextBox
		{
			get
			{
				object obj = this.ViewState["TargetFreeTextBox"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
			set
			{
				this.ViewState["TargetFreeTextBox"] = value;
			}
		}
		[Category("Behavior"), Description("Gets or sets the root folder where images are stored.")]
		public string RootImagesFolder
		{
			get
			{
				object obj = this.ViewState["RootImagesFolder"];
				if (obj == null)
				{
					return "~/images";
				}
				string text = (string)obj;
				text = text.Replace("\\", "/");
				while (text.IndexOf("//") > -1)
				{
					text = text.Replace("//", "/");
				}
				while (text.EndsWith("/"))
				{
					text = text.Substring(0, text.Length - 1);
				}
				return text;
			}
			set
			{
				this.ViewState["RootImagesFolder"] = value;
			}
		}
		[Category("Behavior"), Description("Gets or sets the current folder the user is browsing.")]
		public string CurrentImagesFolder
		{
			get
			{
				object obj = this.ViewState["CurrentImagesFolder"];
				if (obj == null)
				{
					return "~/images";
				}
				string text = (string)obj;
				text = text.Replace("\\", "/");
				while (text.IndexOf("//") > -1)
				{
					text = text.Replace("//", "/");
				}
				while (text.EndsWith("/"))
				{
					text = text.Substring(0, text.Length - 1);
				}
				return text;
			}
			set
			{
				this.ViewState["CurrentImagesFolder"] = value;
			}
		}
		[Category("Behavior"), Description("Gets or sets the imge file extensions allowed.")]
		public string[] AcceptedFileTypes
		{
			get
			{
				object obj = this.ViewState["AcceptedFileTypes"];
				if (obj != null)
				{
					return (string[])obj;
				}
				return new string[]
				{
					"jpg",
					"jpeg",
					"jpe",
					"gif",
					"bmp",
					"png"
				};
			}
			set
			{
				this.ViewState["AcceptedFileTypes"] = value;
			}
		}
		[Category("Thumbnails"), Description("Gets or sets the maximum height of thumbnails.")]
		public int ThumbnailHeight
		{
			get
			{
				object obj = this.ViewState["ThumbnailHeight"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 94;
			}
			set
			{
				this.ViewState["ThumbnailHeight"] = value;
			}
		}
		[Category("Thumbnails"), Description("Gets or sets the maximum width of thumbnails.")]
		public int ThumbnailWidth
		{
			get
			{
				object obj = this.ViewState["ThumbnailWidth"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 94;
			}
			set
			{
				this.ViewState["ThumbnailWidth"] = value;
			}
		}
		[Category("Behavior"), Description("Gets or sets if uploading files is allowed.")]
		public bool AllowImageUpload
		{
			get
			{
				object obj = this.ViewState["AllowImageUpload"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AllowImageUpload"] = value;
			}
		}
		[Category("Behavior"), Description("Gets or sets if file deleting is allowed.")]
		public bool AllowImageDelete
		{
			get
			{
				object obj = this.ViewState["AllowImageDelete"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AllowImageDelete"] = value;
			}
		}
		[Category("Behavior"), Description("Gets or sets if directory deleting is allowed.")]
		public bool AllowDirectoryDelete
		{
			get
			{
				object obj = this.ViewState["AllowDirectoryDelete"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AllowDirectoryDelete"] = value;
			}
		}
		[Category("Behavior"), Description("Gets or sets if directory creation is allowed.")]
		public bool AllowDirectoryCreate
		{
			get
			{
				object obj = this.ViewState["AllowDirectoryCreate"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AllowDirectoryCreate"] = value;
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
		public ResourceLocation UtilityImagesLocation
		{
			get
			{
				object obj = this.ViewState["UtilityImagesLocation"];
				if (obj != null)
				{
					return (ResourceLocation)obj;
				}
				return ResourceLocation.InternalResource;
			}
			set
			{
				this.ViewState["UtilityImagesLocation"] = value;
			}
		}
		[Category("Behavior"), Description("Gets or sets where the helper ")]
		public string SupportFolder
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
			set
			{
				this.ViewState["SupportFolder"] = value;
			}
		}
		public ImageGallery()
		{
			this.context = HttpContext.Current;
			this.request = this.context.Request;
			this.response = this.context.Response;
			this.browserInfo = new BrowserInfo();
		}
		public virtual void RaisePostBackEvent(string eventArgument)
		{
			this.EnsureChildControls();
			DirectoryInfo directoryInfo = new DirectoryInfo(this.context.Server.MapPath(this.CurrentImagesFolder));
			string[] array = eventArgument.Split(new char[]
			{
				':'
			});
			string a;
			if ((a = array[0]) != null)
			{
				if (!(a == "GoToFolder"))
				{
					if (!(a == "CreateFolder"))
					{
						if (!(a == "DeleteFolder"))
						{
							if (a == "DeleteImage")
							{
								goto IL_1DF;
							}
							if (!(a == "UploadImage"))
							{
								return;
							}
							goto IL_278;
						}
					}
					else
					{
						if (array.Length != 2)
						{
							return;
						}
						if (!this.AllowDirectoryCreate)
						{
							this.returnMessage = "Your permissions do not allow you do create directories";
							return;
						}
						string text = array[1];
						try
						{
							directoryInfo.CreateSubdirectory(text);
							this.returnMessage = "Folder created";
							return;
						}
						catch (Exception ex)
						{
							this.returnMessage = string.Concat(new string[]
							{
								"Error creating folder: ",
								directoryInfo.FullName,
								"\\",
								text,
								": ",
								ex.ToString()
							});
							return;
						}
					}
					if (array.Length != 2)
					{
						return;
					}
					if (!this.AllowDirectoryDelete)
					{
						this.returnMessage = "Your permissions do not allow you do delete directories";
						return;
					}
					string text2 = array[1];
					try
					{
						Directory.Delete(directoryInfo.FullName + "\\" + text2, true);
						this.returnMessage = "Directory deleted.";
						return;
					}
					catch (Exception ex2)
					{
						this.returnMessage = string.Concat(new string[]
						{
							"Failed to delete: ",
							directoryInfo.FullName,
							"\\",
							text2,
							": ",
							ex2.ToString()
						});
						return;
					}
					IL_1DF:
					if (array.Length != 2)
					{
						return;
					}
					if (!this.AllowImageDelete)
					{
						this.returnMessage = "Your permissions do not allow you do delete images";
						return;
					}
					string text3 = array[1];
					try
					{
						File.Delete(directoryInfo.FullName + "\\" + text3);
						this.returnMessage = "Image deleted.";
						return;
					}
					catch (Exception ex3)
					{
						this.returnMessage = string.Concat(new string[]
						{
							"failed to delete: ",
							directoryInfo.FullName,
							"\\",
							text3,
							": ",
							ex3.ToString()
						});
						return;
					}
					IL_278:
					if (!this.AllowImageUpload)
					{
						this.returnMessage = "Your permissions do not allow you do upload images";
						return;
					}
					if (this.inputFile == null)
					{
						this.returnMessage = "Error: InputFile control not yet created!";
						return;
					}
					if (this.inputFile.PostedFile != null && this.inputFile.PostedFile.FileName != null)
					{
						string str = this.inputFile.PostedFile.FileName.Substring(this.inputFile.PostedFile.FileName.LastIndexOf("\\") + 1);
						this.inputFile.PostedFile.SaveAs(this.context.Server.MapPath(this.CurrentImagesFolder) + "\\" + str);
						this.returnMessage = "Image uploaded";
						this.context.Cache.Remove("FTB-Images-" + this.CurrentImagesFolder);
						return;
					}
					throw new Exception("No file was uploaded!!");
				}
				else
				{
					if (array.Length != 2)
					{
						return;
					}
					string text4 = array[1];
					this.CurrentImagesFolder = text4;
					this.returnMessage = "Navigated to " + text4 + ".";
					return;
				}
			}
		}
		public void RaisePostDataChangedEvent()
		{
		}
		public bool LoadPostData(string postDataKey, NameValueCollection values)
		{
			return false;
		}
		protected override void CreateChildControls()
		{
			this.inputFile = new HtmlInputFile();
			this.inputFile.ID = "command_UploadFile";
			base.CreateChildControls();
		}
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
		}
		protected override void OnLoad(EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				string text = this.request.QueryString["rif"] ?? "";
				if (text != "")
				{
					this.RootImagesFolder = text;
				}
				string text2 = this.request.QueryString["cif"] ?? "";
				if (text2 != "")
				{
					this.CurrentImagesFolder = text2;
				}
				string text3 = this.request.QueryString["ftb"] ?? "";
				if (text3 != "")
				{
					this.TargetFreeTextBox = text3;
				}
			}
			base.OnLoad(e);
		}
		protected override void OnPreRender(EventArgs e)
		{
			if (!this.Page.IsClientScriptBlockRegistered("FTB-Utility"))
			{
				this.Page.RegisterClientScriptBlock("FTB-Utility", "<script type=\"text/javascript\" src=\"" + this.CreateResourceString("FTB-Utility.js", ResourceType.JavaScript) + "\"></script>");
			}
			if (!this.Page.IsClientScriptBlockRegistered("FTB-ImageGallery"))
			{
				this.Page.RegisterClientScriptBlock("FTB-ImageGallery", "<script type=\"text/javascript\" src=\"" + this.CreateResourceString("FTB-ImageGallery.js", ResourceType.JavaScript) + "\"></script>");
			}
			this.Page.RegisterStartupScript("FTB-StartUp-Resize", "<script type=\"text/javascript\">\r\n\t\t\t\t\tFTB_AddEvents(window,new Array(\"load\",\"resize\"),FTB_ResizeGalleryArea);\r\n\t\t\t\t</script>");
			this.Page.RegisterRequiresPostBack(this);
			this.Page.GetPostBackEventReference(this);
		}
		protected override void Render(HtmlTextWriter writer)
		{
			this.RenderStyles(writer);
			writer.Write("\r\n<table height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n<tr><td colspan=\"2\" height=\"20\" id=\"GalleryTop\"><h3>Image Gallery</h3></td></tr>\r\n<tr>\r\n<td style=\"padding:0px;margin:0px;\">\r\n\t<div id=\"Gallery\" style=\"width:100%; height:" + (this.browserInfo.IsIE ? "100%" : "400px") + "; overflow: auto;margin:0px; background-color: #fff;border: 1px solid #CCC;\">");
			this.RenderImages(writer);
			writer.Write(string.Concat(new string[]
			{
				"\r\n\t</div>\r\n</td>\r\n<td width=\"220\" valign=\"top\" style=\"padding:5px;\">\r\n\t<div id=\"GallerySideBar\">\r\n\t<fieldset style=\"width:220px;\"><legend>Preview</legend>\r\n\t\t<div style=\"width:210px;height:150px;overflow:auto;\">\r\n\t\t\t<img id=\"img_preview\" src=\"",
				this.CreateResourceString("spacer", ResourceType.Utility),
				"\" />\t\t\t\t\r\n\t\t</div>\r\n\t</fieldset>\r\n\t<!--\r\n\t<fieldset style=\"width:220px;\"><legend>Info</legend>\r\n\t\t<table>\t\r\n\t\t\t<tr><td class=\"f_title\">Url</td>\r\n\t\t\t\t<td><div id=\"img_url\" style=\"width:150px;overflow:hidden;\"  /></td>\r\n\t\t\t\t</tr>\t\r\n\t\t\t<tr><td class=\"f_title\">Filesize</td>\r\n\t\t\t\t<td><div id=\"img_size\" style=\"width:150px;\"  /></td>\r\n\t\t\t\t</tr>\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t</table>\r\n\t</fieldset>\r\n\t-->\t\r\n\r\n\t<fieldset style=\"width:220px;\"><legend>Dimensions</legend>\r\n\t\t<table>\r\n\t\t\t<tr>\r\n\t\t\t\t<td valign=\"top\"><input type=\"radio\" id=\"img_dim_original\" name=\"img_dim\" onclick=\"FTB_DimensionChange(this);\" checked=\"true\" /><label for=\"img_dim_original\">Original Size</a></td>\r\n\t\t\t\t<td><input type=\"text\" id=\"img_width\" style=\"width:45px;\" onchange=\"FTB_UpdatePreview(this);\" disabled=\"true\" />x<input type=\"text\" id=\"img_height\" style=\"width:45px;\" onchange=\"FTB_UpdatePreview(this);\" disabled=\"true\" />\r\n\t\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t\t<tr>\r\n\t\t\t\t<td valign=\"top\"><input type=\"radio\" id=\"img_dim_custom\" name=\"img_dim\" onclick=\"FTB_DimensionChange(this);\" /><label for=\"img_dim_custom\">Custom Size</a></td>\r\n\t\t\t\t<td><input type=\"text\" id=\"img_width_custom\" style=\"width:45px;\" onkeyup=\"FTB_UpdatePreview(this);\" disabled=\"true\" />x<input type=\"text\" id=\"img_height_custom\" style=\"width:45px;\" onkeyup=\"FTB_UpdatePreview(this);\" disabled=\"true\" />\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t<input type=\"checkbox\" id=\"img_lockRatio\" checked=\"checked\"  /> <label for=\"img_lockRatio\">Lock image ratio</a>\r\n\t\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t\t<tr>\r\n\t\t\t\t<td><input type=\"radio\" id=\"img_dim_percentage\" name=\"img_dim\" onclick=\"FTB_DimensionChange(this);\" /><label for=\"img_dim_percentage\">Percentage</label></td>\r\n\t\t\t\t<td><input type=\"text\" id=\"img_percentage\" style=\"width:45px;\" onkeyup=\"FTB_UpdatePreview(this);\" disabled=\"true\" />\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t</table>\r\n\t</fieldset>\r\n\r\n\t<fieldset style=\"width:220px;\"><legend>Properties</legend>\r\n\t\t<table>\r\n\t\t\t<tr>\r\n\t\t\t<tr><td class=\"f_title\">Align</td>\r\n\t\t\t\t<td><select id=\"img_align\" >\r\n\t\t\t\t\t\t<option value=''>NotSet</option>\r\n\t\t\t\t\t\t<option value='top'>Top</option>\r\n\t\t\t\t\t\t<option value='bottom'>Bottom</option>\r\n\t\t\t\t\t\t<option value='left'>Left</option>\r\n\t\t\t\t\t\t<option value='right'>Right</option>\r\n\t\t\t\t\t\t<option value='center'>Center</option>\r\n\t\t\t\t\t\t<option value='absmiddle'>AbsMiddle</option>\r\n\t\t\t\t\t</select>\r\n\t\t\t\t\tBorder \t<input type=\"text\" id=\"img_border\" style=\"width:30px;\" />\t\t\t\t\t\r\n\t\t\t\t\t</td>\r\n\t\t\t\t</tr>\t\r\n\t\t\t<tr><td class=\"f_title\">VSpace</td>\r\n\t\t\t\t<td><input type=\"text\" id=\"img_vspace\" style=\"width:30px;\"/> HSpace<input type=\"text\" id=\"img_hspace\" style=\"width:30px;\"/></td>\r\n\t\t\t\t</tr>\r\n\t\t\t<tr>\r\n\t\t\t<tr><td class=\"f_title\">Alt</td>\r\n\t\t\t\t<td><input type=\"text\" id=\"img_alt\" style=\"width:150px;\"/></td>\r\n\t\t\t\t</tr>\r\n\t\t\t<tr><td class=\"f_title\">Title</td>\r\n\t\t\t\t<td><input type=\"text\" id=\"img_title\" style=\"width:150px;\"/></td>\r\n\t\t\t\t</tr>\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t</table>\r\n\t</fieldset>\r\n\t<div style=\"text-align:center;padding-bottom: 20px;\">\r\n\t\t<input type=\"button\" class=\"button\" value=\"Insert\" onclick=\"FTB_InsertImage();\" />\r\n\t</div>\r\n\t</div>\r\n</td>\r\n</tr>\r\n\t\t<tr id=\"GalleryBottom\"><td colspan=\"2\" style=\"height:60px;padding-left:10px;\">\r\n\t\t\t<table>\r\n\t\t\t<tr><td colspan=\"2\"><div id=\"img_feedback_title\">",
				(this.returnMessage != string.Empty) ? "Message" : "Status",
				"</div></td>\r\n\t\t\t\t<td><input type=\"text\" id=\"img_feedback_message\"",
				(this.returnMessage != string.Empty) ? ("value=\"" + this.returnMessage + "\"") : "",
				" disabled=\"disabled\" style=\"width:300px;\" /></td>\t\t\t\r\n\t\t\t\t<td>\t\r\n\t\t\t\t\t<input type=\"button\" id=\"command_DeleteFolderButton\" class=\"button\" value=\"Delete Folder\" onclick=\"FTB_DeleteFolder('",
				this.ClientID,
				"');\" style=\"display:none;\" />\r\n\t\t\t\t\t<input type=\"button\" id=\"command_DeleteImageButton\" class=\"button\" value=\"Delete Image\" onclick=\"FTB_DeleteImage('",
				this.ClientID,
				"');\" style=\"display:none;\" />\r\n\t\t\t\t</td>\r\n\t\t\t</tr>\r\n\r\n\t\t\t<tr><td>Upload File</td>\r\n\t\t\t<td><img src=\"",
				this.CreateResourceString("image", ResourceType.Utility),
				"\" width=\"16\" height=\"16\" /></td>\r\n\t\t\t<td>\t\t\t\r\n"
			}));
			this.inputFile.Attributes["class"] = "button";
			this.inputFile.Size = 42;
			this.inputFile.Attributes["style"] = "width: 300px;";
			this.inputFile.RenderControl(writer);
			writer.Write(string.Concat(new string[]
			{
				"\r\n\t\t\t</td><td>\r\n\t\t\t\t<input type=\"button\" id=\"command_UploadButton\" class=\"button\" value=\"Upload\" onclick=\"FTB_UploadFile('",
				this.ClientID,
				"');\" />\r\n\t\t\t</td><td>\r\n\t\t\t\t&nbsp;\r\n\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t\t<tr><td>Create Folder</td>\r\n\t\t\t<td><img src=\"",
				this.CreateResourceString("folder.small", ResourceType.Utility),
				"\" width=\"16\" height=\"16\" /></td>\r\n\t\t\t<td>\r\n\t\t\t\t<input type=\"text\" id=\"command_NewFolderName\" style=\"width:300px;\" />\r\n\t\t\t</td><td>\r\n\t\t\t\t<input type=\"button\" id=\"command_NewFolderButton\" class=\"button\" value=\"Create Folder\" onclick=\"FTB_CreateFolder('",
				this.ClientID,
				"');\" />\r\n\t\t\t</td><td>\r\n\t\t\t\t&nbsp;\t\t\t\t\r\n\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t\t</table>\t\t\r\n\t\r\n\t\t</td></tr>"
			}));
			writer.Write("\r\n</table>");
			writer.Write("<input type=\"hidden\" id=\"TargetFreeTextBox\" value=\"" + this.TargetFreeTextBox + "\" />");
			base.Render(writer);
		}
		protected virtual void RenderImages(HtmlTextWriter writer)
		{
			ArrayList images = this.GetImages();
			string[] array = this.ReturnDirectoriesArray();
			string physicalApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
			if (this.request.ApplicationPath == "/")
			{
				string arg_40_0 = this.request.ApplicationPath;
			}
			else
			{
				this.request.ApplicationPath + "/";
			}
			if (this.CurrentImagesFolder != this.RootImagesFolder)
			{
				string text = "";
				if (this.CurrentImagesFolder.IndexOf("/") > -1)
				{
					text = this.CurrentImagesFolder.Substring(0, this.CurrentImagesFolder.LastIndexOf("/"));
				}
				writer.Write("\n<div class=\"thumbnail\">\n");
				writer.Write(string.Concat(new string[]
				{
					"\t<div class=\"imageholder\" unselectable=\"on\" onclick=\"FTB_FolderClick(this);\" ondblclick=\"FTB_GoToFolder('",
					this.ClientID,
					"','",
					this.RootImagesFolder,
					"','",
					text,
					"');\">"
				}));
				writer.Write("\t\t" + string.Format("<img src=\"{0}\" title=\"{1}\" unselectable=\"on\" align=\"absmiddle\" vspace=\"26\" />", this.CreateResourceString("folder.up", ResourceType.Utility), text, "Navigate Up"));
				writer.Write("\t</div>\n");
				writer.Write("<div class=\"titleHolder\">Up</div>");
				writer.Write("</div>");
			}
			if (array != null && array.Length != 0)
			{
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					string text3 = text2.Replace(physicalApplicationPath, "").Replace("\\", "/");
					string text4 = text3.Substring(text3.LastIndexOf("/") + 1);
					string text5 = this.CurrentImagesFolder + "/" + text4;
					writer.Write("\n<div class=\"thumbnail\">\n");
					writer.Write(string.Concat(new string[]
					{
						"\t<div class=\"imageholder\" unselectable=\"on\" onclick=\"FTB_FolderClick(this,'",
						text4,
						"');\" ondblclick=\"FTB_GoToFolder('",
						this.ClientID,
						"','",
						this.RootImagesFolder,
						"','",
						text5,
						"');\">"
					}));
					writer.Write("\t\t" + string.Format("<img src=\"{0}\" title=\"{1}\" unselectable=\"on\" align=\"absmiddle\" vspace=\"19\" />", this.CreateResourceString("folder.big", ResourceType.Utility), text5));
					writer.Write("\t</div>\n");
					writer.Write("<div class=\"titleHolder\">" + text4 + "</div>");
					writer.Write("</div>");
				}
			}
			if (images == null || images.Count == 0)
			{
				writer.Write("There are no images: " + this.context.Server.MapPath(this.CurrentImagesFolder).Replace(physicalApplicationPath, "").Replace("\\", "/"));
				return;
			}
			foreach (ImageInfo imageInfo in images)
			{
				writer.Write("\n<div class=\"thumbnail\">\n");
				writer.Write(string.Concat(new string[]
				{
					"\t<div class=\"imageholder\" unselectable=\"on\" onclick=\"FTB_PreviewImage(this,'",
					imageInfo.AbsoluteWebPath.Substring(0, imageInfo.AbsoluteWebPath.LastIndexOf("/")),
					"','",
					imageInfo.Filename,
					"',",
					imageInfo.Width.ToString(),
					",",
					imageInfo.Height.ToString(),
					",'",
					(imageInfo.Size / 1024L).ToString(),
					" KB');\" ondblclick=\"FTB_InsertImage();\">\n"
				}));
				writer.Write("\t\t" + string.Format("<img src=\"{0}\" title=\"{1}\" unselectable=\"on\" align=\"absmiddle\" width=\"{2}\" height=\"{3}\" vspace=\"{4}\" />", new object[]
				{
					imageInfo.ThumbnailAbsoluteWebPath,
					imageInfo.AbsoluteWebPath,
					imageInfo.ThumbnailWidth,
					imageInfo.ThumbnailHeight,
					imageInfo.ThumbnailVSpace
				}) + "\n");
				writer.Write("\t</div>\n");
				writer.Write("\t<div class=\"titleHolder\">" + imageInfo.Filename + "</div>\n");
				writer.Write("</div>\n");
			}
		}
		public virtual ArrayList GetImages()
		{
			if (this.CurrentImagesFolder == "")
			{
				return null;
			}
			string key = "FTB-Images-" + this.CurrentImagesFolder;
			if (this.context.Cache[key] == null || this._currentImages != null)
			{
				ArrayList arrayList = new ArrayList();
				string text = this.request.ApplicationPath;
				if (!text.EndsWith("/"))
				{
					text += "/";
				}
				text = base.ResolveUrl(this.CurrentImagesFolder) + "/";
				while (text.IndexOf("//") > -1)
				{
					text = text.Replace("//", "/");
				}
				DirectoryInfo directoryInfo = new DirectoryInfo(this.context.Server.MapPath(this.CurrentImagesFolder));
				FileInfo[] array = null;
				if (this._currentImages != null)
				{
					array = this._currentImages;
				}
				else
				{
					try
					{
						array = directoryInfo.GetFiles("*");
					}
					catch
					{
					}
				}
				if (array != null)
				{
					FileInfo[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						FileInfo fileInfo = array2[i];
						bool flag = false;
						string[] acceptedFileTypes = this.AcceptedFileTypes;
						for (int j = 0; j < acceptedFileTypes.Length; j++)
						{
							string text2 = acceptedFileTypes[j];
							if (fileInfo.Extension.ToLower() == "." + text2.ToLower())
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							ImageInfo imageInfo = new ImageInfo();
							imageInfo.Filename = fileInfo.Name;
							imageInfo.PhysicalPath = fileInfo.FullName;
							imageInfo.AbsoluteWebPath = (text + fileInfo.Name).Replace("\\", "/");
							imageInfo.Size = fileInfo.Length;
							try
							{
								Image image = Image.FromFile(imageInfo.PhysicalPath);
								imageInfo.Width = image.Width;
								imageInfo.Height = image.Height;
								image.Dispose();
							}
							catch (Exception)
							{
								goto IL_381;
							}
							imageInfo.ThumbnailAbsoluteWebPath = imageInfo.AbsoluteWebPath;
							if (imageInfo.Width > imageInfo.Height)
							{
								if (imageInfo.Width > this.MaxThumbnailWidth)
								{
									imageInfo.ThumbnailWidth = this.MaxThumbnailWidth;
									imageInfo.ThumbnailHeight = Convert.ToInt32(imageInfo.Height * this.MaxThumbnailWidth / imageInfo.Width);
								}
								else
								{
									imageInfo.ThumbnailWidth = imageInfo.Width;
									imageInfo.ThumbnailHeight = imageInfo.Height;
								}
							}
							else
							{
								if (imageInfo.Height > this.MaxThumbnailHeight)
								{
									imageInfo.ThumbnailHeight = this.MaxThumbnailHeight;
									imageInfo.ThumbnailWidth = Convert.ToInt32(imageInfo.Width * this.MaxThumbnailHeight / imageInfo.Height);
								}
								else
								{
									imageInfo.ThumbnailWidth = imageInfo.Width;
									imageInfo.ThumbnailHeight = imageInfo.Height;
								}
							}
							if (imageInfo.ThumbnailHeight < this.MaxThumbnailHeight)
							{
								imageInfo.ThumbnailVSpace = Convert.ToInt32(this.MaxThumbnailHeight / 2 - imageInfo.ThumbnailHeight / 2);
							}
							if (imageInfo.Size > 1048576L)
							{
								imageInfo.SizeString = string.Format("{0:###.##} MB", imageInfo.Size / 1024L / 1024L);
							}
							else
							{
								if (imageInfo.Size > 1024L)
								{
									imageInfo.SizeString = string.Format("{0:###.##} KB", imageInfo.Size / 1024L);
								}
								else
								{
									imageInfo.SizeString = string.Format("{0} bytes", imageInfo.Size);
								}
							}
							arrayList.Add(imageInfo);
						}
						IL_381:;
					}
				}
				this.context.Cache.Insert(key, arrayList, new CacheDependency(this.context.Server.MapPath(this.CurrentImagesFolder)), DateTime.MaxValue, TimeSpan.Zero);
			}
			return (ArrayList)this.context.Cache[key];
		}
		protected virtual string[] ReturnDirectoriesArray()
		{
			if (this._currentDirectories != null)
			{
				return this._currentDirectories;
			}
			if (this.CurrentImagesFolder != "")
			{
				try
				{
					string[] directories = Directory.GetDirectories(this.context.Server.MapPath(this.CurrentImagesFolder), "*");
					string[] result = directories;
					return result;
				}
				catch
				{
					string[] result = null;
					return result;
				}
			}
			return null;
		}
		protected virtual void RenderStyles(HtmlTextWriter writer)
		{
			writer.Write("<style type=\"text/css\">\r\n\r\nbody {\r\n\tmargin: 0px 0px 0px 0px;\r\n\tpadding: 0px 0px 0px 0px;\r\n\twidth: 100%;\r\n\toverflow:hidden;\r\n\tborder: 0;\r\n\tbackground-color: #ECE9D8;\r\n\tcolor: #000000;\r\n}\r\nform { \r\n\tmargin: 0px;\r\n\tpadding: 0px;\r\n}\r\ndiv.thumbnail {\r\n\twidth: 120px;\r\n\theight: 100px;\r\n\ttext-align: center;\t\t\t\r\n\tfloat: left;\r\n\tfont: 10pt verdana;\r\n\tmargin: 5px;\r\n\toverflow: hidden;\r\n}\r\ndiv.imageholder {\r\n\tmargin: 0px;\r\n\tpadding: 1px;\r\n\tborder: 1px solid #CCCCCC;\t\r\n\twidth: 101px;\r\n\theight: 81px;\r\n}\r\n\r\ndiv.titleholder {\r\n\tfont-family: arial;\r\n\tfont-size: 8pt;\r\n\twidth: 100px;\r\n\ttext-overflow: ellipsis;\r\n\toverflow: hidden;\r\n\twhite-space: nowrap;\t\t\t\r\n}\t\t\r\ntable {\r\n  font: 11px Tahoma,Verdana,sans-serif;\r\n}\r\nform p {\r\n  margin-top: 5px;\r\n  margin-bottom: 5px;\r\n}\r\nh3 { margin: 0; margin-top: 4px;  margin-bottom: 5px; font-size: 12px; border-bottom: 2px solid #90A8F0; color: #90A8F0;}\r\nfieldset { padding: 0px 10px 5px 5px; }\r\n.button { width: 75px; }\r\nselect, input, button { font: 11px Tahoma,Verdana,sans-serif; }\r\n.space { padding: 2px; }\r\n.title { background: #ddf; color: #000; font-weight: bold; font-size: 120%; padding: 3px 10px; margin-bottom: 10px;\r\n\tborder-bottom: 1px solid black; letter-spacing: 2px;\r\n}\r\n.f_title { text-align:right; }\\\r\n.footer { border-top:2px solid #90A8F0; padding-top: 3px; margin-top: 4px; text-align:right; }\r\n</style>");
		}
		private string CreateResourceString(string filename, ResourceType resourceType)
		{
			switch (resourceType)
			{
			case ResourceType.JavaScript:
				if (this.JavaScriptLocation == ResourceLocation.InternalResource)
				{
					return ClientScriptWrapper.GetWebResourceUrl(this, "FreeTextBoxControls.Resources.JavaScript." + filename, this.AssemblyResourceHandlerPath);
				}
				return base.ResolveUrl(this.SupportFolder + filename);
			case ResourceType.Utility:
				if (this.UtilityImagesLocation == ResourceLocation.InternalResource)
				{
					return ClientScriptWrapper.GetWebResourceUrl(this, "FreeTextBoxControls.Resources.Images.Utility." + filename + ".gif", this.AssemblyResourceHandlerPath);
				}
				return base.ResolveUrl(this.SupportFolder + "Utility/" + filename + ".gif");
			default:
				return string.Empty;
			}
		}
	}
}

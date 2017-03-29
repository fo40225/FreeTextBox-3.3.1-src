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
using FreeTextBoxControls.Support;

namespace FreeTextBoxControls {
	/// <summary>
	/// An Image Gallery and Upload Utility for <see cref="FreeTextBox"/>
	/// </summary>
	/// <example>
	/// <code><![CDATA[
	/// -- ftb.imagegallery.aspx ----------------------------
	/// <%@ Register TagPrefix="FTB" Assembly="FreeTextBox" Namespace="FreeTextBoxControls" %>
	/// <html>
	///	 <head>
	///	 <style type="text/css">
	///	 body,form { margin: 0; }
	///	 </style>
	///	 </head>
	///	 <body>
	///		<form runat="server">
	///			<FTB:ImageGallery id="FreeTextBox1" runat="server" />
	///		</form>
	///	 </body>
	/// </html>
	/// </configuration>
	/// ]]></code>
	/// </example>
	[
		ToolboxData("<{0}:ImageGallery runat=\"server\"></{0}:ImageGallery>")
	]
	public class ImageGallery : Control, IPostBackDataHandler, IPostBackEventHandler {
		public ImageGallery() {
			context = HttpContext.Current;
			request = context.Request;
			response = context.Response;
			browserInfo = new BrowserInfo();
		}
		
		#region Private Properties
		private HttpRequest request = null;
		private BrowserInfo browserInfo = null;
		private HttpResponse response = null;
		private HttpContext context = null;
		protected int MaxThumbnailWidth = 100;
		protected int MaxThumbnailHeight = 80;
		protected string returnMessage = "";

		protected HtmlInputFile inputFile = null;
		#endregion

		#region Public Properties

		

		#region Temp gallery properties
		
		string[] _currentDirectories = null;
		/// <summary>
		/// Sets the current directories used (overrides the default read)
		/// </summary>
		[
			Category("Behavior"),
				Description("Sets the current folders used (overrides the default read).")
		]		
		public string[] CurrentDirectories {
			set {
				_currentDirectories = value;
			}
		}

		FileInfo[] _currentImages = null;
		/// <summary>
		/// Sets the current images used (overrides the default read)
		/// </summary>
		[
			Category("Behavior"),
				Description("Sets the current images used (overrides the default read).")
		]		
		public FileInfo[] CurrentImages {
			set {
				_currentImages = value;
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
                return (savedState == null) ? ResolveUrl("~/") : (string)savedState;
            }
            set
            {
                ViewState["AssemblyResourceHandlerPath"] = value;
            }
        }		
        
        /// <summary>
		/// Gets or sets the FreeTextBox to insert images into
		/// </summary>
		[
			Category("Behavior"),
				Description("Gets or sets the FreeTextBox to insert images into.")
		]		
		public string TargetFreeTextBox {
			get { 
				object savedState = this.ViewState["TargetFreeTextBox"];
				return (savedState == null) ? "" : (string) savedState;
			}
			set {
				ViewState["TargetFreeTextBox"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the root folder where images are stored
		/// </summary>
		[
			Category("Behavior"),
				Description("Gets or sets the root folder where images are stored.")
		]		
		public string RootImagesFolder {
			get { 
				object savedState = this.ViewState["RootImagesFolder"];
				if (savedState == null) { 
					return "~/images";
				} else {
					string folder = (string) savedState;	
					
					// switch to web folder foward slashes
					folder = folder.Replace(@"\","/");
					// removed double slashes
					while (folder.IndexOf("//") > -1) folder = folder.Replace("//","/");
					// remove trailing slash
					while (folder.EndsWith("/")) folder = folder.Substring(0,folder.Length-1);

					return folder;
				}
			}
			set {
				ViewState["RootImagesFolder"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the current folder the user is browsing
		/// </summary>
		[
			Category("Behavior"),
				Description("Gets or sets the current folder the user is browsing.")
		]		
		public string CurrentImagesFolder {
			get { 
				object savedState = this.ViewState["CurrentImagesFolder"];
				if (savedState == null) { 
					return "~/images";
				} else {
					string folder = (string) savedState;	
					
					// switch to web folder foward slashes
					folder = folder.Replace(@"\","/");
					// removed double slashes
					while (folder.IndexOf("//") > -1) folder = folder.Replace("//","/");
					// remove trailing slash
					while (folder.EndsWith("/")) folder = folder.Substring(0,folder.Length-1);
					
					return folder;
				}
			}
			set {
				ViewState["CurrentImagesFolder"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the imge file extensions allowed
		/// </summary>
		[
			Category("Behavior"),
				Description("Gets or sets the imge file extensions allowed.")
		]		
		public string[] AcceptedFileTypes {
			get { 
				object savedState = this.ViewState["AcceptedFileTypes"];
				return (savedState == null) ? new string[] {"jpg","jpeg","jpe","gif","bmp","png"} : (string[]) savedState;
			}
			set {
				ViewState["AcceptedFileTypes"] = value;
			}
		}
		#endregion

		#region Thumbnails
		/// <summary>
		/// Gets or sets the maximum height of thumbnails
		/// </summary>
		[
			Category("Thumbnails"),
				Description("Gets or sets the maximum height of thumbnails.")
		]		
		public int ThumbnailHeight {
			get { 
				object savedState = this.ViewState["ThumbnailHeight"];
				return (savedState == null) ? 94 : (int) savedState;
			}
			set {
				ViewState["ThumbnailHeight"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the maximum height of thumbnails
		/// </summary>
		[
			Category("Thumbnails"),
				Description("Gets or sets the maximum width of thumbnails.")
		]		
		public int ThumbnailWidth {
			get { 
				object savedState = this.ViewState["ThumbnailWidth"];
				return (savedState == null) ? 94 : (int) savedState;
			}
			set {
				ViewState["ThumbnailWidth"] = value;
			}
		}
		#endregion

		#region Permissions
		/// <summary>
		/// Gets or sets if uploading files is allowed
		/// </summary>
		[
			Category("Behavior"),
				Description("Gets or sets if uploading files is allowed.")
		]		
		public bool AllowImageUpload {
			get { 
				object savedState = this.ViewState["AllowImageUpload"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["AllowImageUpload"] = value;
			}
		}
		/// <summary>
		/// Gets or sets if file deleting is allowed.
		/// </summary>
		[
			Category("Behavior"),
				Description("Gets or sets if file deleting is allowed.")
		]		
		public bool AllowImageDelete {
			get { 
				object savedState = this.ViewState["AllowImageDelete"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["AllowImageDelete"] = value;
			}
		}
		/// <summary>
		/// Gets or sets if directory deleting is allowed.
		/// </summary>
		[
			Category("Behavior"),
				Description("Gets or sets if directory deleting is allowed.")
		]		
		public bool AllowDirectoryDelete {
			get { 
				object savedState = this.ViewState["AllowDirectoryDelete"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["AllowDirectoryDelete"] = value;
			}
		}
		/// <summary>
		/// Gets or sets if directory creation is allowed.
		/// </summary>
		[
			Category("Behavior"),
				Description("Gets or sets if directory creation is allowed.")
		]		
		public bool AllowDirectoryCreate {
			get { 
				object savedState = this.ViewState["AllowDirectoryCreate"];
				return (savedState == null) ? true : (bool) savedState;
			}
			set {
				ViewState["AllowDirectoryCreate"] = value;
			}
		}
		#endregion
		
		#region Resources
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
		public ResourceLocation UtilityImagesLocation {
			get { 
				object savedState = this.ViewState["UtilityImagesLocation"];
				return (savedState == null) ? ResourceLocation.InternalResource : (ResourceLocation) savedState;
			}
			set {
				ViewState["UtilityImagesLocation"] = value;
			}
		}
		/// <summary>
		/// Gets or sets the FreeTextBox to insert images into
		/// </summary>
		[
			Category("Behavior"),
				Description("Gets or sets where the helper ")
		]		
		public string SupportFolder {
			get { 
				object savedState = this.ViewState["SupportFolder"];
				return (savedState == null) ? "/aspnet_client/FreeTextBox/" : (string) savedState;
			}
			set {
				ViewState["SupportFolder"] = value;
			}
		}

		#endregion

		#endregion
		
		#region IPostBack Implimentation and Events

		public virtual void RaisePostBackEvent(string eventArgument) { 
			EnsureChildControls();
			
			DirectoryInfo directoryInfo = new DirectoryInfo(context.Server.MapPath(CurrentImagesFolder));

			string[] args = eventArgument.Split(':');

			switch (args[0]) {
				// check for folder change
				case "GoToFolder":
					if (args.Length != 2) return;
					string gotoFolder = args[1];
					CurrentImagesFolder = gotoFolder;
					returnMessage = "Navigated to " + gotoFolder + ".";
					break;
			
				// check for new folder
				case "CreateFolder":
					if (args.Length != 2) return;
					if (!this.AllowDirectoryCreate) {
						returnMessage = "Your permissions do not allow you do create directories";
						break;
					}
					string newFolder = args[1];

					try {
						directoryInfo.CreateSubdirectory(newFolder);
						returnMessage = "Folder created";
					} catch (Exception e) {
						returnMessage = "Error creating folder: " + directoryInfo.FullName + "\\" + newFolder + ": " + e.ToString();
					}
					
					break;

				// delete folder
				case "DeleteFolder":
					if (args.Length != 2) return;
					if (!this.AllowDirectoryDelete) {
						returnMessage = "Your permissions do not allow you do delete directories";
						break;
					}
					string folderToDelete = args[1];

					try {
						Directory.Delete(directoryInfo.FullName + "\\" + folderToDelete,true);
						returnMessage = "Directory deleted.";
					} catch (Exception e) {
						returnMessage = "Failed to delete: " + directoryInfo.FullName + "\\" + folderToDelete + ": " + e.ToString();
					}
					break;

					// delete folder
				case "DeleteImage":
					if (args.Length != 2) return;
					if (!this.AllowImageDelete) {
						returnMessage = "Your permissions do not allow you do delete images";
						break;
					}
					string fileToDelete = args[1];

					try {
						File.Delete(directoryInfo.FullName + "\\" + fileToDelete);
						returnMessage = "Image deleted.";
					} catch (Exception e) {
						returnMessage = "failed to delete: " + directoryInfo.FullName + "\\" + fileToDelete + ": " + e.ToString();
					}
					break;
			
				// check for uploaded image
				case "UploadImage": 
					if (!this.AllowImageUpload) {
						returnMessage = "Your permissions do not allow you do upload images";
						break;
					}
					if (inputFile == null) {
						returnMessage = "Error: InputFile control not yet created!";
					} else {
						if (inputFile.PostedFile != null && inputFile.PostedFile.FileName != null) {

							string filename = inputFile.PostedFile.FileName.Substring(inputFile.PostedFile.FileName.LastIndexOf(@"\")+1);

							this.inputFile.PostedFile.SaveAs(context.Server.MapPath(CurrentImagesFolder) + @"\" + filename);	

							returnMessage = "Image uploaded";

                            // kill the cache so it get reloaded
                            context.Cache.Remove("FTB-Images-" + CurrentImagesFolder);
						} else {
							throw new Exception("No file was uploaded!!");
							returnMessage = "Error: No file was uploaded. Ensure <form enctype='multipart/form-data'>";
						}
					}

					break;
				default:
					break;
			}
		}
		public void RaisePostDataChangedEvent() {
			// nothing happens becuase nothing changes
		}
		public bool LoadPostData(String postDataKey, NameValueCollection values) {			
			// load possible uploaded image?

			

			// nothing is changing...
			return false;

		}
		#endregion

		protected override void CreateChildControls() {
			inputFile = new HtmlInputFile();
			inputFile.ID = "command_UploadFile";
			
			base.CreateChildControls ();
		}

		protected override void OnInit(EventArgs e) {
			
			base.OnInit (e);
		}
		protected override void OnLoad(EventArgs e) {
			// load the query string variables
			
			if (!Page.IsPostBack) {
				string rootImagesFolder = request.QueryString["rif"] + "";
				if (rootImagesFolder != "") {
					this.RootImagesFolder = rootImagesFolder;
				}
				string currentImagesFolder = request.QueryString["cif"] + "";
				if (currentImagesFolder != "") {
					this.CurrentImagesFolder = currentImagesFolder;
				}
				string targetFreeTextBox = request.QueryString["ftb"] + "";

				if (targetFreeTextBox != "") {
					this.TargetFreeTextBox = targetFreeTextBox;
				}
			}			
			base.OnLoad (e);
		}

		protected override void OnPreRender( EventArgs e ) {
			// Main Script block
			if (!this.Page.IsClientScriptBlockRegistered("FTB-Utility")) {
				this.Page.RegisterClientScriptBlock("FTB-Utility",
					@"<script type=""text/javascript"" src=""" + CreateResourceString("FTB-Utility.js", ResourceType.JavaScript) + @"""></script>");
			}

			// Gallery Script
			if (!this.Page.IsClientScriptBlockRegistered("FTB-ImageGallery")) {
				this.Page.RegisterClientScriptBlock("FTB-ImageGallery",
					@"<script type=""text/javascript"" src=""" + CreateResourceString("FTB-ImageGallery.js", ResourceType.JavaScript) + @"""></script>");
			}
			
			// resizing script
			Page.RegisterStartupScript("FTB-StartUp-Resize",
				@"<script type=""text/javascript"">
					FTB_AddEvents(window,new Array(""load"",""resize""),FTB_ResizeGalleryArea);
				</script>");

			// postback
			Page.RegisterRequiresPostBack(this);
			Page.GetPostBackEventReference(this);
		}


		protected override void Render(HtmlTextWriter writer) {
			RenderStyles(writer);

			writer.Write(@"
<table height=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
<tr><td colspan=""2"" height=""20"" id=""GalleryTop""><h3>Image Gallery</h3></td></tr>
<tr>
<td style=""padding:0px;margin:0px;"">
	<div id=""Gallery"" style=""width:100%; height:" + ((browserInfo.IsIE) ? "100%" : "400px") + @"; overflow: auto;margin:0px; background-color: #fff;border: 1px solid #CCC;"">");

			RenderImages(writer);

			writer.Write(@"
	</div>
</td>
<td width=""220"" valign=""top"" style=""padding:5px;"">
	<div id=""GallerySideBar"">
	<fieldset style=""width:220px;""><legend>Preview</legend>
		<div style=""width:210px;height:150px;overflow:auto;"">
			<img id=""img_preview"" src=""" + CreateResourceString("spacer",ResourceType.Utility) + @""" />				
		</div>
	</fieldset>
	<!--
	<fieldset style=""width:220px;""><legend>Info</legend>
		<table>	
			<tr><td class=""f_title"">Url</td>
				<td><div id=""img_url"" style=""width:150px;overflow:hidden;""  /></td>
				</tr>	
			<tr><td class=""f_title"">Filesize</td>
				<td><div id=""img_size"" style=""width:150px;""  /></td>
				</tr>																														
		</table>
	</fieldset>
	-->	

	<fieldset style=""width:220px;""><legend>Dimensions</legend>
		<table>
			<tr>
				<td valign=""top""><input type=""radio"" id=""img_dim_original"" name=""img_dim"" onclick=""FTB_DimensionChange(this);"" checked=""true"" /><label for=""img_dim_original"">Original Size</a></td>
				<td><input type=""text"" id=""img_width"" style=""width:45px;"" onchange=""FTB_UpdatePreview(this);"" disabled=""true"" />x<input type=""text"" id=""img_height"" style=""width:45px;"" onchange=""FTB_UpdatePreview(this);"" disabled=""true"" />
				</td>
			</tr>
			<tr>
				<td valign=""top""><input type=""radio"" id=""img_dim_custom"" name=""img_dim"" onclick=""FTB_DimensionChange(this);"" /><label for=""img_dim_custom"">Custom Size</a></td>
				<td><input type=""text"" id=""img_width_custom"" style=""width:45px;"" onkeyup=""FTB_UpdatePreview(this);"" disabled=""true"" />x<input type=""text"" id=""img_height_custom"" style=""width:45px;"" onkeyup=""FTB_UpdatePreview(this);"" disabled=""true"" />
					<br />
					<input type=""checkbox"" id=""img_lockRatio"" checked=""checked""  /> <label for=""img_lockRatio"">Lock image ratio</a>
				</td>
			</tr>
			<tr>
				<td><input type=""radio"" id=""img_dim_percentage"" name=""img_dim"" onclick=""FTB_DimensionChange(this);"" /><label for=""img_dim_percentage"">Percentage</label></td>
				<td><input type=""text"" id=""img_percentage"" style=""width:45px;"" onkeyup=""FTB_UpdatePreview(this);"" disabled=""true"" />									
				</td>
			</tr>
		</table>
	</fieldset>

	<fieldset style=""width:220px;""><legend>Properties</legend>
		<table>
			<tr>
			<tr><td class=""f_title"">Align</td>
				<td><select id=""img_align"" >
						<option value=''>NotSet</option>
						<option value='top'>Top</option>
						<option value='bottom'>Bottom</option>
						<option value='left'>Left</option>
						<option value='right'>Right</option>
						<option value='center'>Center</option>
						<option value='absmiddle'>AbsMiddle</option>
					</select>
					Border 	<input type=""text"" id=""img_border"" style=""width:30px;"" />					
					</td>
				</tr>	
			<tr><td class=""f_title"">VSpace</td>
				<td><input type=""text"" id=""img_vspace"" style=""width:30px;""/> HSpace<input type=""text"" id=""img_hspace"" style=""width:30px;""/></td>
				</tr>
			<tr>
			<tr><td class=""f_title"">Alt</td>
				<td><input type=""text"" id=""img_alt"" style=""width:150px;""/></td>
				</tr>
			<tr><td class=""f_title"">Title</td>
				<td><input type=""text"" id=""img_title"" style=""width:150px;""/></td>
				</tr>																						
		</table>
	</fieldset>
	<div style=""text-align:center;padding-bottom: 20px;"">
		<input type=""button"" class=""button"" value=""Insert"" onclick=""FTB_InsertImage();"" />
	</div>
	</div>
</td>
</tr>
		<tr id=""GalleryBottom""><td colspan=""2"" style=""height:60px;padding-left:10px;"">
			<table>
			<tr><td colspan=""2""><div id=""img_feedback_title"">" + ((returnMessage != string.Empty) ? "Message" : "Status" ) + @"</div></td>
				<td><input type=""text"" id=""img_feedback_message""" + ((returnMessage != string.Empty) ? @"value=""" + returnMessage + @"""" : "" ) + @" disabled=""disabled"" style=""width:300px;"" /></td>			
				<td>	
					<input type=""button"" id=""command_DeleteFolderButton"" class=""button"" value=""Delete Folder"" onclick=""FTB_DeleteFolder('" + this.ClientID + @"');"" style=""display:none;"" />
					<input type=""button"" id=""command_DeleteImageButton"" class=""button"" value=""Delete Image"" onclick=""FTB_DeleteImage('" + this.ClientID + @"');"" style=""display:none;"" />
				</td>
			</tr>

			<tr><td>Upload File</td>
			<td><img src=""" + CreateResourceString("image",ResourceType.Utility) + @""" width=""16"" height=""16"" /></td>
			<td>			
");
				inputFile.Attributes["class"] = "button";
				inputFile.Size = 42;
				inputFile.Attributes["style"] = "width: 300px;";
				inputFile.RenderControl(writer);

				writer.Write(@"
			</td><td>
				<input type=""button"" id=""command_UploadButton"" class=""button"" value=""Upload"" onclick=""FTB_UploadFile('" + this.ClientID + @"');"" />
			</td><td>
				&nbsp;
			</td>
			</tr>
			<tr><td>Create Folder</td>
			<td><img src=""" + CreateResourceString("folder.small",ResourceType.Utility) + @""" width=""16"" height=""16"" /></td>
			<td>
				<input type=""text"" id=""command_NewFolderName"" style=""width:300px;"" />
			</td><td>
				<input type=""button"" id=""command_NewFolderButton"" class=""button"" value=""Create Folder"" onclick=""FTB_CreateFolder('" + this.ClientID + @"');"" />
			</td><td>
				&nbsp;				
			</td>
			</tr>
			</table>		
	
		</td></tr>");

			writer.Write(@"
</table>");			

			//writer.Write(@"</div>");
			writer.Write(@"<input type=""hidden"" id=""TargetFreeTextBox"" value=""" + this.TargetFreeTextBox + @""" />");

			base.Render (writer);
		}

		
		protected virtual void RenderImages(HtmlTextWriter writer) {

			ArrayList imageList = GetImages();
			string[] directoryList = ReturnDirectoriesArray();
			string AppPath = HttpContext.Current.Request.PhysicalApplicationPath;
			string AppUrl;
	
			//Get the application's URL
			if (request.ApplicationPath == "/")
				AppUrl = request.ApplicationPath;
			else
				AppUrl = request.ApplicationPath + "/";
			
			// check if we are in a level deeper than the root level,
			// if so we need to render an "up" button
			if (CurrentImagesFolder != RootImagesFolder) {
										
				string parentFolder = "";
				if (CurrentImagesFolder.IndexOf("/") > -1) 
					parentFolder = CurrentImagesFolder.Substring(0,CurrentImagesFolder.LastIndexOf("/"));

				// outer div
				writer.Write("\n" + @"<div class=""thumbnail"">" + "\n");					
					
				// img
				writer.Write("\t" + @"<div class=""imageholder"" unselectable=""on"" onclick=""FTB_FolderClick(this);"" ondblclick=""FTB_GoToFolder('" + this.ClientID + @"','" + RootImagesFolder + "','" + parentFolder + @"');"">");
				writer.Write("\t\t" + String.Format(@"<img src=""{0}"" title=""{1}"" unselectable=""on"" align=""absmiddle"" vspace=""26"" />", CreateResourceString("folder.up",ResourceType.Utility), parentFolder, "Navigate Up"));
				writer.Write("\t" + "</div>" + "\n");

				writer.Write(@"<div class=""titleHolder"">Up</div>");

				// close 
				writer.Write("</div>");			
			}

			// render directory folders
			if ( directoryList != null && directoryList.Length != 0 ) {
				foreach (string directory in directoryList) {
				
					
					string fullWebDirectory = directory.Replace(AppPath,"").Replace(@"\","/");
					string directoryName = fullWebDirectory.Substring(fullWebDirectory.LastIndexOf("/")+1);
					string gotoDirectory = CurrentImagesFolder + "/" + directoryName;

					// outer div
					writer.Write("\n" + @"<div class=""thumbnail"">" + "\n");					
			
					// img
					writer.Write("\t" + @"<div class=""imageholder"" unselectable=""on"" onclick=""FTB_FolderClick(this,'" + directoryName + @"');"" ondblclick=""FTB_GoToFolder('" + this.ClientID + @"','" + RootImagesFolder + "','" + gotoDirectory + @"');"">");
					writer.Write("\t\t" + String.Format(@"<img src=""{0}"" title=""{1}"" unselectable=""on"" align=""absmiddle"" vspace=""19"" />",CreateResourceString("folder.big",ResourceType.Utility),gotoDirectory));
					writer.Write("\t" + "</div>" + "\n");

					writer.Write(@"<div class=""titleHolder"">" + directoryName + "</div>");

					// close 
					writer.Write("</div>");	
				}
			}

			// check for images
			if ( imageList == null || imageList.Count == 0 ) {
				writer.Write("There are no images: " + context.Server.MapPath(CurrentImagesFolder).Replace(AppPath,"").Replace(@"\","/"));
			} else {
					
				// render images
				foreach (ImageInfo imageInfo in imageList) {

					// outer div
					writer.Write("\n" + @"<div class=""thumbnail"">" + "\n");
					
					// img
					writer.Write("\t" + @"<div class=""imageholder"" unselectable=""on"" onclick=""FTB_PreviewImage(this,'" + imageInfo.AbsoluteWebPath.Substring(0,imageInfo.AbsoluteWebPath.LastIndexOf("/")) + @"','" + imageInfo.Filename + @"'," + imageInfo.Width.ToString() + "," + imageInfo.Height.ToString() + @",'" + (imageInfo.Size/1024).ToString() + @" KB');"" ondblclick=""FTB_InsertImage();"">" + "\n");
					writer.Write("\t\t" + String.Format(@"<img src=""{0}"" title=""{1}"" unselectable=""on"" align=""absmiddle"" width=""{2}"" height=""{3}"" vspace=""{4}"" />",imageInfo.ThumbnailAbsoluteWebPath,imageInfo.AbsoluteWebPath,imageInfo.ThumbnailWidth,imageInfo.ThumbnailHeight,imageInfo.ThumbnailVSpace) + "\n");
					writer.Write("\t" + "</div>" + "\n");

					//writer.Write("\t" + @"<div class=""titleHolder"">" + imageInfo.Filename + "<br />" + imageInfo.Height.ToString() + "x" + imageInfo.Height.ToString() + " (" + imageInfo.SizeString + ")" + "</div>" + "\n");
					writer.Write("\t" + @"<div class=""titleHolder"">" + imageInfo.Filename  + "</div>" + "\n");

					// close 
					writer.Write("</div>" + "\n");					
				}
			}
		}

		public virtual ArrayList GetImages() {
			if (CurrentImagesFolder == "") return null;
			
			string cacheKey = "FTB-Images-" + CurrentImagesFolder;
			
			if (context.Cache[cacheKey] == null || _currentImages != null) {

				ArrayList imagesList = new ArrayList();
				
				// setup web path
				
				string WebPath = request.ApplicationPath;
				if (!WebPath.EndsWith("/")) WebPath+= "/";
				
				WebPath = ResolveUrl(CurrentImagesFolder) + "/";
				while (WebPath.IndexOf("//") > -1) WebPath = WebPath.Replace("//","/");
				DirectoryInfo directoryInfo = new DirectoryInfo(context.Server.MapPath(CurrentImagesFolder));
				FileInfo[] files = null;

				// if possible use the programmatically defined images
				if (_currentImages != null) {
					files = _currentImages;
				} else {

					try {
						files = directoryInfo.GetFiles("*");
					} catch {}
				}
				
				if (files != null) {
					foreach(FileInfo fileInfo in files) {
					
						// check for valid types
						bool validImage = false;					
						foreach (string validExtension in AcceptedFileTypes) {
							if (fileInfo.Extension.ToLower() == "." + validExtension.ToLower()) {
								validImage = true;
								break;
							}
						}

						if (validImage) {
							ImageInfo imageInfo = new ImageInfo();
					
							// get file info
							imageInfo.Filename = fileInfo.Name;
							imageInfo.PhysicalPath = fileInfo.FullName;
							imageInfo.AbsoluteWebPath = (WebPath + fileInfo.Name).Replace(@"\","/");
							imageInfo.Size = fileInfo.Length;
					
							// get image info
							try {
								Image image = Image.FromFile(imageInfo.PhysicalPath);
								imageInfo.Width = image.Width;
								imageInfo.Height = image.Height;
								image.Dispose();
							} catch (Exception e) {
								// TODO: debug in some way that makes sense to the user
								continue;
								throw new Exception("Can't load: " + imageInfo.PhysicalPath + "; " + e.ToString());
							}

							// set thumbnail info
							imageInfo.ThumbnailAbsoluteWebPath = imageInfo.AbsoluteWebPath;
					
							// landscape image
							if (imageInfo.Width > imageInfo.Height) {
								if (imageInfo.Width > MaxThumbnailWidth) {
									imageInfo.ThumbnailWidth = MaxThumbnailWidth;
									imageInfo.ThumbnailHeight = Convert.ToInt32(imageInfo.Height * MaxThumbnailWidth/imageInfo.Width);						
								} else {
									imageInfo.ThumbnailWidth = imageInfo.Width;
									imageInfo.ThumbnailHeight = imageInfo.Height;
								}
								// portrait image
							} else {
								if (imageInfo.Height > MaxThumbnailHeight) {
									imageInfo.ThumbnailHeight = MaxThumbnailHeight;
									imageInfo.ThumbnailWidth = Convert.ToInt32(imageInfo.Width * MaxThumbnailHeight/imageInfo.Height);
								} else {
									imageInfo.ThumbnailWidth = imageInfo.Width;
									imageInfo.ThumbnailHeight = imageInfo.Height;
								}
							}

							if (imageInfo.ThumbnailHeight < MaxThumbnailHeight) {
								imageInfo.ThumbnailVSpace = Convert.ToInt32((MaxThumbnailHeight/2)-(imageInfo.ThumbnailHeight/2)); 
							}

							if (imageInfo.Size > (1024 * 1024)) {
								imageInfo.SizeString = String.Format("{0:###.##} MB",imageInfo.Size / 1024 / 1024);
							} else if (imageInfo.Size > 1024) {
								imageInfo.SizeString = String.Format("{0:###.##} KB",imageInfo.Size / 1024 );
							} else {
								imageInfo.SizeString = String.Format("{0} bytes",imageInfo.Size);
							}
					
							imagesList.Add(imageInfo);
						}
					}
				}
		
				context.Cache.Insert(cacheKey,imagesList,new CacheDependency(context.Server.MapPath(CurrentImagesFolder)), DateTime.MaxValue, TimeSpan.Zero);
			}

			return (ArrayList) context.Cache[cacheKey];

		}

		protected virtual  string[] ReturnDirectoriesArray() {
			if (this._currentDirectories != null) return _currentDirectories;
			
			if (CurrentImagesFolder != "") {
				try {
					// TODO: figure out how to get this to work with ResolveUrl
					string[] DirectoriesArray = Directory.GetDirectories(context.Server.MapPath(CurrentImagesFolder),"*");
					return DirectoriesArray ;
				} catch {
					return null;
				}
			} else {
				return null;
			}
		}


		protected virtual void RenderStyles(HtmlTextWriter writer) {
			
			writer.Write(@"<style type=""text/css"">

body {
	margin: 0px 0px 0px 0px;
	padding: 0px 0px 0px 0px;
	width: 100%;
	overflow:hidden;
	border: 0;
	background-color: #ECE9D8;
	color: #000000;
}
form { 
	margin: 0px;
	padding: 0px;
}
div.thumbnail {
	width: 120px;
	height: 100px;
	text-align: center;			
	float: left;
	font: 10pt verdana;
	margin: 5px;
	overflow: hidden;
}
div.imageholder {
	margin: 0px;
	padding: 1px;
	border: 1px solid #CCCCCC;	
	width: 101px;
	height: 81px;
}

div.titleholder {
	font-family: arial;
	font-size: 8pt;
	width: 100px;
	text-overflow: ellipsis;
	overflow: hidden;
	white-space: nowrap;			
}		
table {
  font: 11px Tahoma,Verdana,sans-serif;
}
form p {
  margin-top: 5px;
  margin-bottom: 5px;
}
h3 { margin: 0; margin-top: 4px;  margin-bottom: 5px; font-size: 12px; border-bottom: 2px solid #90A8F0; color: #90A8F0;}
fieldset { padding: 0px 10px 5px 5px; }
.button { width: 75px; }
select, input, button { font: 11px Tahoma,Verdana,sans-serif; }
.space { padding: 2px; }
.title { background: #ddf; color: #000; font-weight: bold; font-size: 120%; padding: 3px 10px; margin-bottom: 10px;
	border-bottom: 1px solid black; letter-spacing: 2px;
}
.f_title { text-align:right; }\
.footer { border-top:2px solid #90A8F0; padding-top: 3px; margin-top: 4px; text-align:right; }
</style>");
		}
		

		#region Helper Functions
		string CreateResourceString(string filename, ResourceType resourceType) {
			// this allows buttons and toolbar background images to be handled separately so 
			// a developer can still have a Green Office 2003 toolbar
			// and custom JS files
				
			switch (resourceType) {
				default:
					return string.Empty;
				case ResourceType.Utility:
					if (this.UtilityImagesLocation == ResourceLocation.InternalResource) {
						//return AssemblyResourceHandler.GetWebResourceUrl(typeof(FreeTextBox),"FreeTextBoxControls.Resources.Images.Utility." + filename + ".gif");
                        return ClientScriptWrapper.GetWebResourceUrl(this, "FreeTextBoxControls.Resources.Images.Utility." + filename + ".gif", this.AssemblyResourceHandlerPath);

					} else {
						return ResolveUrl(this.SupportFolder + "Utility/" + filename + ".gif");
						
					}
				case ResourceType.JavaScript:
					if (this.JavaScriptLocation == ResourceLocation.InternalResource) {
                        //return AssemblyResourceHandler.GetWebResourceUrl(typeof(FreeTextBox), "FreeTextBoxControls.Resources.JavaScript." + filename);
                        return ClientScriptWrapper.GetWebResourceUrl(this, "FreeTextBoxControls.Resources.JavaScript." + filename, this.AssemblyResourceHandlerPath);
					} else {
						return ResolveUrl(this.SupportFolder + filename);
					}
			}
		}
		#endregion
	}

	public class ImageInfo {
		public string Filename = string.Empty;
		public string PhysicalPath = string.Empty;
		public string AbsoluteWebPath = string.Empty;
		public int Width = 0;
		public int Height = 0;
		public long Size = 0;
		public string SizeString = string.Empty;
		public string ThumbnailAbsoluteWebPath = string.Empty;
		public int ThumbnailWidth = 0;
		public int ThumbnailHeight = 0;
		public int ThumbnailVSpace = 0;
	}
}

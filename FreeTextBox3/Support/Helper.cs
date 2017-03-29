using System;
using System.Drawing;
using System.Collections;

namespace FreeTextBoxControls.Support {
	/// <summary>
	/// Helper functions for FreeTextBox
	/// </summary>
	public class Helper {		
		
		/// <summary>
		/// Converts Color objects to HTML Hexedecimals
		/// </summary>	
		public static string ColorToHtml(Color c) {
			string html = "";
			html = c.R.ToString("X");
			if (html.Length < 2) html+="0";
			html += c.G.ToString("X");
			if (html.Length < 4) html+="0";	
			html += c.B.ToString("X");
			if (html.Length < 6) html+="0";
			return "#" + html;
		}
		/// <summary>
		/// Converts boolean values to JavaScript usable 0 or 1.
		/// </summary>	
		public static string BoolToNumber(bool theBool) {
			return theBool == true ? "1" : "0";
		}

		public static ArrayList GetColorList() {

			ArrayList colors = new ArrayList();
			colors.Add(Color.Black);
			colors.Add(Color.Gray);
			colors.Add(Color.DarkGray);
			colors.Add(Color.LightGray);
			colors.Add(Color.White);
			colors.Add(Color.Aquamarine);
			colors.Add(Color.Blue);
			colors.Add(Color.Navy);
			colors.Add(Color.Purple);
			colors.Add(Color.DeepPink);
			colors.Add(Color.Violet);
			colors.Add(Color.Pink);
			colors.Add(Color.DarkGreen);
			colors.Add(Color.Green);
			colors.Add(Color.YellowGreen);
			colors.Add(Color.Yellow);
			colors.Add(Color.Orange);
			colors.Add(Color.Red);
			colors.Add(Color.Brown);
			colors.Add(Color.BurlyWood);
			colors.Add(Color.Beige);

			return colors;
		}
		/// <summary>
		/// Fills empty <see cref="ToolbarDropDownList"/>s with default items.
		/// </summary>
		public static void PopulateDefaultDropDownList(ToolbarDropDownList toolbarDropDownList, FreeTextBox freeTextBox, ResourceManager resourceManager) {
			
			if (toolbarDropDownList.className == "FontForeColorsMenu") {
				if (freeTextBox.FontForeColorMenuList != null && freeTextBox.FontForeColorMenuList.Length > 0) {
					
					if (freeTextBox.FontForeColorMenuNames != null && freeTextBox.FontForeColorMenuList.Length == freeTextBox.FontForeColorMenuNames.Length) {
						for (int i=0; i<freeTextBox.FontForeColorMenuList.Length; i++) {
							Color color = freeTextBox.FontForeColorMenuList[i];
							toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontForeColorMenuNames[i],Helper.ColorToHtml(color),color));
						}
						
					} else {
						for (int i=0; i<freeTextBox.FontForeColorMenuList.Length; i++) {
							Color color = freeTextBox.FontForeColorMenuList[i];
							toolbarDropDownList.Items.Add(new ToolbarListItem(color.Name,Helper.ColorToHtml(color),color));
						}
					}
				} else {
					ArrayList colors = Helper.GetColorList();
					foreach(Color color in colors) {
						toolbarDropDownList.Items.Add(new ToolbarListItem(color.Name,Helper.ColorToHtml(color),color));
					}
				}
				
			} else if (toolbarDropDownList.className == "FontBackColorsMenu") {
					
				if (freeTextBox.FontBackColorMenuList != null && freeTextBox.FontBackColorMenuList.Length > 0) {
					
					if (freeTextBox.FontBackColorMenuNames != null && freeTextBox.FontBackColorMenuList.Length == freeTextBox.FontBackColorMenuNames.Length) {
						for (int i=0; i<freeTextBox.FontBackColorMenuList.Length; i++) {
							Color color = freeTextBox.FontBackColorMenuList[i];
							toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontBackColorMenuNames[i],Helper.ColorToHtml(color),color));
						}
						
					} else {
						for (int i=0; i<freeTextBox.FontBackColorMenuList.Length; i++) {
							Color color = freeTextBox.FontBackColorMenuList[i];
							toolbarDropDownList.Items.Add(new ToolbarListItem(color.Name,Helper.ColorToHtml(color),color));
						}
					}
				} else {
					ArrayList colors = Helper.GetColorList();
					foreach(Color color in colors) {
						toolbarDropDownList.Items.Add(new ToolbarListItem(color.Name,Helper.ColorToHtml(color),color));
					}
				}	

			} else if (toolbarDropDownList.className == "InsertHtmlMenu") {
					
				if (freeTextBox.InsertHtmlMenuList != null && freeTextBox.InsertHtmlMenuNames != null && freeTextBox.InsertHtmlMenuList.Length == freeTextBox.InsertHtmlMenuNames.Length) {
					
					for (int i=0; i<freeTextBox.InsertHtmlMenuList.Length; i++) {
						toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.InsertHtmlMenuNames[i],freeTextBox.InsertHtmlMenuList[i]));
					}
				}	
			} else if (toolbarDropDownList.className == "FontFacesMenu") {
				
				if (freeTextBox.FontFacesMenuList != null && freeTextBox.FontFacesMenuList.Length > 0) {
					
					if (freeTextBox.FontFacesMenuNames != null && freeTextBox.FontFacesMenuList.Length == freeTextBox.FontFacesMenuNames.Length) {
						for (int i=0; i<freeTextBox.FontFacesMenuList.Length; i++)
							toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontFacesMenuNames[i],freeTextBox.FontFacesMenuList[i]));
						
					} else {
						for (int i=0; i<freeTextBox.FontFacesMenuList.Length; i++)
							toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontFacesMenuList[i],freeTextBox.FontFacesMenuList[i]));

					}

				} else {				
					toolbarDropDownList.Items.Add(new ToolbarListItem("Arial"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("Courier New"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("Garamond"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("Georgia"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("Tahoma"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("Times","Times New Roman"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("Verdana"));				
				}
			} else if (toolbarDropDownList.className == "FontSizesMenu") {
				
				if (freeTextBox.FontSizesMenuList != null && freeTextBox.FontSizesMenuList.Length > 0) {

                    if (freeTextBox.FontSizesMenuNames != null && freeTextBox.FontSizesMenuList.Length == freeTextBox.FontSizesMenuNames.Length)
                    {
						for (int i=0; i<freeTextBox.FontSizesMenuList.Length; i++)
							toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontSizesMenuNames[i],freeTextBox.FontSizesMenuList[i]));
						
					} else {
						for (int i=0; i<freeTextBox.FontSizesMenuList.Length; i++)
							toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontSizesMenuList[i],freeTextBox.FontSizesMenuList[i]));

					}
				} else {
					toolbarDropDownList.Items.Add(new ToolbarListItem("1"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("2"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("3"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("4"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("5"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("6"));
				}
			} else if (toolbarDropDownList.className == "ParagraphMenu") {
				if (freeTextBox.ParagraphMenuList != null && freeTextBox.ParagraphMenuNames != null && freeTextBox.ParagraphMenuList.Length == freeTextBox.ParagraphMenuNames.Length) {
					for (int i=0; i<freeTextBox.ParagraphMenuList.Length; i++)
						toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.ParagraphMenuNames[i],freeTextBox.ParagraphMenuList[i]));

				} else {		
					toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Normal"),"<p>"));
					toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading1"),"<h1>"));
					toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading2"),"<h2>"));
					toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading3"),"<h3>"));
					toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading4"),"<h4>"));
					toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading5"),"<h5>"));
					toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading6"),"<h6>"));
					//toolbarDropDownList.Items.Add(new ToolbarListItem("Directory List","<dir>"));
					//toolbarDropDownList.Items.Add(new ToolbarListItem("Menu List","<menu>"));			
					toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Formatted"),"<pre>"));
					toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Address"),"<address>"));
				}
			} else if (toolbarDropDownList.className == "SymbolsMenu") {

				if (freeTextBox.SymbolsMenuList != null && freeTextBox.SymbolsMenuList.Length > 0) {
					for (int i=0; i<freeTextBox.SymbolsMenuList.Length; i++)
						toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.SymbolsMenuList[i],freeTextBox.SymbolsMenuList[i]));


				} else {				
					
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#8364;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#162;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#163;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#165;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#167;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#191;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#161"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#169;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#174;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#8482;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("-"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#8211;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#8212;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#8216;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#8217;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#8220;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#8221;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#225;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#233;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#237;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#239;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#241;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#243;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#176;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#183;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#171;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#187;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#188;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#189;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#190;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#185;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#178;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#179;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#247;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#177;"));
					toolbarDropDownList.Items.Add(new ToolbarListItem("&#215;"));
				}
		
			} else if (toolbarDropDownList.className == "StylesMenu") {

				if (freeTextBox.DesignModeCss != string.Empty && freeTextBox.AutoParseStyles) {
					string[] styles = StyleSheetParser.ParseStyleSheet(freeTextBox.DesignModeCssViewState);
					foreach (string style in styles) 
						toolbarDropDownList.Items.Add(new ToolbarListItem(style,style));
				} else if (freeTextBox.StylesMenuList != null && freeTextBox.StylesMenuList.Length > 0) {
					
					if (freeTextBox.StylesMenuList.Length == freeTextBox.StylesMenuNames.Length) {

						string[] styles = freeTextBox.StylesMenuList;
						string[] names = freeTextBox.StylesMenuNames;
						for (int i=0; i<names.Length; i++)
							toolbarDropDownList.Items.Add(new ToolbarListItem(names[i],styles[i]));
					} else {
						for (int i=0; i<freeTextBox.StylesMenuList.Length; i++)
							toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.StylesMenuList[i],freeTextBox.StylesMenuList[i]));


					}
					
				}
			}
		}
	}
}

using System;
using System.Collections;
using System.Drawing;
namespace FreeTextBoxControls.Support
{
	public class Helper
	{
		public static string ColorToHtml(Color c)
		{
			string text = c.R.ToString("X");
			if (text.Length < 2)
			{
				text += "0";
			}
			text += c.G.ToString("X");
			if (text.Length < 4)
			{
				text += "0";
			}
			text += c.B.ToString("X");
			if (text.Length < 6)
			{
				text += "0";
			}
			return "#" + text;
		}
		public static string BoolToNumber(bool theBool)
		{
			if (!theBool)
			{
				return "0";
			}
			return "1";
		}
		public static ArrayList GetColorList()
		{
			return new ArrayList
			{
				Color.Black,
				Color.Gray,
				Color.DarkGray,
				Color.LightGray,
				Color.White,
				Color.Aquamarine,
				Color.Blue,
				Color.Navy,
				Color.Purple,
				Color.DeepPink,
				Color.Violet,
				Color.Pink,
				Color.DarkGreen,
				Color.Green,
				Color.YellowGreen,
				Color.Yellow,
				Color.Orange,
				Color.Red,
				Color.Brown,
				Color.BurlyWood,
				Color.Beige
			};
		}
		public static void PopulateDefaultDropDownList(ToolbarDropDownList toolbarDropDownList, FreeTextBox freeTextBox, ResourceManager resourceManager)
		{
			if (toolbarDropDownList.className == "FontForeColorsMenu")
			{
				if (freeTextBox.FontForeColorMenuList != null && freeTextBox.FontForeColorMenuList.Length > 0)
				{
					if (freeTextBox.FontForeColorMenuNames != null && freeTextBox.FontForeColorMenuList.Length == freeTextBox.FontForeColorMenuNames.Length)
					{
						for (int i = 0; i < freeTextBox.FontForeColorMenuList.Length; i++)
						{
							Color color = freeTextBox.FontForeColorMenuList[i];
							toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontForeColorMenuNames[i], Helper.ColorToHtml(color), color));
						}
						return;
					}
					for (int j = 0; j < freeTextBox.FontForeColorMenuList.Length; j++)
					{
						Color color2 = freeTextBox.FontForeColorMenuList[j];
						toolbarDropDownList.Items.Add(new ToolbarListItem(color2.Name, Helper.ColorToHtml(color2), color2));
					}
					return;
				}
				else
				{
					ArrayList colorList = Helper.GetColorList();
					IEnumerator enumerator = colorList.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Color color3 = (Color)enumerator.Current;
							toolbarDropDownList.Items.Add(new ToolbarListItem(color3.Name, Helper.ColorToHtml(color3), color3));
						}
						return;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
			if (toolbarDropDownList.className == "FontBackColorsMenu")
			{
				if (freeTextBox.FontBackColorMenuList != null && freeTextBox.FontBackColorMenuList.Length > 0)
				{
					if (freeTextBox.FontBackColorMenuNames != null && freeTextBox.FontBackColorMenuList.Length == freeTextBox.FontBackColorMenuNames.Length)
					{
						for (int k = 0; k < freeTextBox.FontBackColorMenuList.Length; k++)
						{
							Color color4 = freeTextBox.FontBackColorMenuList[k];
							toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontBackColorMenuNames[k], Helper.ColorToHtml(color4), color4));
						}
						return;
					}
					for (int l = 0; l < freeTextBox.FontBackColorMenuList.Length; l++)
					{
						Color color5 = freeTextBox.FontBackColorMenuList[l];
						toolbarDropDownList.Items.Add(new ToolbarListItem(color5.Name, Helper.ColorToHtml(color5), color5));
					}
					return;
				}
				else
				{
					ArrayList colorList2 = Helper.GetColorList();
					IEnumerator enumerator2 = colorList2.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							Color color6 = (Color)enumerator2.Current;
							toolbarDropDownList.Items.Add(new ToolbarListItem(color6.Name, Helper.ColorToHtml(color6), color6));
						}
						return;
					}
					finally
					{
						IDisposable disposable2 = enumerator2 as IDisposable;
						if (disposable2 != null)
						{
							disposable2.Dispose();
						}
					}
				}
			}
			if (toolbarDropDownList.className == "InsertHtmlMenu")
			{
				if (freeTextBox.InsertHtmlMenuList != null && freeTextBox.InsertHtmlMenuNames != null && freeTextBox.InsertHtmlMenuList.Length == freeTextBox.InsertHtmlMenuNames.Length)
				{
					for (int m = 0; m < freeTextBox.InsertHtmlMenuList.Length; m++)
					{
						toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.InsertHtmlMenuNames[m], freeTextBox.InsertHtmlMenuList[m]));
					}
					return;
				}
			}
			else
			{
				if (toolbarDropDownList.className == "FontFacesMenu")
				{
					if (freeTextBox.FontFacesMenuList == null || freeTextBox.FontFacesMenuList.Length <= 0)
					{
						toolbarDropDownList.Items.Add(new ToolbarListItem("Arial"));
						toolbarDropDownList.Items.Add(new ToolbarListItem("Courier New"));
						toolbarDropDownList.Items.Add(new ToolbarListItem("Garamond"));
						toolbarDropDownList.Items.Add(new ToolbarListItem("Georgia"));
						toolbarDropDownList.Items.Add(new ToolbarListItem("Tahoma"));
						toolbarDropDownList.Items.Add(new ToolbarListItem("Times", "Times New Roman"));
						toolbarDropDownList.Items.Add(new ToolbarListItem("Verdana"));
						return;
					}
					if (freeTextBox.FontFacesMenuNames != null && freeTextBox.FontFacesMenuList.Length == freeTextBox.FontFacesMenuNames.Length)
					{
						for (int n = 0; n < freeTextBox.FontFacesMenuList.Length; n++)
						{
							toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontFacesMenuNames[n], freeTextBox.FontFacesMenuList[n]));
						}
						return;
					}
					for (int num = 0; num < freeTextBox.FontFacesMenuList.Length; num++)
					{
						toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontFacesMenuList[num], freeTextBox.FontFacesMenuList[num]));
					}
					return;
				}
				else
				{
					if (toolbarDropDownList.className == "FontSizesMenu")
					{
						if (freeTextBox.FontSizesMenuList == null || freeTextBox.FontSizesMenuList.Length <= 0)
						{
							toolbarDropDownList.Items.Add(new ToolbarListItem("1"));
							toolbarDropDownList.Items.Add(new ToolbarListItem("2"));
							toolbarDropDownList.Items.Add(new ToolbarListItem("3"));
							toolbarDropDownList.Items.Add(new ToolbarListItem("4"));
							toolbarDropDownList.Items.Add(new ToolbarListItem("5"));
							toolbarDropDownList.Items.Add(new ToolbarListItem("6"));
							return;
						}
						if (freeTextBox.FontSizesMenuNames != null && freeTextBox.FontSizesMenuList.Length == freeTextBox.FontSizesMenuNames.Length)
						{
							for (int num2 = 0; num2 < freeTextBox.FontSizesMenuList.Length; num2++)
							{
								toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontSizesMenuNames[num2], freeTextBox.FontSizesMenuList[num2]));
							}
							return;
						}
						for (int num3 = 0; num3 < freeTextBox.FontSizesMenuList.Length; num3++)
						{
							toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.FontSizesMenuList[num3], freeTextBox.FontSizesMenuList[num3]));
						}
						return;
					}
					else
					{
						if (toolbarDropDownList.className == "ParagraphMenu")
						{
							if (freeTextBox.ParagraphMenuList != null && freeTextBox.ParagraphMenuNames != null && freeTextBox.ParagraphMenuList.Length == freeTextBox.ParagraphMenuNames.Length)
							{
								for (int num4 = 0; num4 < freeTextBox.ParagraphMenuList.Length; num4++)
								{
									toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.ParagraphMenuNames[num4], freeTextBox.ParagraphMenuList[num4]));
								}
								return;
							}
							toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Normal"), "<p>"));
							toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading1"), "<h1>"));
							toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading2"), "<h2>"));
							toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading3"), "<h3>"));
							toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading4"), "<h4>"));
							toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading5"), "<h5>"));
							toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Heading6"), "<h6>"));
							toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Formatted"), "<pre>"));
							toolbarDropDownList.Items.Add(new ToolbarListItem(resourceManager.GetString("ParagraphMenu_Address"), "<address>"));
							return;
						}
						else
						{
							if (toolbarDropDownList.className == "SymbolsMenu")
							{
								if (freeTextBox.SymbolsMenuList != null && freeTextBox.SymbolsMenuList.Length > 0)
								{
									for (int num5 = 0; num5 < freeTextBox.SymbolsMenuList.Length; num5++)
									{
										toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.SymbolsMenuList[num5], freeTextBox.SymbolsMenuList[num5]));
									}
									return;
								}
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
								return;
							}
							else
							{
								if (toolbarDropDownList.className == "StylesMenu")
								{
									if (freeTextBox.DesignModeCss != string.Empty && freeTextBox.AutoParseStyles)
									{
										string[] array = StyleSheetParser.ParseStyleSheet(freeTextBox.DesignModeCssViewState);
										string[] array2 = array;
										for (int num6 = 0; num6 < array2.Length; num6++)
										{
											string text = array2[num6];
											toolbarDropDownList.Items.Add(new ToolbarListItem(text, text));
										}
										return;
									}
									if (freeTextBox.StylesMenuList != null && freeTextBox.StylesMenuList.Length > 0)
									{
										if (freeTextBox.StylesMenuList.Length == freeTextBox.StylesMenuNames.Length)
										{
											string[] stylesMenuList = freeTextBox.StylesMenuList;
											string[] stylesMenuNames = freeTextBox.StylesMenuNames;
											for (int num7 = 0; num7 < stylesMenuNames.Length; num7++)
											{
												toolbarDropDownList.Items.Add(new ToolbarListItem(stylesMenuNames[num7], stylesMenuList[num7]));
											}
											return;
										}
										for (int num8 = 0; num8 < freeTextBox.StylesMenuList.Length; num8++)
										{
											toolbarDropDownList.Items.Add(new ToolbarListItem(freeTextBox.StylesMenuList[num8], freeTextBox.StylesMenuList[num8]));
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}

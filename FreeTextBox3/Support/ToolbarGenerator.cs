using System;
using FreeTextBoxControls.Support;

namespace FreeTextBoxControls {
	/// <summary>
	/// Builds Toolbars and ToolbarItems from strings 
	/// </summary>	
	public class ToolbarGenerator {
		//Toolbar layouts
		public static string DefaultConfigString = "ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu|Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImage,InsertRule|Cut,Copy,Paste;Undo,Redo,Print";

		public static string DefaultPlusTablesConfigString = "ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu|Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImage,InsertRule|Cut,Copy,Paste;Undo,Redo,Print;InsertTable,InsertTableRowBelow,InsertTableRowAbove,DeleteTableRow,InsertTableColumnBelow,InsertTableColumnAbove,DeleteTableColumn";
		/*
		public static string AlternateConfigString = "Save,Print,Undo,Redo,WordClean,InsertTable|ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorPicker,FontBackColorPicker,SymbolsMenu|Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImageFromGallery,InsertRule|Cut,Copy,Paste,ieSpellCheck";
		*/
		public static string EnableAllConfigString = "ParagraphMenu, FontFacesMenu, FontSizesMenu, FontForeColorsMenu, FontForeColorPicker, FontBackColorsMenu, FontBackColorPicker, Bold, Italic, Underline, Strikethrough, Superscript, Subscript, CreateLink, Unlink, RemoveFormat, JustifyLeft, JustifyRight, JustifyCenter, JustifyFull, BulletedList, NumberedList, Indent, Outdent, Cut, Copy, Paste, Delete, Undo, Redo, Print, Save, SymbolsMenu, StylesMenu, InsertHtmlMenu, InsertRule, InsertDate, InsertTime, ieSpellCheck, NetSpell, WordClean, InsertImageFromGallery, InsertTable, InsertTableRowBelow, InsertTableRowAbove, DeleteTableRow, InsertTableColumnBelow, InsertTableColumnAbove, DeleteTableColumn, InsertForm, InsertForm,InsertTextBox,InsertTextArea,InsertRadioButton,InsertCheckBox,InsertDropDownList,InsertButton, InsertImageFromGallery, InsertDiv";
		
		public static string MinimalConfigString = "Bold,Italic,Underline";	
		
		/// <summary>
		/// Returns a Toolbars with the default configuration
		/// </summary>			
		public static ToolbarCollection Default {
			get {
				return ToolbarGenerator.ToolbarsFromString(DefaultConfigString);
			}
		}
	
		/// <summary>
		/// Returns a Toolbars with the all buttons and dropdowns enabled
		/// </summary>			
		public static ToolbarCollection EnableAll {
			get {
				return ToolbarGenerator.ToolbarsFromString(EnableAllConfigString);
			}
		}

		/// <summary>
		/// Returns a Toolbars with the default configuration plus tables
		/// </summary>			
		public static ToolbarCollection DefaultPlusTables {
			get {
				return ToolbarGenerator.ToolbarsFromString(DefaultPlusTablesConfigString);
			}
		}
	
		/*
		/// <summary>
		/// Returns a Toolbars with an alternate configuration
		/// </summary>			
		public static ToolbarCollection Alternate {
			get {
				return ToolbarGenerator.ToolbarsFromString(AlternateConfigString);
			}
		}
		*/
		/// <summary>
		/// Returns a Toolbars with only bold, italic, and underline
		/// </summary>			
		public static ToolbarCollection Minimal {
			get {
				return ToolbarsFromString(MinimalConfigString);
			}
		}

		/// <summary>
		/// Generates Toolbars from ToolbarLayout string
		/// </summary>
		public static ToolbarCollection ToolbarsFromString(string toolbarLayout) {
			ToolbarCollection toolbarCollection = new ToolbarCollection();
			try {
				string[] ToolbarStrings = toolbarLayout.Replace(" ","").Replace(";",",;,").Replace(",,",",").ToLower().Split(new char[] {'|'});
				for (int i=0; i<ToolbarStrings.Length; i++) {
					Toolbar toolbar = new Toolbar();
					string[] ItemStrings = ToolbarStrings[i].Split(new char[] {','});
					for (int j = 0; j<ItemStrings.Length; j++) {
						toolbar.Items.Add(ToolbarItemFromString(ItemStrings[j]));
					}
					toolbarCollection.Add(toolbar);
				}
			} catch (Exception e) {
				throw new Exception("Invalid ToolbarLayout -> " + e.ToString());
			}
			return toolbarCollection;
		}	

		/// <summary>
		/// Returns a ToolbarItem from it's string representation.  If an invalid string is given, a ToolbarSeparator is returned.
		/// </summary>
		public static ToolbarItem ToolbarItemFromString(string StringName) {
			switch (StringName.ToLower()) {
				case "save":
					return new Save(); 
				case "bold":
					return new Bold();
				case "italic":
					return new Italic();
				case "underline":
					return new Underline();
				case "strikethrough":
					return new StrikeThrough();
				case "superscript":
					return new SuperScript();
				case "subscript":
					return new SubScript();
				case "removeformat":
					return new RemoveFormat();

				case "justifyleft":
					return new JustifyLeft();
				case "justifycenter":
					return new JustifyCenter();
				case "justifyright":
					return new JustifyRight();
				case "justifyfull":
					return new JustifyFull();
				case "bulletedlist":
					return new BulletedList();
				case "numberedlist":
					return new NumberedList();
				case "indent":
					return new Indent();
				case "outdent":
					return new Outdent();
				case "cut":
					return new Cut();
				case "copy":
					return new Copy();
				case "paste":
					return new Paste();
				case "delete":
					return new Delete();
				case "undo":
					return new Undo();
				case "redo":
					return new Redo();
				case "print":
					return new Print();
				case "createlink":
					return new CreateLink();
				case "unlink":
					return new Unlink();
				case "insertimagefromgallery":
					return new InsertImageFromGallery();		
				case "insertimage":
					return new InsertImage();				

				case "insertrule":
					return new InsertRule();
				case "insertdate":
					return new InsertDate();
				case "inserttime":
					return new InsertTime();
				//case "wordcount":
				//	return new WordCount();
				case "fontfacesmenu":
					return new FontFacesMenu();
				case "fontsizesmenu":
					return new FontSizesMenu();
				case "fontforecolorsmenu":
					return new FontForeColorsMenu();
				case "fontbackcolorsmenu":
					return new FontBackColorsMenu();
				case "stylesmenu":
					return new StylesMenu();
				case "inserthtmlmenu":
					return new InsertHtmlMenu();
				case "symbolsmenu":
					return new SymbolsMenu();
				case "paragraphmenu":
					return new ParagraphMenu();
				case "wordclean":
					return new WordClean();
				case "netspell":
					return new NetSpell();
				case "iespellcheck":
					return new IeSpellCheck();
	
				// table functions
				case "inserttable":
					return new InsertTable();	
				case "edittable":
					return new EditTable();				
				case "deletetablerow":
					return new DeleteTableRow();
				case "deletetablecolumn":
					return new DeleteTableColumn();				
				case "inserttablerow":
				case "inserttablerowafter":
					return new InsertTableRowAfter();
				case "inserttablerowbefore":
					return new InsertTableRowBefore();
				case "inserttablecolumn":
				case "inserttablecolumnafter":
					return new InsertTableColumnAfter();
				case "inserttablecolumnbefore":
					return new InsertTableColumnBefore();

				// form
					// pro features
				case "insertform":
					return new InsertForm();
				case "insertbutton":
					return new InsertButton();
				case "insertradiobutton":
					return new InsertRadioButton();
				case "insertdropdownlist":
					return new InsertDropDownList();
				case "insertcheckbox":
					return new InsertCheckBox();
				case "inserttextarea":
					return new InsertTextArea();
				case "inserttextbox":
					return new InsertTextBox();

				// pro features
				case "preview":
					return new Preview();
				case "selectall":
					return new SelectAll();
				case "fontforecolorpicker":
					return new FontForeColorPicker();
				case "fontbackcolorpicker":
					return new FontBackColorPicker();
				case "editstyle":
					return new EditStyle();
				case "insertdiv":
					return new InsertDiv();

				default:					
					return new ToolbarSeparator();
			}
		}	
	}
}

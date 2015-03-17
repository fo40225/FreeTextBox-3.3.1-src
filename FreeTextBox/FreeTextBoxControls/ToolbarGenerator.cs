using FreeTextBoxControls.Support;
using System;
namespace FreeTextBoxControls
{
	public class ToolbarGenerator
	{
		public static string DefaultConfigString = "ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu|Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImage,InsertRule|Cut,Copy,Paste;Undo,Redo,Print";
		public static string DefaultPlusTablesConfigString = "ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu|Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImage,InsertRule|Cut,Copy,Paste;Undo,Redo,Print;InsertTable,InsertTableRowBelow,InsertTableRowAbove,DeleteTableRow,InsertTableColumnBelow,InsertTableColumnAbove,DeleteTableColumn";
		public static string EnableAllConfigString = "ParagraphMenu, FontFacesMenu, FontSizesMenu, FontForeColorsMenu, FontForeColorPicker, FontBackColorsMenu, FontBackColorPicker, Bold, Italic, Underline, Strikethrough, Superscript, Subscript, CreateLink, Unlink, RemoveFormat, JustifyLeft, JustifyRight, JustifyCenter, JustifyFull, BulletedList, NumberedList, Indent, Outdent, Cut, Copy, Paste, Delete, Undo, Redo, Print, Save, SymbolsMenu, StylesMenu, InsertHtmlMenu, InsertRule, InsertDate, InsertTime, ieSpellCheck, NetSpell, WordClean, InsertImageFromGallery, InsertTable, InsertTableRowBelow, InsertTableRowAbove, DeleteTableRow, InsertTableColumnBelow, InsertTableColumnAbove, DeleteTableColumn, InsertForm, InsertForm,InsertTextBox,InsertTextArea,InsertRadioButton,InsertCheckBox,InsertDropDownList,InsertButton, InsertImageFromGallery, InsertDiv";
		public static string MinimalConfigString = "Bold,Italic,Underline";
		public static ToolbarCollection Default
		{
			get
			{
				return ToolbarGenerator.ToolbarsFromString(ToolbarGenerator.DefaultConfigString);
			}
		}
		public static ToolbarCollection EnableAll
		{
			get
			{
				return ToolbarGenerator.ToolbarsFromString(ToolbarGenerator.EnableAllConfigString);
			}
		}
		public static ToolbarCollection DefaultPlusTables
		{
			get
			{
				return ToolbarGenerator.ToolbarsFromString(ToolbarGenerator.DefaultPlusTablesConfigString);
			}
		}
		public static ToolbarCollection Minimal
		{
			get
			{
				return ToolbarGenerator.ToolbarsFromString(ToolbarGenerator.MinimalConfigString);
			}
		}
		public static ToolbarCollection ToolbarsFromString(string toolbarLayout)
		{
			ToolbarCollection toolbarCollection = new ToolbarCollection();
			try
			{
				string[] array = toolbarLayout.Replace(" ", "").Replace(";", ",;,").Replace(",,", ",").ToLower().Split(new char[]
				{
					'|'
				});
				for (int i = 0; i < array.Length; i++)
				{
					Toolbar toolbar = new Toolbar();
					string[] array2 = array[i].Split(new char[]
					{
						','
					});
					for (int j = 0; j < array2.Length; j++)
					{
						toolbar.Items.Add(ToolbarGenerator.ToolbarItemFromString(array2[j]));
					}
					toolbarCollection.Add(toolbar);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Invalid ToolbarLayout -> " + ex.ToString());
			}
			return toolbarCollection;
		}
		public static ToolbarItem ToolbarItemFromString(string StringName)
		{
			string key;
			switch (key = StringName.ToLower())
			{
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
			}
			return new ToolbarSeparator();
		}
	}
}

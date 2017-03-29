using System;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.IO;
using FreeTextBoxControls;

namespace FreeTextBoxControls.Design {
	/// <summary>
	/// This is the class which displays FreeTextBox in VS.NET
	/// </summary>
	/// <exclude />
	public class FreeTextBoxDesigner : ControlDesigner {

		public FreeTextBoxDesigner() {}

		/*
		 public override bool AllowResize {
			get {return false;}
		}
		*/

		private FreeTextBox ftb = null;
        string message = "";

		public override void Initialize(IComponent component) {

            if (component == null) {
                message = "Component is null";
            } else {
                message = "";
                ftb = (FreeTextBox)component;
			    base.Initialize(ftb);
			    base.Initialize(component);
            }
		}

		public override string GetDesignTimeHtml() {

            if (message != "")
            {
                return message;
            }

			StringWriter sw = new StringWriter();

			HtmlTextWriter htw = new HtmlTextWriter(sw);
			HtmlTable t = new HtmlTable();
			t.CellPadding = 3;
			t.CellSpacing = 0;
			t.BorderColor = ColorTranslator.ToHtml(ftb.EditorBorderColorDark);
			t.BgColor = ColorTranslator.ToHtml(ftb.BackColor);			
			t.Width = ftb.Width.ToString();
			t.Height = ftb.Height.ToString();
			HtmlTableRow tr = new HtmlTableRow();
			HtmlTableCell td = new HtmlTableCell();
			td.VAlign = "top";
			td.Align = "center";
			
			// inner table for iframe
				HtmlTable iframe = new HtmlTable();
				iframe.BgColor = "#FFFFFF";
				iframe.Width="100%";
				iframe.Height="100%";
				iframe.CellPadding = 0;
				iframe.CellSpacing = 0;
				iframe.Style.Add("border","1 solid " + ColorTranslator.ToHtml(ftb.EditorBorderColorDark));
				HtmlTableRow tr2 = new HtmlTableRow();
				HtmlTableCell td2 = new HtmlTableCell();
				td2.VAlign = "middle";
				td2.Align = "center";
				td2.Controls.Add(new LiteralControl("<b><font face=arial size=2><font color=green>Free</font>TextBox:</b> " + ftb.ID + "</font>"));
				tr2.Cells.Add(td2);
				iframe.Rows.Add(tr2);

			td.Controls.Add(iframe);
			td.Controls.Add(new LiteralControl("<br><br><br>"));
			tr.Cells.Add(td);
			t.Rows.Add(tr);
			t.RenderControl(htw);
			return sw.ToString();
		}
	}
}

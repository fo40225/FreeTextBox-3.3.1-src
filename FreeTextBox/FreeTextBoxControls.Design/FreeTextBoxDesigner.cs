using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
namespace FreeTextBoxControls.Design
{
	public class FreeTextBoxDesigner : ControlDesigner
	{
		private FreeTextBox ftb;
		private string message = "";
		public override void Initialize(IComponent component)
		{
			if (component == null)
			{
				this.message = "Component is null";
				return;
			}
			this.message = "";
			this.ftb = (FreeTextBox)component;
			base.Initialize(this.ftb);
			base.Initialize(component);
		}
		public override string GetDesignTimeHtml()
		{
			if (this.message != "")
			{
				return this.message;
			}
			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
			HtmlTable htmlTable = new HtmlTable();
			htmlTable.CellPadding = 3;
			htmlTable.CellSpacing = 0;
			htmlTable.BorderColor = ColorTranslator.ToHtml(this.ftb.EditorBorderColorDark);
			htmlTable.BgColor = ColorTranslator.ToHtml(this.ftb.BackColor);
			htmlTable.Width = this.ftb.Width.ToString();
			htmlTable.Height = this.ftb.Height.ToString();
			HtmlTableRow htmlTableRow = new HtmlTableRow();
			HtmlTableCell htmlTableCell = new HtmlTableCell();
			htmlTableCell.VAlign = "top";
			htmlTableCell.Align = "center";
			HtmlTable htmlTable2 = new HtmlTable();
			htmlTable2.BgColor = "#FFFFFF";
			htmlTable2.Width = "100%";
			htmlTable2.Height = "100%";
			htmlTable2.CellPadding = 0;
			htmlTable2.CellSpacing = 0;
			htmlTable2.Style.Add("border", "1 solid " + ColorTranslator.ToHtml(this.ftb.EditorBorderColorDark));
			HtmlTableRow htmlTableRow2 = new HtmlTableRow();
			HtmlTableCell htmlTableCell2 = new HtmlTableCell();
			htmlTableCell2.VAlign = "middle";
			htmlTableCell2.Align = "center";
			htmlTableCell2.Controls.Add(new LiteralControl("<b><font face=arial size=2><font color=green>Free</font>TextBox:</b> " + this.ftb.ID + "</font>"));
			htmlTableRow2.Cells.Add(htmlTableCell2);
			htmlTable2.Rows.Add(htmlTableRow2);
			htmlTableCell.Controls.Add(htmlTable2);
			htmlTableCell.Controls.Add(new LiteralControl("<br><br><br>"));
			htmlTableRow.Cells.Add(htmlTableCell);
			htmlTable.Rows.Add(htmlTableRow);
			htmlTable.RenderControl(writer);
			return stringWriter.ToString();
		}
	}
}

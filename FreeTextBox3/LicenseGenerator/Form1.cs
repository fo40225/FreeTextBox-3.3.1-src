using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace LicenseGenerator
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button GenerateLicenseButton;
		private System.Windows.Forms.GroupBox groupbox2;
		private System.Windows.Forms.TextBox Output;
		private System.Windows.Forms.ComboBox LicenseType;
		private System.Windows.Forms.TextBox SecondField;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox DecryptedLicense;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SecondField = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.LicenseType = new System.Windows.Forms.ComboBox();
			this.GenerateLicenseButton = new System.Windows.Forms.Button();
			this.groupbox2 = new System.Windows.Forms.GroupBox();
			this.Output = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.DecryptedLicense = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.groupBox1.SuspendLayout();
			this.groupbox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// SecondField
			// 
			this.SecondField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SecondField.Location = new System.Drawing.Point(16, 56);
			this.SecondField.Name = "SecondField";
			this.SecondField.Size = new System.Drawing.Size(232, 20);
			this.SecondField.TabIndex = 0;
			this.SecondField.Text = "Enter Company Name Here";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.LicenseType);
			this.groupBox1.Controls.Add(this.SecondField);
			this.groupBox1.Location = new System.Drawing.Point(16, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(264, 88);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "License Type";
			this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
			// 
			// LicenseType
			// 
			this.LicenseType.Items.AddRange(new object[] {
															 "SingleLicense",
															 "DistributionLicense",
															 "ExpiringLicense"});
			this.LicenseType.Location = new System.Drawing.Point(16, 24);
			this.LicenseType.Name = "LicenseType";
			this.LicenseType.Size = new System.Drawing.Size(232, 21);
			this.LicenseType.TabIndex = 1;
			// 
			// GenerateLicenseButton
			// 
			this.GenerateLicenseButton.Location = new System.Drawing.Point(80, 112);
			this.GenerateLicenseButton.Name = "GenerateLicenseButton";
			this.GenerateLicenseButton.Size = new System.Drawing.Size(120, 23);
			this.GenerateLicenseButton.TabIndex = 2;
			this.GenerateLicenseButton.Text = "Generate License";
			this.GenerateLicenseButton.Click += new System.EventHandler(this.GenerateLicenseButton_Click);
			// 
			// groupbox2
			// 
			this.groupbox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupbox2.Controls.Add(this.label1);
			this.groupbox2.Controls.Add(this.button1);
			this.groupbox2.Controls.Add(this.Output);
			this.groupbox2.Location = new System.Drawing.Point(16, 144);
			this.groupbox2.Name = "groupbox2";
			this.groupbox2.Size = new System.Drawing.Size(264, 152);
			this.groupbox2.TabIndex = 3;
			this.groupbox2.TabStop = false;
			this.groupbox2.Text = "Output";
			// 
			// Output
			// 
			this.Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Output.Location = new System.Drawing.Point(16, 24);
			this.Output.Multiline = true;
			this.Output.Name = "Output";
			this.Output.Size = new System.Drawing.Size(232, 72);
			this.Output.TabIndex = 0;
			this.Output.Text = "";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.DecryptedLicense);
			this.groupBox3.Location = new System.Drawing.Point(16, 304);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(264, 112);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Decrypted";
			// 
			// DecryptedLicense
			// 
			this.DecryptedLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.DecryptedLicense.Location = new System.Drawing.Point(16, 24);
			this.DecryptedLicense.Multiline = true;
			this.DecryptedLicense.Name = "DecryptedLicense";
			this.DecryptedLicense.Size = new System.Drawing.Size(232, 72);
			this.DecryptedLicense.TabIndex = 0;
			this.DecryptedLicense.Text = "";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(168, 112);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "Save";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 112);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 32);
			this.label1.TabIndex = 2;
			this.label1.Text = "Save as \'FreeTextBox.lic\' and place in /bin/ folder";
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 429);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupbox2);
			this.Controls.Add(this.GenerateLicenseButton);
			this.Controls.Add(this.groupBox1);
			this.Name = "Form1";
			this.Text = "FreeTextBox3 License Generator";
			this.groupBox1.ResumeLayout(false);
			this.groupbox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void GenerateLicenseButton_Click(object sender, System.EventArgs e) {
			//Output.Text = "FreeTextBox License\n[" + LicenseType.Items[LicenseType.SelectedIndex].ToString() + "]\n[" + SecondField.Text + "]";
			Encryptor encryptor = new Encryptor();
			string licenseType = LicenseType.Items[LicenseType.SelectedIndex].ToString();
			string secondField = SecondField.Text;
			Output.Text = "FreeTextBox License\r\n[" + encryptor.EncryptData(licenseType) + "]\r\n[" + encryptor.EncryptData(secondField) + "]";

			Match m = Regex.Match(Output.Text, "FreeTextBox License" +
				"\r\n" +
				@"\[(?<licenseType>[^\]]+)\]" +
				"\r\n" +
				@"\[(?<secondField>[^\]]+)\]");
			
			licenseType = encryptor.DecryptData(m.Groups["licenseType"].Value);
			secondField = encryptor.DecryptData(m.Groups["secondField"].Value);

			DecryptedLicense.Text = "FreeTextBox License\r\n[" + licenseType + "]\r\n[" + secondField + "]";
		}

		private void groupBox1_Enter(object sender, System.EventArgs e) {
		
		}

		private void label1_Click(object sender, System.EventArgs e) {
		
		}

		private void button1_Click(object sender, System.EventArgs e) {
			saveFileDialog1.DefaultExt = "lic";
			saveFileDialog1.FileName = "FreeTextBox.lic";
			
			if(this.saveFileDialog1.ShowDialog()==DialogResult.OK) {
				try {
					System.IO.StreamWriter streamWriter = System.IO.File.CreateText(saveFileDialog1.FileName);
					streamWriter.WriteLine(Output.Text);
					streamWriter.Close();
				} catch (Exception ex) {
					throw new Exception("Error Saving License: " + ex.ToString());
				}
			} 

		}
	}
}

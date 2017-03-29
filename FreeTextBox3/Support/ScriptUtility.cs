using System;
using System.IO;
using System.Text;
using System.Reflection;
using FreeTextBoxControls;

namespace FreeTextBoxControls.Support {
	/// <summary>
	/// Outputs FreeTextBox resources such as JavaScript files and images
	/// </summary>
	public class ResourceCreator {

		/// <summary>
		/// Saves the JavaScript for <see cref="ToolbarItem"/>s to the desired location
		/// </summary>
		public static void SaveToolbarScript(string filePath) {
			StreamWriter sw = File.CreateText(filePath);
			sw.Write(ResourceCreator.CreateToolbarItemScript());
			sw.Close();
		}

		/// <summary>
		/// Generates the JavaScript for <see cref="ToolbarItem"/>s
		/// </summary>
		public static string CreateToolbarItemScript() {
			
			StringBuilder sb = new StringBuilder();	    

			string versionInfo = System.Reflection.Assembly.GetAssembly(typeof(FreeTextBox)).FullName;
			int start = versionInfo.IndexOf("Version=")+8;
			int end = versionInfo.IndexOf(",",start);
			versionInfo = versionInfo.Substring(start,end-start);
			versionInfo = versionInfo.Replace(".5000","");
	
			sb.Append("//** FreeTextBox Builtin ToolbarItems Script (" + versionInfo + ") ***/" + "\n");
			sb.Append("//   by John Dyer" + "\n");
			sb.Append("//   http://www.freetextbox.com/" + "\n");
			sb.Append("//**********************************************/" + "\n");	
			sb.Append("\n");
	
			//Get the mscorlib assembly, it's the one Object is defined in
			Assembly a = typeof(FreeTextBox).Module.Assembly;

			//Get all the types in this assembly
			Type [] types = a.GetTypes ();

			foreach (Type t in types) {
				if (t.IsClass) { 
			
					if (t.BaseType.BaseType == typeof(ToolbarItem)) {

						object button = t.InvokeMember(null, 
							BindingFlags.DeclaredOnly | 
							BindingFlags.Public | BindingFlags.NonPublic | 
							BindingFlags.Instance | BindingFlags.CreateInstance, null, null, new object[] {});
				
						string scriptBlock = t.InvokeMember("BuiltInScript", BindingFlags.GetProperty, null, button, new object[] {}).ToString();
						
						/*
						Output.Text += " - <b>" + t.ToString() + "</b><br>";
						Output.Text += 	
							"<pre>"
							+ scriptBlock + 
							"</pre>";
						*/
						sb.Append(scriptBlock + "\n");
				
					} else {
						// Output.Text += t.ToString() + "<br>";
					}
				}
			}
	
			return sb.ToString();
		}


	}
}

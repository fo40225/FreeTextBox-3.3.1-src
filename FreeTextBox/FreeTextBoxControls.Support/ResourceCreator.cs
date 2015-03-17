using System;
using System.IO;
using System.Reflection;
using System.Text;
namespace FreeTextBoxControls.Support
{
	public class ResourceCreator
	{
		public static void SaveToolbarScript(string filePath)
		{
			StreamWriter streamWriter = File.CreateText(filePath);
			streamWriter.Write(ResourceCreator.CreateToolbarItemScript());
			streamWriter.Close();
		}
		public static string CreateToolbarItemScript()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = Assembly.GetAssembly(typeof(FreeTextBox)).FullName;
			int num = text.IndexOf("Version=") + 8;
			int num2 = text.IndexOf(",", num);
			text = text.Substring(num, num2 - num);
			text = text.Replace(".5000", "");
			stringBuilder.Append("//** FreeTextBox Builtin ToolbarItems Script (" + text + ") ***/\n");
			stringBuilder.Append("//   by John Dyer\n");
			stringBuilder.Append("//   http://www.freetextbox.com/\n");
			stringBuilder.Append("//**********************************************/\n");
			stringBuilder.Append("\n");
			Assembly assembly = typeof(FreeTextBox).Module.Assembly;
			Type[] types = assembly.GetTypes();
			Type[] array = types;
			for (int i = 0; i < array.Length; i++)
			{
				Type type = array[i];
				if (type.IsClass && type.BaseType.BaseType == typeof(ToolbarItem))
				{
					object target = type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, new object[0]);
					string str = type.InvokeMember("BuiltInScript", BindingFlags.GetProperty, null, target, new object[0]).ToString();
					stringBuilder.Append(str + "\n");
				}
			}
			return stringBuilder.ToString();
		}
	}
}

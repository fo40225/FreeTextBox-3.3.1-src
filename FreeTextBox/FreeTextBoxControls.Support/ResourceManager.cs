using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
namespace FreeTextBoxControls.Support
{
	public class ResourceManager
	{
		private string language;
		private string path;
		private int resourceFailures;
		public string Language
		{
			get
			{
				return this.language;
			}
			set
			{
				this.language = value;
			}
		}
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.path = value;
			}
		}
		public ResourceManager() : this("en-US", "~/aspnet_client/FreeTextBox/Languages/")
		{
		}
		public ResourceManager(string language, string path)
		{
			this.language = language;
			this.path = path.Replace("/", "\\");
		}
		public NameValueCollection GetSupportedLanguages()
		{
			string name = "FreeTextBoxControls.Resources.Languages.Languages.xml";
			NameValueCollection nameValueCollection = new NameValueCollection();
			Stream manifestResourceStream = base.GetType().Assembly.GetManifestResourceStream(name);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(manifestResourceStream);
			foreach (XmlNode xmlNode in xmlDocument.SelectSingleNode("root").ChildNodes)
			{
				if (xmlNode.NodeType != XmlNodeType.Comment)
				{
					nameValueCollection.Add(xmlNode.Attributes["name"].Value, xmlNode.Attributes["key"].Value);
				}
			}
			return nameValueCollection;
		}
		public string GetString(string name)
		{
			Hashtable resources = this.GetResources();
			if (resources == null)
			{
				if (this.resourceFailures < 2)
				{
					return this.GetString(name);
				}
				throw new Exception("The resources have failed.");
			}
			else
			{
				if (resources[name] == null)
				{
					throw new Exception("Value not found in original en-US.xml for: " + name);
				}
				return (string)resources[name];
			}
		}
		private Hashtable GetResources()
		{
			string text = "en-US";
			string text2 = this.language;
			string text3 = "FreeTextBox-" + text + "-" + text2;
			if (text2 == "")
			{
				text2 = text;
			}
			if (HttpContext.Current.Cache[text3] == null)
			{
				Hashtable target = new Hashtable();
				this.LoadResource(target, text, text3);
				if (text != text2)
				{
					this.LoadResource(target, text2, text3);
				}
			}
			return (Hashtable)HttpContext.Current.Cache[text3];
		}
		private void LoadResource(Hashtable target, string language, string cacheKey)
		{
			XmlDocument xmlDocument = new XmlDocument();
			CacheDependency cacheDependency = null;
			string text = "FreeTextBoxControls.Resources.Languages." + language + ".xml";
			if (!(language.ToLower() != "en-us"))
			{
				if (target.Count <= 0)
				{
					goto IL_68;
				}
			}
			try
			{
				string filename = HttpContext.Current.Server.MapPath(this.path + language + ".xml");
				xmlDocument.Load(filename);
				cacheDependency = new CacheDependency(filename);
			}
			catch (Exception)
			{
			}
			IL_68:
			if (xmlDocument == null || xmlDocument.SelectSingleNode("root") == null)
			{
				Stream manifestResourceStream = base.GetType().Assembly.GetManifestResourceStream(text);
				if (manifestResourceStream != null)
				{
					xmlDocument.Load(manifestResourceStream);
				}
				manifestResourceStream.Close();
			}
			if (xmlDocument == null || xmlDocument.SelectSingleNode("root") == null)
			{
				return;
			}
			try
			{
				foreach (XmlNode xmlNode in xmlDocument.SelectSingleNode("root").ChildNodes)
				{
					if (xmlNode.NodeType != XmlNodeType.Comment)
					{
						if (target[xmlNode.Attributes["name"].Value] == null)
						{
							target.Add(xmlNode.Attributes["name"].Value, xmlNode.InnerText);
						}
						else
						{
							target[xmlNode.Attributes["name"].Value] = xmlNode.InnerText;
						}
					}
				}
			}
			catch
			{
				throw new Exception(string.Concat(new string[]
				{
					"FTB cannot load the resource: ",
					language,
					". File exists: ",
					this.path,
					language,
					".xml. Resource: ",
					text
				}));
			}
			if (cacheDependency != null)
			{
				HttpContext.Current.Cache.Insert(cacheKey, target, cacheDependency, DateTime.MaxValue, TimeSpan.Zero);
				return;
			}
			HttpContext.Current.Cache.Insert(cacheKey, target, null, DateTime.MaxValue, new TimeSpan(1, 0, 0, 0, 0));
		}
	}
}

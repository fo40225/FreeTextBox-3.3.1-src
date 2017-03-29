using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;

namespace FreeTextBoxControls.Support {

	/// <summary>
	/// Gets language specific information for <see cref="FreeTextBox"/> and its child controls.
	/// </summary>
	public class ResourceManager {
		private string language;
		private string path;
		/// <summary>
		/// Gets or sets the language used.
		/// </summary>
		public string Language {
			get {return language;}
			set {language = value;}
		}
		/// <summary>
		/// Gets or sets the path to FreeTextBox language files
		/// </summary>
		public string Path {
			get {return path;}
			set {path = value;}
		}

		public ResourceManager() : this("en-US", "~/aspnet_client/FreeTextBox/Languages/"){
		}

		public ResourceManager(string language, string path) {
			this.language = language;
			this.path = path.Replace("/","\\");
		}
		/// <summary>
		/// Gets all the supported languages files.
		/// </summary>
		public NameValueCollection GetSupportedLanguages() {
			string resourceName = "FreeTextBoxControls.Resources.Languages.Languages.xml";
			
			NameValueCollection supportedLanguages = new NameValueCollection();
			
			System.IO.Stream resourceStream = this.GetType().Assembly.GetManifestResourceStream(resourceName);
			XmlDocument d = new XmlDocument();
			d.Load( resourceStream );

			foreach (XmlNode n in d.SelectSingleNode("root").ChildNodes) {
				if (n.NodeType != XmlNodeType.Comment) {
					supportedLanguages.Add(n.Attributes["name"].Value, n.Attributes["key"].Value);
				}
			}

			return supportedLanguages;
		}
	
		private int resourceFailures = 0;

		/// <summary>
		/// Gets a string value from the XML source.
		/// </summary>
		public string GetString(string name) {

			Hashtable resources = GetResources();

			if (resources == null)  
			{				
				// call recursive
				if (resourceFailures < 2)
					return GetString(name);
				else
					throw new Exception("The resources have failed.");
			}
			if (resources[name] == null)
				throw new Exception("Value not found in original en-US.xml for: " + name);

			return (string) resources[name];
		}

		private Hashtable GetResources() {
			Hashtable resources;

			string defaultLanguage = "en-US";
			string setLanguage = language;
			string cacheKey = "FreeTextBox-" + defaultLanguage + "-" + setLanguage;

			// Ensure a language is set
			//
			if (setLanguage == "")
				setLanguage = defaultLanguage;

			// Attempt to get the resources from the Cache
			//
			if (HttpContext.Current.Cache[cacheKey] == null) {
				resources = new Hashtable();
			

				// First load the default language
				//
				LoadResource(resources, defaultLanguage, cacheKey);

				// If the set language is different load it
				//
				if (defaultLanguage != setLanguage)
					LoadResource(resources, setLanguage, cacheKey);
			}

			resources = (Hashtable) HttpContext.Current.Cache[cacheKey];
			
			return resources;
		}

		private void LoadResource (Hashtable target, string language, string cacheKey) {

			XmlDocument d = new XmlDocument();
				
			// (1) first check for a version on disk

			// this will be used if there is a disk version
			//
			CacheDependency dp = null;
			
			string resourceName = "FreeTextBoxControls.Resources.Languages." + language + ".xml";
			
		
			// make sure to load the base en-US from memory first to avoid conflicts with other versions
			// 
			if (language.ToLower() != "en-us" || target.Count > 0) {
				try {
                    string filePath = HttpContext.Current.Server.MapPath(path + language + ".xml");
					d.Load( filePath );
					dp = new CacheDependency(filePath);
				} catch (Exception e) {				
					// no error to throw -> default back to English resource
				}
			}

			// (2) try to load from Resources
			//
			if (d == null || d.SelectSingleNode("root") == null) {
				System.IO.Stream resourceStream = this.GetType().Assembly.GetManifestResourceStream(resourceName);
			
				if (resourceStream != null) 
					d.Load(resourceStream);
				
				resourceStream.Close();
			}
			
			// if neither a file or resource is found, then exit!
			//
			if (d == null || d.SelectSingleNode("root") == null) 
				return;
			
			try {
				foreach (XmlNode n in d.SelectSingleNode("root").ChildNodes) {
					if (n.NodeType != XmlNodeType.Comment) {
						if (target[n.Attributes["name"].Value] == null)
							target.Add(n.Attributes["name"].Value, n.InnerText);
						else
							target[n.Attributes["name"].Value] = n.InnerText;
					}
				} 
			} catch {
                throw new Exception("FTB cannot load the resource: " + language + ". File exists: " + path + language + ".xml" + ". Resource: " + resourceName);
			}

			// Create a new cache dependency and set it to never expire
			// unless the underlying file changes
			// 
			if (dp != null) 
				HttpContext.Current.Cache.Insert(cacheKey, target, dp, DateTime.MaxValue, TimeSpan.Zero);		
			
			// internal resource expires every 1 day
			//
			else 
				HttpContext.Current.Cache.Insert(cacheKey, target, null, DateTime.MaxValue, new TimeSpan(1,0,0,0,0));		
		}
	}
}

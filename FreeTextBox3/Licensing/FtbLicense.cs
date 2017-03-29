// ServerLicense.cs
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace FreeTextBoxControls.Licensing {
	/// <summary>
	/// Stores information found in the FreeTextBox.lic file
	/// </summary>
	/// <exclude />
	public class FtbLicense : License {

		private Type _type;
		private string _key;
		private bool _isPro;
		private string _data;

		public FtbLicense(Type type, string key, string data) {
			_type = type;
			_key = key;
			_data = data;
		}

		public FtbLicense(Type type, string key, string data, bool isPro) : this(type,key,data) {
			_isPro = isPro;
		}

		public override string LicenseKey {
			get {
				return _key;
			}
		}

		public string Data {
			get {
				return _data;
			}
		}

		public bool IsPro {
			get {
				return _isPro;
			}
		}

		public override void Dispose() {
		}
	}
}

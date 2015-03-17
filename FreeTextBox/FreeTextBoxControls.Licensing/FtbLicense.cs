using System;
using System.ComponentModel;
namespace FreeTextBoxControls.Licensing
{
	public class FtbLicense : License
	{
		private Type _type;
		private string _key;
		private bool _isPro;
		private string _data;
		public override string LicenseKey
		{
			get
			{
				return this._key;
			}
		}
		public string Data
		{
			get
			{
				return this._data;
			}
		}
		public bool IsPro
		{
			get
			{
				return this._isPro;
			}
		}
		public FtbLicense(Type type, string key, string data)
		{
			this._type = type;
			this._key = key;
			this._data = data;
		}
		public FtbLicense(Type type, string key, string data, bool isPro) : this(type, key, data)
		{
			this._isPro = isPro;
		}
		public override void Dispose()
		{
		}
	}
}

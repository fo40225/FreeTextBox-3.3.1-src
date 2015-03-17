using System;
using System.Web;
namespace FreeTextBoxControls
{
	public class AssemblyResourceHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
		public void ProcessRequest(HttpContext context)
		{
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace FreeTextBoxControls
{
    public class AssemblyResourceHandler : IHttpHandler
    {
        // empty class to ensure compatibility with web.config from ASP.NET 1.x application 
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // do nothing!
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsistentSystem
{
    public class GlobalAsax : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.SetData(
                "DataDirectory",
                Server.MapPath("~/App_Data"));
        }
    }
}
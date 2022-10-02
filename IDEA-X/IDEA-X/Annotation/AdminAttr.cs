using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDEA_X.Annotation
{
    public class AdminAttr:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UNAME"] != null && httpContext.Session["LEVEL"] != null)
            {
                if (httpContext.Session["LEVEL"].Equals("ADMIN"))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
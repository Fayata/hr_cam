using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace hr_cam
{
    /// <summary>
    /// Summary description for PersonImageHandler
    /// </summary>
    public class PersonImageHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                string fileName = context.Request.QueryString["fileName"];
                string filePath = context.Server.MapPath("~/person_image/" + fileName);

                if (System.IO.File.Exists(filePath))
                {
                    context.Response.ContentType = MimeMapping.GetMimeMapping(fileName);
                    context.Response.WriteFile(filePath);
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.StatusDescription = "File not found";
                }
            }
            else
            {
                context.Response.StatusCode = 403;
                context.Response.StatusDescription = "Unauthorized";
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }

}
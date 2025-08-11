using System.Web.Http;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using System.Web.Http.Cors;

[assembly: OwinStartup(typeof(hr_cam.Startup))]

namespace hr_cam
{
    public class Startup
    {
        //public void Configuration(IAppBuilder app)
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Mengaktifkan CORS
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Konfigurasi dan rute Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //app.UseCors(CorsOptions.AllowAll);
            //app.UseWebApi(config);
        }
    }
}

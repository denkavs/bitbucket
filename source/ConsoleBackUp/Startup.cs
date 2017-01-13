using ConsoleBackUp.Infrastructure;
using EFolderDomain.Infrastructure;
using EFolderDomain.Interfaces;
using Owin;
using System.Web.Http;

namespace ConsoleBackUp
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            NinjectResolver.ConfMgr = new ConfMgr();
            config.DependencyResolver = new NinjectResolver();

            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Insert(0, new CsvToDoFormatter());

            appBuilder.UseWebApi(config);
            Logger.Log.Info("Backup service initialized.");
        }
    }
}

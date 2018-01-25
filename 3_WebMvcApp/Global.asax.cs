using DataAccess;
using Ninject;
using Ninject.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebMvcApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Setup Ninject DI
            IKernel kernel = new StandardKernel();
            var dataSource = ConfigurationManager.AppSettings["DataSource"];
            if (dataSource == "SQL")
            {
                kernel.Bind<IDataAccess>().To<SqlDataAccess>();
            }
            else if (dataSource == "static")
            {
                kernel.Bind<IDataAccess>().To<StaticDataAccess>();
            }
            else
            {
                throw new InvalidOperationException("Data source must be 'SQL' or 'static'.");
            }
            // If you don't unbind this, it will cause errors in the model validation on the client.
            kernel.Unbind<ModelValidatorProvider>();

            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));


        }
    }
}

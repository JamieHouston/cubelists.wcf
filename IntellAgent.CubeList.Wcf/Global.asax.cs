using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;

namespace IntellAgent.CubeList.Wcf {
    public class Global : HttpApplication {
        void Application_Start(object sender, EventArgs e) {
            RegisterRoutes();
        }

        private void RegisterRoutes() {
            RouteTable.Routes.Add(new ServiceRoute("", new WebServiceHostFactory(), typeof(CubeService)));
            //RouteTable.Routes.Add(new ServiceRoute("Cubes", new WebServiceHostFactory(), typeof(CubeService)));
        }
    }
}

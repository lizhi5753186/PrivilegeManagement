using System.Web;
using System.Web.Optimization;

namespace LH.Web.API
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //类库依赖文件
            bundles.Add(new ScriptBundle("~/js/base/lib").Include(
                    "~/app/modules/jquery-1.11.2.min.js",
                    "~/app/modules/angular/angular.min.js",
                    "~/app/modules/angular/angular-route.min.js",
                    "~/app/modules/bootstrap/js/ui-bootstrap-tpls-0.13.0.min.js",
                    "~/app/modules/bootstrap-notify/bootstrap-notify.min.js"
                   ));
            //angularjs 项目文件
            bundles.Add(new ScriptBundle("~/js/angularjs/app").Include(
                    "~/app/scripts/services/*.js",
                    "~/app/scripts/controllers/*.js",
                    "~/app/scripts/directives/*.js",
                    "~/app/scripts/filters/*.js",
                    "~/app/scripts/app.js"));
            //样式
            bundles.Add(new StyleBundle("~/js/base/style").Include(
                    "~/app/modules/bootstrap/css/bootstrap.min.css",
                    "~/app/styles/dashboard.css",
                    "~/app/styles/console.css"
                    ));
        }
    }
}

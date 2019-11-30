using System.Web.Optimization;

namespace frontend_app
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // CSS
            bundles.Add(new StyleBundle("~/bundles/style").Include(
                        "~/Styles/authentication.css",
                        "~/Styles/alerts.css",
                        "~/Styles/menu.css",
                        "~/Styles/shared.css",
                        "~/Styles/modal-box.css"));

            // JS
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                       "~/Scripts/jquery-3.4.1.min.js",
                       "~/Scripts/alerts.js",
                       "~/Scripts/menu.js",
                       "~/Scripts/conversation.js"));
        }
    }
}
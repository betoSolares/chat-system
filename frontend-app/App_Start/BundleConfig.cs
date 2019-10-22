using System.Web.Optimization;

namespace frontend_app
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // CSS
            bundles.Add(new StyleBundle("~/bundles/style").Include(
                        "~/Styles/authentication.css"));
            // JS
        }
    }
}
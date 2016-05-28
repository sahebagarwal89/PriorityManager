using System.Web;
using System.Web.Optimization;

namespace PriorityManager
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Styles/themes/base/css").Include(
                        "~/Styles/themes/base/jquery.ui.core.css",
                        "~/Styles/themes/base/jquery.ui.resizable.css",
                        "~/Styles/themes/base/jquery.ui.selectable.css",
                        "~/Styles/themes/base/jquery.ui.accordion.css",
                        "~/Styles/themes/base/jquery.ui.autocomplete.css",
                        "~/Styles/themes/base/jquery.ui.button.css",
                        "~/Styles/themes/base/jquery.ui.dialog.css",
                        "~/Styles/themes/base/jquery.ui.slider.css",
                        "~/Styles/themes/base/jquery.ui.tabs.css",
                        "~/Styles/themes/base/jquery.ui.datepicker.css",
                        "~/Styles/themes/base/jquery.ui.progressbar.css",
                        "~/Styles/themes/base/jquery.ui.theme.css"));
        }
    }
}
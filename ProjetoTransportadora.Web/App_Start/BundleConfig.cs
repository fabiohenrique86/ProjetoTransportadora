﻿using System.Web.Optimization;

namespace ProjetoTransportadora.Web
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // JS
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-1.11.3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery.ui-1.11.4.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/globalize").Include("~/Scripts/jquery.globalize/globalize.js",
                                                                        "~/Scripts/jquery.globalize/cultures/globalize.cultures.js",
                                                                        "~/Scripts/jquery.globalize/cultures/globalize.culture.pt-BR.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap-3.3.6.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/mask").Include("~/Scripts/mask.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include("~/Scripts/jquery.dataTables-1.10.12.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/moment").Include("~/Scripts/moment.min_2.11.1.js"));
            bundles.Add(new ScriptBundle("~/bundles/blockUI").Include("~/Scripts/jquery.blockUI.js"));
            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include("~/Scripts/fullcalendar.min.js",
                                                                           "~/Scripts/fullcalendar-locale-all.js"));

            // CSS
            bundles.Add(new StyleBundle("~/bundles/css").Include("~/Content/bootstrap-3.3.6.min.css",
                                                                "~/Content/jquery.dataTables.css",
                                                                "~/Content/modern-business.css",
                                                                "~/Content/font-awesome.min.css",
                                                                "~/Content/fullcalendar.min.css"));

            // Jquery UI - images
            bundles.Add(new StyleBundle("~/bundles/jqueryuicss").Include("~/Content/jquery.ui-1.11.4.css", new CssRewriteUrlTransform()));
        }
    }
}

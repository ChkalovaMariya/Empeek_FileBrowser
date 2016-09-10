using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace FileBrowser_SPA
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
           
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/Site.css"));

            //bundles.Add(new ScriptBundle("~/bundles/AngularApp")
            //.Include("~/Scripts/AngularApp.js"));

            bundles.Add(new ScriptBundle("~/bundles/AngularApp")
            .IncludeDirectory("~/Scripts/Controllers", "*.js")
            .Include("~/Scripts/AngularApp.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}

using System;
using System.Web;
using System.Web.Optimization;

namespace DKC.JBus
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Bundle จะใช้ .min file ที่อยู่ใน path เดียวกัน
            // ดังนั้นถ้าใช้ CssRewriteUrlTransform โดยมี .min file อยู่ด้วย จะใช้ .min file แทน โดยไม่ทำ RewriteUrl
            // CssRewriteUrlTransform ทำงานเมื่อไม่มี .min file หรือเรียกใช้ .min file แทน
            bundles.Add(new StyleBundle("~/bundle/global-styles")
                .Include("~/assets/global/plugins/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/plugins/simple-line-icons/simple-line-icons.min.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/plugins/bootstrap/css/bootstrap.min.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/plugins/bootstrap-combobox/bootstrap-combobox.css")
                .Include("~/assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.css")
                .Include("~/assets/global/plugins/bootstrap-modal/css/bootstrap-modal-bs3patch.css")
                .Include("~/assets/global/plugins/bootstrap-modal/css/bootstrap-modal.css")
                .Include("~/assets/global/plugins/icheck/skins/minimal/_all.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/plugins/icheck/skins/square/_all.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundle/theme-styles")
                .Include("~/assets/global/css/font.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/css/components.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/css/plugins.css", new CssRewriteUrlTransform())
                .Include("~/assets/admin/layout/css/layout.css", new CssRewriteUrlTransform())
                .Include("~/assets/admin/layout/css/themes/light.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundle/custom-styles")
                .Include("~/assets/admin/pages/css/login3.css")
                .Include("~/assets/admin/layout/css/custom.css"));

            bundles.Add(new StyleBundle("~/bundle/home-styles")
                .Include("~/assets/home/css/reset.css")
                .Include("~/assets/global/plugins/simple-line-icons/simple-line-icons.min.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/css/font.css", new CssRewriteUrlTransform())
                .Include("~/assets/home/css/edata.css", new CssRewriteUrlTransform())
                .Include("~/assets/home/css/main.css", new CssRewriteUrlTransform())
                .Include("~/assets/home/css/styles.css", new CssRewriteUrlTransform())
                .Include("~/assets/home/css/edata-responsive.css"));

            bundles.Add(new ScriptBundle("~/bundle/global-scripts")
                .Include("~/assets/global/plugins/jquery.js")
                .Include("~/assets/global/plugins/jquery-migrate.js")
                .Include("~/assets/global/plugins/bootstrap/js/bootstrap.js")
                .Include("~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.js")
                .Include("~/assets/global/plugins/jquery.blockui.js")
                .Include("~/assets/global/plugins/bootstrap-combobox/bootstrap-combobox-custom.js")
                .Include("~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker-custom.js")
                .Include("~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.th.min.js")
                .Include("~/assets/global/plugins/bootstrap-modal/js/bootstrap-modalmanager.js")
                .Include("~/assets/global/plugins/bootstrap-modal/js/bootstrap-modal.js")
                .Include("~/assets/global/plugins/icheck/icheck.js")
                .Include("~/assets/global/plugins/jquery-validation/js/jquery.validate.js"));

            bundles.Add(new ScriptBundle("~/bundle/theme-scripts")
                .Include("~/assets/global/scripts/metronic.js")
                .Include("~/assets/admin/layout/scripts/layout.js"));

            bundles.Add(new ScriptBundle("~/bundle/home-scripts")
                .Include("~/assets/global/plugins/jquery.js")
                .Include("~/assets/global/plugins/jquery-migrate.js")
                .Include("~/assets/home/js/main.js")
                .Include("~/assets/home/js/plugins.js")
                .Include("~/assets/home/js/jquery.easing.1.3.js")
                .Include("~/assets/home/js/mediaelement-and-player.min.js")
                .Include("~/assets/home/js/global.js")
                .Include("~/assets/home/js/slide.js"));

            bundles.Add(new ScriptBundle("~/bundle/index-scripts")
                .Include("~/assets/home/js/index.js"));

            //bundles.Add(new ScriptBundle("~/bundle/academy-create-scripts")
            //    .Include("~/assets/agent/js/academy-create.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
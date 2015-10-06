using DKC.JBus.Constants;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace DKC.JBus
{
    internal class AsIsBundleOrderer : IBundleOrderer
    {
        public virtual IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }

    internal static class BundleExtensions
    {
        public static Bundle ForceOrdered(this Bundle sb)
        {
            sb.Orderer = new AsIsBundleOrderer();
            return sb;
        }
    }

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
#if !DEBUG
            BundleTable.EnableOptimizations = true;
            bundles.UseCdn = true;
#endif
            // Bundle จะใช้ .min file ที่อยู่ใน path เดียวกัน
            // ดังนั้นถ้าใช้ CssRewriteUrlTransform โดยมี .min file อยู่ด้วย จะใช้ .min file แทน โดยไม่ทำ RewriteUrl
            // CssRewriteUrlTransform ทำงานเมื่อไม่มี .min file หรือเรียกใช้ .min file แทน
            bundles.Add(new StyleBundle("~/bundle/global-styles")
                .Include("~/assets/global/plugins/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/plugins/simple-line-icons/simple-line-icons.min.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/plugins/bootstrap/css/bootstrap.min.css", new CssRewriteUrlTransform()).ForceOrdered());
            //.Include("~/assets/global/plugins/bootstrap-combobox/bootstrap-combobox.css")
            //.Include("~/assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.css")
            //.Include("~/assets/global/plugins/bootstrap-modal/css/bootstrap-modal-bs3patch.css")
            //.Include("~/assets/global/plugins/bootstrap-modal/css/bootstrap-modal.css")
            //.Include("~/assets/global/plugins/icheck/skins/minimal/_all.css", new CssRewriteUrlTransform())
            //.Include("~/assets/global/plugins/icheck/skins/square/_all.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundle/theme-styles")
                .Include("~/assets/global/css/components.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/css/plugins.css", new CssRewriteUrlTransform())
                .Include("~/assets/admin/layout3/css/layout.css", new CssRewriteUrlTransform())
                .Include("~/assets/admin/layout3/css/themes/red-intense.css", new CssRewriteUrlTransform()).ForceOrdered());

            bundles.Add(new StyleBundle("~/bundle/custom-styles")
                .Include("~/assets/app/css/custom.css").ForceOrdered());

            bundles.Add(new ScriptBundle("~/bundle/global-scripts")
                .Include("~/assets/global/plugins/jquery.min.js")
                .Include("~/assets/global/plugins/jquery-migrate.min.js")
                // <!-- IMPORTANT! Load jquery-ui.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->
                .Include("~/assets/global/plugins/jquery-ui/jquery-ui.min.js")
                .Include("~/assets/global/plugins/bootstrap/js/bootstrap.min.js")
                .Include("~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.js")
                .Include("~/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js")
                .Include("~/assets/global/plugins/jquery.blockui.min.js")
                .Include("~/assets/global/plugins/jquery.cokie.min.js")
                .Include("~/assets/global/plugins/jquery-validation/js/jquery.validate.js").ForceOrdered());

            //.Include("~/assets/global/plugins/bootstrap-combobox/bootstrap-combobox-custom.js")
            //.Include("~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker-custom.js")
            //.Include("~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.th.min.js")
            //.Include("~/assets/global/plugins/bootstrap-modal/js/bootstrap-modalmanager.js")
            //.Include("~/assets/global/plugins/bootstrap-modal/js/bootstrap-modal.js")
            //.Include("~/assets/global/plugins/icheck/icheck.js")

            bundles.Add(new ScriptBundle("~/bundle/theme-scripts")
                .Include("~/assets/global/scripts/metronic.js")
                .Include("~/assets/admin/layout3/scripts/layout.js").ForceOrdered());
        }
    }
}
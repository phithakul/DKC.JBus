using JBus.Web.Helpers;
using JBus.Web.Infrastructure;
using JBus.Web.Models;
using JBus.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JBus.Web.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class AppUserController : BaseController
    {
        public ActionResult Index(string searchString, string download)
        {
            ViewBag._CurrentFilter = searchString;
            var users = AppUser.GetList(false, searchString);
            if (download == null)
            {
                ViewBag._UserTypeItems = GetUserTypeItems();
                return View("Index", users);
            }
            else if (download == "xlsx")
            {
                var table = new System.Data.DataTable("model");
                table.Columns.Add("รหัสผู้ใช้", typeof(string));
                table.Columns.Add("ประเภทผู้ใช้", typeof(string));
                table.Columns.Add("อีเมล์", typeof(string));
                table.Columns.Add("สถานะ", typeof(string));
                table.Columns.Add("วันที่สร้าง", typeof(string));

                foreach (var item in users)
                {
                    table.Rows.Add(
                        item.Username,
                        item.UserType,
                        item.Email,
                        AppUtils.FormatReportDateTime(item.CreationDate)
                    );
                }
                ExcelHelper.DumpXlsx(Response, table, "User_" + DateTime.Now.ToString("dd-MM-yyyy", AppUtils.ThaiCulture), "User");
                return new EmptyResult();
            }
            else
            {
                return HttpNotFound();
            }
        }

        public IEnumerable<SelectListItem> GetUserTypeItems()
        {
            return new[] {
                new SelectListItem { Text = UserType.Admin.ToString(), Value = ((int)UserType.Admin).ToString(), Selected = false },
                new SelectListItem { Text = UserType.Manager.ToString(), Value = ((int)UserType.Manager).ToString(), Selected = false},
                new SelectListItem { Text = UserType.Staff.ToString(), Value = ((int)UserType.Staff).ToString(), Selected = false },
                new SelectListItem { Text = UserType.Customer.ToString(), Value = ((int)UserType.Customer).ToString(), Selected = false }
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(AppUserViewModel viewModel)
        {
            if (viewModel.IsValid(ModelState))
            {
                var model = AppUserViewModel.ToModel(viewModel);
                bool foundDup;
                AppUser.Create(model, out foundDup);
                if (!foundDup)
                {
                    return Json(new
                    {
                        success = true,
                        data = new
                        {
                            Id = model.Id.ToString(),
                            Username = model.Username,
                            UserType = model.UserType.ToString(),
                            Email = model.Email
                        },
                        url = Url.Action("Edit", new { id = model.Id })
                    });
                }
                else
                {
                    return Json(new { success = false, errors = new { Username = "รหัสผู้ใช้ซ้ำ กรุณากำหนดใหม่" } });
                }
            }

            return Json(new { success = false, errors = GetErrorsFromModelState() });
        }

        public ActionResult Edit(int id)
        {
            var model = Current.DB.Users.Get(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var viewModel = AppUserViewModel.FromModel(model);
            viewModel.UserTypeItems = GetUserTypeItems();
            return View("CreateEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(AppUserViewModel viewModel)
        {
            if (viewModel.IsValid(ModelState))
            {
                var model = AppUserViewModel.ToModel(viewModel);
                bool foundDup;
                AppUser.Update(model, out foundDup);
                if (!foundDup)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Username", "รหัสผู้ใช้ซ้ำ กรุณากำหนดใหม่");
                }
            }
            //var items = GetUserTypeItems();
            //viewModel.UserTypeItems = new SelectList(items, "Value", "Text");
            viewModel.UserTypeItems = GetUserTypeItems();
            return View("CreateEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string ids)
        {
            AppUser.Delete(ids);
            return Json(new { success = true });
        }
    }
}
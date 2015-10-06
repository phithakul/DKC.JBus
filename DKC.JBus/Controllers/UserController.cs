using DKC.JBus.Domains;
using DKC.JBus.Helpers;
using DKC.JBus.Infrastructure;
using DKC.JBus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DKC.JBus.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        public ActionResult Index(string searchString, string download)
        {
            ViewBag._CurrentFilter = searchString;
            var users = Domains.User.GetList(false, searchString);
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
        public ActionResult Create(UserViewModel viewModel)
        {
            if (viewModel.IsValid(ModelState))
            {
                var model = UserViewModel.ToModel(viewModel);
                bool foundDup;
                Domains.User.Create(model, out foundDup);
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
            var viewModel = UserViewModel.FromModel(model);
            viewModel.UserTypeItems = GetUserTypeItems();
            return View("CreateEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(UserViewModel viewModel)
        {
            if (viewModel.IsValid(ModelState))
            {
                var model = UserViewModel.ToModel(viewModel);
                bool foundDup;
                Domains.User.Update(model, out foundDup);
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
            Domains.User.Delete(ids);
            return Json(new { success = true });
        }
    }
}
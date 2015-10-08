using JBus.Web.Helpers;
using JBus.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JBus.Web.ViewModels
{
    public class AppUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string UserType { get; set; }

        public IEnumerable<SelectListItem> UserTypeItems { get; set; }

        public string Email { get; set; }

        public bool SetPassword { get; set; }

        public string Password1 { get; set; }

        public string Password2 { get; set; }

        public string Active { get; set; }

        public bool IsValid(ModelStateDictionary modelState)
        {
            ValidationHelper.IsValidTextField(modelState, "รหัสผู้ใช้", "Username", Username, 50, true);
            ValidationHelper.IsValidSelectField(modelState, "ประเภทผู้ใช้", "UserType", UserType, true);
            ValidationHelper.IsValidEmailField(modelState, "อีเมล์", "Email", Email, 100, false);
            if (SetPassword)
            {
                ValidationHelper.IsValidPasswordField(modelState, "รหัสผ่าน", "Password1", Password1, "Password2", Password2, 30, true);
            }

            return modelState.IsValid;
        }

        public static AppUser ToModel(AppUserViewModel viewModel)
        {
            var user = new AppUser()
            {
                Id = viewModel.Id.IsNullOrEmptyReturn<int>(),
                Username = viewModel.Username,
                UserType = (UserType)int.Parse(viewModel.UserType),
                Email = viewModel.Email ?? ""
            };
            return user;
        }

        public static AppUserViewModel FromModel(AppUser model)
        {
            return new AppUserViewModel()
            {
                Id = model.Id.ToString(),
                Username = model.Username,
                UserType = ((int)model.UserType).ToString(),
                Email = model.Email
            };
        }
    }
}
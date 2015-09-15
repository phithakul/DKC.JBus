﻿using DKC.JBus.Helpers;
using DKC.JBus.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DKC.JBus.ViewModels
{
    public class UserViewModel
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

        public static User ToModel(UserViewModel viewModel)
        {
            var user = new User()
            {
                Id = viewModel.Id.IsNullOrEmptyReturn<int>(),
                Username = viewModel.Username,
                UserType = (UserType)int.Parse(viewModel.UserType),
                Email = viewModel.Email ?? "",
                Active = viewModel.Active.IsNullOrEmptyReturn<bool>()
            };
            if (viewModel.SetPassword)
            {
                user.Password = viewModel.Password1;
            }
            return user;
        }

        public static UserViewModel FromModel(User model)
        {
            return new UserViewModel()
            {
                Id = model.Id.ToString(),
                Username = model.Username,
                UserType = ((int)model.UserType).ToString(),
                Email = model.Email,
                Active = model.Active.ToString()
            };
        }
    }
}
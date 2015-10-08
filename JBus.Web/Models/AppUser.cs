using Dapper;
using JBus.Web.Constants;
using JBus.Web.Helpers;
using JBus.Web.Helpers.Security;
using JBus.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace JBus.Web.Models
{
    public enum UserType
    {
        None = 0,
        Admin = 1,
        Manager = 2,
        Staff = 3,
        Customer = 4
    }

    public class AppUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserType UserType { get; set; }
        public string FullName { get; set; }
        public string MemberType { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastActivityDate { get; set; }

        public string UserInfo
        {
            get
            {
                return string.Format("{0}", FullName);
            }
        }

        public bool IsAdmin
        {
            get { return UserType == UserType.Admin; }
        }

        public bool IsCustomer
        {
            get { return UserType == UserType.Customer; }
        }

        public static AppUser CreateLoginUser(AppUser user)
        {
            user.UserType = UserType.Customer; // ใช้ insert อย่างเดียว
            user.CreationDate = DateTime.Now;
            user = Current.DB.Query<AppUser>(@"
begin tran
  update Users with (serializable)
  set
      FullName     = @FullName,
      MemberType   = @MemberType,
      Department   = @Department,
      Section      = @Section
   where Username = @Username
   if @@rowcount = 0
   begin
      insert Users (Username,UserType,FullName,MemberType,Department,Section,CreationDate)
      values (@Username,@UserType,@FullName,@MemberType,@Department,@Section,@CreationDate)
   end
   select * FROM Users WHERE Username = @Username
commit tran", user).Single();
            return user;
        }

        public static void Create(AppUser model, out bool foundDup)
        {
            foundDup = false;
            try
            {
                model.CreationDate = DateTime.Now;
                model.Id = (int)Current.DB.Users.Insert(
                    new
                    {
                        model.Username,
                        model.UserType,
                        model.Email,
                        model.CreationDate,
                    });
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627: // 2627 is unique constraint (includes primary key)
                    case 2601: // 2601 is unique index
                        foundDup = true;
                        break;

                    default:
                        throw;
                }
            }
        }

        public static void Update(AppUser model, out bool foundDup)
        {
            foundDup = false;
            var updateModel = Current.DB.Users.Get(model.Id);

            var snapshot = Snapshotter.Start(updateModel);
            updateModel.Id = model.Id;
            updateModel.Username = model.Username;
            updateModel.UserType = model.UserType;
            updateModel.Email = model.Email;

            var diff = snapshot.Diff();
            if (diff.ParameterNames.Any())
            {
                try
                {
                    Current.DB.Users.Update(updateModel.Id, diff);
                }
                catch (SqlException ex)
                {
                    switch (ex.Number)
                    {
                        case 2627: // 2627 is unique constraint (includes primary key)
                        case 2601: // 2601 is unique index
                            foundDup = true;
                            break;

                        default:
                            throw;
                    }
                }
            }
        }

        public static void Delete(string ids)
        {
            Current.DB.Execute("delete from Users where Id in (" + ids + ")");
        }

        public static IEnumerable<AppUser> GetList(bool? isAgent = null, string name = null)
        {
            return null;
        }

        public static void UpdateActivity(int id)
        {
            Current.DB.Execute("update Users set LastActivityDate=GETDATE() where Id=@id", new { id });
        }
    }
}
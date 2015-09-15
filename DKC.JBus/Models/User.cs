using Dapper;
using DKC.JBus.Constants;
using DKC.JBus.Helpers;
using DKC.JBus.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DKC.JBus.Models
{
    public enum UserType
    {
        None = 0,
        Admin = 1,
        Manager = 2,
        Officer = 3,
        Customer = 4
    }

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Hash { get; set; }

        public UserType UserType { get; set; }

        public string FullName { get; set; }
        public string MobileNo { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime LastActivityDate { get; set; }

        public bool IsAnonymous { get; set; }

        public string ActiveText
        {
            get
            {
                return Active ? "ใช้งาน" : "ไม่ใช้งาน";
            }
        }

        public string UserInfo
        {
            get
            {
                return string.Format("{0}", FullName);
            }
        }

        public string Password
        {
            set { Hash = Crypto.HashPassword(value); }
        }

        public bool IsAdmin
        {
            get { return UserType == UserType.Admin; }
        }

        public static void Create(User model, out bool foundDup)
        {
            foundDup = false;
            try
            {
                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                model.Id = (int)Current.DB.Users.Insert(
                    new
                    {
                        model.Username,
                        model.Hash,
                        model.UserType,
                        model.Email,
                        model.Active,
                        CreatedDate = model.CreatedDate,
                        UpdatedDate = model.UpdatedDate
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

        public static void Update(User model, out bool foundDup)
        {
            foundDup = false;
            var updateModel = Current.DB.Users.Get(model.Id);

            var snapshot = Snapshotter.Start(updateModel);
            updateModel.Id = model.Id;
            updateModel.Username = model.Username;
            updateModel.UserType = model.UserType;
            updateModel.Email = model.Email;
            updateModel.Active = model.Active;
            if (!model.Hash.IsNullOrEmpty())
            {
                updateModel.Hash = model.Hash;
            }

            var diff = snapshot.Diff();
            if (diff.ParameterNames.Any())
            {
                updateModel.UpdatedDate = DateTime.Now;
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

        public static User Authenticate(string username, string password, out string errorMsg)
        {
            errorMsg = "";
            return null;
        }

        public static IEnumerable<User> GetList(bool? isAgent = null, string name = null)
        {
            return null;
        }

        public static void UpdateActivity(int id)
        {
            Current.DB.Execute("update Users set LastActivityDate=GETDATE() where Id=@id", new { id });
        }
    }
}
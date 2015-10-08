using JBus.Web.Constants;
using JBus.Web.Helpers;
using JBus.Web.Helpers.Security;
using JBus.Web.Infrastructure;
using JBus.Web.Models;
using JBus.Web.ViewModels;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JBus.Web.Controllers
{
    public class AccountController : BaseController
    {
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")] // fix This error occurs when the user enter their credentials (username & password) and click login.
        // After the successfully authenticate the credentials, user will navigate to Home page of their portal. Here if user uses the browser back to move back to login page,
        //then re-enter their credentials again and clicking on the login button. This time user will see this error on their screens.
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            FormsAuthentication.SignOut();
            this.HttpContext.User = new GenericPrincipal(new GenericIdentity(""), null);
            return View("Login", new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }

            AppUser user = null;
            try
            {
#if !DEBUG
                if (!ActiveDirectory.AuthenticateUser(model.Username, model.Password))
                {
                    return ErrorLogin(model, "รหัสผู้ใช้หรือรหัสผ่านไม่ถูกต้อง");
                }
#endif
                user = HRService.GetUser(model.Username);
                if (user == null)
                {
                    return ErrorLogin(model, "ไม่พบข้อมูลรหัสผู้ใช้นี้ในฐานข้อมูลบุคคล");
                }
                user = AppUser.CreateLoginUser(user);

                CreateAuthenCookie(user);
            }
            catch (Exception ex)
            {
                // TODO:
                //Current.LogException(ex);
                return ErrorLogin(model, "Error: " + ex.Message);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ErrorLogin(LoginViewModel model, string message)
        {
            ModelState.AddModelError("Username", message);
            return View("Login", model);
        }

        private void CreateAuthenCookie(AppUser user)
        {
            DateTime ticketTimeout = DateTime.Now.AddMinutes(Application.TicketTimeout);

            bool isPersistent = false;
            var ticket = new FormsAuthenticationTicket(
                1,
                user.Id.ToString(),
                DateTime.Now,
                ticketTimeout, // timeout: This means that after a certain amount of time of inactivity, a user is prompted to login again.
                isPersistent,
                user.UserType.ToString());

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath,
                Domain = FormsAuthentication.CookieDomain
            };
            if (isPersistent)
            {
                authenticationCookie.Expires = ticket.Expiration; // If not set an expiry date it defaults to expire at the end of the session
            }
            Response.AppendCookie(authenticationCookie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
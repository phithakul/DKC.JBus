using DKC.JBus.Constants;
using DKC.JBus.Infrastructure;
using DKC.JBus.Models;
using DKC.JBus.ViewModels;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DKC.JBus.Controllers
{
    public class AccountController : BaseController
    {
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")] // fix This error occurs when the user enter their credentials (username & password) and click login.
        // After the successfully authenticate the credentials, user will navigate to Home page of their portal. Here if user uses the browser back to move back to login page,
        //then re-enter their credentials again and clicking on the login button. This time user will see this error on their screens.
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            FormsAuthentication.SignOut();
            this.HttpContext.User = new GenericPrincipal(new GenericIdentity(""), null);
            return View("Login", new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [HandleAntiForgeryError]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }

            string errorMsg;
            var user = Models.User.Authenticate(model.Username, model.Password, out errorMsg);
            if (user == null)
            {
                if (errorMsg != "")
                {
                    ModelState.AddModelError("Username", errorMsg);
                }
                else
                {
                    ModelState.AddModelError("Username", "รหัสผู้ใช้หรือรหัสผ่านไม่ถูกต้อง!");
                }
                return View("Login", model);
            }

            CreateAuthenCookie(user);

            switch (user.UserType)
            {
                case UserType.Customer:
                    return RedirectToAction("Index", "");

                case UserType.Manager:
                case UserType.Officer:
                    return RedirectToAction("Index", "");

                case UserType.Admin:
                    return RedirectToAction("Index", "");

                default:
                    return HttpNotFound();
            }
        }

        private void CreateAuthenCookie(User user)
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
        [HandleAntiForgeryError]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
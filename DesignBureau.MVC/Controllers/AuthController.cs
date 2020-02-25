using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DesignBureau.BLL.Managers;
using DesignBureau.Entities.Enums;
using DesignBureau.MVC.Models;

namespace DesignBureau.MVC.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Login(string email, string password)
        {
            if (UserSession.Current != null)
            {
                return Json(new { status = 0, data = Url.Action("Index", "Home") }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            var manager = new UserManager();
            var login = manager.Login(email, password);
            if (login == null)
            {
                return Json(new { status = 1, message = "Invalid name or password" }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }

            switch (login.Status)
            {
                case WebLoginStatuses.Success:
                    UserSession.Current = UserSession.CreateUserSession(email, password);
                    return Json(new { status = 0, url = Url.Action("Index", "Home") }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { status = 1, message = "Invalid name or password" }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Logout()
        {
            UserSession.DestroySession();
            return RedirectToAction("Login", "Auth");
        }
    }
}
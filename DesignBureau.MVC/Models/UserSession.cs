using DesignBureau.BLL.Managers;
using DesignBureau.Entities.Entity;
using DesignBureau.Entities.Enums;
using DesignBureau.Entities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DesignBureau.DAL.Services;

namespace DesignBureau.MVC.Models
{
    public class UserSession
    {
        public User User { get; set; }
        public DateTime LastRequestTime { get; set; }
        public DateTime CreatedDate;
        public bool IsAdmin => User.Role == UserRoles.Administrator;
        public bool IsApprove => HasApprovePermissions();
        public bool IsLayout => HasLayoutModePermissions();

        public const string SESSION_KEY = "AdInvoiceReader_UserSession";

        public static UserSession Current
        {
            get => HttpContext.Current?.Session?[SESSION_KEY] as UserSession;
            set => HttpContext.Current.Session[SESSION_KEY] = value;
        }

        public static UserSession CreateUserSession(string email, string password)
        {
            User user;
            using (var dal = new UserDalService())
            {
                user = dal.GetWebUserByEmail(email.ToUpper()) ?? dal.GetWebUserByUsername(email);
            }
            if (user == null || user.IsLocked || (user.Password != CryptHelper.Instance.EncryptToString(password)))
            {
                return null;
            }

            return new UserSession()
            {
                CreatedDate = DateTime.UtcNow,
                LastRequestTime = DateTime.UtcNow,
                User = user
            };
        }
        public static UserSession CreateAdminSession(string userName, string password)
        {
            return new UserSession
            {
                CreatedDate = DateTime.UtcNow,
                LastRequestTime = DateTime.UtcNow,
                User = new User { Email = userName, RoleCode = (int)UserRoles.Administrator }
            };
        }
        public static string SessionID => HttpContext.Current != null && HttpContext.Current.Session != null ? HttpContext.Current.Session.SessionID : string.Empty;

        public static string ClientIP => HttpContext.Current.Request.UserHostAddress;


        public bool HasApprovePermissions()
        {
            return User.Permissions == 3 || User.Permissions == 1;
        }
        public bool HasLayoutModePermissions()
        {
            return User.Permissions == 3 || User.Permissions == 2;
        }

        public static bool DestroySession()
        {
            if (Current == null)
            {
                return false;
            }

            new UserManager().LogOut(Current.User);
            HttpContext.Current.Session.Remove(SESSION_KEY);
            HttpContext.Current.Session.Remove("AdAdminDataStorage");
            return true;
        }
    }
}
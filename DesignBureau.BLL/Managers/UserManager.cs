using DesignBureau.DAL.Services;
using DesignBureau.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DesignBureau.Entities.Entity.BaseEntities;
using DesignBureau.Entities.Enums;
using DesignBureau.Entities.Helpers;

namespace DesignBureau.BLL.Managers
{
    public class UserManager : IDisposable
    {
        private readonly UserDalService _service = new UserDalService();
        public User GetUser(int userId)
        {
            var user = _service.GetWebUserById(userId);
            user.PermissionsList = new List<int>();
            for (var i = 0; i < 32; i++)
            {
                if ((user.Permissions & 1 << i) > 0)
                {
                    user.PermissionsList.Add(1 << i);
                }
            }
            return user;
        }

        public User SaveUser(User user)
        {
            user.Permissions = user.PermissionsList != null && user.PermissionsList.Any() ? user.PermissionsList.Sum() : 0;
            return user.UserId == 0 ? CreateWebUser(user) : Update(user);
        }

        public User CreateWebUser(User user)
        {
            var confirmCode = Guid.NewGuid().ToString();
            user.CreateDate = DateTime.Now;
            user.ConfirmCode = confirmCode;
            user = _service.CreateWebUser(user);
            return user;
        }

        public User Update(User user)
        {
            var result = _service.UpdateUser(user);
            return result;
        }

        public List<User> GetUsers() => _service.GetWebUsers();

        public List<BaseEntity> GetUserRoles()
        {
            var userRoles = new List<BaseEntity>();
            foreach (int role in Enum.GetValues(typeof(UserRoles)))
            {
                var entity = new BaseEntity();
                switch (role)
                {
                    case 1:
                        entity.Name = "User";
                        entity.Id = role;
                        userRoles.Add(entity);
                        break;
                    case 2:
                        entity.Name = "Administrator";
                        entity.Id = role;
                        userRoles.Add(entity);
                        break;
                }
            }
            return userRoles;
        }

        public List<BaseEntity> GetUserPermissions()
        {
            var userPermissions = new List<BaseEntity>();
            foreach (int permission in Enum.GetValues(typeof(Permissions)))
            {
                var entity = new BaseEntity();
                switch (permission)
                {
                    case 1:
                        entity.Name = "Designer";
                        entity.Id = permission;
                        userPermissions.Add(entity);
                        break;
                    case 2:
                        entity.Name = "Customer";
                        entity.Id = permission;
                        userPermissions.Add(entity);
                        break;
                    case 4:
                        entity.Name = "ChiefDesigner";
                        entity.Id = permission;
                        userPermissions.Add(entity);
                        break;
                }
            }

            return userPermissions;
        }

        public bool IsValidEmail(string email)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.Match(email).Success;
        }

        public Login Login(string mail, string password, int userId = 0)
        {
            if (mail == null || password == null)
            {
                return new Login(WebLoginStatuses.InvalidNamePass, "Invalid name or password");
            }

            return IsValidEmail(mail) ? LoginByEmail(mail, password) : null;
        }

        public Login LoginByEmail(string email, string password)
        {
            var user = _service.GetWebUserByEmail(email);
            if (user == null)
            {
                return new Login(WebLoginStatuses.NotExistUser, "User not exists");
            }
            if (user.Password != CryptHelper.Instance.EncryptToString(password))
            {
                return new Login(WebLoginStatuses.InvalidNamePass, "Invalid email or password");
            }
            return new Login(user);
        }

        public void LogOut(User user) { }

        public void Dispose() { }
    }
}

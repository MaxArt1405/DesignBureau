using DesignBureau.DAL.Services;
using DesignBureau.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesignBureau.BLL.Managers
{
    public class UserManager
    {
        public User GetUser(int userId)
        {
            var _service = new BaseDalService<User>();
            var user = _service.GetItem(userId);
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
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using DesignBureau.Entities.Enums;

namespace DesignBureau.Entities.Entity
{
    public class Login
    {
        public Login(WebLoginStatuses status, string message)
        {
            Status = status;
            Message = message;
        }

        public Login(User user) : this(WebLoginStatuses.Success, "Successful login")
        {
            User = user;
        }
        public WebLoginStatuses Status { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
    }
}

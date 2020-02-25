using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesignBureau.DAL.Repositories;
using DesignBureau.Entities.Entity;
using DesignBureau.Entities.Entity.BaseEntities;
using DesignBureau.Entities.Enums;

namespace DesignBureau.DAL.Services
{
    public class UserDalService : IDisposable
    {
        private readonly BaseDbRepository<User> _repository = new BaseDbRepository<User>();

        public User CreateWebUser(User user)
        {
            return !ExistWebUser(user.Id) ? _repository.Add(user) : new User();
        }

        public User UpdateUser(User user)
        {
            return _repository.Update(user);
        }

        public bool DropWebUser(User user)
        {
            return _repository.Delete(_repository.GetItem(user.Id));
        }

        public List<User> GetWebUsers()
        {
            return _repository.GetAll().ToList();
        }

        public List<User> GetWebUsers(SelectionInfo selection)
        {
            return _repository.GetAll(selection).ToList();
        }

        public User GetWebUserByEmail(string email)
        {
            var users = _repository.GetAll().Where(x => x.Email == email).ToList();
            return users.Any() ? users.First() : null;
        }

        public User GetWebUserById(int userId)
        {
            var filter = new SelectionInfo
            {
                IntOptions = new List<KeyValuePair<int, int>>()
                {
                    new KeyValuePair<int, int>((int)SqlFields.Code, userId),
                },
                CaseIgnore = true
            };
            return _repository.GetAll(filter).FirstOrDefault();
        }

        public User GetWebUserByUsername(string userName)
        {
            var filter = new SelectionInfo
            {
                StringOptions = new List<KeyValuePair<int, string>>()
                {
                    new KeyValuePair<int, string>((int)SqlFields.UserName, userName),
                },
                CaseIgnore = true
            };
            return _repository.GetAll(filter).FirstOrDefault();
        }

        public bool ExistWebUser(string email)
        {
            var selection = new SelectionInfo
            {
                StringOptions = new List<KeyValuePair<int, string>>()
                {
                    new KeyValuePair<int, string>((int)SqlFields.EMail, email.ToUpper())
                },
                CaseIgnore = true
            };
            var result = _repository.GetAll(selection);
            return result.Any();
        }
        public bool ExistWebUser(int code)
        {
            var selection = new SelectionInfo
            {
                IntOptions = new List<KeyValuePair<int, int>>()
                {
                    new KeyValuePair<int, int>((int)SqlFields.Code, code)
                },
                CaseIgnore = true
            };
            var result = _repository.GetAll(selection);
            return result.Any();
        }
        public void Dispose() { }
    }
}

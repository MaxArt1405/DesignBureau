using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DesignBureau.Entities.Attributes;
using DesignBureau.Entities.Entity.BaseEntities;
using DesignBureau.Entities.Enums;
using Newtonsoft.Json;

namespace DesignBureau.Entities.Entity
{
    [Table("Users")]
    public class User : SqlEntity
    {
        public override SqlFields ObjectField => SqlFields.User;
        public override SqlFields CodeField => SqlFields.User;

        public override int Id
        {
            get => UserId;
            set => UserId = value;
        }

        [SqlColumn("USERID", IsPrimaryKey = true, Type = SqlColumnTypes.Integer, SqlField = SqlFields.Code, SeqName = "recid_seq")]
        public int UserId { get; set; }

        [SqlColumn("USERNAME", Type = SqlColumnTypes.String, DataLength = 75, SqlField = SqlFields.UserName)]
        public string UserName { get; set; }

        [SqlColumn("FIRSTNAME", Type = SqlColumnTypes.String, DataLength = 75, SqlField = SqlFields.FirstName)]
        public string FirstName { get; set; }

        [SqlColumn("LASTNAME", Type = SqlColumnTypes.String, DataLength = 75, SqlField = SqlFields.LastName)]
        public string LastName { get; set; }

        [SqlColumn("ROLECODE", Type = SqlColumnTypes.Integer, DataLength = 75, SqlField = SqlFields.Role)]
        public int RoleCode { get; set; }

        [SqlColumn("EMAIL", Type = SqlColumnTypes.String, DataLength = 75, SqlField = SqlFields.EMail)]
        public string Email { get; set; }

        [JsonIgnore]
        [SqlColumn("PASSWORD", Type = SqlColumnTypes.String, DataLength = 100, SqlField = SqlFields.Password)]
        public string Password { get; set; }

        [SqlColumn("CREATEDATE", Type = SqlColumnTypes.Date, SqlField = SqlFields.CreateDate)]
        public DateTime? CreateDate { get; set; }

        [JsonIgnore]
        [SqlColumn("CONFIRMATIONCODE", Type = SqlColumnTypes.String, DataLength = 75, SqlField = SqlFields.ConfirmationCode)]
        public string ConfirmCode { get; set; }

        [SqlColumn("ISLOCKED", Type = SqlColumnTypes.Integer, SqlField = SqlFields.IsLocked)]
        public int Locked { get; set; }

        [SqlColumn("PERMISSIONS", Type = SqlColumnTypes.Integer, SqlField = SqlFields.Permissions)]
        public int Permissions { get; set; }

        public bool IsConfirmed => ConfirmCode == "";

        public List<int> PermissionsList { get; set; }

        public bool IsLocked => Locked != 0;

        public string CreateDateStr => CreateDate?.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

        public UserRoles Role => (UserRoles)RoleCode;

    }
}

using DesignBureau.Entities.Attributes;
using DesignBureau.Entities.Entity.BaseEntities;
using DesignBureau.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesignBureau.Entities.Entity
{
    public class Customer : SqlEntity
    {
        public override SqlFields ObjectField => SqlFields.Customer;
        public override SqlFields CodeField => SqlFields.Customer;
        public override int Id
        {
            get => CustomerId;
            set => CustomerId = value;
        }

        [SqlColumn("ID", IsPrimaryKey = true, Type = SqlColumnTypes.Integer, SqlField = SqlFields.Code)]
        public int CustomerId { get; set; }

        [SqlColumn("NAME", Type = SqlColumnTypes.String, SqlField = SqlFields.CustomerName)]
        public string CustomerName{get;set;}

        [SqlColumn("DESC", Type = SqlColumnTypes.String, SqlField = SqlFields.Description)]
        public string Description { get; set; }
    }
}

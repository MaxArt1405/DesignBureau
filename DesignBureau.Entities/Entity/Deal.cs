﻿using DesignBureau.Entities.Attributes;
using DesignBureau.Entities.Entity.BaseEntities;
using DesignBureau.Entities.Enums;

namespace DesignBureau.Entities.Entity
{
    [Table("Deal")]
    public class Deal : SqlEntity
    {
        public override int Id 
        {
            get => DealId;
            set => DealId = value; 
        }

        [SqlColumn("DEALID", IsPrimaryKey = true, Type = SqlColumnTypes.Integer, SqlField = SqlFields.Code, SeqName = "recid_seq")]
        public int DealId { get; set; }

        [SqlColumn("DEALNAME",  Type = SqlColumnTypes.String, SqlField = SqlFields.DealName)]
        public string DealName { get; set; }

        [SqlColumn("OWNER", Type = SqlColumnTypes.String, SqlField = SqlFields.Owner)]
        public string Owner { get; set; }

        [SqlColumn("CUSTOMER", Type = SqlColumnTypes.String, SqlField = SqlFields.Customer)]
        public string Customer { get; set; }

        [SqlColumn("PRICE", Type = SqlColumnTypes.String, SqlField = SqlFields.Price)]
        public int Price { get; set; }
    }
}

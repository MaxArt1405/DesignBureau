using DesignBureau.Entities.Attributes;
using DesignBureau.Entities.Entity.BaseEntities;
using DesignBureau.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesignBureau.Entities.Entity
{
    [Table("Deals")]
    public class Deal : SqlEntity
    {
        public override int Id 
        {
            get => DealId;
            set => DealId = value; 
        }

        [SqlColumn("DEALID", IsPrimaryKey = true, Type = SqlColumnTypes.Integer, SqlField = SqlFields.Code, SeqName = "recid_seq")]
        public int DealId { get; set; }

        [SqlColumn("DEALID",  Type = SqlColumnTypes.String, SqlField = SqlFields.DealName)]
        public string DealName { get; set; }
        [SqlColumn("OWNER", Type = SqlColumnTypes.String, SqlField = SqlFields.Owner)]
        public string Owner { get; set; }



    }
}

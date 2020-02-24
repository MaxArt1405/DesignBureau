using System;
using System.Collections.Generic;
using System.Text;
using DesignBureau.Entities.Enums;

namespace DesignBureau.Entities.Entity.BaseEntities
{
    public class SelectionInfo
    {
        public SelectionInfo()
        {
            FieldId = SqlFields.None;
            Codes = new List<int>();
            IntOptions = new List<KeyValuePair<int, int>>();
            StringOptions = new List<KeyValuePair<int, string>>();
        }
        public SqlFields FieldId { get; set; }
        public List<int> Codes { get; set; }
        public int DateFrom { get; set; }
        public int DateTo { get; set; }
        public List<KeyValuePair<int, int>> IntOptions { get; set; }
        public List<KeyValuePair<int, string>> StringOptions { get; set; }
        public bool CaseIgnore { get; set; }
    }
}

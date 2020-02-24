using System;
using System.Collections.Generic;
using System.Text;

namespace DesignBureau.Entities.Attributes
{
    public class TableAttribute : Attribute
    {
        public TableAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}

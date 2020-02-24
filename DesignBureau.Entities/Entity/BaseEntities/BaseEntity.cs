using DesignBureau.Entities.Interfaces;
using Newtonsoft.Json;

namespace DesignBureau.Entities.Entity.BaseEntities
{
    public class BaseEntity : IBaseEntity
    {
        [JsonProperty(PropertyName = "NAME")]
        public virtual string Name { get; set; }

        [JsonProperty(PropertyName = "ID")]
        public virtual int Id { get; set; }
    }
}

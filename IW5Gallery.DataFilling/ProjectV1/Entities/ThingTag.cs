using IW5Gallery.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Gallery.DAL.Entities
{
    public class ThingTag : TaggableBase
    {
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}

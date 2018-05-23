using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Gallery.BL.Models
{
    public class ThingDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<MiniatureModel> Images { get; set; } = new List<MiniatureModel>();
    }
}

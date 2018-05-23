using IW5Gallery.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Gallery.DAL.Entities
{
    public class Album : EntityBase
    {
        [Required]
        public string Name { get; set; }
        public Guid CoverPhotoId { get; set; }
        public virtual Image CoverPhoto { get; set; }

        public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    }
}

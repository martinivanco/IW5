using IW5Gallery.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Gallery.DAL.Entities
{
    public class Tag : EntityBase
    {
        [Required]
        public Guid TaggableId { get; set; }
        public virtual ITaggable Taggable { get; set; }
        [Required]
        public Guid LocationId { get; set; }
        public virtual Location Location { get; set; }
        [Required]
        public Guid ImageId { get; set; }
        public virtual Image Image { get; set; }
    }
}

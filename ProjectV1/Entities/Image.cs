using IW5Gallery.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Gallery.DAL.Entities
{
    
    public class Image : EntityBase
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DateTaken { get; set; }
        [Required]
        public DateTime DateAdded { get; set; }
        public Format Format { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Note { get; set; }
        [Required]
        public string Path { get; set; }
        public string ThumbnailPath { get; set; }
        public virtual ICollection<AlbumImage> Albums { get; set; } = new List<AlbumImage>();
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}

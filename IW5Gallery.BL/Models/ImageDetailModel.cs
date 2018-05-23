using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Gallery.BL.Models
{
    using IW5Gallery.DAL.Entities;

    public class ImageDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTaken { get; set; }
        public DateTime DateAdded { get; set; }
        public Format Format { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Note { get; set; }
        public string Path { get; set; }

        public virtual ICollection<MiniatureModel> Albums { get; set; }
        public virtual ICollection<TagModel> Tags { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.DAL.Entities;
using IW5Gallery.DAL.Entities.Base;

namespace IW5Gallery.BL.Models
{
    public class TagModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public Guid TaggableId { get; set; }
        public TagType TagType { get; set; }
    }
}

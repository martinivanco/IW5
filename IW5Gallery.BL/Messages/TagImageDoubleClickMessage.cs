using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.BL.Messages
{
    public class TagImageDoubleClickMessage
    {
        public Guid ImageId { get; set; }
        public Guid TagId { get; set; }
        public TagType TagType { get; set; }
    }
}

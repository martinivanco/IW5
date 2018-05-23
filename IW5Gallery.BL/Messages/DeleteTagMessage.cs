using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.BL.Messages
{
    public class DeleteTagMessage
    {
        public Guid Id { get; set; }
        public TagType Type { get; set; }
    }
}

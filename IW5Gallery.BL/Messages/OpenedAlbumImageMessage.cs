using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Gallery.BL.Messages
{
    public class OpenedAlbumImageMessage
    {
        public Guid ImageId { get; set; }
        public Guid AlbumId { get; set; }
    }
}

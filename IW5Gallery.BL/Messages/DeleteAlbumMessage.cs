using System;

namespace IW5Gallery.BL.Messages
{
    public class DeleteAlbumMessage
    {
        public DeleteAlbumMessage(Guid albumId)
        {
            AlbumId = albumId;
        }

        public Guid AlbumId { get; set; }
    }
}

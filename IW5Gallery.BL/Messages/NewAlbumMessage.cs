namespace IW5Gallery.BL.Messages
{
    public class NewAlbumMessage
    {
        public NewAlbumMessage(string albumName)
        {
            AlbumName = albumName;
        }

        public string AlbumName { get; set; }
    }
}
using System;

namespace IW5Gallery.BL.Messages
{
    public class NewImageAddedMessage
    {
        public Guid Id { get; set; }

        public NewImageAddedMessage(Guid id)
        {
            Id = id;
        }
    }
}
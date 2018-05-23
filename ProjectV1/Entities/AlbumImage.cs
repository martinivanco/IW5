using System;
using System.ComponentModel.DataAnnotations;
using IW5Gallery.DAL.Entities.Base;

namespace IW5Gallery.DAL.Entities
{
    public class AlbumImage : EntityBase
    {
        [Required]
        public Guid AlbumId { get; set; }
        public virtual Album Album{ get; set; }

        [Required]
        public Guid ImageId { get; set; }
        public virtual Image Image { get; set; }
    }
}
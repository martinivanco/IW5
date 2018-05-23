using IW5Gallery.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Gallery.DAL.Entities
{
    public class Location : EntityBase
    {
        [Required]
        public int XCoordinate { get; set; }
        [Required]
        public int YCoordinate { get; set; }
        [Required]
        public int Width { get; set; }
        [Required]
        public int Height { get; set; }
    }
}

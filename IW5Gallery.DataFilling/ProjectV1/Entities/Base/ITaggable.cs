using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Gallery.DAL.Entities.Base
{
    public interface ITaggable : IEntity
    {
        string Name { get; set; }
    }
}

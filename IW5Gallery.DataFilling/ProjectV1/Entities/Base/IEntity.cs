﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Gallery.DAL.Entities.Base
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}

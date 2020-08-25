using System;
using System.Collections.Generic;
using System.Text;

namespace CarMD.Fleet.Data.EntityFramework
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}

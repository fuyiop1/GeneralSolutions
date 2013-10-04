using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.MyDbContext.Entities
{
    public abstract class SortableEntity
    {
        public virtual int ID { get; set; }
        public virtual DateTime TimeForSort { get; set; }
    }
}
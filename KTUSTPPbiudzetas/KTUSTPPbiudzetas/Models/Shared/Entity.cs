using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Models.Shared
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

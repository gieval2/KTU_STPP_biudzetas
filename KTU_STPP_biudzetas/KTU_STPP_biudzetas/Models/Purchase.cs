using KTUSTPPBiudzetas.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Models
{
    public class Purchase : Entity
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public int? CheckId { get; set; }
        [NotMapped]
        public Check Check { get; set; }
    }
}
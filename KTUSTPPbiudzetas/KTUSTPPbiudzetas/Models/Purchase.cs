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
        [Required]
        public string Name { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public double Price { get; set; }
        public int? CheckId { get; set; }
        [NotMapped]
        public Check Check { get; set; }
    }
}
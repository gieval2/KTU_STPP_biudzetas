using KTUSTPPBiudzetas.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Models
{
    public class Check : Entity
    {
        [Required]
        public DateTime Date { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public int? MemberId { get; set; }
        [NotMapped]
        public Member Member { get; set; }
    }
}

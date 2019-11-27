﻿using KTU_STPP_biudzetas.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Models
{
    public class Check : Entity
    {
        [Required]
        public DateTime Date { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public int? UserId { get; set; }
        [NotMapped]
        public Member User { get; set; }
    }
}

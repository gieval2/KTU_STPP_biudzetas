using KTUSTPPBiudzetas.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Models
{
    public class Message : Entity
    {
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        public string Text { get; set; }

        [ForeignKey("Sender")]
        public int? SenderId { get; set; }
        [NotMapped]
        public Member Sender { get; set; }

        [ForeignKey("Reciever")]
        public int? RecieverId { get; set; }
        [NotMapped]
        public Member Reciever { get; set; }
    }
}

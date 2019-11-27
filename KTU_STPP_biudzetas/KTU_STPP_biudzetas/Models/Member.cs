using KTU_STPP_biudzetas.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Models
{
    public class Member : Entity
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Family { get; set; }
        public int FamilyLevel { get; set; }
        public string Token { get; set; }
        public double Limit { get; set; }
        public int LimitState { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Check> Checks { get; set; }

        [InverseProperty("Sender")]
        public ICollection<Message> Sent { get; set; }

        [InverseProperty("Reciever")]
        public ICollection<Message> Recieved { get; set; }
    }
}

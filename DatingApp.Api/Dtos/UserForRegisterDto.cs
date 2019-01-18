using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters")]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnowkAs { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Counrty { get; set; }
        [Required]
        public DateTime LastActive { get; set; } = DateTime.Now;
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
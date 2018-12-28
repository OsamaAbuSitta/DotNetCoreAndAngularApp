using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Dtos.Tofix
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(30,MinimumLength = 6,ErrorMessage="You must spacify password between 6 and 30 charachters ")]
        public string Password { get; set; }
    }
}
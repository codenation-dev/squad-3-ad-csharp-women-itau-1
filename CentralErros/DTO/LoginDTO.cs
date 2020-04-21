using System;
using System.ComponentModel.DataAnnotations;


namespace CentralErros.DTO
{
    public class LoginDTO
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


    }
}

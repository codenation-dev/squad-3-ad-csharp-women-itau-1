using System;
using System.ComponentModel.DataAnnotations;


namespace CentralErros.DTO
{
    public class UserDTO
    {
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CentralErros.DTO
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string UserId { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Code { get; set; }
    }
}

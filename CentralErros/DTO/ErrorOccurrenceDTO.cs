using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CentralErros.DTO
{
    public class ErrorOccurrenceDTO
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public bool Filed { get; set; }

        [Required]
        public string Details { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int EnvironmentId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int LevelId  { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CentralErros.DTO
{
    public class ErrorOccurrenceDTO
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Details { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int EnvironmentId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int LevelId { get; set; }
        
        public string Origin { get; set; }
    }

    public class ErrorDetails
    {
        public string Title { get; set; }

        public string Details { get; set; }

        public string Origin { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string LevelName { get; set; }

        public int EventId { get; set; }

    }
  
}

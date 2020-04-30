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
        public string Origin { get; set; }

        [Required]
        public bool Filed { get; set; }

        [Required]
        public string Details { get; set; }

        [Required]
        public string IPError { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int EnvironmentId { get; set; }

        [Required]
        public int LevelId  { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;


namespace CentralErros.DTO
{
    public class EnvironmentDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}

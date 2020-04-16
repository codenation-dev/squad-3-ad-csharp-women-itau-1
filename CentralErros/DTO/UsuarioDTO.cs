using System.ComponentModel.DataAnnotations;


namespace CentralErros.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}

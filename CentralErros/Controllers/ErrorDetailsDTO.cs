using System;
namespace CentralErros.Controllers
{
    public class ErrorDetailsDTO
    {
        public string UserToken { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public string Origin { get; set; }

        public int IdEvent { get; set; }

        public string LevelName { get; set; }

        public DateTime RegistrationDate { get; set; }

    }
}

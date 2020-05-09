using System;


namespace CentralErros.DTO
{
    public class ErrorDetailsDTO
    {
        public string Details { get; set; }
        public int IdEvent { get; set; }
        public string LevelName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Origin { get; set; }
        public string Title { get; set; }
       public string Username { get; set; }             

    }
}

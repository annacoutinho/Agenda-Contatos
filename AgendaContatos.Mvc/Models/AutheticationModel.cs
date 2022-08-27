namespace AgendaContatos.Mvc.Models
{
    public class AutheticationModel
    {
        public Guid IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataHoraAcesso { get; set; }
    }
}

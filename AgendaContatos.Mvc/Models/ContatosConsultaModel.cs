namespace AgendaContatos.Mvc.Models
{
    public class ContatosConsultaModel
    {
        public  Guid  IdContato{get; set; }
        public  string Nome { get; set; }
        public string DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int Idade { get; set; }


    }
}

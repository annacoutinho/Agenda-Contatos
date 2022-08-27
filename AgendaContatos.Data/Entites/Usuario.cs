using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaContatos.Data.Entites
{
    public class Usuario
    {

        public Guid IdUsuario{ get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public string Senha { get; set; }
        public DateTime DataCadastro { get; set; }
        public List<Contato> Contatos { get; set; }
    }
}

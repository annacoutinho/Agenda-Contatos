using System.ComponentModel.DataAnnotations;

namespace AgendaContatos.Mvc.Models
{
        public class AccountPasswordRecoverModel
        {

        [EmailAddress(ErrorMessage = "Por favor,, informe um endereço de e-mail válido")]
        [Required(ErrorMessage = "Por favor, informe o e-mail de acesso.")]
        public string Email { get; set; }
        }

      

    }


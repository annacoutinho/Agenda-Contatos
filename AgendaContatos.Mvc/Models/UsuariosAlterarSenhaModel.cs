using System.ComponentModel.DataAnnotations;

namespace AgendaContatos.Mvc.Models
{
    public class UsuariosAlterarSenhaModel
    {
        [Required(ErrorMessage ="Por favor, informe sua nova senha.")]
        [MinLength(8, ErrorMessage = "Por favor, informe no mínimo {1} caracteres")]
        [MaxLength(20, ErrorMessage = "Por favor, informe no máximo {1} caracteres")]
        public string NovaSenha { get; set; }

        [Compare("NovaSenha", ErrorMessage ="Senhas não conferem, por favor verifique.")]
        [Required(ErrorMessage = "Por favor, confirme sua nova senha.")]
        public string NovaSenhaConfirmacao { get; set; }
    }
}

using AgendaContatos.Data.Repositories;
using AgendaContatos.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaContatos.Mvc.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        //ROTA: /Usuarios/Dados
        public IActionResult Dados()
        {
            return View();
        }
        //ROTA: /Usuarios/AlterarSenha

        public IActionResult AlterarSenha()
        {
            return View();
        }
        [HttpPost] ///Método recebe os dados enviados pelo formulario(SUBMIT)
        public IActionResult AlterarSenha(UsuariosAlterarSenhaModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //capturar os dados do usuario autenticado no sistema
                    var json = User.Identity.Name;
                    var auth = JsonConvert.DeserializeObject<AutheticationModel>(json);

                    //atualizando a senha do usuário no banco de dados
                    var usuarioRepository = new UsuarioRepository();
                    usuarioRepository.Update(auth.IdUsuario, model.NovaSenha);

                    TempData["Mensagem"] = $"Senha do usuário atualizada com sucesso";
                    ModelState.Clear();

                }
                catch(Exception e)
                {
                    TempData["Mensagem"] = $"Erro ao alterar senha: {e.Message}.";
                }

            }
            return View();
        }
    }
}

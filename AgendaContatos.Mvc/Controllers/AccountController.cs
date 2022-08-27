using AgendaContatos.Data.Entites;
using AgendaContatos.Data.Repositories;
using AgendaContatos.Messages;
using AgendaContatos.Mvc.Models;
using Bogus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AgendaContatos.Mvc.Controllers
{
    public class AccountController : Controller
    {
        //ROTA: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountLoginModel model)

        {
            if (ModelState.IsValid)
            {
                try
                {


                    //consultar o usuário no banco de dados através do email e da senha
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmailAndSenha(model.Email, model.Senha);

                    if (usuario != null)
                    {
                        //Gravar os dados do usuário autenticado em um arquivo de cookie do AspNet
                        //Este cokie de autenticação é que fará com que o AspNet saiba diferenciar
                        //Um usuário autenticado de um usuário não autenticado
                        var authenticationModel = new AutheticationModel();
                        authenticationModel.IdUsuario = usuario.IdUsuario;
                        authenticationModel.Nome = usuario.Nome;
                        authenticationModel.Email = usuario.Email;
                        authenticationModel.DataHoraAcesso = DateTime.Now;

                        //Para gravar os dados do objeto no cokie do AspNet precisamos
                        //Serializar este objeto em formato JSON
                        var json = JsonConvert.SerializeObject(authenticationModel);

                        //Autenticar o usuário!!
                        GravarCookieDeAutenticacao(json);



                        //redirecionar para outra página
                        return RedirectToAction("Consulta", "Contatos");
                    }
                    else
                    {
                        TempData["Mensagem"] = "Acesso negado. Usuário inválido";
                    }
                }
                catch (Exception e)
                {
                    TempData["Mensagem"] = $"Falha ao cadastrar: {e.Message}";
                }

            }


            return View();
        }

        private void GravarCookieDeAutenticacao(string json)
        {
            var claimsIdentity = new ClaimsIdentity
                (new[] { new Claim(ClaimTypes.Name, json) }, CookieAuthenticationDefaults.AuthenticationScheme);


            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(AccountRegisterModel model)

        {
            //verificar se todos os campos trazidos pela classe model
            //passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioRepository = new UsuarioRepository();

                    if (usuarioRepository.GetByEmail(model.Email) != null)
                    {
                        TempData["Mensagem"] = $"O email{model.Email} já está cadastrado com outro usuário. Tente outro email.";

                    }
                    else
                    {
                        var usuario = new Usuario();
                        usuario.IdUsuario = Guid.NewGuid();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = model.Senha;
                        usuario.DataCadastro = DateTime.Now;


                        usuarioRepository.Create(usuario);

                        TempData["Mensagem"] = $"Parabéns{usuario.Nome}, sua conta foi cadastrada com sucesso!";

                    }


                }
                catch (Exception e)
                {
                    TempData["Mensagem"] = $"Falha ao cadastrar {e.Message}";
                }
            }

            return View();
        }


        public IActionResult PasswordRecover()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PasswordRecover(AccountPasswordRecoverModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //buscar usuario no banco de dados através do email informado
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmail(model.Email);

                    if (usuario != null)
                    {
                        RecuperarSenhaDoUsuario(usuario);

                        TempData["Mensagem"] = $"Olá, {usuario.Nome}, você receberá um email contendo uma nova senha de acesso.";
                    }
                    else
                    {
                        TempData["Mensagem"] = $"Email informado não encontrado";
                    }
                }
                catch (Exception e)
                {
                    TempData["Mensagem"] = $"Falha ao cadastrar: {e.Message}";
                }

            }
            else
            {

            }


            return View();
        }


        //ROTA: //Account/Logout
        public IActionResult Logout()
        {
            //apagando o cookie de autenticação do usuário
            RemoverCookieDeAutenticacao();


            //redirecionar de volta a página de Login
            return RedirectToAction("Login", "Account");
        }

        //Método para gravar o Cookie de autenticação do usuário
        public void GarvarCookieDeAutenticacao(string json)
        {
            var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, json) }, CookieAuthenticationDefaults.AuthenticationScheme);

            //gravando o cookie de autenticação
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        }
        //método para apagar o cookie de autenticação do usuário
        public void RemoverCookieDeAutenticacao()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        //Método para fazer a recuperação da senha do usuário
        private void RecuperarSenhaDoUsuario(Usuario usuario)
        {
            //Gerando uma nova senha senha para o usuário
            var faker = new Faker();
            var novaSenha = faker.Internet.Password(10);

            var mailTo = usuario.Email;
            var subject = "Recuperação de senha de acesso - Agenda de Contatos";
            var body = $@"
                <div>
                    <p>Olá {usuario.Nome}, uma nova senha foi gerada com sucesso</p> 
                    <p>Utilize a senha <strong>{novaSenha} para acessar sua conta</strong><p>
                    <p>Depois de acessar, você poderá atualizar esta senha para outra de sua preferência.</p>
                    <p> Att,</p>
                    <p>Equipe Agenda de Contatos</p>
                 </div>   
            ";

            //enviando a senha para o email do usuario
            var emailMessage = new EmailMessage();
            emailMessage.SendMail(mailTo, subject, body);

            //atualizando a senha do usuário no banco dados
            var usuarioRepository = new UsuarioRepository();
            usuarioRepository.Update(usuario.IdUsuario, novaSenha);

        }
    }
}
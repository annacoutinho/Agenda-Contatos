using AgendaContatos.Data.Entites;
using AgendaContatos.Data.Repositories;
using AgendaContatos.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaContatos.Mvc.Controllers
{
    [Authorize]
    public class ContatosController : Controller
    {

        //Rota: /Contatos/Cadastro
        public IActionResult Cadastro()
        {
            return View();
        }
        [HttpPost]//Mátodo para receber os dados enciados pelo formulário (POST)
        public IActionResult Cadastro(ContatosCadastroModel model)
        {
            //verificar se os campos da model passaram na validação
            if (ModelState.IsValid)
            {
                try
                {
                    var authenticationModel = ObterUsuarioAutenticado();

                    var contato = new Contato();
                    contato.IdContato = Guid.NewGuid();
                    contato.Nome = model.Nome;
                    contato.Email = model.Email;
                    contato.Telefone = model.Telefone;
                    contato.DataNascimento = DateTime.Parse(model.DataNascimento);
                    contato.IdUsuario = authenticationModel.IdUsuario;

                    var contatoRepository = new ContatoRepository();
                    contatoRepository.Create(contato);

                    TempData["Mensagem"] = $"Contato {contato.Nome}, cadastrado com sucesso!";
                    ModelState.Clear();


                }
                catch (Exception e)
                {
                    TempData["Mensagem"] = $" Falha ao cadastrar contato: {e.Message}";
                }
            }
            return View();
        }
        //Rota: /Contatos/Consulta
        public IActionResult Consulta()
        {
            var lista = new List<ContatosConsultaModel>();
            try
            {
                //capturando os dados do usuário autenticado no projeto
                var authenticationModel = ObterUsuarioAutenticado();


                //acessando o banco de dados e trazer os contatos do usuário autenticado
                var contatoRepository = new ContatoRepository();
                var contatos = contatoRepository.GetByUsuario(authenticationModel.IdUsuario);

                foreach (var item in contatos)
                {
                    var model = new ContatosConsultaModel();

                    model.IdContato = item.IdContato;
                    model.Nome = item.Nome;
                    model.Email = item.Email;
                    model.Telefone = item.Telefone;
                    model.DataNascimento = item.DataNascimento.ToString("dd/MM/yyyy");
                    model.Idade = ObterIdade(item.DataNascimento);

                    lista.Add(model);


                }


            }
            catch (Exception e)
            {
                TempData["Mensagem"] = $"Falha ao consultar contatos: {e.Message}.";
            }

            return View(lista);
        }

        //Rota: /Contatos/Edicao
        public IActionResult Edicao(Guid id)
        {
            var model = new ContatosEdicaoModel();
            try
            {
                var authenticationModel = ObterUsuarioAutenticado();
                var ContatoRepository = new ContatoRepository();
                var contato = ContatoRepository.GetById(id,authenticationModel.IdUsuario);

                model.IdContato = contato.IdContato;
                model.Nome = contato.Nome;
                model.Telefone = contato.Telefone;
                model.Email = contato.Email;
                model.DataNascimento = contato.DataNascimento.ToString("yyyyy-MM-dd");


            } 
            catch(Exception e)
            {
                TempData["Mensagem"] = e.Message;
            }
            return View(model);
        } 
        [HttpPost]//Método para receber o submit do formulário
        public IActionResult Edicao(ContatosEdicaoModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var contato = new Contato();
                    contato.IdContato = model.IdContato; 
                    contato.Nome = model.Nome;
                    contato.Telefone = model.Telefone;
                    contato.Email = model.Email;
                    contato.DataNascimento = DateTime.Parse(model.DataNascimento);
                    
                    var contatoRepository = new ContatoRepository();
                    contatoRepository.Update(contato);


                    TempData["Mensagem"] = $"Contato {contato.Nome}, atualizado com sucesso.";

                }
                catch(Exception e)
                {
                    TempData["Mensagem"] = e.Message;
                }
            }
            return View(model);
        }

        //ROTA: /Contatos/Exclusao/id
        public IActionResult Exclusao(Guid id)
        {
            try
            {
                var contato = new Contato();
                contato.IdContato = id;

                //excluindo no banco de dados
                var contatoRepository = new ContatoRepository();
                contatoRepository.Delete(contato);
                TempData["Mensagem"] = $"Contato excluído com sucesso.";
            }
            catch (Exception e)
            {
                TempData["Mensagem"] = $"Falha ao excluircontato: {e.Message}.";
            }
            //redirecionar de volta para a página de consulta
            return RedirectToAction("Consulta");
             }
           

            public AutheticationModel ObterUsuarioAutenticado()
            {
                var json = User.Identity.Name;
                return JsonConvert.DeserializeObject<AutheticationModel>(json);


            }

            public int ObterIdade(DateTime dataNascimento)
            {
                var idade = DateTime.Now.Year - dataNascimento.Year;
                if (DateTime.Now.DayOfYear < dataNascimento.DayOfYear)
                    idade--;
                return idade;
            }
        }
    }



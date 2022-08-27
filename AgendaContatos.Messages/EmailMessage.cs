using System.Net;
using System.Net.Mail;

namespace AgendaContatos.Messages
{

    //Classe para envio de emails
    public class EmailMessage
    {
        #region Atributos
        private string _conta = "seuemail@seuemail.com";
        private string _senha = "suasenha";
        private string _smtp = "smtp-mail.outlook.com";
        private int _porta = 587;
        #endregion

        //método para realizar o envio de um e-mail
        public void SendMail(string emailTo, string subject, string body)
        {
            //criando o email que será  enviado
            var mailMessage = new MailMessage(_conta, emailTo);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            //enviando o email
            var smtpClient = new SmtpClient(_smtp, _porta);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_conta, _senha);
            smtpClient.Send(mailMessage);

        }


    }
}
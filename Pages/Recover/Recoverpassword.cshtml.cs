using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;
using RosaMaríaBookstore.Props;

namespace RosaMaríaBookstore.Pages.Recover
{
    public class RecoverpasswordModel : PageModel
    {
        [BindProperty]
        public String Email {get;set;}
        public String Message {get;set;}
        private bookstoreContext _context {get;}

        public RecoverpasswordModel(bookstoreContext context){
            this._context=context;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (Email == null || String.IsNullOrEmpty(Email) || String.IsNullOrWhiteSpace(Email))
            {
                Message = "Ingrese su correo electrónico para cambiar su contraseña.";
                return Page();
            }
            Usuario user = _context.Usuario
            .Include(u => u.IdempleadoNavigation)
            .ThenInclude(p => p.IdpersonaNavigation)
            .Where(u => u.Correo == Email)
            .FirstOrDefault();
            if (user == null)
            {
                Message = "Error, el correo no existe";
                return Page();
            }
            
            string code = EncryptDecrypt.CreateCode(6);
            Recuperarcuenta recover = new Recuperarcuenta
            {
                Codigo = code, 
                Fechaenvio = DateTime.Now,
                Fecharecuperacion = null,
                Estado=(sbyte)1,
                Idusuario = user.Id
            };
            this._context.Recuperarcuenta.Add(recover);
            SendCodeForEmail(Email, code, user);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("email", user.Correo);
            HttpContext.Session.SetInt32("idrecover", recover.Id);
            return RedirectToPage("/Recover/LoginWithCode");
        }
        public void SendCodeForEmail(string email, string code, Usuario usuario) {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("josemaichol@gmail.com","Librería Rosa María", System.Text.Encoding.UTF8);
                mail.To.Add(email);
                mail.Subject = "Recuperación de contraseña de Librería Rosa María.";
                mail.Body = "<p style='font-size:16px'> Hola "+usuario.IdempleadoNavigation.IdpersonaNavigation.Fullname()+", este es tu código de verificación para recuperar tu contraseña: <strong>"+code+"</strong></p>";
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("josemaichol@gmail.com", "1998Sony1998");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: "+ex);
            }
        }
    }
}
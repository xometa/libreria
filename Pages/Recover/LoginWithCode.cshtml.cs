using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Recover
{
    public class LoginWithCodeModel : PageModel
    {
        [BindProperty]
        public String Code {get;set;}
        public String Message {get;set;}
        private bookstoreContext _context {get;}

        public LoginWithCodeModel(bookstoreContext context){
            this._context=context;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
            int? Idrecover = HttpContext.Session.GetInt32("idrecover");
            string Email = HttpContext.Session.GetString("email");
            if (Idrecover == null || Email==null)
            {
                return RedirectToPage("/Recover/RecoverPassword");
            }
            if (Code == null)
            {
                Message = "Ingrese el código que fue enviado a su correo electrónico.";
                return Page();
            }
            DateTime currentDate = DateTime.Now;

            Recuperarcuenta recover =await this._context.Recuperarcuenta
               .Include(r => r.IdusuarioNavigation)
               .FirstOrDefaultAsync(r =>r.Fechaenvio.AddMinutes(60) > currentDate
                   && r.Fecharecuperacion == null
                   && r.IdusuarioNavigation.Correo == Email
                   && r.Id==Idrecover 
                   && r.Estado==1);
            if(recover==null) {
                Message = "Este correo no posee ningún código de verificación para recuperación de cuenta.";
                return Page();
            }
            if (recover.Codigo == Code)
            {
                recover.Estado =(sbyte) 0;
                recover.Fecharecuperacion = DateTime.Now;
                await _context.SaveChangesAsync();
                HttpContext.Session.SetInt32("Idtemporaryuser", recover.Idusuario);
                return RedirectToPage("/Recover/ChangePassword");
            }
            else
            {
                Message = "El código ingresado no coincide con el código enviado a su correo electrónico.";
                return Page();
            }
        }
    }
}
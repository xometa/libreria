using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;
using RosaMaríaBookstore.Props;

namespace RosaMaríaBookstore.Pages.Recover
{
    public class ChangePasswordModel : PageModel
    {
        [BindProperty]
        public Usuario UserInformation { get; set; }
        [BindProperty]
        public String Password {get;set;}
        public String Message {get;set;}
        private bookstoreContext _context {get;}

        public ChangePasswordModel(bookstoreContext context){
            this._context=context;
        }
        public async Task<IActionResult> OnGet()
        {
            int? Idtemporaryuser = HttpContext.Session.GetInt32("Idtemporaryuser");
            if (Idtemporaryuser != null)
            {
                this.UserInformation = await this._context.Usuario
                .Include(i => i.IdimagenNavigation)
                .Include(e => e.IdempleadoNavigation)
                .ThenInclude(p => p.IdpersonaNavigation)
                .FirstOrDefaultAsync(u => u.Id == Idtemporaryuser);
            }
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            int? Idtemporaryuser = HttpContext.Session.GetInt32("Idtemporaryuser");
            var Session = HttpContext.Session;
            if (Idtemporaryuser == null)
            {

                return RedirectToPage("Login/Login");
            }
            if (this.Password == null || 
            String.IsNullOrWhiteSpace(Password) || 
            String.IsNullOrEmpty(Password))
            {
                this.Message = "Campos vacíos";
                return Page();
            }
            else
            {

                var userData = await this._context.Usuario
                .Where(u => u.Id ==Idtemporaryuser.Value )
                .Include(u => u.IdempleadoNavigation)
                .ThenInclude(u => u.IdpersonaNavigation)
                .Include(u => u.IdimagenNavigation)
                .SingleOrDefaultAsync();
                if (userData == null)
                {
                    this.Message = "El usuario no existe.";
                    return Page();
                }
                if (userData != null && userData.Estado == 0)
                {
                    this.Message = "Su acceso ha sido denegado al sistema. Pongase en contacto con el administrador.";
                    return Page();
                }

                if (userData != null && userData.Estado == 1)
                {
                    Persona persona = userData.IdempleadoNavigation.IdpersonaNavigation;
                    Imagen image = userData.IdimagenNavigation;

                    string userName = persona.Nombre + " " + persona.Apellido;
                    string userImage = "";
                    if (image != null)
                    {
                        userImage = "/images/users/" + image.Nombre;
                    }
                    else
                    {
                        userImage = "/images/not-user.png";
                    }
                    //modificamos la contraseña
                    userData.Contrasena=EncryptDecrypt.GetSHA256(this.Password);
                    await this._context.SaveChangesAsync();

                    //registrando la sesión del usuario
                    Bitacora bitacora = new Bitacora();
                    bitacora.Idusuario = userData.Id;
                    bitacora.Iniciosesion = DateTime.Now;
                    this._context.Bitacora.Add(bitacora);
                    await this._context.SaveChangesAsync();

                    //creando las variables de sesión    
                    Session.SetInt32("IdUser", userData.Id);
                    Session.SetInt32("IdBitacora", bitacora.Id);
                    Session.SetString("Name", userName);
                    Session.SetString("Rol", userData.Rol);
                    Session.SetString("Image", userImage);
                    await UserActionsAsync("modificó la contraseña de acceso, para su usuario " + userData.Usuario1 + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                    await UserActionsAsync("inicio sesión como "+userData.Rol+", la fecha " + DateTime.Now.ToString("dd/MM/yyyy")+" a la "+DateTime.Now.ToString("hh:mm:ss"));
                }
            }
            return RedirectToPage("/Index");
        }
        public async Task UserActionsAsync(String Description)
        {
            Accion Action = new Accion();
            Action.Idbitacora = HttpContext.Session.GetInt32("IdBitacora").Value;
            Action.Descripcion = Description;
            Action.Hora = DateTime.Now;
            this._context.Accion.Add(Action);
            await this._context.SaveChangesAsync();
        }
    }
}
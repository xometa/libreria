using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using RosaMaríaBookstore.Models;
using RosaMaríaBookstore.Props;

namespace RosaMaríaBookstore.Pages.Login
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Usuario UserAccount { get; set; }
        public string Message { get; set; }
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        private bookstoreContext _context { get; }
        public LoginModel(bookstoreContext context)
        {
            this._context = context;
            this.UploadImage = new UploadImage(context);
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            var Session = HttpContext.Session;
            if (this.UserAccount.Usuario1 == null || this.UserAccount.Contrasena == null)
            {
                this.Message = "Campos vacíos";
                return Page();
            }
            else
            {
                var user = await this._context.Usuario.AnyAsync(u => u.Usuario1.Equals(this.UserAccount.Usuario1) && u.Contrasena.Equals(EncryptDecrypt.GetSHA256(this.UserAccount.Contrasena)));
                if (!user)
                {
                    this.Message = "El usuario o la contraseña son incorrectos.";
                    return Page();
                }

                Usuario userData = await this._context.Usuario
                .Where(u => u.Usuario1 == this.UserAccount.Usuario1
                && u.Contrasena == EncryptDecrypt.GetSHA256(this.UserAccount.Contrasena))
                .Include(u => u.IdempleadoNavigation)
                .ThenInclude(u => u.IdpersonaNavigation)
                .Include(u => u.IdimagenNavigation)
                .SingleOrDefaultAsync();
                if (userData == null)
                {
                    this.Message = "El usuario o la contraseña son incorrectos.";
                    return Page();
                }
                if (userData != null && userData.Estado == 0)
                {
                    this.Message = "Su acceso ha sido denegado al sistema. Pongase en contacto con el administrador";
                    return Page();
                }

                if (userData != null && user && userData.Estado == 1)
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
                    await UserActionsAsync("inicio sesión como " + userData.Rol + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
                }

                if (!userData.Rol.Equals("Administrador"))
                {
                    task = await UploadImage.accessUser(userData.Id, "Session", "");
                    if (task)
                    {
                        page = UploadImage.redirect;
                    }
                    else
                    {
                        //si hay un empleado con usuario y este no posee permisos,
                        //se debera sacar del sistema
                        page = "/Login/Logout";
                    }
                }
                else
                {
                    page = "/Index";
                }
                return RedirectToPage(page);
            }
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
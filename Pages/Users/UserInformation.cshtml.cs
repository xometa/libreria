using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;
using RosaMaríaBookstore.Props;

namespace RosaMaríaBookstore.Pages.Users
{

    public class UserInformationModel : PageModel
    {
        [BindProperty]
        public Empleado Empleado { get; set; }
        [BindProperty]
        public Persona Persona { get; set; }
        [BindProperty]
        public Usuario Usuario { get; set; }
        [BindProperty]
        public Telefono Telefono { get; set; }
        public Usuario UserInformation { get; set; }
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public UploadImage UploadImage { get; set; }
        String Message = "";
        private bookstoreContext _context { get; }
        public UserInformationModel(bookstoreContext context)
        {
            this._context = context;
            this.Empleado = new Empleado();
            this.Persona = new Persona();
            this.UserInformation = new Usuario();
            this.Usuario = new Usuario();
            this.Telefono = new Telefono();
            this.UploadImage = new UploadImage(context);
        }

        public void OnGet()
        {

        }

        public async Task<PartialViewResult> OnGetUserInformation()
        {
            //obteniendo el id de inicio de sesión del usuario
            if (HttpContext.Session.GetInt32("IdUser") != null)
            {
                int idUser = HttpContext.Session.GetInt32("IdUser").Value;
                this.UserInformation = await this._context.Usuario
                .Include(u => u.IdempleadoNavigation)
                    .ThenInclude(u => u.IdpersonaNavigation)
                .Include(u => u.IdimagenNavigation)
                .FirstOrDefaultAsync(u => u.Id == idUser);
                //agregamos los valores para editar en las respectivas variables
                this.Empleado = this.UserInformation.IdempleadoNavigation;
                this.Persona = this.UserInformation.IdempleadoNavigation.IdpersonaNavigation;
                this.Usuario = this.UserInformation;

                //agregamos teléfono es caso de que posea
                if (this.UserInformation.IdempleadoNavigation.Idtelefono != null)
                {
                    this.UserInformation.IdempleadoNavigation.IdtelefonoNavigation = await this._context.Telefono
                    .FirstOrDefaultAsync(t => t.Id == this.UserInformation.IdempleadoNavigation.Idtelefono);
                    this.Telefono = this.UserInformation.IdempleadoNavigation.IdtelefonoNavigation;
                }

                //agregamos cargo es caso de que posea
                if (this.UserInformation.IdempleadoNavigation.Idcargo != null)
                {
                    this.UserInformation.IdempleadoNavigation.IdcargoNavigation = await this._context.Cargo
                    .FirstOrDefaultAsync(t => t.Id == this.UserInformation.IdempleadoNavigation.Idcargo);
                }
            }
            return Partial("_UserInformationPartial", this);
        }

        public async Task<JsonResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Employed = (object)null, Action = "edit", Icon = "error" });
            }

            var user = await this._context.Usuario.FirstOrDefaultAsync(u => u.Id == this.Usuario.Id);
            var person = await this._context.Persona.FirstOrDefaultAsync(p => p.Id == this.Persona.Id);
            var employed = await this._context.Empleado.FirstOrDefaultAsync(e => e.Id == this.Empleado.Id);
            var phone = await this._context.Telefono.FirstOrDefaultAsync(t => t.Id == this.Telefono.Id);

            if ((employed.Idpersona != person.Id) ||
            (employed == null || person == null) ||
            (employed == null && person == null) ||
            user == null || (user.Idempleado != employed.Id))
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Employed = (object)null, Action = "edit", Icon = "error" });
            }

            if (!String.IsNullOrEmpty(Persona.Nombre) && !String.IsNullOrEmpty(Persona.Apellido)
                && !String.IsNullOrEmpty(Persona.Dui) && !String.IsNullOrEmpty(Empleado.Sexo) &&
                !String.IsNullOrEmpty(this.Usuario.Usuario1) && !String.IsNullOrEmpty(this.Usuario.Correo))
            {
                //verificación de que el # de DUI no exista
                if (!(person.Dui.Equals(this.Persona.Dui)))
                {
                    if (Exists(this.Persona.Dui, "", ""))
                    {
                        Message = "El DUI ingresado ya ha sido registrado.";
                        return new JsonResult(new { Message = Message, Employed = (object)null, Institution = (object)null, Action = "edit", Icon = "warning" });
                    }
                }

                //verificación de que el usuario no exista
                if (!(user.Usuario1.Equals(this.Usuario.Usuario1)))
                {
                    if (Exists("", this.Usuario.Usuario1, ""))
                    {
                        Message = "El usuario " + this.Usuario.Usuario1.ToLower() + " no esta disponible.";
                        return new JsonResult(new { Message = Message, User = (object)null, Action = "edit", Icon = "warning" });
                    }
                }

                //verifiación de que el correo no exista
                if (!(user.Correo.Equals(this.Usuario.Correo)))
                {
                    if (Exists("", "", this.Usuario.Correo))
                    {
                        Message = "El correo " + this.Usuario.Correo.ToLower() + " ya ha sido registrado.";
                        return new JsonResult(new { Message = Message, User = (object)null, Action = "edit", Icon = "warning" });
                    }
                }

                //agregar,modificación o eliminación del número de contacto
                if (phone != null && employed.Idtelefono > 0)
                {
                    if (employed.Idtelefono != phone.Id)
                    {
                        Message = "La información del servidor ha sido alterada.";
                        return new JsonResult(new { Message = Message, Employed = (object)null, Action = "edit", Icon = "error" });
                    }
                    if (!String.IsNullOrEmpty(this.Telefono.Telefono1))
                    {
                        phone.Telefono1 = this.Telefono.Telefono1;
                        await this._context.SaveChangesAsync();
                        this.Empleado.Idtelefono = phone.Id;
                    }
                    else
                    {
                        this.Empleado.Idtelefono = null;
                        this._context.Telefono.Remove(phone);
                        await this._context.SaveChangesAsync();
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(Telefono.Telefono1))
                    {
                        this.Telefono.Estado = (sbyte)1;
                        this._context.Telefono.Add(Telefono);
                        await this._context.SaveChangesAsync();
                        this.Empleado.Idtelefono = this.Telefono.Id;
                    }
                }
                //registro de persona
                person.Nombre = this.Persona.Nombre;
                person.Apellido = this.Persona.Apellido;
                person.Dui = this.Persona.Dui;
                await this._context.SaveChangesAsync();
                //registro de empleado
                employed.Sexo = this.Empleado.Sexo;
                employed.Fechanacimiento = this.Empleado.Fechanacimiento;
                employed.Idtelefono = this.Empleado.Idtelefono;
                await this._context.SaveChangesAsync();


                if (this.Usuario.Archivo != null)
                {
                    String Route = "users";
                    int IdImage = await UploadImage.saveImage(this.Usuario.Archivo, Route);
                    if (IdImage > 0)
                    {
                        //si se asigna una nueva imagen, al usuario, y este ya posee una, es de eliminarla de la DB,
                        //posteriormente se procede a asignar el Id de la imagen nueva del producto
                        await deleteImage(user);
                        this.Usuario.Idimagen = IdImage;
                    }
                    else if (IdImage == -1)
                    {
                        Message = "El nombre del archivo: " + this.Usuario.Archivo.FileName + " es incorrecto";
                        return new JsonResult(new { Message = Message, User = (object)null, Action = "edit", Icon = "warning" });
                    }
                    else if (IdImage == -2)
                    {
                        Message = "La imagen: " + this.Usuario.Archivo.FileName + " ya ha sido agregada. Cambie el nombre de la imagen y vuelva a intentarlo.";
                        return new JsonResult(new { Message = Message, User = (object)null, Action = "edit", Icon = "warning" });
                    }
                }
                else
                {
                    int IdImage = await searchImage(this.Usuario.Id);
                    if (IdImage != 0)
                    {
                        this.Usuario.Idimagen = IdImage;
                    }
                }

                user.Usuario1 = this.Usuario.Usuario1;
                if (!String.IsNullOrEmpty(this.Usuario.Contrasena))
                {
                    //Encryptando la contraseña
                    String Password = EncryptDecrypt.GetSHA256(this.Usuario.Contrasena);
                    user.Contrasena = Password;
                }
                user.Correo = this.Usuario.Correo;
                user.Idimagen = this.Usuario.Idimagen;
                await this._context.SaveChangesAsync();

                await UserActionsAsync("modificó la información de su cuenta, la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "La información de su cuenta ha sido actualizada.";
                return new JsonResult(new { Message = Message, Employed = (object)this.Empleado, Action = "edit", Icon = "success" });
            }
            else
            {
                Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                return new JsonResult(new { Message = Message, Employed = (object)null, Action = "add", Icon = "error" });
            }
        }
        public bool Exists(String DUI, String User, String Email)
        {
            if (!DUI.Equals(""))
            {
                return _context.Persona.Where(p => p.Dui == DUI).Where(p => p.Tipo == "Empleado").Any();
            }
            else if (!User.Equals(""))
            {
                return _context.Usuario.Where(u => u.Usuario1 == User).Any();
            }
            else if (!Email.Equals(""))
            {
                return _context.Usuario.Where(u => u.Correo == Email).Any();
            }
            else
            {
                return false;
            }
        }
        public async Task deleteImage(Usuario User)
        {
            var image = await this._context.Imagen.FirstOrDefaultAsync(i => i.Id == User.Idimagen);
            //eliminamos la imagen que corresponde al usuario, para que no
            //hallan registros sin utilizar en la db
            if (image != null)
            {
                this._context.Imagen.Remove(image);
                await this._context.SaveChangesAsync();
                //eliminación de la imagen del servidor
                UploadImage.deleteServerImage("wwwroot/images/users/", image.Nombre);
            }
        }
        public async Task<int> searchImage(int Id)
        {
            int IdImage = 0;
            var user = await this._context.Usuario.FirstOrDefaultAsync(u => u.Id == Id);
            if (user != null)
            {
                if (user.Idimagen != null) { IdImage = (int)user.Idimagen; }
            }
            return IdImage;
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
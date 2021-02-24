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
    public class EmployedModel : PageModel
    {
        [BindProperty]
        public Empleado Empleado { get; set; }
        [BindProperty]
        public Persona Persona { get; set; }
        [BindProperty]
        public Usuario Usuario { get; set; }
        [BindProperty]
        public Telefono Telefono { get; set; }
        public IList<Cargo> positionsList { get; set; }
        public UploadImage UploadImage { get; set; }
        String Message = "";
        private bookstoreContext _context { get; }
        public string page { get; set; }
        public bool task { get; set; }
        public EmployedModel(bookstoreContext context)
        {
            this._context = context;
            this.UploadImage = new UploadImage(context);
        }
        public async Task<IActionResult> OnGet()
        {
            //obteniendo el id de inicio de sesión del usuario y su rol
            int? idUser = HttpContext.Session.GetInt32("IdUser");
            string rol = HttpContext.Session.GetString("Rol");
            if (idUser != null)
            {
                if (!rol.Equals("Administrador"))
                {
                    task = await UploadImage.accessUser(idUser.Value, "Personal", "/Users/Employed");
                    if (task)
                    {
                        return Page();
                    }
                    else
                    {
                        //si hay un empleado con usuario y este no posee permisos,
                        //se debera sacar del sistema
                        page = UploadImage.redirect;
                        return RedirectToPage(page);
                    }
                }
                else
                {
                    return Page();
                }
            }
            else
            {
                page = "/Login/Login";
                return RedirectToPage(page);
            }
        }
        public async Task<PartialViewResult> OnPostPartialView(int Id)
        {
            if (Id > 0)
            {
                this.Empleado = await this._context.Empleado.FirstOrDefaultAsync(e => e.Id == Id);
                this.Persona = await this._context.Persona.FirstOrDefaultAsync(p => p.Id == this.Empleado.Idpersona);
                if (this.Empleado.Idtelefono != null && this.Empleado.Idtelefono > 0)
                {
                    this.Telefono = await this._context.Telefono.FirstOrDefaultAsync(t => t.Id == this.Empleado.Idtelefono);
                }

            }
            this.positionsList = this._context.Cargo.ToList();
            return Partial("_ModalEmployedPartial", this);
        }
        public async Task<PartialViewResult> OnPostUserPartialView(int Id)
        {
            if (Id > 0)
            {
                this.Usuario = await this._context.Usuario
                .Include(i => i.IdimagenNavigation)
                .Include(e => e.IdempleadoNavigation)
                .ThenInclude(e => e.IdpersonaNavigation)
                .FirstOrDefaultAsync(u => u.Idempleado == Id);
                this.Usuario.Contrasena = null;

            }
            return Partial("_ModalUserPartial", this);
        }

        public async Task<PartialViewResult> OnPostUserInformationPartialView(int Id)
        {
            if (Id > 0)
            {
                this.Usuario = await this._context.Usuario
                .Include(i => i.IdimagenNavigation)
                .Include(e => e.IdempleadoNavigation)
                .ThenInclude(p => p.IdpersonaNavigation)
                .FirstOrDefaultAsync(u => u.Idempleado == Id);

            }
            return Partial("_ModalInfUsPartial", this);
        }
        public async Task<JsonResult> OnPostDelete(int Id)
        {
            if (Id < 1)
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Employed = (object)null, Action = "delete", Icon = "error" });
            }
            var employed = await this._context.Empleado
            .Include(e => e.Usuario)
            .Include(p => p.IdpersonaNavigation)
            .FirstOrDefaultAsync(e => e.Id == Id);
            var person = await this._context.Persona
            .FirstOrDefaultAsync(p => p.Id == employed.Idpersona);
            if (employed == null && person == null)
            {
                Message = "El empleado a eliminar no existe en los registros.";
                return new JsonResult(new { Message = Message, Employed = (object)null, Action = "delete", Icon = "warning" });
            }
            if (employed.Usuario.Count > 0)
            {
                var user = await UserDeleteAll(employed.Id);
                if (user == null)
                {
                    Message = "El empleado no se elimino, ya que posee registros de operaciones.";
                    return new JsonResult(new { Message = Message, Employed = (object)null, Action = "delete", Icon = "warning" });
                }
            }
            this._context.Empleado.Remove(employed);
            await this._context.SaveChangesAsync();
            this._context.Persona.Remove(person);
            await this._context.SaveChangesAsync();
            if (employed.Idtelefono != null && employed.Idtelefono > 0)
            {
                var phone = await this._context.Telefono.FirstOrDefaultAsync(p => p.Id == employed.Idtelefono);
                this._context.Telefono.Remove(phone);
                await this._context.SaveChangesAsync();
            }
            await UserActionsAsync("eliminó el empleado " + employed.IdpersonaNavigation.Fullname() + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
            Message = "El empleado ha sido eliminado satisfactoriamente.";
            return new JsonResult(new { Message = Message, Employed = (object)employed, Action = "delete", Icon = "success" });
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

        public async Task<JsonResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Employed = (object)null, Action = "add", Icon = "error" });
            }
            if (!String.IsNullOrEmpty(Persona.Nombre) && !String.IsNullOrEmpty(Persona.Apellido)
            && !String.IsNullOrEmpty(Persona.Dui) && !String.IsNullOrEmpty(Empleado.Sexo))
            {
                if (Exists(Persona.Dui, "", ""))
                {
                    Message = "El DUI del empleado ya ha sido registrado.";
                    return new JsonResult(new { Message = Message, Employed = (object)null, Action = "add", Icon = "warning" });
                }
                this._context.Persona.Add(Persona);
                await this._context.SaveChangesAsync();
                this.Empleado.Idpersona = this.Persona.Id;
                if (!String.IsNullOrEmpty(Telefono.Telefono1))
                {
                    this.Telefono.Estado = (sbyte)1;
                    this._context.Telefono.Add(Telefono);
                    await this._context.SaveChangesAsync();
                    this.Empleado.Idtelefono = this.Telefono.Id;
                }
                this._context.Empleado.Add(Empleado);
                await this._context.SaveChangesAsync();
                await UserActionsAsync("registro el empleado " + Persona.Fullname() + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "El empleado " + this.Persona.Nombre + " ha sido registrado correctamente.";
                return new JsonResult(new { Message = Message, Employed = (object)Empleado, Action = "add", Icon = "success" });
            }
            else
            {
                Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                return new JsonResult(new { Message = Message, Employed = (object)null, Action = "add", Icon = "error" });
            }
        }

        public async Task<JsonResult> OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Employed = (object)null, Action = "edit", Icon = "error" });
            }
            var person = await this._context.Persona.FirstOrDefaultAsync(p => p.Id == this.Persona.Id);
            var employed = await this._context.Empleado.FirstOrDefaultAsync(e => e.Id == this.Empleado.Id);
            var phone = await this._context.Telefono.FirstOrDefaultAsync(t => t.Id == this.Telefono.Id);
            if ((employed.Idpersona != person.Id) ||
            (employed == null || person == null) ||
            (employed == null && person == null))
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Employed = (object)null, Action = "edit", Icon = "error" });
            }

            if (!String.IsNullOrEmpty(Persona.Nombre) && !String.IsNullOrEmpty(Persona.Apellido)
            && !String.IsNullOrEmpty(Persona.Dui) && !String.IsNullOrEmpty(Empleado.Sexo))
            {
                if (!(person.Dui.Equals(this.Persona.Dui)))
                {
                    if (Exists(this.Persona.Dui, "", ""))
                    {
                        Message = "El DUI ingresado para el empleado " + this.Persona.Nombre.ToLower() + " ya ha sido registrado.";
                        return new JsonResult(new { Message = Message, Employed = (object)null, Institution = (object)null, Action = "edit", Icon = "warning" });
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
                employed.Idcargo = this.Empleado.Idcargo;
                await this._context.SaveChangesAsync();
                await UserActionsAsync("modificó la información del empleado " + person.Fullname() + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "La información del empleado ha sido moficada correctamente.";
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

        public async Task<JsonResult> OnPostAddUser()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, User = (object)null, Action = "edit", Icon = "error" });
            }
            if (!String.IsNullOrEmpty(this.Usuario.Usuario1) &&
            !String.IsNullOrEmpty(this.Usuario.Contrasena) &&
            !String.IsNullOrEmpty(this.Usuario.Correo) &&
            !String.IsNullOrEmpty(this.Usuario.Rol))
            {
                if (Exists("", this.Usuario.Usuario1, ""))
                {
                    Message = "El usuario no esta disponible, por favor ingrese otro.";
                    return new JsonResult(new { Message = Message, User = (object)null, Action = "add", Icon = "warning" });
                }

                if (Exists("", "", this.Usuario.Correo))
                {
                    Message = "El correo " + this.Usuario.Correo.ToLower() + " ya ha sido registrado.";
                    return new JsonResult(new { Message = Message, User = (object)null, Action = "add", Icon = "warning" });
                }

                /* Comprobamos que se asignado una imagen al usuario, de ser así se 
                 * procede a realizar las validaciones respectivas al archivo
                 */
                String Route = "users";
                int IdImage = await UploadImage.saveImage(this.Usuario.Archivo, Route);
                if (IdImage > 0)
                {
                    this.Usuario.Idimagen = IdImage;
                }
                else if (IdImage == -1)
                {
                    Message = "El nombre del archivo: " + this.Usuario.Archivo.FileName + " es incorrecto";
                    return new JsonResult(new { Message = Message, User = (object)null, Action = "add", Icon = "warning" });
                }
                else if (IdImage == -2)
                {
                    Message = "La imagen: " + this.Usuario.Archivo.FileName + " ya ha sido agregada. Cambie el nombre de la imagen y vuelva a intentarlo.";
                    return new JsonResult(new { Message = Message, User = (object)null, Action = "add", Icon = "warning" });
                }
                //Encryptando la contraseña
                String Password = EncryptDecrypt.GetSHA256(this.Usuario.Contrasena);
                this.Usuario.Contrasena = Password;
                //Guardando el registro
                this.Usuario.Estado = (sbyte)0;
                this._context.Usuario.Add(Usuario);
                await this._context.SaveChangesAsync();
                await UserActionsAsync("creo el usuario " + Usuario.Usuario1 + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "El usuario se ha creado correctamente.";
                return new JsonResult(new { Message = Message, User = (object)this.Usuario, Action = "add", Icon = "success" });
            }
            else
            {
                Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                return new JsonResult(new { Message = Message, User = (object)null, Action = "add", Icon = "warning" });
            }
        }

        public async Task<JsonResult> OnPostEditUser()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, User = (object)null, Action = "edit", Icon = "error" });
            }
            if (!String.IsNullOrEmpty(this.Usuario.Usuario1) &&
            !String.IsNullOrEmpty(this.Usuario.Correo) &&
           !String.IsNullOrEmpty(this.Usuario.Rol))
            {
                var user = await this._context.Usuario.FirstOrDefaultAsync(u => u.Id == this.Usuario.Id);
                if (!(user.Usuario1.Equals(this.Usuario.Usuario1)))
                {
                    if (Exists("", this.Usuario.Usuario1, ""))
                    {
                        Message = "El usuario " + this.Usuario.Usuario1.ToLower() + " no esta disponible.";
                        return new JsonResult(new { Message = Message, User = (object)null, Action = "edit", Icon = "warning" });
                    }
                }

                if (!(user.Correo.Equals(this.Usuario.Correo)))
                {
                    if (Exists("", "", this.Usuario.Correo))
                    {
                        Message = "El correo " + this.Usuario.Correo.ToLower() + " ya ha sido registrado.";
                        return new JsonResult(new { Message = Message, User = (object)null, Action = "edit", Icon = "warning" });
                    }
                }

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
                    int IdImage = await searchProduct(this.Usuario.Id);
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
                user.Rol = this.Usuario.Rol;
                user.Idimagen = this.Usuario.Idimagen;
                await this._context.SaveChangesAsync();
                await UserActionsAsync("modificó el usuario " + user.Usuario1 + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "El usuario se modificado correctamente.";
                return new JsonResult(new { Message = Message, User = (object)user, Action = "edit", Icon = "success" });
            }
            else
            {
                Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                return new JsonResult(new { Message = Message, User = (object)null, Action = "edit", Icon = "success" });
            }
        }
        public async Task<int> searchProduct(int Id)
        {
            int IdImage = 0;
            var user = await this._context.Usuario.FirstOrDefaultAsync(u => u.Id == Id);
            if (user != null)
            {
                if (user.Idimagen != null) { IdImage = (int)user.Idimagen; }
            }
            return IdImage;
        }

        public async Task<JsonResult> OnPostDeleteUser(int Id)
        {
            if (Id < 1)
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, User = (object)null, Action = "delete", Icon = "error" });
            }
            else
            {
                var user = await UserDeleteAll(Id);
                if (user == null)
                {
                    Message = "El usuario no se elimino, ya que posee registros de operaciones.";
                    return new JsonResult(new { Message = Message, User = (object)null, Action = "delete", Icon = "warning" });
                }
                else
                {
                    Message = "El usuario se ha eliminado correctamente.";
                    return new JsonResult(new { Message = Message, User = (object)user, Action = "delete", Icon = "success" });
                }
            }
        }

        public async Task<Usuario> UserDeleteAll(int Id)
        {
            var user = await this._context.Usuario
                .Include(u => u.IdimagenNavigation)
                .Include(u => u.Bitacora)
                .Include(u => u.Compra)
                .Include(u => u.Detallepermisosusuario)
                .Include(u => u.Recuperarcuenta)
                .Include(u => u.Venta)
                .FirstOrDefaultAsync(u => u.Idempleado == Id);
            if (user.Venta.Count > 0 || user.Compra.Count > 0
            || user.Bitacora.Count > 0 || user.Recuperarcuenta.Count > 0)
            {
                return null;
            }
            if (user.Detallepermisosusuario.Count > 0)
            {
                IList<Detallepermisosusuario> detail = this._context.Detallepermisosusuario.Where(dpu => dpu.Idusuario == user.Id).ToList();
                foreach (var permission in detail)
                {
                    this._context.Detallepermisosusuario.Remove(permission);
                    await this._context.SaveChangesAsync();
                }
                await UserActionsAsync("eliminó los permisos del usuario " + user.Usuario1 + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
            }
            this._context.Usuario.Remove(user);
            await this._context.SaveChangesAsync();
            await UserActionsAsync("eliminó el usuario " + user.Usuario1 + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
            //procedimiento para eliminar la imagen del usuario
            if (user.IdimagenNavigation != null && user.Idimagen > 0)
            {
                await deleteImage(user);
            }
            return user;
        }

        public async Task<JsonResult> OnPostUserModifiedStatus(int Id, int Status)
        {
            if (Id < 1)
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, User = (object)null, Action = "status", Icon = "error" });
            }
            else
            {
                var user = await this._context.Usuario.FirstOrDefaultAsync(u => u.Idempleado == Id);
                if (user == null)
                {
                    Message = "El estado del usuario no se puede modificar (El usuario no existe).";
                    return new JsonResult(new { Message = Message, User = (object)null, Action = "status", Icon = "warning" });
                }
                user.Estado = (sbyte)Status;
                await this._context.SaveChangesAsync();
                await UserActionsAsync("modificó el estado del usuario " + user.Usuario1 + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "El estado del usuario se ha modificado correctamente.";
                return new JsonResult(new { Message = Message, User = (object)user, Action = "delete", Icon = "success" });
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

/*
var user = await this._context.Usuario
               .Include(u => u.IdimagenNavigation)
               .Include(u => u.Bitacora)
               .Include(u => u.Compra)
               .Include(u => u.Detallepermisosusuario)
               .Include(u => u.Recuperarcuenta)
               .Include(u => u.Venta)
               .FirstOrDefaultAsync(u => u.Idempleado == employed.Id);
if (user.Venta.Count > 0 || user.Compra.Count > 0
|| user.Bitacora.Count > 0 || user.Recuperarcuenta.Count > 0)
{
    Message = "El empleado no se elimino, ya que posee registros de operaciones.";
    return new JsonResult(new { Message = Message, Employed = (object)null, Action = "delete", Icon = "warning" });
}
if (user.Detallepermisosusuario.Count > 0)
{
    IList<Detallepermisosusuario> detail = this._context.Detallepermisosusuario.Where(dpu => dpu.Idusuario == user.Id).ToList();
    foreach (var permission in detail)
    {
        this._context.Detallepermisosusuario.Remove(permission);
        await this._context.SaveChangesAsync();
    }
}
this._context.Usuario.Remove(user);
await this._context.SaveChangesAsync();

//procedimiento para eliminar la imagen del usuario
if (user.IdimagenNavigation != null && user.Idimagen > 0)
{
    await deleteImage(user);
}
*/
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
    public class PositionsModel : PageModel
    {
        [BindProperty]
        public Cargo Cargo { get; set; }
        private bookstoreContext _context { get; }
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        String Message = "";
        public PositionsModel(bookstoreContext context)
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
                    task = await UploadImage.accessUser(idUser.Value, "Personal", "/Users/Positions");
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
        public async Task<PartialViewResult> OnPostPartialView(int? Id)
        {
            if (Id != 0)
            {
                this.Cargo = await this._context.Cargo
                .FirstOrDefaultAsync(p => p.Id == Id);
            }
            return Partial("_ModalPositionPartial", this);
        }

        public async Task<PartialViewResult> OnPostPositionInformation(int Id)
        {
            if (Id > 0)
            {
                this.Cargo = await this._context.Cargo
                   .FirstOrDefaultAsync(p => p.Id == Id);
            }
            return Partial("_ModalPosiInfoPartial", this);
        }

        public async Task<JsonResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Position = (object)null, Action = "save", Icon = "error" });
            }
            else
            {

                if (PositionExists(0, Cargo.Nombre))
                {
                    Message = "El nombre ingresado para el cargo " + Cargo.Nombre + ", ya ha sido asignado.";
                    return new JsonResult(new { Message = Message, Position = (object)null, Action = "save", Icon = "warning" });
                }
                else
                {
                    this._context.Cargo.Add(Cargo);
                    await this._context.SaveChangesAsync();
                    await UserActionsAsync("registro un nuevo cargo (" + Cargo.Nombre + "), la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
                    Message = "El cargo se ha registrado correctamente.";
                    return new JsonResult(new { Message = Message, Position = (object)Cargo, Action = "save", Icon = "success" });
                }
            }
        }

        public async Task<JsonResult> OnPostDelete(int Id)
        {
            if (Id < 0)
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Position = (object)null, Action = "delete", Icon = "error" });
            }
            var position = await this._context.Cargo
            .Include(c => c.Empleado)
            .FirstOrDefaultAsync(p => p.Id == Id);
            if (position == null)
            {
                Message = "El cargo a eliminar no existe en los registros.";
                return new JsonResult(new { Message = Message, Position = (object)null, Action = "delete", Icon = "warning" });
            }
            if (position.Empleado.Count > 0)
            {
                Message = "El cargo no se debe eliminar, ya que se han registrado empleados con el mismo.";
                return new JsonResult(new { Message = Message, Position = (object)null, Action = "delete", Icon = "warning" });
            }
            this._context.Cargo.Remove(position);
            await this._context.SaveChangesAsync();
            await UserActionsAsync("eliminó la información del cargo " + position.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
            Message = "El cargo ha sido eliminado satisfactoriamente.";
            return new JsonResult(new { Message = Message, Position = (object)position, Action = "delete", Icon = "success" });
        }

        public async Task<JsonResult> OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Position = (object)null, Action = "edit", Icon = "error" });
            }
            else
            {
                var position = await this._context.Cargo.FirstOrDefaultAsync(p => p.Id == Cargo.Id);
                if (!(position.Nombre.Equals(Cargo.Nombre)))
                {
                    if (PositionExists(0, Cargo.Nombre))
                    {
                        Message = "El nombre ingresado para el cargo " + Cargo.Nombre + ", ya ha sido asignado.";
                        return new JsonResult(new { Message = Message, Position = (object)null, Action = "edit", Icon = "error" });
                    }
                }

                position.Nombre = Cargo.Nombre;
                await this._context.SaveChangesAsync();
                await UserActionsAsync("modificó la información del cargo " + position.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
                Message = "El cargo ha sido modificado satisfactoriamente.";
            }
            return new JsonResult(new { Message = Message, Position = (object)Cargo, Action = "edit", Icon = "success" });
        }

        public bool PositionExists(int Id, String Nick)
        {
            if (Id > 0)
            {
                return _context.Cargo.Where(p => p.Id == Id).Any();
            }
            else if (!Nick.Equals(""))
            {
                return _context.Cargo.Where(p => p.Nombre == Nick).Any();
            }
            else
            {
                return false;
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
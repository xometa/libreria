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

namespace RosaMaríaBookstore.Pages.Trademarks
{
    public class TrademarksModel : PageModel
    {
        [BindProperty]
        public Marca Marca { get; set; }
        private bookstoreContext _context { get; }
        String Message = "";
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        public TrademarksModel(bookstoreContext context)
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
                    task = await UploadImage.accessUser(idUser.Value, "Marcas", "/Trademarks/Trademarks");
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
                this.Marca = await this._context.Marca
                .FirstOrDefaultAsync(p => p.Id == Id);
            }
            return Partial("_ModalTrademarkPartial", this);
        }

        public async Task<PartialViewResult> OnPostTrademarkInformation(int Id)
        {
            if (Id > 0)
            {
                this.Marca = await this._context.Marca
                   .FirstOrDefaultAsync(p => p.Id == Id);
            }
            return Partial("_ModalTradeInfoPartial", this);
        }

        public async Task<JsonResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Trademark = (object)null, Action = "save", Icon = "error" });
            }
            else
            {

                if (TrademarkExists(0, Marca.Nombre))
                {
                    Message = "El nombre ingresado para la marca " + Marca.Nombre + ", ya ha sido asignado.";
                    return new JsonResult(new { Message = Message, Trademark = (object)null, Action = "save", Icon = "warning" });
                }
                else
                {
                    this._context.Marca.Add(Marca);
                    await this._context.SaveChangesAsync();
                    await UserActionsAsync("registro una nueva marca (" + Marca.Nombre + "), la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
                    Message = "La marca se ha registrado correctamente.";
                }
            }
            return new JsonResult(new { Message = Message, Trademark = (object)Marca, Action = "save", Icon = "success" });
        }

        public async Task<JsonResult> OnPostDelete(int Id)
        {
            if (Id < 0)
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Trademark = (object)null, Action = "delete", Icon = "error" });
            }
            var trademark = await this._context.Marca
            .Include(t => t.Producto)
            .FirstOrDefaultAsync(p => p.Id == Id);
            if (trademark == null)
            {
                Message = "La marca a eliminar no existe en los registros.";
                return new JsonResult(new { Message = Message, Trademark = (object)null, Action = "delete", Icon = "warning" });
            }
            if (trademark.Producto.Count > 0)
            {
                Message = "La marca no se debe eliminar, ya que se han registrado productos con la misma.";
                return new JsonResult(new { Message = Message, Trademark = (object)null, Action = "delete", Icon = "warning" });
            }
            this._context.Marca.Remove(trademark);
            await this._context.SaveChangesAsync();
            await UserActionsAsync("eliminó la información de la marca " + trademark.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
            Message = "La marca ha sido eliminada satisfactoriamente.";
            return new JsonResult(new { Message = Message, Trademark = (object)trademark, Action = "delete", Icon = "success" });
        }

        public async Task<JsonResult> OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Trademark = (object)null, Action = "edit", Icon = "error" });
            }
            else
            {
                var trademark = await this._context.Marca.FirstOrDefaultAsync(p => p.Id == Marca.Id);
                if (!(trademark.Nombre.Equals(Marca.Nombre)))
                {
                    if (TrademarkExists(0, Marca.Nombre))
                    {
                        Message = "El nombre ingresado para la marca " + Marca.Nombre + ", ya ha sido asignada.";
                        return new JsonResult(new { Message = Message, Trademark = (object)null, Action = "edit", Icon = "error" });
                    }
                }

                trademark.Nombre = Marca.Nombre;
                await this._context.SaveChangesAsync();
                await UserActionsAsync("modificó la información de la marca " + trademark.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
                Message = "La marca ha sido modificada satisfactoriamente.";
            }
            return new JsonResult(new { Message = Message, Trademark = (object)Marca, Action = "edit", Icon = "success" });
        }

        public bool TrademarkExists(int Id, String Nick)
        {
            if (Id > 0)
            {
                return _context.Marca.Where(p => p.Id == Id).Any();
            }
            else if (!Nick.Equals(""))
            {
                return _context.Marca.Where(p => p.Nombre == Nick).Any();
            }
            else
            {
                return false;
            }
        }

        public async Task<JsonResult> OnPostEditStatus(int Id, int Status)
        {
            if (Id > 0)
            {
                var obj = await this._context.Marca.FirstOrDefaultAsync(t => t.Id == Id);
                if (obj == null)
                {
                    Message = "Advertencia. El estado del contacto no se puede modificar.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "change", Icon = "warning" });
                }
                obj.Estado = (sbyte)Status;
                this._context.Attach(obj).State = EntityState.Modified;
                await this._context.SaveChangesAsync();
                await UserActionsAsync("modificó el estado de la marca " + obj.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                if (obj.Estado == 1)
                {

                    Message = "La marca " + obj.Nombre + " ha sido habilitada.";
                }
                else
                {

                    Message = "La marca " + obj.Nombre + " ha sido deshabilitada.";
                }
                return new JsonResult(new { Message = Message, Trademark = (object)obj, Action = "change", Icon = "success" });
            }
            else
            {
                Message = "Advertencia. La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Trademark = (object)null, Action = "change", Icon = "warning" });
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
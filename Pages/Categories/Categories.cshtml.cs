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

namespace RosaMaríaBookstore.Pages.Categories
{
    public class CategoriesModel : PageModel
    {
        [BindProperty]
        public Categoria Categoria { get; set; }
        private bookstoreContext _context { get; }
        String Message = "";
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        public CategoriesModel(bookstoreContext context)
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
                    task = await UploadImage.accessUser(idUser.Value, "Categorías", "/Categories/Categories");
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
                this.Categoria = await this._context.Categoria
                .FirstOrDefaultAsync(p => p.Id == Id);
            }
            return Partial("_ModalCategoryPartial", this);
        }

        public async Task<PartialViewResult> OnPostCategoryInformation(int Id)
        {
            if (Id > 0)
            {
                this.Categoria = await this._context.Categoria
                   .FirstOrDefaultAsync(p => p.Id == Id);
            }
            return Partial("_ModalCateInfoPartial", this);
        }

        public async Task<JsonResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Category = (object)null, Action = "save", Icon = "error" });
            }
            else
            {

                if (CategoryExists(0, Categoria.Nombre))
                {
                    Message = "El nombre ingresado para la categoría " + Categoria.Nombre + ", ya ha sido asignado.";
                    return new JsonResult(new { Message = Message, Category = (object)null, Action = "save", Icon = "warning" });
                }
                else
                {
                    this._context.Categoria.Add(Categoria);
                    await this._context.SaveChangesAsync();
                    await UserActionsAsync("registro una nueva categoría (" + Categoria.Nombre + "), la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
                    Message = "La categoria se ha registrado correctamente.";
                }
            }
            return new JsonResult(new { Message = Message, Category = (object)Categoria, Action = "save", Icon = "success" });
        }

        public async Task<JsonResult> OnPostDelete(int Id)
        {
            if (Id < 0)
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Category = (object)null, Action = "delete", Icon = "error" });
            }
            var category = await this._context.Categoria
            .Include(c => c.Producto)
            .FirstOrDefaultAsync(p => p.Id == Id);
            if (category == null)
            {
                Message = "La categoría a eliminar no existe en los registros.";
                return new JsonResult(new { Message = Message, Category = (object)null, Action = "delete", Icon = "warning" });
            }
            if (category.Producto.Count > 0)
            {
                Message = "La categoría no se debe eliminar, ya que se han registrado productos con la misma.";
                return new JsonResult(new { Message = Message, Category = (object)null, Action = "delete", Icon = "warning" });
            }
            this._context.Categoria.Remove(category);
            await this._context.SaveChangesAsync();
            await UserActionsAsync("eliminó la información de la categoría " + category.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
            Message = "La categoría ha sido eliminada satisfactoriamente.";
            return new JsonResult(new { Message = Message, Category = (object)category, Action = "delete", Icon = "success" });
        }

        public async Task<JsonResult> OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Category = (object)null, Action = "edit", Icon = "error" });
            }
            else
            {
                var category = await this._context.Categoria.FirstOrDefaultAsync(p => p.Id == Categoria.Id);
                if (!(category.Nombre.Equals(Categoria.Nombre)))
                {
                    if (CategoryExists(0, Categoria.Nombre))
                    {
                        Message = "El nombre ingresado para la categoría " + Categoria.Nombre + ", ya ha sido asignado.";
                        return new JsonResult(new { Message = Message, Category = (object)null, Action = "edit", Icon = "error" });
                    }
                }

                category.Nombre = Categoria.Nombre;
                await this._context.SaveChangesAsync();
                await UserActionsAsync("modificó la información de la categoría " + category.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
                Message = "La categoría ha sido modificada satisfactoriamente.";
            }
            return new JsonResult(new { Message = Message, Category = (object)Categoria, Action = "edit", Icon = "success" });
        }

        public bool CategoryExists(int Id, String Nick)
        {
            if (Id > 0)
            {
                return _context.Categoria.Where(p => p.Id == Id).Any();
            }
            else if (!Nick.Equals(""))
            {
                return _context.Categoria.Where(p => p.Nombre == Nick).Any();
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
                var obj = await this._context.Categoria.FirstOrDefaultAsync(t => t.Id == Id);
                if (obj == null)
                {
                    Message = "Advertencia. El estado del contacto no se puede modificar.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "change", Icon = "warning" });
                }
                obj.Estado = (sbyte)Status;
                this._context.Attach(obj).State = EntityState.Modified;
                await this._context.SaveChangesAsync();
                await UserActionsAsync("modificó el estado de la categoría " + obj.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                if (obj.Estado == 1)
                {

                    Message = "La categoría " + obj.Nombre + " ha sido habilitada.";
                }
                else
                {

                    Message = "La categoría " + obj.Nombre + " ha sido deshabilitada.";
                }
                return new JsonResult(new { Message = Message, Category = (object)obj, Action = "change", Icon = "success" });
            }
            else
            {
                Message = "Advertencia. La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Category = (object)null, Action = "change", Icon = "warning" });
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
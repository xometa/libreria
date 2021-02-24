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
    public class PermissionsModel : PageModel
    {
        [BindProperty]
        public Detallepermisosusuario Permission { get; set; }
        public IList<Detallepermisosusuario> permissionsList { get; set; }
        public IList<Permisos> aux { get; set; }
        public IList<Usuario> usersList { get; set; }
        String Message = "";
        private bookstoreContext _context { get; }
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        public PermissionsModel(bookstoreContext context)
        {
            this._context = context;
            this.UploadImage = new UploadImage(context);
        }

        public async Task<IActionResult> OnGet()
        {
            this.usersList = this._context.Usuario
            .Include(i => i.IdimagenNavigation)
            .Include(e => e.IdempleadoNavigation)
            .ThenInclude(p => p.IdpersonaNavigation)
            .Include(p => p.IdempleadoNavigation.IdcargoNavigation)
            .Where(u => u.Rol == "Empleado" && u.Estado == 1)
            .ToList();

            //obteniendo el id de inicio de sesión del usuario y su rol
            int? idUser = HttpContext.Session.GetInt32("IdUser");
            string rol = HttpContext.Session.GetString("Rol");
            if (idUser != null)
            {
                if (!rol.Equals("Administrador"))
                {
                    task = await UploadImage.accessUser(idUser.Value, "Permisos", "/Users/Permissions");
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

        public async Task<JsonResult> OnGetUserPermissions(int Iduser)
        {
            this.aux = await this._context.Permisos.ToListAsync();
            List<Permisos> userpermissionsList = new List<Permisos>();
            if (Iduser > 0)
            {
                foreach (Permisos p in this.aux)
                {
                    if (!Exists(Iduser, p.Id))
                    {
                        userpermissionsList.Add(p);
                    }

                }
            }
            return new JsonResult(new { List = (object)userpermissionsList });
        }
        public async Task<JsonResult> OnGetUserPermissionsList(int Iduser)
        {
            this.permissionsList = await this._context.Detallepermisosusuario
            .Include(u => u.IdusuarioNavigation)
            .Include(p => p.IdpermisoNavigation)
            .Where(dp => dp.Idusuario == Iduser)
            .ToListAsync();
            return new JsonResult(new { Permissions = (object)this.permissionsList });
        }
        public bool Exists(int Iduser, int Idpermission)
        {
            if (Iduser > 0 && Idpermission > 0)
            {
                return _context.Detallepermisosusuario.Where(u => u.Idusuario == Iduser && u.Idpermiso == Idpermission).Any();
            }
            else
            {
                return false;
            }
        }

        public async Task<JsonResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Permission = (object)null, Action = "save", Icon = "error" });

            }
            if (this.Permission.Idpermiso < 1 || this.Permission.Idusuario < 1)
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Permission = (object)null, Action = "save", Icon = "error" });
            }
            var p = await this._context.Permisos.FirstOrDefaultAsync(pr => pr.Id == Permission.Idpermiso);
            var u = await this._context.Usuario
            .Include(e => e.IdempleadoNavigation)
            .ThenInclude(p => p.IdpersonaNavigation)
            .FirstOrDefaultAsync(u => u.Id == Permission.Idusuario);
            this._context.Detallepermisosusuario.Add(Permission);
            await this._context.SaveChangesAsync();
            await UserActionsAsync("agrego el permiso " + p.Nombre + ", para el usuario " + u.IdempleadoNavigation.IdpersonaNavigation.Fullname());
            Message = "EL permiso " + p.Nombre + ", se ha agregado correctamente al usuario " + u.IdempleadoNavigation.IdpersonaNavigation.Fullname() + ".";
            return new JsonResult(new { Message = Message, Permission = (object)Permission, Action = "save", Icon = "success" });
        }

        public async Task<JsonResult> OnPostDelete(int Iduser, int Idpermission)
        {
            if (Iduser < 1 || Idpermission < 1)
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Permission = (object)null, Action = "delete", Icon = "error" });
            }
            var permissionDetail = await this._context.Detallepermisosusuario
            .Include(p => p.IdpermisoNavigation)
            .Include(u => u.IdusuarioNavigation)
            .ThenInclude(e => e.IdempleadoNavigation)
            .ThenInclude(p => p.IdpersonaNavigation)
            .FirstOrDefaultAsync(dpu => dpu.Idusuario == Iduser && dpu.Idpermiso == Idpermission);
            if (permissionDetail == null)
            {
                Message = "El permiso a eliminar no existe en los registros.";
                return new JsonResult(new { Message = Message, Permission = (object)null, Action = "delete", Icon = "warning" });
            }
            this._context.Detallepermisosusuario.Remove(permissionDetail);
            await this._context.SaveChangesAsync();
            await UserActionsAsync("eliminó el permiso " + permissionDetail.IdpermisoNavigation.Nombre + " del usuario " + permissionDetail.IdusuarioNavigation.IdempleadoNavigation.IdpersonaNavigation.Fullname());
            Message = "El permiso otorgado al usuario " + permissionDetail.IdusuarioNavigation.IdempleadoNavigation.IdpersonaNavigation.Fullname() + ", se ha eliminado correctamente.";
            return new JsonResult(new { Message = Message, Permission = (object)permissionDetail, Action = "delete", Icon = "success" });
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
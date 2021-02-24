using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;
using RosaMaríaBookstore.Props;

namespace RosaMaríaBookstore.Pages.Queries.Employed
{
    public class WithPermissionModel : PageModel
    {
        public IList<Usuario> UserList { get; set; }
        public IList<Detallepermisosusuario> permissionsList {get;set;}
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public WithPermissionModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }
        public void OnGet()
        {
            this.UserList = this._context.Usuario
            .Include(u => u.IdempleadoNavigation)
                .ThenInclude(e => e.IdpersonaNavigation)
            .Where(u => u.Detallepermisosusuario.Count > 0)
            .ToList();
        }
        public async Task<PartialViewResult> OnGetWithPermission(int id, int? quantity, int? currentPage)
        {
            IQueryable<Usuario> Query = this._context.Usuario
            .Include(u => u.IdempleadoNavigation)
                .ThenInclude(e => e.IdpersonaNavigation)
            .Include(u => u.Detallepermisosusuario)
                .ThenInclude(u => u.IdpermisoNavigation)
            .Where(u => u.Detallepermisosusuario.Count > 0)
            .OrderByDescending(e => e.Id);

            if (quantity == null)
            {
                quantity = this.pageSize;
                this.pageSize = quantity.Value;
            }
            else
            {
                this.pageSize = quantity.Value;
            }

            if (currentPage == null)
            {
                currentPage = this.currentPage;
            }
            else
            {
                this.currentPage = currentPage.Value;
            }

            if (id > 0)
            {
                Query = Query.Where(u => u.Id == id);
            }

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.UserList = await Query.ToListAsync();
            this.showRegisters = this.UserList.Count;

            foreach (var u in this.UserList)
            {
                if (u.IdempleadoNavigation.Idcargo != null)
                {
                    u.IdempleadoNavigation.IdcargoNavigation = this._context.Cargo
                    .FirstOrDefault(c => c.Id == u.IdempleadoNavigation.Idcargo);
                }

                if (u.IdempleadoNavigation.Idtelefono != null)
                {
                    u.IdempleadoNavigation.IdtelefonoNavigation = this._context.Telefono
                    .FirstOrDefault(t => t.Id == u.IdempleadoNavigation.Idtelefono);
                }
            }
            return Partial("_WithPermissions", this);
        }

        public async Task<JsonResult> OnGetUserPermissionsList(int Iduser){
            this.permissionsList=await this._context.Detallepermisosusuario
            .Include(u=>u.IdusuarioNavigation)
            .Include(p=>p.IdpermisoNavigation)
            .Where(dp=>dp.Idusuario==Iduser)
            .ToListAsync();
            return new JsonResult(new { Permissions = (object)this.permissionsList });
        }
    }
}
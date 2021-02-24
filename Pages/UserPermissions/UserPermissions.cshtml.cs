using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.UserPermissions
{
    public class UserPermissionsModel:PageModel
    {
        private bookstoreContext _context { get; }
        public UserPermissionsModel(bookstoreContext context)
        {
            this._context = context;
        }
        public async Task<JsonResult> OnPostUserPermissions(string[] modules,string[] icons)
        {
            if (modules.Length<16 && icons.Length<16)
            {
                return new JsonResult(new { Count = 0 });
            }
            Permisos permission;
            int k=0;
            for (int i = 0; i < modules.Length; i++)
            {
                permission=new Permisos();
                permission.Nombre=modules[i];
                permission.Icono=icons[i];
                if (!PermissionExists(permission.Nombre))
                {
                    this._context.Permisos.Add(permission);
                    await this._context.SaveChangesAsync();
                }
                k++;
            }
            return new JsonResult(new { Count = k });
        } 

        public bool PermissionExists(String Name)
        {
             if (!Name.Equals(""))
            {
                return _context.Permisos.Where(p => p.Nombre == Name).Any();
            }
            else
            {
                return false;
            }
        }
    }
}
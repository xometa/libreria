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

namespace RosaMaríaBookstore.Pages.RecentActivity
{
    public class TimeLineModel : PageModel
    {
        public IList<UserActions> UserActionsList { get; set; }
        public IList<Usuario> UsersList { get; set; }
        private bookstoreContext _context { get; }
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        public TimeLineModel(bookstoreContext _context)
        {
            this._context = _context;
            this.UploadImage = new UploadImage(_context);
        }

        public async Task<IActionResult> OnGet()
        {
            this.UsersList = this._context.Usuario
            .Include(e => e.IdempleadoNavigation)
            .ThenInclude(p => p.IdpersonaNavigation).ToList();

            //obteniendo el id de inicio de sesión del usuario y su rol
            int? idUser = HttpContext.Session.GetInt32("IdUser");
            string rol = HttpContext.Session.GetString("Rol");
            if (idUser != null)
            {
                if (!rol.Equals("Administrador"))
                {
                    task = await UploadImage.accessUser(idUser.Value, "Bitácora", "/RecentActivity/TimeLine");
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
        public PartialViewResult OnGetTimeLine(int IdEmployed, DateTime HomeDate, DateTime EndDate)
        {
            string ftfc = "yyyy-MM-dd";
            object[] parameters = new object[] { HomeDate.ToString(ftfc), EndDate.ToString(ftfc), IdEmployed };
            this.UserActionsList = this._context.UserActions.FromSqlRaw(UserActions.sqlActions(), parameters)
            .ToList();
            return Partial("_BinnaclePartial", this);
        }
    }
}
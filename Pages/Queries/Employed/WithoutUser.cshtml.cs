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
    public class WithoutUserModel : PageModel
    {
        public IList<Empleado> EmplployedList { get; set; }
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public WithoutUserModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }
        public async Task<PartialViewResult> OnGetNotUser(string search, int? quantity, int? currentPage)
        {
            IQueryable<Empleado> Query = this._context.Empleado
            .Include(e => e.IdcargoNavigation)
            .Include(e => e.IdpersonaNavigation)
            .Include(e => e.IdtelefonoNavigation)
            .Include(e => e.Usuario)
            .Where(e => e.Usuario.Count == 0)
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

            Query = Query
            .Where(e => EF.Functions.Like(e.IdcargoNavigation.Nombre, $"%{search}%") ||
            EF.Functions.Like(e.IdpersonaNavigation.Nombre, $"%{search}%") ||
            EF.Functions.Like(e.IdpersonaNavigation.Apellido, $"%{search}%") ||
            EF.Functions.Like(e.IdpersonaNavigation.Dui, $"%{search}%") ||
            EF.Functions.Like(e.Sexo, $"%{search}%") ||
            EF.Functions.Like(e.IdtelefonoNavigation.Telefono1, $"%{search}%"));

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.EmplployedList = await Query.ToListAsync();
            this.showRegisters = this.EmplployedList.Count;
            return Partial("_NotUser", this);
        }
    }
}
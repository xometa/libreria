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
    public class EmployedAjaxModel : PageModel
    {
        public IList<EmployedQuery> EmplployedList { get; set; }
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public EmployedAjaxModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }

        public IActionResult OnGet(string search, int? quantity, int? currentPage)
        {
            int Iduser = HttpContext.Session.GetInt32("IdUser").Value;
            var user = this._context.Usuario.FirstOrDefault(u => u.Id == Iduser);
            IQueryable<EmployedQuery> Query = this._context.EmployedQuery
            .FromSqlRaw(EmployedQuery.sqlEmployed(), user.Idempleado)
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
            .Where(e => EF.Functions.Like(e.Nombre, $"%{search}%") ||
            EF.Functions.Like(e.DUI, $"%{search}%") ||
            EF.Functions.Like(e.Sexo, $"%{search}%") ||
            EF.Functions.Like(e.Fechanacimiento, $"%{search}%") ||
            EF.Functions.Like(e.Telefono, $"%{search}%") ||
            EF.Functions.Like(e.Cargo, $"%{search}%"));

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.EmplployedList = Query.ToList();
            this.showRegisters = this.EmplployedList.Count;

            return Page();
        }
    }
}
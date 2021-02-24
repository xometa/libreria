using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Users
{

    public class PositionsAjaxModel : PageModel
    {
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public IList<Cargo> PositionsList { get; set; }
        public PositionsAjaxModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 12;
            this.total = 0;
        }
        public IActionResult OnGet(string search, int? quantity, int? currentPage)
        {
            IQueryable<Cargo> Query = this._context.Cargo
            .OrderByDescending(c => c.Id);

            //inicializando las variables de la paginación
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

            //filtros de búsqueda
            Query = Query
            .Where(c => EF.Functions.Like(c.Nombre, $"%{search}%"));

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.PositionsList = Query.ToList();
            this.showRegisters = this.PositionsList.Count;
            return Page();
        }
    }
}
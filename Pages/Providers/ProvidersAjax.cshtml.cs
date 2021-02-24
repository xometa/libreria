using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Providers
{

    public class ProvidersAjaxModel : PageModel
    {
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public IList<Proveedor> ProvidersList { get; set; }
        public ProvidersAjaxModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }
        public IActionResult OnGet(string search, int? quantity, int? currentPage)
        {
            IQueryable<Proveedor> Query = this._context.Proveedor
            .Include(p => p.IdrepresentanteNavigation)
            .OrderByDescending(p => p.Id);

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
            .Where(p => EF.Functions.Like(p.Nombre, $"%{search}%") ||
            EF.Functions.Like(p.Direccion, $"%{search}%") ||
            EF.Functions.Like(p.IdrepresentanteNavigation.Nombre, $"%{search}%") ||
            EF.Functions.Like(p.IdrepresentanteNavigation.Apellido, $"%{search}%") ||
            EF.Functions.Like(p.IdrepresentanteNavigation.Dui, $"%{search}%"));

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.ProvidersList = Query.ToList();
            this.showRegisters = this.ProvidersList.Count;
            return Page();
        }
    }
}
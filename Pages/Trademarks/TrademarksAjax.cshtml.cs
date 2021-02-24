using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Trademarks
{

    public class TrademarksAjaxModel : PageModel
    {
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public IList<Marca> TrademarksList { get; set; }
        public TrademarksAjaxModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 8;
            this.total = 0;
        }
        public IActionResult OnGet(string search, int? quantity, int? currentPage)
        {
            IQueryable<Marca> Query = this._context.Marca
            .OrderByDescending(m => m.Id);

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
            .Where(m => EF.Functions.Like(m.Nombre, $"%{search}%"));

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.TrademarksList = Query.ToList();
            this.showRegisters = this.TrademarksList.Count;
            return Page();
        }
    }
}
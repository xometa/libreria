using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Queries.Shoppings
{

    public class ShoppingsQueryAjaxModel : PageModel
    {
        public IList<Compra> ShoppingsList { get; set; }
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public ShoppingsQueryAjaxModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }
        public async Task<IActionResult> OnGet(int idprovider, string date, string typeShopping, int? quantity, int? currentPage)
        {
            IQueryable<Compra> Query = this._context.Compra
            .Include(p => p.IdproveedorNavigation)
                .ThenInclude(r => r.IdrepresentanteNavigation)
            .Include(u => u.IdusuarioNavigation)
                .ThenInclude(u => u.IdempleadoNavigation)
                    .ThenInclude(u => u.IdpersonaNavigation)
            .Include(dc => dc.Detallecompra)
                .ThenInclude(p => p.IdproductoNavigation)
                    .ThenInclude(i => i.IdimagenNavigation)
            .OrderByDescending(c => c.Id);

            if (idprovider>0)
            {
                Query=Query.Where(c => c.Idproveedor == idprovider);
            }

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

            if (date != null && typeShopping != null)
            {
                Query = Query
                .Where(v => v.Fecha == Convert.ToDateTime(date) && v.Tipo == typeShopping);
            }
            else
            {
                if (date != null)
                {
                    Query = Query
                    .Where(v => v.Fecha == Convert.ToDateTime(date));
                }
                else if (typeShopping != null)
                {
                    Query = Query
                    .Where(v => v.Tipo == typeShopping);
                }
            }

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.ShoppingsList = await Query.ToListAsync();
            this.showRegisters = this.ShoppingsList.Count;

            return Page();
        }
    }
}
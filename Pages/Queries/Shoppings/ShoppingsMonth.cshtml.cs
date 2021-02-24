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
    public class ShoppingsMonthModel : PageModel
    {
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public IList<Proveedor> ProvidersList { get; set; }
        public IList<Compra> ShoppingsList { get; set; }
        private bookstoreContext _context { get; }
        public ShoppingsMonthModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }

        public void OnGet()
        {
            this.ProvidersList = this._context.Proveedor
            .Include(p => p.IdrepresentanteNavigation)
            .ToList();
        }

        public async Task<PartialViewResult> OnGetMonthYear(int idprovider, int month, int? quantity, int? currentPage)
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

            if (idprovider > 0 && month > 0)
            {
                Query = Query
                .Where(v => v.Idproveedor == idprovider
                && v.Fecha.Month == month
                && v.Fecha.Year == DateTime.Now.Year);
            }
            else
            {
                if (idprovider > 0)
                {
                    Query = Query
                    .Where(v => v.Idproveedor == idprovider
                    && v.Fecha.Year == DateTime.Now.Year);
                }
                else if (month > 0)
                {
                    Query = Query
                    .Where(v => v.Fecha.Month == month
                    && v.Fecha.Year == DateTime.Now.Year);
                }
                else
                {

                    Query = Query
                    .Where(v => v.Fecha.Year == DateTime.Now.Year);
                }
            }

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.ShoppingsList = await Query.ToListAsync();
            this.showRegisters = this.ShoppingsList.Count;
            return Partial("_ShoppingsMonthYear", this);
        }
    }
}
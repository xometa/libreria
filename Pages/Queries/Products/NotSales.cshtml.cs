using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Queries.Products
{
    public class NotSalesModel : PageModel
    {
        public IList<Preciosproducto> PricesList { get; set; }
        public IList<Producto> Products { get; set; }
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public NotSalesModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }
        public void OnGet()
        {
            this.Products = this._context.Producto.ToList();
        }
        public async Task<PartialViewResult> OnGetNotSales(int idproduct, int? quantity, int? currentPage)
        {
            IQueryable<Preciosproducto> Query = this._context.Preciosproducto
                .Include(c => c.IdcompraNavigation)
                .ThenInclude(dc => dc.Detallecompra)
                .Include(p => p.IdproductoNavigation)
                .Where(p => p.Detalleventa.Count == 0 &&
                p.Idproducto == idproduct);

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

            this.total = Query.Count();
            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.PricesList = await Query.ToListAsync();
            this.showRegisters = this.PricesList.Count;
            return Partial("_ProductsNotSales", this);
        }
    }
}
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
    public class NotShoppingsModel : PageModel
    {
        public IList<Producto> ProductsList { get; set; }
        public IList<Marca> TrademarksList { get; set; }
        public IList<Categoria> CategoriesList { get; set; }
        public IList<Producto> Products { get; set; }
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public NotShoppingsModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }
        public void OnGet()
        {
            this.TrademarksList = this._context.Marca.ToList();
            this.CategoriesList = this._context.Categoria.ToList();
            this.Products = this._context.Producto.ToList();
        }
        public async Task<PartialViewResult> OnGetNotShoppings(int idproduct, string tag, string category, int? quantity, int? currentPage)
        {
            IQueryable<Producto> Query = this._context.Producto
            .Include(m => m.IdmarcaNavigation)
            .Include(c => c.IdcategoriaNavigation)
            .Include(i => i.IdimagenNavigation)
            .Include(dc => dc.Detallecompra)
            .Where(dc => dc.Detallecompra.Count == 0)
            .OrderByDescending(p => p.Id);
            if (idproduct > 0)
            {
                Query = Query.Where(p => p.Id == idproduct);
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

            //filtros de búsqueda
            if (tag != null && category != null)
            {
                Query = Query
                .Where(p => p.IdmarcaNavigation.Nombre == tag ||
                p.IdcategoriaNavigation.Nombre == category);
            }
            else
            {
                if (tag != null)
                {
                    Query = Query.Where(p => p.IdmarcaNavigation.Nombre == tag);
                }
                else if (category != null)
                {
                    Query = Query.Where(p => p.IdcategoriaNavigation.Nombre == category);
                }
            }

            this.total = Query.Count();
            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.ProductsList = await Query.ToListAsync();
            this.showRegisters = this.ProductsList.Count;
            return Partial("_ProductsNotShoppings", this);
        }
    }
}
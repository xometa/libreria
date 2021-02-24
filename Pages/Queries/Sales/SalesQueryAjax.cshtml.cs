using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Queries.Sales
{

    public class SalesQueryAjaxModel : PageModel
    {
        public IList<Venta> SaleList { get; set; }
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public SalesQueryAjaxModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }

        public async Task<IActionResult> OnGet(int idclient, string date, string typeSale, int? quantity, int? currentPage)
        {
            IQueryable<Venta> Query = this._context.Venta
            .Include(p => p.IdclienteNavigation)
                .ThenInclude(p => p.Institucion)
            .Include(u => u.IdusuarioNavigation)
                .ThenInclude(u => u.IdempleadoNavigation)
                    .ThenInclude(u => u.IdpersonaNavigation)
            .Include(dv => dv.Detalleventa)
                .ThenInclude(dv => dv.IdproductoNavigation)
                    .ThenInclude(c => c.IdcompraNavigation)
            .ThenInclude(dc => dc.Detallecompra)
            .OrderByDescending(v => v.Id);

            if (idclient > 0)
            {
                Query = Query.Where(v => v.Idcliente == idclient);
            }

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

            if (date != null && typeSale != null)
            {
                Query = Query
                .Where(v => v.Fecha == Convert.ToDateTime(date) && v.Tipo == typeSale);
            }
            else
            {
                if (date != null)
                {
                    Query = Query
                    .Where(v => v.Fecha == Convert.ToDateTime(date));
                }
                else if (typeSale != null)
                {
                    Query = Query
                    .Where(v => v.Tipo == typeSale);
                }
            }

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.SaleList = Query.ToList();
            this.showRegisters = this.SaleList.Count;

            foreach (var item in this.SaleList)
            {
                foreach (var p in item.Detalleventa)
                {
                    p.IdproductoNavigation.IdproductoNavigation = await this._context.Producto.Include(pr => pr.IdimagenNavigation).Where(pr => pr.Id == p.IdproductoNavigation.Idproducto).FirstOrDefaultAsync();
                }
            }
            return Page();
        }
    }
}
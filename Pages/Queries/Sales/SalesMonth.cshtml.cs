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
    public class SalesMonthModel : PageModel
    {
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public IList<Persona> ClientsList { get; set; }
        public IList<Institucion> InstitutionsList { get; set; }
        public IList<Venta> SaleList { get; set; }
        private bookstoreContext _context { get; }
        public SalesMonthModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }

        public void OnGet()
        {
            this.ClientsList = this._context.Persona.Where(p => p.Tipo == "Público")
            .ToList();
            this.InstitutionsList = this._context.Institucion
            .Include(p => p.IdrepresentanteNavigation)
            .ToList();
        }

        public async Task<PartialViewResult> OnGetMonthYear(int idclient, int month, int? quantity, int? currentPage)
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

            if (idclient > 0 && month > 0)
            {
                Query = Query
                .Where(v => v.Idcliente == idclient
                && v.Fecha.Month == month
                && v.Fecha.Year == DateTime.Now.Year);
            }
            else
            {
                if (idclient > 0)
                {
                    Query = Query
                    .Where(v => v.Idcliente == idclient
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

            this.SaleList = await Query.ToListAsync();
            this.showRegisters = this.SaleList.Count;

            foreach (var item in this.SaleList)
            {
                foreach (var p in item.Detalleventa)
                {
                    p.IdproductoNavigation.IdproductoNavigation = await this._context.Producto.Include(pr => pr.IdimagenNavigation).Where(pr => pr.Id == p.IdproductoNavigation.Idproducto).FirstOrDefaultAsync();
                }
            }
            return Partial("_SalesMonthYear", this);
        }
    }
}
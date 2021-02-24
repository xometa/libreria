using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;
using RosaMaríaBookstore.Props;

namespace RosaMaríaBookstore.Pages.Kardex
{
    public class KardexModel : PageModel
    {
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public IList<KardexQuery> KardexList { get; set; }
        public IList<Producto> ProductList { get; set; }
        public int Existencia { get; set; }
        public KardexModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
            this.Existencia = 0;
        }

        public void OnGet()
        {
            this.ProductList = this._context.Producto
            .Include(dc => dc.Detallecompra)
            .ToList();
        }
        public PartialViewResult OnGetKardexPartialView(int id, string search, int? quantity, int? currentPage)
        {
            object[] parameters = new object[] { id, id };
            IEnumerable<KardexQuery> Query = this._context
            .KardexQuery.FromSqlRaw(KardexQuery.kardex(), parameters)
            .IgnoreQueryFilters().AsNoTracking().AsEnumerable().OrderBy(p => p.Id);
            IList<KardexQuery> ck = this._context.KardexQuery.FromSqlRaw(KardexQuery.stockAvailable(), id, id).ToList();
            if (ck != null && ck.Count > 0)
            {
                if (ck.Count == 2)
                {
                    if (ck[0].Id > ck[1].Id)
                    {
                        Existencia = ck.First().Existencia;
                    }
                    else
                    {
                        Existencia = ck.Last().Existencia;
                    }
                }
                else
                {
                    Existencia = ck.Last().Existencia;
                }
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
            //filtros de búsqueda
            /*Query = Query
             .Where(p => EF.Functions.Like(p.Fecha, $"%{search}%")).AsEnumerable();*/

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.KardexList = Query.ToList();
            this.showRegisters = this.KardexList.Count;

            return Partial("_KardexPartial", this);
        }
    }
}

/*

public PartialViewResult OnGetKardexPartialView(int id, string search, int? quantity, int? currentPage)
        {
            object[] parameters = new object[] { id, id };
            IEnumerable<KardexQuery> Query = this._context
            .KardexQuery.FromSqlRaw(KardexQuery.kardex(), parameters)
            .IgnoreQueryFilters().AsNoTracking().AsEnumerable().OrderBy(p => p.Id);
            IList<KardexQuery> ck = this._context.KardexQuery.FromSqlRaw(KardexQuery.stockAvailable(), parameters).ToList();
            if (ck != null && ck.Count > 0)
            {
                if (ck.Count == 2)
                {
                    if (ck[0].Id > ck[1].Id)
                    {
                        Existencia = ck.First().Existencia;
                    }
                    else
                    {
                        Existencia = ck.Last().Existencia;
                    }
                }
                else
                {
                    Existencia = ck.Last().Existencia;
                }
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
            //filtros de búsqueda
            Query = Query
            .Where(p => EF.Functions.Like(p.Fecha, $"%{search}%"));

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.KardexList = Query.ToList();
            this.showRegisters = this.KardexList.Count;

            return Partial("_KardexPartial", this);
        }
*/
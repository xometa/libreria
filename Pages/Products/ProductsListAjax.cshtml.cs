using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;
using RosaMaríaBookstore.Props;

namespace RosaMaríaBookstore.Pages.Products
{

    public class ProductsListAjaxModel : PageModel
    {
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public IList<ProductsQuery> ProductsList { get; set; }
        public ProductsListAjaxModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 24;
            this.total = 0;
        }
        public async Task<IActionResult> OnGet(string search, int? quantity, int? currentPage)
        {

            //obtenemos la consulta de los productos 
            IQueryable<ProductsQuery> Query = this._context.ProductsQuery
            .FromSqlRaw(ProductsQuery.sqlProductsAvailable())
            .Where(p => p.Estado == (sbyte)1)
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
            .Where(p => EF.Functions.Like(p.Categoria, $"%{search}%"));

            //obteniendo el total de registros filtrados mediante la busqueda
            this.total = Query.Count();

            //limites de la paginación
            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            //los agregamos a la lista, para poder visualizarlos
            this.ProductsList = await Query.ToListAsync();
            this.showRegisters = this.ProductsList.Count;
            return Page();
        }
    }
}
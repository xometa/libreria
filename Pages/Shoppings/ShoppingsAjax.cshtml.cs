using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Shoppings
{
    public class ShoppingsAjaxModel : PageModel
    {
        public IList<Compra> ShoppingsList { get; set; }
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public ShoppingsAjaxModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }

        public async Task<IActionResult> OnGet(string search, int? quantity, int? currentPage)
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
            .Where(c => EF.Functions.Like(c.Documento, $"%{search}%") ||
            EF.Functions.Like(c.IdproveedorNavigation.Nombre, $"%{search}%") ||
            EF.Functions.Like(c.IdproveedorNavigation.IdrepresentanteNavigation.Nombre, $"%{search}%") ||
            EF.Functions.Like(c.IdproveedorNavigation.IdrepresentanteNavigation.Apellido, $"%{search}%") ||
            EF.Functions.Like(c.IdproveedorNavigation.IdrepresentanteNavigation.Dui, $"%{search}%") ||
            EF.Functions.Like(c.Tipo, $"%{search}%"));

            //obteniendo el total de registros filtrados mediante la busqueda
            this.total = Query.Count();

            //limites de la paginación
            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            //los agregamos a la lista, para poder visualizarlos
            this.ShoppingsList = await Query.ToListAsync();
            this.showRegisters = this.ShoppingsList.Count;

            return Page();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Queries.Employed
{
    public class ShoppingsUserModel : PageModel
    {
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public IList<Usuario> UserList { get; set; }
        public IList<Compra> ShoppingsList { get; set; }
        private bookstoreContext _context { get; }
        public ShoppingsUserModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }

        public void OnGet()
        {
            this.UserList = this._context.Usuario
            .Include(u => u.IdempleadoNavigation)
                .ThenInclude(e => e.IdpersonaNavigation)
            .ToList();
        }

        public async Task<PartialViewResult> OnGetShoppingsUser(int iduser, int month, bool current, int? quantity, int? currentPage)
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
            .Where(c => c.Idusuario == iduser)
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

            //filtros
            if (current)
            {
                if (month > 0)
                {
                    Query = Query.Where(c => c.Fecha.Month == month && c.Fecha.Year == DateTime.Now.Year);
                }
                else
                {
                    Query = Query.Where(c => c.Fecha.Year == DateTime.Now.Year);
                }
            }
            else
            {
                if (month > 0)
                {
                    Query = Query.Where(c => c.Fecha.Month == month);
                }
            }

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.ShoppingsList = await Query.ToListAsync();
            this.showRegisters = this.ShoppingsList.Count;
            return Partial("_ShoppingsUser", this);
        }
    }
}
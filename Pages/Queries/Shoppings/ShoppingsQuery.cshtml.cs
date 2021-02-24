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
    public class ShoppingsQueryModel : PageModel
    {
        public IList<Proveedor> ProvidersList { get; set; }
        private bookstoreContext _context { get; }
        public ShoppingsQueryModel(bookstoreContext context)
        {
            this._context = context;
        }

        public void OnGet(){
            this.ProvidersList = this._context.Proveedor
            .Include(p => p.IdrepresentanteNavigation)
            .ToList();
        }
    }
}
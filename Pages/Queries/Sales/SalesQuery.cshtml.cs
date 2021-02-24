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
    public class SalesQueryModel : PageModel
    {
        public IList<Persona> ClientsList { get; set; }
        public IList<Institucion> InstitutionsList { get; set; }
        private bookstoreContext _context { get; }
        public SalesQueryModel(bookstoreContext context)
        {
            this._context = context;
        }
        public void OnGet()
        {
            this.ClientsList = this._context.Persona.Where(p => p.Tipo == "Público")
            .ToList();
            this.InstitutionsList = this._context.Institucion
            .Include(p => p.IdrepresentanteNavigation)
            .ToList();
        }
    }
}
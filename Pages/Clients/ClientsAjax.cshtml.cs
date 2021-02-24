using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Clients
{

    public class ClientsAjaxModel : PageModel
    {
        private bookstoreContext _context { get; }
        public IList<Persona> ClientsList { get; set; }
        public IList<Institucion> InstitutionsList { get; set; }
        public String Option { get; set; }
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public ClientsAjaxModel(bookstoreContext context)
        {
            this._context = context;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
        }
        public IActionResult OnGet(String Option, string search, int? quantity, int? currentPage)
        {
            this.Option = Option;
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

            if (Option.Equals("Público"))
            {

                IQueryable<Persona> Query = this._context.Persona
                .Where(p => p.Tipo == Option)
                .OrderByDescending(p => p.Id);
                //filtros de búsqueda
                Query = Query
                .Where(p => EF.Functions.Like(p.Nombre, $"%{search}%") ||
                EF.Functions.Like(p.Apellido, $"%{search}%") ||
                EF.Functions.Like(p.Dui, $"%{search}%"));

                //obteniendo el total de registros filtrados mediante la busqueda
                this.total = Query.Count();

                //limites de la paginación
                Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

                //los agregamos a la lista, para poder visualizarlos
                this.ClientsList = Query.ToList();
                this.showRegisters = this.ClientsList.Count;
            }
            else
            {
                IQueryable<Institucion> Query = this._context.Institucion
               .Include(p => p.IdrepresentanteNavigation)
                .OrderByDescending(i => i.Id);
                //filtros de búsqueda
                Query = Query
                .Where(p => EF.Functions.Like(p.IdrepresentanteNavigation.Nombre, $"%{search}%") ||
                EF.Functions.Like(p.IdrepresentanteNavigation.Apellido, $"%{search}%") ||
                EF.Functions.Like(p.IdrepresentanteNavigation.Dui, $"%{search}%") ||
                EF.Functions.Like(p.Nombre, $"%{search}%") ||
                EF.Functions.Like(p.Direccion, $"%{search}%"));

                //obteniendo el total de registros filtrados mediante la busqueda
                this.total = Query.Count();

                //limites de la paginación
                Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

                //los agregamos a la lista, para poder visualizarlos
                this.InstitutionsList = Query.ToList();
                this.showRegisters = this.InstitutionsList.Count;
            }
            return Page();
        }
    }
}
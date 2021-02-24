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

    public class InventoryProductsAjaxModel : PageModel
    {
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public List<ProductsQuery> ProductsList { get; set; }
        public IList<ProductsQuery> Aux { get; set; }
        public IList<ProductsQuery> List { get; set; }
        public InventoryProductsAjaxModel(bookstoreContext context)
        {
            this._context = context;
            this.ProductsList = new List<ProductsQuery>();
            this.currentPage = 1;
            this.pageSize = 12;
            this.total = 0;
        }
        public async Task<IActionResult> OnGet(string search, string category, int? quantity, int? currentPage)
        {
            this.Aux = this._context.ProductsQuery
            .FromSqlRaw(ProductsQuery.sqlInventoryProducts())
            .OrderByDescending(p => p.Id)
            .ToList();
            //se puede haber realizado compras de un producto, pero si este no tiene precios de venta
            //no se podra vender, es por ello que se verifica que este posea al menos un precio de venta,
            //de igual forma se verifica que el estado del producto sea habilitado como el de los precios.
            foreach (ProductsQuery pq in Aux)
            {
                var producto = await this._context.Producto.Include(p => p.Preciosproducto).FirstOrDefaultAsync(p => p.Id == pq.Id);
                if (producto.Preciosproducto.Count > 0 && producto.Estado == 1)
                {
                    //contador
                    int i = 0;
                    //verificamos que tenga al menos un precio activo, para la venta
                    foreach (Preciosproducto pp in producto.Preciosproducto)
                    {
                        //si el estado es uno, incrementara el contador
                        if (pp.Estado == 1)
                        {
                            i++;
                        }
                    }
                    //si el contador es mayor cero, quiere decir que posee precios activos
                    //entonces se procede a agregarlo a la lista
                    if (i > 0)
                    {
                        this.ProductsList.Add(pq);
                    }
                }
            }

            IQueryable<ProductsQuery> Query = this.ProductsList.AsQueryable<ProductsQuery>();

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
            if (category != null)
            {
                Query = Query
                .Where(p => p.Categoria == category);
            }
            else
            {
                Query = Query
                .Where(p => EF.Functions.Like(p.Categoria, $"%{search}%") ||
                EF.Functions.Like(p.Marca, $"%{search}%") ||
                EF.Functions.Like(p.Nombre, $"%{search}%") ||
                EF.Functions.Like(p.Descripcion, $"%{search}%") ||
                EF.Functions.Like(p.Existencia, $"%{search}%"));
            }

            //obteniendo el total de registros filtrados mediante la busqueda
            this.total = Query.Count();

            //limites de la paginación
            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            //los agregamos a la lista, para poder visualizarlos
            this.List = Query.ToList();
            this.showRegisters = this.List.Count;
            return Page();
        }
    }
}
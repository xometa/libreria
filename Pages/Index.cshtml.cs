using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RosaMaríaBookstore.Models;
using RosaMaríaBookstore.Props;

namespace RosaMaríaBookstore.Pages
{
    public class IndexModel : PageModel
    {
        public IList<UserActions> UserActionsList { get; set; }
        public IList<Institucion> InstitutionsList { get; set; }
        public IList<Usuario> UsersList { get; set; }
        public IList<Venta> SalesList { get; set; }
        public IList<Compra> ShoppingsList { get; set; }
        public IList<SaleDayProducts> DayProducts { get; set; }
        public IList<Revenue> Revenues { get; set; }
        public int countSales { get; set; }
        public int countShoppings { get; set; }
        private readonly ILogger<IndexModel> _logger;
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public decimal cWeek { get; set; }
        public decimal lWeek { get; set; }
        private bookstoreContext _context { get; }
        public string page { get; set; }
        public bool task { get; set; }
        public UploadImage UploadImage { get; set; }
        public IndexModel(ILogger<IndexModel> logger, bookstoreContext context)
        {
            _logger = logger;
            this._context = context;
            this.countSales = 0;
            this.countShoppings = 0;
            this.currentPage = 1;
            this.pageSize = 4;
            this.total = 0;
            this.cWeek = 0;
            this.lWeek = 0;
            this.total = 0;
            this.UploadImage = new UploadImage(context);
        }

        public async Task<IActionResult> OnGet()
        {
            string ftfc = "yyyy-MM-dd";
            object[] parameters = new object[] { DateTime.Now.ToString(ftfc), DateTime.Now.ToString(ftfc), 0 };
            this.UserActionsList = this._context.UserActions.FromSqlRaw(UserActions.sqlActions(), parameters)
                .ToList();
            this.InstitutionsList = this._context.Institucion.ToList();
            this.UsersList = this._context.Usuario.ToList();
            this.SalesList = this._context.Venta.ToList();
            this.ShoppingsList = this._context.Compra.ToList();
            foreach (var sale in this.SalesList)
            {
                if (sale.Fecha.ToString(ftfc) == DateTime.Now.ToString(ftfc))
                {
                    this.countSales += 1;
                }
            }
            foreach (var shopping in this.ShoppingsList)
            {
                if (shopping.Fecha.ToString(ftfc) == DateTime.Now.ToString(ftfc))
                {
                    this.countShoppings += 1;
                }
            }

            //obteniendo los totales de los ingresos de la semana actual con la anterior

            decimal totalSale = 0;
            double iva = 0;
            this.Revenues = this._context.Revenue.FromSqlRaw(Revenue.currentWeek()).ToList();
            foreach (var current in this.Revenues)
            {
                totalSale = current.Ingresos;
                iva = Convert.ToDouble(totalSale) * 0.13;
                this.cWeek += totalSale + Convert.ToDecimal(iva);
            }

            this.Revenues = this._context.Revenue.FromSqlRaw(Revenue.lastWeek()).ToList();
            foreach (var current in this.Revenues)
            {
                totalSale = current.Ingresos;
                iva = Convert.ToDouble(totalSale) * 0.13;
                this.lWeek += totalSale + Convert.ToDecimal(iva);
            }

            //obteniendo el id de inicio de sesión del usuario y su rol
            int? idUser = HttpContext.Session.GetInt32("IdUser");
            string rol = HttpContext.Session.GetString("Rol");
            if (idUser != null)
            {
                if (!rol.Equals("Administrador"))
                {
                    task = await UploadImage.accessUser(idUser.Value, "Inicio", "/Index");
                    if (task)
                    {
                        return Page();
                    }
                    else
                    {
                        //si hay un empleado con usuario y este no posee permisos,
                        //se debera sacar del sistema
                        page = UploadImage.redirect;
                        return RedirectToPage(page);
                    }
                }
                else
                {
                    return Page();
                }
            }
            else
            {
                page = "/Login/Login";
                return RedirectToPage(page);
            }
        }
        public async Task<PartialViewResult> OnGetProductsDay(string search, int? quantity, int? currentPage)
        {
            IQueryable<SaleDayProducts> Query = this._context.SaleDayProducts
            .FromSqlRaw(SaleDayProducts.dayProduct());

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
            .Where(dp => EF.Functions.Like(dp.Producto, $"%{search}%") ||
            EF.Functions.Like(dp.Categoria, $"%{search}%") ||
            EF.Functions.Like(dp.Cantidad, $"%{search}%") ||
            EF.Functions.Like(dp.PrecioVenta, $"%{search}%"))
            .OrderByDescending(p => p.Id);

            this.total = Query.Count();

            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            this.DayProducts = await Query.ToListAsync();
            this.showRegisters = this.DayProducts.Count;

            return Partial("_DayProductsPartial", this);
        }

        public async Task<JsonResult> OnGetWeekDay()
        {
            IList<Revenue> lw = await this._context.Revenue.FromSqlRaw(Revenue.lastWeekRevenue()).ToListAsync();
            IList<Revenue> cw = await this._context.Revenue.FromSqlRaw(Revenue.currentWeekRevenue()).ToListAsync();
            return new JsonResult(new { cWeekDay = (object)cw, lWeekDay = (object)lw });

        }
    }
}

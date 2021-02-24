using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;
using RosaMaríaBookstore.Props;

namespace RosaMaríaBookstore.Pages.Sales
{
    public class SalesListModel : PageModel
    {
        public IList<Pago> PaymentsList { get; set; }
        public Venta SaleDetails { get; set; }
        private bookstoreContext _context { get; }
        //para el detalle
        public decimal totalSale { get; set; }
        public decimal priceSale { get; set; }
        public decimal subTotalSale { get; set; }
        public decimal totalRow { get; set; }
        public double ivaSale { get; set; }
        //para la diferencia
        public decimal totalPayments { get; set; }
        public decimal restante { get; set; }
        //Estado de la venta
        public int Status { get; set; }
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        public SalesListModel(bookstoreContext context)
        {
            this._context = context;
            this.totalSale = 0;
            this.priceSale = 0;
            this.subTotalSale = 0;
            this.totalRow = 0;
            this.ivaSale = 0;
            this.totalPayments = 0;
            this.restante = 0;
            this.Status = 0;
            this.currentPage = 1;
            this.pageSize = 3;
            this.total = 0;
            this.UploadImage = new UploadImage(context);
        }

        public async Task<IActionResult> OnGet()
        {
            //obteniendo el id de inicio de sesión del usuario y su rol
            int? idUser = HttpContext.Session.GetInt32("IdUser");
            string rol = HttpContext.Session.GetString("Rol");
            if (idUser != null)
            {
                if (!rol.Equals("Administrador"))
                {
                    task = await UploadImage.accessUser(idUser.Value, "Ventas", "/Sales/SalesList");
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

        public async Task<PartialViewResult> OnGetPayments(int IdSale, string date, int? quantity, int? currentPage)
        {
            if (IdSale > 0)
            {
                //recuperamos el listado de los pagos
                this.PaymentsList = await this._context.Pago
                .Include(a => a.IdabonoNavigation)
                .Where(p => p.Idventa == IdSale)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

                //recuperamos el detalle de la venta, para obtener el total restante de deuda
                this.SaleDetails = await this._context.Venta
                .Include(p => p.IdclienteNavigation)
                    .ThenInclude(p => p.Institucion)
                .Include(u => u.IdusuarioNavigation)
                    .ThenInclude(u => u.IdempleadoNavigation)
                        .ThenInclude(u => u.IdpersonaNavigation)
                .Include(dv => dv.Detalleventa)
                    .ThenInclude(dv => dv.IdproductoNavigation)
                        .ThenInclude(c => c.IdcompraNavigation)
                            .ThenInclude(dc => dc.Detallecompra)
                .FirstOrDefaultAsync(v => v.Id == IdSale);
                //agregamos el producto al objeto de la clase precios de producto
                foreach (var p in SaleDetails.Detalleventa)
                {
                    p.IdproductoNavigation.IdproductoNavigation = await this._context.Producto.Include(pr => pr.IdimagenNavigation).Where(pr => pr.Id == p.IdproductoNavigation.Idproducto).FirstOrDefaultAsync();
                }

                //sacando el total de la venta
                foreach (var p in SaleDetails.Detalleventa)
                {
                    foreach (var dc in p.IdproductoNavigation.IdcompraNavigation.Detallecompra)
                    {
                        if (dc.Idcompra == p.IdproductoNavigation.Idcompra && p.IdproductoNavigation.IdproductoNavigation.Id == dc.Idproducto)
                        {
                            priceSale = dc.Precio;
                            priceSale = (priceSale + (priceSale * (p.IdproductoNavigation.Margen / 100)));
                            subTotalSale = p.Cantidad * Convert.ToDecimal(priceSale.ToString("0.00"));
                            totalSale += subTotalSale;
                        }
                    }
                }
                ivaSale = Convert.ToDouble(totalSale) * 0.13;
                //total de la venta más iva
                totalRow = totalSale + Convert.ToDecimal(ivaSale);

                //obteniendo la sumatoria total de los abonos realizados
                foreach (var payment in this.PaymentsList)
                {
                    totalPayments += payment.IdabonoNavigation.Monto;
                }
                //obteniendo la diferencia (deuda aún sin cancelar)
                restante = totalRow - totalPayments;
                if (Convert.ToDecimal(restante.ToString("0.00")) > 0)
                {
                    Status = 1;
                }

                //creando la paginación
                IQueryable<Pago> Query = this._context.Pago
                .Include(a => a.IdabonoNavigation)
                .Where(p => p.Idventa == IdSale)
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
                .Where(v => EF.Functions.Like(v.IdabonoNavigation.Fechaabono, $"%{date}%"))
                .OrderByDescending(v => v.Id);

                this.total = Query.Count();

                Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

                this.PaymentsList = Query.ToList();
                this.showRegisters = this.PaymentsList.Count;
            }
            return Partial("_ModalPaymentsPartial", this);
        }
    }
}
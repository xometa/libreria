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

namespace RosaMaríaBookstore.Pages.Payments
{
    public class PaymentsModel : PageModel
    {
        [BindProperty]
        public Abono Payment { get; set; }
        //para el detalle
        public decimal totalSale { get; set; }
        public decimal priceSale { get; set; }
        public decimal subTotalSale { get; set; }
        public decimal totalRow { get; set; }
        public double ivaSale { get; set; }
        //para la diferencia
        public decimal totalPayments { get; set; }
        //para el listado de abonos
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public int notFilters { get; set; }
        //para la paginacion de los abonos realizados
        public int showRegistersP { get; set; }
        public int currentPageP { get; set; }
        public int pageSizeP { get; set; }
        public int totalP { get; set; }
        //para saber cuanto debe el cliente
        public int daysNotPay { get; set; }
        public DateTime lastDate { get; set; }
        public decimal restante { get; set; }
        public IList<Persona> ClientsList { get; set; }
        public IList<Institucion> InstitutionsList { get; set; }
        public IList<Venta> Sales { get; set; }
        public IList<Pago> PaymentsList { get; set; }
        public Venta SaleDetails { get; set; }
        private bookstoreContext _context { get; }
        string Message = "";
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        public PaymentsModel(bookstoreContext context)
        {
            this._context = context;
            this.UploadImage = new UploadImage(context);
            this.totalSale = 0;
            this.priceSale = 0;
            this.subTotalSale = 0;
            this.totalRow = 0;
            this.ivaSale = 0;
            this.currentPage = 1;
            this.pageSize = 6;
            this.total = 0;
            this.currentPageP = 1;
            this.pageSizeP = 3;
            this.totalP = 0;
            this.daysNotPay = 0;
            this.lastDate = DateTime.Now;
            this.restante = 0;
            this.notFilters = 0;
        }
        public async Task<IActionResult> OnGet()
        {
            this.ClientsList = this._context.Persona.Where(p => p.Tipo == "Público")
            .ToList();
            this.InstitutionsList = this._context.Institucion
            .Include(p => p.IdrepresentanteNavigation)
            .ToList();

            //obteniendo el id de inicio de sesión del usuario y su rol
            int? idUser = HttpContext.Session.GetInt32("IdUser");
            string rol = HttpContext.Session.GetString("Rol");
            if (idUser != null)
            {
                if (!rol.Equals("Administrador"))
                {
                    task = await UploadImage.accessUser(idUser.Value, "Abonos", "/Payments/Payments");
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
        public async Task<PartialViewResult> OnGetSalesCredit(int id, string home, string end, int? quantity, int? currentPage)
        {
            IQueryable<Venta> Query = this._context.Venta
            .Include(p => p.IdclienteNavigation)
                .ThenInclude(p => p.Institucion)
            .Include(u => u.IdusuarioNavigation)
                .ThenInclude(u => u.IdempleadoNavigation)
                    .ThenInclude(u => u.IdpersonaNavigation)
            .Include(dv => dv.Detalleventa)
                .ThenInclude(dv => dv.IdproductoNavigation)
                    .ThenInclude(c => c.IdcompraNavigation)
                        .ThenInclude(dc => dc.Detallecompra)
            .Where(v => v.Tipo == "Crédito" && v.Estado == 1)
            .OrderByDescending(v => v.Id);

            if (id > 0)
            {
                Query = Query.Where(v => v.Idcliente == id);
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
            if (home != null && end != null)
            {
                string aux = end;
                int result = DateTime.Compare(Convert.ToDateTime(home), Convert.ToDateTime(end));
                if (result > 0)
                {
                    end = home;
                    home = aux;
                }
                Query = Query
                .Where(v => v.Fecha >= Convert.ToDateTime(home) && v.Fecha <= Convert.ToDateTime(end));
            }
            else
            {
                if (home != null)
                {
                    Query = Query
                    .Where(v => v.Fecha == Convert.ToDateTime(home));
                }
                else if (end != null)
                {
                    Query = Query
                    .Where(v => v.Fecha == Convert.ToDateTime(end));
                }
            }

            //obteniendo el total de registros filtrados mediante la busqueda
            this.total = Query.Count();

            //limites de la paginación
            Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

            //los agregamos a la lista, para poder visualizarlos
            this.Sales = Query.ToList();
            this.showRegisters = this.Sales.Count;

            //agregamos el producto al objeto de la clase precios de producto
            foreach (var item in this.Sales)
            {
                foreach (var p in item.Detalleventa)
                {
                    p.IdproductoNavigation.IdproductoNavigation = await this._context.Producto.Include(pr => pr.IdimagenNavigation).Where(pr => pr.Id == p.IdproductoNavigation.Idproducto).FirstOrDefaultAsync();
                }
            }

            return Partial("_ClientsSalesCredit", this);
        }

        public async Task<PartialViewResult> OnGetPayments(int IdSale, string date, int? quantityPayment, int? currentPagePayment)
        {
            if (IdSale > 0)
            {
                this.restante = await restanteSale(IdSale);

                //creando la paginación
                IQueryable<Pago> Query = this._context.Pago
                .Include(a => a.IdabonoNavigation)
                .Where(p => p.Idventa == IdSale)
                .OrderByDescending(p => p.Id);

                //inicializando las variables de la paginación
                if (quantityPayment == null)
                {
                    quantityPayment = this.pageSizeP;
                    this.pageSizeP = quantityPayment.Value;
                }
                else
                {
                    this.pageSizeP = quantityPayment.Value;
                }

                if (currentPagePayment == null)
                {
                    currentPagePayment = this.currentPageP;
                }
                else
                {
                    this.currentPageP = currentPagePayment.Value;
                }

                //filtros de búsqueda
                Query = Query
                .Where(v => EF.Functions.Like(v.IdabonoNavigation.Fechaabono, $"%{date}%"))
                .OrderByDescending(v => v.Id);

                this.totalP = Query.Count();

                Query = Query.Skip((currentPagePayment.Value - 1) * quantityPayment.Value).Take(quantityPayment.Value);

                this.PaymentsList = Query.ToList();
                this.showRegistersP = this.PaymentsList.Count;
            }
            return Partial("_ModalSalePaymentsPartial", this);
        }

        public async Task<decimal> restanteSale(int IdSale)
        {
            //recuperamos el listado de los pagos
            this.PaymentsList = await this._context.Pago
            .Include(a => a.IdabonoNavigation)
            .Where(p => p.Idventa == IdSale)
            .OrderBy(p => p.Id)
            .ToListAsync();

            //obteniendo la última fecha de pago
            if (this.PaymentsList.Count > 0)
            {
                this.notFilters = this.PaymentsList.Count;
                lastDate = this.PaymentsList.Last().IdabonoNavigation.Fechaabono;
                TimeSpan timeSpan = DateTime.Now - lastDate;
                this.daysNotPay = timeSpan.Days;
            }

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
            return restante;
        }

        public async Task<JsonResult> OnPost(int IdSale)
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Category = (object)null, Action = "save", Icon = "error" });
            }
            else
            {
                string nameClient = "";
                Pago pago;
                if (IdSale <= 0)
                {
                    Message = "La información del servidor ha sido alterada.";
                    return new JsonResult(new { Message = Message, Sale = (object)null, Action = "save", Icon = "warning" });
                }

                if (this.Payment.Monto <= 0 || this.Payment.Fechaabono == null)
                {
                    Message = "Por favor complete la información del formulario.";
                    return new JsonResult(new { Message = Message, Sale = (object)null, Action = "save", Icon = "warning" });
                }

                var sale = await this._context.Venta
                .Include(c => c.IdclienteNavigation)
                .ThenInclude(v => v.Institucion)
                .FirstOrDefaultAsync(v => v.Id == IdSale);

                if (sale == null)
                {
                    Message = "La información del servidor ha sido alterada.";
                    return new JsonResult(new { Message = Message, Sale = (object)null, Action = "save", Icon = "warning" });
                }

                //obteniendo el nombre del cliente
                if (sale.IdclienteNavigation.Institucion.Count > 0)
                {
                    foreach (var c in sale.IdclienteNavigation.Institucion)
                    {
                        if (c.Idrepresentante == sale.Idcliente)
                        {
                            nameClient = sale.IdclienteNavigation.Fullname() + " / " + c.Nombre;
                        }
                    }
                }
                else
                {
                    if (sale.IdclienteNavigation.Nombre.Equals("Público general") &&
                                sale.IdclienteNavigation.Apellido.Equals("Público general") &&
                                String.IsNullOrEmpty(sale.IdclienteNavigation.Dui))
                    {
                        nameClient = sale.IdclienteNavigation.Nombre;
                    }
                    else
                    {
                        nameClient = sale.IdclienteNavigation.Fullname();
                    }
                }

                //registramos el abono
                this._context.Abono.Add(Payment);
                await this._context.SaveChangesAsync();
                //guardamo el abono con la venta amarrada
                pago = new Pago
                {
                    Idabono = this.Payment.Id,
                    Idventa = sale.Id
                };

                this._context.Pago.Add(pago);
                await this._context.SaveChangesAsync();
                await UserActionsAsync("registro un nuevo abono para la venta del cliente " + nameClient + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));

                //recuperamos el restante, para ver si es cero poner el estado de la venta
                //en 0 (cancelada)
                this.restante = await restanteSale(IdSale);
                if (Convert.ToDecimal(this.restante.ToString("0.00")) == 0)
                {
                    sale.Estado = (sbyte)0;
                    this._context.Attach(sale).State = EntityState.Modified;
                    await this._context.SaveChangesAsync();
                    await UserActionsAsync("registro el último pago de la venta al crédito del cliente " + nameClient + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                }
                Message = "El abono se ha registrado correctamente.";

                return new JsonResult(new { Message = Message, Sale = (object)sale, Action = "save", Icon = "success" });
            }
        }
        public async Task<JsonResult> OnPostDeletePayment(int IdSale, int IdPayment)
        {
            if (IdSale > 0 && IdPayment > 0)
            {
                string nameClient = "";
                var sale = await this._context.Venta
                .Include(c => c.IdclienteNavigation)
                .ThenInclude(v => v.Institucion)
                .FirstOrDefaultAsync(v => v.Id == IdSale);

                if (sale == null)
                {
                    Message = "La información del servidor ha sido alterada.";
                    return new JsonResult(new { Message = Message, Sale = (object)null, Action = "delete", Icon = "warning" });
                }

                //obteniendo el nombre del cliente
                if (sale.IdclienteNavigation.Institucion.Count > 0)
                {
                    foreach (var c in sale.IdclienteNavigation.Institucion)
                    {
                        if (c.Idrepresentante == sale.Idcliente)
                        {
                            nameClient = sale.IdclienteNavigation.Fullname() + " / " + c.Nombre;
                        }
                    }
                }
                else
                {
                    if (sale.IdclienteNavigation.Nombre.Equals("Público general") &&
                                sale.IdclienteNavigation.Apellido.Equals("Público general") &&
                                String.IsNullOrEmpty(sale.IdclienteNavigation.Dui))
                    {
                        nameClient = sale.IdclienteNavigation.Nombre;
                    }
                    else
                    {
                        nameClient = sale.IdclienteNavigation.Fullname();
                    }
                }

                //recuperamos el detalle de abonos
                var pay = await this._context.Pago
                .Include(a => a.IdabonoNavigation)
                .FirstOrDefaultAsync(p => p.Id == IdPayment && p.Idventa == sale.Id);

                if (pay == null)
                {
                    Message = "El abono a eliminar no existe en los registros.";
                    return new JsonResult(new { Message = Message, Sale = (object)null, Action = "delete", Icon = "warning" });
                }
                //eliminamos el pago
                this._context.Pago.Remove(pay);
                await this._context.SaveChangesAsync();
                //eliminamos el abono
                this._context.Abono.Remove(pay.IdabonoNavigation);
                await this._context.SaveChangesAsync();
                await UserActionsAsync("eliminó un abono de la venta del cliente " + nameClient + ", el valor del abono era de " + pay.IdabonoNavigation.Monto.ToString("0.00") + "; la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "El abono se ha eliminado correctamente.";
                return new JsonResult(new { Message = Message, Sale = (object)sale, Action = "delete", Icon = "success" });
            }
            else
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Sale = (object)null, Action = "delete", Icon = "error" });
            }
        }
        public async Task UserActionsAsync(String Description)
        {
            Accion Action = new Accion();
            Action.Idbitacora = HttpContext.Session.GetInt32("IdBitacora").Value;
            Action.Descripcion = Description;
            Action.Hora = DateTime.Now;
            this._context.Accion.Add(Action);
            await this._context.SaveChangesAsync();
        }
    }
}
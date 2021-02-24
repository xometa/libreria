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

namespace RosaMaríaBookstore.Pages.Shoppings
{
    public class ShoppingsModel : PageModel
    {
        [BindProperty]
        public Compra Shopping { get; set; }
        public IList<Categoria> categoriesList { get; set; }
        public IList<Proveedor> ProvidersList { get; set; }
        private bookstoreContext _context { get; }
        String Message = "";
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        public ShoppingsModel(bookstoreContext context)
        {
            this._context = context;
            this.UploadImage = new UploadImage(context);
        }
        public async Task<IActionResult> OnGet()
        {
            this.categoriesList = this._context.Categoria
            .ToList();

            this.ProvidersList = this._context.Proveedor
            .Include(p => p.IdrepresentanteNavigation)
            .ToList();

            //obteniendo el id de inicio de sesión del usuario y su rol
            int? idUser = HttpContext.Session.GetInt32("IdUser");
            string rol = HttpContext.Session.GetString("Rol");
            if (idUser != null)
            {
                if (!rol.Equals("Administrador"))
                {
                    task = await UploadImage.accessUser(idUser.Value, "Compras", "/Shoppings/Shoppings");
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

        public async Task<JsonResult> OnPost(Detallecompra[] details)
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Shopping = (object)null, Action = "save", Icon = "error" });
            }
            //obteniendo el id de inicio de sesión del usuario
            int idUser = HttpContext.Session.GetInt32("IdUser").Value;
            int Length = details.Length;
            Detallecompra dc;
            Producto pr;
            if (!String.IsNullOrEmpty(this.Shopping.Documento) &&
                    !String.IsNullOrEmpty(this.Shopping.Tipo) &&
                    this.Shopping.Idproveedor > 0 &&
                    this.Shopping.Fecha != null)
            {
                if (details.Length <= 0)
                {
                    Message = "Por favor, seleccione los productos necesarios, para poder realizar la compra.";
                    return new JsonResult(new { Message = Message, Shopping = (object)null, Action = "save", Icon = "warning" });
                }

                if (idUser <= 0)
                {
                    Message = "Su información de usuario ha expirado. Por favor, vuelva a iniciar sesión, para poder realizar la venta.";
                    return new JsonResult(new { Message = Message, Shopping = (object)null, Action = "save", Icon = "info" });
                }
                String message = "";
                var provider = await this._context.Proveedor
                .Include(p => p.IdrepresentanteNavigation)
                .Where(p => p.Id == this.Shopping.Idproveedor)
                .FirstOrDefaultAsync();
                if (provider != null)
                {
                    message = provider.IdrepresentanteNavigation.Fullname();
                }
                //agregamos el id del usuario al encabezado compra
                this.Shopping.Idusuario = idUser;
                //procedemos a guardar la compra
                await this._context.Compra.AddAsync(Shopping);
                await this._context.SaveChangesAsync();

                //procedemos a registrar el detalle de la compra
                for (int i = 0; i < Length; i++)
                {
                    //guardando en el detalle de la venta
                    dc = new Detallecompra();
                    dc = details[i];
                    dc.Idcompra = this.Shopping.Id;
                    await this._context.Detallecompra.AddAsync(dc);
                    await this._context.SaveChangesAsync();
                    //recuperamos el id del producto
                    pr = this._context.Producto.FirstOrDefault(p => p.Id == dc.Idproducto);

                    //procedimiento para guardarlo en el inventario
                    int Stock = 0;
                    IList<KardexQuery> kq = this._context.KardexQuery.FromSqlRaw(KardexQuery.stockAvailable(), pr.Id, pr.Id).ToList();
                    if (kq != null && kq.Count > 0)
                    {
                        if (kq.Count == 2)
                        {
                            if (kq[0].Id > kq[1].Id)
                            {
                                Stock = kq.First().Existencia;
                            }
                            else
                            {
                                Stock = kq.Last().Existencia;
                            }
                        }
                        else
                        {
                            Stock = kq.Last().Existencia;
                        }
                    }
                    //Restamos a la existencia de inventario (Si hay en el inventario) lo del detalle de la venta
                    Stock += dc.Cantidad;
                    Inventario inventory = new Inventario
                    {
                        Existencia = Stock,
                        Fecha = Shopping.Fecha,
                        Idcompra = dc.Id,
                        Idproducto = pr.Id
                    };
                    await this._context.Inventario.AddAsync(inventory);
                    await this._context.SaveChangesAsync();

                }
                await UserActionsAsync("registro una nueva venta para el proveedor " + message + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
                Message = "La compra se ha registrado satisfactoriamente.";
                return new JsonResult(new { Message = Message, Shopping = (object)Shopping, Action = "save", Icon = "success" });
            }
            else
            {
                Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                return new JsonResult(new { Message = Message, Shopping = (object)null, Action = "save", Icon = "warning" });
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
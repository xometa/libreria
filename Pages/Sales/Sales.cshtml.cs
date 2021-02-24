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
    public class SalesModel : PageModel
    {
        [BindProperty]
        public Venta Sale { get; set; }
        public Detalleventa saleDetails { get; }
        public IList<Preciosproducto> PricesList { get; set; }
        public IList<Persona> ClientsList { get; set; }
        public IList<Institucion> InstitutionsList { get; set; }
        public IList<Categoria> categoriesList { get; set; }
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        private bookstoreContext _context { get; }
        String Message = "";
        public SalesModel(bookstoreContext context)
        {
            this._context = context;
            this.UploadImage = new UploadImage(context);
        }
        public async Task<IActionResult> OnGet()
        {
            this.ClientsList = this._context.Persona.Where(p => p.Tipo == "Público")
            .ToList();
            this.InstitutionsList = this._context.Institucion
            .Include(p => p.IdrepresentanteNavigation)
            .ToList();
            this.categoriesList = this._context.Categoria
            .ToList();
            

             //obteniendo el id de inicio de sesión del usuario y su rol
            int? idUser = HttpContext.Session.GetInt32("IdUser");
            string rol = HttpContext.Session.GetString("Rol");
            if (idUser != null)
            {
                if (!rol.Equals("Administrador"))
                {
                    task = await UploadImage.accessUser(idUser.Value, "Ventas", "/Sales/Sales");
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
        public async Task<JsonResult> OnGetPricesList(int Id)
        {
            if (Id != 0)
            {
                this.PricesList = await this._context.Preciosproducto
                .Include(c => c.IdcompraNavigation)
                .ThenInclude(dc => dc.Detallecompra)
                .Include(p => p.IdproductoNavigation)
                .Where(p => p.Idproducto == Id && p.Estado == 1).OrderByDescending(p => p.Id).ToListAsync();
            }
            return new JsonResult(new { Prices = this.PricesList });
        }

        public async Task<JsonResult> OnPost(Detalleventa[] details)
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Sale = (object)null, Action = "save", Icon = "error" });
            }
            //obteniendo el id de inicio de sesión del usuario
            int idUser = HttpContext.Session.GetInt32("IdUser").Value;
            int Length = details.Length;
            Detalleventa dv;
            Preciosproducto pp;
            if (!String.IsNullOrEmpty(this.Sale.Documento) &&
                    !String.IsNullOrEmpty(this.Sale.Tipo) &&
                    this.Sale.Idcliente > 0 &&
                    this.Sale.Fecha != null)
            {
                if (details.Length <= 0)
                {
                    Message = "Por favor, seleccione los productos necesarios, para poder realizar la venta.";
                    return new JsonResult(new { Message = Message, Sale = (object)null, Action = "save", Icon = "warning" });
                }
                if (idUser <= 0)
                {
                    Message = "Su información de usuario ha expirado. Por favor, vuelva a iniciar sesión, para poder realizar la venta.";
                    return new JsonResult(new { Message = Message, Sale = (object)null, Action = "save", Icon = "info" });
                }
                String message = "";
                var persona = await this._context.Persona
                .Include(i => i.Institucion)
                .Where(p => p.Id == this.Sale.Idcliente)
                .FirstOrDefaultAsync();
                if (persona.Institucion.Count > 0)
                {
                    message = persona.Fullname() + " (Cliente institucional)";
                }
                else
                {
                    if (persona.Nombre.Equals("Público general") &&
                    persona.Apellido.Equals("Público general") &&
                    String.IsNullOrEmpty(persona.Dui))
                    {
                        message = persona.Nombre;
                    }
                    else
                    {
                        message = persona.Fullname();
                    }
                }
                //agregamos el estado activo si es una venta al crédito
                if (this.Sale.Tipo.Equals("Crédito"))
                {
                    this.Sale.Estado = (sbyte)1;
                }
                else
                {
                    this.Sale.Estado = (sbyte)0;
                }
                //agregamos el id del usuario al encabezado venta
                this.Sale.Idusuario = idUser;
                //procedemos a guardar la venta
                await this._context.Venta.AddAsync(Sale);
                await this._context.SaveChangesAsync();

                //procedemos a registrar el detalle
                for (int i = 0; i < Length; i++)
                {
                    //guardando en el detalle de la venta
                    dv = new Detalleventa();
                    dv = details[i];
                    dv.Idventa = this.Sale.Id;
                    await this._context.Detalleventa.AddAsync(dv);
                    await this._context.SaveChangesAsync();
                    //recuperamos el id del producto
                    pp = this._context.Preciosproducto.FirstOrDefault(p => p.Id == dv.Idproducto);

                    //procedimiento para guardarlo en el inventario
                    int Stock = 0;
                    IList<KardexQuery> kq = this._context.KardexQuery.FromSqlRaw(KardexQuery.stockAvailable(), pp.Idproducto, pp.Idproducto).ToList();
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
                    Stock -= dv.Cantidad;
                    Inventario inventory = new Inventario
                    {
                        Existencia = Stock,
                        Fecha = Sale.Fecha,
                        Idventa = dv.Id,
                        Idproducto = pp.Idproducto
                    };
                    await this._context.Inventario.AddAsync(inventory);
                    await this._context.SaveChangesAsync();

                }
                await UserActionsAsync("registro una nueva venta para el cliente " + message + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
                Message = "La venta se ha registrado satisfactoriamente.";
                return new JsonResult(new { Message = Message, Sale = (object)Sale, Action = "save", Icon = "success" });
            }
            else
            {
                Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                return new JsonResult(new { Message = Message, Sale = (object)Sale, Action = "save", Icon = "warning" });
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
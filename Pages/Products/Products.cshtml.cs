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

namespace RosaMaríaBookstore.Pages.Products
{

    public class ProductsModel : PageModel
    {
        [BindProperty]
        public Producto Producto { get; set; }
        public UploadImage UploadImage { get; set; }
        public Preciosproducto Preciosproducto { get; set; }
        public IList<Preciosproducto> PricesList { get; set; }
        public IList<Marca> TrademarksList { get; set; }
        public IList<Categoria> CategoriesList { get; set; }
        public IList<LastPurchasePrice> LastPurchasePriceList { get; set; }
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        private bookstoreContext _context { get; }
        String Message = "";
        public ProductsModel(bookstoreContext context)
        {
            this._context = context;
            this.UploadImage = new UploadImage(context);
            this.Preciosproducto = new Preciosproducto();
            this.currentPage = 1;
            this.pageSize = 3;
            this.total = 0;
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
                    task = await UploadImage.accessUser(idUser.Value, "Productos", "/Products/Products");
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
        public async Task<PartialViewResult> OnPostPartialView(int? Id)
        {
            if (Id != 0)
            {
                this.Producto = await this._context.Producto
                .Include(m => m.IdmarcaNavigation)
                .Include(c => c.IdcategoriaNavigation)
                .Include(i => i.IdimagenNavigation)
                .FirstOrDefaultAsync(p => p.Id == Id);
            }
            this.TrademarksList = this._context.Marca.ToList();
            this.CategoriesList = this._context.Categoria.ToList();
            return Partial("_ModalProductPartial", this);
        }

        public PartialViewResult OnPostPricePartialView(int Id)
        {
            this.LastPurchasePriceList = this._context.LastPurchasePrice.FromSqlRaw(LastPurchasePrice.sqlLastPrice(), Id).ToList();
            this.Preciosproducto.Idcompra = this.LastPurchasePriceList[0].Id;
            this.Preciosproducto.Idproducto = this.LastPurchasePriceList[0].IdProducto;
            return Partial("_ModalPricePartial", this);
        }

        public async Task<PartialViewResult> OnPostProductInformation(int Id)
        {
            if (Id > 0)
            {
                this.Producto = await this._context.Producto
                   .Include(m => m.IdmarcaNavigation)
                   .Include(c => c.IdcategoriaNavigation)
                   .Include(i => i.IdimagenNavigation)
                   .FirstOrDefaultAsync(p => p.Id == Id);
            }
            return Partial("_ModalProInfoPartial", this);
        }

        /*Metodo para guardar un nuevo registro*/
        public async Task<JsonResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Product = (object)null, Action = "save", Icon = "error" });
            }
            else
            {
                if (!String.IsNullOrEmpty(this.Producto.Codigo) &&
                    !String.IsNullOrEmpty(this.Producto.Nombre) &&
                    this.Producto.Stockminimo > 0 &&
                    this.Producto.Idcategoria > 0)
                {

                    if (ProductExists(0, Producto.Codigo, 0))
                    {
                        Message = "El código ingresado para el producto " + Producto.Nombre + ", ya ha sido asignado.";
                        return new JsonResult(new { Message = Message, Product = (object)null, Action = "save", Icon = "warning" });
                    }
                    else
                    {
                        /* Comprobamos que se asignado una imagen al producto, de ser así se 
                        procede a realizar las validaciones respectivas al archivo*/
                        String Route = "products";
                        int IdImage = await UploadImage.saveImage(Producto.Archivo, Route);
                        if (IdImage > 0)
                        {
                            Producto.Idimagen = IdImage;
                        }
                        else if (IdImage == -1)
                        {
                            Message = "El nombre del archivo: " + Producto.Archivo.FileName + " es incorrecto";
                            return new JsonResult(new { Message = Message, Product = (object)null, Action = "save", Icon = "warning" });
                        }
                        else if (IdImage == -2)
                        {
                            Message = "La imagen: " + Producto.Archivo.FileName + " ya ha sido agregada. Cambie el nombre de la imagen y vuelva a intentarlo.";
                            return new JsonResult(new { Message = Message, Product = (object)null, Action = "save", Icon = "warning" });
                        }

                        Producto.Estado = (sbyte)0;
                        this._context.Producto.Add(Producto);
                        await this._context.SaveChangesAsync();
                        await UserActionsAsync("registro un nuevo producto (" + Producto.Nombre + "), la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                        Message = "El producto se ha registrado correctamente.";
                        return new JsonResult(new { Message = Message, Product = (object)Producto, Action = "save", Icon = "success" });
                    }
                }
                else
                {
                    Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                    return new JsonResult(new { Message = Message, Product = (object)null, Action = "save", Icon = "warning" });
                }
            }
        }

        public async Task<JsonResult> OnPostDelete(int Id)
        {
            if (Id < 1)
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Product = (object)null, Action = "delete", Icon = "error" });
            }
            var product = await this._context.Producto
            .Include(dc => dc.Detallecompra)
            .FirstOrDefaultAsync(p => p.Id == Id);
            if (product == null)
            {
                Message = "El producto a eliminar no existe en los registros.";
                return new JsonResult(new { Message = Message, Product = (object)null, Action = "delete", Icon = "warning" });
            }
            if (product.Detallecompra.Count > 0)
            {
                Message = "El producto no se debe eliminar, ya que se han registrado compras del mismo.";
                return new JsonResult(new { Message = Message, Product = (object)null, Action = "delete", Icon = "warning" });
            }
            this._context.Producto.Remove(product);
            await this._context.SaveChangesAsync();
            await UserActionsAsync("eliminó el producto " + product.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
            Message = "El producto ha sido eliminado satisfactoriamente.";
            await deleteImage(product);
            return new JsonResult(new { Message = Message, Product = (object)product, Action = "delete", Icon = "success" });
        }

        public async Task<JsonResult> OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Product = (object)null, Action = "edit", Icon = "error" });
            }
            else
            {
                if (!String.IsNullOrEmpty(this.Producto.Codigo) &&
                    !String.IsNullOrEmpty(this.Producto.Nombre) &&
                    this.Producto.Stockminimo > 0 &&
                    this.Producto.Idcategoria > 0 &&
                    this.Producto.Id > 0)
                {
                    var product = await this._context.Producto.FirstOrDefaultAsync(p => p.Id == Producto.Id);
                    if (!(product.Codigo.Equals(Producto.Codigo)))
                    {
                        if (ProductExists(0, Producto.Codigo, 0))
                        {
                            Message = "El código ingresado para el producto " + Producto.Nombre + ", ya ha sido asignado.";
                            return new JsonResult(new { Message = Message, Product = (object)null, Action = "edit", Icon = "error" });
                        }
                    }

                    if (Producto.Archivo != null)
                    {
                        String Route = "products";
                        int IdImage = await UploadImage.saveImage(Producto.Archivo, Route);
                        if (IdImage > 0)
                        {
                            //si se asigna una nueva imagen, al producto, y este ya posee una, es de eliminarla del DB,
                            //posteriormente se procede a asignar el Id de la imagen nueva del producto
                            await deleteImage(product);
                            Producto.Idimagen = IdImage;
                        }
                        else if (IdImage == -1)
                        {
                            Message = "El nombre del archivo: " + Producto.Archivo.FileName + " es incorrecto";
                            return new JsonResult(new { Message = Message, Product = (object)null, Action = "edit", Icon = "warning" });
                        }
                        else if (IdImage == -2)
                        {
                            Message = "La imagen: " + Producto.Archivo.FileName + " ya ha sido agregada. Cambie el nombre de la imagen y vuelva a intentarlo.";
                            return new JsonResult(new { Message = Message, Product = (object)Producto, Action = "edit", Icon = "warning" });
                        }
                    }
                    else
                    {
                        int IdImage = await searchProduct(Producto.Id);
                        if (IdImage != 0)
                        {
                            Producto.Idimagen = IdImage;
                        }
                    }

                    product.Codigo = Producto.Codigo;
                    product.Nombre = Producto.Nombre;
                    product.Descripcion = Producto.Descripcion;
                    product.Stockminimo = Producto.Stockminimo;
                    product.Idmarca = Producto.Idmarca;
                    product.Idcategoria = Producto.Idcategoria;
                    product.Idimagen = Producto.Idimagen;
                    await this._context.SaveChangesAsync();
                    await UserActionsAsync("modificó el producto " + Producto.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                    Message = "El producto ha sido modificado satisfactoriamente.";
                    return new JsonResult(new { Message = Message, Product = (object)Producto, Action = "edit", Icon = "success" });
                }
                else
                {
                    Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                    return new JsonResult(new { Message = Message, Product = (object)Producto, Action = "edit", Icon = "warning" });
                }
            }
        }

        public bool ProductExists(int Id, String Code, int IdCompra)
        {
            /**
             * Este metodo me permitira comprobar si existe un producto mediante la busqueda de su Id ó Código.
             * De igual forma tambien permitira buscar una compra, la cual se agregara a los precios de los
             * de los productos
             *
             * El metodo Any del Framework devuelve true o false
             */
            if (Id > 0)
            {
                return _context.Producto.Where(p => p.Id == Id).Any();
            }
            else if (!Code.Equals(""))
            {
                return _context.Producto.Where(p => p.Codigo == Code).Any();
            }
            else
            {
                return _context.Compra.Where(c => c.Id == IdCompra).Any();
            }
        }

        public async Task<int> searchProduct(int Id)
        {
            int IdImage = 0;
            var product = await this._context.Producto.FirstOrDefaultAsync(p => p.Id == Id);
            if (product != null)
            {
                if (product.Idimagen != null) { IdImage = (int)product.Idimagen; }
            }
            return IdImage;
        }

        public async Task deleteImage(Producto Product)
        {
            var image = await this._context.Imagen.FirstOrDefaultAsync(i => i.Id == Product.Idimagen);
            //eliminamos la imagen que corresponde al producto, para que no
            //hallan registros sin utilizar en la db
            if (image != null)
            {
                this._context.Imagen.Remove(image);
                await this._context.SaveChangesAsync();
                //eliminación de la imagen del servidor
                UploadImage.deleteServerImage("wwwroot/images/products/", image.Nombre);
            }
        }

        //metodos para guardar y eliminar precios del producto
        public async Task<JsonResult> OnPostProductSavePrice(Preciosproducto Preciosproducto)
        {
            if (!ModelState.IsValid)
            {
                Message = "La información que se ha proporcionado es incorrecta, verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Price = (object)null, Action = "save", Icon = "error" });
            }
            else
            {
                if (Preciosproducto.Idcompra == 0 || Preciosproducto.Idproducto == 0)
                {
                    Message = "La información del servidor ha sido alterada, por favor vuelva a intentarlo.";
                    return new JsonResult(new { Message = Message, Price = (object)null, Action = "save", Icon = "warning" });
                }
                if (!ProductExists(Preciosproducto.Idproducto, "", 0) || !ProductExists(0, "", Preciosproducto.Idcompra))
                {
                    Message = "La información del servidor ha sido alterada. El producto y/o compra refernciada, no existen en los registros.";
                    return new JsonResult(new { Message = Message, Price = (object)null, Action = "save", Icon = "warning" });
                };
                if (Preciosproducto.Margen == 0 || Preciosproducto.Margen.Equals(""))
                {
                    Message = "El margen de ganancia ingresado es incorrecto.";
                    return new JsonResult(new { Message = Message, Price = (object)null, Action = "save", Icon = "error" });
                }
                Preciosproducto.Estado = (sbyte)0;
                Preciosproducto.Fecha = DateTime.Now;
                this._context.Preciosproducto.Add(Preciosproducto);
                await this._context.SaveChangesAsync();
                await UserActionsAsync("registro un nuevo precio, para un producto; la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "El precio ha sido asignado correctamente al producto.";
            }
            return new JsonResult(new { Message = Message, Price = (object)Preciosproducto, Action = "save", Icon = "success" });
        }

        public PartialViewResult OnPostPricesListPartialView(int Id, string date, int? quantity, int? currentPage)
        {
            if (Id != 0)
            {
                IQueryable<Preciosproducto> Query = this._context.Preciosproducto
                .Include(c => c.IdcompraNavigation)
                .ThenInclude(dc => dc.Detallecompra)
                .Include(p => p.IdproductoNavigation)
                .Where(p => p.Idproducto == Id);
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
                .Where(pp => EF.Functions.Like(pp.Fecha, $"%{date}%"))
                .OrderByDescending(p => p.Id);

                this.total = Query.Count();

                Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

                this.PricesList = Query.ToList();
                this.showRegisters = this.PricesList.Count;
            }
            return Partial("_ModalPricesDetailPartial", this);
        }

        public async Task<JsonResult> OnPostDeletePrice(int Id)
        {
            if (Id <= 0)
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Price = (object)null, Action = "delete", Icon = "error" });
            }
            var price = await this._context.Preciosproducto
            .Include(p => p.IdproductoNavigation)
            .FirstOrDefaultAsync(p => p.Id == Id);
            if (price == null)
            {
                Message = "Advertencia. El precio a eliminar no existe en los registros.";
                return new JsonResult(new { Message = Message, Price = (object)null, Action = "delete", Icon = "warning" });
            }
            this._context.Preciosproducto.Remove(price);
            await this._context.SaveChangesAsync();
            await UserActionsAsync("eliminó un precio del producto " + price.IdproductoNavigation.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
            Message = "El precio del producto ha sido eliminado satisfactoriamente.";
            return new JsonResult(new { Message = Message, Price = (object)price, Action = "delete", Icon = "success" });
        }

        public async Task<JsonResult> OnPostEditStatus(int Id, int Status, String Option)
        {
            if (Id <= 0)
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Price = (object)null, Action = "change", Icon = "error" });
            }
            if (Option.Equals("Price"))
            {
                var obj = await this._context.Preciosproducto
                .Include(p => p.IdproductoNavigation)
                .FirstOrDefaultAsync(p => p.Id == Id);
                if (obj == null)
                {
                    Message = "Advertencia. El estado del precio no se puede modificar.";
                    return new JsonResult(new { Message = Message, Price = (object)null, Action = "change", Icon = "warning" });
                }
                obj.Estado = (sbyte)Status;
                this._context.Attach(obj).State = EntityState.Modified;
                await this._context.SaveChangesAsync();
                await UserActionsAsync("modificó el estado del precio del producto " + obj.IdproductoNavigation.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                if (obj.Estado == 1)
                {
                    Message = "El precio del producto de la fecha " + obj.Fecha.ToString("dd/MM/yyyy") + " ha sido activado.";
                }
                else
                {
                    Message = "El precio del producto de la fecha " + obj.Fecha.ToString("dd/MM/yyyy") + " ha sido desactivado.";
                }
                return new JsonResult(new { Message = Message, Price = (object)obj, Action = "change", Icon = "success" });
            }
            else
            {
                var obj = await this._context.Producto.FirstOrDefaultAsync(p => p.Id == Id);
                if (obj == null)
                {
                    Message = "Advertencia. El estado del producto no se puede modificar.";
                    return new JsonResult(new { Message = Message, Price = (object)null, Action = "change", Icon = "warning" });
                }
                obj.Estado = (sbyte)Status;
                this._context.Attach(obj).State = EntityState.Modified;
                await this._context.SaveChangesAsync();
                await UserActionsAsync("modificó el estado del producto " + obj.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                if (obj.Estado == 1)
                {

                    Message = "El producto " + obj.Nombre.ToLower() + " ha sido habilitado.";
                }
                else
                {

                    Message = "El producto " + obj.Nombre.ToLower() + " ha sido deshabilitado.";
                }
                return new JsonResult(new { Message = Message, Price = (object)obj, Action = "change", Icon = "success" });
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
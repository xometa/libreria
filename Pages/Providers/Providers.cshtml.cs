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

namespace RosaMaríaBookstore.Pages.Providers
{
    public class ProvidersModel : PageModel
    {
        [BindProperty]
        public Persona Person { get; set; }
        [BindProperty]
        public Proveedor Provider { get; set; }
        [BindProperty]
        public Telefono Phone { get; set; }
        [BindProperty]
        public Telefonoproveedor ProviderPhone { get; set; }
        public IList<Telefonoproveedor> phoneList { get; set; }
        String Message = "";
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        public ProvidersModel(bookstoreContext context)
        {
            this._context = context;
            this.UploadImage = new UploadImage(context);
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
                    task = await UploadImage.accessUser(idUser.Value, "Proveedores", "/Providers/Providers");
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
                this.Person = await this._context.Persona
                .FirstOrDefaultAsync(p => p.Id == Id);
                this.Provider = await this._context.Proveedor
                .Include(p => p.IdrepresentanteNavigation)
                .FirstOrDefaultAsync(i => i.Idrepresentante == Id);
            }
            return Partial("_ModalProviderPartial", this);
        }

        public PartialViewResult OnGetPhoneList(int Id, string search, int? quantity, int? currentPage)
        {
            if (Id != 0)
            {
                IQueryable<Telefonoproveedor> Query = this._context.Telefonoproveedor
                .Include(t => t.IdtelefonoNavigation)
                .Include(p => p.IdproveedorNavigation)
                .Where(tp => tp.Idproveedor == Id)
                .OrderByDescending(tp => tp.Id);

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
                .Where(tp => EF.Functions.Like(tp.IdtelefonoNavigation.Telefono1, $"%{search}%"))
                .OrderByDescending(t => t.Id);

                this.total = Query.Count();
                Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

                this.phoneList = Query.ToList();
                this.showRegisters = this.phoneList.Count;
            }
            return Partial("_ModalPhoneProviderPartial", this);
        }

        public async Task<JsonResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Provi = (object)null, Provider = (object)null, Action = "save", Icon = "error" });
            }
            else
            {
                if (this.Provider.Nombre != null)
                {
                    if (!String.IsNullOrEmpty(this.Person.Tipo) &&
                        !String.IsNullOrEmpty(this.Person.Nombre) &&
                        !String.IsNullOrEmpty(this.Person.Apellido) &&
                        !String.IsNullOrEmpty(this.Person.Dui) &&
                        !String.IsNullOrEmpty(this.Provider.Nombre) &&
                        !String.IsNullOrEmpty(this.Provider.Direccion))
                    {
                        if (Exists(this.Person.Dui, "", "Proveedor"))
                        {
                            Message = "El DUI ingresado del representante, ya ha sido registrado.";
                            return new JsonResult(new { Message = Message, Provi = (object)null, Provider = (object)null, Action = "save", Icon = "warning" });
                        }
                        if (Exists("", this.Provider.Nombre, ""))
                        {
                            Message = "El nombre del proveedor ya ha sido ingresado.";
                            return new JsonResult(new { Message = Message, Provi = (object)null, Provider = (object)null, Action = "save", Icon = "warning" });
                        }
                        this._context.Persona.Add(Person);
                        await this._context.SaveChangesAsync();
                        this.Provider.Idrepresentante = this.Person.Id;
                        this._context.Proveedor.Add(Provider);
                        await this._context.SaveChangesAsync();
                        await UserActionsAsync("registro un nuevo proveedor (" + Provider.Nombre + "), la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                        Message = "El proveedor " + this.Provider.Nombre + " ha sido registrado correctamente.";
                        return new JsonResult(new { Message = Message, Provi = (object)Person, Provider = (object)Provider, Action = "save", Icon = "success" });
                    }
                    else
                    {
                        Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                        return new JsonResult(new { Message = Message, Provi = (object)null, Provider = (object)null, Action = "save", Icon = "warning" });
                    }
                }
                else
                {
                    Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                    return new JsonResult(new { Message = Message, Provi = (object)null, Provider = (object)null, Action = "save", Icon = "warning" });
                }
            }
        }

        public async Task<JsonResult> OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Provi = (object)null, Provider = (object)null, Action = "edit", Icon = "error" });
            }
            else
            {
                var person = await this._context.Persona.FirstOrDefaultAsync(p => p.Id == this.Person.Id);
                var provider = await this._context.Proveedor.FirstOrDefaultAsync(p => p.Idrepresentante == this.Person.Id);
                if (person != null && provider != null)
                {
                    if (!String.IsNullOrEmpty(this.Person.Nombre) &&
                        !String.IsNullOrEmpty(this.Person.Apellido) &&
                        !String.IsNullOrEmpty(this.Person.Dui) &&
                        !String.IsNullOrEmpty(this.Provider.Nombre) &&
                        !String.IsNullOrEmpty(this.Provider.Direccion))
                    {
                        if (!(person.Dui.Equals(this.Person.Dui)))
                        {
                            if (Exists(this.Person.Dui, "", "Proveedor"))
                            {
                                Message = "El DUI ingresado para el representante " + this.Person.Nombre.ToLower() + " ya ha sido registrado.";
                                return new JsonResult(new { Message = Message, Provi = (object)null, Provider = (object)null, Action = "edit", Icon = "error" });
                            }
                        }
                        if (!(provider.Nombre.Equals(this.Provider.Nombre)))
                        {
                            if (Exists("", this.Provider.Nombre, ""))
                            {
                                Message = "El nombre ingresado del proveedor ya ha sido registrado.";
                                return new JsonResult(new { Message = Message, Provi = (object)null, Provider = (object)null, Action = "edit", Icon = "error" });
                            }
                        }
                        person.Nombre = this.Person.Nombre;
                        person.Apellido = this.Person.Apellido;
                        person.Dui = this.Person.Dui;
                        await this._context.SaveChangesAsync();
                        provider.Nombre = this.Provider.Nombre;
                        provider.Direccion = this.Provider.Direccion;
                        await this._context.SaveChangesAsync();
                        await UserActionsAsync("modificó la información del proveedor " + provider.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                        Message = "La información del proveedor, se ha modificado correctamente.";
                        return new JsonResult(new { Message = Message, Provi = (object)person, Provider = (object)provider, Action = "edit", Icon = "success" });
                    }
                    else
                    {
                        Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                        return new JsonResult(new { Message = Message, Provi = (object)null, Provider = (object)null, Action = "edit", Icon = "warning" });
                    }
                }
                else
                {
                    Message = "La información del servidor ha sido alterada.";
                    return new JsonResult(new { Message = Message, Provi = (object)null, Provider = (object)null, Action = "edit", Icon = "error" });
                }
            }
        }

        public bool Exists(String DUI, String Name, String Option)
        {
            if (!DUI.Equals(""))
            {
                return _context.Persona.Where(p => p.Dui == DUI).Where(p => p.Tipo == Option).Any();
            }
            else if (!Name.Equals(""))
            {
                return _context.Proveedor.Where(i => i.Nombre == Name).Any();
            }
            else
            {
                return false;
            }
        }

        public async Task<JsonResult> OnPostDelete(int IdPerson, String Option)
        {
            var person = await this._context.Persona.Include(p => p.Venta).FirstOrDefaultAsync(p => p.Id == IdPerson);
            var provider = await this._context.Proveedor.Include(i => i.Telefonoproveedor).FirstOrDefaultAsync(p => p.Idrepresentante == IdPerson);
            if (person != null && provider != null && Option.Equals("Provi"))//es un proveedor con representante
            {
                if (person.Venta.Count > 0)
                {
                    Message = "El proveedor " + provider.Nombre.ToLower() + " no se puede eliminar, porque posee registros de ventas.";
                    return new JsonResult(new { Message = Message, Provi = (object)Person, Provider = (object)null, Action = "delete", Icon = "warning" });
                }
                //si posee telefonos el proveedor procedemos a eliminarlos
                if (provider.Telefonoproveedor.Count > 0)
                {
                    IList<Telefonoproveedor> detail = this._context.Telefonoproveedor.Where(ti => ti.Idproveedor == provider.Id).ToList();
                    foreach (var phone in detail)
                    {
                        var aux = await this._context.Telefono.FirstOrDefaultAsync(p => p.Id == phone.Idtelefono);
                        if (aux != null)
                        {
                            this._context.Telefonoproveedor.Remove(phone);
                            await this._context.SaveChangesAsync();
                            this._context.Telefono.Remove(aux);
                            await this._context.SaveChangesAsync();
                        }
                    }
                    await UserActionsAsync("eliminó todos los números de contacto del proveedor " + provider.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                }
                this._context.Proveedor.Remove(provider);
                await this._context.SaveChangesAsync();
                this._context.Persona.Remove(person);
                await this._context.SaveChangesAsync();
                await UserActionsAsync("eliminó el proveedor " + provider.Nombre + ", la fecha" + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "El proveedor se ha eliminado correctamente";
                return new JsonResult(new { Message = Message, Provi = (object)Person, Provider = (object)provider, Action = "delete", Icon = "success" });
            }
            else//si no la información se ha alterado, por lo cual se debe informaar al usuario final
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Provi = (object)null, Provider = (object)null, Action = "delete", Icon = "error" });
            }
        }

        public async Task<JsonResult> OnPostPhone()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Phone = (object)null, Action = "savephone", Icon = "error" });
            }
            {
                if (!this.Phone.Telefono1.Equals(""))
                {
                    //guardamos el número de telefono
                    this._context.Telefono.Add(this.Phone);
                    await this._context.SaveChangesAsync();
                    //lo agregamos al detalle
                    this.ProviderPhone.Idtelefono = this.Phone.Id;
                    this._context.Telefonoproveedor.Add(this.ProviderPhone);
                    await this._context.SaveChangesAsync();
                    await UserActionsAsync("registro el número de contacto " + Phone.Telefono1 + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                    Message = "El número de contacto se ha registrado correctamente.";
                    return new JsonResult(new { Message = Message, Phone = (object)ProviderPhone, Action = "savephone", Icon = "success" });
                }
                else
                {
                    Message = "Ingrese un número de contacto.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "savephone", Icon = "warning" });
                }
            }
        }

        public async Task<JsonResult> OnPostEditPhone()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Phone = (object)null, Action = "editphone", Icon = "error" });
            }
            {
                var phone = await this._context.Telefono.FirstOrDefaultAsync(t => t.Id == this.Phone.Id);
                var obj = await this._context.Telefonoproveedor.FirstOrDefaultAsync(t => t.Idtelefono == this.Phone.Id);
                if (phone == null)
                {
                    Message = "El número de contacto a modificar no existe.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "editphone", Icon = "warning" });
                }
                if (!this.Phone.Telefono1.Equals(""))
                {
                    //guardamos el número de telefono
                    phone.Telefono1 = this.Phone.Telefono1;
                    await this._context.SaveChangesAsync();
                    await UserActionsAsync("modificó el número de contacto " + phone.Telefono1 + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                    Message = "El número de contacto se ha modificado correctamente.";
                    return new JsonResult(new { Message = Message, Phone = (object)obj, Action = "editphone", Icon = "success" });
                }
                else
                {
                    Message = "Ingrese un número de contacto.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "editphone", Icon = "warning" });
                }
            }
        }

        public async Task<JsonResult> OnPostDeletePhone(int Id)
        {
            if (Id > 0)
            {
                var phone = await this._context.Telefono.FirstOrDefaultAsync(t => t.Id == Id);
                var ti = await this._context.Telefonoproveedor.FirstOrDefaultAsync(tn => tn.Idtelefono == Id);
                if (phone == null)
                {
                    Message = "El número de contacto que desea eliminar, no existe; verifique y vuelva a intentarlo.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "deletephone", Icon = "warning" });
                }
                if (phone != null && ti == null)
                {
                    Message = "El número de contacto a eliminar no pertenece a este proveedor.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "deletephone", Icon = "error" });
                }
                if (phone == null && ti == null)
                {
                    Message = "La información del servidor ha sido alterada.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "deletephone", Icon = "error" });
                }
                //eliminamos el número de telefono del detalle
                this._context.Telefonoproveedor.Remove(ti);
                await this._context.SaveChangesAsync();
                //eliminamos el número de los registros
                this._context.Telefono.Remove(phone);
                await this._context.SaveChangesAsync();
                await UserActionsAsync("eliminó el número de contacto " + phone.Telefono1 + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "El número de contacto se ha eliminado correctamente.";
                return new JsonResult(new { Message = Message, Phone = (object)ti, Action = "deletephone", Icon = "success" });
            }
            else
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Phone = (object)null, Action = "deletephone", Icon = "error" });
            }
        }

        public async Task<JsonResult> OnPostEditStatus(int Id, int Status)
        {
            if (Id > 0)
            {
                var obj = await this._context.Telefono.FirstOrDefaultAsync(t => t.Id == Id);
                if (obj == null)
                {
                    Message = "Advertencia. El estado del contacto no se puede modificar.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "change", Icon = "warning" });
                }
                obj.Estado = (sbyte)Status;
                this._context.Attach(obj).State = EntityState.Modified;
                await this._context.SaveChangesAsync();
                await UserActionsAsync("modificó el estado del número de contacto " + obj.Telefono1 + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                if (obj.Estado == 1)
                {
                    Message = "El número " + obj.Telefono1 + " ha sido habilitado.";
                }
                else
                {
                    Message = "El número " + obj.Telefono1 + " ha sido deshabilitado.";
                }
                return new JsonResult(new { Message = Message, Phone = (object)obj, Action = "change", Icon = "success" });
            }
            else
            {
                Message = "Advertencia. La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Phone = (object)null, Action = "change", Icon = "warning" });
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
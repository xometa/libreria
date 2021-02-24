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

namespace RosaMaríaBookstore.Pages.Clients
{
    public class ClientsModel : PageModel
    {
        [BindProperty]
        public Persona Person { get; set; }
        [BindProperty]
        public Institucion Institution { get; set; }
        [BindProperty]
        public Telefono Phone { get; set; }
        [BindProperty]
        public Telefonoinstitucion InstitutionPhone { get; set; }
        public IList<Telefonoinstitucion> phoneList { get; set; }
        String Message = "";
        public int showRegisters { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        private bookstoreContext _context { get; }
        public UploadImage UploadImage { get; set; }
        public string page { get; set; }
        public bool task { get; set; }
        public ClientsModel(bookstoreContext context)
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
                    task = await UploadImage.accessUser(idUser.Value, "Clientes", "/Clients/Clients");
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
        public async Task<PartialViewResult> OnPostPartialView(int? Id, String Option)
        {
            if (Id != 0 && Option.Equals("Client"))
            {
                this.Person = await this._context.Persona
                .FirstOrDefaultAsync(p => p.Id == Id);
                this.Institution = await this._context.Institucion
                .Include(p => p.IdrepresentanteNavigation)
                .FirstOrDefaultAsync(i => i.Idrepresentante == Id);
            }
            if (Id != 0 && Option.Equals("Public"))
            {
                this.Person = await this._context.Persona
                .FirstOrDefaultAsync(p => p.Id == Id);
            }
            if (Option.Equals("Public"))
            {
                return Partial("_ModalPersonPartial", this);
            }
            else
            {

                return Partial("_ModalClientPartial", this);
            }
        }

        public PartialViewResult OnGetPhoneList(int Id, string search, int? quantity, int? currentPage)
        {
            if (Id != 0)
            {
                IQueryable<Telefonoinstitucion> Query = this._context.Telefonoinstitucion
                .Include(t => t.IdtelefonoNavigation)
                .Include(i => i.IdinstitucionNavigation)
                .Where(ti => ti.Idinstitucion == Id)
                .OrderByDescending(ti => ti.Id);
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
                .Where(t => EF.Functions.Like(t.IdtelefonoNavigation.Telefono1, $"%{search}%"))
                .OrderByDescending(t => t.Id);

                this.total = Query.Count();
                Query = Query.Skip((currentPage.Value - 1) * quantity.Value).Take(quantity.Value);

                this.phoneList = Query.ToList();
                this.showRegisters = this.phoneList.Count;
            }
            return Partial("_ModalPhonePartial", this);
        }

        public async Task<JsonResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "save", Icon = "error" });
            }
            else
            {
                if (this.Institution.Nombre != null)
                {
                    if (!String.IsNullOrEmpty(this.Person.Tipo) &&
                        !String.IsNullOrEmpty(this.Person.Nombre) &&
                        !String.IsNullOrEmpty(this.Person.Apellido) &&
                        !String.IsNullOrEmpty(this.Person.Dui) &&
                        !String.IsNullOrEmpty(this.Institution.Nombre) &&
                        !String.IsNullOrEmpty(this.Institution.Direccion))
                    {
                        if (Exists(this.Person.Dui, "", "Institución"))
                        {
                            Message = "El DUI ingresado del representante, ya ha sido registrado.";
                            return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "save", Icon = "warning" });
                        }
                        if (Exists("", this.Institution.Nombre, ""))
                        {
                            Message = "El nombre de la institución educativa ya ha sido ingresado.";
                            return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "save", Icon = "warning" });
                        }

                        this._context.Persona.Add(Person);
                        await this._context.SaveChangesAsync();
                        this.Institution.Idrepresentante = this.Person.Id;
                        this._context.Institucion.Add(Institution);
                        await this._context.SaveChangesAsync();
                        await UserActionsAsync("registro una nueva institución (" + Institution.Nombre + "), la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                        Message = "La institución " + this.Institution.Nombre + " ha sido registrada correctamente.";
                        return new JsonResult(new { Message = Message, Client = (object)Person, Institution = (object)Institution, Action = "save", Icon = "success" });
                    }
                    else
                    {
                        Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                        return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "save", Icon = "warning" });
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(this.Person.Tipo) &&
                        !String.IsNullOrEmpty(this.Person.Nombre) &&
                        !String.IsNullOrEmpty(this.Person.Apellido) &&
                        !String.IsNullOrEmpty(this.Person.Dui))
                    {
                        if (Exists(this.Person.Dui, "", "Público"))
                        {
                            Message = "El DUI ingresado del cliente, ya ha sido registrado.";
                            return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "save", Icon = "warning" });
                        }
                        this._context.Persona.Add(Person);
                        await this._context.SaveChangesAsync();
                        await UserActionsAsync("registro un nuevo cliente (" + Person.Fullname() + "), la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                        Message = "El cliente se ha registrado correctamente";
                        return new JsonResult(new { Message = Message, Client = (object)Person, Institution = (object)null, Action = "save", Icon = "success" });
                    }
                    else
                    {
                        Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                        return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "save", Icon = "warning" });
                    }
                }
            }
        }

        public async Task<JsonResult> OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                Message = "Se han proporcionado datos incorrrectos para el registro, por favor verifique y vuelva a intentarlo.";
                return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "edit", Icon = "error" });
            }
            else
            {
                var person = await this._context.Persona.FirstOrDefaultAsync(p => p.Id == this.Person.Id);
                var institution = await this._context.Institucion.FirstOrDefaultAsync(p => p.Idrepresentante == this.Person.Id);
                if (person != null && institution != null)
                {
                    if (!String.IsNullOrEmpty(this.Person.Nombre) &&
                        !String.IsNullOrEmpty(this.Person.Apellido) &&
                        !String.IsNullOrEmpty(this.Person.Dui) &&
                        !String.IsNullOrEmpty(this.Institution.Nombre) &&
                        !String.IsNullOrEmpty(this.Institution.Direccion))
                    {
                        if (!(person.Dui.Equals(this.Person.Dui)))
                        {
                            if (Exists(this.Person.Dui, "", "Institución"))
                            {
                                Message = "El DUI ingresado para el representante " + this.Person.Nombre.ToLower() + " ya ha sido registrado.";
                                return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "edit", Icon = "error" });
                            }
                        }
                        if (!(institution.Nombre.Equals(this.Institution.Nombre)))
                        {
                            if (Exists("", this.Institution.Nombre, ""))
                            {
                                Message = "El nombre ingresado de la institución ya ha sido registrado.";
                                return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "edit", Icon = "error" });
                            }
                        }
                        person.Nombre = this.Person.Nombre;
                        person.Apellido = this.Person.Apellido;
                        person.Dui = this.Person.Dui;
                        await this._context.SaveChangesAsync();
                        institution.Nombre = this.Institution.Nombre;
                        institution.Direccion = this.Institution.Direccion;
                        await this._context.SaveChangesAsync();
                        await UserActionsAsync("modificó la información de la institución " + institution.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                        Message = "La información de la institución, se ha modificado correctamente.";
                        return new JsonResult(new { Message = Message, Client = (object)person, Institution = (object)institution, Action = "edit", Icon = "success" });
                    }
                    else
                    {

                        Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                        return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "edit", Icon = "warning" });
                    }
                }
                else if (person != null && institution == null)
                {
                    if (!String.IsNullOrEmpty(this.Person.Nombre) &&
                       !String.IsNullOrEmpty(this.Person.Apellido) &&
                       !String.IsNullOrEmpty(this.Person.Dui))
                    {
                        if (!(person.Dui.Equals(this.Person.Dui)))
                        {
                            if (Exists(this.Person.Dui, "", "Público"))
                            {
                                Message = "El DUI ingresado para el cliente " + this.Person.Nombre.ToLower() + " ya ha sido ingresado.";
                                return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "edit", Icon = "error" });
                            }
                        }
                        person.Nombre = this.Person.Nombre;
                        person.Apellido = this.Person.Apellido;
                        person.Dui = this.Person.Dui;
                        await this._context.SaveChangesAsync();
                        await UserActionsAsync("modificó la información del cliente " + person.Fullname() + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                        Message = "La información del cliente, se ha modificado correctamente.";
                        return new JsonResult(new { Message = Message, Client = (object)person, Institution = (object)null, Action = "edit", Icon = "success" });
                    }
                    else
                    {
                        Message = "Campos vacíos, por favor verifique y vuelva ha intentarlo.";
                        return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "edit", Icon = "warning" });
                    }
                }
                else
                {
                    Message = "La información del servidor ha sido alterada.";
                    return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "edit", Icon = "error" });
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
                return _context.Institucion.Where(i => i.Nombre == Name).Any();
            }
            else
            {
                return false;
            }
        }

        public async Task<JsonResult> OnPostDelete(int IdPerson, String Option)
        {
            var person = await this._context.Persona.Include(p => p.Venta).FirstOrDefaultAsync(p => p.Id == IdPerson);
            var institution = await this._context.Institucion.Include(i => i.Telefonoinstitucion).FirstOrDefaultAsync(p => p.Idrepresentante == IdPerson);
            if (person != null && institution == null && Option.Equals("Public"))//es un cliente general
            {
                if (person.Venta.Count > 0)
                {
                    Message = "El cliente " + person.Nombre.ToLower() + " no se puede eliminar, porque posee registros de ventas.";
                    return new JsonResult(new { Message = Message, Client = (object)Person, Institution = (object)null, Action = "delete", Icon = "warning" });
                }
                this._context.Persona.Remove(person);
                await this._context.SaveChangesAsync();
                await UserActionsAsync("eliminó el cliente " + person.Fullname() + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "El cliente se ha eliminado correctamente.";
                return new JsonResult(new { Message = Message, Client = (object)Person, Institution = (object)null, Action = "delete", Icon = "success" });
            }
            else if (person != null && institution != null && Option.Equals("Client"))//es una institución con representante
            {
                if (person.Venta.Count > 0)
                {
                    Message = "El cliente " + institution.Nombre.ToLower() + " no se puede eliminar, porque posee registros de ventas.";
                    return new JsonResult(new { Message = Message, Client = (object)Person, Institution = (object)null, Action = "delete", Icon = "warning" });
                }
                //si posee telefonos la institución procedemos a eliminarlos
                if (institution.Telefonoinstitucion.Count > 0)
                {
                    IList<Telefonoinstitucion> detail = this._context.Telefonoinstitucion.Where(ti => ti.Idinstitucion == institution.Id).ToList();
                    foreach (var phone in detail)
                    {
                        var aux = await this._context.Telefono.FirstOrDefaultAsync(p => p.Id == phone.Idtelefono);
                        if (aux != null)
                        {
                            this._context.Telefonoinstitucion.Remove(phone);
                            await this._context.SaveChangesAsync();
                            this._context.Telefono.Remove(aux);
                            await this._context.SaveChangesAsync();
                        }
                    }
                    await UserActionsAsync("eliminó todos los números de contacto del cliente institucional " + institution.Nombre + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                }
                this._context.Institucion.Remove(institution);
                await this._context.SaveChangesAsync();
                this._context.Persona.Remove(person);
                await this._context.SaveChangesAsync();
                await UserActionsAsync("eliminó la institución " + institution.Nombre + ", la fecha" + DateTime.Now.ToString("dd/MM/yyyy"));
                Message = "La institución se ha eliminado correctamente";
                return new JsonResult(new { Message = Message, Client = (object)Person, Institution = (object)institution, Action = "delete", Icon = "success" });
            }
            else//si no la información se ha alterado, por lo cual se debe informaar al usuario final
            {
                Message = "La información del servidor ha sido alterada.";
                return new JsonResult(new { Message = Message, Client = (object)null, Institution = (object)null, Action = "delete", Icon = "error" });
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
                    this.InstitutionPhone.Idtelefono = this.Phone.Id;
                    this._context.Telefonoinstitucion.Add(this.InstitutionPhone);
                    await this._context.SaveChangesAsync();
                    await UserActionsAsync("registro el número de contacto " + Phone.Telefono1 + ", la fecha " + DateTime.Now.ToString("dd/MM/yyyy"));
                    Message = "El número de contacto se ha registrado correctamente.";
                    return new JsonResult(new { Message = Message, Phone = (object)InstitutionPhone, Action = "savephone", Icon = "success" });
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
                var obj = await this._context.Telefonoinstitucion.FirstOrDefaultAsync(t => t.Idtelefono == this.Phone.Id);
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
                var ti = await this._context.Telefonoinstitucion.FirstOrDefaultAsync(tn => tn.Idtelefono == Id);
                if (phone == null)
                {
                    Message = "El número de contacto que desea eliminar, no existe; verifique y vuelva a intentarlo.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "deletephone", Icon = "warning" });
                }
                if (phone != null && ti == null)
                {
                    Message = "El número de contacto a eliminar no pertenece a esta institución.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "deletephone", Icon = "error" });
                }

                if (phone == null && ti == null)
                {
                    Message = "La información del servidor ha sido alterada.";
                    return new JsonResult(new { Message = Message, Phone = (object)null, Action = "deletephone", Icon = "error" });
                }
                //eliminamos el número de telefono del detalle
                this._context.Telefonoinstitucion.Remove(ti);
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
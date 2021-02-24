using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Login
{
    public class LogoutModel : PageModel
    {
        private bookstoreContext _context { get; }
        public LogoutModel(bookstoreContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> OnGet()
        {
            await Logout();
            return RedirectToPage("/Login/Login");
        }
        public async Task<IActionResult> OnPost()
        {
            await Logout();
            return RedirectToPage("/Login/Login");
        }
        public async Task Logout()
        {
            Bitacora bitacora = this._context.Bitacora.FirstOrDefault(b => b.Id == HttpContext.Session.GetInt32("IdBitacora"));
            if (bitacora != null)
            {
                bitacora.Cierresesion = DateTime.Now;
                this._context.Attach(bitacora).State = EntityState.Modified;
                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine("Mensaje =" + e.Message);
                    throw;
                }
                await UserActionsAsync("cerro sesión, la fecha " + DateTime.Now.ToString("dd/MM/yyyy") + " a la " + DateTime.Now.ToString("hh:mm:ss"));
                HttpContext.Session.Clear();
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
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

namespace RosaMaríaBookstore.Pages
{
    public class ValidationModel:PageModel
    {
        public IList<Detallepermisosusuario> permissionsList {get;set;}
        private bookstoreContext _context {get;}
        public string Message {get;set;}

        public ValidationModel(bookstoreContext context){
            this._context=context;
        }

        public void OnGet(){
            this.Message="Hola";
        }
    }
}
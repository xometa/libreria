using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace RosaMaríaBookstore.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Bitacora = new HashSet<Bitacora>();
            Compra = new HashSet<Compra>();
            Detallepermisosusuario = new HashSet<Detallepermisosusuario>();
            Recuperarcuenta = new HashSet<Recuperarcuenta>();
            Venta = new HashSet<Venta>();
        }

        public int Id { get; set; }
        public string Usuario1 { get; set; }
        public string Contrasena { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public sbyte Estado { get; set; }
        public int Idempleado { get; set; }
        public int? Idimagen { get; set; }
        [NotMapped]
        public IFormFile Archivo { get; set; }

        public virtual Empleado IdempleadoNavigation { get; set; }
        public virtual Imagen IdimagenNavigation { get; set; }
        public virtual ICollection<Bitacora> Bitacora { get; set; }
        public virtual ICollection<Compra> Compra { get; set; }
        public virtual ICollection<Detallepermisosusuario> Detallepermisosusuario { get; set; }
        public virtual ICollection<Recuperarcuenta> Recuperarcuenta { get; set; }
        public virtual ICollection<Venta> Venta { get; set; }
    }
}

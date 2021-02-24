using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Persona
    {
        public Persona()
        {
            Empleado = new HashSet<Empleado>();
            Institucion = new HashSet<Institucion>();
            Proveedor = new HashSet<Proveedor>();
            Venta = new HashSet<Venta>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dui { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<Empleado> Empleado { get; set; }
        public virtual ICollection<Institucion> Institucion { get; set; }
        public virtual ICollection<Proveedor> Proveedor { get; set; }
        public virtual ICollection<Venta> Venta { get; set; }
        public string Fullname()
        {
            return $"{this.Nombre} {this.Apellido}";
        }
    }
}

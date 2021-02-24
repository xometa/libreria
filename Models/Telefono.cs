using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Telefono
    {
        public Telefono()
        {
            Empleado = new HashSet<Empleado>();
            Telefonoinstitucion = new HashSet<Telefonoinstitucion>();
            Telefonoproveedor = new HashSet<Telefonoproveedor>();
        }

        public int Id { get; set; }
        public string Telefono1 { get; set; }
        public sbyte Estado { get; set; }

        public virtual ICollection<Empleado> Empleado { get; set; }
        public virtual ICollection<Telefonoinstitucion> Telefonoinstitucion { get; set; }
        public virtual ICollection<Telefonoproveedor> Telefonoproveedor { get; set; }
    }
}

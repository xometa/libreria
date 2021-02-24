using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Proveedor
    {
        public Proveedor()
        {
            Compra = new HashSet<Compra>();
            Telefonoproveedor = new HashSet<Telefonoproveedor>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int Idrepresentante { get; set; }

        public virtual Persona IdrepresentanteNavigation { get; set; }
        public virtual ICollection<Compra> Compra { get; set; }
        public virtual ICollection<Telefonoproveedor> Telefonoproveedor { get; set; }
    }
}

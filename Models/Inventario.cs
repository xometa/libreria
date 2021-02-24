using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Inventario
    {
        public int Id { get; set; }
        public int Existencia { get; set; }
        public DateTime Fecha { get; set; }
        public int? Idcompra { get; set; }
        public int? Idventa { get; set; }
        public int Idproducto { get; set; }

        public virtual Detallecompra IdcompraNavigation { get; set; }
        public virtual Producto IdproductoNavigation { get; set; }
        public virtual Detalleventa IdventaNavigation { get; set; }
    }
}

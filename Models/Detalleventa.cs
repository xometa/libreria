using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Detalleventa
    {
        public Detalleventa()
        {
            Inventario = new HashSet<Inventario>();
        }

        public int Id { get; set; }
        public int Cantidad { get; set; }
        public int Idventa { get; set; }
        public int Idproducto { get; set; }

        public virtual Preciosproducto IdproductoNavigation { get; set; }
        public virtual Venta IdventaNavigation { get; set; }
        public virtual ICollection<Inventario> Inventario { get; set; }
    }
}

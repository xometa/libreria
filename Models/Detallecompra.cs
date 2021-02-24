using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Detallecompra
    {
        public Detallecompra()
        {
            Inventario = new HashSet<Inventario>();
        }

        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public int Idcompra { get; set; }
        public int Idproducto { get; set; }

        public virtual Compra IdcompraNavigation { get; set; }
        public virtual Producto IdproductoNavigation { get; set; }
        public virtual ICollection<Inventario> Inventario { get; set; }
    }
}

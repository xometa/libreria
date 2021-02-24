using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Preciosproducto
    {
        public Preciosproducto()
        {
            Detalleventa = new HashSet<Detalleventa>();
        }

        public int Id { get; set; }
        public decimal Margen { get; set; }
        public DateTime Fecha { get; set; }
        public sbyte Estado { get; set; }
        public int Idproducto { get; set; }
        public int Idcompra { get; set; }

        public virtual Compra IdcompraNavigation { get; set; }
        public virtual Producto IdproductoNavigation { get; set; }
        public virtual ICollection<Detalleventa> Detalleventa { get; set; }
    }
}

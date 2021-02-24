using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Compra
    {
        public Compra()
        {
            Detallecompra = new HashSet<Detallecompra>();
            Preciosproducto = new HashSet<Preciosproducto>();
        }

        public int Id { get; set; }
        public string Documento { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public int Idusuario { get; set; }
        public int Idproveedor { get; set; }

        public virtual Proveedor IdproveedorNavigation { get; set; }
        public virtual Usuario IdusuarioNavigation { get; set; }
        public virtual ICollection<Detallecompra> Detallecompra { get; set; }
        public virtual ICollection<Preciosproducto> Preciosproducto { get; set; }
    }
}

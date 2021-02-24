using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Venta
    {
        public Venta()
        {
            Detalleventa = new HashSet<Detalleventa>();
            Pago = new HashSet<Pago>();
        }

        public int Id { get; set; }
        public string Documento { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public sbyte? Estado { get; set; }
        public int Idcliente { get; set; }
        public int Idusuario { get; set; }

        public virtual Persona IdclienteNavigation { get; set; }
        public virtual Usuario IdusuarioNavigation { get; set; }
        public virtual ICollection<Detalleventa> Detalleventa { get; set; }
        public virtual ICollection<Pago> Pago { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Pago
    {
        public int Id { get; set; }
        public int Idventa { get; set; }
        public int Idabono { get; set; }

        public virtual Abono IdabonoNavigation { get; set; }
        public virtual Venta IdventaNavigation { get; set; }
    }
}

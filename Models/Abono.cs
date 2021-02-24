using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Abono
    {
        public Abono()
        {
            Pago = new HashSet<Pago>();
        }

        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fechaabono { get; set; }

        public virtual ICollection<Pago> Pago { get; set; }
    }
}

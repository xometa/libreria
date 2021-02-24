using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Cargo
    {
        public Cargo()
        {
            Empleado = new HashSet<Empleado>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Empleado> Empleado { get; set; }
    }
}

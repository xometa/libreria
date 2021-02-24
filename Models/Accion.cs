using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Accion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime Hora { get; set; }
        public int Idbitacora { get; set; }

        public virtual Bitacora IdbitacoraNavigation { get; set; }
    }
}

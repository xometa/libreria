using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Bitacora
    {
        public Bitacora()
        {
            Accion = new HashSet<Accion>();
        }

        public int Id { get; set; }
        public DateTime Iniciosesion { get; set; }
        public DateTime Cierresesion { get; set; }
        public int Idusuario { get; set; }

        public virtual Usuario IdusuarioNavigation { get; set; }
        public virtual ICollection<Accion> Accion { get; set; }
    }
}

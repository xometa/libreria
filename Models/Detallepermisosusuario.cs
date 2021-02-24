using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Detallepermisosusuario
    {
        public int Id { get; set; }
        public int Idusuario { get; set; }
        public int Idpermiso { get; set; }

        public virtual Permisos IdpermisoNavigation { get; set; }
        public virtual Usuario IdusuarioNavigation { get; set; }
    }
}

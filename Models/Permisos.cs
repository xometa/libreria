using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Permisos
    {
        public Permisos()
        {
            Detallepermisosusuario = new HashSet<Detallepermisosusuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Icono { get; set; }

        public virtual ICollection<Detallepermisosusuario> Detallepermisosusuario { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Imagen
    {
        public Imagen()
        {
            Producto = new HashSet<Producto>();
            Usuario = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Producto> Producto { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}

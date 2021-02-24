using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Producto = new HashSet<Producto>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public sbyte Estado { get; set; }

        public virtual ICollection<Producto> Producto { get; set; }
    }
}

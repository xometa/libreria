using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace RosaMaríaBookstore.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Detallecompra = new HashSet<Detallecompra>();
            Inventario = new HashSet<Inventario>();
            Preciosproducto = new HashSet<Preciosproducto>();
        }

        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? Stockminimo { get; set; }
        public sbyte Estado { get; set; }
        public int? Idmarca { get; set; }
        public int Idcategoria { get; set; }
        public int? Idimagen { get; set; }
        [NotMapped]
        public IFormFile Archivo { get; set; }

        public virtual Categoria IdcategoriaNavigation { get; set; }
        public virtual Imagen IdimagenNavigation { get; set; }
        public virtual Marca IdmarcaNavigation { get; set; }
        public virtual ICollection<Detallecompra> Detallecompra { get; set; }
        public virtual ICollection<Inventario> Inventario { get; set; }
        public virtual ICollection<Preciosproducto> Preciosproducto { get; set; }
    }
}

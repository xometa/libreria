using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Recuperarcuenta
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public DateTime Fechaenvio { get; set; }
        public DateTime? Fecharecuperacion { get; set; }
        public sbyte Estado { get; set; }
        public int Idusuario { get; set; }

        public virtual Usuario IdusuarioNavigation { get; set; }
    }
}

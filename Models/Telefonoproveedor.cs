using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Telefonoproveedor
    {
        public int Id { get; set; }
        public int Idproveedor { get; set; }
        public int Idtelefono { get; set; }

        public virtual Proveedor IdproveedorNavigation { get; set; }
        public virtual Telefono IdtelefonoNavigation { get; set; }
    }
}

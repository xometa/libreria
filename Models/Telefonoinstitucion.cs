using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Telefonoinstitucion
    {
        public int Id { get; set; }
        public int Idinstitucion { get; set; }
        public int Idtelefono { get; set; }

        public virtual Institucion IdinstitucionNavigation { get; set; }
        public virtual Telefono IdtelefonoNavigation { get; set; }
    }
}

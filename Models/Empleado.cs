using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Empleado
    {
        public Empleado()
        {
            Usuario = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Sexo { get; set; }
        public DateTime? Fechanacimiento { get; set; }
        public int Idpersona { get; set; }
        public int? Idtelefono { get; set; }
        public int? Idcargo { get; set; }

        public virtual Cargo IdcargoNavigation { get; set; }
        public virtual Persona IdpersonaNavigation { get; set; }
        public virtual Telefono IdtelefonoNavigation { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}

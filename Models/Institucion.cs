using System;
using System.Collections.Generic;

namespace RosaMaríaBookstore.Models
{
    public partial class Institucion
    {
        public Institucion()
        {
            Telefonoinstitucion = new HashSet<Telefonoinstitucion>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int Idrepresentante { get; set; }

        public virtual Persona IdrepresentanteNavigation { get; set; }
        public virtual ICollection<Telefonoinstitucion> Telefonoinstitucion { get; set; }
    }
}

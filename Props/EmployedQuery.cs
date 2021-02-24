using System;
using System.Linq;
namespace RosaMaríaBookstore.Props
{
    public class EmployedQuery
    {
        public int Id {get;set;}
        public int? IdUsuario {get;set;}
        public string Imagen {get;set;}
        public string Nombre {get;set;}
        public string Tipo {get;set;}
        public string Cargo {get;set;}
        public string DUI {get;set;}
        public string Sexo {get;set;}
        public DateTime? Fechanacimiento {get;set;}
        public string Telefono {get;set;}
        public static string sqlEmployed()
        {
            string sql = @"SELECT (em.id) as Id,
                (us.id) as IdUsuario,
                (im.nombre) as Imagen, 
                CONCAT(per.nombre,' ',per.apellido) as Nombre,
                (per.tipo) as Tipo,
                (ca.nombre) as Cargo,
                (per.dui) as DUI,
                (em.sexo) as Sexo,
                (em.fechanacimiento) as Fechanacimiento,
                (tel.telefono) as Telefono
                FROM empleado em 
                INNER JOIN persona per on per.id=em.idpersona 
                LEFT JOIN cargo ca on ca.id=em.idcargo 
                LEFT JOIN telefono tel on tel.id=em.idtelefono 
                LEFT JOIN usuario us on us.idempleado=em.id 
                LEFT JOIN imagen im on im.id=us.idimagen 
                WHERE em.id!={0} 
                ORDER BY Id DESC";
            return sql;
        }
    }
}
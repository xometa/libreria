using System;
using System.Linq;
namespace RosaMaríaBookstore.Props
{
    public class UserActions
    {
        public int Id { get; set; }
        public int Idusuario { get; set; }
        public int Idempleado { get; set; }
        public string Empleado { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fechahora { get; set; }
        public string Imagen { get; set; }
        public static string sqlActions()
        {
            string sql = @"SELECT 
                        (ac.id) as Id,
                        (us.id) as Idusuario,
                        (em.id) as Idempleado,
                        CONCAT(per.nombre,' ',per.apellido) as Empleado,
                        CONCAT(CONCAT(per.nombre,' ',per.apellido),' ',ac.descripcion) as Descripcion,
                        (ac.hora) as Fechahora,
                        CASE WHEN im.nombre IS NULL OR im.nombre=''
	                    THEN
		                    '/images/not-user.png'
	                    ELSE
		                    CONCAT('/images/users/',im.nombre)
                        END as Imagen 
                        FROM bitacora as bt 
                        INNER JOIN accion as ac on ac.idbitacora=bt.id 
                        INNER JOIN usuario as us on us.id=bt.idusuario 
                        INNER JOIN empleado as em on em.id=us.idempleado 
                        INNER JOIN persona as per on per.id=em.idpersona 
                        LEFT JOIN imagen as im on im.id=us.idimagen 
                        WHERE DATE(ac.hora) BETWEEN {0} AND {1} OR us.id={2} 
                        ORDER BY ac.id DESC";
            return sql;
        }
    }
}
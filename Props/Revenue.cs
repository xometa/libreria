using System;
using System.Linq;
namespace RosaMaríaBookstore.Props
{
    public class Revenue
    {
        public int Id { get; set; }
        public string Cadena { get; set; }
        public decimal Ingresos { get; set; }

        public static string lastWeek()
        {
            string sql = @"SELECT (vt.id) as Id,
                        CONVERT((YEARWEEK(vt.fecha)),CHAR) as Cadena,
                        SUM(ROUND((dc.precio+(dc.precio*(pp.margen/100))),2)*dv.cantidad) as Ingresos 
                        FROM venta as vt INNER JOIN detalleventa as dv on dv.idventa=vt.id 
                        INNER JOIN preciosproducto as pp on pp.id=dv.idproducto 
                        INNER JOIN compra as cmp on cmp.id=pp.idcompra 
                        INNER JOIN detallecompra dc on dc.idcompra=cmp.id AND dc.idproducto=pp.idproducto
                        WHERE YEARWEEK(vt.fecha)=YEARWEEK(NOW()-INTERVAL 7 DAY) 
                        GROUP BY vt.id 
                        ORDER BY vt.id DESC";
            return sql;
        }
        public static string currentWeek()
        {
            string sql = @"SELECT (vt.id) as Id,
                        CONVERT((YEARWEEK(vt.fecha)),CHAR) as Cadena,
                        SUM(ROUND((dc.precio+(dc.precio*(pp.margen/100))),2)*dv.cantidad) as Ingresos 
                        FROM venta as vt INNER JOIN detalleventa as dv on dv.idventa=vt.id 
                        INNER JOIN preciosproducto as pp on pp.id=dv.idproducto 
                        INNER JOIN compra as cmp on cmp.id=pp.idcompra 
                        INNER JOIN detallecompra dc on dc.idcompra=cmp.id AND dc.idproducto=pp.idproducto
                        WHERE YEARWEEK(NOW()) = YEARWEEK(vt.fecha) 
                        GROUP BY vt.id 
                        ORDER BY vt.id DESC";
            return sql;
        }
        public static string currentWeekRevenue()
        {
            string sql = @"SELECT (vt.id) as Id,
                        ELT(WEEKDAY(vt.fecha)+1,'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado', 'Domingo') as Cadena,
                        SUM(ROUND((dc.precio+(dc.precio*(pp.margen/100))),2)*dv.cantidad) as Ingresos
                        FROM venta as vt INNER JOIN detalleventa as dv on dv.idventa=vt.id 
                        INNER JOIN preciosproducto as pp on pp.id=dv.idproducto 
                        INNER JOIN compra as cmp on cmp.id=pp.idcompra 
                        INNER JOIN detallecompra dc on dc.idcompra=cmp.id AND dc.idproducto=pp.idproducto
                        WHERE YEARWEEK(vt.fecha)=YEARWEEK(CURDATE()) 
                        GROUP BY (WEEKDAY(vt.fecha)+1)
                        ORDER BY vt.id DESC";
            return sql;
        }
        public static string lastWeekRevenue()
        {
            string sql = @"SELECT (vt.id) as Id,
                        ELT(WEEKDAY(vt.fecha)+1,'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado', 'Domingo') as Cadena,
                        SUM(ROUND((dc.precio+(dc.precio*(pp.margen/100))),2)*dv.cantidad) as Ingresos
                        FROM venta as vt INNER JOIN detalleventa as dv on dv.idventa=vt.id 
                        INNER JOIN preciosproducto as pp on pp.id=dv.idproducto 
                        INNER JOIN compra as cmp on cmp.id=pp.idcompra 
                        INNER JOIN detallecompra dc on dc.idcompra=cmp.id AND dc.idproducto=pp.idproducto
                        WHERE YEARWEEK(vt.fecha)=YEARWEEK(NOW()-INTERVAL 7 DAY) 
                        GROUP BY (WEEKDAY(vt.fecha)+1)
                        ORDER BY vt.id DESC";
            return sql;
        }
    }
}
using System;
using System.Linq;
namespace RosaMaríaBookstore.Props
{
    public class LastPurchasePrice
    {
        
        public int Id { get; set; }
        public decimal PrecioCompra { get; set; }
        public DateTime FechaCompra{get;set;}
        public int IdProducto { get; set; }
        public int IdDetalleCompra { get; set; }
        public static string sqlLastPrice()
        {
            string sql = @"SELECT 
                (Compra_Max.Max_Id) as Id,
                (dt.precio) as PrecioCompra,
                (Compra_Max.Max_Fecha) as FechaCompra,
                (dt.idproducto ) as IdProducto,
                (dt.id) as IdDetalleCompra
                FROM detallecompra as dt 
                INNER JOIN 
                (
                    SELECT 
                    MAX(cn.fecha) as Max_Fecha,
                    MAX(cn.id) as Max_Id,
                    dtc.idproducto
                    FROM compra as cn 
                    INNER JOIN detallecompra as dtc 
                    on dtc.idcompra=cn.id 
                    WHERE dtc.idproducto= {0}
                ) as Compra_Max
                on dt.idcompra=Compra_Max.Max_Id
                and dt.idproducto=Compra_Max.idproducto
            ";
            return sql;
        }
    }
}
/*
SELECT dc.id,dc.idcompra,dc.idproducto,dc.precio 
FROM detallecompra as dc 
INNER JOIN 
(
SELECT MAX(cn.id) as Max_Id
FROM compra as cn 
INNER JOIN detallecompra as dtc 
on dtc.idcompra=cn.id 
WHERE dtc.idproducto=21
) as Max_Compra 
on dc.idcompra=Max_Compra.Max_Id 
ORDER BY dc.id desc
*/

/*
SELECT 
(Compra_Max.Max_Id) as Id,
(dt.precio) as Preciocompra,
(Compra_Max.Max_Fecha) as Fechacompra,
(dt.idproducto ) as Idproducto,
(dt.id) as Iddetallecompra
FROM detallecompra as dt 
INNER JOIN 
(
SELECT 
MAX(cn.fecha) as Max_Fecha,
MAX(cn.id) as Max_Id,
dtc.idproducto
FROM compra as cn 
INNER JOIN detallecompra as dtc 
on dtc.idcompra=cn.id 
WHERE dtc.idproducto=21
) as Compra_Max
on dt.idcompra=Compra_Max.Max_Id
and dt.idproducto=Compra_Max.idproducto
*/
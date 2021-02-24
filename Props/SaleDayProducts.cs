using System;
using System.Linq;
namespace RosaMaríaBookstore.Props
{
    public class SaleDayProducts
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int IdCompra { get; set; }
        public int IdVenta { get; set; }
        public string Producto { get; set; }
        public string Imagen { get; set; }
        public string Categoria { get; set; }
        public DateTime FechaPrecioCompra { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal Margen { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }

        public static string dayProduct()
        {
            string sql = @"SELECT (pp.id) as Id,
                        (pr.id) as IdProducto,
                        (cmp.id) as IdCompra,
                        (vnt.id) as IdVenta,
                        (pr.nombre) as Producto,
                        CASE WHEN img.nombre IS NULL OR img.nombre='' 
	                        THEN
		                        '/images/not-image.png'
	                        ELSE
		                        CONCAT('/images/products/',img.nombre)
                        END as Imagen,
                        (cat.nombre) as Categoria,
                        (pp.fecha) as FechaPrecioCompra,
                        (dc.precio) as PrecioCompra,
                        (pp.margen) as Margen,
                        (dc.precio+(dc.precio*(pp.margen/100))) as PrecioVenta,
                        SUM(dv.cantidad) as Cantidad
                        FROM producto as pr 
                        LEFT JOIN imagen as img on img.id=pr.idimagen 
                        INNER JOIN categoria as cat on cat.id=pr.idcategoria 
                        INNER JOIN preciosproducto as pp on pp.idproducto=pr.id 
                        INNER JOIN compra as cmp on cmp.id=pp.idcompra 
                        INNER JOIN detallecompra dc on dc.idcompra=cmp.id AND dc.idproducto=pr.id
                        INNER JOIN detalleventa as dv on dv.idproducto=pp.id 
                        INNER JOIN venta as vnt on vnt.id=dv.idventa 
                        WHERE DATE(vnt.fecha)=CURRENT_DATE 
                        GROUP BY pp.id
                        ORDER BY cat.nombre ASC";
            return sql;
        }
    }
}
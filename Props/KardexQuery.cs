using System;
using System.Linq;
namespace RosaMaríaBookstore.Props
{
    public class KardexQuery
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int? IdCompra { get; set; }
        public int? IdVenta { get; set; }
        public string Documento { get; set; }
        public DateTime Fecha { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int StockMinimo { get; set; }
        public int Existencia { get; set; }

        public static string stockAvailable()
        {
            /*string sql = @"(
                    SELECT (inv.id) as Id, 
                    (pr.id) as IdProducto,
                    (pr.nombre) as Producto,
                    (inv.existencia) as Existencia,
                    (pr.stockminimo) as StockMinimo 
                    FROM producto as pr 
                    INNER JOIN detallecompra as dc on dc.idproducto=pr.id 
                    INNER JOIN inventario as inv on inv.idcompra=dc.id 
                    WHERE pr.id={0} ORDER BY inv.id DESC LIMIT 1
                )
                UNION ALL 
                (
                    SELECT (inv.id) as Id, 
                    (pr.id) as IdProducto,
                    (pr.nombre) as Producto,
                    (inv.existencia) as Existencia,
                    (pr.stockminimo) as StockMinimo 
                    FROM producto as pr 
                    INNER JOIN preciosproducto as pp on pp.idproducto=pr.id
                    INNER JOIN detalleventa as dv on dv.idproducto=pp.id 
                    INNER JOIN inventario as inv on inv.idventa=dv.id
                    WHERE pr.id={1} ORDER BY inv.id DESC LIMIT 1
                )";*/
            string sql = @"(
                    SELECT (inv.id) as Id,
                    (pr.id) as IdProducto,	
					(inv.idcompra)as IdCompra,	
					(inv.idventa)as IdVenta,
					(cmp.documento) as Documento,
					(inv.fecha) as Fecha,
                    (pr.nombre) as Producto,
					(dc.cantidad) as Cantidad,
					(dc.precio) as PrecioUnitario,
                    (pr.stockminimo) as StockMinimo,
                    (inv.existencia) as Existencia
                    FROM producto as pr 
                    INNER JOIN detallecompra as dc on dc.idproducto=pr.id 
					INNER JOIN compra cmp on cmp.id=dc.idcompra
                    INNER JOIN inventario as inv on inv.idcompra=dc.id 
                    WHERE pr.id={0} ORDER BY inv.id DESC LIMIT 1
                ) UNION ALL 
                (
                    SELECT (inv.id) as Id, 
                    (pr.id) as IdProducto,
					(inv.idcompra)as IdCompra,	
					(inv.idventa)as IdVenta,	
					(vnt.documento) as Documento,
					(inv.fecha) as Fecha,
                    (pr.nombre) as Producto,
					(dv.cantidad) as Cantidad,
					ROUND((dc.precio+(dc.precio*(pp.margen/100))),2) as PrecioUnitario,
                    (pr.stockminimo) as StockMinimo,
                    (inv.existencia) as Existencia
                    FROM producto as pr 
                    INNER JOIN preciosproducto as pp on pp.idproducto=pr.id 
					INNER JOIN compra as cmp on cmp.id=pp.idcompra 
					INNER JOIN detallecompra as dc on dc.idcompra=cmp.id and dc.idproducto=pr.id 
                    INNER JOIN detalleventa as dv on dv.idproducto=pp.id 
					INNER JOIN venta as vnt on vnt.id=dv.idventa
                    INNER JOIN inventario as inv on inv.idventa=dv.id
                    WHERE pr.id={1} ORDER BY inv.id DESC LIMIT 1
                )";
            return sql;
        }

        public static string kardex()
        {
            string sql = @"(
                    SELECT (inv.id) as Id,
                    (pr.id) as IdProducto,	
					(inv.idcompra)as IdCompra,	
					(inv.idventa)as IdVenta,
					(cmp.documento) as Documento,
					(inv.fecha) as Fecha,
                    (pr.nombre) as Producto,
					(dc.cantidad) as Cantidad,
					(dc.precio) as PrecioUnitario,
                    (pr.stockminimo) as StockMinimo,
                    (inv.existencia) as Existencia
                    FROM producto as pr 
                    INNER JOIN detallecompra as dc on dc.idproducto=pr.id 
					INNER JOIN compra cmp on cmp.id=dc.idcompra
                    INNER JOIN inventario as inv on inv.idcompra=dc.id 
                    WHERE pr.id={0}
                ) union all (
                    SELECT (inv.id) as Id, 
                    (pr.id) as IdProducto,
					(inv.idcompra)as IdCompra,	
					(inv.idventa)as IdVenta,
					(vnt.documento) as Documento,	
					(inv.fecha) as Fecha,
                    (pr.nombre) as Producto,
					(dv.cantidad) as Cantidad,
					ROUND((dc.precio+(dc.precio*(pp.margen/100))),2) as PrecioUnitario,
                    (pr.stockminimo) as StockMinimo,
                    (inv.existencia) as Existencia
                    FROM producto as pr 
                    INNER JOIN preciosproducto as pp on pp.idproducto=pr.id 
					INNER JOIN compra as cmp on cmp.id=pp.idcompra 
				    INNER JOIN detallecompra as dc on dc.idcompra=cmp.id and dc.idproducto=pr.id 
                    INNER JOIN detalleventa as dv on dv.idproducto=pp.id 
					INNER JOIN venta as vnt on vnt.id=dv.idventa
                    INNER JOIN inventario as inv on inv.idventa=dv.id
                    WHERE pr.id={1}
                )";
            return sql;
        }
    }
}
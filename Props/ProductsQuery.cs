using System;
using System.Linq;
namespace RosaMaríaBookstore.Props
{
    public class ProductsQuery
    {
        public int Id { get; set; }
        public int? Idinventario { get; set; }
        public int? Idcompra { get; set; }
        public int? Idventa { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public string Marca { get; set; }
        public int Existencia { get; set; }
        public int Stockminimo { get; set; }
        public string Imagen { get; set; }
        public sbyte Estado { get; set; }
        public static string sqlProductsAvailable()
        {
            //para la vista productos
            string sql = @"SELECT (pr.id) as Id,
                        (ivt.id) as Idinventario,
                        (it.idcompra) as Idcompra,
                        (it.idventa) as Idventa,
                        (pr.nombre) as Nombre,
                        (pr.descripcion) as Descripcion,
                        (
                            SELECT ct.nombre 
                            FROM categoria as ct 
                            WHERE ct.id=pr.idcategoria 
                            LIMIT 1
                        ) as Categoria,
                        (
                            SELECT m.nombre 
                            FROM marca as m 
                            WHERE m.id=pr.idmarca 
                            LIMIT 1
                        ) as Marca,
                        CASE WHEN it.existencia IS NULL OR it.existencia='' 
                        THEN
	                        0
                        ELSE
	                        it.existencia
                        END as Existencia,
                        (pr.stockminimo) as Stockminimo,
                        (
                            SELECT img.nombre 
                            FROM imagen as img 
                            WHERE img.id=pr.idimagen 
                            LIMIT 1
                        ) as Imagen,
                        (pr.estado) as Estado
                        FROM 
                        (
                                SELECT MAX(inv.id) as Id 
                                FROM inventario as inv 
                                WHERE inv.idproducto 
                                IN (
                                        SELECT p.id FROM producto as p
                                    )
                                GROUP BY inv.idproducto
                        ) as ivt 
                        INNER JOIN inventario as it 
                        ON it.id=ivt.Id 
                        RIGHT JOIN producto as pr 
                        ON pr.id=it.idproducto 
                        ORDER BY pr.id ASC";
            return sql;
        }

        public static string sqlMinimumStock()
        {
            //productos que estan por debajo del stock minímo
            string sql = @"SELECT (pr.id) as Id,
                        (ivt.id) as Idinventario,
                        (it.idcompra) as Idcompra,
                        (it.idventa) as Idventa,
                        (pr.nombre) as Nombre,
                        (pr.descripcion) as Descripcion,
                        (
                            SELECT ct.nombre 
                            FROM categoria as ct 
                            WHERE ct.id=pr.idcategoria 
                            LIMIT 1
                        ) as Categoria,
                        (
                            SELECT m.nombre 
                            FROM marca as m 
                            WHERE m.id=pr.idmarca 
                            LIMIT 1
                        ) as Marca,
                        CASE WHEN it.existencia IS NULL OR it.existencia='' 
                        THEN
	                        0
                        ELSE
	                        it.existencia
                        END as Existencia,
                        (pr.stockminimo) as Stockminimo,
                        (
                            SELECT img.nombre 
                            FROM imagen as img 
                            WHERE img.id=pr.idimagen 
                            LIMIT 1
                        ) as Imagen,
                        (pr.estado) as Estado
                        FROM 
                        (
                                SELECT MAX(inv.id) as Id 
                                FROM inventario as inv 
                                WHERE inv.idproducto 
                                IN (
                                        SELECT p.id FROM producto as p
                                    )
                                GROUP BY inv.idproducto
                        ) as ivt 
                        INNER JOIN inventario as it 
                        ON it.id=ivt.Id 
                        INNER JOIN producto as pr 
                        ON pr.id=it.idproducto 
                        WHERE it.existencia<=pr.stockminimo 
                        ORDER BY pr.id ASC";
            return sql;
        }

        public static string sqlInventoryProducts()
        {
            //para la vista de ventas
            string sql = @"SELECT (pr.id) as Id,
                        (ivt.id) as Idinventario,
                        (it.idcompra) as Idcompra,
                        (it.idventa) as Idventa,
                        (pr.nombre) as Nombre,
                        (pr.descripcion) as Descripcion,
                        (
                            SELECT ct.nombre 
                            FROM categoria as ct 
                            WHERE ct.id=pr.idcategoria 
                            LIMIT 1
                        ) as Categoria,
                        (
                            SELECT m.nombre 
                            FROM marca as m 
                            WHERE m.id=pr.idmarca 
                            LIMIT 1
                        ) as Marca,
                        CASE WHEN it.existencia IS NULL OR it.existencia='' 
                        THEN
	                        0
                        ELSE
	                        it.existencia
                        END as Existencia,
                        (pr.stockminimo) as Stockminimo,
                        (
                            SELECT img.nombre 
                            FROM imagen as img 
                            WHERE img.id=pr.idimagen 
                            LIMIT 1
                        ) as Imagen,
                        (pr.estado) as Estado
                        FROM 
                        (
                                SELECT MAX(inv.id) as Id 
                                FROM inventario as inv 
                                WHERE inv.idproducto 
                                IN (
                                        SELECT p.id FROM producto as p
                                    )
                                GROUP BY inv.idproducto
                        ) as ivt 
                        INNER JOIN inventario as it 
                        ON it.id=ivt.Id 
                        INNER JOIN producto as pr 
                        ON pr.id=it.idproducto 
                        WHERE it.existencia>0 
                        ORDER BY pr.id ASC";
            return sql;
        }
        /*
                public static string sqlProductsShopping()
                {
                    //para la vista compras
                    string sql = @"SELECT (pr.id) as Id,
                                (ivt.id) as Idinventario,
                                (it.idcompra) as Idcompra,
                                (it.idventa) as Idventa,
                                (pr.nombre) as Nombre,
                                (
                                    SELECT ct.nombre 
                                    FROM categoria as ct 
                                    WHERE ct.id=pr.idcategoria 
                                    LIMIT 1
                                ) as Categoria,
                                (
                                    SELECT m.nombre 
                                    FROM marca as m 
                                    WHERE m.id=pr.idmarca 
                                    LIMIT 1
                                ) as Marca,
                                CASE WHEN it.existencia IS NULL OR it.existencia='' 
                                THEN
                                    0
                                ELSE
                                    it.existencia
                                END as Existencia,
                                (pr.stockminimo) as Stockminimo,
                                (
                                    SELECT img.nombre 
                                    FROM imagen as img 
                                    WHERE img.id=pr.idimagen 
                                    LIMIT 1
                                ) as Imagen
                                FROM 
                                (
                                        SELECT MAX(inv.id) as Id 
                                        FROM inventario as inv 
                                        WHERE inv.idproducto 
                                        IN (
                                                SELECT p.id FROM producto as p
                                            )
                                        GROUP BY inv.idproducto
                                ) as ivt 
                                INNER JOIN inventario as it 
                                ON it.id=ivt.Id 
                                RIGHT JOIN producto as pr 
                                ON pr.id=it.idproducto 
                                WHERE pr.estado=1
                                ORDER BY pr.id ASC";
                    return sql;
                }*/
    }
}
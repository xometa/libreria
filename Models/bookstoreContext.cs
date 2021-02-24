using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using RosaMaríaBookstore.Props;

namespace RosaMaríaBookstore.Models
{
    public partial class bookstoreContext : DbContext
    {
        public bookstoreContext()
        {
        }

        public bookstoreContext(DbContextOptions<bookstoreContext> options, IConfiguration Configuration)
            : base(options)
        {
            this.Configuration = Configuration;
        }

        public virtual DbSet<Abono> Abono { get; set; }
        public virtual DbSet<Accion> Accion { get; set; }
        public virtual DbSet<Bitacora> Bitacora { get; set; }
        public virtual DbSet<Cargo> Cargo { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Compra> Compra { get; set; }
        public virtual DbSet<Detallecompra> Detallecompra { get; set; }
        public virtual DbSet<Detallepermisosusuario> Detallepermisosusuario { get; set; }
        public virtual DbSet<Detalleventa> Detalleventa { get; set; }
        public virtual DbSet<Empleado> Empleado { get; set; }
        public virtual DbSet<Imagen> Imagen { get; set; }
        public virtual DbSet<Institucion> Institucion { get; set; }
        public virtual DbSet<Inventario> Inventario { get; set; }
        public virtual DbSet<Marca> Marca { get; set; }
        public virtual DbSet<Pago> Pago { get; set; }
        public virtual DbSet<Permisos> Permisos { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Preciosproducto> Preciosproducto { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<Recuperarcuenta> Recuperarcuenta { get; set; }
        public virtual DbSet<Telefono> Telefono { get; set; }
        public virtual DbSet<Telefonoinstitucion> Telefonoinstitucion { get; set; }
        public virtual DbSet<Telefonoproveedor> Telefonoproveedor { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Venta> Venta { get; set; }
        public virtual DbSet<LastPurchasePrice> LastPurchasePrice { get; set; }
        public virtual DbSet<EmployedQuery> EmployedQuery { get; set; }
        public virtual DbSet<UserActions> UserActions { get; set; }
        public virtual DbSet<ProductsQuery> ProductsQuery { get; set; }
        public virtual DbSet<KardexQuery> KardexQuery { get; set; }
        public virtual DbSet<SaleDayProducts> SaleDayProducts { get; set; }
        public virtual DbSet<Revenue> Revenue { get; set; }
        public IConfiguration Configuration { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(Configuration["Conecction"], x => x.ServerVersion("10.4.11-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Abono>(entity =>
            {
                entity.ToTable("abono");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Fechaabono)
                    .HasColumnName("fechaabono")
                    .HasColumnType("date");

                entity.Property(e => e.Monto)
                    .HasColumnName("monto")
                    .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<Accion>(entity =>
            {
                entity.ToTable("accion");

                entity.HasIndex(e => e.Idbitacora)
                    .HasName("fk_bitacora_accion");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Hora)
                    .HasColumnName("hora")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.Idbitacora)
                    .HasColumnName("idbitacora")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdbitacoraNavigation)
                    .WithMany(p => p.Accion)
                    .HasForeignKey(d => d.Idbitacora)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_bitacora_accion");
            });

            modelBuilder.Entity<Bitacora>(entity =>
            {
                entity.ToTable("bitacora");

                entity.HasIndex(e => e.Idusuario)
                    .HasName("fk_usuario_bitacora");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cierresesion)
                    .HasColumnName("cierresesion")
                    .HasColumnType("datetime");

                entity.Property(e => e.Idusuario)
                    .HasColumnName("idusuario")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Iniciosesion)
                    .HasColumnName("iniciosesion")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdusuarioNavigation)
                    .WithMany(p => p.Bitacora)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_usuario_bitacora");
            });

            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.ToTable("cargo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("categoria");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");
            });

            modelBuilder.Entity<Compra>(entity =>
            {
                entity.ToTable("compra");

                entity.HasIndex(e => e.Idproveedor)
                    .HasName("fk_proveedor_compra");

                entity.HasIndex(e => e.Idusuario)
                    .HasName("fk_usuario_compra");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Documento)
                    .IsRequired()
                    .HasColumnName("documento")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("date");

                entity.Property(e => e.Idproveedor)
                    .HasColumnName("idproveedor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idusuario)
                    .HasColumnName("idusuario")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("tipo")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.HasOne(d => d.IdproveedorNavigation)
                    .WithMany(p => p.Compra)
                    .HasForeignKey(d => d.Idproveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_proveedor_compra");

                entity.HasOne(d => d.IdusuarioNavigation)
                    .WithMany(p => p.Compra)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_usuario_compra");
            });

            modelBuilder.Entity<Detallecompra>(entity =>
            {
                entity.ToTable("detallecompra");

                entity.HasIndex(e => e.Idcompra)
                    .HasName("fk_compra_detallecompra");

                entity.HasIndex(e => e.Idproducto)
                    .HasName("fk_producto_detallecompra");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cantidad)
                    .HasColumnName("cantidad")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idcompra)
                    .HasColumnName("idcompra")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idproducto)
                    .HasColumnName("idproducto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Precio)
                    .HasColumnName("precio")
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(d => d.IdcompraNavigation)
                    .WithMany(p => p.Detallecompra)
                    .HasForeignKey(d => d.Idcompra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_compra_detallecompra");

                entity.HasOne(d => d.IdproductoNavigation)
                    .WithMany(p => p.Detallecompra)
                    .HasForeignKey(d => d.Idproducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_producto_detallecompra");
            });

            modelBuilder.Entity<Detallepermisosusuario>(entity =>
            {
                entity.ToTable("detallepermisosusuario");

                entity.HasIndex(e => e.Idpermiso)
                    .HasName("fk_permisos_detallepermiso");

                entity.HasIndex(e => e.Idusuario)
                    .HasName("fk_usuario_detallepermiso");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idpermiso)
                    .HasColumnName("idpermiso")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idusuario)
                    .HasColumnName("idusuario")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdpermisoNavigation)
                    .WithMany(p => p.Detallepermisosusuario)
                    .HasForeignKey(d => d.Idpermiso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_permisos_detallepermiso");

                entity.HasOne(d => d.IdusuarioNavigation)
                    .WithMany(p => p.Detallepermisosusuario)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_usuario_detallepermiso");
            });

            modelBuilder.Entity<Detalleventa>(entity =>
            {
                entity.ToTable("detalleventa");

                entity.HasIndex(e => e.Idproducto)
                    .HasName("fk_producto_detalleventa");

                entity.HasIndex(e => e.Idventa)
                    .HasName("fk_venta_detalleventa");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cantidad)
                    .HasColumnName("cantidad")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idproducto)
                    .HasColumnName("idproducto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idventa)
                    .HasColumnName("idventa")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdproductoNavigation)
                    .WithMany(p => p.Detalleventa)
                    .HasForeignKey(d => d.Idproducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_producto_detalleventa");

                entity.HasOne(d => d.IdventaNavigation)
                    .WithMany(p => p.Detalleventa)
                    .HasForeignKey(d => d.Idventa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_venta_detalleventa");
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.ToTable("empleado");

                entity.HasIndex(e => e.Idcargo)
                    .HasName("fk_cargo_empleado");

                entity.HasIndex(e => e.Idpersona)
                    .HasName("fk_persona_empleado");

                entity.HasIndex(e => e.Idtelefono)
                    .HasName("fk_telefono_empleado");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Fechanacimiento)
                    .HasColumnName("fechanacimiento")
                    .HasColumnType("date");

                entity.Property(e => e.Idcargo)
                    .HasColumnName("idcargo")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idpersona)
                    .HasColumnName("idpersona")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idtelefono)
                    .HasColumnName("idtelefono")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Sexo)
                    .IsRequired()
                    .HasColumnName("sexo")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.HasOne(d => d.IdcargoNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.Idcargo)
                    .HasConstraintName("fk_cargo_empleado");

                entity.HasOne(d => d.IdpersonaNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.Idpersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_persona_empleado");

                entity.HasOne(d => d.IdtelefonoNavigation)
                    .WithMany(p => p.Empleado)
                    .HasForeignKey(d => d.Idtelefono)
                    .HasConstraintName("fk_telefono_empleado");
            });

            modelBuilder.Entity<Imagen>(entity =>
            {
                entity.ToTable("imagen");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");
            });

            modelBuilder.Entity<Institucion>(entity =>
            {
                entity.ToTable("institucion");

                entity.HasIndex(e => e.Idrepresentante)
                    .HasName("fk_cliente_institucion");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasColumnName("direccion")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Idrepresentante)
                    .HasColumnName("idrepresentante")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.HasOne(d => d.IdrepresentanteNavigation)
                    .WithMany(p => p.Institucion)
                    .HasForeignKey(d => d.Idrepresentante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cliente_institucion");
            });

            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.ToTable("inventario");

                entity.HasIndex(e => e.Idcompra)
                    .HasName("fk_compra_inventario");

                entity.HasIndex(e => e.Idproducto)
                    .HasName("fk_producto_inventario");

                entity.HasIndex(e => e.Idventa)
                    .HasName("fk_venta_inventario");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Existencia)
                    .HasColumnName("existencia")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("date");

                entity.Property(e => e.Idcompra)
                    .HasColumnName("idcompra")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idproducto)
                    .HasColumnName("idproducto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idventa)
                    .HasColumnName("idventa")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdcompraNavigation)
                    .WithMany(p => p.Inventario)
                    .HasForeignKey(d => d.Idcompra)
                    .HasConstraintName("fk_compra_inventario");

                entity.HasOne(d => d.IdproductoNavigation)
                    .WithMany(p => p.Inventario)
                    .HasForeignKey(d => d.Idproducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_producto_inventario");

                entity.HasOne(d => d.IdventaNavigation)
                    .WithMany(p => p.Inventario)
                    .HasForeignKey(d => d.Idventa)
                    .HasConstraintName("fk_venta_inventario");
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.ToTable("marca");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.ToTable("pago");

                entity.HasIndex(e => e.Idabono)
                    .HasName("fk_abono_pago");

                entity.HasIndex(e => e.Idventa)
                    .HasName("fk_venta_pago");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idabono)
                    .HasColumnName("idabono")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idventa)
                    .HasColumnName("idventa")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdabonoNavigation)
                    .WithMany(p => p.Pago)
                    .HasForeignKey(d => d.Idabono)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_abono_pago");

                entity.HasOne(d => d.IdventaNavigation)
                    .WithMany(p => p.Pago)
                    .HasForeignKey(d => d.Idventa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_venta_pago");
            });

            modelBuilder.Entity<Permisos>(entity =>
            {
                entity.ToTable("permisos");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Icono)
                    .HasColumnName("icono")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("persona");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasColumnName("apellido")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Dui)
                    .HasColumnName("dui")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("tipo")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");
            });

            modelBuilder.Entity<Preciosproducto>(entity =>
            {
                entity.ToTable("preciosproducto");

                entity.HasIndex(e => e.Idcompra)
                    .HasName("fk_compra_precioventa");

                entity.HasIndex(e => e.Idproducto)
                    .HasName("fk_precio_producto");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("date");

                entity.Property(e => e.Idcompra)
                    .HasColumnName("idcompra")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idproducto)
                    .HasColumnName("idproducto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Margen)
                    .HasColumnName("margen")
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(d => d.IdcompraNavigation)
                    .WithMany(p => p.Preciosproducto)
                    .HasForeignKey(d => d.Idcompra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_compra_precioventa");

                entity.HasOne(d => d.IdproductoNavigation)
                    .WithMany(p => p.Preciosproducto)
                    .HasForeignKey(d => d.Idproducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_precio_producto");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("producto");

                entity.HasIndex(e => e.Idcategoria)
                    .HasName("fk_categoria_producto");

                entity.HasIndex(e => e.Idimagen)
                    .HasName("fk_imagen_producto");

                entity.HasIndex(e => e.Idmarca)
                    .HasName("fk_marca_producto");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnName("codigo")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Idcategoria)
                    .HasColumnName("idcategoria")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idimagen)
                    .HasColumnName("idimagen")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idmarca)
                    .HasColumnName("idmarca")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Stockminimo)
                    .HasColumnName("stockminimo")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.IdcategoriaNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.Idcategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_categoria_producto");

                entity.HasOne(d => d.IdimagenNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.Idimagen)
                    .HasConstraintName("fk_imagen_producto");

                entity.HasOne(d => d.IdmarcaNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.Idmarca)
                    .HasConstraintName("fk_marca_producto");
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.ToTable("proveedor");

                entity.HasIndex(e => e.Idrepresentante)
                    .HasName("fk_representante_proveedor");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasColumnName("direccion")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Idrepresentante)
                    .HasColumnName("idrepresentante")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.HasOne(d => d.IdrepresentanteNavigation)
                    .WithMany(p => p.Proveedor)
                    .HasForeignKey(d => d.Idrepresentante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_representante_proveedor");
            });

            modelBuilder.Entity<Recuperarcuenta>(entity =>
            {
                entity.ToTable("recuperarcuenta");

                entity.HasIndex(e => e.Idusuario)
                    .HasName("fk_usuario_recuperarc");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnName("codigo")
                    .HasColumnType("varchar(8)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Fechaenvio)
                    .HasColumnName("fechaenvio")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fecharecuperacion)
                    .HasColumnName("fecharecuperacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.Idusuario)
                    .HasColumnName("idusuario")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdusuarioNavigation)
                    .WithMany(p => p.Recuperarcuenta)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_usuario_recuperarc");
            });

            modelBuilder.Entity<Telefono>(entity =>
            {
                entity.ToTable("telefono");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Telefono1)
                    .IsRequired()
                    .HasColumnName("telefono")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");
            });

            modelBuilder.Entity<Telefonoinstitucion>(entity =>
            {
                entity.ToTable("telefonoinstitucion");

                entity.HasIndex(e => e.Idinstitucion)
                    .HasName("fk_institucion_telefono");

                entity.HasIndex(e => e.Idtelefono)
                    .HasName("fk_telefono_institucion");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idinstitucion)
                    .HasColumnName("idinstitucion")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idtelefono)
                    .HasColumnName("idtelefono")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdinstitucionNavigation)
                    .WithMany(p => p.Telefonoinstitucion)
                    .HasForeignKey(d => d.Idinstitucion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_institucion_telefono");

                entity.HasOne(d => d.IdtelefonoNavigation)
                    .WithMany(p => p.Telefonoinstitucion)
                    .HasForeignKey(d => d.Idtelefono)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_telefono_institucion");
            });

            modelBuilder.Entity<Telefonoproveedor>(entity =>
            {
                entity.ToTable("telefonoproveedor");

                entity.HasIndex(e => e.Idproveedor)
                    .HasName("fk_telefono_proveedor");

                entity.HasIndex(e => e.Idtelefono)
                    .HasName("fk_proveedor_telefono");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idproveedor)
                    .HasColumnName("idproveedor")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idtelefono)
                    .HasColumnName("idtelefono")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdproveedorNavigation)
                    .WithMany(p => p.Telefonoproveedor)
                    .HasForeignKey(d => d.Idproveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_telefono_proveedor");

                entity.HasOne(d => d.IdtelefonoNavigation)
                    .WithMany(p => p.Telefonoproveedor)
                    .HasForeignKey(d => d.Idtelefono)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_proveedor_telefono");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.HasIndex(e => e.Idempleado)
                    .HasName("fk_empleado_usuario");

                entity.HasIndex(e => e.Idimagen)
                    .HasName("fk_imagen_usuario");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Contrasena)
                    .IsRequired()
                    .HasColumnName("contrasena")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Correo)
                    .HasColumnName("correo")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Idempleado)
                    .HasColumnName("idempleado")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idimagen)
                    .HasColumnName("idimagen")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Rol)
                    .IsRequired()
                    .HasColumnName("rol")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Usuario1)
                    .IsRequired()
                    .HasColumnName("usuario")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.HasOne(d => d.IdempleadoNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.Idempleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_empleado_usuario");

                entity.HasOne(d => d.IdimagenNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.Idimagen)
                    .HasConstraintName("fk_imagen_usuario");
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.ToTable("venta");

                entity.HasIndex(e => e.Idcliente)
                    .HasName("fk_persona_venta");

                entity.HasIndex(e => e.Idusuario)
                    .HasName("fk_usuario_venta");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Documento)
                    .IsRequired()
                    .HasColumnName("documento")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("date");

                entity.Property(e => e.Idcliente)
                    .HasColumnName("idcliente")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idusuario)
                    .HasColumnName("idusuario")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("tipo")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_spanish_ci");

                entity.HasOne(d => d.IdclienteNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.Idcliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_persona_venta");

                entity.HasOne(d => d.IdusuarioNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_usuario_venta");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

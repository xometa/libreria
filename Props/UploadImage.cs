using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using RosaMaríaBookstore.Models;
namespace RosaMaríaBookstore.Props
{
    public class UploadImage
    {
        public IList<Detallepermisosusuario> PermissionsDetails { get; set; }
        public string redirect { get; set; }
        private bookstoreContext _context;
        public UploadImage(bookstoreContext context)
        {
            this._context = context;
        }

        public async Task<int> saveImage(IFormFile File, String Route)
        {
            int IdImageProduct = 0;
            //esta instancia se crea porque hay que guardar la imagen en la db
            Imagen Imagen = new Imagen();

            //Route representara el lugar donde almacenaremos nuestra imagen
            //cabe recalcar que para hacer esto, debemos tener creada la carpeta
            //en wwwroot, para nuestro caso seria wwwroot>images>products. De
            //no ser así pues desde el metodo uploadFileAsync se creara según
            // el nombre que trae la variable Ruta
            if (File != null)
            {
                //directorio destino de la imagen
                var FilePath = "wwwroot/images/" + Route + "/";
                var FileName = File.FileName;
                //antes de subirlo, aplicaremos validaciones respectivas
                //esto con el fín de no repetir imagenes, que ya se encuentren
                //en nuestro servidor, de igual manera que posea nombres validos
                var validateName = fileValidateName(FileName);
                var validateFile = fileExist(FilePath, FileName);
                if (validateName && !validateFile)
                {
                    /*
                     * Si la imagen no existe en nuestro directorio, tampoco existe en la DB, por lo cual
                     * procede a registrarse y asignarse al producto correspondiente
                     */
                    await uploadFileAsync(FilePath, FileName, File);
                    Imagen.Nombre = FileName;
                    this._context.Imagen.Add(Imagen);
                    await this._context.SaveChangesAsync();
                    IdImageProduct = Imagen.Id;
                }
                else if (validateFile && ExistsImage(File.FileName))
                {
                    /*Si el nombre de la imagen existe en la db, se retorna -2, obligando al usuario a cambiar el nombre*/
                    IdImageProduct = -2;
                }
                else if (!validateName)
                {
                    //si el nombre de la imagen es incorrecto se retorna -1, obligando a no registrar el producto
                    IdImageProduct = -1;
                }
            }
            else
            {
                //si no se especifica una imagen retornamos cero y no le asignamos imagen al producto, y se procedera 
                //a guardar el registro
                IdImageProduct = 0;
            }

            return IdImageProduct;
        }

        public bool ExistsImage(String Name)
        {
            if (!Name.Equals(""))
            {
                return _context.Imagen.Where(i => i.Nombre == Name).Any();
            }
            else
            {
                return false;
            }
        }

        //Metodos que validan el  nombre de la imagen,
        //de igual forma comprueban si la imagen existe
        //en el servidor
        public bool fileValidateName(String Name)
        {
            var NameValidate = !string.IsNullOrEmpty(Name) &&
            Name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
            return NameValidate;
        }

        public bool fileExist(String PathFile, String Name)
        {
            return System.IO.File.Exists(Path.Combine(PathFile, Name));
        }

        //metodo que subira la imagen al servidor
        public async Task<bool> uploadFileAsync(String Directory, String Name, IFormFile File)
        {
            String path = Directory + Name;
            //Comprobamos si la carpeta existe, de no ser así, se procede a crearla
            bool existeCarpeta = System.IO.Directory.Exists(Directory);
            if (!existeCarpeta)
            {
                System.IO.Directory.CreateDirectory(Directory);
            }
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await File.CopyToAsync(fileStream);
            }
            return true;
        }

        public void deleteServerImage(String PathFile, String Name)
        {
            //primero comprobamos, si el archivo existe en el path 
            //especificado, de ser así procedemos a eliminarlo
            if (System.IO.File.Exists(Path.Combine(PathFile, Name)))
            {
                System.IO.File.Delete(Path.Combine(PathFile, Name));
            }
        }

        public async Task<bool> accessUser(int idUser, string permission, string page)
        {
            bool access = false;
            this.PermissionsDetails = await this._context.Detallepermisosusuario
            .Include(p => p.IdpermisoNavigation)
            .Where(u => u.Idusuario == idUser)
            .OrderBy(dp => dp.Id).ToListAsync();
            if (this.PermissionsDetails.Count > 0)
            {
                if (permission.Equals("Session"))
                {
                    redirect = this.PermissionsDetails.First().IdpermisoNavigation.Nombre;
                    access = true;
                }
                else
                {
                    foreach (var dp in this.PermissionsDetails)
                    {
                        if (dp.IdpermisoNavigation.Nombre.Equals(permission))
                        {
                            redirect = dp.IdpermisoNavigation.Nombre;
                            access = true;
                            break;
                        }
                    }
                }

                if (!page.Equals(""))
                {
                    if (access)
                    {
                        redirect = page;
                    }
                    else
                    {
                        redirect = "/Error";
                    }
                }
                else
                {
                    if (redirect.Equals("Ventas"))
                    {
                        redirect = "/Sales/Sales";
                    }
                    else if (redirect.Equals("Compras"))
                    {
                        redirect = "/Shoppings/Shoppings";
                    }
                    else if (redirect.Equals("Abonos"))
                    {
                        redirect = "/Payments/Payments";
                    }
                    else if (redirect.Equals("Personal"))
                    {
                        redirect = "/Users/Positions";
                    }
                    else if (redirect.Equals("Respaldo de datos"))
                    {
                        redirect = "/Security/Backup";
                    }
                    else if (redirect.Equals("Listado de reportes"))
                    {
                        redirect = "/Reports/Reports";
                    }
                    else if (redirect.Equals("Inicio"))
                    {
                        redirect = "/Index";
                    }
                    else if (redirect.Equals("Marcas"))
                    {
                        redirect = "/Trademarks/Trademarks";
                    }
                    else if (redirect.Equals("Categorías"))
                    {
                        redirect = "/Categories/Categories";
                    }
                    else if (redirect.Equals("Productos"))
                    {
                        redirect = "/Products/Products";
                    }
                    else if (redirect.Equals("Clientes"))
                    {
                        redirect = "/Clients/Clients";
                    }
                    else if (redirect.Equals("Proveedores"))
                    {
                        redirect = "/Providers/Providers";
                    }
                    else if (redirect.Equals("Permisos"))
                    {
                        redirect = "/Users/Permissions";
                    }
                    else if (redirect.Equals("Bitácora"))
                    {
                        redirect = "/RecentActivity/TimeLine";
                    }
                    else if (redirect.Equals("Listado de consultas"))
                    {
                        redirect = "/Queries/Queries";
                    }
                    else if (redirect.Equals("Acerca de"))
                    {
                        redirect = "/About";
                    }
                    else
                    {
                        redirect = "/Error";
                    }
                }
            }
            else
            {
                redirect = "/Login/Logout";
                access = false;
            }
            return access;
        }
    }
}
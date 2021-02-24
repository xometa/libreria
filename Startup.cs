using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddDbContext<bookstoreContext>();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //para tratar json extensos
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithRedirects("/Error");//
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();//

            app.UseRouting();

            app.UseRequestLocalization();
            app.UseCors();//

            app.UseAuthentication();//
            app.UseAuthorization();

            app.UseSession();//

            //middleware
            app.Use(async (context, next) =>
            {
                int? Iduser = context.Session.GetInt32("IdUser");
                string path = context.Request.Path;
                if (Iduser == null && !path.StartsWith("/Login/"))
                {
                    //habilitar para poder recuperar contraseña,
                    //previo no haber realizado sesión, caso 
                    //contrario redireccionamos al inicio de sesíón
                    if (path == "/Recover/Recoverpassword" ||
                    path == "/Recover/ChangePassword" ||
                    path == "/Recover/LoginWithCode")
                    {
                        await next();
                    }
                    else
                    {

                        context.Response.Redirect("/Login/Login");
                        await next();
                    }
                }
                else if (Iduser != null && path.StartsWith("/Login/"))
                {

                    //Si ya inicio sesión, verificamos si desea cerrar la sesión
                    //caso contrario sera redireccionado al Index

                    if (path == "/Login/Logout")
                    {
                        await next();
                    }
                    else
                    {
                        context.Response.Redirect("/Index");
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}

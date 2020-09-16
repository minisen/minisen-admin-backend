using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using MiniSen_Service.Service_Interface;
using Autofac;
using System.IO;
using log4net.Repository;
using log4net;
using log4net.Config;
using MiniSen_Common.Helpers.Log;
using MiniSen_MVC_Common.MVCFilter.Filter;
using MiniSen_Backend.MVCFilter.Filter;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using MiniSen_Backend.MiniSenHubs;
using Microsoft.AspNetCore.SignalR;

namespace MiniSen_Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            #region 加载log4net日志配置文件
            XmlConfigurator.Configure(LogHelper.Repository, new FileInfo("log4net.config"));
            #endregion
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //將基本拉丁字元與中日韓字元納入允許範圍不做轉碼
            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { 
                UnicodeRanges.BasicLatin, 
                UnicodeRanges.CjkUnifiedIdeographs,
                UnicodeRanges.All
            }));

            //跨域配置
            services.AddCors(options => options.AddPolicy("MiniSenPolicy",
            builder =>
            {
                //测试：localhost:8081
                //正式：81.71.0.216:2021
                builder.WithOrigins(new string[] { "xxxxxx" })
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));

            //属性注入必须加上这个
            services.AddControllersWithViews().AddControllersAsServices(); 

            services.AddSignalR();

            services.AddSession();

            services.AddMvc(options =>
            {
                #region 注册全局过滤器

                options.Filters.Add<HandleExceptionFilter>();
                options.Filters.Add<CheckLoginFilter>();
                //options.Filters.Add<CheckApiPermFilter>();

                #endregion
            });
        }

        // ConfigureContainer is where you can register things directly with Autofac.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region Autofac自動注入

            //获取所有Hub类型并使用属性注入
            var hubType = typeof(Hub);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => hubType.IsAssignableFrom(t) && t != hubType)
                .PropertiesAutowired();

            //获取所有控制器类型并使用属性注入
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();

            //獲取Service類並使用屬性注入
            Assembly[] assemblies = new Assembly[] { Assembly.Load("MiniSen_Service") };
            builder.RegisterAssemblyTypes(assemblies)
                   .Where(type => !type.IsAbstract && 
                                  typeof(IServiceSupport).IsAssignableFrom(type))
                   .AsImplementedInterfaces()
                   .PropertiesAutowired();

            #endregion
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors("MiniSenPolicy");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "api/{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<BroadcastHub>("/api/broadcastHub");
            });

            
        }
    }
}

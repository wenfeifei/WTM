using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Demo.Models;
using WalkingTec.Mvvm.Mvc;
using WalkingTec.Mvvm.TagHelpers.LayUI;

namespace WalkingTec.Mvvm.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            if (args != null) configurationBuilder.AddCommandLine(args);

            var hostingConfig = configurationBuilder.Build();
            var globalConfig = hostingConfig.Get<GlobalConfig>();

            var ASPNETCORE_URLS = hostingConfig.GetValue<string>("ASPNETCORE_URLS");
            if (!string.IsNullOrEmpty(ASPNETCORE_URLS))
                globalConfig.ApplicationUrl = ASPNETCORE_URLS;

            return
                WebHost.CreateDefaultBuilder(args)
                    .ConfigureServices((hostingCtx, x) =>
                    {
                        var pris = new List<IDataPrivilege>
                        {
                            new DataPrivilegeInfo<School>("学校", y => y.SchoolName),
                            new DataPrivilegeInfo<Major>("专业", y => y.MajorName)
                        };
                        x.AddFrameworkService(dataPrivilegeSettings: pris, webHostBuilderContext: hostingCtx);
                        x.AddLayui();
                        x.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                        });
                    })
                    .Configure(x =>
                    {
                        var configs = x.ApplicationServices.GetRequiredService<Configs>();
                        if (configs.IsQuickDebug == true)
                        {
                            x.UseSwagger();
                            x.UseSwaggerUI(c =>
                            {
                                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                            });
                        }
                        x.UseFrameworkService();
                    })
                    .UseUrls(globalConfig.ApplicationUrl);
        }
    }
}

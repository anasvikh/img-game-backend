using Imaginarium.Hubs;
using Imaginarium.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Imaginarium
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddLogging(b => b.AddSerilog());

            services.AddDbContext<ImaginariumContext>(options =>
            {
                options.UseNpgsql(Configuration["Data:DefaultConnection:ConnectionString"],
                    b => b.MigrationsAssembly(typeof(ImaginariumContext).Assembly.GetName().Name));
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ImaginariumContext context)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ImaginariumContext>();
                dbContext.Database.Migrate();
            }

            app.UseCors(options => options
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<GameHub>("/game");
            });

            var emptyCardSets = context.CardSets
                .Where(x => x.Cards.Count() == 0)
                .ToList();

            emptyCardSets.ForEach(set =>
            {
                var setFolder = $"{env.WebRootPath}/cardSets/{set.NameEng}";

                var images = Directory.GetFiles(setFolder, "*.*", SearchOption.AllDirectories)
                .ToList();

                images.ForEach(image =>
                {
                    var fileName =  Path.GetFileName(image);
                    var numberInSet = Convert.ToInt32(fileName.Replace(".jpg", ""));
                    var newCard = new Card()
                    {
                        NumberInSet = numberInSet,
                        Src = $@"/cardSets/{set.NameEng}/{fileName}",
                        CardSetId = set.Id
                    };
                    context.Cards.Add(newCard);
                });

                context.SaveChanges();
            });

        }
    }
}

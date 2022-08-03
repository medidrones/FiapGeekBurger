using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using AutoMapper;
using GeekBurger.Production.Extension;
using GeekBurger.Production.Repository;
using GeekBurger.Production.Service;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace GeekBurger.Production
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var mvcCoreBuilder = services.AddMvcCore().AddApiExplorer();

            mvcCoreBuilder
                .AddFormatterMappings()
                .AddJsonFormatters()
                .AddCors();

            services.AddMvc()
                .AddJsonOptions(o =>
                {
                    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddAutoMapper();
            services.AddDbContext<ProductionContext>(o => o.UseInMemoryDatabase("geekburger-production"));
            services.AddScoped<IProductionAreaRepository, ProductionAreaRepository>();
            services.AddScoped<IProductionAreaChangedService, ProductionAreaChangedService>();
            services.AddScoped<IOrderFinishedService, OrderFinishedService>();
            services.AddScoped<INewOrderService, NewOrderService>();
            services.AddScoped<IPaidOrderService, PaidOrderService>();

            services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new Info { Title = "Production Area API", Version = "v1" });
                });
        }
        
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            ProductionContext productionContext, 
            INewOrderService newOrderService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();

            productionContext.Seed();

            var availableProductionAreas = productionContext.ProductionAreas?.ToList();

            newOrderService.SubscribeToTopic("ProductionAreaChangedTopic", availableProductionAreas);
            newOrderService.SubscribeToTopic("orderpaid", availableProductionAreas);

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Production Area API v1");
                }
            );
                        
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}

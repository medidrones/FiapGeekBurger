using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using GeekBurger.Production.Repository;
using Microsoft.EntityFrameworkCore;
using GeekBurger.Production.Extension;
using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;
using GeekBurger.Production.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;

namespace GeekBurger.Production
{
    public class Startup
    {
        public static IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcCoreBuilder = services.AddMvcCore().AddApiExplorer();

            mvcCoreBuilder
                .AddFormatterMappings()
                .AddJsonFormatters()
                .AddCors();
            
            services.AddMvc()
                        .AddJsonOptions(o => {
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
            INewOrderService newOrderService, 
            IPaidOrderService paidOrderService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMvc();
            
            productionContext.Seed();

            var availableProductionAreas = productionContext.ProductionAreas?.ToList();

            newOrderService.SubscribeToTopic("ProductionAreaChangedTopic", availableProductionAreas);
            newOrderService.SubscribeToTopic("orderpaid", availableProductionAreas);
            paidOrderService.SubscribeToTopic("orderpaid", availableProductionAreas);

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Production Area API v1");
                });
            
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Seja bem vindo a API Producytion!");
            });
        }
    }
}

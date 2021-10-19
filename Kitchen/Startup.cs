using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Kitchen
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kitchen", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapPost("/", async context =>
                // {
                //     if (!context.Request.HasJsonContentType())
                //     {
                //         context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                //         return;
                //     }
                //     //var weather = await context.Request.ReadFromJsonAsync<WeatherForecast>();
                //     
                //     //await context.Response.WriteAsync("Got order!");
                //     Console.WriteLine("got order");
                //     //await UpdateDatabaseAsync(weather);
                //
                //     context.Response.StatusCode = (int) HttpStatusCode.Accepted;
                // });
                
                endpoints.MapPost("/order", async context =>
                {
                    if (!context.Request.HasJsonContentType())
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                        return;
                    }

                    Console.WriteLine("got order!");
                    var orderData = await context.Request.ReadFromJsonAsync<OrderData>();
                    
                    KitchenManager.Instance().ReceiveOrder(orderData);
                    
                    //Console.WriteLine(orderData?.ToString());

                    context.Response.StatusCode = (int) HttpStatusCode.Accepted;
                });
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kitchen.CookingApparatus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kitchen
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CreateHostBuilder(args).Build().RunAsync();
            
            List<Oven> ovens = new List<Oven>();
            List<Stove> stoves = new List<Stove>();
            List<Cook> cooks = new List<Cook>();
            
            foreach (var _ in Enumerable.Range(0, Configuration.OvensCount).ToArray())
            {
                ovens.Add(new Oven());
            }
            
            foreach (var _ in Enumerable.Range(0, Configuration.StovesCount).ToArray())
            {
                stoves.Add(new Stove());
            }

            foreach (var id in Enumerable.Range(0, Configuration.CooksCount).ToArray())
            {
                cooks.Add(new Cook(id, "cook"+id, "phrase"+id, 1+id, 1+id));
            }

            KitchenSetup kitchenSetup = new KitchenSetup();
            kitchenSetup.Ovens = ovens.ToArray();
            kitchenSetup.Stoves = stoves.ToArray();
            //kitchenSetup.Cooks =
            
            KitchenManager.Instance().KitchenSetup = kitchenSetup;

            foreach (var cook in cooks)
            {
                cook.Start();
            }
            
            

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    
                    webBuilder.UseStartup<Startup>();
                });
    }
}

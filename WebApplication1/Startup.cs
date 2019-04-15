using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
            app.Use(async (context, next) =>
            {
                var consulClient = new ConsulClient();
                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = $"http://localhost:{context.Request.Host.Port}/HealthCheck",
                    Timeout = TimeSpan.FromSeconds(5)
                };

                // Register service with consul
                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    ID = "consul-" + context.Request.Host.Port.Value,
                    Name = "consul-api",
                    Address = "localhost",
                    Port = (int)context.Request.Host.Port,
                    Tags = new[] { $"urlprefix-/consul-api" }
                };

                await consulClient.Agent.ServiceRegister(registration);

                lifetime.ApplicationStopping.Register(async () =>
                {
                    await consulClient.Agent.ServiceDeregister(registration.ID);
                });
            });           

        }
    }
}

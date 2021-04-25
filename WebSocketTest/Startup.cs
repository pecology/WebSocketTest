using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketTest
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
            services.AddControllersWithViews();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/webSocketTest")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            await Echo(context, webSocket);
                        }

                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });
        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 1024];

            var count = 0;
            while(true)
            {
                // 0.1•b‚Éˆê‰ñ‘—‚ç‚ê‚Ä‚­‚é
                await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                count++;

                if ( count % 200 == 0)
                {
                    // 200‰ñ‚Éˆê‰ñ‘Ò‚Â
                    Debug.WriteLine("wait 10 seconds...");
                    await Task.Delay(10000);
                    Debug.WriteLine("resume");
                }
            }

        }
    }
}

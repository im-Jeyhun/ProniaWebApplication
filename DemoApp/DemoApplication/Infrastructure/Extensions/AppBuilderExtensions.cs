﻿using DemoApplication.Areas.Admin.Hubs;

namespace DemoApplication.Infrastructure.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void ConfigureMiddlewarePipeline(this WebApplication app)
        {
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=exists}/{controller=home}/{action=index}");

            app.MapHub<AlertHub>("alert-hub"); // hubun register olunmasi
        }
    }
}


using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.Services.General;
using Smurf_Village_Statistical_Office.Services.LeisureVenueServices.General;
using Smurf_Village_Statistical_Office.Services.MushroomHouseServices.General;
using Smurf_Village_Statistical_Office.Services.SmurfServices.ExportStrategies;
using Smurf_Village_Statistical_Office.Services.SmurfServices.General;
using Smurf_Village_Statistical_Office.Services.WorkingPlaceServices.General;

namespace Smurf_Village_Statistical_Office
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SmurfVillageContext>(options =>
                options.UseInMemoryDatabase("SmurfVillageDb"));

            // Add services to the container.
            builder.Services.AddScoped<ISmurfService, SmurfService>();
            builder.Services.AddScoped<IMushroomHouseService, MushroomHouseService>();
            builder.Services.AddScoped<IWorkingPlaceService, WorkingPlaceService>();
            builder.Services.AddScoped<ILeisureVenueService, LeisureVenueService>();

            builder.Services.AddScoped<ISmurfsExportStrategy, TxtExportSmurfsStrategy>();

            builder.Services.AddScoped<ExportService<ISmurfsExportStrategy>>();  

            builder.Services.AddControllers();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<SmurfVillageContext>();
                DbInitializer.Initialize(context);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

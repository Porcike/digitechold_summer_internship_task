using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Utils;
using System.Text;

namespace Smurf_Village_Statistical_Office.Services.SmurfServices.ExportStrategies
{
    public class TxtExportSmurfsStrategy : ISmurfsExportStrategy
    {
        private readonly SmurfVillageContext _context;

        public TxtExportSmurfsStrategy(SmurfVillageContext context)
        {
            _context = context;
        }

        public async Task<(byte[], ExportType)> ExportAllAsync()
        {
            var smurfs = await _context.Smurfs
                .AsNoTracking()
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine(new string('-', 30));

            foreach(var smurf in smurfs)
            {
                sb.Append(CreateStringRepresentation(smurf));
                sb.AppendLine(new string('-', 30));
            }

            return (Encoding.UTF8.GetBytes(sb.ToString()), ExportType.Txt);
        }

        public async Task<(byte[]?, ExportType?)> ExportById(int id)
        {
            var smurf = await _context.Smurfs
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return smurf != null
                ? (Encoding.UTF8.GetBytes(CreateStringRepresentation(smurf)), ExportType.Txt)
                : (null, null);
        }

        private string CreateStringRepresentation(Smurf smurf) =>
            $"Name: {smurf.Name}\n" +
            $"Age: {smurf.Age}\n" +
            $"Job: {smurf.Job.ToString()}\n" +
            $"Favourite food: {smurf.FavouriteFood.ToString()}\n" +
            $"Favourite brand: {smurf.FavouriteBrand.ToString()}\n" +
            $"Favourite color: {smurf.FavouriteColor.Name}\n";
    }
}

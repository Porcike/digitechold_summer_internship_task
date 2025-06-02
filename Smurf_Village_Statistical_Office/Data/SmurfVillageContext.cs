using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Models;

namespace Smurf_Village_Statistical_Office.Data
{
    public class SmurfVillageContext :DbContext
    {
        public SmurfVillageContext(DbContextOptions<SmurfVillageContext> options)
           : base(options)
        {
        }

        public DbSet<LeisureVenue> LeisureVenues { get; set; }
        public DbSet<Smurf> Smurfs { get; set; }
        public DbSet<WorkingPlace> WorkingPlaces { get; set; }
        public DbSet<MushroomHouse> MushroomHouses { get; set; }
    }
}

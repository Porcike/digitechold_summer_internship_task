using Smurf_Village_Statistical_Office.Utils;
using System.Drawing;

namespace Smurf_Village_Statistical_Office.Models
{
    public class Smurf
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public Job Job { get; set; }
        public Food FavouriteFood { get; set; }    
        public Brand FavouriteBrand { get; set; }
        public Color FavouriteColor { get; set; } 
        
    }
}

using Smurf_Village_Statistical_Office.Utils;
using System.Drawing;

namespace Smurf_Village_Statistical_Office.Models
{
    public class MushroomHouse
    {
        public int Id { get; set; } 
        public List<Smurf> Residents { get; set; } = new List<Smurf>();
        public int Capacity { get; set; } 
        public List<Food> AcceptedFoods { get; set; } = new List<Food>();
        public Color Color { get; set; }
        public string Motto { get; set; } = string.Empty;
    }
}

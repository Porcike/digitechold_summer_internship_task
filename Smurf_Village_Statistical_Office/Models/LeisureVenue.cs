using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Models
{
    public class LeisureVenue
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public List<Smurf> Members { get; set; } = new List<Smurf>();
        public Brand AcceptedBrand { get; set; }
    }
}

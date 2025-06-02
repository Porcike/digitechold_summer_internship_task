using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Models
{
    public class WorkingPlace
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Smurf> Employees { get; set; } = new List<Smurf>();
        public List<Job> AcceptedJobs { get; set; } = new List<Job>();
    }
}

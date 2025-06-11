using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services
{
    public class WorkingPlaceService : IWorkingPlaceService
    {
        private readonly SmurfVillageContext _context;

        public WorkingPlaceService(SmurfVillageContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkingPlaceDto>> GetAllAsync(WorkingPlaceFilterDto filter)
        {
            var isNameProvided = string.IsNullOrEmpty(filter.name);
            var isEmployeeProvided = string.IsNullOrEmpty(filter.employee);
            var isJobProvided = string.IsNullOrEmpty(filter.acceptedJob);

            var jobParseWasSuccessFul = Enum.TryParse(filter.acceptedJob, true, out Job parsedJob);

            var workplaces = await _context.WorkingPlaces
                .Where(w =>
                    (isNameProvided || EF.Functions.Like(w.Name, filter.name)) &&
                    (isEmployeeProvided || w.Employees.Any(e => EF.Functions.Like(e.Name, filter.employee))) &&
                    (isJobProvided || (jobParseWasSuccessFul && w.AcceptedJobs.Contains(parsedJob))))
                .Select(w => new WorkingPlaceDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    AcceptedJobs = w.AcceptedJobs,
                    EmployeeIds = w.Employees.Select(e => e.Id).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return workplaces;
        }

        public async Task<WorkingPlaceDto?> GetByIdAsnyc(int id)
        {
            var workplace = await _context.WorkingPlaces
                .Where(w => w.Id == id)
                .Select(w => new WorkingPlaceDto 
                {
                    Id = w.Id,
                    Name = w.Name,
                    AcceptedJobs = w.AcceptedJobs,
                    EmployeeIds = w.Employees.Select(e => e.Id).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);

            return workplace;
        }
    }
}

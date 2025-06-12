using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.WorkingPlaceService
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
                    (isJobProvided || jobParseWasSuccessFul && w.AcceptedJobs.Contains(parsedJob)))
                .Select(w => ToDto(w))
                .AsNoTracking()
                .ToListAsync();

            return workplaces;
        }

        public async Task<WorkingPlaceDto?> GetByIdAsnyc(int id)
        {
            var workplace = await _context.WorkingPlaces
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);

            return workplace != null 
                ? ToDto(workplace) 
                : null;
        }

        private WorkingPlaceDto ToDto(WorkingPlace workingPlace)
        {
            return new WorkingPlaceDto
            {
                Id = workingPlace.Id,
                Name = workingPlace.Name,
                AcceptedJobs = workingPlace.AcceptedJobs,
                EmployeeIds = workingPlace.Employees.Select(e => e.Id).ToList()
            };
        }
    }
}

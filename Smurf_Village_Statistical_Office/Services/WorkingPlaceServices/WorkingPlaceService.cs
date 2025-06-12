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

        public async Task<IReadOnlyCollection<WorkingPlaceDto>> GetAllAsync(WorkingPlaceFilterDto filter)
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
                .FirstOrDefaultAsync();

            return workplace;
        }

        public async Task<WorkingPlaceDto> InsertAsync(CreateWorkingPlaceDto value)
        {
            foreach(var job in value.AcceptedJobs)
            {
                if(!Enum.IsDefined(typeof(Job), job))
                {
                    throw new ArgumentException("Unknown job!");
                }
            }

            var employees = await _context.Smurfs
                .Where(s => value.EmployeeIds.Contains(s.Id))
                .ToListAsync();

            if(value.EmployeeIds.Count != employees.Count)
            {
                throw new ArgumentException("Unknown employee!");
            }

            foreach(var employee in employees)
            {
                if (!value.AcceptedJobs.Contains((int)employee.Job))
                {
                    throw new ArgumentException("The accepted jobs aren't compatible with the employees' qualifications!");
                }
            }

            var workplace = new WorkingPlace
            {
                Name = value.Name,
                Employees = employees,
                AcceptedJobs = value.AcceptedJobs.Select(j => (Job)j).ToList()
            };

            await _context.WorkingPlaces.AddAsync(workplace);
            await _context.SaveChangesAsync();

            return new WorkingPlaceDto
            {
                Id = workplace.Id,
                Name = workplace.Name,
                AcceptedJobs = workplace.AcceptedJobs,
                EmployeeIds = workplace.Employees.Select(e => e.Id).ToList()
            };
        }
    }
}

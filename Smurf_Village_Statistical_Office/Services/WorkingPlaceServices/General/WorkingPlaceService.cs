using Microsoft.EntityFrameworkCore;
using Smurf_Village_Statistical_Office.Data;
using Smurf_Village_Statistical_Office.DTO.WorkingPlaceDtos;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.WorkingPlaceServices.General
{
    public class WorkingPlaceService(SmurfVillageContext context) : IWorkingPlaceService
    {
        private readonly SmurfVillageContext _context = context;

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
            var message = await CheckForGeneralConstrains(value);
            if (message != null)
            {
                throw new ArgumentException(message);
            }

            var employees = await _context.Smurfs
                .Where(s => value.EmployeeIds.Contains(s.Id))
                .ToListAsync();

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

        public async Task UpdateAsync(UpdateWorkingPlaceDto value)
        {
            var message = await CheckForGeneralConstrains(value);
            if (message != null)
            {
                throw new ArgumentException(message);
            }

            var workplace = await _context.WorkingPlaces
                .Include(w => w.Employees)
                .FirstOrDefaultAsync(w => w.Id == value.Id);

            if(workplace == null)
            {
                throw new KeyNotFoundException("Workplace not found!");
            }

            var employees = await _context.Smurfs
                .Where(s => value.EmployeeIds.Contains(s.Id))
                .ToListAsync();

            workplace.Name = value.Name;
            workplace.Employees = employees;
            workplace.AcceptedJobs = value.AcceptedJobs.Select(j => (Job)j).ToList();

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var workplace = await _context.WorkingPlaces.FindAsync(id);
            if (workplace == null)
            {
                throw new KeyNotFoundException("Workplace not found!");
            }

            var hasEmployees = await _context.WorkingPlaces
                .Where(w => w.Id == id)
                .SelectMany(w => w.Employees)
                .AnyAsync();

            if (hasEmployees)
            {
                throw new InvalidOperationException("This workplace still has employees!");
            }

            _context.WorkingPlaces.Remove(workplace);
            await _context.SaveChangesAsync();
        }

        public async Task AddEmployeeAsync(int workplaceId, int smurfId)
        {
            var workplace = await _context.WorkingPlaces.FindAsync(workplaceId);
            if (workplace == null)
            {
                throw new KeyNotFoundException("Workplace not found!");
            }

            var smurf = await _context.Smurfs.FindAsync(smurfId);
            if (smurf == null)
            {
                throw new KeyNotFoundException("Smurf not found!");
            }

            if (!workplace.AcceptedJobs.Contains(smurf.Job))
            {
                throw new InvalidOperationException("Incompatible jobs!");
            }

            if (workplace.Employees.Contains(smurf))
            {
                throw new InvalidOperationException("This smurf is already an employee!");
            }

            workplace.Employees.Add(smurf);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveEmployeeAsync(int workplaceId, int smurfId)
        {
            var workplace = await _context.WorkingPlaces.FindAsync(workplaceId);
            if (workplace == null)
            {
                throw new KeyNotFoundException("Workplace not found!");
            }

            var smurf = await _context.Smurfs.FindAsync(smurfId);
            if (smurf == null)
            {
                throw new KeyNotFoundException("Smurf not found!");
            }

            if (!workplace.Employees.Contains(smurf))
            {
                throw new KeyNotFoundException("This smurf is not an employee!");
            }

            workplace.Employees.Remove(smurf);
            await _context.SaveChangesAsync();
        }

        private async Task<string?> CheckForGeneralConstrains(BaseWorkingPlaceDto value)
        {
            if(value.AcceptedJobs.Any(j => !Enum.IsDefined(typeof(Job), j)))
            {
                return "Unknown job!";
            }

            var employeeCount = await _context.Smurfs
                .CountAsync(s => value.EmployeeIds.Contains(s.Id));

            if(value.EmployeeIds.Count != employeeCount)
            {
                return "Unknown employee!";
            }

            var isJobInCompatible = await _context.Smurfs
                .AnyAsync(s => 
                    value.EmployeeIds.Contains(s.Id) &&
                    !value.AcceptedJobs.Contains((int)s.Job));

            if (isJobInCompatible)
            {
                return "Incompatible jobs!";
            }

            return null;
        }
    }
}

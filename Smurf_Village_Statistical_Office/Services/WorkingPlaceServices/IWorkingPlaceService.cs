using Smurf_Village_Statistical_Office.DTO;
using Smurf_Village_Statistical_Office.DTO.Filters;

namespace Smurf_Village_Statistical_Office.Services.WorkingPlaceService
{
    public interface IWorkingPlaceService : IEntityService<WorkingPlaceDto, CreateWorkingPlaceDto, WorkingPlaceFilterDto>
    {
    }
}

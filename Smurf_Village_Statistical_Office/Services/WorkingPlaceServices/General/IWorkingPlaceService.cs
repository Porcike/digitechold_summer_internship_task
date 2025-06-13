using Smurf_Village_Statistical_Office.DTO.WorkingPlaceDtos;
using Smurf_Village_Statistical_Office.Services.General;

namespace Smurf_Village_Statistical_Office.Services.WorkingPlaceServices.General
{
    public interface IWorkingPlaceService : IEntityService<
        WorkingPlaceDto, 
        CreateWorkingPlaceDto,
        UpdateWorkingPlaceDto,
        WorkingPlaceFilterDto>
    {
    }
}

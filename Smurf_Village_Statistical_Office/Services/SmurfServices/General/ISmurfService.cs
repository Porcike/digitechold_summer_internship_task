using Smurf_Village_Statistical_Office.DTO.SmurfDtos;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Services.General;

namespace Smurf_Village_Statistical_Office.Services.SmurfServices.General
{
    public interface ISmurfService : IEntityService<
        Smurf,
        SmurfDto, 
        CreateSmurfDto, 
        UpdateSmurfDto,
        SmurfFilterDto>
    {
    }
}

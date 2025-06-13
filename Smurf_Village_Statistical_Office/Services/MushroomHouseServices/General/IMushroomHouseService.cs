using Smurf_Village_Statistical_Office.DTO.MushroomHouseDtos;
using Smurf_Village_Statistical_Office.Models;
using Smurf_Village_Statistical_Office.Services.General;

namespace Smurf_Village_Statistical_Office.Services.MushroomHouseServices.General
{
    public interface IMushroomHouseService : IEntityService<
        MushroomHouseDto, 
        CreateMushroomHouseDto,
        UpdateMushroomHouseDto,
        MushroomHouseFilterDto>
    {
    }
}

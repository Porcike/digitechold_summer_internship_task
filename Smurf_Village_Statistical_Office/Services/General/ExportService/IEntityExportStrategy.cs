using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.General.ExportService
{
    public interface IEntityExportStrategy
    {
        Task<(byte[], ExportType)> ExportAllAsync();
        Task<(byte[]?, ExportType?)> ExportById(int id);
    }
}

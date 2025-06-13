using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.General
{
    public class ExportService<T>(IEnumerable<T> smurfsExportStrategies) where T: IEntityExportStrategy
    {
        private readonly IDictionary<string, T> _smurfsExportStrategies = smurfsExportStrategies.ToDictionary(
                s => s.GetType().Name.Replace("ExportSmurfsStrategy", "").ToLower(),
                s => s);

        public async Task<(byte[], string, string)> ExportAllAsync(string type)
        {
            if(!_smurfsExportStrategies.TryGetValue(type.ToLower(), out var strategy))
            {
                throw new ArgumentException("Invalid type!");
            }

            var (bytes, exportType) = await strategy.ExportAllAsync();
            var contentType = ExportUtil.exportDictionary[exportType];
            var fileName = $"data_{DateTime.Now:yyyyMMddHHmmss}.{exportType.ToString().ToLower()}";

            return (bytes, contentType, fileName);
        }

        public async Task<(byte[], string, string)> ExportByIdAsync(int id, string type)
        {
            if (!_smurfsExportStrategies.TryGetValue(type.ToLower(), out var strategy))
            {
                throw new ArgumentException("Invalid type!");
            }

            var (bytes, exportType) = await strategy.ExportById(id);
            if(bytes == null || exportType == null)
            {
                throw new KeyNotFoundException("Unknown entity!");
            }

            var contentType = ExportUtil.exportDictionary[exportType ?? 0];
            var fileName = $"data_{DateTime.Now:yyyyMMddHHmmss}.{exportType?.ToString().ToLower()}";

            return (bytes, contentType, fileName);
        }
    }
}

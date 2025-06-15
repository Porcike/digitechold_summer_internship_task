using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.General.ExportService
{
    public class ExportService<TExportStrategy, TEntity>(IEnumerable<TExportStrategy> exportStrategies) 
        where TExportStrategy: IEntityExportStrategy
        where TEntity : class
    {
        private readonly IDictionary<string, TExportStrategy> _smurfsExportStrategies = exportStrategies.ToDictionary(
                s => s.GetType().Name.Replace($"Export{typeof(TEntity).Name}Strategy", "").ToLower(),
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
                throw new KeyNotFoundException("Entity not found!");
            }

            var contentType = ExportUtil.exportDictionary[exportType ?? 0];
            var fileName = $"data_{DateTime.Now:yyyyMMddHHmmss}.{exportType?.ToString().ToLower()}";

            return (bytes, contentType, fileName);
        }
    }
}

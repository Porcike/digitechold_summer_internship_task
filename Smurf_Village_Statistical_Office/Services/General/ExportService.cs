using Smurf_Village_Statistical_Office.Utils;

namespace Smurf_Village_Statistical_Office.Services.General
{
    public class ExportService<T> where T: IEntityExportStrategy
    {
        private readonly IDictionary<string, T> _smurfsExportStrategies;

        public ExportService(IEnumerable<T> smurfsExportStrategies)
        {
            _smurfsExportStrategies = smurfsExportStrategies.ToDictionary(
                s => s.GetType().Name.Replace("ExportSmurfsStrategy", "").ToLower(),
                s => s);
        }

        public async Task<ExportStatus> ExportAllAsync(string type)
        {
            if(!_smurfsExportStrategies.TryGetValue(type.ToLower(), out var strategy))
            {
                return new ExportStatus
                {
                    Bytes = null,
                    ContentType = null,
                    FileName = null,
                    TypeNotFound = true,
                    EnityNotFound = false,
                };
            }

            var (bytes, exportType) = await strategy.ExportAllAsync();
            var contentType = ExportUtil.exportDictionary[exportType];
            var fileName = $"data_{DateTime.Now:yyyyMMddHHmmss}.{exportType.ToString().ToLower()}";

            return new ExportStatus
            {
                Bytes = bytes,
                ContentType = contentType,
                FileName = fileName,
                TypeNotFound = false,
                EnityNotFound = false
            };
        }

        public async Task<ExportStatus> ExportByIdAsync(int id, string type)
        {
            if (!_smurfsExportStrategies.TryGetValue(type.ToLower(), out var strategy))
            {
                return new ExportStatus
                {
                    Bytes = null,
                    ContentType = null,
                    FileName = null,
                    TypeNotFound = true,
                    EnityNotFound = false,
                };
            }

            var (bytes, exportType) = await strategy.ExportById(id);
            if(bytes == null || exportType == null)
            {
                return new ExportStatus
                {
                    Bytes = null,
                    ContentType = null,
                    FileName = null,
                    TypeNotFound = false,
                    EnityNotFound = true,
                };
            }

            var contentType = ExportUtil.exportDictionary[exportType ?? 0];
            var fileName = $"data_{DateTime.Now:yyyyMMddHHmmss}.{exportType?.ToString().ToLower()}";

            return new ExportStatus
            {
                Bytes = bytes,
                ContentType = contentType,
                FileName = fileName,
                TypeNotFound = false,
                EnityNotFound = false,
            };
        }
    }
}

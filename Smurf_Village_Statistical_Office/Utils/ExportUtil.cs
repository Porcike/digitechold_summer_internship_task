namespace Smurf_Village_Statistical_Office.Utils
{
    public static class ExportUtil
    {
        public static readonly Dictionary<ExportType, string> exportDictionary = new Dictionary<ExportType, string>
        {
            { ExportType.Txt, "text/plain" }
        };
    }
}

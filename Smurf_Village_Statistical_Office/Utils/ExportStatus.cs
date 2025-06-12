namespace Smurf_Village_Statistical_Office.Utils
{
    public record ExportStatus
    {
        public byte[]? Bytes { get; init; }
        public string? ContentType { get; init; }
        public string? FileName { get; init; }
        public bool TypeNotFound { get; init; }
        public bool EnityNotFound { get; init; }
    }
}

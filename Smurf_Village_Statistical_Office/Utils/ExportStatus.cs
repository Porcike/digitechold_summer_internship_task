using System.Collections.Immutable;

namespace Smurf_Village_Statistical_Office.Utils
{
    public struct ExportStatus
    {
        public readonly byte[]? bytes;
        public readonly string? contentType;
        public readonly string? fileName;
        public readonly bool typeNotFound;
        public readonly bool entityNotFound;

        public ExportStatus(
            byte[]? bytes, 
            string? contentType, 
            string? fileName, 
            bool typeNotFound, 
            bool entityNotFound)
        {
            this.bytes = bytes;
            this.contentType = contentType;
            this.fileName = fileName;
            this.typeNotFound = typeNotFound;
            this.entityNotFound = entityNotFound;
        }
    }
}

using Catalog.Domain.Notifications;

namespace Catalog.Application.Dtos
{
    public class ImportResultDto
    {
        public int TotalRecords { get; set; }

        public int TotalErrors { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
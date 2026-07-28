using System.Collections.Generic;

namespace Catalog.Domain.Models
{
    public enum ImportStatus
    {
        Created,
        Skipped,
        Failed
    }

    public class ProductImportResult
    {
        public Product Product { get; }
        public string SellerName { get; }
        public int SellerProductId { get; }
        public ImportStatus Status { get; }
        public List<string> Errors { get; }
        public string? SkipReason { get; }

        public ProductImportResult(Product product, string sellerName, int sellerProductId, ImportStatus status, List<string> errors = null, string? skipReason = null)
        {
            Product = product;
            SellerName = sellerName;
            SellerProductId = sellerProductId;
            Status = status;
            Errors = errors ?? new List<string>();
            SkipReason = skipReason;
        }

        public static ProductImportResult SuccessCreated(Product product, string sellerName, int sellerProductId) =>
            new(product, sellerName, sellerProductId, ImportStatus.Created);

        public static ProductImportResult SuccessSkipped(Product product, string sellerName, int sellerProductId, string reason) =>
            new(product, sellerName, sellerProductId, ImportStatus.Skipped, skipReason: reason);

        public static ProductImportResult Failure(Product product, string sellerName, int sellerProductId, List<string> errors) =>
            new(product, sellerName, sellerProductId, ImportStatus.Failed, errors);
    }
}

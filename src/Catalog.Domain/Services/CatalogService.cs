using Catalog.Domain.Extensions;
using Catalog.Domain.Interfaces;
using Catalog.Domain.Models;
using Catalog.Domain.Validations;

namespace Catalog.Domain.Services
{
    public class CatalogService : DomainService, ICatalogService
    {
        private IProductRepository _productRepository;
        private ISellerProductRepository _sellerProductRepository;

        public CatalogService(IProductRepository productRepository,
            INotificationCollector notificationCollector,
            ISellerProductRepository sellerProductRepository) : base(notificationCollector)
        {
            _productRepository = productRepository;
            _sellerProductRepository = sellerProductRepository;
        }

        public async Task Import(List<Product> products)
        {
            foreach (var product in products)
            {
                if (!Validate(new ProductValidation(), product))
                    continue;

                Normalize(product);

                var dbProduct = await _productRepository.GetByNameAndBrand(product.Name, product.Brand);
                var sellers = product.Sellers.ToList();

                if (dbProduct == null)
                {
                    product.Sellers.Clear();

                    await _productRepository.Add(product);

                    dbProduct = product;
                }

                await SaveSellers(dbProduct.Id, sellers);

                Notify(Notifications.NotificationType.Processed, $"{product.SellerProductId} - processed");
            }
        }

        private async Task SaveSellers(int productId, IEnumerable<SellerProduct> sellers)
        {
            foreach (var seller in sellers)
            {
                if (!Validate(new SellerProductValidation(), seller))
                    continue;

                if (await _sellerProductRepository.ProductAlreadyHasTheSeller(productId, seller.SellerName))
                    continue;

                seller.ProductId = productId;
                seller.Product = null;

                await _sellerProductRepository.Add(seller);
            }
        }

        private void Normalize(Product product)
        {
            if (product == null) return;

            if (!string.IsNullOrWhiteSpace(product.Name))
                product.Name = product.Name.NormalizeSpaces();

            if (!string.IsNullOrWhiteSpace(product.Brand))
                product.Brand = product.Brand?.NormalizeSpaces();
        }
    }
}
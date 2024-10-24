using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _collectionProduct;
        public ProductRepository(IMongoDatabase mongoDatabase)
        {
                _collectionProduct = mongoDatabase.GetCollection<Product>(nameof(Product)); ;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _collectionProduct.Find(p => true).ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _collectionProduct.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string productName)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, productName);

            return await _collectionProduct.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _collectionProduct.Find(filter).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _collectionProduct.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var result = await _collectionProduct.ReplaceOneAsync(c => c.Id == product.Id, product);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var result = await _collectionProduct.DeleteOneAsync(c => c.Id == id);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}

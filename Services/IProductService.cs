using PROG7311POE_ST10178800.Models;


    public interface IProductService
    {
        Task<List<Product>> GetProductsByFarmerAsync(string farmerId);
        Task AddProductAsync(Product product, string farmerId);
        Task<List<Product>> FilterProductsAsync(string category, DateTime? from, DateTime? to);
    Task DeleteProductAsync(int productId, string farmerId);


}

public interface IFarmerService
    {
        Task<List<Farmer>> GetAllFarmersAsync();
        Task AddFarmerAsync(Farmer farmer);
    }

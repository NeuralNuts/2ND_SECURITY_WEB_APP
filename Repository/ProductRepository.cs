using _2ND_SECURITY_WEB_APP.Context;
using _2ND_SECURITY_WEB_APP.Models;
using Dapper;

namespace _2ND_SECURITY_WEB_APP.Repository
{
    public class ProductRepository
    {
        #region Dapper inastilzed
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context) =>
            _context = context;
        #endregion

        #region Gets all products for product view
        public async Task<IEnumerable<ProductModel>> GetProducts()
        {
            var query = "SELECT * FROM [Products]";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<ProductModel>(query);
                return users.ToList();
            }
        }
        #endregion
    }
}
